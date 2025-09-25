using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using Profile;

namespace KartRider
{
    /// <summary>
    /// GitHub Releases API返回的根对象 (仅包含需要的字段, 其他字段已忽略)
    /// </summary>
    public class GitHubReleaseRoot
    {
        /// <summary>
        /// 发布版本下的所有资产文件 (如exe、zip)
        /// </summary>
        public GitHubReleaseAsset[] Assets { get; set; }

        /// <summary>
        /// 发布版本的标签名 (如250101)
        /// </summary>
        public string Tag_Name { get; set; }

        /// <summary>
        /// 发布版本的详细信息 (如更新日志)
        /// </summary>
        public string Body { get; set; }
    }

    /// <summary>
    /// GitHub Releases中的单个资产文件 (如Launcher.exe)
    /// </summary>
    public class GitHubReleaseAsset
    {
        /// <summary>
        /// 文件名 (如Launcher.exe)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 文件的SHA256哈希值 (格式: sha256:xxxxxx)
        /// </summary>
        public string Digest { get; set; }

        /// <summary>
        /// 文件的浏览器下载链接
        /// </summary>
        public string Browser_Download_Url { get; set; }
    }

    internal static class Update
    {
        public const string owner = "TheMagicFlute";    // GitHub Repo Owner
        public const string repo = "Launcher_V2";       // GitHub Repo Name

        public static string simpleName = "Launcher_" + Program.architecture;
        public static string zipName = simpleName + ".zip";
        public static string exeName = simpleName + ".exe";

        public static GitHubReleaseRoot? releaseData;
        public static GitHubReleaseAsset? launcherAsset;

        public static string currentVersion = GetCurrentVersion();
        private static string update_url = "";

        private static string updateScript = $"@echo off{Environment.NewLine}" +
            $"timeout /t 3 /nobreak{Environment.NewLine}" +
            $"move \"{Path.GetFullPath(Path.Combine(FileName.Update_Folder, exeName))}\" \"{Path.GetFullPath(FileName.AppDir)}\"{Environment.NewLine}" +
            $"del \"{Path.GetFullPath(FileName.AppDir)}\" /f /q{Environment.NewLine}" +
            $"start \"{Path.GetFullPath(Path.Combine(FileName.AppDir, exeName))}\"{Environment.NewLine}";

        private static readonly List<string> proxyUrlList =
        [
            "https://ghproxy.net/",
            "https://gh-proxy.com/",
        ];

        /// <summary>check and try to apply update</summary>
        /// <returns>true if update should be applied</returns>
        public static async Task<bool> UpdateDataAsync()
        {
            Console.WriteLine("正在检查更新...");
            Console.WriteLine($"当前Branch: {ThisAssembly.Git.RepositoryUrl}/{ThisAssembly.Git.Branch}");
            Console.WriteLine($"当前Commit: {ThisAssembly.Git.Commit}");
            Console.WriteLine($"当前Commit SHA: {ThisAssembly.Git.Sha}");
            Console.WriteLine($"当前Commit日期: {ThisAssembly.Git.CommitDate}");
            Console.WriteLine($"当前版本为: {currentVersion}");

            releaseData = await GetReleaseDetailAsync();

            if (releaseData == null)
            {
                Console.WriteLine("更新信息获取失败!");
                Console.WriteLine($"如果仍想更新, 请重新启动本程序, 或者访问 https://github.com/{owner}/{repo}/releases/latest 手动下载最新版本");
                return false;
            }

            if (releaseData?.Assets == null || releaseData.Assets.Length == 0)
            {
                Console.WriteLine("错误: API返回的资产列表为空");
                return false;
            }
            launcherAsset = Array.Find(
                releaseData.Assets,
                asset => string.Equals(asset.Name, zipName, StringComparison.OrdinalIgnoreCase) // 忽略文件名大小写
           );
            if (launcherAsset == null)
            {
                Console.WriteLine($"错误: 在资产列表中找不到 {zipName}");
                return false;
            }

            // 检查更新: 由于版本号使用日期格式, 当天多次发布的版本号相同, 故选择直接比较Digest
            // 如目前版本的SHA256与最新版本的SHA256不同, 则说明需要更新
            string? currentExeFile = Process.GetCurrentProcess().MainModule.FileName ?? Assembly.GetExecutingAssembly().Location;
            if (String.IsNullOrEmpty(currentExeFile))
            {
                Console.WriteLine("无法获取当前可执行文件路径, 更新检查失败.");
                return false;
            }
            if (CheckFileHash(Path.Combine(FileName.AppDir, currentExeFile), launcherAsset.Digest))
            {
                // ask user whether to update
                Console.WriteLine($"发现 {releaseData.Tag_Name} 中有更新!");
                LauncherSystem.PrintDivLine(25);
                Console.WriteLine($"更新信息: \n{releaseData.Body}");
                LauncherSystem.PrintDivLine(25);
                Console.Write("请问是否需要更新? (Y/n): ");
                string? usrInput = Console.ReadLine().ToLower();
                if (usrInput != "y")
                {
                    Console.WriteLine($"用户取消更新. 如果仍想更新, 请重新启动本程序, 或者访问 https://github.com/{owner}/{repo}/releases/latest 手动下载最新版本");
                    return false; // cancel update
                }
                // 尝试下载最新的版本
                update_url = await ProcessUrlAsync(launcherAsset.Browser_Download_Url);
                return await DownloadUpdateAsync(update_url);
            }
            else
            {
                Console.WriteLine("当前已是最新版本. ");
                return false;
            }
        }

        public static async Task<string> ProcessUrlAsync(string url)
        {
            try
            {
                string country = ProfileService.ProfileConfig.ServerSetting.CC.ToString();
                // 中国大陆需要使用代理下载, 处理 url
                if (country != "" && country == "CN")
                {
                    Console.WriteLine("Using proxy...");
                    foreach (string proxyUrl in proxyUrlList)
                    {
                        string testUrl = proxyUrl + url;
                        // testUrl = proxyUrl + url.Replace("https://", ""); // another api format without "https://"

                        if (await GetUrl(testUrl))
                        {
                            url = testUrl;
                            break;
                        }
                    }
                }
                // 代理网址处理完成/无需处理, 返回.
                return url;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取下载地址时出现错误: {ex.Message}");
            }
            return "";
        }

        public static async Task<bool> DownloadUpdateAsync(string UpdatePackageUrl)
        {
            try
            {
                Console.WriteLine($"正在 从 {update_url} 下载 {releaseData.Tag_Name}...");
                using (HttpClient client = new HttpClient())
                {
                    using (HttpResponseMessage response = await client.GetAsync(UpdatePackageUrl, HttpCompletionOption.ResponseHeadersRead))
                    {
                        response.EnsureSuccessStatusCode();
                        using (Stream contentStream = await response.Content.ReadAsStreamAsync())
                        {
                            if (!Directory.Exists(FileName.Update_Folder))
                                Directory.CreateDirectory(FileName.Update_Folder);

                            long? totalBytes = response.Content.Headers.ContentLength;
                            DateTime startTime = DateTime.Now;
                            using (FileStream fileStream = new FileStream(Path.Combine(FileName.Update_Folder, zipName), FileMode.Create, FileAccess.Write, FileShare.None))
                            {
                                byte[] buffer = new byte[4096];
                                int bytesRead;
                                long totalRead = 0;
                                while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                                {
                                    await fileStream.WriteAsync(buffer, 0, bytesRead);
                                    totalRead += bytesRead;
                                    double progress = totalBytes.HasValue ? (double)totalRead / totalBytes.Value * 100 : 0;
                                    string progressBar = "";
                                    int i = 1;
                                    for (; i * 2 <= progress; i++)
                                        progressBar += "#";
                                    for (; i * 2 <= 100; i++)
                                        progressBar += "=";
                                    Console.Write($"\r下载进度: {progress:F2}% [{progressBar}]");
                                }
                            }
                            DateTime endTime = DateTime.Now;
                            Console.WriteLine($"\n下载完成! 总用时: {(endTime - startTime).TotalMilliseconds}ms");
                        }
                    }
                    Console.WriteLine($"期望 SHA256 为: {launcherAsset.Digest}");
                    if (!CheckFileHash(Path.Combine(FileName.Update_Folder, zipName), launcherAsset.Digest))
                    {
                        Console.WriteLine($"文件 SHA256 校验失败, 下载可能不完整或被篡改.");
                        Console.WriteLine($"如果仍想更新, 请重新启动本程序, 或者访问 https://github.com/{owner}/{repo}/releases/latest 手动下载最新版本");
                        return false;
                    }
                    Console.WriteLine($"文件 SHA256 校验成功.");
                    return ApplyUpdate();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"下载过程中出现错误: {ex.Message}");
                Console.WriteLine($"如果仍想更新, 请重新启动本程序, 或者访问 https://github.com/{owner}/{repo}/releases/latest 手动下载最新版本");
                return false;
            }
        }

        public static bool ApplyUpdate()
        {
            try
            {
                Console.WriteLine("正在尝试应用更新...");
                System.IO.Compression.ZipFile.ExtractToDirectory(Path.Combine(FileName.Update_Folder, zipName), FileName.Update_Folder);
                try
                {
                    File.WriteAllText(FileName.Update_File, updateScript, Program.targetEncoding);
                    Console.WriteLine("\n写入更新脚本成功.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n写入更新脚本时出错: {ex.Message}");
                }
                MessageBox.Show("更新已准备就绪! 确认以重启程序应用更新!", "更新准备就绪", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Process.Start(FileName.Update_File);
                Environment.Exit(0);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n应用更新时出错: {ex.Message}");
                Console.WriteLine($"如果仍想更新, 请重新启动本程序, 或者访问 https://github.com/{owner}/{repo}/releases/latest 手动下载最新版本");
                return false;
            }
        }

        public static async Task<GitHubReleaseRoot?> GetReleaseDetailAsync()
        {
            try
            {
                // 创建HttpClient (设置User-Agent, 避免GitHub API拒绝请求)
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("GitHubReleaseParser/1.0"); // GitHub要求必须设置User-Agent
                httpClient.Timeout = TimeSpan.FromSeconds(10); // 设置10秒超时

                // 发送GET请求获取API响应
                var response = await httpClient.GetAsync($"https://api.github.com/repos/{owner}/{repo}/releases/latest");
                response.EnsureSuccessStatusCode(); // 若状态码不是200-299, 抛出异常 (如404、500)

                // 读取响应内容并反序列化为C#对象
                var jsonContent = await response.Content.ReadAsStringAsync();
                var releaseData = JsonSerializer.Deserialize<GitHubReleaseRoot>(
                    jsonContent,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true } // 忽略JSON字段大小写 (适配API的camelCase)
               );
                foreach (var asset in releaseData.Assets)
                {
                    asset.Digest = asset.Digest.Replace("sha256:", "").ToLower(); // 只保留哈希值部分, 并转换为小写
                }
                return releaseData;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON反序列化错误: {ex.Message}");
                Console.WriteLine("可能原因: API返回格式异常");
                return null;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP请求错误: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"未知错误: {ex.Message}");
                return null;
            }
        }

        public static async Task<string> GetCountryAsync()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync("https://ipinfo.io/json");
                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        JObject data = JObject.Parse(json);
                        if (data.TryGetValue("country", out JToken? countryToken))
                        {
                            return countryToken.ToString();
                        }
                        else
                        {
                            Console.WriteLine("响应中未找到 'country' 字段.");
                            return "";
                        }
                    }
                    else
                    {
                        Console.WriteLine($"请求 IP 地址失败, 状态码: {response.StatusCode}");
                        return "";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"请求 IP 地址时发生异常: {ex.Message}");
                return "";
            }
        }

        public static string GetCompileDate()
        {
            // Assembly assembly = Assembly.GetExecutingAssembly();
            // AssemblyName assemblyName = assembly.GetName();
            // string simpleName = assemblyName.Name + ".exe";
            DateTime compilationDate = File.GetLastWriteTime(Process.GetCurrentProcess().MainModule.FileName);
            string formattedDate = compilationDate.ToString("yyMMdd");
            return formattedDate;
        }

        public static string GetCurrentVersion()
        {
#if DEBUG
            return DateTime.Now.ToString("yyMMdd");
#else
            return ThisAssembly.Git.CommitDate.Substring(0, 10).Replace("-", "").Substring(2, 6);
#endif
        }

        public static bool CheckFileHash(string filePath, string expectedHash)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"文件 {filePath} 不存在.");
                return false;
            }
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] hash = sha256.ComputeHash(fileStream);
                    string actualHash = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                    Console.WriteLine($"文件 SHA256 为: {actualHash}");
                    return actualHash == expectedHash;
                }
            }
        }

        public static async Task<bool> GetUrl(string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));

                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"请求 URL 时发生异常: {ex.Message}");
                Console.WriteLine($"请检查你的网络是否连接正常.如连接正常, 请在 https://github.com/{owner}/{repo}/issues 提问.");
                return false;
            }
        }
    }
}

