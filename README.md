# Facepod Proximity Detect Client

This is a sample Windows .NET client application using the Facepod Windows service
named: "Facepod Proximity Detection".

The "Facepod Proximity Detection" service provides distance (proximity) information to 
anything that is directly in front of the TOF sensor located in the middle cutout under the front display.

A client application can request the distance information using the 
[ZeroMQ](https://zeromq.org/) socket Request-Reply pattern. The service can support multiple
clients requesting the distance without conflicts. 

The distance service is provided by a ZeroMQ socket on localhost port 5555 ("tcp://localhost:5555").
The default port 5555 can be configured by changing the command line arguments for the Windows service.
See the [Facepod Proximity Detection service](https://github.com/davidkilp/VL53L5CX_Sensor/tree/master/FacepodProximityService) for
further details on the Windows Service.

The .NET implementation for ZeroMQ is provided by the [NetMQ](https://github.com/zeromq/netmq) package which
is available on [NuGet](https://nuget.org/packages/NetMQ/). 
ZeroMQ provides support for many different languages and platforms. 
NetMq is recommended for C# as it is a 100% native port of the ZeroMQ library.

## Requirements:

* Visual Studio 2022
* .NET 8 Runtime must be installed on hosted system. See: [.NET 8 Runtime Installer](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-8.0.2-windows-x64-installer)

## Client Development

Developing a proximity detection client is relatively straigthforward using the Request-Reply pattern provided by 
ZeroMQ. See the [ZeroMQ Guide](https://zguide.zeromq.org/) for further details. 


The following shows a possible function to retrieve the current distance to an object using the NetMQ library in a non-blocking mode:

```
        string serverHost = "tcp://localhost:5555";
		
		string getDistance()
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
```

ZeroMQ also supports blocking mode operation which is simpler but non-blocking mode would be recommended for most applications.

ZeroMQ by default uses frame based messages. In the simplest form as used here it will use a simple string as the message format
for both the Request and Reply.

The proximity client simply sends a ZeroMQ request string, "REQ", to request the currrent distance reading from 
the TOF sensor and then waits for a reply string which will be also be returned as a string. 
The returned string will be the TOF sensor measurement in millimeters as a floating point value with fractional part to 1/10 of a mm.
The returned string might be be something like "573.4" for example. If the TOF sensor does not detect 
anything in range it will send back the string "0.0".

As explained in the VL53L5CX sensor operating model description for the Facepod, the service will use 
only use the TOF data from the 4 zones in the middle of the sensor. Namely zones: 5, 6, 9, 10. 
The service will also reject any TOF values that are not in the range of 10.0 mm to 1219.0 mm (approximately 4 feet).

## Usage

Find the .NET GUI application, ProximityClient.exe, and launch it. 

![screenshot](https://github.com/davidkilp/FacepodProxmityClient/blob/master/Screenshot-ProximityDetectClient.png)

To request a single distance request from the service press the "Single Request" button. 
To display continuous distance updates from the service press the "Start" button and the
display will update as fast as the service can supply data.


## Python client example 

As a further example of the multi-language support for ZeroMQ. Here is a simple client in Python:

```
#
#   Facepod Proximity Request client in Python
#   Connects REQ socket to tcp://localhost:5555
#   Sends "REQ" to server, expects distance as "XX.X" back
#

import zmq

context = zmq.Context()

#  Socket to talk to server
print("Connecting to Facepod Proximity server...")
socket = context.socket(zmq.REQ)
socket.connect("tcp://localhost:5555")

#  Do 10 requests, waiting each time for a response
for request in range(10):
    print(f"Sending request {request} ...")
    socket.send_string("REQ")

    #  Get the reply.
    message = socket.recv()
    print(f"Received reply {request} [ distance: {message} mm ]")
```
 
