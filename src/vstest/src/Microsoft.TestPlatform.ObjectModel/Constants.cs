// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel;

/// <summary>
/// Defines the defaults/constants used across different components.
/// </summary>
public static class Constants
{
    /// <summary>
    /// The in process data collection run settings name.
    /// </summary>
    public const string InProcDataCollectionRunSettingsName = "InProcDataCollectionRunSettings";

    /// <summary>
    /// The in process data collector setting name.
    /// </summary>
    public const string InProcDataCollectorSettingName = "InProcDataCollector";

    /// <summary>
    /// The in process data collectors setting name.
    /// </summary>
    public const string InProcDataCollectorsSettingName = "InProcDataCollectors";

    /// <summary>
    /// Name of collect dump option for blame.
    /// </summary>
    public const string BlameCollectDumpKey = "CollectDump";

    /// <summary>
    /// Name of collect dump option for blame.
    /// The internal visibility is because the new feature is not publicly exposed yet and so we can retire it.
    /// </summary>
    internal const string BlameCollectMonitorPostMortemDebuggerKey = "MonitorPostmortemDebugger";

    /// <summary>
    /// Name of collect dump option for blame.
    /// </summary>
    public const string BlameCollectHangDumpKey = "CollectHangDump";

    /// <summary>
    /// Name of collect hang dump option for blame.
    /// </summary>
    public const string CollectDumpOnTestSessionHang = "CollectDumpOnTestSessionHang";

    /// <summary>
    /// Name of data collection settings node in RunSettings.
    /// </summary>
    public const string DataCollectionRunSettingsName = "DataCollectionRunSettings";

    /// <summary>
    /// Name of logger run settings node in RunSettings.
    /// </summary>
    public const string LoggerRunSettingsName = "LoggerRunSettings";

    /// <summary>
    /// Name of loggers node in RunSettings.
    /// </summary>
    public const string LoggersSettingName = "Loggers";

    /// <summary>
    /// Name of logger node in RunSettings.
    /// </summary>
    public const string LoggerSettingName = "Logger";

    /// <summary>
    /// Name of friendlyName attribute of logger node.
    /// </summary>
    public const string LoggerFriendlyName = "friendlyName";

    /// <summary>
    /// Name of friendlyName attribute of logger node in lower case.
    /// </summary>
    public const string LoggerFriendlyNameLower = "friendlyname";

    /// <summary>
    /// Name of uri attribute of logger node.
    /// </summary>
    public const string LoggerUriName = "uri";

    /// <summary>
    /// Name of assemblyQualifiedName attribute of logger node.
    /// </summary>
    public const string LoggerAssemblyQualifiedName = "assemblyQualifiedName";

    /// <summary>
    /// Name of assemblyQualifiedName attribute of logger node in lower case.
    /// </summary>
    public const string LoggerAssemblyQualifiedNameLower = "assemblyqualifiedname";

    /// <summary>
    /// Name of codeBase attribute of logger node.
    /// </summary>
    public const string LoggerCodeBase = "codeBase";

    /// <summary>
    /// Name of codeBase attribute of logger node in lower case.
    /// </summary>
    public const string LoggerCodeBaseLower = "codebase";

    /// <summary>
    /// Name of enabled attribute of logger node.
    /// </summary>
    public const string LoggerEnabledName = "enabled";

    /// <summary>
    /// Name of configuration element of logger node.
    /// </summary>
    public const string LoggerConfigurationName = "Configuration";

    /// <summary>
    /// Name of configuration element of logger node in lower case.
    /// </summary>
    public const string LoggerConfigurationNameLower = "configuration";

    /// <summary>
    /// Name of TreatNoTestsAsError parameter
    /// </summary>
    public const string TreatNoTestsAsError = "TreatNoTestsAsError";

    /// <summary>
    /// Name of RunConfiguration settings node in RunSettings.
    /// </summary>
    public const string RunConfigurationSettingsName = "RunConfiguration";

    /// <summary>
    /// Default testrunner if testrunner is not specified
    /// </summary>
    public const string UnspecifiedAdapterPath = "_none_";

    public const string DataCollectorsSettingName = "DataCollectors";

    public const string RunSettingsName = "RunSettings";

    public const string DataCollectorSettingName = "DataCollector";

    /// <summary>
    /// Pattern used to find test run parameter node.
    /// </summary>
    public const string TestRunParametersName = "TestRunParameters";

    /// <summary>
    /// Type of the unit test extension. (Extension author will use this name while authoring their Vsix)
    /// </summary>
    public const string UnitTestExtensionType = "UnitTestExtension";

    /// <summary>
    /// Maximum size of the trace log file (in kilobytes).
    /// </summary>
    public const string TraceLogMaxFileSizeInKB = "TraceLogMaxFileSizeInKb";

    public const string EmptyRunSettings = @"<RunSettings></RunSettings>";

    public static readonly Architecture DefaultPlatform = XmlRunSettingsUtilities.OSArchitecture;

    /// <summary>
    /// Adding this for compatibility
    /// </summary>
    public const FrameworkVersion DefaultFramework = FrameworkVersion.Framework40;

    /// <summary>
    /// Default option for parallel execution
    /// </summary>
    public const int DefaultCpuCount = 1;

    /// <summary>
    /// The default batch size.
    /// </summary>
    public const long DefaultBatchSize = 10;

    /// <summary>
    /// The default protocol version
    /// </summary>
    // TODO: COMPAT: This is not great because it is not a constant, and so an external dll tells us what
    // capabilities we have in the implementer (e.g. client), instead we should move this closer to communication
    // utilities where protocol version actually makes sense. And review all the usages of this, because there are few
    // ways that the default version is determined.
    public static readonly ProtocolConfig DefaultProtocolConfig = new() { Version = 7 };

    /// <summary>
    /// The minimum protocol version that has debug support
    /// </summary>
    public const int MinimumProtocolVersionWithDebugSupport = 3;

    /// <summary>
    /// The minimum protocol version that has debug support
    /// </summary>
    public const int MinimumProtocolVersionWithCancelDiscoveryEventHandlerSupport = 6;

    /// <summary>
    /// Name of the results directory
    /// </summary>
    public const string ResultsDirectoryName = "TestResults";

    /// <summary>
    /// Default results directory.
    /// </summary>
    public static readonly string DefaultResultsDirectory = Path.Combine(Directory.GetCurrentDirectory(), ResultsDirectoryName);

    /// <summary>
    /// Default treatment of error from test adapters.
    /// </summary>
    public const bool DefaultTreatTestAdapterErrorsAsWarnings = false;

    /// <summary>
    /// The default execution thread apartment state.
    /// </summary>
    [CLSCompliant(false)]
#if NETFRAMEWORK
    // Keeping default STA thread for desktop tests for UI/Functional test scenarios
    public static readonly PlatformApartmentState DefaultExecutionThreadApartmentState = PlatformApartmentState.STA;
#else
    // STA threads are not supported for net core, default to MTA
    public static readonly PlatformApartmentState DefaultExecutionThreadApartmentState = PlatformApartmentState.MTA;
#endif

    /// <summary>
    ///  Constants for detecting .net framework.
    /// </summary>
    public const string TargetFrameworkAttributeFullName = "System.Runtime.Versioning.TargetFrameworkAttribute";

    public const string DotNetFrameWorkStringPrefix = ".NETFramework,Version=";

    public const string DotNetFramework35 = ".NETFramework,Version=v3.5";

    public const string DotNetFramework40 = ".NETFramework,Version=v4.0";

    public const string DotNetFramework45 = ".NETFramework,Version=v4.5";

    public const string DotNetFramework451 = ".NETFramework,Version=v4.5.1";

    public const string DotNetFramework46 = ".NETFramework,Version=v4.6";

    public const string DotNetFrameworkCore10 = ".NETCoreApp,Version=v1.0";

    public const string DotNetFrameworkUap10 = "UAP,Version=v10.0";

    public const string TargetFrameworkName = "TargetFrameworkName";
}

/// <summary>
/// Default parameters to be passed onto all loggers.
/// </summary>
public static class DefaultLoggerParameterNames
{
    // Denotes target location for test run results
    // For ex. TrxLogger saves test run results at this target
    public const string TestRunDirectory = "TestRunDirectory";

    // Denotes target framework for the tests.
    public const string TargetFramework = "TargetFramework";
}
