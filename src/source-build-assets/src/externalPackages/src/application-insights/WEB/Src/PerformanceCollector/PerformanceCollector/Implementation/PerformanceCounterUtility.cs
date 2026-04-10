namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.StandardPerfCollector;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.WebAppPerfCollector;
#if NETSTANDARD2_0
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.XPlatform;
#endif

    /// <summary>
    /// Utility functionality for performance counter collection.
    /// </summary>
    [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "This class has different code for Net452/NetCore")]
    internal static class PerformanceCounterUtility
    {
#if NETSTANDARD2_0
        public static bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
#endif
        // Internal for testing
        internal static bool? isAzureWebApp = null;

        private const string Win32ProcessInstancePlaceholder = @"APP_WIN32_PROC";
        private const string ClrProcessInstancePlaceholder = @"APP_CLR_PROC";
        private const string W3SvcProcessInstancePlaceholder = @"APP_W3SVC_PROC";

        private const string Win32ProcessCategoryName = "Process";
        private const string ClrProcessCategoryName = ".NET CLR Memory";
        private const string Win32ProcessCounterName = "ID Process";
        private const string ClrProcessCounterName = "Process ID";
#if NETSTANDARD2_0
        private const string StandardSdkVersionPrefix = "pccore:";
#else
        private const string StandardSdkVersionPrefix = "pc:";
#endif
        private const string AzureWebAppSdkVersionPrefix = "azwapc:";
        private const string AzureWebAppCoreSdkVersionPrefix = "azwapccore:";

        private const string WebSiteEnvironmentVariable = "WEBSITE_SITE_NAME";
        private const string WebSiteIsolationEnvironmentVariable = "WEBSITE_ISOLATION";
        private const string WebSiteIsolationHyperV = "hyperv";

        private static readonly ConcurrentDictionary<string, Tuple<DateTime, PerformanceCounterCategory, InstanceDataCollectionCollection>> cache = new ConcurrentDictionary<string, Tuple<DateTime, PerformanceCounterCategory, InstanceDataCollectionCollection>>();
        private static readonly ConcurrentDictionary<string, string> PlaceholderCache =
            new ConcurrentDictionary<string, string>();

        private static readonly Regex InstancePlaceholderRegex = new Regex(
            @"^\?\?(?<placeholder>[a-zA-Z0-9_]+)\?\?$",
            RegexOptions.Compiled);

        private static readonly Regex PerformanceCounterRegex =
            new Regex(
                @"^\\(?<categoryName>[^(]+)(\((?<instanceName>[^)]+)\)){0,1}\\(?<counterName>[\s\S]+)$",
                RegexOptions.Compiled);

        /// <summary>
        /// Formats a counter into a readable string.
        /// </summary>
        public static string FormatPerformanceCounter(PerformanceCounter pc)
        {
            return FormatPerformanceCounter(pc.CategoryName, pc.CounterName, pc.InstanceName);
        }

        public static bool IsPerfCounterSupported()
        {
            return true;
        }

#if NET452
        public static IPerformanceCollector GetPerformanceCollector()
        {
            IPerformanceCollector collector;
            if (PerformanceCounterUtility.IsWebAppRunningInAzure())
            {
                collector = (IPerformanceCollector)new WebAppPerfCollector.WebAppPerformanceCollector();
                PerformanceCollectorEventSource.Log.InitializedWithCollector(collector.GetType().Name);
            }
            else
            {
                collector = (IPerformanceCollector)new StandardPerformanceCollector();
                PerformanceCollectorEventSource.Log.InitializedWithCollector(collector.GetType().Name);
            }

            return collector;
        }
#elif NETSTANDARD2_0
        public static IPerformanceCollector GetPerformanceCollector()
        {   
            IPerformanceCollector collector;
            if (PerformanceCounterUtility.IsWebAppRunningInAzure())
            {
                if (PerformanceCounterUtility.IsWindows)
                {
                    // WebApp For windows
                    collector = (IPerformanceCollector)new WebAppPerformanceCollector();
                    PerformanceCollectorEventSource.Log.InitializedWithCollector(collector.GetType().Name);
                }
                else
                {
                    // We are in WebApp, but not Windows. Use XPlatformPerfCollector.
                    collector = (IPerformanceCollector)new PerformanceCollectorXPlatform();
                    PerformanceCollectorEventSource.Log.InitializedWithCollector(collector.GetType().Name);
                }
            }
            else if (PerformanceCounterUtility.IsWindows)
            {
                // The original Windows PerformanceCounter collector which is also
                // supported in NetStandard2.0 in Windows.
                collector = (IPerformanceCollector)new StandardPerformanceCollector();
                PerformanceCollectorEventSource.Log.InitializedWithCollector(collector.GetType().Name);
            }
            else
            {
                // This is NetStandard2.0 and non-windows. Use XPlatformPerfCollector
                collector = (IPerformanceCollector)new PerformanceCollectorXPlatform();
                PerformanceCollectorEventSource.Log.InitializedWithCollector(collector.GetType().Name);
            }

            return collector;
        }
#endif

        /// <summary>
        /// Formats a counter into a readable string.
        /// </summary>
        /// <param name="pc">Performance counter structure.</param>
        public static string FormatPerformanceCounter(PerformanceCounterStructure pc)
        {
            return FormatPerformanceCounter(pc.CategoryName, pc.CounterName, pc.InstanceName);
        }

        /// <summary>
        /// Searches for the environment variable specific to Azure Web App.
        /// </summary>
        /// <returns>Boolean, which is true if the current application is an Azure Web App.</returns>
        public static bool IsWebAppRunningInAzure()
        {
            if (!isAzureWebApp.HasValue)
            {
                try
                {
                    // Presence of "WEBSITE_SITE_NAME" indicate web apps.
                    // "WEBSITE_ISOLATION"!="hyperv" indicate premium containers. In this case, perf counters
                    // can be read using regular mechanism and hence this method retuns false for
                    // premium containers.
                    isAzureWebApp = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(WebSiteEnvironmentVariable)) &&
                                    Environment.GetEnvironmentVariable(WebSiteIsolationEnvironmentVariable) != WebSiteIsolationHyperV;
                }
                catch (Exception ex)
                {
                    PerformanceCollectorEventSource.Log.AccessingEnvironmentVariableFailedWarning(WebSiteEnvironmentVariable, ex.ToString());
                    return false;
                }
            }

            return (bool)isAzureWebApp;
        }

        /// <summary>
        /// Gets the processor count from the appropriate environment variable depending on whether the app is a WebApp or not.
        /// </summary>
        /// <returns>The number of processors in the system or null if failed to determine.</returns>
        public static int? GetProcessorCount()
        {
            int count;
            try
            {
                count = Environment.ProcessorCount;
            }
            catch (Exception ex)
            {
                PerformanceCollectorEventSource.Log.ProcessorsCountIncorrectValueError(ex.ToString());
                return null;
            }

            if (count < 1 || count > 1000)
            {
                PerformanceCollectorEventSource.Log.ProcessorsCountIncorrectValueError(count.ToString(CultureInfo.InvariantCulture));
                return null;
            }

            return count;
        }

        /// <summary>
        /// Differentiates the SDK version prefix for azure web applications with standard applications.
        /// </summary>
        /// <returns>Returns the SDK version prefix based on the platform.</returns>
        public static string SDKVersionPrefix()
        {
            if (IsWebAppRunningInAzure())
            {
#if NETSTANDARD2_0
                return AzureWebAppCoreSdkVersionPrefix;
#else
                return AzureWebAppSdkVersionPrefix;
#endif
            }
            else
            {
                return StandardSdkVersionPrefix;
            }
        }

        /// <summary>
        /// Formats a counter into a readable string.
        /// </summary>
        public static string FormatPerformanceCounter(string categoryName, string counterName, string instanceName)
        {
            if (string.IsNullOrWhiteSpace(instanceName))
            {
                return string.Format(CultureInfo.InvariantCulture, @"\{0}\{1}", categoryName, counterName);
            }

            return string.Format(
                CultureInfo.InvariantCulture,
                @"\{0}({2})\{1}",
                categoryName,
                counterName,
                instanceName);
        }

        /// <summary>
        /// Validates the counter by parsing.
        /// </summary>
        /// <param name="perfCounterName">Performance counter name to validate.</param>
        /// <param name="win32Instances">Windows 32 instances.</param>
        /// <param name="clrInstances">CLR instances.</param>
        /// <param name="supportInstanceNames">Boolean indicating if InstanceNames are supported. For WebApp and XPlatform counters, counters are always read from own process instance.</param>
        /// <param name="usesInstanceNamePlaceholder">Boolean to check if it is using an instance name place holder.</param>
        /// <param name="error">Error message.</param>
        /// <returns>Performance counter.</returns>
        public static PerformanceCounterStructure CreateAndValidateCounter(
            string perfCounterName,
            IEnumerable<string> win32Instances,
            IEnumerable<string> clrInstances,
            bool supportInstanceNames,
            out bool usesInstanceNamePlaceholder,
            out string error)
        {
            error = null;

            try
            {
                return PerformanceCounterUtility.ParsePerformanceCounter(
                    perfCounterName,
                    win32Instances,
                    clrInstances,
                    supportInstanceNames,
                    out usesInstanceNamePlaceholder);
            }
            catch (Exception e)
            {
                usesInstanceNamePlaceholder = false;
                PerformanceCollectorEventSource.Log.CounterParsingFailedEvent(e.Message, perfCounterName);
                error = e.Message;

                return null;
            }
        }

        /// <summary>
        /// Parses a performance counter canonical string into a PerformanceCounter object.
        /// </summary>
        /// <remarks>This method also performs placeholder expansion.</remarks>
        public static PerformanceCounterStructure ParsePerformanceCounter(
            string performanceCounter,
            IEnumerable<string> win32Instances,
            IEnumerable<string> clrInstances,
            bool supportInstanceNames,
            out bool usesInstanceNamePlaceholder)
        {
            var match = PerformanceCounterRegex.Match(performanceCounter);

            if (!match.Success)
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        @"Invalid performance counter name format: {0}. Expected formats are \category(instance)\counter or \category\counter",
                        performanceCounter),
                    nameof(performanceCounter));
            }

            return new PerformanceCounterStructure()
            {
                CategoryName = match.Groups["categoryName"].Value,
                InstanceName =
                    ExpandInstanceName(
                        match.Groups["instanceName"].Value,
                        win32Instances,
                        clrInstances,
                        supportInstanceNames,
                        out usesInstanceNamePlaceholder),
                CounterName = match.Groups["counterName"].Value,
            };
        }

        /// <summary>
        /// Invalidates placeholder cache.
        /// </summary>
        public static void InvalidatePlaceholderCache()
        {
            PlaceholderCache.Clear();
        }

        /// <summary>
        /// Matches an instance name against the placeholder regex.
        /// </summary>
        /// <param name="instanceName">Instance name to match.</param>
        /// <returns>Regex match.</returns>
        public static Match MatchInstancePlaceholder(string instanceName)
        {
            var match = InstancePlaceholderRegex.Match(instanceName);

            if (!match.Success || !match.Groups["placeholder"].Success)
            {
                return null;
            }

            return match;
        }

        internal static string GetInstanceForCurrentW3SvcWorker()
        {
#if NETSTANDARD2_0
            string name = new AssemblyName(Assembly.GetEntryAssembly().FullName).Name;
#else
            string name = AppDomain.CurrentDomain.FriendlyName;
#endif

            return GetInstanceFromApplicationDomain(name);
        }

        internal static string GetInstanceFromApplicationDomain(string domainFriendlyName)
        {
            const string Separator = "-";

            string[] segments = domainFriendlyName.Split(Separator.ToCharArray());

            var nameWithoutTrailingData = string.Join(
                Separator,
                segments.Take(segments.Length > 2 ? segments.Length - 2 : segments.Length));

            return nameWithoutTrailingData.Replace('/', '_');
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "This method has different code for Net452/NetCore")]
        internal static string GetInstanceForWin32Process(IEnumerable<string> win32Instances)
        {
            return FindProcessInstance(
                Process.GetCurrentProcess().Id,
                win32Instances,
                Win32ProcessCategoryName,
                Win32ProcessCounterName);
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "This method has different code for Net452/NetCore")]
        internal static string GetInstanceForClrProcess(IEnumerable<string> clrInstances)
        {
            return FindProcessInstance(
                Process.GetCurrentProcess().Id,
                clrInstances,
                ClrProcessCategoryName,
                ClrProcessCounterName);
        }

        internal static IList<string> GetWin32ProcessInstances()
        {
            return GetInstances(Win32ProcessCategoryName);
        }

        internal static IList<string> GetClrProcessInstances()
        {
            return GetInstances(ClrProcessCategoryName);
        }

        private static string ExpandInstanceName(
            string instanceName,
            IEnumerable<string> win32Instances,
            IEnumerable<string> clrInstances,
            bool supportInstanceNames,
            out bool usesPlaceholder)
        {
            if (!supportInstanceNames)
            {
                usesPlaceholder = false;
                return instanceName;
            }

            var match = MatchInstancePlaceholder(instanceName);
            if (match == null)
            {
                // not a placeholder, do not expand
                usesPlaceholder = false;
                return instanceName;
            }

            usesPlaceholder = true;

            var placeholder = match.Groups["placeholder"].Value;

            // use a cached value if available
            string cachedResult;
            if (PlaceholderCache.TryGetValue(placeholder, out cachedResult))
            {
                return cachedResult;
            }

            // expand
            if (string.Equals(placeholder, Win32ProcessInstancePlaceholder, StringComparison.OrdinalIgnoreCase))
            {
                cachedResult = GetInstanceForWin32Process(win32Instances);
            }
            else if (string.Equals(placeholder, ClrProcessInstancePlaceholder, StringComparison.OrdinalIgnoreCase))
            {
                cachedResult = GetInstanceForClrProcess(clrInstances);
            }
            else if (string.Equals(placeholder, W3SvcProcessInstancePlaceholder, StringComparison.OrdinalIgnoreCase))
            {
                cachedResult = GetInstanceForCurrentW3SvcWorker();
            }
            else
            {
                // a non-supported placeholder, return as is
                return instanceName;
            }

            // add to cache
            PlaceholderCache[placeholder] = cachedResult;

            return cachedResult;
        }

        private static string FindProcessInstance(int pid, IEnumerable<string> instances, string categoryName, string counterName)
        {
            Tuple<DateTime, PerformanceCounterCategory, InstanceDataCollectionCollection> cached;

            DateTime utcNow = DateTime.UtcNow;

            InstanceDataCollectionCollection result = null;

            PerformanceCounterCategory category = null;

            if (cache.TryGetValue(categoryName, out cached))
            {
                category = cached.Item2;

                if (cached.Item1 < utcNow)
                {
                    result = cached.Item3;
                }
            }

            if (result == null)
            {
                if (category == null)
                {
                    category = new PerformanceCounterCategory(categoryName);
                }

                result = category.ReadCategory();

                cache.TryAdd(categoryName, new Tuple<DateTime, PerformanceCounterCategory, InstanceDataCollectionCollection>(utcNow.AddMinutes(1), category, result));
            }

            InstanceDataCollection counters = result[counterName];

            if (counters != null)
            {
                foreach (string i in instances)
                {
                    InstanceData instance = counters[i];

                    if ((instance != null) && (pid == instance.RawValue))
                    {
                        return i;
                    }
                }
            }

            return null;
        }

        private static IList<string> GetInstances(string categoryName)
        {
            var cat = new PerformanceCounterCategory() { CategoryName = categoryName };

            try
            {
                return cat.GetInstanceNames();
            }
            catch (Exception)
            {
                // something went wrong and the category hasn't been found
                // we can't perform this operation
#if NETSTANDARD2_0
                return Array.Empty<string>();
#else
                return new string[] { };
#endif
            }
        }
    }
}