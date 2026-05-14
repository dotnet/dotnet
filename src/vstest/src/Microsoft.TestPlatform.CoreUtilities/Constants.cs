// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.CoreUtilities;

/// <summary>
/// The set of constants required for across various(Communication, CrossPlatform, etc.) modules.
/// </summary>
public class Constants
{
    /// <summary>
    ///  Vstest.console process name, without file extension(.exe/.dll)
    /// </summary>
    public const string VstestConsoleProcessName = "vstest.console";

    /// <summary>
    /// Testhost process name, without file extension(.exe/.dll) and architecture type(x86).
    /// </summary>
    public const string TesthostProcessName = "testhost";

    /// <summary>
    /// Datacollector process name, without file extension(.exe/.dll)
    /// </summary>
    public const string DatacollectorProcessName = "datacollector";

    /// <summary>
    /// Number of character should be logged on child process exited with
    /// error message on standard error.
    /// </summary>
    public const int StandardErrorMaxLength = 8192; // 8 KB

    /// <summary>
    /// Environment Variable Specified by user to setup Culture.
    /// </summary>
    public const string DotNetUserSpecifiedCulture = "DOTNET_CLI_UI_LANGUAGE";

    /// <summary>
    /// Test sources key name
    /// </summary>
    public const string TestSourcesKeyName = "TestSources";
}
