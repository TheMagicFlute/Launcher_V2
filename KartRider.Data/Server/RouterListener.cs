using System.Net;
using System.Net.Sockets;

namespace KartRider
{
    /// <summary>
    /// 用于监听客户端连接的路由器监听器静态类
    /// </summary>
    public static class RouterListener
    {
        public static string sIP;

        public static int port;

        public static string forceConnect;

        public static IPEndPoint client;

        public static IPEndPoint CurrentUDPServer { get; set; }

        public static string ForceConnect { get; set; }

        public static TcpListener Listener { get; private set; }

        public static SessionGroup MySession { get; set; }

        static RouterListener()
        {
            sIP = "127.0.0.1";
            port = 39312;
        }

        public static int[] DataTime()
        {
            DateTime dt = DateTime.Now;
            DateTime time = new DateTime(1900, 1, 1, 0, 0, 0);
            TimeSpan t = dt.Subtract(time);
            double totalSeconds = dt.TimeOfDay.TotalSeconds / 4;
            int Month = (dt.Year - 1900) * 12;
            int MonthCount = Month + dt.Month;
            double tempResult = (double)MonthCount / 2;
            int oddMonthCount = (int)tempResult + (tempResult % 1 != 0 ? 1 : 0);
            return new int[] { t.Days, (int)totalSeconds, oddMonthCount };
        }

        public static void OnAcceptSocket(IAsyncResult ar)
        {
            try
            {
                Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Socket clientSocket = Listener.EndAcceptSocket(ar);
                forceConnect = sIP;
                if (ForceConnect != "")
                {
                    forceConnect = ForceConnect;
                }
                MySession = new SessionGroup(clientSocket, null);
                IPEndPoint clientEndPoint = clientSocket.RemoteEndPoint as IPEndPoint;
                client = clientEndPoint;
                Console.WriteLine($"Client IP: {client.Address}:{client.Port}");
                if (File.Exists(Launcher.PinFileBak)) // restore PinFile
                {
                    File.Delete(Launcher.PinFile);
                    File.Move(Launcher.PinFileBak, Launcher.PinFile);
                }
                GameSupport.PcFirstMessage();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生异常: {ex.Message}");
            }
            finally
            {
                Listener.BeginAcceptSocket(new AsyncCallback(OnAcceptSocket), null);
            }
        }

        public static void Start()
        {
            if (Listener == null || CurrentUDPServer == null)
            {
                Listener = new TcpListener(IPAddress.Parse(sIP), port);
                CurrentUDPServer = new IPEndPoint(IPAddress.Parse(sIP), 39311);
            }
            if (!Listener.Server.IsBound)
            {
                Console.WriteLine($"Load Server: {sIP}:{port}");
                ForceConnect = "";
                Listener.Start();
                Listener.BeginAcceptSocket(OnAcceptSocket, Listener);
            }
            else
            {
                Listener.BeginAcceptSocket(OnAcceptSocket, Listener);
            }
        }
    }
}
