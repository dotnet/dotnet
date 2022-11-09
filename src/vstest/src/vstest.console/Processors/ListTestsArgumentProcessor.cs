// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.Client.RequestHelper;
using Microsoft.VisualStudio.TestPlatform.CommandLine.Internal;
using Microsoft.VisualStudio.TestPlatform.CommandLine.TestPlatformHelpers;
using Microsoft.VisualStudio.TestPlatform.Common;
using Microsoft.VisualStudio.TestPlatform.Common.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.Utilities;

using CommandLineResources = Microsoft.VisualStudio.TestPlatform.CommandLine.Resources.Resources;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;

/// <summary>
/// Argument Executor for the "-lt|--ListTests|/lt|/ListTests" command line argument.
/// </summary>
internal class ListTestsArgumentProcessor : IArgumentProcessor
{
    /// <summary>
    /// The short name of the command line argument that the ListTestsArgumentExecutor handles.
    /// </summary>
    public const string ShortCommandName = "/lt";

    /// <summary>
    /// The name of the command line argument that the ListTestsArgumentExecutor handles.
    /// </summary>
    public const string CommandName = "/ListTests";

    private Lazy<IArgumentProcessorCapabilities>? _metadata;
    private Lazy<IArgumentExecutor>? _executor;

    /// <summary>
    /// Gets the metadata.
    /// </summary>
    public Lazy<IArgumentProcessorCapabilities> Metadata
        => _metadata ??= new Lazy<IArgumentProcessorCapabilities>(() =>
            new ListTestsArgumentProcessorCapabilities());

    /// <summary>
    /// Gets or sets the executor.
    /// </summary>
    public Lazy<IArgumentExecutor>? Executor
    {
        get => _executor ??= new Lazy<IArgumentExecutor>(() =>
            new ListTestsArgumentExecutor(
                CommandLineOptions.Instance,
                RunSettingsManager.Instance,
                TestRequestManager.Instance));

        set => _executor = value;
    }
}

internal class ListTestsArgumentProcessorCapabilities : BaseArgumentProcessorCapabilities
{
    public override string CommandName => ListTestsArgumentProcessor.CommandName;

    public override string ShortCommandName => ListTestsArgumentProcessor.ShortCommandName;

    public override bool AllowMultiple => false;

    public override bool IsAction => true;

    public override ArgumentProcessorPriority Priority => ArgumentProcessorPriority.Normal;

    public override string HelpContentResourceName => CommandLineResources.ListTestsHelp;

    public override HelpContentPriority HelpPriority => HelpContentPriority.ListTestsArgumentProcessorHelpPriority;
}

/// <summary>
/// Argument Executor for the "/ListTests" command line argument.
/// </summary>
internal class ListTestsArgumentExecutor : IArgumentExecutor
{
    /// <summary>
    /// Used for getting sources.
    /// </summary>
    private readonly CommandLineOptions _commandLineOptions;

    /// <summary>
    /// Used for getting tests.
    /// </summary>
    private readonly ITestRequestManager _testRequestManager;

    /// <summary>
    /// Used for sending output.
    /// </summary>
    internal IOutput Output;

    /// <summary>
    /// RunSettingsManager to get currently active run settings.
    /// </summary>
    private readonly IRunSettingsProvider _runSettingsManager;

    /// <summary>
    /// Registers for discovery events during discovery
    /// </summary>
    private readonly ITestDiscoveryEventsRegistrar _discoveryEventsRegistrar;

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="options">
    /// The options.
    /// </param>
    public ListTestsArgumentExecutor(
        CommandLineOptions options,
        IRunSettingsProvider runSettingsProvider,
        ITestRequestManager testRequestManager) :
        this(options, runSettingsProvider, testRequestManager, ConsoleOutput.Instance)
    {
    }

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="options">
    /// The options.
    /// </param>
    internal ListTestsArgumentExecutor(
        CommandLineOptions options,
        IRunSettingsProvider runSettingsProvider,
        ITestRequestManager testRequestManager,
        IOutput output)
    {
        ValidateArg.NotNull(options, nameof(options));

        _commandLineOptions = options;
        Output = output;
        _testRequestManager = testRequestManager;

        _runSettingsManager = runSettingsProvider;
        _discoveryEventsRegistrar = new DiscoveryEventsRegistrar(output);
    }

    #region IArgumentExecutor

    /// <summary>
    /// Initializes with the argument that was provided with the command.
    /// </summary>
    /// <param name="argument">Argument that was provided with the command.</param>
    public void Initialize(string? argument)
    {
        if (!argument.IsNullOrWhiteSpace())
        {
            _commandLineOptions.AddSource(argument);
        }
    }

    /// <summary>
    /// Lists out the available discoverers.
    /// </summary>
    public ArgumentProcessorResult Execute()
    {
        TPDebug.Assert(Output != null);
        TPDebug.Assert(_commandLineOptions != null);
        TPDebug.Assert(!StringUtils.IsNullOrWhiteSpace(_runSettingsManager?.ActiveRunSettings?.SettingsXml));

        if (!_commandLineOptions.Sources.Any())
        {
            throw new CommandLineException(CommandLineResources.MissingTestSourceFile);
        }

        Output.WriteLine(CommandLineResources.ListTestsHeaderMessage, OutputLevel.Information);
        if (!StringUtils.IsNullOrEmpty(EqtTrace.LogFile))
        {
            Output.Information(false, CommandLineResources.VstestDiagLogOutputPath, EqtTrace.LogFile);
        }

        var runSettings = _runSettingsManager.ActiveRunSettings.SettingsXml;

        _testRequestManager.DiscoverTests(
            new DiscoveryRequestPayload() { Sources = _commandLineOptions.Sources, RunSettings = runSettings },
            _discoveryEventsRegistrar, Constants.DefaultProtocolConfig);

        return ArgumentProcessorResult.Success;
    }

    #endregion

    private class DiscoveryEventsRegistrar : ITestDiscoveryEventsRegistrar
    {
        private readonly IOutput _output;

        public DiscoveryEventsRegistrar(IOutput output)
        {
            _output = output;
        }

        public void LogWarning(string message)
        {
            ConsoleLogger.RaiseTestRunWarning(message);
        }

        public void RegisterDiscoveryEvents(IDiscoveryRequest discoveryRequest)
        {
            discoveryRequest.OnDiscoveredTests += DiscoveryRequest_OnDiscoveredTests;
        }

        public void UnregisterDiscoveryEvents(IDiscoveryRequest discoveryRequest)
        {
            discoveryRequest.OnDiscoveredTests -= DiscoveryRequest_OnDiscoveredTests;
        }

        private void DiscoveryRequest_OnDiscoveredTests(object? sender, DiscoveredTestsEventArgs args)
        {
            // List out each of the tests.
            foreach (var test in args.DiscoveredTestCases!)
            {
                _output.WriteLine(
                    string.Format(CultureInfo.CurrentCulture, CommandLineResources.AvailableTestsFormat, test.DisplayName),
                    OutputLevel.Information);
            }
        }
    }
}
