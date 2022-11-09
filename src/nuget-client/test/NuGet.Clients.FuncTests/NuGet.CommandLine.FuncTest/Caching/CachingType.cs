using System;

namespace NuGet.CommandLine.Test.Caching
{
    [Flags]
    public enum CachingType
    {
        Default = 0,
        NoCache = 1,
        DirectDownload = 2
    }
}
