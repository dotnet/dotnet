// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Xunit;

namespace Microsoft.DotNet.XUnitExtensions
{
    internal static class DiscovererHelpers
    {
        private static readonly Lazy<bool> s_isMonoRuntime = new Lazy<bool>(() => Type.GetType("Mono.RuntimeStructs") != null);
        public static bool IsMonoRuntime => s_isMonoRuntime.Value;
        public static bool IsRunningOnNetCoreApp { get; } = (Environment.Version.Major >= 5 || !RuntimeInformation.FrameworkDescription.StartsWith(".NET Framework", StringComparison.OrdinalIgnoreCase));
        public static bool IsRunningOnNetFramework { get; } = RuntimeInformation.FrameworkDescription.StartsWith(".NET Framework", StringComparison.OrdinalIgnoreCase);

        public static bool TestPlatformApplies(TestPlatforms platforms) =>
                (platforms.HasFlag(TestPlatforms.Any)) ||
                (platforms.HasFlag(TestPlatforms.FreeBSD) && RuntimeInformation.IsOSPlatform(OSPlatform.Create("FREEBSD"))) ||
                (platforms.HasFlag(TestPlatforms.Linux) && RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) ||
                (platforms.HasFlag(TestPlatforms.NetBSD) && RuntimeInformation.IsOSPlatform(OSPlatform.Create("NETBSD"))) ||
                (platforms.HasFlag(TestPlatforms.OSX) && RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) ||
                (platforms.HasFlag(TestPlatforms.illumos) && RuntimeInformation.IsOSPlatform(OSPlatform.Create("ILLUMOS"))) ||
                (platforms.HasFlag(TestPlatforms.Solaris) && RuntimeInformation.IsOSPlatform(OSPlatform.Create("SOLARIS"))) ||
                (platforms.HasFlag(TestPlatforms.iOS) && RuntimeInformation.IsOSPlatform(OSPlatform.Create("IOS")) && !RuntimeInformation.IsOSPlatform(OSPlatform.Create("MACCATALYST"))) ||
                (platforms.HasFlag(TestPlatforms.tvOS) && RuntimeInformation.IsOSPlatform(OSPlatform.Create("TVOS"))) ||
                (platforms.HasFlag(TestPlatforms.MacCatalyst) && RuntimeInformation.IsOSPlatform(OSPlatform.Create("MACCATALYST"))) ||
                (platforms.HasFlag(TestPlatforms.LinuxBionic) && RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("ANDROID_STORAGE"))) ||
                (platforms.HasFlag(TestPlatforms.Android) && RuntimeInformation.IsOSPlatform(OSPlatform.Create("ANDROID"))) ||
                (platforms.HasFlag(TestPlatforms.Browser) && RuntimeInformation.IsOSPlatform(OSPlatform.Create("BROWSER"))) ||
                (platforms.HasFlag(TestPlatforms.Wasi) && RuntimeInformation.IsOSPlatform(OSPlatform.Create("WASI"))) ||
                (platforms.HasFlag(TestPlatforms.Haiku) && RuntimeInformation.IsOSPlatform(OSPlatform.Create("HAIKU"))) ||
                (platforms.HasFlag(TestPlatforms.Windows) && RuntimeInformation.IsOSPlatform(OSPlatform.Windows));

        public static bool TestRuntimeApplies(TestRuntimes runtimes) =>
                (runtimes.HasFlag(TestRuntimes.Mono) && IsMonoRuntime) ||
                (runtimes.HasFlag(TestRuntimes.CoreCLR) && !IsMonoRuntime); // assume CoreCLR if it's not Mono

        public static bool TestFrameworkApplies(TargetFrameworkMonikers frameworks) =>
                (frameworks.HasFlag(TargetFrameworkMonikers.Netcoreapp) && IsRunningOnNetCoreApp) ||
                (frameworks.HasFlag(TargetFrameworkMonikers.NetFramework) && IsRunningOnNetFramework);

        internal static bool Evaluate(Type calleeType, string[] conditionMemberNames)
        {
            foreach (string entry in conditionMemberNames)
            {
                // Null condition member names are silently tolerated.
                if (string.IsNullOrWhiteSpace(entry)) continue;

                Func<bool> conditionFunc = ConditionalTestDiscoverer.LookupConditionalMember(calleeType, entry);
                if (conditionFunc == null)
                {
                    throw new InvalidOperationException($"Unable to get member, please check input for {entry}.");
                }

                if (!conditionFunc()) return false;
            }

            return true;
        }

        internal static IEnumerable<KeyValuePair<string, string>> EvaluateArguments(IEnumerable<object> ctorArgs,string category, int skipFirst=1)
        {
            Debug.Assert(ctorArgs.Count() >= 2);

            TestPlatforms platforms = TestPlatforms.Any;
            TargetFrameworkMonikers frameworks = TargetFrameworkMonikers.Any;
            TestRuntimes runtimes = TestRuntimes.Any;
            Type calleeType = null;
            string[] conditionMemberNames = null;

            foreach (object arg in ctorArgs.Skip(skipFirst)) // First argument is the issue number or reason.
            {
                if (arg is TestPlatforms)
                {
                    platforms = (TestPlatforms)arg;
                }
                else if (arg is TargetFrameworkMonikers)
                {
                    frameworks = (TargetFrameworkMonikers)arg;
                }
                else if (arg is TestRuntimes)
                {
                    runtimes = (TestRuntimes)arg;
                }
                else if (arg is Type)
                {
                    calleeType = (Type)arg;
                }
                else if (arg is string[])
                {
                    conditionMemberNames = (string[])arg;
                }
            }

            if (calleeType != null && conditionMemberNames != null)
            {
                if (DiscovererHelpers.Evaluate(calleeType, conditionMemberNames))
                {
                    yield return new KeyValuePair<string, string>(XunitConstants.Category, category);
                }
            }
            else if (DiscovererHelpers.TestPlatformApplies(platforms) &&
                DiscovererHelpers.TestRuntimeApplies(runtimes) &&
                DiscovererHelpers.TestFrameworkApplies(frameworks))
            {
                yield return new KeyValuePair<string, string>(XunitConstants.Category, category);
            }
        }

        internal static string AppendAdditionalMessage(this string message, string additionalMessage)
            => !string.IsNullOrWhiteSpace(additionalMessage) ? $"{message} {additionalMessage}" : message;
    }
}
