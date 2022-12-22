using System;
using System.IO;
using System.Linq;
using NuGet.Common;

namespace NuGet.Test.Utility
{
    public static class XunitAttributeUtility
    {
        public static string GetFileExistsInDirSkipMessageOrNull(bool allowSkipOnCI, string directory, params string[] paths)
        {
            if (!IsCI || allowSkipOnCI)
            {
                foreach (var path in paths)
                {
                    try
                    {
                        var fullPath = Path.Combine(directory, path);

                        // Skip if a file does not exist, otherwise run the test.
                        if (!File.Exists(fullPath))
                        {
                            return $"Required file does not exist: '{fullPath}'.";
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Returns a message to apply to the xunit attribute if it should be skipped.
        /// Null is returned if the test should run.
        /// </summary>
        public static string GetFileExistsSkipMessageOrNull(bool allowSkipOnCI, params string[] paths)
        {
            if (!IsCI || allowSkipOnCI)
            {
                foreach (var path in paths)
                {
                    try
                    {
                        // Skip if a file does not exist, otherwise run the test.
                        if (!File.Exists(path))
                        {
                            return $"Required file does not exist: '{path}'.";
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Returns a message to apply to the xunit attribute if it should be skipped.
        /// Null is returned if the test should run.
        /// </summary>
        public static string GetPlatformSkipMessageOrNull(params string[] platforms)
        {
            var current = CurrentPlatform;

            var runTest = platforms.Any(s => StringComparer.OrdinalIgnoreCase.Equals(current, s));

            if (!runTest)
            {
                var plural = platforms.Length == 1 ? "" : "s";

                return $"Test does not apply to: {current}. Target platform{plural}: {String.Join(", ", platforms)}";
            }

            return null;
        }

        /// <summary>
        /// Current platform, windows, darwin, linux
        /// </summary>
        public static string CurrentPlatform
        {
            get
            {
                return _currentPlatform.Value;
            }
        }

        /// <summary>
        /// CI env var value.
        /// </summary>
        public static bool IsCI
        {
            get
            {
                return _isCI.Value;
            }
        }

        public static string GetMonoMessage(bool onlyOnMono, bool skipMono)
        {
            if (onlyOnMono && !RuntimeEnvironmentHelper.IsMono)
            {
                return "This test only runs on mono.";
            }

            if (skipMono && RuntimeEnvironmentHelper.IsMono)
            {
                return "This test does not run on mono.";
            }

            return null;
        }

        private static readonly Lazy<bool> _isCI = new Lazy<bool>(GetCIVar);

        private static bool GetCIVar()
        {
            var val = Environment.GetEnvironmentVariable("CI");

            if (!string.IsNullOrEmpty(val) && bool.TryParse(val, out var b) && b)
            {
                return true;
            }

            return false;
        }

        private static readonly Lazy<string> _currentPlatform = new Lazy<string>(GetCurrentPlatform);

        private static string GetCurrentPlatform()
        {
            if (RuntimeEnvironmentHelper.IsWindows)
            {
                return Platform.Windows;
            }

            if (RuntimeEnvironmentHelper.IsLinux)
            {
                return Platform.Linux;
            }

            if (RuntimeEnvironmentHelper.IsMacOSX)
            {
                return Platform.Darwin;
            }

            return "UNKNOWN";
        }
    }
}
