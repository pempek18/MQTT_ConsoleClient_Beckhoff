using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using System.Net;
using Newtonsoft.Json;

namespace Mqtt_1
{
    internal class Program
    {
        static string[] Topics = new string[1] { "Tornado/Tornado RD/TcIotCommunicator/Json/Tx/Data" };
        static byte[] qosLevel = new byte[1] { 0 };
        static byte[] IpAddress = new byte[4] {172,31,166,220 };
        string Json;
        static void Main(string[] args)
        {
            MqttClient mqttClient = new MqttClient(new IPAddress( IpAddress )) ;

            mqttClient.Connect("Tornado", "Admin", "04713");

            mqttClient.MqttMsgPublishReceived += MqttClient_MqttMsgPublishReceived;

            mqttClient.Subscribe(Topics, qosLevel);
        }

        private static void MqttClient_MqttMsgPublishReceived(object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
        {
            ASCIIEncoding ascii = new ASCIIEncoding();
            string msg = ascii.GetString(e.Message);;

            Console.WriteLine("Topic : " + e.Topic);

            ReciveData RD = JsonConvert.DeserializeObject<ReciveData>(msg);

            foreach ( var propertinfo in RD.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
            {
                Console.WriteLine("RD." + propertinfo.ToString() + " : \t\t\t\t" + propertinfo.GetValue(RD, null));
                if (propertinfo.Name == "values")
                {
                    foreach (var propertyinfo in RD.values.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
                    {
                        Console.WriteLine("RD.Values" + propertyinfo.ToString() + " : \t\t\t\t" + propertyinfo.GetValue(RD.values, null));
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine();
        }
    }
    /*Tornado/Tornado RD/TcIotCommunicator/Json/Tx/Data object*/
    class ReciveData
    {
        public DateTime Timestamp { get; set; }
        public string GroupName { get; set; }
        public Values values { get; set; }
    }
    /*Tornado/Tornado RD/TcIotCommunicator/Json/Tx/Data/Values object */
    class Values
    {
        public bool IoT_Safety { get; set; }
        public float IoT_BPM { get; set; }
        public float IoT_Power_AVG { get; set; }
        public float IoT_Current_L1 { get; set; }
        public float IoT_Current_L2 { get; set; }
        public float IoT_Current_L3 { get; set; }
        public float IoT_Frequnce { get; set; }
        public Int16 IoT_Tests_CNT_Total { get; set; }
        public Int16 IoT_Tests_CNT_Hourly { get; set; }
        public Int16 IoT_Good_CNT_Total { get; set; }
        public Int16 IoT_Good_CNT_Hourly { get; set; }
        public Int16 IoT_Bad_CNT_Total { get; set; }
        public Int16 IoT_Bad_CNT_Hourly { get; set; }
        public float IoT_Rejects_percentage { get; set; }
        public bool IoT_FallenBtlEnable { get; set; }
        public bool IoT_RejectVerificationEnable { get; set; }
        public bool IoT_ChuteBlockedEnable { get; set; }
        public bool IoT_OutfeedJamEnable { get; set; }
    }
}
