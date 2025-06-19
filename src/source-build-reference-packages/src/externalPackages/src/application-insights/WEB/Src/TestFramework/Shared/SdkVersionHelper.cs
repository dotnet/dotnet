namespace Microsoft.ApplicationInsights.TestFramework
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    using System;
#if NETCOREAPP
    using System.Collections.Generic;
#endif
    using System.Linq;
    using System.Reflection;

    public static class SdkVersionHelper
    {
        public static string GetExpectedSdkVersion(Type assemblyType, string prefix)
        {
#if NETCOREAPP
            IEnumerable<Attribute> assemblyCustomAttributes = assemblyType.GetTypeInfo().Assembly.GetCustomAttributes();
#else
            object[] assemblyCustomAttributes = assemblyType.Assembly.GetCustomAttributes(false);
#endif
            string versionStr = assemblyCustomAttributes
                .OfType<AssemblyFileVersionAttribute>()
                .First()
                .Version;
            string[] versionParts = new Version(versionStr).ToString().Split('.');

            var expected = prefix + string.Join(".", versionParts[0], versionParts[1], versionParts[2]) + "-" + versionParts[3];

            return expected;
        }
    }
}
