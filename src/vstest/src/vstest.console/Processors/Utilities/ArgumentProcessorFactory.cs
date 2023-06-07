// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.Utilities;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;

/// <summary>
/// Used to create the appropriate instance of an argument processor.
/// </summary>
internal class ArgumentProcessorFactory
{
    /// <summary>
    /// Available argument processors.
    /// </summary>
    private Dictionary<string, IArgumentProcessor>? _commandToProcessorMap;
    private Dictionary<string, IArgumentProcessor>? _specialCommandToProcessorMap;

    /// Initializes the argument processor factory.
    /// </summary>
    /// <param name="argumentProcessors">
    /// The argument Processors.
    /// </param>
    /// <param name="featureFlag">
    /// The feature flag support.
    /// </param>
    /// <remarks>
    /// This is not public because the static Create method should be used to access the instance.
    /// </remarks>
    protected ArgumentProcessorFactory(IEnumerable<IArgumentProcessor> argumentProcessors)
    {
        ValidateArg.NotNull(argumentProcessors, nameof(argumentProcessors));
        AllArgumentProcessors = argumentProcessors;
    }

    /// <summary>
    /// Creates ArgumentProcessorFactory.
    /// </summary>
    /// <param name="featureFlag">
    /// The feature flag support.
    /// </param>
    /// <returns>ArgumentProcessorFactory.</returns>
    internal static ArgumentProcessorFactory Create(IFeatureFlag? featureFlag = null)
    {
        var defaultArgumentProcessor = DefaultArgumentProcessors;

        if (!(featureFlag ?? FeatureFlag.Instance).IsSet(FeatureFlag.DISABLE_ARTIFACTS_POSTPROCESSING))
        {
            defaultArgumentProcessor.Add(new ArtifactProcessingCollectModeProcessor());
            defaultArgumentProcessor.Add(new ArtifactProcessingPostProcessModeProcessor());
            defaultArgumentProcessor.Add(new TestSessionCorrelationIdProcessor());
        }

        // Get the ArgumentProcessorFactory
        return new ArgumentProcessorFactory(defaultArgumentProcessor);
    }

    /// <summary>
    /// Returns all of the available argument processors.
    /// </summary>
    public IEnumerable<IArgumentProcessor> AllArgumentProcessors { get; }

    /// <summary>
    /// Gets a mapping between command and Argument Executor.
    /// </summary>
    internal Dictionary<string, IArgumentProcessor> CommandToProcessorMap
    {
        get
        {
            // Build the mapping if it does not already exist.
            if (_commandToProcessorMap == null)
            {
                BuildCommandMaps();
            }

            return _commandToProcessorMap;
        }
    }

    /// <summary>
    /// Gets a mapping between special commands and their Argument Processors.
    /// </summary>
    internal Dictionary<string, IArgumentProcessor> SpecialCommandToProcessorMap
    {
        get
        {
            // Build the mapping if it does not already exist.
            if (_specialCommandToProcessorMap == null)
            {
                BuildCommandMaps();
            }

            return _specialCommandToProcessorMap;
        }
    }

    /// <summary>
    /// Creates the argument processor associated with the provided command line argument.
    /// The Lazy that is returned will initialize the underlying argument processor when it is first accessed.
    /// </summary>
    /// <param name="argument">Command line argument to create the argument processor for.</param>
    /// <returns>The argument processor or null if one was not found.</returns>
    public IArgumentProcessor? CreateArgumentProcessor(string argument)
    {
        ValidateArg.NotNullOrWhiteSpace(argument, nameof(argument));

        // Parse the input into its command and argument parts.
        var pair = new CommandArgumentPair(argument);

        // Find the associated argument processor.
        CommandToProcessorMap.TryGetValue(pair.Command, out IArgumentProcessor? argumentProcessor);

        // If an argument processor was not found for the command, then consider it as a test source argument.
        if (argumentProcessor == null)
        {
            // Update the command pair since the command is actually the argument in the case of
            // a test source.
            pair = new CommandArgumentPair(TestSourceArgumentProcessor.CommandName, argument);

            argumentProcessor = SpecialCommandToProcessorMap[TestSourceArgumentProcessor.CommandName];
        }

        if (argumentProcessor != null)
        {
            argumentProcessor = WrapLazyProcessorToInitializeOnInstantiation(argumentProcessor, pair.Argument);
        }

        return argumentProcessor;
    }

    /// <summary>
    /// Creates the argument processor associated with the provided command line argument.
    /// The Lazy that is returned will initialize the underlying argument processor when it is first accessed.
    /// </summary>
    /// <param name="command">Command name of the argument processor.</param>
    /// <param name="arguments">Command line arguments to create the argument processor for.</param>
    /// <returns>The argument processor or null if one was not found.</returns>
    public IArgumentProcessor? CreateArgumentProcessor(string command, string[] arguments)
    {
        if (arguments == null || arguments.Length == 0)
        {
            throw new ArgumentException("Cannot be null or empty", nameof(arguments));
        }
        Contract.EndContractBlock();

        // Find the associated argument processor.
        CommandToProcessorMap.TryGetValue(command, out IArgumentProcessor? argumentProcessor);

        if (argumentProcessor != null)
        {
            argumentProcessor = WrapLazyProcessorToInitializeOnInstantiation(argumentProcessor, arguments);
        }

        return argumentProcessor;
    }

    /// <summary>
    /// Creates the default action argument processor.
    /// The Lazy that is returned will initialize the underlying argument processor when it is first accessed.
    /// </summary>
    /// <returns>The default action argument processor.</returns>
    public IArgumentProcessor CreateDefaultActionArgumentProcessor()
    {
        var argumentProcessor = SpecialCommandToProcessorMap[RunTestsArgumentProcessor.CommandName];
        return WrapLazyProcessorToInitializeOnInstantiation(argumentProcessor);
    }

    /// <summary>
    /// Gets the argument processors that are tagged as special and to be always executed.
    /// The Lazy's that are returned will initialize the underlying argument processor when first accessed.
    /// </summary>
    /// <returns>The argument processors that are tagged as special and to be always executed.</returns>
    public IEnumerable<IArgumentProcessor> GetArgumentProcessorsToAlwaysExecute()
    {
        return SpecialCommandToProcessorMap.Values
            .Where(lazyProcessor => lazyProcessor.Metadata.Value.IsSpecialCommand && lazyProcessor.Metadata.Value.AlwaysExecute);
    }

    private static IList<IArgumentProcessor> DefaultArgumentProcessors => new List<IArgumentProcessor> {
        new HelpArgumentProcessor(),
        new TestSourceArgumentProcessor(),
        new ListTestsArgumentProcessor(),
        new RunTestsArgumentProcessor(),
        new RunSpecificTestsArgumentProcessor(),
        new TestAdapterPathArgumentProcessor(),
        new TestAdapterLoadingStrategyArgumentProcessor(),
        new TestCaseFilterArgumentProcessor(),
        new ParentProcessIdArgumentProcessor(),
        new PortArgumentProcessor(),
        new RunSettingsArgumentProcessor(),
        new PlatformArgumentProcessor(),
        new FrameworkArgumentProcessor(),
        new EnableLoggerArgumentProcessor(),
        new ParallelArgumentProcessor(),
        new EnableDiagArgumentProcessor(),
        new CliRunSettingsArgumentProcessor(),
        new ResultsDirectoryArgumentProcessor(),
        new InIsolationArgumentProcessor(),
        new CollectArgumentProcessor(),
        new EnableCodeCoverageArgumentProcessor(),
        new DisableAutoFakesArgumentProcessor(),
        new ResponseFileArgumentProcessor(),
        new EnableBlameArgumentProcessor(),
        new AeDebuggerArgumentProcessor(),
        new UseVsixExtensionsArgumentProcessor(),
        new ListDiscoverersArgumentProcessor(),
        new ListExecutorsArgumentProcessor(),
        new ListLoggersArgumentProcessor(),
        new ListSettingsProvidersArgumentProcessor(),
        new ListFullyQualifiedTestsArgumentProcessor(),
        new ListTestsTargetPathArgumentProcessor(),
        new ShowDeprecateDotnetVStestMessageArgumentProcessor(),
        new EnvironmentArgumentProcessor()
    };

    /// <summary>
    /// Builds the command to processor map and special command to processor map.
    /// </summary>
    [MemberNotNull(nameof(_commandToProcessorMap), nameof(_specialCommandToProcessorMap))]
    private void BuildCommandMaps()
    {
        _commandToProcessorMap = new Dictionary<string, IArgumentProcessor>(StringComparer.OrdinalIgnoreCase);
        _specialCommandToProcessorMap = new Dictionary<string, IArgumentProcessor>(StringComparer.OrdinalIgnoreCase);

        foreach (IArgumentProcessor argumentProcessor in AllArgumentProcessors)
        {
            // Add the command to the appropriate dictionary.
            var processorsMap = argumentProcessor.Metadata.Value.IsSpecialCommand
                ? _specialCommandToProcessorMap
                : _commandToProcessorMap;

            string commandName = argumentProcessor.Metadata.Value.CommandName;
            processorsMap.Add(commandName, argumentProcessor);

            // Add xplat name for the command name
            commandName = string.Concat("--", commandName.Remove(0, 1));
            processorsMap.Add(commandName, argumentProcessor);

            if (!argumentProcessor.Metadata.Value.ShortCommandName.IsNullOrEmpty())
            {
                string shortCommandName = argumentProcessor.Metadata.Value.ShortCommandName;
                processorsMap.Add(shortCommandName, argumentProcessor);

                // Add xplat short name for the command name
                shortCommandName = shortCommandName.Replace('/', '-');
                processorsMap.Add(shortCommandName, argumentProcessor);
            }
        }
    }

    /// <summary>
    /// Decorates a lazy argument processor so that the real processor is initialized when the lazy value is obtained.
    /// </summary>
    /// <param name="processor">The lazy processor.</param>
    /// <param name="initArg">The argument with which the real processor should be initialized.</param>
    /// <returns>The decorated lazy processor.</returns>
    public static IArgumentProcessor WrapLazyProcessorToInitializeOnInstantiation(IArgumentProcessor processor, string? initArg = null)
    {
        var processorExecutor = processor.Executor;
        var lazyArgumentProcessor = new Lazy<IArgumentExecutor>(() =>
        {
            IArgumentExecutor? instance;
            try
            {
                instance = processorExecutor!.Value;
            }
            catch (Exception e)
            {
                EqtTrace.Error("ArgumentProcessorFactory.WrapLazyProcessorToInitializeOnInstantiation: Exception creating argument processor: {0}", e);
                throw;
            }

            try
            {
                instance.Initialize(initArg);
            }
            catch (Exception e)
            {
                EqtTrace.Error("ArgumentProcessorFactory.WrapLazyProcessorToInitializeOnInstantiation: Exception initializing argument processor: {0}", e);
                throw;
            }

            return instance;
        }, System.Threading.LazyThreadSafetyMode.PublicationOnly);
        processor.Executor = lazyArgumentProcessor;

        return processor;
    }

    /// <summary>
    /// Decorates a lazy argument processor so that the real processor is initialized when the lazy value is obtained.
    /// </summary>
    /// <param name="processor">The lazy processor.</param>
    /// <param name="initArg">The argument with which the real processor should be initialized.</param>
    /// <returns>The decorated lazy processor.</returns>
    private static IArgumentProcessor WrapLazyProcessorToInitializeOnInstantiation(
        IArgumentProcessor processor,
        string[] initArgs)
    {
        var processorExecutor = processor.Executor;
        var lazyArgumentProcessor = new Lazy<IArgumentExecutor>(() =>
        {
            IArgumentsExecutor? instance;
            try
            {
                instance = (IArgumentsExecutor)processorExecutor!.Value;
            }
            catch (Exception e)
            {
                EqtTrace.Error("ArgumentProcessorFactory.WrapLazyProcessorToInitializeOnInstantiation: Exception creating argument processor: {0}", e);
                throw;
            }

            try
            {
                instance.Initialize(initArgs);
            }
            catch (Exception e)
            {
                EqtTrace.Error("ArgumentProcessorFactory.WrapLazyProcessorToInitializeOnInstantiation: Exception initializing argument processor: {0}", e);
                throw;
            }

            return instance;
        }, System.Threading.LazyThreadSafetyMode.PublicationOnly);
        processor.Executor = lazyArgumentProcessor;

        return processor;
    }

}
