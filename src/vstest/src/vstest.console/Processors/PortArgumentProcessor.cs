// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

extern alias Abstraction;

using System;
using System.Globalization;

using Microsoft.VisualStudio.TestPlatform.Client.DesignMode;
using Microsoft.VisualStudio.TestPlatform.Client.RequestHelper;
using Microsoft.VisualStudio.TestPlatform.CommandLine.TestPlatformHelpers;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Abstraction::Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Abstraction::Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;

using CommandLineResources = Microsoft.VisualStudio.TestPlatform.CommandLine.Resources.Resources;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;

/// <summary>
/// Argument Processor for the "--Port|/Port" command line argument.
/// </summary>
internal class PortArgumentProcessor : IArgumentProcessor
{
    /// <summary>
    /// The name of the command line argument that the PortArgumentExecutor handles.
    /// </summary>
    public const string CommandName = "/Port";

    private Lazy<IArgumentProcessorCapabilities>? _metadata;
    private Lazy<IArgumentExecutor>? _executor;

    /// <summary>
    /// Gets the metadata.
    /// </summary>
    public Lazy<IArgumentProcessorCapabilities> Metadata
        => _metadata ??= new Lazy<IArgumentProcessorCapabilities>(() => new PortArgumentProcessorCapabilities());

    /// <summary>
    /// Gets or sets the executor.
    /// </summary>
    public Lazy<IArgumentExecutor>? Executor
    {
        get => _executor ??= new Lazy<IArgumentExecutor>(() =>
            new PortArgumentExecutor(CommandLineOptions.Instance, TestRequestManager.Instance));

        set => _executor = value;
    }
}

internal class PortArgumentProcessorCapabilities : BaseArgumentProcessorCapabilities
{
    public override string CommandName => PortArgumentProcessor.CommandName;

    public override bool AllowMultiple => false;

    public override bool IsAction => false;

    public override ArgumentProcessorPriority Priority => ArgumentProcessorPriority.DesignMode;

    public override string HelpContentResourceName => CommandLineResources.PortArgumentHelp;

    public override HelpContentPriority HelpPriority => HelpContentPriority.PortArgumentProcessorHelpPriority;
}

/// <summary>
/// Argument Executor for the "/Port" command line argument.
/// </summary>
internal class PortArgumentExecutor : IArgumentExecutor
{
    /// <summary>
    /// Used for getting sources.
    /// </summary>
    private readonly CommandLineOptions _commandLineOptions;

    /// <summary>
    /// Test Request Manager
    /// </summary>
    private readonly ITestRequestManager _testRequestManager;

    /// <summary>
    /// Initializes Design mode when called
    /// </summary>
    private readonly Func<int, IProcessHelper, IDesignModeClient> _designModeInitializer;

    /// <summary>
    /// IDesignModeClient
    /// </summary>
    private IDesignModeClient? _designModeClient;

    /// <summary>
    /// Process helper for process management actions.
    /// </summary>
    private readonly IProcessHelper _processHelper;

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="options">
    /// The options.
    /// </param>
    /// <param name="testRequestManager"> Test request manager</param>
    public PortArgumentExecutor(CommandLineOptions options, ITestRequestManager testRequestManager)
        : this(options, testRequestManager, InitializeDesignMode, new ProcessHelper())
    {
    }

    /// <summary>
    /// For Unit testing only
    /// </summary>
    internal PortArgumentExecutor(CommandLineOptions options, ITestRequestManager testRequestManager, IProcessHelper processHelper)
        : this(options, testRequestManager, InitializeDesignMode, processHelper)
    {
    }

    /// <summary>
    /// For Unit testing only
    /// </summary>
    internal PortArgumentExecutor(CommandLineOptions options, ITestRequestManager testRequestManager, Func<int, IProcessHelper, IDesignModeClient> designModeInitializer, IProcessHelper processHelper)
    {
        ValidateArg.NotNull(options, nameof(options));
        _commandLineOptions = options;
        _testRequestManager = testRequestManager;
        _designModeInitializer = designModeInitializer;
        _processHelper = processHelper;
    }


    #region IArgumentExecutor

    /// <summary>
    /// Initializes with the argument that was provided with the command.
    /// </summary>
    /// <param name="argument">Argument that was provided with the command.</param>
    public void Initialize(string? argument)
    {
        if (argument.IsNullOrWhiteSpace() || !int.TryParse(argument, out int portNumber))
        {
            throw new CommandLineException(CommandLineResources.InvalidPortArgument);
        }

        _commandLineOptions.Port = portNumber;
        _commandLineOptions.IsDesignMode = true;
        RunSettingsHelper.Instance.IsDesignMode = true;
        _designModeClient = _designModeInitializer?.Invoke(_commandLineOptions.ParentProcessId, _processHelper);
    }

    /// <summary>
    /// Initialize the design mode client.
    /// </summary>
    /// <returns> The <see cref="ArgumentProcessorResult.Success"/> if initialization is successful. </returns>
    public ArgumentProcessorResult Execute()
    {
        try
        {
            _designModeClient?.ConnectToClientAndProcessRequests(_commandLineOptions.Port, _testRequestManager);
        }
        catch (TimeoutException ex)
        {
            throw new CommandLineException(string.Format(CultureInfo.CurrentCulture, CommandLineResources.DesignModeClientTimeoutError, _commandLineOptions.Port), ex);
        }

        return ArgumentProcessorResult.Success;
    }

    #endregion

    private static IDesignModeClient InitializeDesignMode(int parentProcessId, IProcessHelper processHelper)
    {
        if (parentProcessId > 0)
        {
            processHelper.SetExitCallback(parentProcessId, (obj) =>
            {
                EqtTrace.Info($"PortArgumentProcessor: parent process:{parentProcessId} exited.");
                DesignModeClient.Instance?.HandleParentProcessExit();
            });
        }

        DesignModeClient.Initialize();
        return DesignModeClient.Instance;
    }
}
