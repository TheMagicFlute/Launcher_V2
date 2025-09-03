using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Profile;

namespace KartRider
{
    internal static class Update
    {
        public const string owner = "TheMagicFlute";    // GitHub Repo Owner
        public const string repo = "Launcher_V2";       // GitHub Repo Name

        public static string simpleName = "Launcher_" + Program.architecture;
        public static string zipName = simpleName + ".zip";
        public static string exeName = simpleName + ".exe";

        public static string currentVersion = GetCurrentVersion();
        private static string update_url = "";
        private static string tag_name = "000000"; // 000000 for default, for unable to get remote tag name.
        private static string update_info = "";

        private static string updateScript = @$"@echo off
timeout /t 3 /nobreak
move {"\"" + Path.GetFullPath(Path.Combine(FileName.Update_Folder, exeName)) + "\""} {"\"" + Path.GetFullPath(FileName.AppDir) + "\""}
start {"\"\" \"" + Path.GetFullPath(Path.Combine(FileName.AppDir, exeName)) + "\""}
";

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

            tag_name = await GetTag_name();

            if (tag_name == "") // 更新请求失败
            {
                Console.WriteLine($"如果仍想更新，请重新启动本程序，或者访问 https://github.com/{owner}/{repo}/releases/latest 手动下载最新版本");
                return false;
            }

            if (int.Parse(currentVersion) < int.Parse(tag_name))
            {
                update_info = await GetUpdate_Info();
                // ask user whether to update
                Console.WriteLine($"发现新版本: {tag_name}");
                Console.WriteLine("-------------------------");
                Console.WriteLine($"更新信息: \n{update_info}");
                Console.WriteLine("-------------------------");
                Console.Write("请问是否需要更新? (Y/n): ");
                string usrInput = null;
                while (usrInput != "y" && usrInput != "n")
                {
                    usrInput = Console.ReadLine();
                    usrInput = usrInput.ToLower();
                    if (usrInput == "n")
                    {
                        return false; // cancel update
                    }
                    else if (usrInput == "y")
                    {
                        break;
                    }
                    // usrInput is invalid, ask again.
                    Console.Write("请输入 (Y for yes / n for no): ");
                }
                // 尝试下载最新的版本
                update_url = $"https://github.com/{owner}/{repo}/releases/download/{tag_name}/{zipName}";
                update_url = await ProcessUrlAsync(update_url);
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
                // 中国大陆需要使用代理下载，处理 url
                if (country != "" && country == "CN")
                {
                    Console.WriteLine("Using proxy...");
                    List<string> urls = new List<string>()
                    {
                        "https://ghproxy.net/",
                        "https://hub.myany.uk/",
                        "https://gh-proxy.com/",
                        "http://kra.myany.uk:2233/",
                        "http://krb.myany.uk:2233/"
                    };
                    foreach (string url_ in urls)
                    {
                        string testUrl = url;
                        if (url_ == "https://ghproxy.net/" || url_ == "https://hub.myany.uk/")
                        {
                            testUrl = url_ + url;
                        }
                        else
                        {
                            testUrl = url_ + url.Replace("https://", "");
                        }
                        if (await GetUrl(testUrl))
                        {
                            url = testUrl;
                            break;
                        }
                    }
                }
                // 代理网址处理完成/无需处理，返回。
                return url;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取下载地址时出现错误: {ex.Message}");
            }
            return "";
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

        public static async Task<bool> DownloadUpdateAsync(string UpdatePackageUrl)
        {
            try
            {
                Console.WriteLine($"正在 从 {update_url} 下载 {tag_name}...");
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
                    string expectedHash = await GetSHA256Async(zipName);
                    Console.WriteLine($"期望 SHA256 为: {expectedHash}");
                    if (!CheckFileHash(Path.Combine(FileName.Update_Folder, zipName), expectedHash))
                    {
                        Console.WriteLine($"文件 SHA256 校验失败，下载可能不完整或被篡改。");
                        Console.WriteLine($"如果仍想更新，请重新启动本程序，或者访问 https://github.com/{owner}/{repo}/releases/latest 手动下载最新版本");
                        return false;
                    }
                    else
                    {
                        Console.WriteLine($"文件 SHA256 校验成功。");
                    }
                    return ApplyUpdate();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"下载过程中出现错误: {ex.Message}");
                Console.WriteLine($"如果仍想更新，请重新启动本程序，或者访问 https://github.com/{owner}/{repo}/releases/latest 手动下载最新版本");
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
                    File.WriteAllText(FileName.Update_File, updateScript);
                    Console.WriteLine("\n写入更新脚本成功。");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n写入更新脚本时出错: {ex.Message}");
                }
                Process.Start(FileName.Update_File);
                Environment.Exit(0);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n应用更新时出错: {ex.Message}");
                Console.WriteLine($"如果仍想更新，请重新启动本程序，或者访问 https://github.com/{owner}/{repo}/releases/latest 手动下载最新版本");
                return false;
            }
        }

        public static async Task<string> GetSHA256Async(string filename)
        {
            HttpResponseMessage responseMsg = await GetReleaseInfoAsync();
            if (responseMsg == null) return ""; // fail to get response
            if (responseMsg.IsSuccessStatusCode)
            {
                // parse JSON
                string json = await responseMsg.Content.ReadAsStringAsync();
                dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                // find the asset with the same name
                foreach (var asset in data.assets)
                {
                    if (asset.name == filename)
                    {
                        string digest = asset.digest;
                        digest = digest.Substring("sha256:".Length);
                        return digest;
                    }
                }
                return "";
            }
            else
            {
                Console.WriteLine($"请求更新信息失败，状态码: {responseMsg.StatusCode}");
                return "";
            }
        }

        public static bool CheckFileHash(string filePath, string expectedHash)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"文件 {filePath} 不存在。");
                return false;
            }
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                SHA256 sha256 = new SHA256Managed();
                byte[] hash = sha256.ComputeHash(fileStream);
                string actualHash = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                Console.WriteLine($"文件 SHA256 为：{actualHash}");
                return actualHash == expectedHash;
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
                        string country = data["country"]?.ToString();
                        return country;
                    }
                    else
                    {
                        Console.WriteLine($"请求 IP 地址失败，状态码: {response.StatusCode}");
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

        public static async Task<HttpResponseMessage> GetReleaseInfoAsync()
        {
            string url = $"https://api.github.com/repos/{owner}/{repo}/releases/latest";
            url = await ProcessUrlAsync(url);
            try
            {

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", repo);
                    HttpResponseMessage response = await client.GetAsync(url);
                    return response;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"请求时发生异常: {ex.Message}");
                return null;
            }
        }

        public static async Task<string> GetTag_name()
        {
            HttpResponseMessage responseMsg = await GetReleaseInfoAsync();
            if (responseMsg == null) return ""; // fail to get response
            if (responseMsg.IsSuccessStatusCode)
            {
                // parse JSON
                string json = await responseMsg.Content.ReadAsStringAsync();
                dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                return data.tag_name;
            }
            else
            {
                Console.WriteLine($"请求当前版本失败，状态码: {responseMsg.StatusCode}");
                return "";
            }
        }

        public static async Task<string> GetUpdate_Info()
        {
            HttpResponseMessage responseMsg = await GetReleaseInfoAsync();
            if (responseMsg == null) return ""; // fail to get response
            if (responseMsg.IsSuccessStatusCode)
            {
                // parse JSON
                string json = await responseMsg.Content.ReadAsStringAsync();
                dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                return data.body;
            }
            else
            {
                Console.WriteLine($"请求更新信息失败，状态码: {responseMsg.StatusCode}");
                return "";
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
                Console.WriteLine($"请检查你的网络是否连接正常。如连接正常，请在 https://github.com/{owner}/{repo}/issues 提问。");
                return false;
            }
        }
    }
}
