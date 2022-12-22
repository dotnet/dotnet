// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;

/// <summary>
/// Argument Processor for the "--artifactsProcessingMode-collect|/ArtifactsProcessingMode-Collect" command line argument.
/// </summary>
internal class ArtifactProcessingCollectModeProcessor : IArgumentProcessor
{
    /// <summary>
    /// The name of the command line argument that the ArtifactProcessingModeProcessor handles.
    /// </summary>
    public const string CommandName = "/ArtifactsProcessingMode-Collect";

    private Lazy<IArgumentProcessorCapabilities>? _metadata;

    private Lazy<IArgumentExecutor>? _executor;

    /// <summary>
    /// Gets the metadata.
    /// </summary>
    public Lazy<IArgumentProcessorCapabilities> Metadata
        => _metadata ??= new Lazy<IArgumentProcessorCapabilities>(() =>
            new ArtifactProcessingCollectModeProcessorCapabilities());

    /// <summary>
    /// Gets or sets the executor.
    /// </summary>
    public Lazy<IArgumentExecutor>? Executor
    {
        get => _executor ??= new Lazy<IArgumentExecutor>(() =>
            new ArtifactProcessingCollectModeProcessorExecutor(CommandLineOptions.Instance));

        set => _executor = value;
    }
}

internal class ArtifactProcessingCollectModeProcessorCapabilities : BaseArgumentProcessorCapabilities
{
    public override string CommandName => ArtifactProcessingCollectModeProcessor.CommandName;

    public override bool AllowMultiple => false;

    // We put priority at the same level of the argument processor for runsettings passed as argument through cli.
    // We'll be sure to run before test run arg processor.
    public override ArgumentProcessorPriority Priority => ArgumentProcessorPriority.CliRunSettings;

    public override HelpContentPriority HelpPriority => HelpContentPriority.None;

    // We want to be sure that this command won't show in user help
    public override string? HelpContentResourceName => null;
}

internal enum ArtifactProcessingMode
{
    None,
    Collect,
    PostProcess
}

/// <summary>
/// Argument Executor for the "/ArtifactsProcessingMode-Collect" command line argument.
/// </summary>
internal class ArtifactProcessingCollectModeProcessorExecutor : IArgumentExecutor
{
    private readonly CommandLineOptions _commandLineOptions;

    public ArtifactProcessingCollectModeProcessorExecutor(CommandLineOptions options)
    {
        _commandLineOptions = options ?? throw new ArgumentNullException(nameof(options));
    }

    public void Initialize(string? _)
    {
        _commandLineOptions.ArtifactProcessingMode = ArtifactProcessingMode.Collect;
        EqtTrace.Verbose($"ArtifactProcessingPostProcessModeProcessorExecutor.Initialize: ArtifactProcessingMode.Collect");
    }

    public ArgumentProcessorResult Execute() => ArgumentProcessorResult.Success;
}
