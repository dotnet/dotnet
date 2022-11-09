using System;
using Xunit;

namespace Microsoft.DotNet.Tools.Tests.Utilities
{
    public class WindowsOnlyFactAttribute : FactAttribute
    {
        public WindowsOnlyFactAttribute()
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                Skip = "Not running on Windows";
            }
        }
    }
}
