namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Security;
    using System.Threading;

    using Microsoft.ApplicationInsights.Common.Extensions;

    using static System.FormattableString;

    /// <summary>
    /// This class contains helper methods that can be shared across file loggers.
    /// </summary>
    internal static class FileHelper
    {
        private static string identityName = null;

        public static string IdentityName => LazyInitializer.EnsureInitialized(ref identityName, GetCurrentIdentityName);

        /// <summary>
        /// Test that this process can read and write to a given directory.
        /// </summary>
        /// <param name="directory">The directory to be evaluated.</param>
        public static void TestDirectoryPermissions(DirectoryInfo directory)
        {
            string testFileName = Path.GetRandomFileName();
            string testFilePath = Path.Combine(directory.FullName, testFileName);

            if (!Directory.Exists(directory.FullName))
            {
                Directory.CreateDirectory(directory.FullName);
            }

            // Create a test file, will auto-delete this file. 
            using (var testFile = new FileStream(testFilePath, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None, 4096, FileOptions.DeleteOnClose))
            {
                testFile.Write(new[] { default(byte) }, 0, 1);
            }
        }

        /// <summary>
        /// Generate a file name in the format "ApplicationInsightsLog_20200101_120000_w3wp_12345.txt.
        /// </summary>
        /// <remarks>
        /// File logging can be controlled by an Environment Variable and may affect multiple running applications on a single machine.
        /// We include the Date, TimeStamp, Process Name, and Process ID to uniquely identify applications.
        /// </remarks>
        /// <returns>File name for use in logging.</returns>
        public static string GenerateFileName()
        {
            var process = Process.GetCurrentProcess();
            return Invariant($"ApplicationInsightsLog_{DateTime.UtcNow.ToInvariantString("yyyyMMdd_HHmmss")}_{process.ProcessName}_{process.Id}.txt");
        }

        /// <summary>
        /// Get the current identity.
        /// </summary>
        private static string GetCurrentIdentityName()
        {
            try
            {
#if NETSTANDARD // This constant is defined for all versions of NetStandard https://docs.microsoft.com/en-us/dotnet/core/tutorials/libraries#how-to-multitarget
                return System.Environment.UserName;
#else
                using (var identity = System.Security.Principal.WindowsIdentity.GetCurrent())
                {
                    return identity.Name;
                }
#endif
            }
            catch (SecurityException exp)
            {
                CoreEventSource.Log.LogWindowsIdentityAccessSecurityException(exp.Message);
                return "Unknown";
            }
        }
    }
}