// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.VisualStudio.TestPlatform.Client.Discovery;
using Microsoft.VisualStudio.TestPlatform.Client.Execution;
using Microsoft.VisualStudio.TestPlatform.Common;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using Microsoft.VisualStudio.TestPlatform.Common.Hosting;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Host;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;

using ClientResources = Microsoft.VisualStudio.TestPlatform.Client.Resources.Resources;

namespace Microsoft.VisualStudio.TestPlatform.Client;

/// <summary>
/// Implementation for TestPlatform.
/// </summary>
internal class TestPlatform : ITestPlatform
{
    private readonly ITestRuntimeProviderManager _testHostProviderManager;

    private readonly IFileHelper _fileHelper;

    static TestPlatform()
    {
        // TODO: This is not the right way to force initialization of default extensions.
        // Test runtime providers require this today. They're getting initialized even before
        // test adapter paths are provided, which is incorrect.
        AddExtensionAssembliesFromExtensionDirectory();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestPlatform"/> class.
    /// </summary>
    public TestPlatform()
        : this(
            new TestEngine(),
            new FileHelper(),
            TestRuntimeProviderManager.Instance)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestPlatform"/> class.
    /// </summary>
    ///
    /// <param name="testEngine">The test engine.</param>
    /// <param name="filehelper">The file helper.</param>
    /// <param name="testHostProviderManager">The data.</param>
    protected internal TestPlatform(
        ITestEngine testEngine,
        IFileHelper filehelper,
        ITestRuntimeProviderManager testHostProviderManager)
    {
        _testEngine = testEngine;
        _fileHelper = filehelper;
        _testHostProviderManager = testHostProviderManager;
    }

    private readonly ITestEngine _testEngine;

    /// <inheritdoc/>
    public IDiscoveryRequest CreateDiscoveryRequest(
        IRequestData requestData,
        DiscoveryCriteria discoveryCriteria,
        TestPlatformOptions? options,
        Dictionary<string, SourceDetail> sourceToSourceDetailMap,
        IWarningLogger warningLogger)
    {
        ValidateArg.NotNull(discoveryCriteria, nameof(discoveryCriteria));

        PopulateExtensions(discoveryCriteria.RunSettings, discoveryCriteria.Sources);

        // Initialize loggers.
        ITestLoggerManager loggerManager = _testEngine.GetLoggerManager(requestData);
        loggerManager.Initialize(discoveryCriteria.RunSettings);

        IProxyDiscoveryManager discoveryManager = _testEngine.GetDiscoveryManager(requestData, discoveryCriteria, sourceToSourceDetailMap, warningLogger);
        discoveryManager.Initialize(options?.SkipDefaultAdapters ?? false);

        return new DiscoveryRequest(requestData, discoveryCriteria, discoveryManager, loggerManager);
    }

    /// <inheritdoc/>
    public ITestRunRequest CreateTestRunRequest(
        IRequestData requestData,
        TestRunCriteria testRunCriteria,
        TestPlatformOptions? options,
        Dictionary<string, SourceDetail> sourceToSourceDetailMap,
        IWarningLogger warningLogger)
    {
        ValidateArg.NotNull(testRunCriteria, nameof(testRunCriteria));

        IEnumerable<string> sources = GetSources(testRunCriteria);
        PopulateExtensions(testRunCriteria.TestRunSettings, sources);

        // Initialize loggers.
        ITestLoggerManager loggerManager = _testEngine.GetLoggerManager(requestData);
        loggerManager.Initialize(testRunCriteria.TestRunSettings);

        IProxyExecutionManager executionManager = _testEngine.GetExecutionManager(requestData, testRunCriteria, sourceToSourceDetailMap, warningLogger);
        executionManager.Initialize(options?.SkipDefaultAdapters ?? false);

        return new TestRunRequest(requestData, testRunCriteria, executionManager, loggerManager);
    }

    /// <inheritdoc/>
    public bool StartTestSession(
        IRequestData requestData,
        StartTestSessionCriteria testSessionCriteria,
        ITestSessionEventsHandler eventsHandler,
        Dictionary<string, SourceDetail> sourceToSourceDetailMap,
        IWarningLogger warningLogger)
    {
        ValidateArg.NotNull(testSessionCriteria, nameof(testSessionCriteria));

        RunConfiguration runConfiguration = XmlRunSettingsUtilities.GetRunConfigurationNode(testSessionCriteria.RunSettings);
        TestAdapterLoadingStrategy strategy = runConfiguration.TestAdapterLoadingStrategy;

        AddExtensionAssemblies(testSessionCriteria.RunSettings, strategy);

        if (!runConfiguration.DesignMode)
        {
            return false;
        }

        IProxyTestSessionManager? testSessionManager = _testEngine.GetTestSessionManager(requestData, testSessionCriteria, sourceToSourceDetailMap, warningLogger);
        if (testSessionManager == null)
        {
            // The test session manager is null because the combination of runsettings and
            // sources tells us we should run in-process (i.e. in vstest.console). Because
            // of this no session will be created because there's no testhost to be launched.
            // Expecting a subsequent call to execute tests with the same set of parameters.
            eventsHandler.HandleStartTestSessionComplete(new());
            return false;
        }

        return testSessionManager.StartSession(eventsHandler, requestData);
    }

    private void PopulateExtensions(string? runSettings, IEnumerable<string> sources)
    {
        RunConfiguration runConfiguration = XmlRunSettingsUtilities.GetRunConfigurationNode(runSettings);
        TestAdapterLoadingStrategy strategy = runConfiguration.TestAdapterLoadingStrategy;

        // Update cache with Extension folder's files.
        AddExtensionAssemblies(runSettings, strategy);

        // Update extension assemblies from source when design mode is false.
        if (!runConfiguration.DesignMode)
        {
            AddLoggerAssembliesFromSource(sources, strategy);
        }
    }

    /// <summary>
    /// The dispose.
    /// </summary>
    public void Dispose()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public void UpdateExtensions(
        IEnumerable<string>? pathToAdditionalExtensions,
        bool skipExtensionFilters)
    {
        _testEngine.GetExtensionManager().UseAdditionalExtensions(pathToAdditionalExtensions, skipExtensionFilters);
    }

    /// <inheritdoc/>
    public void ClearExtensions()
    {
        _testEngine.GetExtensionManager().ClearExtensions();
    }

    private static void ThrowExceptionIfTestHostManagerIsNull(
        ITestRuntimeProvider? testHostManager,
        string settingsXml)
    {
        if (testHostManager == null)
        {
            EqtTrace.Error($"{nameof(TestPlatform)}.{nameof(ThrowExceptionIfTestHostManagerIsNull)}: No suitable testHostProvider found for runsettings: {settingsXml}");
            throw new TestPlatformException(string.Format(CultureInfo.CurrentCulture, ClientResources.NoTestHostProviderFound));
        }
    }


    private void AddExtensionAssemblies(string? runSettings, TestAdapterLoadingStrategy adapterLoadingStrategy)
    {
        IEnumerable<string> customTestAdaptersPaths = RunSettingsUtilities.GetTestAdaptersPaths(runSettings);

        if (customTestAdaptersPaths == null)
        {
            return;
        }

        foreach (string customTestAdaptersPath in customTestAdaptersPaths)
        {
            IEnumerable<string> extensionAssemblies = ExpandTestAdapterPaths(customTestAdaptersPath, _fileHelper, adapterLoadingStrategy);

            if (extensionAssemblies.Any())
            {
                UpdateExtensions(extensionAssemblies, skipExtensionFilters: false);
            }

        }
    }

    /// <summary>
    /// Updates the test logger paths from source directory.
    /// </summary>
    ///
    /// <param name="sources">The list of sources.</param>
    private void AddLoggerAssembliesFromSource(IEnumerable<string> sources, TestAdapterLoadingStrategy strategy)
    {
        // Skip discovery unless we're using the default behavior, or NextToSource is specified.
        if (strategy != TestAdapterLoadingStrategy.Default && !strategy.HasFlag(TestAdapterLoadingStrategy.NextToSource))
        {
            return;
        }

        // Currently we support discovering loggers only from Source directory.
        List<string> loggersToUpdate = new();

        foreach (string source in sources)
        {
            var sourceDirectory = Path.GetDirectoryName(source);
            if (!string.IsNullOrEmpty(sourceDirectory) && _fileHelper.DirectoryExists(sourceDirectory))
            {
                SearchOption searchOption = GetSearchOption(strategy, SearchOption.TopDirectoryOnly);

                loggersToUpdate.AddRange(
                    _fileHelper.EnumerateFiles(
                        sourceDirectory,
                        searchOption,
                        TestPlatformConstants.TestLoggerEndsWithPattern));
            }
        }

        if (loggersToUpdate.Count > 0)
        {
            UpdateExtensions(loggersToUpdate, skipExtensionFilters: false);
        }
    }

    /// <summary>
    /// Finds all test platform extensions from the `.\Extensions` directory. This is used to
    /// load the inbox extensions like TrxLogger and legacy test extensions like MSTest v1,
    /// MSTest C++, etc..
    /// </summary>
    private static void AddExtensionAssembliesFromExtensionDirectory()
    {
        // This method needs to run statically before we have any adapter discovery.
        // TestHostProviderManager get initialized just after this call and it
        // requires DefaultExtensionPaths to be set to resolve a TestHostProvider.
        // Since it's static, it forces us to set the adapter paths.
        //
        // Otherwise we will always get a "No suitable test runtime provider found for this run." error.
        // I (@haplois) will modify this behavior later on, but we also need to consider legacy adapters
        // and make sure they still work after modification.
        string? runSettings = RunSettingsManager.Instance.ActiveRunSettings.SettingsXml;
        RunConfiguration runConfiguration = XmlRunSettingsUtilities.GetRunConfigurationNode(runSettings);
        TestAdapterLoadingStrategy strategy = runConfiguration.TestAdapterLoadingStrategy;

        FileHelper fileHelper = new();
        IEnumerable<string> defaultExtensionPaths = Enumerable.Empty<string>();

        // Explicit adapter loading
        if (strategy.HasFlag(TestAdapterLoadingStrategy.Explicit))
        {
            defaultExtensionPaths = RunSettingsUtilities.GetTestAdaptersPaths(runSettings)
                .SelectMany(path => ExpandTestAdapterPaths(path, fileHelper, strategy))
                .Union(defaultExtensionPaths);
        }

        string extensionsFolder = Path.Combine(
            Path.GetDirectoryName(typeof(TestPlatform).GetTypeInfo().Assembly.GetAssemblyLocation())!,
            "Extensions");
        if (!fileHelper.DirectoryExists(extensionsFolder))
        {
            // TODO: Since we no-longer run from <playground>\vstest.console\vstest.conosle.exe in Playground, the relative
            // extensions folder location changed and we need to patch it. This should be a TEMPORARY solution though, we
            // should come up with a better way of fixing this.
            // NOTE: This is specific to Playground which references vstest.console from a location that doesn't contain
            // the Extensions folder. Normal projects shouldn't have this issue.
            extensionsFolder = Path.Combine(Path.GetDirectoryName(extensionsFolder)!, "vstest.console", "Extensions");
        }

        if (fileHelper.DirectoryExists(extensionsFolder))
        {
            // Load default runtime providers
            if (strategy.HasFlag(TestAdapterLoadingStrategy.DefaultRuntimeProviders))
            {
                defaultExtensionPaths = fileHelper
                    .EnumerateFiles(extensionsFolder, SearchOption.TopDirectoryOnly, TestPlatformConstants.RunTimeEndsWithPattern)
                    .Union(defaultExtensionPaths);
            }

            // Default extension loader
            if (strategy == TestAdapterLoadingStrategy.Default || strategy.HasFlag(TestAdapterLoadingStrategy.ExtensionsDirectory))
            {
                defaultExtensionPaths = fileHelper
                    .EnumerateFiles(extensionsFolder, SearchOption.TopDirectoryOnly, ".dll", ".exe")
                    .Union(defaultExtensionPaths);
            }
        }

        TestPluginCache.Instance.DefaultExtensionPaths = defaultExtensionPaths.Distinct();
    }

    private static SearchOption GetSearchOption(TestAdapterLoadingStrategy strategy, SearchOption defaultStrategyOption)
    {
        return strategy == TestAdapterLoadingStrategy.Default
            ? defaultStrategyOption
            : strategy.HasFlag(TestAdapterLoadingStrategy.Recursive) ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
    }

    private static IEnumerable<string> ExpandTestAdapterPaths(string path, IFileHelper fileHelper, TestAdapterLoadingStrategy strategy)
    {
        string adapterPath = Path.GetFullPath(Environment.ExpandEnvironmentVariables(path));

        // Default behavior is to only accept directories.
        if (strategy == TestAdapterLoadingStrategy.Default)
        {
            return ExpandAdaptersWithDefaultStrategy(adapterPath, fileHelper);
        }

        IEnumerable<string> adapters = ExpandAdaptersWithExplicitStrategy(adapterPath, fileHelper, strategy);

        return adapters.Distinct();
    }

    private static IEnumerable<string> ExpandAdaptersWithExplicitStrategy(string path, IFileHelper fileHelper, TestAdapterLoadingStrategy strategy)
    {
        if (!strategy.HasFlag(TestAdapterLoadingStrategy.Explicit))
        {
            return Enumerable.Empty<string>();
        }

        if (fileHelper.Exists(path))
        {
            return new[] { path };
        }
        else if (fileHelper.DirectoryExists(path))
        {
            SearchOption searchOption = GetSearchOption(strategy, SearchOption.TopDirectoryOnly);

            IEnumerable<string> adapterPaths = fileHelper.EnumerateFiles(
                path,
                searchOption,
                TestPlatformConstants.TestAdapterEndsWithPattern,
                TestPlatformConstants.TestLoggerEndsWithPattern,
                TestPlatformConstants.DataCollectorEndsWithPattern,
                TestPlatformConstants.RunTimeEndsWithPattern);

            return adapterPaths;
        }

        EqtTrace.Warning($"{nameof(TestPlatform)}.{nameof(ExpandAdaptersWithExplicitStrategy)} AdapterPath Not Found: {path}");
        return Enumerable.Empty<string>();
    }

    private static IEnumerable<string> ExpandAdaptersWithDefaultStrategy(string path, IFileHelper fileHelper)
    {
        // This is the legacy behavior, please do not modify this method unless you're sure of
        // side effect when running tests with legacy adapters.
        if (!fileHelper.DirectoryExists(path))
        {
            EqtTrace.Warning($"{nameof(TestPlatform)}.{nameof(ExpandAdaptersWithDefaultStrategy)} AdapterPath Not Found: {path}");

            return Enumerable.Empty<string>();
        }

        return fileHelper.EnumerateFiles(
                path,
                SearchOption.AllDirectories,
                TestPlatformConstants.TestAdapterEndsWithPattern,
                TestPlatformConstants.TestLoggerEndsWithPattern,
                TestPlatformConstants.DataCollectorEndsWithPattern,
                TestPlatformConstants.RunTimeEndsWithPattern);
    }

    private static IEnumerable<string> GetSources(TestRunCriteria testRunCriteria)
    {
        if (testRunCriteria.HasSpecificTests)
        {
            // If the test execution is with a test filter, filter sources too.
            return testRunCriteria.Tests.Select(tc => tc.Source).Distinct();
        }

        TPDebug.Assert(testRunCriteria.Sources is not null, "testRunCriteria.Sources is null");
        return testRunCriteria.Sources;
    }
}
