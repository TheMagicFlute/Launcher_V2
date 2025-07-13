using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KartRider
{
    internal static class Update
    {
        public static string currentVersion = GetCurrentVersion();

        public static string owner = "TheMagicFlute"; // GitHub Repo Owner
        public static string repo = "Launcher_V2"; // GitHub Repo Name

        public static async Task<bool> UpdateDataAsync()
        {
            Console.WriteLine("正在检查更新...");
            Console.WriteLine("当前Branch: {0}/{1}", ThisAssembly.Git.RepositoryUrl, ThisAssembly.Git.Branch);
            Console.WriteLine("当前Commit: {0}", ThisAssembly.Git.Commit);
            Console.WriteLine("当前Commit SHA: {0}", ThisAssembly.Git.Sha);
            Console.WriteLine("当前Commit日期: {0}", ThisAssembly.Git.CommitDate);
            Console.WriteLine("当前Tag: {0}", ThisAssembly.Git.BaseTag);

            string tag_name = await GetTag_name();
            string update_info = await GetUpdate_Info();
            Console.WriteLine($"当前版本为: {currentVersion}");
            if (tag_name != "" && int.Parse(currentVersion) < int.Parse(tag_name))
            {
                // 询问是否需要更新
                Console.WriteLine($"发现新版本: {tag_name}");
                Console.WriteLine($"更新信息: \n{update_info}");
                Console.Write("请问是否需要更新? (Y/n): ");
                string usrInput = null;
                while (usrInput != "y" && usrInput != "Y" && usrInput != "n" && usrInput != "N")
                {
                    usrInput = Console.ReadLine();
                    if (usrInput == "n" || usrInput == "N")
                    {
                        return false; // cancel update
                    }
                    else if (usrInput != "y" && usrInput != "Y")
                    {
                        // usrInput is not valid, ask again
                        Console.Write("请输入 (Y for yes / n for no): ");
                    }
                }
                // 尝试下载最新的版本
                Console.WriteLine($"正在下载 {tag_name}...");
                try
                {
                    string country = await GetCountryAsync();
                    string url = $"https://github.com/{owner}/{repo}/releases/download/" + tag_name + "/Launcher.zip";
                    // 中国大陆需要使用代理下载，处理 url
                    if (country != "" && country == "CN")
                    {
                        List<string> urls = new List<string>() { "https://ghproxy.net/", "https://gh-proxy.com/", "https://hub.myany.uk/", "http://kra.myany.uk:2233/", "http://krb.myany.uk:2233/" };
                        Console.WriteLine("Using proxy.");
                        foreach (string url_ in urls)
                        {
                            if (url_ == "https://ghproxy.net/" || url_ == "https://hub.myany.uk/")
                            {
                                url = url_ + url;
                            }
                            else
                            {
                                url = url_ + url.Replace("https://", "");
                            }
                            if (await GetUrl(url))
                            {
                                return await DownloadUpdate(url);
                                break;
                            }
                        }
                    }
                    else
                    {
                        return await DownloadUpdate(url);
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"下载过程中出现错误: {ex.Message}");
                    Console.WriteLine($"如果仍想更新，请重新启动本程序，或者访问 https://github.com/{owner}/{repo}/releases/latest 手动下载新版本");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("当前已是最新版本。");
                return false;
            }
        }

        public static string GetCompileDate()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            AssemblyName assemblyName = assembly.GetName();
            string simpleName = assemblyName.Name + ".exe";
            DateTime compilationDate = File.GetLastWriteTime(AppDomain.CurrentDomain.BaseDirectory + simpleName);
            string formattedDate = compilationDate.ToString("yyMMdd");
            return formattedDate;
        }

        public static string GetCurrentDate()
        {
            return DateTime.Now.ToString("yyMMdd");
        }

        public static string GetCurrentVersion()
        {
#if DEBUG
            return GetCurrentDate();
#else
            return ThisAssembly.Git.BaseTag;
            // return ThisAssembly.Git.BaseTag ?? ThisAssembly.Git.CommitDate.Substring(0, 10).Replace("-", "").Substring(2, 6);
#endif
        }

        public static async Task<bool> DownloadUpdate(string UpdatePackageUrl)
        {
            try
            {
                Console.WriteLine($"开始下载更新包: {UpdatePackageUrl}");
                using (HttpClient client = new HttpClient())
                {
                    using (HttpResponseMessage response = await client.GetAsync(UpdatePackageUrl, HttpCompletionOption.ResponseHeadersRead))
                    {
                        response.EnsureSuccessStatusCode();
                        using (Stream contentStream = await response.Content.ReadAsStreamAsync())
                        {
                            string folderPath = AppDomain.CurrentDomain.BaseDirectory + "Update";
                            if (!Directory.Exists(folderPath))
                            {
                                Directory.CreateDirectory(folderPath);
                            }
                            long? totalBytes = response.Content.Headers.ContentLength;
                            using (FileStream fileStream = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "Update\\Launcher.zip", FileMode.Create, FileAccess.Write, FileShare.None))
                            {
                                byte[] buffer = new byte[4096];
                                int bytesRead;
                                long totalRead = 0;
                                while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                                {
                                    await fileStream.WriteAsync(buffer, 0, bytesRead);
                                    totalRead += bytesRead;
                                    double progress = totalBytes.HasValue ? (double)totalRead / totalBytes.Value * 100 : 0;
                                    Console.Write($"\r下载进度: {progress:F2}%");
                                }
                            }
                        }
                    }
                    return ApplyUpdate();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"下载过程中出现错误: {ex.Message}");
                Console.WriteLine($"如果仍想更新，请重新启动本程序，或者访问 https://github.com/{owner}/{repo}/releases/latest 手动下载新版本");
                return false;
            }
        }

        public static bool ApplyUpdate()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            AssemblyName assemblyName = assembly.GetName();
            string simpleName = assemblyName.Name + ".exe";
            try
            {
                System.IO.Compression.ZipFile.ExtractToDirectory(AppDomain.CurrentDomain.BaseDirectory + "Update\\Launcher.zip", AppDomain.CurrentDomain.BaseDirectory + "Update\\");
                string script = @$"@echo off
timeout /t 3 /nobreak
move {"\"" + AppDomain.CurrentDomain.BaseDirectory + "Update\\" + simpleName + "\""} {"\"" + AppDomain.CurrentDomain.BaseDirectory + "\""}
start {"\"\" \"" + AppDomain.CurrentDomain.BaseDirectory + simpleName + "\""}
";
                string filePath = AppDomain.CurrentDomain.BaseDirectory + "Update.bat";
                try
                {
                    File.WriteAllText(filePath, script);
                    Console.WriteLine("\n写入文件成功。");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n写入文件时出错: {ex.Message}");
                }
                Process.Start(filePath);
                Process.GetCurrentProcess().Kill();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n应用更新时出错: {ex.Message}");
                Console.WriteLine($"如果仍想更新，请重新启动本程序，或者访问 https://github.com/{owner}/{repo}/releases/latest 手动下载新版本");
                return false;
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
                        Console.WriteLine($"请求IP地址失败，状态码: {response.StatusCode}");
                        return "";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生异常: {ex.Message}");
                return "";
            }
        }

        public static async Task<HttpResponseMessage> GetReleaseInfoAsync()
        {
            string url = $"https://api.github.com/repos/{owner}/{repo}/releases/latest";
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

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
