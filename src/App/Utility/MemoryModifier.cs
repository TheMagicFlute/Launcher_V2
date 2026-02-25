using Launcher.App.Profile;
using Launcher.App.Server;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Launcher.App.Utility;

class MemoryModifier
{
    // 导入Windows API（内存操作所需）
    [DllImport("kernel32.dll")]
    private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

    [DllImport("kernel32.dll")]
    private static extern bool ReadProcessMemory(
        IntPtr hProcess,
        IntPtr lpBaseAddress,
        byte[] lpBuffer,
        int dwSize,
        out int lpNumberOfBytesRead
    );

    [DllImport("kernel32.dll")]
    private static extern bool WriteProcessMemory(
        IntPtr hProcess,
        IntPtr lpBaseAddress,
        byte[] lpBuffer,
        int nSize,
        out int lpNumberOfBytesWritten
    );

    [DllImport("kernel32.dll")]
    private static extern bool CloseHandle(IntPtr hObject);

    [DllImport("kernel32.dll")]
    private static extern IntPtr VirtualQueryEx(
        IntPtr hProcess,
        IntPtr lpAddress,
        out MEMORY_BASIC_INFORMATION lpBuffer,
        uint dwLength
    );

    // 窗口操作 API（Unicode）
    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern bool SetWindowTextW(IntPtr hWnd, string lpString);

    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern int GetWindowTextLengthW(IntPtr hWnd);

    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern int GetWindowTextW(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool IsWindowVisible(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

    private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    // 内存区域信息结构体（用于枚举内存页）
    [StructLayout(LayoutKind.Sequential)]
    private struct MEMORY_BASIC_INFORMATION
    {
        public IntPtr BaseAddress;
        public IntPtr AllocationBase;
        public uint AllocationProtect;
        public IntPtr RegionSize;
        public uint State;
        public uint Protect;
        public uint Type;
    }

    // 进程内存操作权限（读取+写入+查询内存信息）
    private const uint PROCESS_ACCESS_FLAGS = 0x0010 | 0x0020 | 0x0008; // PROCESS_VM_READ | PROCESS_VM_WRITE | PROCESS_QUERY_INFORMATION

    public async Task LaunchAndModifyMemory(string kartRiderDirectory)
    {
        DataPacket packet = new DataPacket
        {
            Nickname = ProfileService.SettingConfig.Name,
            TimeTicks = MultiPlayer.GetUpTime()
        };

        Process? process = default;
        try
        {
            // 1. 启动目标进程
            string passport = Base64Helper.Encode(JsonHelper.Serialize(packet));
            ProcessStartInfo startInfo = new ProcessStartInfo("KartRider.exe", $"TGC -region:3 -passport:{passport}")
            {
                WorkingDirectory = Path.GetFullPath(kartRiderDirectory),
                UseShellExecute = true,
                Verb = "runas" // 以管理员权限运行
            };

            process = Process.Start(startInfo);
            if (process is null)
            {
                Console.WriteLine("游戏进程启动失败, 请检查权限后重启.");
                MessageBox.Show("游戏进程启动失败, 请检查权限后重启.", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Console.WriteLine($"进程已启动, PID: {process.Id}");

            // 2. 立即尝试修改内存
            Console.WriteLine("正在尝试修改星标赛道数量限制...");
            bool success = ModifyMemory(process.Id, [0x83, 0xFA, 0x32], [0x83, 0xFA, 0x78]);
            if (success)
                Console.WriteLine("修改星标赛道数量限制: 50 -> 120");
            else
                Console.WriteLine("未找到目标内存特征码，修改失败");

            // 3. 等待窗口出现并追加标题（更稳健的等待策略）
            string suffix = $" - {ProfileService.SettingConfig.Name} (launched by Kart Launcher)";
            // 异步等待并追加；不阻塞调用线程
            _ = Task.Run(async () =>
            {
                // TODO: 60 秒内以固定 10s 间隔无限次尝试
                TimeSpan totalTimeout = TimeSpan.FromSeconds(60);
                bool appended = await WaitAndAppendWindowTitleByPid(process.Id, suffix, totalTimeout);
                Console.WriteLine(appended ? $"窗口标题已追加: {suffix}" : "追加窗口标题失败或超时");
            });

            // 原有等待（如果需要让主流程等待进程进入空闲）
            try { process.WaitForInputIdle(10000); } catch { /* 忽略 */ }
        }
        catch (System.ComponentModel.Win32Exception ex)
        {
            Console.WriteLine($"UAC取消或权限不足: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"操作失败: {ex.Message}");
        }
        finally
        {
            process?.Dispose(); // 释放进程资源（不影响目标进程运行）
        }
    }

    /// <summary>
    /// 按 PID 枚举顶级窗口，等待可见且标题非空的窗口出现，然后在其标题尾部追加 suffix。
    /// 使用总超时与固定重试间隔（100ms），在 60 秒内无限次尝试直到成功或超时。
    /// </summary>
    private static async Task<bool> WaitAndAppendWindowTitleByPid(int pid, string suffix, TimeSpan timeout)
    {
        var sw = Stopwatch.StartNew();
        const int retryDelayMs = 10000; // 固定重试间隔 10s
        while (sw.Elapsed < timeout)
        {
            try
            {
                var handles = FindTopLevelWindowHandlesByPid(pid);
                foreach (var hWnd in handles)
                {
                    if (hWnd == IntPtr.Zero)
                        continue;

                    if (!IsWindowVisible(hWnd))
                        continue;

                    // 读取现有标题
                    int len = GetWindowTextLengthW(hWnd);
                    string original = string.Empty;
                    if (len > 0)
                    {
                        var sb = new StringBuilder(len + 1);
                        if (GetWindowTextW(hWnd, sb, sb.Capacity) > 0)
                            original = sb.ToString();
                    }

                    // 去重：如果已有相同后缀则跳过
                    if (!string.IsNullOrEmpty(original) && original.EndsWith(suffix))
                        return true;

                    string newTitle = string.IsNullOrEmpty(original) ? suffix.TrimStart() : original + suffix;
                    bool result = SetWindowTextW(hWnd, newTitle);
                    if (!result)
                    {
                        int err = Marshal.GetLastWin32Error();
                        Console.WriteLine($"SetWindowTextW 失败，错误码: {err}");
                        continue;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"等待/追加窗口标题时异常: {ex.Message}");
            }

            // 固定间隔重试（无限次，直到 timeout）
            await Task.Delay(retryDelayMs);
        }
        return false;
    }

    /// <summary>
    /// 枚举指定 PID 的顶级窗口句柄（可能有多个），返回列表。
    /// </summary>
    private static List<IntPtr> FindTopLevelWindowHandlesByPid(int pid)
    {
        var results = new List<IntPtr>();

        try
        {
            EnumWindows((hWnd, lParam) =>
            {
                if (hWnd == IntPtr.Zero)
                    return true;

                GetWindowThreadProcessId(hWnd, out uint windowPid);
                if ((int)windowPid == pid)
                {
                    results.Add(hWnd);
                }
                return true; // 继续枚举
            }, IntPtr.Zero);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"枚举窗口失败: {ex.Message}");
        }

        return results;
    }

    /// <summary>
    /// 在目标进程中查找特征码并修改
    /// </summary>
    /// <param name="processId">进程ID</param>
    /// <param name="searchBytes">要查找的字节序列</param>
    /// <param name="replaceBytes">要替换的字节序列</param>
    /// <returns>是否修改成功</returns>
    private bool ModifyMemory(int processId, byte[] searchBytes, byte[] replaceBytes)
    {
        if (searchBytes.Length != replaceBytes.Length)
            Console.WriteLine("查找和替换的字节长度必须一致");

        IntPtr hProcess = OpenProcess(PROCESS_ACCESS_FLAGS, false, processId);
        if (hProcess == IntPtr.Zero)
            Console.WriteLine("无法打开进程, 可能权限不足");

        try
        {
            IntPtr address = IntPtr.Zero;
            while (true)
            {
                // 枚举进程内存页
                if (VirtualQueryEx(hProcess, address, out MEMORY_BASIC_INFORMATION mbi, (uint)Marshal.SizeOf<MEMORY_BASIC_INFORMATION>()) == IntPtr.Zero)
                    break;

                // 只处理可读写的私有内存页（避免系统内存或只读内存）
                if (mbi.State == 0x1000 && // MEM_COMMIT（已提交的内存）
                    (mbi.Protect == 0x04 || mbi.Protect == 0x08 || mbi.Protect == 0x10 || // PAGE_READWRITE, PAGE_WRITECOPY, PAGE_EXECUTE_READWRITE
                     mbi.Protect == 0x80 || mbi.Protect == 0x40)) // PAGE_EXECUTE_WRITECOPY, PAGE_READWRITE
                {
                    // 读取当前内存页数据
                    byte[] buffer = new byte[(int)mbi.RegionSize];
                    if (ReadProcessMemory(hProcess, mbi.BaseAddress, buffer, buffer.Length, out int bytesRead) && bytesRead > 0)
                    {
                        // 在当前页中搜索特征码
                        int index = FindBytes(buffer, searchBytes);
                        if (index != -1)
                        {
                            // 计算实际内存地址
                            IntPtr targetAddress = IntPtr.Add(mbi.BaseAddress, index);
                            Console.WriteLine($"找到特征码, 地址: 0x{targetAddress:X}");

                            // 修改内存
                            if (WriteProcessMemory(hProcess, targetAddress, replaceBytes, replaceBytes.Length, out int bytesWritten) && bytesWritten == replaceBytes.Length)
                            {
                                return true;
                            }
                            else
                            {
                                Console.WriteLine("写入内存失败, 可能没有写入权限");
                            }
                        }
                    }
                }

                // 移动到下一个内存页
                address = IntPtr.Add(mbi.BaseAddress, (int)mbi.RegionSize);
            }

            return false; // 未找到特征码
        }
        finally
        {
            CloseHandle(hProcess); // 释放进程句柄
        }
    }

    /// <summary>
    /// 在字节数组中查找目标序列
    /// </summary>
    private int FindBytes(byte[] buffer, byte[] searchBytes)
    {
        for (int i = 0; i <= buffer.Length - searchBytes.Length; i++)
        {
            bool match = true;
            for (int j = 0; j < searchBytes.Length; j++)
            {
                if (buffer[i + j] != searchBytes[j])
                {
                    match = false;
                    break;
                }
            }
            if (match)
                return i;
        }
        return -1;
    }
}
