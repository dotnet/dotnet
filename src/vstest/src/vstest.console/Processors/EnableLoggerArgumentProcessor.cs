// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.CommandLine.Processors.Utilities;
using Microsoft.VisualStudio.TestPlatform.Common;
using Microsoft.VisualStudio.TestPlatform.Common.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

using CommandLineResources = Microsoft.VisualStudio.TestPlatform.CommandLine.Resources.Resources;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;

/// <summary>
/// An argument processor that allows the user to enable a specific logger
/// from the command line using the --Logger|/Logger command line switch.
/// </summary>
internal class EnableLoggerArgumentProcessor : IArgumentProcessor
{
    /// <summary>
    /// The command name.
    /// </summary>
    public const string CommandName = "/Logger";

    private Lazy<IArgumentProcessorCapabilities>? _metadata;
    private Lazy<IArgumentExecutor>? _executor;

    /// <summary>
    /// Gets or sets the executor.
    /// </summary>
    public Lazy<IArgumentExecutor>? Executor
    {
        get => _executor ??= new Lazy<IArgumentExecutor>(() =>
            new EnableLoggerArgumentExecutor(RunSettingsManager.Instance));

        set => _executor = value;
    }

    /// <summary>
    /// Gets the metadata.
    /// </summary>
    public Lazy<IArgumentProcessorCapabilities> Metadata
        => _metadata ??= new Lazy<IArgumentProcessorCapabilities>(() =>
            new EnableLoggerArgumentProcessorCapabilities());
}

internal class EnableLoggerArgumentProcessorCapabilities : BaseArgumentProcessorCapabilities
{
    /// <summary>
    /// Gets the command name.
    /// </summary>
    public override string CommandName => EnableLoggerArgumentProcessor.CommandName;

    /// <summary>
    /// Gets a value indicating whether allow multiple.
    /// </summary>
    public override bool AllowMultiple => true;

    /// <summary>
    /// Gets a value indicating whether is action.
    /// </summary>
    public override bool IsAction => false;

    /// <summary>
    /// Gets the priority.
    /// </summary>
    public override ArgumentProcessorPriority Priority => ArgumentProcessorPriority.Logging;

    /// <summary>
    /// Gets the help content resource name.
    /// </summary>
#if NETFRAMEWORK
    public override string HelpContentResourceName => CommandLineResources.EnableLoggersArgumentHelp;
#else
    public override string HelpContentResourceName => CommandLineResources.EnableLoggerArgumentsInNetCore;
#endif

    /// <summary>
    /// Gets the help priority.
    /// </summary>
    public override HelpContentPriority HelpPriority => HelpContentPriority.EnableLoggerArgumentProcessorHelpPriority;
}

/// <summary>
/// The argument executor.
/// </summary>
internal class EnableLoggerArgumentExecutor : IArgumentExecutor
{
    private readonly IRunSettingsProvider _runSettingsManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnableLoggerArgumentExecutor"/> class.
    /// </summary>
    public EnableLoggerArgumentExecutor(IRunSettingsProvider runSettingsManager)
    {
        ValidateArg.NotNull(runSettingsManager, nameof(runSettingsManager));
        _runSettingsManager = runSettingsManager;
    }

    #region IArgumentProcessor

    /// <summary>
    /// Initializes with the argument that was provided with the command.
    /// </summary>
    /// <param name="argument">Argument that was provided with the command.</param>
    public void Initialize(string? argument)
    {
        string exceptionMessage = string.Format(CultureInfo.CurrentCulture, CommandLineResources.LoggerUriInvalid, argument);

        // Throw error in case logger argument null or empty.
        if (argument.IsNullOrWhiteSpace())
        {
            throw new CommandLineException(exceptionMessage);
        }

        // Get logger argument list.
        var loggerArgumentList = ArgumentProcessorUtilities.GetArgumentList(argument, ArgumentProcessorUtilities.SemiColonArgumentSeparator, exceptionMessage);

        // Get logger identifier.
        var loggerIdentifier = loggerArgumentList[0];
        if (loggerIdentifier.Contains("="))
        {
            throw new CommandLineException(exceptionMessage);
        }

        // Get logger parameters
        var loggerParameterArgs = loggerArgumentList.Skip(1);
        var loggerParameters = ArgumentProcessorUtilities.GetArgumentParameters(loggerParameterArgs, ArgumentProcessorUtilities.EqualNameValueSeparator, exceptionMessage);

        // Add logger to run settings.
        LoggerUtilities.AddLoggerToRunSettings(loggerIdentifier, loggerParameters, _runSettingsManager);
    }

    /// <summary>
    /// Execute logger argument.
    /// </summary>
    /// <returns>
    /// The <see cref="ArgumentProcessorResult"/>.
    /// </returns>
    public ArgumentProcessorResult Execute()
    {
        // Nothing to do since we enabled the logger in the initialize method.
        return ArgumentProcessorResult.Success;
    }

    #endregion
}
