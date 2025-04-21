using NetMQ;
using NetMQ.Sockets;
using System.Diagnostics;
using System.Net.Sockets;

namespace ProximityClient
{
    public partial class Form1 : Form
    {
        readonly string serverHost = "tcp://localhost:5555";

        CancellationTokenSource cts = null!;

        public Form1()
        {
            InitializeComponent();
            StopButton.Enabled = false;
            progressBar1.Style = ProgressBarStyle.Continuous;
            progressBar1.Value = 0;
            progressBar1.Step = 1;
            progressBar1.Maximum = 100;
            progressBar1.Visible = false; // hide progress bar until we get the service running

        }

        String getDistance()
        {
            using (var client = new RequestSocket())
            {
                client.Connect(serverHost);
                var timeout = new TimeSpan();
                timeout += TimeSpan.FromMilliseconds(500);
                timeout += TimeSpan.FromSeconds(1);

                Debug.WriteLine("Sending REQ");
                if (false == client.TrySendFrame(timeout, "REQ"))
                {
                    Debug.WriteLine("No server available");
                    return "N/A";
                }

                if (client.TryReceiveFrameString(timeout, out string? message))
                {
                    Debug.WriteLine("Received {0}", message);
                    return message;
                }
                else
                {
                    Debug.WriteLine("No message recevied");
                }

                client.Disconnect(serverHost); // Disconnect to clean up the connection, if needed. 
                client.Close();
                return "N/A";
            }
        }

        // remap a value from one range to another convert valid distance 10 to 1219 mm to 0 to 100
        private double RangeConv(double value, double oldMin, double oldMax, double newMin, double newMax)
        {
            return (value - oldMin) * (newMax - newMin) / (oldMax - oldMin) + newMin;
        }

        private void updateProgressBar(string distance)
        {
            double remapdist = 0.0f;
            if (double.TryParse(distance, out double dist))
            {
                // Remap the distance to a value between 0 and 100 for the progress bar.
                remapdist = RangeConv(
                    dist,           // value to remap
                    10,            // old min (10 mm)
                    1219,          // old max (1219 mm)
                    0,             // new min (0)
                    100            // new max (100)
                );

                progressBar1.Visible = true; // Make sure the progress bar is visible when updating it.
                progressBar1.Value = (int)remapdist; // Set the progress bar value, ensure it's an integer.
                progressBar1.Update(); // Update the progress bar to reflect the new value.
            }
            else
            {
                remapdist = 0; // Default to 0 if parsing fails
                progressBar1.Visible = false; // Hide the progress bar if the distance is invalid
            }
        }

        private void SingleButton_Click(object sender, EventArgs e)
        {
            String distance = getDistance();
            DistanceLabel.Text = distance;
            updateProgressBar(distance);
        }

        private async void StartButton_Click(object sender, EventArgs e)
        {
            SingleButton.Enabled = false;
            StartButton.Enabled = false;
            StopButton.Enabled = true;

            var progress = new Progress<string>(s => DistanceLabel.Text = s);
            cts = new CancellationTokenSource();
            await Task.Run(() => DistanceThreadConcern.distanceWork(progress, serverHost, cts.Token, this)); // Pass 'this' as the instance

            SingleButton.Enabled = true;
            StartButton.Enabled = true;
            StopButton.Enabled = false;

        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            cts.Cancel();
            SingleButton.Enabled = true;
            StartButton.Enabled = true;
        }

        class DistanceThreadConcern
        {
            public static void distanceWork(IProgress<string> progress, string hostname, CancellationToken ct, Form1 formInstance)
            {
                var timeout = new TimeSpan();
                timeout += TimeSpan.FromMilliseconds(500);
                timeout += TimeSpan.FromSeconds(1);

                Debug.WriteLine("Starting distance loop");
                progress.Report("wait...");

                using (var client = new RequestSocket())
                {
                    client.Connect(hostname);
                    client.Options.Linger = TimeSpan.Zero; // Ensure the socket closes immediately when Disconnect is called. This is important for clean shutdowns.

                    while (true)
                    {
                        if (ct.IsCancellationRequested)
                        {
                            Debug.WriteLine("User cancelled");
                            break;
                        }
                        Debug.WriteLine("Sending REQ");
                        if (false == client.TrySendFrame(timeout, "REQ"))
                        {
                            Debug.WriteLine("No server available");
                            progress.Report("No Service");
                            break;
                        }

                        if (client.TryReceiveFrameString(timeout, out string? distance))
                        {
                            Debug.WriteLine("Received {0}", distance);
                            progress.Report(distance.ToString());
                            formInstance.Invoke(new Action(() => formInstance.updateProgressBar(distance)));
                            //formInstance.updateProgressBar(distance); // Use the instance to call the method
                        }
                        else
                        {
                            Debug.WriteLine("Receive timed out");
                            progress.Report("No Service");
                            break;
                        }
                    }

                    client.Disconnect(hostname);
                    client.Close();
                }

            }
        }

    }
}
