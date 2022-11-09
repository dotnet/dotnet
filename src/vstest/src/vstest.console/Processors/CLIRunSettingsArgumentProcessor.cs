// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Xml.XPath;

using Microsoft.VisualStudio.TestPlatform.Common;
using Microsoft.VisualStudio.TestPlatform.Common.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;

using CommandLineResources = Microsoft.VisualStudio.TestPlatform.CommandLine.Resources.Resources;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;

/// <summary>
/// The argument processor for runsettings passed as argument through cli
/// </summary>
internal class CliRunSettingsArgumentProcessor : IArgumentProcessor
{
    /// <summary>
    /// The name of the command line argument that the PortArgumentExecutor handles.
    /// </summary>
    public const string CommandName = "--";

    private Lazy<IArgumentProcessorCapabilities>? _metadata;
    private Lazy<IArgumentExecutor>? _executor;

    /// <summary>
    /// Gets the metadata.
    /// </summary>
    public Lazy<IArgumentProcessorCapabilities> Metadata
        => _metadata ??= new Lazy<IArgumentProcessorCapabilities>(() =>
            new CliRunSettingsArgumentProcessorCapabilities());

    /// <summary>
    /// Gets or sets the executor.
    /// </summary>
    public Lazy<IArgumentExecutor>? Executor
    {
        get => _executor ??= new Lazy<IArgumentExecutor>(() =>
            new CliRunSettingsArgumentExecutor(RunSettingsManager.Instance, CommandLineOptions.Instance));

        set => _executor = value;
    }
}

internal class CliRunSettingsArgumentProcessorCapabilities : BaseArgumentProcessorCapabilities
{
    public override string CommandName => CliRunSettingsArgumentProcessor.CommandName;

    public override bool AllowMultiple => false;

    public override bool IsAction => false;

    public override ArgumentProcessorPriority Priority => ArgumentProcessorPriority.CliRunSettings;

    public override string HelpContentResourceName => CommandLineResources.CLIRunSettingsArgumentHelp;

    public override HelpContentPriority HelpPriority => HelpContentPriority.CliRunSettingsArgumentProcessorHelpPriority;
}

internal class CliRunSettingsArgumentExecutor : IArgumentsExecutor
{
    private readonly IRunSettingsProvider _runSettingsManager;
    private readonly CommandLineOptions _commandLineOptions;

    internal CliRunSettingsArgumentExecutor(IRunSettingsProvider runSettingsManager, CommandLineOptions commandLineOptions)
    {
        _runSettingsManager = runSettingsManager;
        _commandLineOptions = commandLineOptions;
    }

    public void Initialize(string? argument)
    {
        throw new NotImplementedException();
    }

    public void Initialize(string[]? arguments)
    {
        // if argument is null or doesn't contain any element, don't do anything.
        if (arguments == null || arguments.Length == 0)
        {
            return;
        }

        Contract.EndContractBlock();

        // Load up the run settings and set it as the active run settings.
        try
        {
            // Append / Override run settings supplied in CLI
            CreateOrOverwriteRunSettings(_runSettingsManager, arguments);
        }
        catch (XPathException exception)
        {
            throw new CommandLineException(CommandLineResources.MalformedRunSettingsKey, exception);
        }
        catch (SettingsException exception)
        {
            throw new CommandLineException(exception.Message, exception);
        }
    }

    public ArgumentProcessorResult Execute()
    {
        // Nothing to do here, the work was done in initialization.
        return ArgumentProcessorResult.Success;
    }

    private void CreateOrOverwriteRunSettings(IRunSettingsProvider runSettingsProvider, string[] args)
    {
        var mergedArgs = new List<string>();
        var mergedArg = string.Empty;
        var merge = false;

        foreach (var arg in args)
        {
            // when we see that the parameter begins with TestRunParameters
            // but does not end with ") we start merging the params
            if (arg.StartsWith("TestRunParameters", StringComparison.OrdinalIgnoreCase))
            {
                if (arg.EndsWith("\")"))
                {
                    // this parameter is complete
                    mergedArgs.Add(arg);
                }
                else
                {
                    // this parameter needs merging
                    merge = true;
                }
            }

            // we merge as long as the flag is set
            // hoping that we find the end of the parameter
            if (merge)
            {
                mergedArg += StringUtils.IsNullOrWhiteSpace(mergedArg) ? arg : $" {arg}";
            }
            else
            {
                // if we are not merging just pass the param as is
                mergedArgs.Add(arg);
            }

            // once we detect the end we add the whole parameter to the args
            if (merge && arg.EndsWith("\")"))
            {
                mergedArgs.Add(mergedArg);
                mergedArg = string.Empty;
                merge = false;
            }
        }

        if (merge)
        {
            // we tried to merge but never found the end of that
            // test paramter, add what we merged up until now
            mergedArgs.Add(mergedArg);
        }


        var length = mergedArgs.Count;

        for (int index = 0; index < length; index++)
        {
            var arg = mergedArgs[index];

            if (UpdateTestRunParameterNode(runSettingsProvider, arg))
            {
                continue;
            }

            var indexOfSeparator = arg.IndexOf("=");

            if (indexOfSeparator <= 0 || indexOfSeparator >= arg.Length - 1)
            {
                continue;
            }

            var key = arg.Substring(0, indexOfSeparator).Trim();
            var value = arg.Substring(indexOfSeparator + 1);

            if (StringUtils.IsNullOrWhiteSpace(key))
            {
                continue;
            }

            // To determine whether to infer framework and platform.
            UpdateFrameworkAndPlatform(key, value);

            runSettingsProvider.UpdateRunSettingsNode(key, value);
        }
    }

    private static bool UpdateTestRunParameterNode(IRunSettingsProvider runSettingsProvider, string node)
    {
        if (!node.Contains(Constants.TestRunParametersName))
        {
            return false;
        }

        var match = runSettingsProvider.GetTestRunParameterNodeMatch(node);

        if (string.Compare(match.Value, node) == 0)
        {
            runSettingsProvider.UpdateTestRunParameterSettingsNode(match);
            return true;
        }

        var exceptionMessage = string.Format(CultureInfo.CurrentCulture, CommandLineResources.InvalidTestRunParameterArgument, node);
        throw new CommandLineException(exceptionMessage);
    }

    private void UpdateFrameworkAndPlatform(string key, string value)
    {
        if (key.Equals(FrameworkArgumentExecutor.RunSettingsPath))
        {
            Framework? framework = Framework.FromString(value);
            if (framework != null)
            {
                _commandLineOptions.TargetFrameworkVersion = framework;
            }
        }

        if (key.Equals(PlatformArgumentExecutor.RunSettingsPath))
        {
            bool success = Enum.TryParse<Architecture>(value, true, out var architecture);
            if (success)
            {
                RunSettingsHelper.Instance.IsDefaultTargetArchitecture = false;
                _commandLineOptions.TargetArchitecture = architecture;
            }
        }
    }
}
