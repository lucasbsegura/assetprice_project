using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using System.Threading.Tasks;


namespace AssetService2.Infrastructure
{
    internal class Mosquitto
    {
        

        public static async void Publish(string msg)
        {
            var mqttConfig = GetConfig();
            var mqttClient = CreateMqttClient();
            var options = BuildMqttClientOptions(mqttConfig);

            await mqttClient.ConnectAsync(options);
            var message = BuildMqttMessage(msg, mqttConfig);

            await mqttClient.PublishAsync(message);
            await Task.Delay(2000); // Wait for 2 second
        }

        private static MqttApplicationMessage BuildMqttMessage(string msg, MqttConfig mqttConfig)
        {
            return new MqttApplicationMessageBuilder()
                                .WithTopic(mqttConfig.Topic)
                                .WithPayload($"{msg}")
                                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                                .WithRetainFlag()
                                .Build();
        }

        private static MqttClientOptions BuildMqttClientOptions(MqttConfig mqttConfig)
        {
            // Create MQTT client options
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(mqttConfig.BrokerAddress, mqttConfig.Port) // MQTT broker address and port
                                                                          //.WithCredentials(username, password) // Set username and password
                .WithClientId(mqttConfig.ClientId)
                .WithCleanSession()
                .Build();
            return options;
        }

        private static IMqttClient CreateMqttClient()
        {
            // Create a MQTT client factory
            var factory = new MqttFactory();
            // Create a MQTT client instance
            var mqttClient = factory.CreateMqttClient();
            return mqttClient;
        }

        private static MqttConfig GetConfig()
        {
            return new MqttConfig
            {
                BrokerAddress = "localhost",
                Port = 1883,
                ClientId = "AssetService2",
                Topic = "AssetPrices"
                //string username = "emqxtest";
                //string password = "******";
            };
        }
    }

    internal class MqttConfig
    {
        public string BrokerAddress { get; set; }
        public int Port { get; set; }
        public string ClientId { get; set; }
        public string Topic { get; set; }   

    }
}
