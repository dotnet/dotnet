// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.Common;
using Microsoft.VisualStudio.TestPlatform.Common.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;

using CommandLineResources = Microsoft.VisualStudio.TestPlatform.CommandLine.Resources.Resources;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;

/// <summary>
/// Allows the user to specify a path to load custom adapters from.
/// </summary>
internal class TestAdapterPathArgumentProcessor : IArgumentProcessor
{
    /// <summary>
    /// The name of the command line argument that the ListTestsArgumentExecutor handles.
    /// </summary>
    public const string CommandName = "/TestAdapterPath";

    private Lazy<IArgumentProcessorCapabilities>? _metadata;
    private Lazy<IArgumentExecutor>? _executor;

    /// <summary>
    /// Gets the metadata.
    /// </summary>
    public Lazy<IArgumentProcessorCapabilities> Metadata
        => _metadata ??= new Lazy<IArgumentProcessorCapabilities>(() =>
            new TestAdapterPathArgumentProcessorCapabilities());

    /// <summary>
    /// Gets or sets the executor.
    /// </summary>
    public Lazy<IArgumentExecutor>? Executor
    {
        get => _executor ??= new Lazy<IArgumentExecutor>(() =>
            new TestAdapterPathArgumentExecutor(CommandLineOptions.Instance, RunSettingsManager.Instance,
                ConsoleOutput.Instance, new FileHelper()));

        set => _executor = value;
    }
}

/// <summary>
/// The argument capabilities.
/// </summary>
internal class TestAdapterPathArgumentProcessorCapabilities : BaseArgumentProcessorCapabilities
{
    public override string CommandName => TestAdapterPathArgumentProcessor.CommandName;

    public override bool AllowMultiple => true;

    public override bool IsAction => false;

    public override ArgumentProcessorPriority Priority => ArgumentProcessorPriority.TestAdapterPath;

    public override string HelpContentResourceName => CommandLineResources.TestAdapterPathHelp;

    public override HelpContentPriority HelpPriority => HelpContentPriority.TestAdapterPathArgumentProcessorHelpPriority;
}

/// <summary>
/// The argument executor.
/// </summary>
internal class TestAdapterPathArgumentExecutor : IArgumentExecutor
{
    /// <summary>
    /// Used for getting sources.
    /// </summary>
    private readonly CommandLineOptions _commandLineOptions;

    /// <summary>
    /// Run settings provider.
    /// </summary>
    private readonly IRunSettingsProvider _runSettingsManager;

    /// <summary>
    /// Used for sending output.
    /// </summary>
    private readonly IOutput _output;

    /// <summary>
    /// For file related operation
    /// </summary>
    private readonly IFileHelper _fileHelper;

    /// <summary>
    /// Separators for multiple paths in argument.
    /// </summary>
    internal readonly static char[] ArgumentSeparators = new[] { ';' };

    public const string RunSettingsPath = "RunConfiguration.TestAdaptersPaths";

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="options"> The options. </param>
    /// <param name="testPlatform">The test platform</param>
    public TestAdapterPathArgumentExecutor(CommandLineOptions options, IRunSettingsProvider runSettingsManager, IOutput output, IFileHelper fileHelper)
    {
        _commandLineOptions = options ?? throw new ArgumentNullException(nameof(options));
        _runSettingsManager = runSettingsManager ?? throw new ArgumentNullException(nameof(runSettingsManager));
        _output = output ?? throw new ArgumentNullException(nameof(output));
        _fileHelper = fileHelper ?? throw new ArgumentNullException(nameof(fileHelper));
    }

    #region IArgumentExecutor

    /// <summary>
    /// Initializes with the argument that was provided with the command.
    /// </summary>
    /// <param name="argument">Argument that was provided with the command.</param>
    public void Initialize(string? argument)
    {
        if (argument.IsNullOrWhiteSpace())
        {
            throw new CommandLineException(
                string.Format(CultureInfo.CurrentCulture, CommandLineResources.TestAdapterPathValueRequired));
        }

        var testAdapterPaths = new List<string>();

        // VSTS task add double quotes around TestAdapterpath. For example if user has given TestAdapter path C:\temp,
        // Then VSTS task will add TestAdapterPath as "/TestAdapterPath:\"C:\Temp\"".
        // Remove leading and trailing ' " ' chars...
        argument = argument.Trim().Trim(new char[] { '\"' });

        // Get test adapter paths from RunSettings.
        var testAdapterPathsInRunSettings = _runSettingsManager.QueryRunSettingsNode(RunSettingsPath);

        if (!testAdapterPathsInRunSettings.IsNullOrWhiteSpace())
        {
            testAdapterPaths.AddRange(SplitPaths(testAdapterPathsInRunSettings));
        }

        testAdapterPaths.AddRange(SplitPaths(argument));
        var customAdaptersPath = testAdapterPaths.Distinct().ToArray();

        _runSettingsManager.UpdateRunSettingsNode(RunSettingsPath, string.Join(";", customAdaptersPath));
        _commandLineOptions.TestAdapterPath = customAdaptersPath;
    }

    /// <summary>
    /// Executes the argument processor.
    /// </summary>
    /// <returns> The <see cref="ArgumentProcessorResult"/>. </returns>
    public ArgumentProcessorResult Execute()
    {
        // Nothing to do since we updated the parameter during initialize parameter
        return ArgumentProcessorResult.Success;
    }
    #endregion

    /// <summary>
    /// Splits provided paths into array.
    /// </summary>
    /// <param name="paths">Source paths joined by semicolons.</param>
    /// <returns>Paths.</returns>
    internal static string[] SplitPaths(string? paths)
    {
        return paths.IsNullOrWhiteSpace() ? new string[0] : paths.Split(ArgumentSeparators, StringSplitOptions.RemoveEmptyEntries);
    }
}
