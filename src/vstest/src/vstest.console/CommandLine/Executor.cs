// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

extern alias Abstraction;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

using Microsoft.VisualStudio.TestPlatform.CommandLine.Internal;
using Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;
using Microsoft.VisualStudio.TestPlatform.CommandLine.TestPlatformHelpers;
using Microsoft.VisualStudio.TestPlatform.Common;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Tracing;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Tracing.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Execution;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

using Abstraction::Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Abstraction::Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;

using Microsoft.VisualStudio.TestPlatform.Utilities;

using CommandLineResources = Microsoft.VisualStudio.TestPlatform.CommandLine.Resources.Resources;

// General Flow:
// Create a command processor for each argument.
//   If there is no matching command processor for an argument, output error, display help and exit.
//   If throws during creation, output error and exit.
// If the help command processor has been requested, execute the help processor and exit.
// Order the command processors by priority.
// Allow command processors to validate against other command processors which are present.
//   If throws during validation, output error and exit.
// Process each command processor.
//   If throws during validation, output error and exit.
//   If the default (RunTests) command processor has no test containers output an error and exit
//   If the default (RunTests) command processor has no tests to run output an error and exit

// Commands metadata:
//  *Command line argument.
//   Priority.
//   Help output.
//   Required
//   Single or multiple

namespace Microsoft.VisualStudio.TestPlatform.CommandLine;

/// <summary>
/// Performs the execution based on the arguments provided.
/// </summary>
internal class Executor
{
    private const string NonARM64RunnerName = "vstest.console.exe";
    private readonly ITestPlatformEventSource _testPlatformEventSource;
    private readonly IProcessHelper _processHelper;
    private readonly IEnvironment _environment;
    private bool _showHelp;

    /// <summary>
    /// Default constructor.
    /// </summary>
    public Executor(IOutput output) : this(output, TestPlatformEventSource.Instance, new ProcessHelper(), new PlatformEnvironment())
    {
        if (!FeatureFlag.Instance.IsSet(FeatureFlag.VSTEST_DISABLE_THREADPOOL_SIZE_INCREASE))
        {
            // TODO: Get rid of this by making vstest.console code properly async.
            // The current implementation of vstest.console is blocking many threads that just wait
            // for completion in non-async way. Because threadpool is setting the limit based on processor count,
            // we exhaust the threadpool threads quickly when we set maxCpuCount to use as many workers as we have threads.
            //
            // This setting allow the threadpool to start start more threads than it normally would without any delay.
            // This won't pre-start the threads, it just pushes the limit of how many are allowed to start without waiting,
            // and in effect makes callbacks processed earlier, because we don't have to wait that much to receive the callback.
            // The correct fix would be to re-visit all code that offloads work to threadpool and avoid blocking any thread,
            // and also use async await when we need to await a completion of an action. But that is a far away goal, so this
            // is a "temporary" measure to remove the threadpool contention.
            //
            // The increase to 5* (1* is the standard + 4*) the standard limit is arbitrary. I saw that making it 2* did not help
            // and there are usually 2-3 threads blocked by waiting for other actions, so 5 seemed like a good limit.
            var additionalThreadsCount = Environment.ProcessorCount * 4;
            ThreadPool.GetMinThreads(out var workerThreads, out var completionPortThreads);
            ThreadPool.SetMinThreads(workerThreads + additionalThreadsCount, completionPortThreads + additionalThreadsCount);
        }
    }

    internal Executor(IOutput output, ITestPlatformEventSource testPlatformEventSource, IProcessHelper processHelper, IEnvironment environment)
    {
        DebuggerBreakpoint.AttachVisualStudioDebugger(WellKnownDebugEnvironmentVariables.VSTEST_RUNNER_DEBUG_ATTACHVS);
        DebuggerBreakpoint.WaitForNativeDebugger(WellKnownDebugEnvironmentVariables.VSTEST_RUNNER_NATIVE_DEBUG);
        DebuggerBreakpoint.WaitForDebugger(WellKnownDebugEnvironmentVariables.VSTEST_RUNNER_DEBUG);

        Output = output;
        _testPlatformEventSource = testPlatformEventSource;
        _showHelp = true;
        _processHelper = processHelper;
        _environment = environment;
    }

    /// <summary>
    /// Instance to use for sending output.
    /// </summary>
    private IOutput Output { get; set; }

    /// <summary>
    /// Performs the execution based on the arguments provided.
    /// </summary>
    /// <param name="args">
    /// Arguments provided to perform execution with.
    /// </param>
    /// <returns>
    /// Exit Codes - Zero (for successful command execution), One (for bad command)
    /// </returns>
    internal int Execute(params string[]? args)
    {
        _testPlatformEventSource.VsTestConsoleStart();

        var isDiag = args != null && args.Any(arg => arg.StartsWith("--diag", StringComparison.OrdinalIgnoreCase));

        // If User specifies --nologo via dotnet, do not print splat screen
        if (args != null && args.Length != 0 && args.Contains("--nologo"))
        {
            // Sanitizing this list, as I don't think we should write Argument processor for this.
            args = args.Where(val => val != "--nologo").ToArray();
        }
        else
        {
            // If we're postprocessing we don't need to show the splash
            if (!ArtifactProcessingPostProcessModeProcessor.ContainsPostProcessCommand(args))
            {
                PrintSplashScreen(isDiag, args);
            }
        }

        int exitCode = 0;

        // If we have no arguments, set exit code to 1, add a message, and include the help processor in the args.
        if (args == null || args.Length == 0 || args.Any(StringUtils.IsNullOrWhiteSpace))
        {
            Output.Error(true, CommandLineResources.NoArgumentsProvided);
            args = [HelpArgumentProcessor.CommandName];
            exitCode = 1;
        }

        if (!isDiag)
        {
            // This takes a path to log directory and log.txt file. Same as the --diag parameter, e.g. VSTEST_DIAG="logs\log.txt"
            var diag = Environment.GetEnvironmentVariable("VSTEST_DIAG");
            // This takes Verbose, Info (not Information), Warning, and Error.
            var diagVerbosity = Environment.GetEnvironmentVariable("VSTEST_DIAG_VERBOSITY");
            if (!StringUtils.IsNullOrWhiteSpace(diag))
            {
                var verbosity = TraceLevel.Verbose;
                if (diagVerbosity != null)
                {
                    if (Enum.TryParse<TraceLevel>(diagVerbosity, ignoreCase: true, out var parsedVerbosity))
                    {
                        verbosity = parsedVerbosity;
                    }
                }

                args = args.Concat(new[] { $"--diag:{diag};TraceLevel={verbosity}" }).ToArray();
            }
        }

        // Flatten arguments and process response files.
        exitCode |= FlattenArguments(args, out var flattenedArguments);

        // Get the argument processors for the arguments.
        exitCode |= GetArgumentProcessors(flattenedArguments, out List<IArgumentProcessor> argumentProcessors);

        // Verify that the arguments are valid.
        exitCode |= IdentifyDuplicateArguments(argumentProcessors);

        // Quick exit for syntax error
        if (exitCode == 1
            && argumentProcessors.All(
                processor => processor.Metadata.Value.CommandName != HelpArgumentProcessor.CommandName))
        {
            _testPlatformEventSource.VsTestConsoleStop();
            return exitCode;
        }

        // Execute all argument processors
        foreach (var processor in argumentProcessors)
        {
            if (!ExecuteArgumentProcessor(processor, ref exitCode))
            {
                break;
            }
        }

        // Use the test run result aggregator to update the exit code.
        exitCode |= (TestRunResultAggregator.Instance.Outcome == TestOutcome.Passed) ? 0 : 1;

        EqtTrace.Verbose("Executor.Execute: Exiting with exit code of {0}", exitCode);

        _testPlatformEventSource.VsTestConsoleStop();

        _testPlatformEventSource.MetricsDisposeStart();

        // Disposing Metrics Publisher when VsTestConsole ends
        TestRequestManager.Instance.Dispose();

        _testPlatformEventSource.MetricsDisposeStop();
        return exitCode;
    }

    /// <summary>
    /// Get the list of argument processors for the arguments.
    /// </summary>
    /// <param name="args">Arguments provided to perform execution with.</param>
    /// <param name="processors">List of argument processors for the arguments.</param>
    /// <returns>0 if all of the processors were created successfully and 1 otherwise.</returns>
    private int GetArgumentProcessors(string[] args, out List<IArgumentProcessor> processors)
    {
        processors = new List<IArgumentProcessor>();
        int result = 0;
        var processorFactory = ArgumentProcessorFactory.Create();
        for (var index = 0; index < args.Length; index++)
        {
            var arg = args[index];
            // If argument is '--', following arguments are key=value pairs for run settings.
            if (arg.Equals("--"))
            {
                var cliRunSettingsProcessor = processorFactory.CreateArgumentProcessor(arg, args.Skip(index + 1).ToArray());
                processors.Add(cliRunSettingsProcessor!);
                break;
            }

            var processor = processorFactory.CreateArgumentProcessor(arg);

            if (processor != null)
            {
                processors.Add(processor);
            }
            else
            {
                // No known processor was found, report an error and continue
                Output.Error(false, string.Format(CultureInfo.CurrentCulture, CommandLineResources.NoArgumentProcessorFound, arg));

                // Add the help processor
                if (result == 0)
                {
                    result = 1;
                    processors.Add(processorFactory.CreateArgumentProcessor(HelpArgumentProcessor.CommandName)!);
                }
            }
        }

        // Add the internal argument processors that should always be executed.
        // Examples: processors to enable loggers that are statically configured, and to start logging,
        // should always be executed.
        var processorsToAlwaysExecute = processorFactory.GetArgumentProcessorsToAlwaysExecute();
        foreach (var processor in processorsToAlwaysExecute)
        {
            if (processors.Any(i => i.Metadata.Value.CommandName == processor.Metadata.Value.CommandName))
            {
                continue;
            }

            // We need to initialize the argument executor if it's set to always execute. This ensures it will be initialized with other executors.
            processors.Add(ArgumentProcessorFactory.WrapLazyProcessorToInitializeOnInstantiation(processor));
        }

        // Initialize Runsettings with defaults
        RunSettingsManager.Instance.AddDefaultRunSettings();

        // Ensure we have an action argument.
        EnsureActionArgumentIsPresent(processors, processorFactory);

        // Instantiate and initialize the processors in priority order.
        processors.Sort((p1, p2) => Comparer<ArgumentProcessorPriority>.Default.Compare(p1.Metadata.Value.Priority, p2.Metadata.Value.Priority));
        foreach (var processor in processors)
        {
            try
            {
                // Ensure the instance is created.  Note that the Lazy not only instantiates
                // the argument processor, but also initializes it.
                var executorInstance = processor.Executor?.Value;
            }
            catch (Exception ex)
            {
                if (ex is CommandLineException or TestPlatformException or SettingsException)
                {
                    Output.Error(false, ex.Message);
                    result = 1;
                    _showHelp = false;
                }
                else if (ex is TestSourceException)
                {
                    Output.Error(false, ex.Message);
                    result = 1;
                    _showHelp = false;
                    break;
                }
                else
                {
                    // Let it throw - User must see crash and report it with stack trace!
                    // No need for recoverability as user will start a new vstest.console anyway
                    throw;
                }
            }
        }

        // If some argument was invalid, add help argument processor in beginning(i.e. at highest priority)
        if (result == 1 && _showHelp && processors.First().Metadata.Value.CommandName != HelpArgumentProcessor.CommandName)
        {
            processors.Insert(0, processorFactory.CreateArgumentProcessor(HelpArgumentProcessor.CommandName)!);
        }
        return result;
    }

    /// <summary>
    /// Verify that the arguments are valid.
    /// </summary>
    /// <param name="argumentProcessors">Processors to verify against.</param>
    /// <returns>0 if successful and 1 otherwise.</returns>
    private int IdentifyDuplicateArguments(IEnumerable<IArgumentProcessor> argumentProcessors)
    {
        int result = 0;

        // Used to keep track of commands that are only allowed to show up once.  The first time it is seen
        // an entry for the command will be added to the dictionary and the value will be set to 1.  If we
        // see the command again and the value is 1 (meaning this is the second time we have seen the command),
        // we will output an error and increment the count.  This ensures that the error message will only be
        // displayed once even if the user does something like /ListDiscoverers /ListDiscoverers /ListDiscoverers.
        var commandSeenCount = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        // Check each processor.
        foreach (var processor in argumentProcessors)
        {
            if (processor.Metadata.Value.AllowMultiple)
            {
                continue;
            }

            if (!commandSeenCount.TryGetValue(processor.Metadata.Value.CommandName, out int count))
            {
                commandSeenCount.Add(processor.Metadata.Value.CommandName, 1);
            }
            else if (count == 1)
            {
                result = 1;

                // Update the count so we do not print the error out for this argument multiple times.
                commandSeenCount[processor.Metadata.Value.CommandName] = ++count;
                Output.Error(false, string.Format(CultureInfo.CurrentCulture, CommandLineResources.DuplicateArgumentError, processor.Metadata.Value.CommandName));
            }
        }
        return result;
    }

    /// <summary>
    /// Ensures that an action argument is present and if one is not, then the default action argument is added.
    /// </summary>
    /// <param name="argumentProcessors">The arguments that are being processed.</param>
    /// <param name="processorFactory">A factory for creating argument processors.</param>
    private static void EnsureActionArgumentIsPresent(List<IArgumentProcessor> argumentProcessors, ArgumentProcessorFactory processorFactory)
    {
        ValidateArg.NotNull(argumentProcessors, nameof(argumentProcessors));
        ValidateArg.NotNull(processorFactory, nameof(processorFactory));

        // Determine if any of the argument processors are actions.
        var isActionIncluded = argumentProcessors.Any((processor) => processor.Metadata.Value.IsAction);

        // If no action arguments have been provided, then add the default action argument.
        if (!isActionIncluded)
        {
            argumentProcessors.Add(processorFactory.CreateDefaultActionArgumentProcessor());
        }
    }

    /// <summary>
    /// Executes the argument processor
    /// </summary>
    /// <param name="processor">Argument processor to execute.</param>
    /// <param name="exitCode">Exit status of Argument processor</param>
    /// <returns> true if continue execution, false otherwise.</returns>
    private bool ExecuteArgumentProcessor(IArgumentProcessor processor, ref int exitCode)
    {
        var continueExecution = true;
        ArgumentProcessorResult result;
        try
        {
            // TODO: Only executor that could return null is ResponseFileArgumentProcessor, maybe it could be updated
            // to follow a pattern similar to other processors and avoid returning null.
            result = processor.Executor!.Value.Execute();
        }
        catch (Exception ex)
        {
            if (ex is CommandLineException or TestPlatformException or SettingsException or InvalidOperationException)
            {
                EqtTrace.Error("ExecuteArgumentProcessor: failed to execute argument process: {0}", ex);
                Output.Error(false, ex.Message);
                result = ArgumentProcessorResult.Fail;

                // Send inner exception only when its message is different to avoid duplicate.
                if (ex is TestPlatformException &&
                    ex.InnerException != null &&
                    !string.Equals(ex.InnerException.Message, ex.Message, StringComparison.CurrentCultureIgnoreCase))
                {
                    Output.Error(false, ex.InnerException.Message);
                }
            }
            else
            {
                // Let it throw - User must see crash and report it with stack trace!
                // No need for recoverability as user will start a new vstest.console anyway
                throw;
            }
        }

        TPDebug.Assert(
            result is >= ArgumentProcessorResult.Success and <= ArgumentProcessorResult.Abort,
            "Invalid argument processor result.");

        if (result == ArgumentProcessorResult.Fail)
        {
            exitCode = 1;
        }

        if (result == ArgumentProcessorResult.Abort)
        {
            continueExecution = false;
        }
        return continueExecution;
    }

    /// <summary>
    /// Displays the Company and Copyright splash title info immediately after launch
    /// </summary>
    private void PrintSplashScreen(bool isDiag, string[]? args)
    {
        string? assemblyVersion = Product.Version;
        if (!isDiag)
        {
            var end = Product.Version?.IndexOf("-release");

            if (end >= 0)
            {
                assemblyVersion = Product.Version?.Substring(0, end.Value);
            }
        }

        string assemblyVersionAndArchitecture = $"{assemblyVersion} ({_processHelper.GetCurrentProcessArchitecture().ToString().ToLowerInvariant()})";
        string commandLineBanner = string.Format(CultureInfo.CurrentCulture, CommandLineResources.MicrosoftCommandLineTitle, assemblyVersionAndArchitecture);
        Output.WriteLine(commandLineBanner, OutputLevel.Information);
        PrintWarningIfRunningEmulatedOnArm64();
        PrintWarningIfInvokedThroughDotnetVSTest(args);
        Output.WriteLine(string.Empty, OutputLevel.Information);
    }

    /// <summary>
    /// Display a warning if we're executed through `dotnet vstest`
    /// </summary>
    private void PrintWarningIfInvokedThroughDotnetVSTest(string[]? args)
    {
        if (ShowDeprecateDotnetVStestMessageArgumentProcessor.ContainsShowDeprecateDotnetVSTestMessageCommand(args))
        {
            Output.Warning(false, CommandLineResources.VSTestDeprecationMessage);
        }
    }

    /// <summary>
    /// Display a warning if we're running the runner on ARM64 but with a different current process architecture.
    /// </summary>
    private void PrintWarningIfRunningEmulatedOnArm64()
    {
        var currentProcessArchitecture = _processHelper.GetCurrentProcessArchitecture();
        if (Path.GetFileName(_processHelper.GetCurrentProcessFileName()) == NonARM64RunnerName &&
            _environment.Architecture == PlatformArchitecture.ARM64 &&
            currentProcessArchitecture != PlatformArchitecture.ARM64)
        {
            Output.Warning(false, CommandLineResources.WarningEmulatedOnArm64, currentProcessArchitecture.ToString().ToLowerInvariant());
        }
    }

    /// <summary>
    /// Flattens command line arguments by processing response files.
    /// </summary>
    /// <param name="arguments">Arguments provided to perform execution with.</param>
    /// <param name="flattenedArguments">Array of flattened arguments.</param>
    /// <returns>0 if successful and 1 otherwise.</returns>
    private int FlattenArguments(IEnumerable<string> arguments, out string[] flattenedArguments)
    {
        List<string> outputArguments = new();
        int result = 0;

        foreach (var arg in arguments)
        {
            if (arg.StartsWith("@", StringComparison.Ordinal))
            {
                // response file:
                string path = arg.Substring(1).TrimEnd(null);
                var hadError = ReadArgumentsAndSanitize(path, out var responseFileArgs, out var nestedArgs);

                if (hadError)
                {
                    result |= 1;
                }
                else
                {
                    Output.WriteLine($"vstest.console.exe {responseFileArgs}", OutputLevel.Information);
                    outputArguments.AddRange(nestedArgs!);
                }
            }
            else
            {
                outputArguments.Add(arg);
            }
        }

        flattenedArguments = outputArguments.ToArray();
        return result;
    }

    /// <summary>
    /// Read and sanitize the arguments.
    /// </summary>
    /// <param name="fileName">File provided by user.</param>
    /// <param name="args">argument in the file as string.</param>
    /// <param name="arguments">Modified argument after sanitizing the contents of the file.</param>
    /// <returns>0 if successful and 1 otherwise.</returns>
    public bool ReadArgumentsAndSanitize(string fileName, out string? args, out string[]? arguments)
    {
        arguments = null;
        return GetContentUsingFile(fileName, out args)
            || (!args.IsNullOrEmpty() && Utilities.CommandLineUtilities.SplitCommandLineIntoArguments(args, out arguments));
    }

    private bool GetContentUsingFile(string fileName, out string? contents)
    {
        contents = null;
        try
        {
            contents = File.ReadAllText(fileName);
        }
        catch (Exception e)
        {
            EqtTrace.Verbose("Executor.Execute: Exiting with exit code of {0}", 1);
            EqtTrace.Error(string.Format(CultureInfo.InvariantCulture, "Error: Can't open command line argument file '{0}' : '{1}'", fileName, e.Message));
            Output.Error(false, string.Format(CultureInfo.CurrentCulture, CommandLineResources.OpenResponseFileError, fileName));
            return true;
        }

        return false;
    }

}
