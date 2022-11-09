// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.CommandLine.Internal;
using Microsoft.VisualStudio.TestPlatform.CommandLine.Processors.Utilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;

using CommandLineResources = Microsoft.VisualStudio.TestPlatform.CommandLine.Resources.Resources;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;

internal class EnableDiagArgumentProcessor : IArgumentProcessor
{
    /// <summary>
    /// The name of the command line argument that the ListTestsArgumentExecutor handles.
    /// </summary>
    public const string CommandName = "/Diag";

    private readonly IFileHelper _fileHelper;

    private Lazy<IArgumentProcessorCapabilities>? _metadata;
    private Lazy<IArgumentExecutor>? _executor;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnableDiagArgumentProcessor"/> class.
    /// </summary>
    public EnableDiagArgumentProcessor() : this(new FileHelper())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EnableDiagArgumentProcessor"/> class.
    /// </summary>
    /// <param name="fileHelper">A file helper instance.</param>
    protected EnableDiagArgumentProcessor(IFileHelper fileHelper)
    {
        _fileHelper = fileHelper;
    }

    public Lazy<IArgumentProcessorCapabilities> Metadata
        => _metadata ??= new Lazy<IArgumentProcessorCapabilities>(() =>
            new EnableDiagArgumentProcessorCapabilities());

    /// <summary>
    /// Gets or sets the executor.
    /// </summary>
    public Lazy<IArgumentExecutor>? Executor
    {
        get => _executor ??= new Lazy<IArgumentExecutor>(() => new EnableDiagArgumentExecutor(_fileHelper, new ProcessHelper()));

        set => _executor = value;
    }
}

/// <summary>
/// The argument capabilities.
/// </summary>
internal class EnableDiagArgumentProcessorCapabilities : BaseArgumentProcessorCapabilities
{
    public override string CommandName => EnableDiagArgumentProcessor.CommandName;

    public override bool AllowMultiple => false;

    public override bool IsAction => false;

    public override ArgumentProcessorPriority Priority => ArgumentProcessorPriority.Diag;

    public override string HelpContentResourceName => CommandLineResources.EnableDiagUsage;

    public override HelpContentPriority HelpPriority => HelpContentPriority.EnableDiagArgumentProcessorHelpPriority;
}

/// <summary>
/// The argument executor.
/// </summary>
internal class EnableDiagArgumentExecutor : IArgumentExecutor
{
    private readonly IFileHelper _fileHelper;
    private readonly IProcessHelper _processHelper;

    /// <summary>
    /// Parameter for trace level
    /// </summary>
    public const string TraceLevelParam = "tracelevel";

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="fileHelper">The file helper.</param>
    public EnableDiagArgumentExecutor(IFileHelper fileHelper, IProcessHelper processHelper)
    {
        _fileHelper = fileHelper;
        _processHelper = processHelper;
    }

    #region IArgumentExecutor

    /// <summary>
    /// Initializes with the argument that was provided with the command.
    /// </summary>
    /// <param name="argument">Argument that was provided with the command.</param>
    public void Initialize(string? argument)
    {
        string exceptionMessage = string.Format(CultureInfo.CurrentCulture, CommandLineResources.InvalidDiagArgument, argument);

        // Throw error if argument is null or empty.
        if (argument.IsNullOrWhiteSpace())
        {
            throw new CommandLineException(exceptionMessage);
        }

        // Get diag argument list.
        var diagArgumentList = ArgumentProcessorUtilities.GetArgumentList(argument, ArgumentProcessorUtilities.SemiColonArgumentSeparator, exceptionMessage);

        // Get diag file path.
        // Note: Even though semi colon is valid file path, we are not respecting the file name having semi-colon [As we are separating arguments based on semi colon].
        var diagFilePathArg = diagArgumentList[0];
        var diagFilePath = GetDiagFilePath(diagFilePathArg);

        // Get diag parameters.
        var diagParameterArgs = diagArgumentList.Skip(1);
        var diagParameters = ArgumentProcessorUtilities.GetArgumentParameters(diagParameterArgs, ArgumentProcessorUtilities.EqualNameValueSeparator, exceptionMessage);

        // Initialize diag logging.
        InitializeDiagLogging(diagFilePath, diagParameters);

        // Write version to the log here, because that is the
        // first place where we know if we log or not.
        EqtTrace.Verbose($"Version: {Product.Version} Current process architecture: {_processHelper.GetCurrentProcessArchitecture()}");
        // https://docs.microsoft.com/en-us/dotnet/api/system.reflection.assembly.location?view=net-6.0#remarks
        // In .NET 5 and later versions, for bundled assemblies, the value returned is an empty string.
        string objectTypeLocation = typeof(object).Assembly.Location;
        if (!objectTypeLocation.IsNullOrEmpty())
        {
            EqtTrace.Verbose($"Runtime location: {Path.GetDirectoryName(objectTypeLocation)}");
        }
    }

    /// <summary>
    /// Executes the argument processor.
    /// </summary>
    /// <returns>The <see cref="ArgumentProcessorResult"/>.</returns>
    public ArgumentProcessorResult Execute()
    {
        // Nothing to do since we updated the parameter during initialize parameter
        return ArgumentProcessorResult.Success;
    }

    /// <summary>
    /// Initialize diag logging.
    /// </summary>
    /// <param name="diagFilePath">Diag file path.</param>
    /// <param name="diagParameters">Diag parameters</param>
    private static void InitializeDiagLogging(string diagFilePath, Dictionary<string, string> diagParameters)
    {
        // Get trace level from diag parameters.
        var traceLevel = GetDiagTraceLevel(diagParameters);

        // Initialize trace.
        // Trace initialized is false in case of any exception at time of initialization like Catch exception(UnauthorizedAccessException, PathTooLongException...)
        var traceInitialized = EqtTrace.InitializeTrace(diagFilePath, traceLevel);

        // Show console warning in case trace is not initialized.
        if (!traceInitialized && !StringUtils.IsNullOrEmpty(EqtTrace.ErrorOnInitialization))
        {
            ConsoleOutput.Instance.Warning(false, EqtTrace.ErrorOnInitialization);
        }
    }

    /// <summary>
    /// Gets diag trace level.
    /// </summary>
    /// <param name="diagParameters">Diag parameters.</param>
    /// <returns>Diag trace level.</returns>
    private static PlatformTraceLevel GetDiagTraceLevel(Dictionary<string, string> diagParameters)
    {
        // If diag parameters is null, set value of trace level as verbose.
        if (diagParameters == null)
        {
            return PlatformTraceLevel.Verbose;
        }

        // Get trace level from diag parameters.
        var traceLevelExists = diagParameters.TryGetValue(TraceLevelParam, out var traceLevelStr);
        if (traceLevelExists && Enum.TryParse(traceLevelStr, true, out PlatformTraceLevel traceLevel))
        {
            return traceLevel;
        }

        // Default value of diag trace level is verbose.
        return PlatformTraceLevel.Verbose;
    }

    /// <summary>
    /// Gets diag file path.
    /// </summary>
    /// <param name="diagFilePathArgument">Diag file path argument.</param>
    /// <returns>Diag file path.</returns>
    private string GetDiagFilePath(string diagFilePathArgument)
    {
        // Remove double quotes if present.
        diagFilePathArgument = diagFilePathArgument.Replace("\"", "");

        // If we provide a directory we don't need to create the base directory.
        if (!diagFilePathArgument.EndsWith(@"\") && !diagFilePathArgument.EndsWith("/"))
        {
            // Create base directory for diag file path (if doesn't exist)
            CreateDirectoryIfNotExists(diagFilePathArgument);
        }

        // return full diag file path. (This is done so that vstest and testhost create logs at same location.)
        return Path.GetFullPath(diagFilePathArgument);
    }

    /// <summary>
    /// Create directory if not exists.
    /// </summary>
    /// <param name="filePath">File path.</param>
    private void CreateDirectoryIfNotExists(string filePath)
    {
        // Create the base directory of file path if doesn't exist.
        // Directory could be empty if just a filename is provided. E.g. log.txt
        var directory = Path.GetDirectoryName(filePath);
        if (!StringUtils.IsNullOrEmpty(directory) && !_fileHelper.DirectoryExists(directory))
        {
            _fileHelper.CreateDirectory(directory);
        }
    }

    #endregion
}
