namespace ProximityClient
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            SingleButton = new Button();
            StartButton = new Button();
            StopButton = new Button();
            groupBox1 = new GroupBox();
            label1 = new Label();
            DistanceLabel = new Label();
            progressBar1 = new ProgressBar();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // SingleButton
            // 
            SingleButton.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            SingleButton.Location = new Point(12, 69);
            SingleButton.Name = "SingleButton";
            SingleButton.Size = new Size(124, 73);
            SingleButton.TabIndex = 0;
            SingleButton.Text = "Single Request";
            SingleButton.UseVisualStyleBackColor = true;
            SingleButton.Click += SingleButton_Click;
            // 
            // StartButton
            // 
            StartButton.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            StartButton.Location = new Point(19, 31);
            StartButton.Name = "StartButton";
            StartButton.Size = new Size(124, 48);
            StartButton.TabIndex = 1;
            StartButton.Text = "Start";
            StartButton.UseVisualStyleBackColor = true;
            StartButton.Click += StartButton_Click;
            // 
            // StopButton
            // 
            StopButton.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            StopButton.Location = new Point(19, 101);
            StopButton.Name = "StopButton";
            StopButton.Size = new Size(124, 50);
            StopButton.TabIndex = 2;
            StopButton.Text = "Stop";
            StopButton.UseVisualStyleBackColor = true;
            StopButton.Click += StopButton_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(StartButton);
            groupBox1.Controls.Add(StopButton);
            groupBox1.Location = new Point(189, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(167, 182);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            groupBox1.Text = "Continuous Updates";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 197);
            label1.Name = "label1";
            label1.Size = new Size(193, 37);
            label1.TabIndex = 4;
            label1.Text = "Distance (mm):";
            // 
            // DistanceLabel
            // 
            DistanceLabel.AutoSize = true;
            DistanceLabel.Font = new Font("Segoe UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            DistanceLabel.Location = new Point(211, 197);
            DistanceLabel.Name = "DistanceLabel";
            DistanceLabel.Size = new Size(65, 37);
            DistanceLabel.TabIndex = 5;
            DistanceLabel.Text = "N/A";
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(12, 237);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(344, 38);
            progressBar1.TabIndex = 6;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(368, 287);
            Controls.Add(progressBar1);
            Controls.Add(DistanceLabel);
            Controls.Add(label1);
            Controls.Add(groupBox1);
            Controls.Add(SingleButton);
            Name = "Form1";
            Text = "Proximity Detect Client";
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button SingleButton;
        private Button StartButton;
        private Button StopButton;
        private GroupBox groupBox1;
        private Label label1;
        private Label DistanceLabel;
        private ProgressBar progressBar1;
    }
}
