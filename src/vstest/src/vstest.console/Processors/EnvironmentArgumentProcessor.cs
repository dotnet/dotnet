// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.Common;
using Microsoft.VisualStudio.TestPlatform.Common.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestPlatform.Utilities;

using CommandLineResources = Microsoft.VisualStudio.TestPlatform.CommandLine.Resources.Resources;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;

/// <summary>
/// Argument Executor for the "-e|--Environment|/e|/Environment" command line argument.
/// </summary>
internal class EnvironmentArgumentProcessor : IArgumentProcessor
{
    /// <summary>
    /// The short name of the command line argument that the EnvironmentArgumentProcessor handles.
    /// </summary>
    public const string ShortCommandName = "/e";

    /// <summary>
    /// The name of the command line argument that the EnvironmentArgumentProcessor handles.
    /// </summary>
    public const string CommandName = "/Environment";
    private Lazy<IArgumentProcessorCapabilities>? _metadata;
    private Lazy<IArgumentExecutor>? _executor;

    public Lazy<IArgumentExecutor>? Executor
    {
        get => _executor ??= new Lazy<IArgumentExecutor>(() =>
            new ArgumentExecutor(CommandLineOptions.Instance, RunSettingsManager.Instance, ConsoleOutput.Instance));

        set => _executor = value;
    }

    public Lazy<IArgumentProcessorCapabilities> Metadata
        => _metadata ??= new Lazy<IArgumentProcessorCapabilities>(() => new ArgumentProcessorCapabilities());

    internal class ArgumentProcessorCapabilities : BaseArgumentProcessorCapabilities
    {
        public override string CommandName => EnvironmentArgumentProcessor.CommandName;
        public override string ShortCommandName => EnvironmentArgumentProcessor.ShortCommandName;
        public override bool AllowMultiple => true;
        public override bool IsAction => false;
        public override string HelpContentResourceName => CommandLineResources.EnvironmentArgumentHelp;
        public override ArgumentProcessorPriority Priority => ArgumentProcessorPriority.Normal;
        public override HelpContentPriority HelpPriority => HelpContentPriority.EnvironmentArgumentProcessorHelpPriority;
    }

    internal class ArgumentExecutor : IArgumentExecutor
    {
        /// <summary>
        /// Used when warning about overridden environment variables.
        /// </summary>
        private readonly IOutput _output;

        /// <summary>
        /// Used when setting Environment variables.
        /// </summary>
        private readonly IRunSettingsProvider _runSettingsProvider;

        /// <summary>
        /// Used when checking and forcing InIsolation mode.
        /// </summary>
        private readonly CommandLineOptions _commandLineOptions;
        public ArgumentExecutor(CommandLineOptions commandLineOptions, IRunSettingsProvider runSettingsProvider, IOutput output)
        {
            _commandLineOptions = commandLineOptions;
            _output = output;
            _runSettingsProvider = runSettingsProvider;
        }

        /// <summary>
        /// Set the environment variables in RunSettings.xml
        /// </summary>
        /// <param name="argument">
        /// Environment variable to set.
        /// </param>
        public void Initialize(string? argument)
        {
            TPDebug.Assert(!StringUtils.IsNullOrWhiteSpace(argument));
            TPDebug.Assert(_output != null);
            TPDebug.Assert(_commandLineOptions != null);
            TPDebug.Assert(!StringUtils.IsNullOrWhiteSpace(_runSettingsProvider.ActiveRunSettings?.SettingsXml));

            var key = argument;
            var value = string.Empty;

            if (key.Contains("="))
            {
                value = key.Substring(key.IndexOf("=") + 1);
                key = key.Substring(0, key.IndexOf("="));
            }

            var node = _runSettingsProvider.QueryRunSettingsNode($"RunConfiguration.EnvironmentVariables.{key}");
            if (node != null)
            {
                _output.Warning(true, CommandLineResources.EnvironmentVariableXIsOverriden, key);
            }

            _runSettingsProvider.UpdateRunSettingsNode($"RunConfiguration.EnvironmentVariables.{key}", value);

            if (!_commandLineOptions.InIsolation)
            {
                _commandLineOptions.InIsolation = true;
                _runSettingsProvider.UpdateRunSettingsNode(InIsolationArgumentExecutor.RunSettingsPath, "true");
            }
        }

        // Nothing to do here, the work was done in initialization.
        public ArgumentProcessorResult Execute() => ArgumentProcessorResult.Success;
    }
}
