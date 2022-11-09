// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;

namespace Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;

/// <summary>
/// RunSettingsHelper is used to globally share additional informations about the state of runsettings.
/// </summary>
internal class RunSettingsHelper : IRunSettingsHelper
{
    private static readonly IRunSettingsHelper RunSettings = new RunSettingsHelper();

    public static IRunSettingsHelper Instance = RunSettings;

    /// <summary>
    /// If false user updated the RunConfiguration.TargetPlatform using
    /// --arch or runsettings file or -- RunConfiguration.TargetPlatform=arch
    /// This option is needed because we otherwise don't know if user specified an architecture or if we inferred it from
    /// the dll. When we add the capability to distinguish this in runsettings, this helper won't be needed.
    /// </summary>
    public bool IsDefaultTargetArchitecture { get; set; } = true;

    /// <summary>
    /// True indicates the test run is started from an Editor or IDE.
    /// Defaults to false.
    /// </summary>
    public bool IsDesignMode { get; set; }
}
