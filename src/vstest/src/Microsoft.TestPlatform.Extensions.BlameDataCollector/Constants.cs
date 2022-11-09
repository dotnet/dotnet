// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.TestPlatform.Extensions.BlameDataCollector;

/// <summary>
/// Class for constants used across the files.
/// </summary>
internal static class Constants
{
    /// <summary>
    /// Root node name for Xml file.
    /// </summary>
    public const string BlameRootNode = "TestSequence";

    /// <summary>
    /// Node name for each Xml node.
    /// </summary>
    public const string BlameTestNode = "Test";

    /// <summary>
    /// Attachment File name.
    /// </summary>
    public const string AttachmentFileName = "Sequence";

    /// <summary>
    /// Test Name Attribute.
    /// </summary>
    public const string TestNameAttribute = "Name";

    /// <summary>
    /// Test Source Attribute.
    /// </summary>
    public const string TestSourceAttribute = "Source";

    /// <summary>
    /// Test Completed Attribute.
    /// </summary>
    public const string TestCompletedAttribute = "Completed";

    /// <summary>
    /// Test Display Name Attribute.
    /// </summary>
    public const string TestDisplayNameAttribute = "DisplayName";

    /// <summary>
    /// Friendly name of the data collector
    /// </summary>
    public const string BlameDataCollectorName = "Blame";

    /// <summary>
    /// Configuration key name for dump mode
    /// </summary>
    public const string DumpModeKey = "CollectDump";

    /// <summary>
    /// Configuration key name for hang dump mode
    /// </summary>
    public const string HangDumpModeKey = "CollectHangDump";

    /// <summary>
    /// Proc dump 32 bit version
    /// </summary>
    public const string ProcdumpProcess = "procdump.exe";

    /// <summary>
    /// Proc dump 64 bit version
    /// </summary>
    public const string Procdump64Process = "procdump64.exe";

    /// <summary>
    /// Proc dump 64 bit version
    /// </summary>
    public const string ProcdumpUnixProcess = "procdump";

    /// <summary>
    /// Configuration key name for collect dump always
    /// </summary>
    public const string CollectDumpAlwaysKey = "CollectAlways";

    /// <summary>
    /// Configuration key name for collecting dump in case of testhost hang
    /// </summary>
    public const string CollectDumpOnTestSessionHang = "CollectDumpOnTestSessionHang";

    /// <summary>
    /// Configuration key name for monitoring a folder for postmortem dumps
    /// </summary>
    public const string MonitorPostmortemDebugger = "MonitorPostmortemDebugger";

    /// <summary>
    /// Configuration key name for specifying what the expected execution time for the longest running test is.
    /// If no events come from the test host for this period a dump will be collected and the test host process will
    /// be killed.
    /// </summary>
    public const string TestTimeout = "TestTimeout";

    /// <summary>
    /// Configuration key name for dump type
    /// </summary>
    public const string DumpTypeKey = "DumpType";

    /// <summary>
    /// Configuration key name for hang dump type
    /// </summary>
    public const string HangDumpTypeKey = "HangDumpType";

    /// <summary>
    /// Configuration value for true
    /// </summary>
    public const string TrueConfigurationValue = "True";

    /// <summary>
    /// Configuration value for false
    /// </summary>
    public const string FalseConfigurationValue = "False";

    /// <summary>
    /// The target framework of test host.
    /// </summary>
    public const string TargetFramework = "Framework";
}
