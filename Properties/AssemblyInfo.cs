using System.Reflection;
using System.Runtime.InteropServices;
using KartRider;
using Launcher.KartRider.Data;

// General Information about an assembly is controlled through the following
[assembly: ComVisible(false)]
[assembly: AssemblyCopyright("Yany, LAON")]
[assembly: AssemblyTrademark("Yany, LAON")]
[assembly: AssemblyTitle("中国跑跑卡丁车单机启动器")]
[assembly: AssemblyProduct("Launcher")]
[assembly: AssemblyDescription("中国跑跑卡丁车单机启动器")]
#if DEBUG
[assembly: AssemblyConfiguration("debug")]
#elif RELEASE
[assembly: AssemblyConfiguration("release")]
#endif