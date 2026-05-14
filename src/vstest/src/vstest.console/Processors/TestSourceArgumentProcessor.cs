// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;

/// <summary>
/// Argument Executor which handles adding the source provided to the TestManager.
/// </summary>
internal class TestSourceArgumentProcessor : IArgumentProcessor
{
    /// <summary>
    /// The command name.
    /// </summary>
    public const string CommandName = "/TestSource";

    private Lazy<IArgumentProcessorCapabilities>? _metadata;
    private Lazy<IArgumentExecutor>? _executor;

    /// <summary>
    /// Gets the metadata.
    /// </summary>
    public Lazy<IArgumentProcessorCapabilities> Metadata
        => _metadata ??= new Lazy<IArgumentProcessorCapabilities>(() =>
            new TestSourceArgumentProcessorCapabilities());

    /// <summary>
    /// Gets or sets the executor.
    /// </summary>
    public Lazy<IArgumentExecutor>? Executor
    {
        get => _executor ??= new Lazy<IArgumentExecutor>(() =>
            new TestSourceArgumentExecutor(CommandLineOptions.Instance));

        set => _executor = value;
    }
}

/// <summary>
/// The test source argument processor capabilities.
/// </summary>
internal class TestSourceArgumentProcessorCapabilities : BaseArgumentProcessorCapabilities
{
    public override string CommandName => TestSourceArgumentProcessor.CommandName;

    public override ArgumentProcessorPriority Priority => ArgumentProcessorPriority.Normal;

    public override bool IsSpecialCommand => true;
}

/// <summary>
/// Argument Executor which handles adding the source provided to the TestManager.
/// </summary>
internal class TestSourceArgumentExecutor : IArgumentExecutor
{
    /// <summary>
    /// Used for adding sources to the test manager.
    /// </summary>
    private readonly CommandLineOptions _testSources;

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="testSources">
    /// The test Sources.
    /// </param>
    public TestSourceArgumentExecutor(CommandLineOptions testSources)
    {
        ValidateArg.NotNull(testSources, nameof(testSources));
        _testSources = testSources;
    }


    #region IArgumentExecutor

    /// <summary>
    /// Initializes with the argument that was provided with the command.
    /// </summary>
    /// <param name="argument">Argument that was provided with the command.</param>
    public void Initialize(string? argument)
    {
        if (!argument.IsNullOrEmpty())
        {
            _testSources.AddSource(argument);
        }
    }

    /// <summary>
    /// Executes the argument processor.
    /// </summary>
    /// <returns>
    /// The <see cref="ArgumentProcessorResult"/>.
    /// </returns>
    public ArgumentProcessorResult Execute()
    {
        // Nothing to do. Our work was done during initialize.
        return ArgumentProcessorResult.Success;
    }

    #endregion

}
