// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework.Utilities;
using Microsoft.VisualStudio.TestPlatform.Common.Filtering;
using Microsoft.VisualStudio.TestPlatform.Common.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Common.Logging;
using Microsoft.VisualStudio.TestPlatform.Common.Telemetry;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Tracing;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Tracing.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Utilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;

using CrossPlatEngineResources = Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Resources.Resources;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Discovery;

/// <summary>
/// Enumerates through all the discoverers.
/// </summary>
internal class DiscovererEnumerator
{
    private readonly DiscoveryResultCache _discoveryResultCache;
    private readonly ITestPlatformEventSource _testPlatformEventSource;
    private readonly IRequestData _requestData;
    private readonly IAssemblyProperties _assemblyProperties;
    private readonly CancellationToken _cancellationToken;

    /// <summary>
    /// Initializes a new instance of the <see cref="DiscovererEnumerator"/> class.
    /// </summary>
    /// <param name="requestData">The request data for providing discovery services and data.</param>
    /// <param name="discoveryResultCache"> The discovery result cache. </param>
    public DiscovererEnumerator(IRequestData requestData, DiscoveryResultCache discoveryResultCache, CancellationToken token)
        : this(requestData, discoveryResultCache, TestPlatformEventSource.Instance, token)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DiscovererEnumerator"/> class.
    /// </summary>
    /// <param name="requestData">The request data for providing discovery services and data.</param>
    /// <param name="discoveryResultCache"> The discovery result cache. </param>
    /// <param name="testPlatformEventSource">Telemetry events receiver</param>
    /// <param name="token">Cancellation Token to abort discovery</param>
    public DiscovererEnumerator(IRequestData requestData,
        DiscoveryResultCache discoveryResultCache,
        ITestPlatformEventSource testPlatformEventSource,
        CancellationToken token)
        : this(requestData, discoveryResultCache, testPlatformEventSource, new AssemblyProperties(), token)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DiscovererEnumerator"/> class.
    /// </summary>
    /// <param name="requestData">The request data for providing discovery services and data.</param>
    /// <param name="discoveryResultCache"> The discovery result cache. </param>
    /// <param name="testPlatformEventSource">Telemetry events receiver</param>
    /// <param name="assemblyProperties">Information on the assemblies being discovered</param>
    /// <param name="token">Cancellation Token to abort discovery</param>
    public DiscovererEnumerator(IRequestData requestData,
        DiscoveryResultCache discoveryResultCache,
        ITestPlatformEventSource testPlatformEventSource,
        IAssemblyProperties assemblyProperties,
        CancellationToken token)
    {
        // Added this to make class testable, needed a PEHeader mocked Instance
        _discoveryResultCache = discoveryResultCache;
        _testPlatformEventSource = testPlatformEventSource;
        _requestData = requestData;
        _assemblyProperties = assemblyProperties;
        _cancellationToken = token;
    }

    /// <summary>
    /// Discovers tests from the sources.
    /// </summary>
    /// <param name="testExtensionSourceMap"> The test extension source map. </param>
    /// <param name="settings"> The settings. </param>
    /// <param name="testCaseFilter"> The test case filter. </param>
    /// <param name="logger"> The logger. </param>
    public void LoadTests(IDictionary<string, IEnumerable<string>> testExtensionSourceMap, IRunSettings? settings, string? testCaseFilter, IMessageLogger logger)
    {
        _testPlatformEventSource.DiscoveryStart();
        try
        {
            foreach (var kvp in testExtensionSourceMap)
            {
                LoadTestsFromAnExtension(kvp.Key, kvp.Value, settings, testCaseFilter, logger);
            }
        }
        finally
        {
            _testPlatformEventSource.DiscoveryStop(_discoveryResultCache.TotalDiscoveredTests);
        }
    }

    /// <summary>
    /// Loads test cases from individual source.
    /// Discovery extensions update progress through ITestCaseDiscoverySink.
    /// Discovery extensions sends discovery messages through TestRunMessageLoggerProxy
    /// </summary>
    /// <param name="extensionAssembly"> The extension Assembly. </param>
    /// <param name="sources"> The sources.   </param>
    /// <param name="settings"> The settings.   </param>
    /// <param name="settings"> The test case filter. </param>
    /// <param name="logger"> The logger.  </param>
    private void LoadTestsFromAnExtension(string extensionAssembly, IEnumerable<string> sources, IRunSettings? settings, string? testCaseFilter, IMessageLogger logger)
    {
        // Stopwatch to collect metrics
        var timeStart = DateTime.UtcNow;

        var discovererToSourcesMap = GetDiscovererToSourcesMap(extensionAssembly, sources, logger, _assemblyProperties);
        var totalAdapterLoadTIme = DateTime.UtcNow - timeStart;

        // Collecting Data Point for TimeTaken to Load Adapters
        _requestData.MetricsCollection.Add(TelemetryDataConstants.TimeTakenToLoadAdaptersInSec, totalAdapterLoadTIme.TotalSeconds);

        // Warning is logged for in the inner function
        if (discovererToSourcesMap == null || !discovererToSourcesMap.Any())
        {
            return;
        }

        double totalTimeTakenByAdapters = 0;
        double totalAdaptersUsed = 0;
        try
        {
            // Collecting Total Number of Adapters Discovered in Machine
            _requestData.MetricsCollection.Add(TelemetryDataConstants.NumberOfAdapterDiscoveredDuringDiscovery, discovererToSourcesMap.Keys.Count);

            var context = new DiscoveryContext { RunSettings = settings };
            context.FilterExpressionWrapper = !StringUtils.IsNullOrEmpty(testCaseFilter) ? new FilterExpressionWrapper(testCaseFilter) : null;

            // Set on the logger the TreatAdapterErrorAsWarning setting from runsettings.
            SetAdapterLoggingSettings(logger, settings);

            var discoverySink = new TestCaseDiscoverySink(_discoveryResultCache);
            foreach (var discoverer in discovererToSourcesMap.Keys)
            {
                if (_cancellationToken.IsCancellationRequested)
                {
                    EqtTrace.Info("DiscovererEnumerator.LoadTestsFromAnExtension: Cancellation Requested. Aborting the discovery");
                    LogTestsDiscoveryCancellation(logger);
                    return;
                }

                DiscoverTestsFromSingleDiscoverer(discoverer, discovererToSourcesMap, context, discoverySink, logger, ref totalAdaptersUsed, ref totalTimeTakenByAdapters);
            }

            if (_discoveryResultCache.TotalDiscoveredTests == 0)
            {
                LogWarningOnNoTestsDiscovered(sources, testCaseFilter, logger);
            }
        }
        finally
        {
            CollectTelemetryAtEnd(totalTimeTakenByAdapters, totalAdaptersUsed);
        }
    }

    private static void LogTestsDiscoveryCancellation(IMessageLogger logger)
    {
        logger.SendMessage(TestMessageLevel.Warning, CrossPlatEngineResources.TestDiscoveryCancelled);
    }

    private void CollectTelemetryAtEnd(double totalTimeTakenByAdapters, double totalAdaptersUsed)
    {
        // Collecting Total Time Taken by Adapters
        _requestData.MetricsCollection.Add(TelemetryDataConstants.TimeTakenInSecByAllAdapters,
            totalTimeTakenByAdapters);

        // Collecting Total Adapters Used to Discover tests
        _requestData.MetricsCollection.Add(TelemetryDataConstants.NumberOfAdapterUsedToDiscoverTests,
            totalAdaptersUsed);
    }

    private void DiscoverTestsFromSingleDiscoverer(
        LazyExtension<ITestDiscoverer, ITestDiscovererCapabilities> discoverer,
        Dictionary<LazyExtension<ITestDiscoverer, ITestDiscovererCapabilities>, IEnumerable<string>> discovererToSourcesMap,
        DiscoveryContext context,
        TestCaseDiscoverySink discoverySink,
        IMessageLogger logger,
        ref double totalAdaptersUsed,
        ref double totalTimeTakenByAdapters)
    {
        if (!TryToLoadDiscoverer(discoverer, logger, out var discovererType))
        {
            // Fail to instantiate the discoverer type.
            return;
        }

        // on instantiated successfully, get tests
        try
        {
            EqtTrace.Verbose(
                "DiscovererEnumerator.DiscoverTestsFromSingleDiscoverer: Loading tests for {0}",
                discoverer.Value.GetType().FullName);

            if (discoverer.Metadata.DefaultExecutorUri == null)
            {
                throw new Exception($@"DefaultExecutorUri is null, did you decorate the discoverer class with [DefaultExecutorUri()] attribute? For example [DefaultExecutorUri(""executor://example.testadapter"")].");
            }

            var currentTotalTests = _discoveryResultCache.TotalDiscoveredTests;
            var newTimeStart = DateTime.UtcNow;

            _testPlatformEventSource.AdapterDiscoveryStart(discoverer.Metadata.DefaultExecutorUri.AbsoluteUri);
            discoverer.Value.DiscoverTests(discovererToSourcesMap[discoverer], context, logger, discoverySink);

            var totalAdapterRunTime = DateTime.UtcNow - newTimeStart;

            _testPlatformEventSource.AdapterDiscoveryStop(_discoveryResultCache.TotalDiscoveredTests -
                                                         currentTotalTests);

            // Record Total Tests Discovered By each Discoverer.
            var totalTestsDiscoveredByCurrentDiscoverer = _discoveryResultCache.TotalDiscoveredTests - currentTotalTests;
            _requestData.MetricsCollection.Add(
                $"{TelemetryDataConstants.TotalTestsByAdapter}.{discoverer.Metadata.DefaultExecutorUri}",
                totalTestsDiscoveredByCurrentDiscoverer);

            totalAdaptersUsed++;

            EqtTrace.Verbose("DiscovererEnumerator.DiscoverTestsFromSingleDiscoverer: Done loading tests for {0}",
                discoverer.Value.GetType().FullName);

            var discovererFromDeprecatedLocations = IsDiscovererFromDeprecatedLocations(discoverer);
            if (discovererFromDeprecatedLocations)
            {
                logger.SendMessage(TestMessageLevel.Warning,
                    string.Format(CultureInfo.CurrentCulture, CrossPlatEngineResources.DeprecatedAdapterPath));
            }

            // Collecting Data Point for Time Taken to Discover Tests by each Adapter
            _requestData.MetricsCollection.Add(
                $"{TelemetryDataConstants.TimeTakenToDiscoverTestsByAnAdapter}.{discoverer.Metadata.DefaultExecutorUri}",
                totalAdapterRunTime.TotalSeconds);
            totalTimeTakenByAdapters += totalAdapterRunTime.TotalSeconds;
        }
        catch (Exception e)
        {
            var message = string.Format(
                CultureInfo.CurrentCulture,
                CrossPlatEngineResources.ExceptionFromLoadTests,
                discovererType.Name,
                e.Message);

            logger.SendMessage(TestMessageLevel.Error, message);
            EqtTrace.Error("DiscovererEnumerator.DiscoverTestsFromSingleDiscoverer: {0} ", e);
        }
    }

    private static bool TryToLoadDiscoverer(LazyExtension<ITestDiscoverer, ITestDiscovererCapabilities> discoverer, IMessageLogger logger, [NotNullWhen(true)] out Type? discovererType)
    {
        discovererType = null;

        // See if discoverer can be instantiated successfully else move next.
        try
        {
            discovererType = discoverer.Value.GetType();
        }
        catch (Exception e)
        {
            var mesage = string.Format(CultureInfo.CurrentCulture, CrossPlatEngineResources.DiscovererInstantiationException, e.Message);
            logger.SendMessage(TestMessageLevel.Warning, mesage);
            EqtTrace.Error("DiscovererEnumerator.LoadTestsFromAnExtension: {0} ", e);

            return false;
        }

        return true;
    }

    private static bool IsDiscovererFromDeprecatedLocations(
        LazyExtension<ITestDiscoverer, ITestDiscovererCapabilities> discoverer)
    {
        TPDebug.Assert(discoverer.Metadata.DefaultExecutorUri is not null, "discoverer.Metadata.DefaultExecutorUri is null");
        if (Constants.DefaultAdapters.Contains(discoverer.Metadata.DefaultExecutorUri.ToString(), StringComparer.OrdinalIgnoreCase))
        {
            return false;
        }

        var discovererLocation = discoverer.Value.GetType().GetTypeInfo().Assembly.GetAssemblyLocation();

        return Path.GetDirectoryName(discovererLocation)!
            .Equals(Constants.DefaultAdapterLocation, StringComparison.OrdinalIgnoreCase);
    }

    private static void LogWarningOnNoTestsDiscovered(IEnumerable<string> sources, string? testCaseFilter, IMessageLogger logger)
    {
        var sourcesString = string.Join(" ", sources);

        // Print warning on no tests.
        if (!testCaseFilter.IsNullOrEmpty())
        {
            var testCaseFilterToShow = TestCaseFilterDeterminer.ShortenTestCaseFilterIfRequired(testCaseFilter);

            logger.SendMessage(
                TestMessageLevel.Warning,
                string.Format(CultureInfo.CurrentCulture, CrossPlatEngineResources.NoTestsAvailableForGivenTestCaseFilter, testCaseFilterToShow, sourcesString));
        }
        else
        {
            logger.SendMessage(
                TestMessageLevel.Warning,
                string.Format(CultureInfo.CurrentCulture, CrossPlatEngineResources.TestRunFailed_NoDiscovererFound_NoTestsAreAvailableInTheSources, sourcesString));
        }
    }

    private static void SetAdapterLoggingSettings(IMessageLogger messageLogger, IRunSettings? runSettings)
    {
        if (messageLogger is TestSessionMessageLogger discoveryMessageLogger && runSettings != null)
        {
#if Todo
                // TODO : Enable this when RunSettings is enabled.
                IRunConfigurationSettingsProvider runConfigurationSettingsProvider =
                        (IRunConfigurationSettingsProvider)runSettings.GetSettings(ObjectModel.Constants.RunConfigurationSettingsName);
                if (runConfigurationSettingsProvider != null
                        && runConfigurationSettingsProvider.Settings != null)
                {
                    discoveryMessageLogger.TreatTestAdapterErrorsAsWarnings
                                = runConfigurationSettingsProvider.Settings.TreatTestAdapterErrorsAsWarnings;
                }
#endif
        }
    }

    /// <summary>
    /// Get the discoverers matching with the parameter sources
    /// </summary>
    /// <param name="extensionAssembly"> The extension assembly. </param>
    /// <param name="sources"> The sources. </param>
    /// <param name="logger"> The logger instance. </param>
    /// <returns> The map between an extension type and a source. </returns>
    internal static Dictionary<LazyExtension<ITestDiscoverer, ITestDiscovererCapabilities>, IEnumerable<string>>? GetDiscovererToSourcesMap(
        string extensionAssembly,
        IEnumerable<string> sources,
        IMessageLogger logger,
        IAssemblyProperties assemblyProperties)
    {
        var allDiscoverers = GetDiscoverers(extensionAssembly, throwOnError: true);

        if (allDiscoverers == null || !allDiscoverers.Any())
        {
            var sourcesString = string.Join(" ", sources);
            // No discoverer available, log a warning
            logger.SendMessage(
                TestMessageLevel.Warning,
                string.Format(CultureInfo.CurrentCulture, CrossPlatEngineResources.TestRunFailed_NoDiscovererFound_NoTestsAreAvailableInTheSources, sourcesString));

            return null;
        }

        IDictionary<AssemblyType, IList<string>>? assemblyTypeToSoucesMap = null;
        var result = new Dictionary<LazyExtension<ITestDiscoverer, ITestDiscovererCapabilities>, IEnumerable<string>>();
        var sourcesForWhichNoDiscovererIsAvailable = new List<string>(sources);

        sources = sources.Distinct().ToList();
        IEnumerable<string> allDirectoryBasedSources = sources.Where(Directory.Exists).ToList();
        IEnumerable<string> allFileBasedSources = sources.Except(allDirectoryBasedSources).ToList();

        foreach (var discoverer in allDiscoverers)
        {
            var applicableFileBasedSources = allFileBasedSources;
            if (discoverer.Metadata.AssemblyType is AssemblyType.Native or AssemblyType.Managed)
            {
                assemblyTypeToSoucesMap ??= GetAssemblyTypeToSoucesMap(applicableFileBasedSources, assemblyProperties);
                applicableFileBasedSources = assemblyTypeToSoucesMap[AssemblyType.None].Concat(assemblyTypeToSoucesMap[discoverer.Metadata.AssemblyType]);
            }

            // Find the sources which this discoverer can look at.
            var matchingSources = Enumerable.Empty<string>();
            var discovererFileExtensions = discoverer.Metadata.FileExtension;
            var discovererIsApplicableToFiles = discovererFileExtensions is not null;
            var discovererIsApplicableToDirectories = discoverer.Metadata.IsDirectoryBased;

            if (!discovererIsApplicableToFiles && !discovererIsApplicableToDirectories)
            {
                // Discoverer is applicable for all sources (regardless of whether they are files or directories).
                // Include all files and directories.
                matchingSources = applicableFileBasedSources.Concat(allDirectoryBasedSources);
            }
            else
            {
                if (discovererIsApplicableToFiles)
                {
                    // Include matching files.
                    var matchingFileBasedSources =
                        applicableFileBasedSources.Where(source =>
                            discovererFileExtensions!.Contains(
                                Path.GetExtension(source),
                                StringComparer.OrdinalIgnoreCase));

                    matchingSources = matchingSources.Concat(matchingFileBasedSources);
                }

                if (discovererIsApplicableToDirectories)
                {
                    // Include all directories.
                    matchingSources = matchingSources.Concat(allDirectoryBasedSources);
                }
            }

            matchingSources = matchingSources.ToList(); // ToList is required to actually execute the query

            // Update the source list for which no matching source is available.
            if (matchingSources.Any())
            {
                sourcesForWhichNoDiscovererIsAvailable =
                    sourcesForWhichNoDiscovererIsAvailable
                        .Except(matchingSources, StringComparer.OrdinalIgnoreCase)
                        .ToList();

                result.Add(discoverer, matchingSources);
            }
        }

        if (EqtTrace.IsWarningEnabled && sourcesForWhichNoDiscovererIsAvailable != null)
        {
            foreach (var source in sourcesForWhichNoDiscovererIsAvailable)
            {
                // Log a warning to log file, not to the "default logger for discovery time messages".
                EqtTrace.Warning(
                    "No test discoverer is registered to perform discovery for the type of test source '{0}'. Register a test discoverer for this source type and try again.",
                    source);
            }
        }

        return result;
    }

    /// <summary>
    /// Get assembly type to sources map.
    /// </summary>
    /// <param name="sources">Sources.</param>
    /// <param name="assemblyType">Assembly type.</param>
    /// <returns>Sources with matching assembly type.</returns>
    private static IDictionary<AssemblyType, IList<string>> GetAssemblyTypeToSoucesMap(IEnumerable<string> sources, IAssemblyProperties assemblyProperties)
    {
        var assemblyTypeToSoucesMap = new Dictionary<AssemblyType, IList<string>>()
        {
            { AssemblyType.Native, new List<string>()},
            { AssemblyType.Managed, new List<string>()},
            { AssemblyType.None, new List<string>()}
        };

        if (sources != null && sources.Any())
        {
            foreach (string source in sources)
            {
                var sourcesList = IsAssembly(source) ?
                    assemblyTypeToSoucesMap[assemblyProperties.GetAssemblyType(source)] :
                    assemblyTypeToSoucesMap[AssemblyType.None];

                sourcesList.Add(source);
            }
        }

        return assemblyTypeToSoucesMap;
    }

    /// <summary>
    /// Finds if a file is an assembly or not.
    /// </summary>
    /// <param name="filePath">File path.</param>
    /// <returns>True if file is an assembly.</returns>
    private static bool IsAssembly(string filePath)
    {
        var fileExtension = Path.GetExtension(filePath);

        return ".dll".Equals(fileExtension, StringComparison.OrdinalIgnoreCase) ||
               ".exe".Equals(fileExtension, StringComparison.OrdinalIgnoreCase);
    }

    private static IEnumerable<LazyExtension<ITestDiscoverer, ITestDiscovererCapabilities>>? GetDiscoverers(
        string extensionAssembly,
        bool throwOnError)
    {
        try
        {
            if (StringUtils.IsNullOrEmpty(extensionAssembly) || string.Equals(extensionAssembly, ObjectModel.Constants.UnspecifiedAdapterPath))
            {
                // full discovery.
                return TestDiscoveryExtensionManager.Create().Discoverers;
            }
            else
            {
                return TestDiscoveryExtensionManager.GetDiscoveryExtensionManager(extensionAssembly).Discoverers;
            }
        }
        catch (Exception ex)
        {
            EqtTrace.Error($"TestDiscoveryManager: LoadExtensions: Exception occurred while loading extensions {ex}");
            if (throwOnError)
            {
                throw;
            }

            return null;
        }
    }
}
