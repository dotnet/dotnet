// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

using CommandLineResources = Microsoft.VisualStudio.TestPlatform.CommandLine.Resources.Resources;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;

/// <summary>
/// Argument Processor for the "--ParentProcessId|/ParentProcessId" command line argument.
/// </summary>
internal class ParentProcessIdArgumentProcessor : IArgumentProcessor
{
    /// <summary>
    /// The name of the command line argument that the ParentProcessIdArgumentExecutor handles.
    /// </summary>
    public const string CommandName = "/ParentProcessId";

    private Lazy<IArgumentProcessorCapabilities>? _metadata;
    private Lazy<IArgumentExecutor>? _executor;

    /// <summary>
    /// Gets the metadata.
    /// </summary>
    public Lazy<IArgumentProcessorCapabilities> Metadata
        => _metadata ??= new Lazy<IArgumentProcessorCapabilities>(() =>
            new ParentProcessIdArgumentProcessorCapabilities());

    /// <summary>
    /// Gets or sets the executor.
    /// </summary>
    public Lazy<IArgumentExecutor>? Executor
    {
        get => _executor ??= new Lazy<IArgumentExecutor>(() =>
            new ParentProcessIdArgumentExecutor(CommandLineOptions.Instance));

        set => _executor = value;
    }
}

internal class ParentProcessIdArgumentProcessorCapabilities : BaseArgumentProcessorCapabilities
{
    public override string CommandName => ParentProcessIdArgumentProcessor.CommandName;

    public override bool AllowMultiple => false;

    public override bool IsAction => false;

    public override ArgumentProcessorPriority Priority => ArgumentProcessorPriority.DesignMode;

    public override string HelpContentResourceName => CommandLineResources.ParentProcessIdArgumentHelp;

    public override HelpContentPriority HelpPriority => HelpContentPriority.ParentProcessIdArgumentProcessorHelpPriority;
}

/// <summary>
/// Argument Executor for the "/ParentProcessId" command line argument.
/// </summary>
internal class ParentProcessIdArgumentExecutor : IArgumentExecutor
{
    /// <summary>
    /// Used for getting sources.
    /// </summary>
    private readonly CommandLineOptions _commandLineOptions;

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="options">
    /// The options.
    /// </param>
    public ParentProcessIdArgumentExecutor(CommandLineOptions options)
    {
        ValidateArg.NotNull(options, nameof(options));
        _commandLineOptions = options;
    }

    /// <summary>
    /// Initializes with the argument that was provided with the command.
    /// </summary>
    /// <param name="argument">Argument that was provided with the command.</param>
    public void Initialize(string? argument)
    {
        if (argument.IsNullOrWhiteSpace() || !int.TryParse(argument, out int parentProcessId))
        {
            throw new CommandLineException(CommandLineResources.InvalidParentProcessIdArgument);
        }

        _commandLineOptions.ParentProcessId = parentProcessId;
    }

    /// <summary>
    /// ParentProcessId is already set, return success.
    /// </summary>
    /// <returns> The <see cref="ArgumentProcessorResult"/> Success </returns>
    public ArgumentProcessorResult Execute()
    {
        // Nothing to do here, the work was done in initialization.
        return ArgumentProcessorResult.Success;
    }
}
