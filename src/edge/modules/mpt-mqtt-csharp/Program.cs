using System;
using System.Runtime.Loader;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Client.Transport.Mqtt;
using M2Mqtt;
using M2Mqtt.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ptm_mqtt_csharp
{
    class Program
    {
        private static ModuleClient ModuleClient;
        private static MqttClient MqttClient;
        private static string ModuleId;

        static void Main(string[] args)
        {
            Init().Wait();

            // Wait until the app unloads or is cancelled
            var cts = new CancellationTokenSource();
            AssemblyLoadContext.Default.Unloading += (ctx) => cts.Cancel();
            Console.CancelKeyPress += (sender, cpe) => cts.Cancel();
            WhenCancelled(cts.Token).Wait();
        }

        /// <summary>
        /// Handles cleanup operations when app is cancelled or unloads
        /// </summary>
        public static Task WhenCancelled(CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).SetResult(true), tcs);
            return tcs.Task;
        }

        public static async Task Init()
        {
            MqttTransportSettings mqttSetting = new MqttTransportSettings(TransportType.Mqtt_Tcp_Only);
            ITransportSettings[] settings = { mqttSetting };

            // Open a connection to the Edge runtime
            ModuleClient = await ModuleClient.CreateFromEnvironmentAsync(settings);
            await ModuleClient.OpenAsync();
            Console.WriteLine("IoT Hub module client initialized.");

            // Get environment variables scoped for this module
            ModuleId = Environment.GetEnvironmentVariable("IOTEDGE_MODULEID");


            await ModuleClient.SetInputMessageHandlerAsync("ptm_dm_input", ForwardDirectMethodRequestToLeafDevice, null);


            MqttClient = new MqttClient("127.0.0.1");
            MqttClient.Connect($"{ModuleId}_client");

            MqttClient.MqttMsgPublishReceived += MqttClientOnMqttMsgPublishReceived;

            MqttClient.Subscribe(new[] {"device/#"}, new [] {MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE});
            
            Console.WriteLine("MQTT client initialized.");
        }

        private static Task<MessageResponse> ForwardDirectMethodRequestToLeafDevice(Message message, object usercontext)
        {
            string leafDeviceId = message.Properties["leafdeviceid"];
            string methodName = message.Properties["method"];

            Console.WriteLine($"Received request for direct method '{methodName}' on leaf device '{leafDeviceId}'");

            string topic = $"device/{leafDeviceId}/directmethod/{methodName}/request";

            var mqttMsg = new
            {
                RequestId = message.MessageId,
                Data = JObject.Parse(Encoding.UTF8.GetString(message.GetBytes()))
            };
            
            MqttClient.Publish(topic, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(mqttMsg)));

            return Task.FromResult(MessageResponse.Completed);
        }

        private static async void MqttClientOnMqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            Console.WriteLine($"Received message on topic '{e.Topic}'");

            var dataObject = JObject.Parse(Encoding.UTF8.GetString(e.Message));

            if (e.Topic.EndsWith("/message"))
            {
                await ForwardMessageFromLeafDevice(e.Topic, dataObject);
            }
            else if (e.Topic.Contains("/directmethod/") && e.Topic.EndsWith("/response"))
            {
                await ForwardDirectMethodResponseFromLeafDevice(e.Topic, dataObject);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Listens to messages with topic "device/{device_id}/message" from the broker
        /// </summary>
        private static async Task ForwardMessageFromLeafDevice(string topic, JObject data)
        {
            try
            {
                var deviceId = topic.Split('/')[1];

                Console.WriteLine($"Received message from leaf device '{deviceId}'.");

                var body = new
                {
                    topic = topic,
                    payload = data
                };

                using (var message = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(body))))
                {
                    message.Properties.Add("leafdeviceid", deviceId);
                    message.Properties.Add("moduleid", ModuleId);

                    await ModuleClient.SendEventAsync("ptm_output", message);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR: Could not forward message from leaf device: {e.Message}\n{e.StackTrace}");
            }
        }

        /// <summary>
        /// Listens to messages with topic "device/{device_id}/directmethod/{method_name}/response" from the broker, which
        /// are responses to earlier direct method request messages
        /// </summary>
        private static async Task ForwardDirectMethodResponseFromLeafDevice(string topic, JObject data)
        {
            try
            {
                var topicParts = topic.Split('/');
                var deviceId = topicParts[1];
                var methodName = topicParts[3];

                var requestId = data["RequestId"].Value<string>();

                Console.WriteLine($"Received response to direct method '{methodName}' from leaf device '{deviceId}'.");

                var body = new
                {
                    topic = topic,
                    payload = data["Data"]
                };

                using (var message = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(body))))
                {
                    message.CorrelationId = requestId;

                    message.Properties.Add("leafdeviceid", deviceId);
                    message.Properties.Add("moduleid", ModuleId);

                    await ModuleClient.SendEventAsync("ptm_dm_output", message);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR: Could not forward message from leaf device: {e.Message}\n{e.StackTrace}");
            }
        }
    }
}
