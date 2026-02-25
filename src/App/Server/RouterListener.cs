using Launcher.App.Profile;
using System.Net;
using System.Net.Sockets;

namespace Launcher.App.Server
{
    public class RouterListener
    {
        public static IPAddress sIP { get; set; }

        public static IPEndPoint CurrentUDPServer { get; set; }

        public static TcpListener Listener { get; private set; }

        public static SessionGroup MySession { get; set; }

        public static bool IsRunning { get; private set; } = false;

        public static int[] DataTime()
        {
            DateTime dt = DateTime.Now;
            DateTime time = new DateTime(1900, 1, 1, 0, 0, 0);
            TimeSpan t = dt.Subtract(time);
            double totalSeconds = dt.TimeOfDay.TotalSeconds / 4;
            int MonthCount = (dt.Year - 1900) * 12 + dt.Month;
            int oddMonthCount = (MonthCount + 1) / 2;
            return [t.Days, (int)totalSeconds, oddMonthCount];
        }

        public static void OnAcceptSocket(IAsyncResult ar)
        {
            try
            {
                // 如果 listener 已停止或正在停止，不要处理或重新开始 Accept
                if (!IsRunning || Listener is null)
                {
                    return;
                }

                // EndAcceptSocket 可能因 Listener 被 Stop 而抛出 ObjectDisposedException
                Socket clientSocket = Listener.EndAcceptSocket(ar);

                // 创建客户端会话（自动开始接收消息）
                MySession = new SessionGroup(clientSocket, null);

                // 将会话添加到管理类
                ClientManager.AddClient(MySession);
            }
            catch (ObjectDisposedException)
            {
                // Listener 已被释放（通常在 Stop() 被调用后），忽略并不重新开始 Accept
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Socket 异常：{ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生异常：{ex.Message}");
            }
            finally
            {
                try
                {
                    // 仅当 listener 仍然存在且仍在绑定时才重新开始 Accept
                    if (IsRunning && Listener is not null && Listener.Server is not null && Listener.Server.IsBound)
                    {
                        Listener.BeginAcceptSocket(new AsyncCallback(OnAcceptSocket), Listener);
                    }
                }
                catch (ObjectDisposedException)
                {
                    // 如果 Listener 在此期间被释放，忽略
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"开始接受连接时发生异常：{ex.Message}");
                }
            }
        }

        public static bool Start()
        {
            Console.WriteLine("[Server] Starting server...");
            try
            {
                if (Listener is null || CurrentUDPServer is null)
                {
                    Listener = new TcpListener(IPAddress.Any, ProfileService.SettingConfig.ServerPort);
                    CurrentUDPServer = new System.Net.IPEndPoint(IPAddress.Any, 39311);
                }
                if (!Listener.Server.IsBound)
                {
                    var RouterIPList = LanIpGetter.GetAllLocalLanIps();
                    foreach (var ip in RouterIPList)
                    {
                        Console.WriteLine($"[Server] Loading remote server: {ip}:{ProfileService.SettingConfig.ServerPort}");
                    }
                    Listener.Start();
                    Listener.BeginAcceptSocket(OnAcceptSocket, Listener);
                }
                else
                {
                    Listener.BeginAcceptSocket(OnAcceptSocket, Listener);
                }
                IsRunning = true;
                Console.WriteLine("[Server] Server started and listening for connections.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Server] 启动监听时发生异常：{ex.Message}");
                return false;
            }
        }

        public static bool Stop()
        {
            Console.WriteLine("[Server] Stopping server...");
            try
            {
                // 先标记为停止，避免 OnAcceptSocket 在 Stop 期间重新开启 Accept
                IsRunning = false;

                if (Listener is not null && Listener.Server is not null && Listener.Server.IsBound)
                {
                    // 停止监听
                    Listener.Stop();
                }
                Console.WriteLine("[Server] Server stopped.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Server] 停止监听时发生异常：{ex.Message}");
                return false;
            }
        }
    }
}
