using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;

namespace azure_iothub_device_simulation_dotnet
{
    class Program
    {
        static DeviceClient deviceClient;
        static string iotHubUri = "HostName={hub}.azure-devices.net;DeviceId=myDotnetDevice;SharedAccessKey={SAS}";

        static void Main(string[] args)
        {
            Console.WriteLine("Simulated device\n");
            deviceClient = DeviceClient.CreateFromConnectionString(iotHubUri, TransportType.Mqtt);

            SendDeviceToCloudMessagesAsync();
            Console.ReadLine();

        }
        private static async void SendDeviceToCloudMessagesAsync()
        {
            double temparature = 70; // m/s
            Random rand = new Random();

            while (true)
            {
                double currenttemparature = temparature + rand.NextDouble() * 10;

                var telemetryDataPoint = new
                {
                    deviceId = "myDotnetDevice",
                    temparature = currenttemparature
                };
                var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));

                await deviceClient.SendEventAsync(message);
                Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);

                Task.Delay(1000).Wait();
            }
        }
    }
}
