// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.Client.RequestHelper;
using Microsoft.VisualStudio.TestPlatform.CommandLine.Internal;
using Microsoft.VisualStudio.TestPlatform.CommandLine.TestPlatformHelpers;
using Microsoft.VisualStudio.TestPlatform.Common;
using Microsoft.VisualStudio.TestPlatform.Common.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.ArtifactProcessing;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestPlatform.Utilities;

using CommandLineResources = Microsoft.VisualStudio.TestPlatform.CommandLine.Resources.Resources;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;

internal class RunTestsArgumentProcessor : IArgumentProcessor
{
    public const string CommandName = "/RunTests";

    private Lazy<IArgumentProcessorCapabilities>? _metadata;
    private Lazy<IArgumentExecutor>? _executor;

    public Lazy<IArgumentProcessorCapabilities> Metadata
        => _metadata ??= new Lazy<IArgumentProcessorCapabilities>(() =>
            new RunTestsArgumentProcessorCapabilities());

    public Lazy<IArgumentExecutor>? Executor
    {
        get => _executor ??= new Lazy<IArgumentExecutor>(() =>
            new RunTestsArgumentExecutor(
                CommandLineOptions.Instance,
                RunSettingsManager.Instance,
                TestRequestManager.Instance,
                new ArtifactProcessingManager(CommandLineOptions.Instance.TestSessionCorrelationId),
                ConsoleOutput.Instance));

        set => _executor = value;
    }
}

internal class RunTestsArgumentProcessorCapabilities : BaseArgumentProcessorCapabilities
{
    public override string CommandName => RunTestsArgumentProcessor.CommandName;

    public override bool AllowMultiple => false;

    public override bool IsAction => true;

    public override ArgumentProcessorPriority Priority => ArgumentProcessorPriority.Normal;

    public override string HelpContentResourceName => CommandLineResources.RunTestsArgumentHelp;

    public override HelpContentPriority HelpPriority => HelpContentPriority.RunTestsArgumentProcessorHelpPriority;

    public override bool IsSpecialCommand => true;

    public override bool AlwaysExecute => false;
}

internal class RunTestsArgumentExecutor : IArgumentExecutor
{
    /// <summary>
    /// Used for getting tests to run.
    /// </summary>
    private readonly CommandLineOptions _commandLineOptions;

    /// <summary>
    /// The instance of testPlatforms
    /// </summary>
    private readonly ITestRequestManager _testRequestManager;

    /// <summary>
    /// Used for sending discovery messages.
    /// </summary>
    internal IOutput Output;

    /// <summary>
    /// Settings manager to get currently active settings.
    /// </summary>
    private readonly IRunSettingsProvider _runSettingsManager;

    /// <summary>
    /// Registers and Unregisters for test run events before and after test run
    /// </summary>
    private readonly ITestRunEventsRegistrar _testRunEventsRegistrar;

    /// <summary>
    /// Shows the number of tests which were executed
    /// </summary>
    private static long s_numberOfExecutedTests;

    /// <summary>
    /// Default constructor.
    /// </summary>
    public RunTestsArgumentExecutor(
        CommandLineOptions commandLineOptions,
        IRunSettingsProvider runSettingsProvider,
        ITestRequestManager testRequestManager,
        IArtifactProcessingManager artifactProcessingManager,
        IOutput output)
    {
        ValidateArg.NotNull(commandLineOptions, nameof(commandLineOptions));

        _commandLineOptions = commandLineOptions;
        _runSettingsManager = runSettingsProvider;
        _testRequestManager = testRequestManager;
        Output = output;
        _testRunEventsRegistrar = new TestRunRequestEventsRegistrar(Output, _commandLineOptions, artifactProcessingManager);
    }

    public void Initialize(string? argument)
    {
        // Nothing to do.
    }

    /// <summary>
    /// Execute all of the tests.
    /// </summary>
    public ArgumentProcessorResult Execute()
    {
        TPDebug.Assert(_commandLineOptions != null);
        TPDebug.Assert(!StringUtils.IsNullOrWhiteSpace(_runSettingsManager?.ActiveRunSettings?.SettingsXml));

        if (_commandLineOptions.IsDesignMode)
        {
            // Do not attempt execution in case of design mode. Expect execution to happen via the design mode client.
            return ArgumentProcessorResult.Success;
        }

        // Ensure a test source file was provided
        var anySource = _commandLineOptions.Sources.FirstOrDefault();
        if (anySource == null)
        {
            throw new CommandLineException(CommandLineResources.MissingTestSourceFile);
        }

        Output.WriteLine(CommandLineResources.StartingExecution, OutputLevel.Information);
        if (!StringUtils.IsNullOrEmpty(EqtTrace.LogFile))
        {
            Output.Information(false, CommandLineResources.VstestDiagLogOutputPath, EqtTrace.LogFile);
        }

        var runSettings = _runSettingsManager.ActiveRunSettings.SettingsXml;

        if (_commandLineOptions.Sources.Any())
        {
            RunTests(runSettings);
        }

        bool treatNoTestsAsError = RunSettingsUtilities.GetTreatNoTestsAsError(runSettings);

        return treatNoTestsAsError && s_numberOfExecutedTests == 0 ? ArgumentProcessorResult.Fail : ArgumentProcessorResult.Success;
    }

    private void RunTests(string runSettings)
    {
        // create/start test run
        EqtTrace.Info("RunTestsArgumentProcessor:Execute: Test run is starting.");
        EqtTrace.Verbose("RunTestsArgumentProcessor:Execute: Queuing Test run.");

        // for command line keep alive is always false.
        // for Windows Store apps it should be false, as Windows Store apps executor should terminate after finishing the test execution.
        var keepAlive = false;

        var runRequestPayload = new TestRunRequestPayload() { Sources = _commandLineOptions.Sources.ToList(), RunSettings = runSettings, KeepAlive = keepAlive, TestPlatformOptions = new TestPlatformOptions() { TestCaseFilter = _commandLineOptions.TestCaseFilterValue } };
        _testRequestManager.RunTests(runRequestPayload, null, _testRunEventsRegistrar, Constants.DefaultProtocolConfig);

        EqtTrace.Info("RunTestsArgumentProcessor:Execute: Test run is completed.");
    }

    private class TestRunRequestEventsRegistrar : ITestRunEventsRegistrar
    {
        private readonly IOutput _output;
        private readonly CommandLineOptions _commandLineOptions;
        private readonly IArtifactProcessingManager _artifactProcessingManager;

        public TestRunRequestEventsRegistrar(IOutput output, CommandLineOptions commandLineOptions, IArtifactProcessingManager artifactProcessingManager)
        {
            _output = output;
            _commandLineOptions = commandLineOptions;
            _artifactProcessingManager = artifactProcessingManager;
        }

        public void LogWarning(string message)
        {
            ConsoleLogger.RaiseTestRunWarning(message);
        }

        public void RegisterTestRunEvents(ITestRunRequest testRunRequest)
        {
            testRunRequest.OnRunCompletion += TestRunRequest_OnRunCompletion;
        }

        public void UnregisterTestRunEvents(ITestRunRequest testRunRequest)
        {
            testRunRequest.OnRunCompletion -= TestRunRequest_OnRunCompletion;
        }

        /// <summary>
        /// Handles the TestRunRequest complete event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">RunCompletion args</param>
        private void TestRunRequest_OnRunCompletion(object? sender, TestRunCompleteEventArgs e)
        {
            // If run is not aborted/canceled then check the count of executed tests.
            // we need to check if there are any tests executed - to try show some help info to user to check for installed vsix extensions
            if (!e.IsAborted && !e.IsCanceled)
            {
                var testsFoundInAnySource = e.TestRunStatistics != null && e.TestRunStatistics.ExecutedTests > 0;
                s_numberOfExecutedTests = e.TestRunStatistics!.ExecutedTests;

                // Indicate the user to use test adapter path command if there are no tests found
                if (!testsFoundInAnySource && !CommandLineOptions.Instance.TestAdapterPathsSet && _commandLineOptions.TestCaseFilterValue == null)
                {
                    _output.Warning(false, CommandLineResources.SuggestTestAdapterPathIfNoTestsIsFound);
                }
            }

            // Collect tests session artifacts for post processing
            if (_commandLineOptions.ArtifactProcessingMode == ArtifactProcessingMode.Collect)
            {
                TPDebug.Assert(RunSettingsManager.Instance.ActiveRunSettings.SettingsXml is not null, "RunSettingsManager.Instance.ActiveRunSettings.SettingsXml is null");
                _artifactProcessingManager.CollectArtifacts(e, RunSettingsManager.Instance.ActiveRunSettings.SettingsXml);
            }
        }
    }
}
