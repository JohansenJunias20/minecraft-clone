using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Websocket.Client;
using Newtonsoft.Json;
using OpenTK.Mathematics;

namespace Minecraft_Clone_with_Open_GL
{
    public delegate void MyDel(string str);
    public struct WS
    {
        public string host;
        public int port;

    };

    public struct UDP
    {
        public string host;
        public int port;

    };
    class Socket
    {
        //public MyDel MyEvent;
        public event MyDel MyEvent;
        private WS WS;
        private UDP UDP;
        public string nickname { get; private set; }
        public string ID { get; private set; }


        private void readConfigFile()
        {
            dynamic result = JsonConvert.DeserializeObject(System.IO.File.ReadAllText(@$"{Program.__DIR__}\config.json"));
            UDP.host = result["UDP"]["host"];
            UDP.port = result["UDP"]["port"];
            WS.host = result["WS"]["host"];
            WS.port = result["WS"]["port"];
        }
        UdpClient clientUDP = new UdpClient();

        private async void connectUDP()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(UDP.host), UDP.port); // endpoint where server is listening
            Console.WriteLine("connecting to udp server ...");
            clientUDP.Connect(ep);
            Console.WriteLine($"connected to UDP Server {UDP.host}:{UDP.port}");
            //sendMessageInitializeIntervally();
            //string initMSG = "join|";
            //send initialize join to UDP
            //clientUDP.Send(Encoding.ASCII.GetBytes(initMSG), Encoding.ASCII.GetBytes(initMSG).Length);
            while (true)
            {

                var receivedResult = await clientUDP.ReceiveAsync();
                recieveUDPMessage(receivedResult);
                //Console.Write(Encoding.ASCII.GetString(receivedResult.Buffer));

                //MyEvent(Encoding.ASCII.GetString(receivedResult.Buffer));
            }
        }
        WebsocketClient client;
        private void connectWS()
        {
            //$""
            Console.WriteLine($"Connecting to WS Server ws://{WS.host}:{WS.port} ...");
            client = new WebsocketClient(new Uri($"ws://{WS.host}:{WS.port}"));
            client.ReconnectTimeout = null;
            client.ReconnectionHappened.Subscribe(info =>
            {
                Console.WriteLine($"Reconnection happened, type: {info.Type}");
                if (info.Type.ToString().ToLower() == "initial")
                    client.Send(
                    "{" +
                    "   \"channel\":\"join\"" +
                    "" +
                    "}"
                    );
            }
                );

            client.MessageReceived.Subscribe(msg =>
            {
                recieveWSMessage(msg.Text);
            }
            );
            client.Start();


            Console.WriteLine("Connected to WS Server");

            //Task.Run(() => client.Send("{ message }"));

            //exitEvent.WaitOne();
        }
        private void recieveWSMessage(string msg)
        {
            Console.WriteLine($"recieving WS msg {msg}");
            dynamic deserializedMsg = JsonConvert.DeserializeObject(msg);

            Console.WriteLine($"channel: {deserializedMsg["channel"]}");

            //MyEvent("msg");
            var sendInitializeIntervallyTask = new Task(sendMessageInitializeIntervally);
            switch (Convert.ToString(deserializedMsg["channel"]))
            {
                case "join":
                    Program.win.playerJoin(Convert.ToString(deserializedMsg["ID"]));
                    break;
                case "nickname":
                    nickname = deserializedMsg["nickname"];

                    break;
                case "ID":
                    Console.WriteLine("recieving ID from WS, starting sending init message to UDP");
                    ID = Convert.ToString(deserializedMsg["ID"]);
                    sendInitializeIntervallyTask.Start();
                    break;
                case "verified":
                    stopSendInitializeMsg = true;
                    Console.WriteLine("Connetion Established");
                    break;
                default:
                    break;
            }


        }
        bool stopSendInitializeMsg = false;
        //kirim pesan inisiialisasi
        private void sendMessageInitializeIntervally()
        {
            string msg = $"init|ID|{ID}";
            var Bmsg = Encoding.ASCII.GetBytes(msg);
            while (!stopSendInitializeMsg)
            {
                System.Threading.Thread.Sleep(1000);
                clientUDP.Send(Bmsg, Bmsg.Length);
            }
        }

        private void recieveUDPMessage(UdpReceiveResult receivedResult)
        {
            //Console.WriteLine(Encoding.ASCII.GetString(receivedResult.Buffer));
            try
            {
                //di try parse karena UDP, biasanya ada beberapa character yang hilang
                dynamic result = JsonConvert.DeserializeObject(Encoding.ASCII.GetString(receivedResult.Buffer));
                if (result["channel"] == "position")
                {
                    //Console.WriteLine($"recieve x:{result["position"]["x"]},y:{result["position"]["y"]},z:{result["position"]["z"]}");
                    Vector3 pos = new Vector3((float)Convert.ToDouble(result["position"]["x"]), (float)Convert.ToDouble(result["position"]["y"]), (float)Convert.ToDouble(result["position"]["z"]));
                    float yaw =(float) Convert.ToDouble(result["position"]["yaw"]);
                    //Console.WriteLine(pos);
                    Program.win.setPosition(pos,yaw,Convert.ToString( result["ID"]));

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error!");
                Console.WriteLine(ex.Message);
            };
        }


        #region public api
        public void connect()
        {
            readConfigFile();
            connectUDP();
            connectWS();

        }
        public async void sendPosition(Vector3 pos, float yaw) // yaw -> degree of the camera
        {
            string jsonFormat =
                "{  " +
                "   \"channel\":\"position\"," +
                $" \"ID\":\"{ID}\"," +
                "   \"position\":{" +
                $"       \"x\":\"{pos.X.ToString("0.####").Replace(",",".")}\"," +
                $"       \"y\":\"{pos.Y.ToString("0.####").Replace(",", ".")}\"," +
                $"       \"z\":\"{pos.Z.ToString("0.####").Replace(",", ".")}\"," +
                $"       \"yaw\":\"{yaw.ToString("0.####").Replace(",", ".")}\"" +
                "   }" +
                "}"
                ;
            //string jsonFormat =
            //    "{  " +
            //    "   \"channel\":\"position\"," +
            //    $"   \"position\":5" +
            //    "}"
            //    ;
            //Console.WriteLine(jsonFormat);
            byte[] bJsonFormat = Encoding.ASCII.GetBytes(jsonFormat);
            await clientUDP.SendAsync(bJsonFormat, bJsonFormat.Length);
        }
        #endregion

    }
}
