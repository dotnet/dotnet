namespace Microsoft.DotNet.Build.Tasks.Workloads.Tests
{
    // This class is generated.
    public class WixToolsetInfo
    {
        public const string WiXExt = "wixext5";
        public static readonly string WixExePath = @"{WixExePath}";

        // Pick up the net472 copy since that still includes Heat.exe and we can wrap that in a ToolTask.
        public static readonly string HeatExePath = @"{HeatExePath}";

        public const string ArcadeVersion = "{ArcadeVersion}";
    }
}
