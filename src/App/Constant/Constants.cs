using System.Runtime.InteropServices;
using System.Security.Principal;

namespace Launcher.App.Constant
{
    internal static class Constants
    {
        /// <summary>
        /// Debug mode flag
        /// </summary>
#if DEBUG
        public const bool DBG = true;
#else
        public const bool DBG = false;
#endif

        /// <summary>
        /// Administrator flag
        /// </summary>
        public static bool ADMIN { get; } = new WindowsPrincipal(WindowsIdentity.GetCurrent()!).IsInRole(WindowsBuiltInRole.Administrator);

        /// <summary>
        /// System architecture (can only be one of x64 x86 arm64)
        /// </summary>
        public static string ARCHITECTURE { get; } = RuntimeInformation.ProcessArchitecture.ToString().ToLower();

        /// <summary>
        /// GitHub repo owner
        /// </summary>
        public const string OWNER = "TheMagicFlute";

        /// <summary>
        /// GitHub repo name
        /// </summary>
        public const string REPO = "Launcher_V2";

        /// <summary>
        /// License for this project
        /// </summary>
        public const string LICENSE = "NO LICENSE";

        /// <summary>
        /// GitHub repo url
        /// </summary>
        public const string GH_REPO_URL = $"https://github.com/{OWNER}/{REPO}";

        /// <summary>
        /// GitHub latest release
        /// </summary>
        public const string GH_LATEST_RELEASE = $"{GH_REPO_URL}/releases/latest";

        /// <summary>
        /// GitHub issues tracker
        /// </summary>
        public const string GH_ISSUE_URL = $"https://github.com/yanygm/Launcher_V2/issues";

        /// <summary>
        /// Docs url
        /// </summary>
        public const string DOCS_URL = "https://themagicflute.github.io/Launcher_V2/";

        /// <summary>
        /// Url of KartInfo forum thread
        /// </summary>
        public const string KARTINFO_URL = "https://kartinfo.me/thread-9369-1-1.html";

        /// <summary>
        /// Url of KartRider Game Archive
        /// </summary>
        public const string KRARCHIVE_URL = "https://github.com/brownsugar/popkart-client-archive/releases";

        /// <summary>
        /// shop page url in game
        /// </summary>
        public const string SHOP_PAGE = GH_LATEST_RELEASE;

        /// <summary>
        /// ending banner url after close the game
        /// </summary>
        public const string ENDING_BANNER = GH_REPO_URL;

        /// <summary>
        /// Program version (e.g. 251001)
        /// </summary>
        public static string VERSION { get; } =
#if DEBUG
            DateTime.Now.ToString("yyMMdd");
#else
            ThisAssembly.Git.CommitDate.Substring(0, 10).Replace("-", "").Substring(2, 6);
#endif
    }
}
