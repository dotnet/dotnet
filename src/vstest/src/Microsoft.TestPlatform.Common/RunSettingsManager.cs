// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.VisualStudio.TestPlatform.Common.Interfaces;

namespace Microsoft.VisualStudio.TestPlatform.Common;

/// <summary>
/// Manages the active run settings.
/// </summary>
internal class RunSettingsManager : IRunSettingsProvider
{
    private static readonly object LockObject = new();

    private static RunSettingsManager? s_runSettingsManagerInstance;

    /// <summary>
    /// Default constructor.
    /// </summary>
    private RunSettingsManager()
    {
        ActiveRunSettings = new RunSettings();
    }

    #region IRunSettingsProvider

    /// <summary>
    /// Gets the active run settings.
    /// </summary>
    public RunSettings ActiveRunSettings { get; private set; }

    #endregion

    [AllowNull]
    public static RunSettingsManager Instance
    {
        get
        {
            if (s_runSettingsManagerInstance != null)
            {
                return s_runSettingsManagerInstance;
            }

            lock (LockObject)
            {
                s_runSettingsManagerInstance ??= new RunSettingsManager();
            }

            return s_runSettingsManagerInstance;
        }
        internal set
        {
            s_runSettingsManagerInstance = value;
        }
    }

    /// <summary>
    /// Set the active run settings.
    /// </summary>
    /// <param name="runSettings">RunSettings to make the active Run Settings.</param>
    public void SetActiveRunSettings(RunSettings runSettings)
    {
        ActiveRunSettings = runSettings ?? throw new ArgumentNullException(nameof(runSettings));
    }

}
