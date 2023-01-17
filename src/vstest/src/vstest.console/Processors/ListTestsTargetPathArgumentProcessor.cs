// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;

/// <summary>
///  An argument processor to provide path to the file for listing fully qualified tests.
/// To be used only with ListFullyQualifiedTests
/// </summary>
internal class ListTestsTargetPathArgumentProcessor : IArgumentProcessor
{
    public const string CommandName = "/ListTestsTargetPath";

    private Lazy<IArgumentProcessorCapabilities>? _metadata;
    private Lazy<IArgumentExecutor>? _executor;

    /// <summary>
    /// Gets the metadata.
    /// </summary>
    public Lazy<IArgumentProcessorCapabilities> Metadata
        => _metadata ??= new Lazy<IArgumentProcessorCapabilities>(() =>
            new ListTestsTargetPathArgumentProcessorCapabilities());

    /// <summary>
    /// Gets or sets the executor.
    /// </summary>
    public Lazy<IArgumentExecutor>? Executor
    {
        get => _executor ??= new Lazy<IArgumentExecutor>(() =>
            new ListTestsTargetPathArgumentExecutor(CommandLineOptions.Instance));

        set => _executor = value;
    }
}

internal class ListTestsTargetPathArgumentProcessorCapabilities : BaseArgumentProcessorCapabilities
{
    public override string CommandName => ListTestsTargetPathArgumentProcessor.CommandName;

    public override bool AllowMultiple => false;

    public override bool IsAction => false;

    public override ArgumentProcessorPriority Priority => ArgumentProcessorPriority.Normal;
}

internal class ListTestsTargetPathArgumentExecutor : IArgumentExecutor
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
    public ListTestsTargetPathArgumentExecutor(CommandLineOptions options)
    {
        ValidateArg.NotNull(options, nameof(options));
        _commandLineOptions = options;
    }

    #region IArgumentExecutor

    /// <summary>
    /// Initializes with the argument that was provided with the command.
    /// </summary>
    /// <param name="argument">Argument that was provided with the command.</param>
    public void Initialize(string? argument)
    {
        if (argument.IsNullOrWhiteSpace())
        {
            // Not adding this string to resources because this processor is only used internally.
            throw new CommandLineException("ListTestsTargetPath is required with ListFullyQualifiedTests!");
        }

        _commandLineOptions.ListTestsTargetPath = argument;
    }

    /// <summary>
    /// The ListTestsTargetPath is already set, return success.
    /// </summary>
    /// <returns> The <see cref="ArgumentProcessorResult"/> Success </returns>
    public ArgumentProcessorResult Execute()
    {
        return ArgumentProcessorResult.Success;
    }
    #endregion
}
