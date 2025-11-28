using Launcher.App.Profile;
using System.Net;
using System.Net.Sockets;

namespace Launcher.App.Server
{
    public class RouterListener
    {
        public static IPAddress sIP { get; set; }

        public static System.Net.IPEndPoint CurrentUDPServer { get; set; }

        public static TcpListener Listener { get; private set; }

        public static SessionGroup MySession { get; set; }

        public static int[] DataTime()
        {
            DateTime dt = DateTime.Now;
            DateTime time = new DateTime(1900, 1, 1, 0, 0, 0);
            TimeSpan t = dt.Subtract(time);
            double totalSeconds = dt.TimeOfDay.TotalSeconds / 4;
            int MonthCount = (dt.Year - 1900) * 12 + dt.Month;
            int oddMonthCount = (MonthCount + 1) / 2;
            return new int[] { t.Days, (int)totalSeconds, oddMonthCount };
        }

        public static void OnAcceptSocket(IAsyncResult ar)
        {
            try
            {
                Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Socket clientSocket = Listener.EndAcceptSocket(ar);

                // 创建客户端会话（自动开始接收消息）
                MySession = new SessionGroup(clientSocket, null);

                // 将会话添加到管理类
                ClientManager.AddClient(MySession);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生异常：{ex.Message}");
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
                Listener = new TcpListener(IPAddress.Any, ProfileService.SettingConfig.ServerPort);
                CurrentUDPServer = new System.Net.IPEndPoint(IPAddress.Any, 39311);
            }
            if (!Listener.Server.IsBound)
            {
                var RouterIPList = LanIpGetter.GetAllLocalLanIps();
                foreach (var ip in RouterIPList)
                {
                    Console.WriteLine($"[Server] Load server IP: {ip}:{ProfileService.SettingConfig.ServerPort}");
                }
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
