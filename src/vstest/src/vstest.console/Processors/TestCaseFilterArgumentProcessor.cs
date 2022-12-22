// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

using CommandLineResources = Microsoft.VisualStudio.TestPlatform.CommandLine.Resources.Resources;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;

/// <summary>
/// Argument Executor for the "/TestCaseFilter" command line argument.
/// </summary>
internal class TestCaseFilterArgumentProcessor : IArgumentProcessor
{
    /// <summary>
    /// The name of the command line argument that the TestCaseFilterArgumentExecutor handles.
    /// </summary>
    public const string CommandName = "/TestCaseFilter";

    private Lazy<IArgumentProcessorCapabilities>? _metadata;
    private Lazy<IArgumentExecutor>? _executor;

    /// <summary>
    /// Gets the metadata.
    /// </summary>
    public Lazy<IArgumentProcessorCapabilities> Metadata
        => _metadata ??= new Lazy<IArgumentProcessorCapabilities>(() =>
            new TestCaseFilterArgumentProcessorCapabilities());

    /// <summary>
    /// Gets or sets the executor.
    /// </summary>
    public Lazy<IArgumentExecutor>? Executor
    {
        get => _executor ??= new Lazy<IArgumentExecutor>(() =>
            new TestCaseFilterArgumentExecutor(CommandLineOptions.Instance));

        set => _executor = value;
    }
}

internal class TestCaseFilterArgumentProcessorCapabilities : BaseArgumentProcessorCapabilities
{
    public override string CommandName => TestCaseFilterArgumentProcessor.CommandName;

    public override bool AllowMultiple => false;

    public override bool IsAction => false;

    public override ArgumentProcessorPriority Priority => ArgumentProcessorPriority.Normal;

    public override string HelpContentResourceName => CommandLineResources.TestCaseFilterArgumentHelp;

    public override HelpContentPriority HelpPriority => HelpContentPriority.TestCaseFilterArgumentProcessorHelpPriority;
}

/// <summary>
/// Argument Executor for the "/TestCaseFilter" command line argument.
/// </summary>
internal class TestCaseFilterArgumentExecutor : IArgumentExecutor
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
    public TestCaseFilterArgumentExecutor(CommandLineOptions options)
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
        var defaultFilter = _commandLineOptions.TestCaseFilterValue;
        var hasDefaultFilter = !defaultFilter.IsNullOrWhiteSpace();

        if (!hasDefaultFilter && argument.IsNullOrWhiteSpace())
        {
            throw new CommandLineException(string.Format(CultureInfo.CurrentCulture, CommandLineResources.TestCaseFilterValueRequired));
        }

        if (!hasDefaultFilter)
        {
            _commandLineOptions.TestCaseFilterValue = argument;
        }
        else
        {
            // Merge default filter an provided filter by AND operator to have both the default filter and custom filter applied.
            _commandLineOptions.TestCaseFilterValue = $"({defaultFilter})&({argument})";
        }
    }

    /// <summary>
    /// The TestCaseFilter is already set, return success.
    /// </summary>
    /// <returns> The <see cref="ArgumentProcessorResult"/> Success </returns>
    public ArgumentProcessorResult Execute()
    {
        return ArgumentProcessorResult.Success;
    }
    #endregion
}
