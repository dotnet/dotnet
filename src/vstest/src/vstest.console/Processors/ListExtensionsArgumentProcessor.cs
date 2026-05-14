// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;

using Microsoft.VisualStudio.TestPlatform.Client;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using Microsoft.VisualStudio.TestPlatform.Common.SettingsProvider;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.Utilities;

using CommandLineResources = Microsoft.VisualStudio.TestPlatform.CommandLine.Resources.Resources;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;

internal class ListExtensionsArgumentProcessorCapabilities : BaseArgumentProcessorCapabilities
{
    private readonly string _commandName;

    /// <summary>
    /// Initializes a new instance of the <see cref="ListExtensionsArgumentProcessorCapabilities"/> class.
    /// </summary>
    /// <param name="commandName">Name of the command</param>
    public ListExtensionsArgumentProcessorCapabilities(string commandName)
    {
        _commandName = commandName;
    }

    public override string CommandName => _commandName;

    /// <inheritdoc />
    public override bool AllowMultiple => false;

    /// <inheritdoc />
    public override bool IsAction => true;
}

internal abstract class ListExtensionsArgumentProcessor : IArgumentProcessor
{
    private Lazy<IArgumentProcessorCapabilities>? _metadata;
    private Lazy<IArgumentExecutor>? _executor;
    private readonly Func<IArgumentExecutor> _getExecutor;
    private readonly Func<IArgumentProcessorCapabilities> _getCapabilities;

    public ListExtensionsArgumentProcessor(Func<IArgumentExecutor> getExecutor, Func<IArgumentProcessorCapabilities> getCapabilities)
    {
        _getExecutor = getExecutor;
        _getCapabilities = getCapabilities;
    }

    public Lazy<IArgumentExecutor>? Executor
    {
        get => _executor ??= new Lazy<IArgumentExecutor>(_getExecutor);

        set => _executor = value;
    }

    public Lazy<IArgumentProcessorCapabilities> Metadata
        => _metadata ??= new Lazy<IArgumentProcessorCapabilities>(_getCapabilities);
}

#region List discoverers
/// <summary>
/// Argument Executor for the "/ListDiscoverers" command line argument.
/// </summary>
internal class ListDiscoverersArgumentProcessor : ListExtensionsArgumentProcessor
{
    private const string CommandName = "/ListDiscoverers";

    public ListDiscoverersArgumentProcessor()
        : base(() => new ListDiscoverersArgumentExecutor(), () => new ListExtensionsArgumentProcessorCapabilities(CommandName))
    {
    }
}

internal class ListDiscoverersArgumentExecutor : IArgumentExecutor
{
    public void Initialize(string? argument)
    {
    }

    public ArgumentProcessorResult Execute()
    {
        ConsoleOutput.Instance.WriteLine(CommandLineResources.AvailableDiscoverersHeaderMessage, OutputLevel.Information);
        _ = TestPlatformFactory.GetTestPlatform();
        var extensionManager = TestDiscoveryExtensionManager.Create();
        foreach (var extension in extensionManager.Discoverers)
        {
            ConsoleOutput.Instance.WriteLine(extension.Value.GetType().FullName, OutputLevel.Information);
            ConsoleOutput.Instance.WriteLine(string.Format(CultureInfo.CurrentCulture, CommandLineResources.UriOfDefaultExecutor, extension.Metadata.DefaultExecutorUri), OutputLevel.Information);
            ConsoleOutput.Instance.WriteLine(string.Format(CultureInfo.CurrentCulture, CommandLineResources.SupportedFileTypesIndicator + " " + string.Join(", ", extension.Metadata.FileExtension!)), OutputLevel.Information);
        }

        return ArgumentProcessorResult.Success;
    }
}
#endregion

#region List executors
/// <summary>
/// Argument Executor for the "/ListExecutors" command line argument.
/// </summary>
internal class ListExecutorsArgumentProcessor : ListExtensionsArgumentProcessor
{
    private const string CommandName = "/ListExecutors";

    public ListExecutorsArgumentProcessor()
        : base(() => new ListExecutorsArgumentExecutor(), () => new ListExtensionsArgumentProcessorCapabilities(CommandName))
    {
    }
}

internal class ListExecutorsArgumentExecutor : IArgumentExecutor
{
    public void Initialize(string? argument)
    {
    }

    public ArgumentProcessorResult Execute()
    {
        ConsoleOutput.Instance.WriteLine(CommandLineResources.AvailableExecutorsHeaderMessage, OutputLevel.Information);
        _ = TestPlatformFactory.GetTestPlatform();
        var extensionManager = TestExecutorExtensionManager.Create();
        foreach (var extension in extensionManager.TestExtensions)
        {
            ConsoleOutput.Instance.WriteLine(extension.Value.GetType().FullName, OutputLevel.Information);
            ConsoleOutput.Instance.WriteLine(string.Format(CultureInfo.CurrentCulture, CommandLineResources.AvailableExtensionsMetadataFormat, "Uri", extension.Metadata.ExtensionUri), OutputLevel.Information);
        }

        return ArgumentProcessorResult.Success;
    }
}
#endregion

#region List loggers
/// <summary>
/// Argument Executor for the "/ListLoggers" command line argument.
/// </summary>
internal class ListLoggersArgumentProcessor : ListExtensionsArgumentProcessor
{
    private const string CommandName = "/ListLoggers";

    public ListLoggersArgumentProcessor()
        : base(() => new ListLoggersArgumentExecutor(), () => new ListExtensionsArgumentProcessorCapabilities(CommandName))
    {
    }
}

internal class ListLoggersArgumentExecutor : IArgumentExecutor
{
    public void Initialize(string? argument)
    {
    }

    public ArgumentProcessorResult Execute()
    {
        ConsoleOutput.Instance.WriteLine(CommandLineResources.AvailableLoggersHeaderMessage, OutputLevel.Information);
        _ = TestPlatformFactory.GetTestPlatform();
        var extensionManager = TestLoggerExtensionManager.Create(new NullMessageLogger());
        foreach (var extension in extensionManager.TestExtensions)
        {
            ConsoleOutput.Instance.WriteLine(extension.Value.GetType().FullName, OutputLevel.Information);
            ConsoleOutput.Instance.WriteLine(string.Format(CultureInfo.CurrentCulture, CommandLineResources.AvailableExtensionsMetadataFormat, "Uri", extension.Metadata.ExtensionUri), OutputLevel.Information);
            ConsoleOutput.Instance.WriteLine(string.Format(CultureInfo.CurrentCulture, CommandLineResources.AvailableExtensionsMetadataFormat, "FriendlyName", string.Join(", ", extension.Metadata.FriendlyName)), OutputLevel.Information);
        }

        return ArgumentProcessorResult.Success;
    }

    private class NullMessageLogger : IMessageLogger
    {
        public void SendMessage(TestMessageLevel testMessageLevel, string message)
        {
        }
    }
}
#endregion

#region List settings providers
/// <summary>
/// Argument Executor for the "/ListSettingsProviders" command line argument.
/// </summary>
internal class ListSettingsProvidersArgumentProcessor : ListExtensionsArgumentProcessor
{
    private const string CommandName = "/ListSettingsProviders";

    public ListSettingsProvidersArgumentProcessor()
        : base(() => new ListSettingsProvidersArgumentExecutor(), () => new ListExtensionsArgumentProcessorCapabilities(CommandName))
    {
    }
}

internal class ListSettingsProvidersArgumentExecutor : IArgumentExecutor
{
    public void Initialize(string? argument)
    {
    }

    public ArgumentProcessorResult Execute()
    {
        ConsoleOutput.Instance.WriteLine(CommandLineResources.AvailableSettingsProvidersHeaderMessage, OutputLevel.Information);
        _ = TestPlatformFactory.GetTestPlatform();
        var extensionManager = SettingsProviderExtensionManager.Create();
        foreach (var extension in extensionManager.SettingsProvidersMap.Values)
        {
            ConsoleOutput.Instance.WriteLine(extension.Value.GetType().FullName, OutputLevel.Information);
            ConsoleOutput.Instance.WriteLine(string.Format(CultureInfo.CurrentCulture, CommandLineResources.AvailableExtensionsMetadataFormat, "SettingName", extension.Metadata.SettingsName), OutputLevel.Information);
        }

        return ArgumentProcessorResult.Success;
    }
}
#endregion
