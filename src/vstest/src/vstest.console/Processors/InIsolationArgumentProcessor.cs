// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;

using Microsoft.VisualStudio.TestPlatform.Common;
using Microsoft.VisualStudio.TestPlatform.Common.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

using CommandLineResources = Microsoft.VisualStudio.TestPlatform.CommandLine.Resources.Resources;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;

/// <summary>
///  An argument processor that allows the user to specify whether the execution
///  should happen in the current vstest.console.exe process or a new different process.
/// </summary>
internal class InIsolationArgumentProcessor : IArgumentProcessor
{
    public const string CommandName = "/InIsolation";

    private Lazy<IArgumentProcessorCapabilities>? _metadata;
    private Lazy<IArgumentExecutor>? _executor;

    /// <summary>
    /// Gets the metadata.
    /// </summary>
    public Lazy<IArgumentProcessorCapabilities> Metadata
        => _metadata ??= new Lazy<IArgumentProcessorCapabilities>(() =>
            new InIsolationArgumentProcessorCapabilities());

    /// <summary>
    /// Gets or sets the executor.
    /// </summary>
    public Lazy<IArgumentExecutor>? Executor
    {
        get => _executor ??= new Lazy<IArgumentExecutor>(() =>
            new InIsolationArgumentExecutor(CommandLineOptions.Instance, RunSettingsManager.Instance));

        set => _executor = value;
    }
}

internal class InIsolationArgumentProcessorCapabilities : BaseArgumentProcessorCapabilities
{
    public override string CommandName => InIsolationArgumentProcessor.CommandName;

    public override bool AllowMultiple => false;

    public override bool IsAction => false;

    public override ArgumentProcessorPriority Priority => ArgumentProcessorPriority.AutoUpdateRunSettings;

    public override string HelpContentResourceName => CommandLineResources.InIsolationHelp;

    public override HelpContentPriority HelpPriority => HelpContentPriority.InIsolationArgumentProcessorHelpPriority;
}

internal class InIsolationArgumentExecutor : IArgumentExecutor
{
    private readonly CommandLineOptions _commandLineOptions;
    private readonly IRunSettingsProvider _runSettingsManager;

    public const string RunSettingsPath = "RunConfiguration.InIsolation";

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="options">Command line options</param>
    /// <param name="runSettingsManager">the runsettings manager</param>
    public InIsolationArgumentExecutor(CommandLineOptions options, IRunSettingsProvider runSettingsManager)
    {
        ValidateArg.NotNull(options, nameof(options));
        _commandLineOptions = options;
        _runSettingsManager = runSettingsManager;
    }

    #region IArgumentProcessor

    /// <summary>
    /// Initializes with the argument that was provided with the command.
    /// </summary>
    /// <param name="argument">Argument that was provided with the command.</param>
    public void Initialize(string? argument)
    {
        // InIsolation does not require any argument, throws exception if argument specified
        if (!argument.IsNullOrWhiteSpace())
        {
            throw new CommandLineException(
                string.Format(CultureInfo.CurrentCulture, CommandLineResources.InvalidInIsolationCommand, argument));
        }

        _commandLineOptions.InIsolation = true;
        _runSettingsManager.UpdateRunSettingsNode(RunSettingsPath, "true");
    }

    /// <summary>
    /// Execute.
    /// </summary>
    public ArgumentProcessorResult Execute()
    {
        // Nothing to do since we updated the parameter during initialize parameter
        return ArgumentProcessorResult.Success;
    }

    #endregion
}
