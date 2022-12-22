// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using CommandLineResources = Microsoft.VisualStudio.TestPlatform.CommandLine.Resources.Resources;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;

/// <summary>
/// An argument processor that allows the user to disable fakes
/// from the command line using the --DisableAutoFakes|/DisableAutoFakes command line switch.
/// </summary>
internal class DisableAutoFakesArgumentProcessor : IArgumentProcessor
{
    public const string CommandName = "/DisableAutoFakes";

    private Lazy<IArgumentProcessorCapabilities>? _metadata;
    private Lazy<IArgumentExecutor>? _executor;

    public Lazy<IArgumentExecutor>? Executor
    {
        get => _executor ??= new Lazy<IArgumentExecutor>(() =>
            new DisableAutoFakesArgumentExecutor(CommandLineOptions.Instance));

        set => _executor = value;
    }

    public Lazy<IArgumentProcessorCapabilities> Metadata
        => _metadata ??= new Lazy<IArgumentProcessorCapabilities>(() =>
            new DisableAutoFakesArgumentProcessorCapabilities());
}

internal class DisableAutoFakesArgumentProcessorCapabilities : BaseArgumentProcessorCapabilities
{
    public override bool AllowMultiple => false;

    public override string CommandName => DisableAutoFakesArgumentProcessor.CommandName;

    public override ArgumentProcessorPriority Priority => ArgumentProcessorPriority.Normal;

    public override HelpContentPriority HelpPriority => HelpContentPriority.DisableAutoFakesArgumentProcessorHelpPriority;
}

internal class DisableAutoFakesArgumentExecutor : IArgumentExecutor
{
    private readonly CommandLineOptions _commandLineOptions;

    public DisableAutoFakesArgumentExecutor(CommandLineOptions commandLineOptions)
    {
        _commandLineOptions = commandLineOptions;
    }

    #region IArgumentProcessor

    /// <summary>
    /// Initializes with the argument that was provided with the command.
    /// </summary>
    /// <param name="argument">Argument that was provided with the command.</param>
    public void Initialize(string? argument)
    {
        if (argument.IsNullOrWhiteSpace() || !bool.TryParse(argument, out bool value))
        {
            throw new CommandLineException(CommandLineResources.DisableAutoFakesUsage);
        }

        _commandLineOptions.DisableAutoFakes = value;
    }

    /// <summary>
    /// Execute.
    /// </summary>
    /// <returns>
    /// The <see cref="ArgumentProcessorResult"/>.
    /// </returns>
    public ArgumentProcessorResult Execute()
    {
        // Nothing to do since we updated the parameter during initialize parameter
        return ArgumentProcessorResult.Success;
    }

    #endregion
}
