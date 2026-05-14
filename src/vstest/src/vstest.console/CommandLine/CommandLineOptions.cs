// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;
using Microsoft.VisualStudio.TestPlatform.CommandLine.TestPlatformHelpers;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;

using vstest.console.Internal;

using CommandLineResources = Microsoft.VisualStudio.TestPlatform.CommandLine.Resources.Resources;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine;

/// <summary>
/// Provides access to the command-line options.
/// </summary>
internal class CommandLineOptions
{
    /// <summary>
    /// The default batch size for Run.
    /// </summary>
    public const long DefaultRunBatchSize = 10;

    /// <summary>
    /// The default batch size for Discovery.
    /// </summary>
    public const long DefaultDiscoveryBatchSize = 1000;

    /// <summary>
    /// The use vsix extensions key.
    /// </summary>
    public const string UseVsixExtensionsKey = "UseVsixExtensions";

    /// <summary>
    /// The default retrieval timeout for fetching of test results or test cases
    /// </summary>
    private readonly TimeSpan _defaultRetrievalTimeout = new(0, 0, 0, 1, 500);

    private static CommandLineOptions? s_instance;

    private List<string> _sources = new();

    private Architecture _architecture;

    private Framework? _frameworkVersion;

    /// <summary>
    /// Gets the instance.
    /// </summary>
    internal static CommandLineOptions Instance
        => s_instance ??= new CommandLineOptions();

    /// <summary>
    /// Default constructor.
    /// </summary>
    internal CommandLineOptions()
    {
        BatchSize = DefaultRunBatchSize;
        TestStatsEventTimeout = _defaultRetrievalTimeout;
        FileHelper = new FileHelper();
        FilePatternParser = new FilePatternParser();
#if TODO
        UseVsixExtensions = Utilities.GetAppSettingValue(UseVsixExtensionsKey, false);
#endif
    }

    /// <summary>
    /// Specifies whether parallel execution is on or off.
    /// </summary>
    public bool Parallel { get; set; }

    /// <summary>
    /// Specifies whether InIsolation is on or off.
    /// </summary>
    public bool InIsolation { get; set; }

    /// <summary>
    /// Read only collection of all available test sources
    /// </summary>
    public IEnumerable<string> Sources
    {
        get
        {
            return _sources.AsReadOnly();
        }
    }

    /// <summary>
    /// Specifies whether dynamic code coverage diagnostic data adapter needs to be configured.
    /// </summary>
    public bool EnableCodeCoverage { get; set; }

    /// <summary>
    /// Specifies whether the Fakes automatic configuration should be disabled.
    /// </summary>
    public bool DisableAutoFakes { get; set; }

    /// <summary>
    /// Specifies whether vsixExtensions is enabled or not.
    /// </summary>
    public bool UseVsixExtensions { get; set; }

    /// <summary>
    /// Path to the custom test adapters.
    /// </summary>
    public string[]? TestAdapterPath { get; set; }

    /// <summary>
    /// Test adapter loading strategy.
    /// </summary>
    public TestAdapterLoadingStrategy TestAdapterLoadingStrategy { get; set; }

    /// <summary>
    /// Process Id of the process which launched vstest runner
    /// </summary>
    public int ParentProcessId { get; set; }

    /// <summary>
    /// Port IDE process is listening to
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// Configuration the project is built for e.g. Debug/Release
    /// </summary>
    public string? Configuration { get; set; }

    /// <summary>
    /// Directory containing the temporary outputs
    /// </summary>
    public string? BuildBasePath { get; set; }

    /// <summary>
    /// Directory containing the binaries to run
    /// </summary>
    public string? Output { get; set; }

    /// <summary>
    /// Specifies the frequency of the runStats/discoveredTests event
    /// </summary>
    public long BatchSize { get; set; }

    /// <summary>
    /// Specifies the timeout of the test stats cache timeout event
    /// </summary>
    public TimeSpan TestStatsEventTimeout { get; set; }

    /// <summary>
    /// Test case filter value for run with sources.
    /// </summary>
    public string? TestCaseFilterValue { get; set; }

    /// <summary>
    /// Target Path used by ListFullyQualifiedTests option
    /// </summary>
    public string? ListTestsTargetPath { get; set; }

    /// <summary>
    /// Specifies the Target Device
    /// </summary>
    public string? TargetDevice { get; set; }

    /// <summary>
    /// Specifies whether the target device has a Windows Phone context or not
    /// </summary>
    public bool HasPhoneContext => !TargetDevice.IsNullOrEmpty();

    public bool TestAdapterPathsSet => (TestAdapterPath?.Length ?? 0) != 0;

    /// <summary>
    /// Specifies the target platform type for test run.
    /// </summary>
    public Architecture TargetArchitecture
    {
        get
        {
            return _architecture;
        }
        set
        {
            _architecture = value;
            ArchitectureSpecified = true;
        }
    }

    /// <summary>
    /// True indicates the test run is started from an Editor or IDE.
    /// Defaults to false.
    /// </summary>
    public bool IsDesignMode
    {
        get;
        set;
    }

    /// <summary>
    /// If not already set from IDE in the runSettings, ShouldCollectSourceInformation defaults to IsDesignMode value
    /// </summary>
    public bool ShouldCollectSourceInformation
    {
        get
        {
            return IsDesignMode;
        }
    }

    /// <summary>
    /// Specifies if /Platform has been specified on command line or not.
    /// </summary>
    internal bool ArchitectureSpecified { get; private set; }

    internal IFileHelper FileHelper { get; set; }

    internal FilePatternParser FilePatternParser { get; set; }

    /// <summary>
    /// Gets or sets the target Framework version for test run.
    /// </summary>
    internal Framework? TargetFrameworkVersion
    {
        get
        {
            return _frameworkVersion;
        }
        set
        {
            _frameworkVersion = value;
            FrameworkVersionSpecified = true;
        }
    }

    /// <summary>
    /// Gets a value indicating whether /Framework has been specified on command line or not.
    /// </summary>
    [MemberNotNullWhen(true, nameof(TargetFrameworkVersion))]
    internal bool FrameworkVersionSpecified { get; private set; }

    /// <summary>
    /// Gets or sets the results directory for test run.
    /// </summary>
    internal string? ResultsDirectory { get; set; }

    /// <summary>
    /// Gets or sets the /setting switch value. i.e path to settings file.
    /// </summary>
    internal string? SettingsFile { get; set; }

    /// <summary>
    /// Gets or sets the /ArtifactsProcessingMode value.
    /// </summary>
    internal ArtifactProcessingMode ArtifactProcessingMode { get; set; }

    /// <summary>
    /// Gets or sets the /TestSessionCorrelationId value.
    /// </summary>
    internal string? TestSessionCorrelationId { get; set; }

    /// <summary>
    /// Adds a source file to look for tests in.
    /// </summary>
    /// <param name="source">Path to source file to look for tests in.</param>
    public void AddSource(string source)
    {
        if (source.IsNullOrWhiteSpace())
        {
            throw new TestSourceException(CommandLineResources.CannotBeNullOrEmpty);
        }

        source = source.Trim();

        List<string> matchingFiles;
        try
        {
            // Get matching files from file pattern parser
            matchingFiles = FilePatternParser.GetMatchingFiles(source);
        }
        catch (TestSourceException ex) when (source.StartsWith("-") || source.StartsWith("/"))
        {
            throw new TestSourceException(
                string.Format(CultureInfo.CurrentCulture, CommandLineResources.InvalidArgument, source), ex);
        }
        // Add the matching files to source list
        var filteredFiles = KnownPlatformSourceFilter.FilterKnownPlatformSources(matchingFiles);
        _sources = _sources.Union(filteredFiles).ToList();
    }

    /// <summary>
    /// Resets the options. Clears the sources.
    /// </summary>
    internal static void Reset()
    {
        s_instance = null;
    }

}
