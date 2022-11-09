// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;

using Microsoft.VisualStudio.TestPlatform.Client.RequestHelper;
using Microsoft.VisualStudio.TestPlatform.CommandLine.TestPlatformHelpers;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using Microsoft.VisualStudio.TestPlatform.Common.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Utilities;

using CommandLineResources = Microsoft.VisualStudio.TestPlatform.CommandLine.Resources.Resources;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;

/// <summary>
/// The argument processor for initializing the vsix based adapters.
/// </summary>
internal class UseVsixExtensionsArgumentProcessor : IArgumentProcessor
{
    /// <summary>
    /// The name of command
    /// </summary>
    public const string CommandName = "/UseVsixExtensions";

    private Lazy<IArgumentProcessorCapabilities>? _metadata;
    private Lazy<IArgumentExecutor>? _executor;

    /// <summary>
    /// Gets the metadata.
    /// </summary>
    public Lazy<IArgumentProcessorCapabilities> Metadata
        => _metadata ??= new Lazy<IArgumentProcessorCapabilities>(() =>
            new UseVsixExtensionsArgumentProcessorCapabilities());

    /// <summary>
    /// Gets or sets the executor.
    /// </summary>
    public Lazy<IArgumentExecutor>? Executor
    {
        get => _executor ??= new Lazy<IArgumentExecutor>(() =>
            new UseVsixExtensionsArgumentExecutor(CommandLineOptions.Instance, TestRequestManager.Instance, new VSExtensionManager(), ConsoleOutput.Instance));

        set => _executor = value;
    }
}

/// <inheritdoc />
internal class UseVsixExtensionsArgumentProcessorCapabilities : BaseArgumentProcessorCapabilities
{
    public override string CommandName => UseVsixExtensionsArgumentProcessor.CommandName;

    public override bool AllowMultiple => false;

    public override bool IsAction => false;

    public override ArgumentProcessorPriority Priority => ArgumentProcessorPriority.AutoUpdateRunSettings;

    // Commenting out the help for this processor as it on the deprecation path.
    // public override string HelpContentResourceName => CommandLineResources.UseVsixExtensionsHelp;
    // public override HelpContentPriority HelpPriority => HelpContentPriority.UseVsixArgumentProcessorHelpPriority;
}

/// <summary>
/// The use vsix extensions argument executor.
/// </summary>
internal class UseVsixExtensionsArgumentExecutor : IArgumentExecutor
{
    private readonly CommandLineOptions _commandLineOptions;
    private readonly ITestRequestManager _testRequestManager;
    private readonly IVSExtensionManager _extensionManager;
    private readonly IOutput _output;

    internal UseVsixExtensionsArgumentExecutor(CommandLineOptions commandLineOptions, ITestRequestManager testRequestManager, IVSExtensionManager extensionManager, IOutput output)
    {
        _commandLineOptions = commandLineOptions;
        _testRequestManager = testRequestManager;
        _extensionManager = extensionManager;
        _output = output;
    }

    /// <inheritdoc />
    public void Initialize(string? argument)
    {
        if (argument.IsNullOrWhiteSpace())
        {
            throw new CommandLineException(string.Format(CultureInfo.CurrentCulture, CommandLineResources.UseVsixExtensionsValueRequired));
        }

        if (!bool.TryParse(argument, out bool value))
        {
            throw new CommandLineException(
                string.Format(CultureInfo.CurrentCulture, CommandLineResources.InvalidUseVsixExtensionsCommand, argument));
        }

        _output.Warning(false, string.Format(CultureInfo.CurrentCulture, CommandLineResources.UseVsixExtensionsDeprecation));
        _commandLineOptions.UseVsixExtensions = value;

        if (_commandLineOptions.UseVsixExtensions)
        {
            var vsixExtensions = _extensionManager.GetUnitTestExtensions();
            _testRequestManager.InitializeExtensions(vsixExtensions, skipExtensionFilters: true);
        }
    }

    /// <inheritdoc />
    public ArgumentProcessorResult Execute()
    {
        return ArgumentProcessorResult.Success;
    }
}
