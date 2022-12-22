// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;

/// <summary>
/// Attribute to be used for displaying the help content in required order.
///
/// Order of settings display (based on lifecycle at https://blogs.msdn.microsoft.com/visualstudioalm/2016/07/25/evolving-the-visual-studio-test-platform-part-1/).
///
/// Selection
/// --Tests
/// --TestCaseFilter
///
/// Configure
/// --Framework
/// --Platform
/// --Settings
/// --CLI runsettings
///
/// Run/Discover
/// --ListTests
/// --Parallel
/// --TestAdapterPath
/// --TestAdapterLoadingStrategy
///
/// Diagnose/Report
/// --Diag
/// --Logger
/// --ResultsDirectory
///
/// IDE Automation
/// --ParentProcessId
/// --Port
///
/// Help
/// --Help
/// </summary>
internal enum HelpContentPriority
{
    /// <summary>
    /// No Content to be shown
    /// </summary>
    None,

    /// <summary>
    /// RunTestsArgumentProcessor Help
    /// </summary>
    RunTestsArgumentProcessorHelpPriority,

    /// <summary>
    /// RunSpecificTestsArgumentProcessor Help
    /// </summary>
    RunSpecificTestsArgumentProcessorHelpPriority,

    /// <summary>
    /// TestCaseFilterArgumentProcessor Help
    /// </summary>
    TestCaseFilterArgumentProcessorHelpPriority,

    /// <summary>
    /// FrameworkArgumentProcessor Help
    /// </summary>
    FrameworkArgumentProcessorHelpPriority,

    /// <summary>
    /// PlatformArgumentProcessor Help
    /// </summary>
    PlatformArgumentProcessorHelpPriority,

    /// <summary>
    /// EnvironmentArgumentProcessor Help
    /// </summary>
    EnvironmentArgumentProcessorHelpPriority,

    /// <summary>
    /// RunSettingsArgumentProcessor Help
    /// </summary>
    RunSettingsArgumentProcessorHelpPriority,

    /// <summary>
    /// CLIRunSettingsArgumentProcessor Help
    /// </summary>
    CliRunSettingsArgumentProcessorHelpPriority,

    /// <summary>
    /// ListTestsArgumentExecutor Help
    /// </summary>
    ListTestsArgumentProcessorHelpPriority,

    /// <summary>
    /// ParallelArgumentProcessor Help
    /// </summary>
    ParallelArgumentProcessorHelpPriority,

    /// <summary>
    /// TestAdapterPathArgumentProcessor Help
    /// </summary>
    TestAdapterPathArgumentProcessorHelpPriority,

    /// <summary>
    /// TestAdapterLoadingStrategyArgumentProcessor Help
    /// </summary>
    TestAdapterLoadingStrategyArgumentProcessorHelpPriority,

    /// <summary>
    /// EnableDiagArgumentProcessor Help
    /// </summary>
    EnableDiagArgumentProcessorHelpPriority,

    /// <summary>
    /// EnableLoggerArgumentProcessor Help
    /// </summary>
    EnableLoggerArgumentProcessorHelpPriority,

    /// <summary>
    /// ResultsDirectoryArgumentProcessor Help
    /// </summary>
    ResultsDirectoryArgumentProcessorHelpPriority,

    /// <summary>
    /// PortArgumentProcessor Help
    /// </summary>
    ParentProcessIdArgumentProcessorHelpPriority,

    /// <summary>
    /// PortArgumentProcessor Help
    /// </summary>
    PortArgumentProcessorHelpPriority,

    /// <summary>
    /// HelpArgumentExecutor
    /// </summary>
    HelpArgumentProcessorHelpPriority,

    /// <summary>
    /// EnableCodeCoverageArgumentProcessor Help
    /// </summary>
    EnableCodeCoverageArgumentProcessorHelpPriority,

    /// <summary>
    /// CollectArgumentProcessor Help
    /// </summary>
    CollectArgumentProcessorHelpPriority,

    /// <summary>
    /// InIsolationArgumentProcessor Help
    /// </summary>
    InIsolationArgumentProcessorHelpPriority,

    /// <summary>
    /// DisableAutoFakesArgumentProcessor Help
    /// </summary>
    DisableAutoFakesArgumentProcessorHelpPriority,

    /// <summary>
    /// UseVsixArgumentProcessor Help
    /// </summary>
    UseVsixArgumentProcessorHelpPriority,

    /// <summary>
    /// ListDiscoverersArgumentProcessor Help
    /// </summary>
    ListDiscoverersArgumentProcessorHelpPriority,

    /// <summary>
    /// ListExecutorsArgumentProcessor Help
    /// </summary>
    ListExecutorsArgumentProcessorHelpPriority,

    /// <summary>
    /// ListLoggersArgumentProcessor Help
    /// </summary>
    ListLoggersArgumentProcessorHelpPriority,

    /// <summary>
    /// ListSettingProviderArgumentProcessor Help
    /// </summary>
    ListSettingsProvidersArgumentProcessorHelpPriority,

    /// <summary>
    /// ResponseFileArgumentProcessor Help
    /// </summary>
    ResponseFileArgumentProcessorHelpPriority,
}
