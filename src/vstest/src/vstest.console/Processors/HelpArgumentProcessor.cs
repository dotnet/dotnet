// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.Utilities;

using CommandLineResources = Microsoft.VisualStudio.TestPlatform.CommandLine.Resources.Resources;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;
// <summary>
//     Argument Executor for the "-?|--Help|/?|/Help" Help command line argument.
// </summary>
internal class HelpArgumentProcessor : IArgumentProcessor
{
    /// <summary>
    /// The name of the command line argument that the HelpArgumentExecutor handles.
    /// </summary>
    public const string CommandName = "/Help";

    /// <summary>
    /// The short name of the command line argument that the HelpArgumentExecutor handles.
    /// </summary>
    public const string ShortCommandName = "/?";


    private Lazy<IArgumentProcessorCapabilities>? _metadata;
    private Lazy<IArgumentExecutor>? _executor;

    /// <summary>
    /// Gets the metadata.
    /// </summary>
    public Lazy<IArgumentProcessorCapabilities> Metadata
        => _metadata ??= new Lazy<IArgumentProcessorCapabilities>(() => new HelpArgumentProcessorCapabilities());

    /// <summary>
    /// Gets or sets the executor.
    /// </summary>
    public Lazy<IArgumentExecutor>? Executor
    {
        get => _executor ??= new Lazy<IArgumentExecutor>(() => new HelpArgumentExecutor());

        set => _executor = value;
    }
}

/// <summary>
/// The help argument processor capabilities.
/// </summary>
internal class HelpArgumentProcessorCapabilities : BaseArgumentProcessorCapabilities
{
    public override string CommandName => HelpArgumentProcessor.CommandName;

    public override string ShortCommandName => HelpArgumentProcessor.ShortCommandName;

    public override string HelpContentResourceName => CommandLineResources.HelpArgumentHelp;

    public override HelpContentPriority HelpPriority => HelpContentPriority.HelpArgumentProcessorHelpPriority;

    public override bool IsAction => false;

    public override ArgumentProcessorPriority Priority => ArgumentProcessorPriority.Help;
}

/// <summary>
/// Argument Executor for the "/?" Help command line argument.
/// </summary>
internal class HelpArgumentExecutor : IArgumentExecutor
{
    /// <summary>
    /// Constructs the HelpArgumentExecutor
    /// </summary>
    public HelpArgumentExecutor()
    {
        Output = ConsoleOutput.Instance;
    }

    /// <summary>
    /// Gets the output object
    /// </summary>
    internal IOutput Output { get; set; }


    #region IArgumentExecutor Members

    public void Initialize(string? argument)
    {
    }

    public ArgumentProcessorResult Execute()
    {
        // Output the stock output text
        OutputSection(CommandLineResources.HelpUsageText);
        OutputSection(CommandLineResources.HelpDescriptionText);
        OutputSection(CommandLineResources.HelpArgumentsText);

        var argumentProcessorFactory = ArgumentProcessorFactory.Create();
        List<IArgumentProcessor> processors = new();
        processors.AddRange(argumentProcessorFactory.AllArgumentProcessors);
        processors.Sort((p1, p2) => Comparer<HelpContentPriority>.Default.Compare(p1.Metadata.Value.HelpPriority, p2.Metadata.Value.HelpPriority));

        // Output the help description for RunTestsArgumentProcessor
        IArgumentProcessor? runTestsArgumentProcessor = processors.Find(p1 => p1.GetType() == typeof(RunTestsArgumentProcessor));
        TPDebug.Assert(runTestsArgumentProcessor is not null, "runTestsArgumentProcessor is null");
        processors.Remove(runTestsArgumentProcessor);
        var helpDescription = LookupHelpDescription(runTestsArgumentProcessor);
        if (helpDescription != null)
        {
            OutputSection(helpDescription);
        }

        // Output the help description for each available argument processor
        OutputSection(CommandLineResources.HelpOptionsText);
        foreach (var argumentProcessor in processors)
        {
            helpDescription = LookupHelpDescription(argumentProcessor);
            if (helpDescription != null)
            {
                OutputSection(helpDescription);
            }
        }
        OutputSection(CommandLineResources.Examples);

        // When Help has finished abort any subsequent argument processor operations
        return ArgumentProcessorResult.Abort;
    }

    #endregion
    /// <summary>
    /// Lookup the help description for the argument processor.
    /// </summary>
    /// <param name="argumentProcessor">The argument processor for which to discover any help content</param>
    /// <returns>The formatted string containing the help description if found null otherwise</returns>
    private string? LookupHelpDescription(IArgumentProcessor argumentProcessor)
    {
        string? result = null;

        if (argumentProcessor.Metadata.Value.HelpContentResourceName != null)
        {
            try
            {
                result = argumentProcessor.Metadata.Value.HelpContentResourceName;
                //ResourceHelper.GetString(argumentProcessor.Metadata.HelpContentResourceName, assembly, CultureInfo.CurrentUICulture);
            }
            catch (Exception e)
            {
                Output.Warning(false, e.Message);
            }
        }

        return result;
    }

    /// <summary>
    /// Output a section followed by an empty line.
    /// </summary>
    /// <param name="message">Message to output.</param>
    private void OutputSection(string message)
    {
        Output.WriteLine(message, OutputLevel.Information);
        Output.WriteLine(string.Empty, OutputLevel.Information);
    }

}
