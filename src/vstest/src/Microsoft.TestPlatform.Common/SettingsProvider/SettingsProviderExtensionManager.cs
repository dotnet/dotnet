// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;

using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework.Utilities;
using Microsoft.VisualStudio.TestPlatform.Common.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Common.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

using CommonResources = Microsoft.VisualStudio.TestPlatform.Common.Resources.Resources;
using ObjectModelCommonResources = Microsoft.VisualStudio.TestPlatform.ObjectModel.Resources.CommonResources;

namespace Microsoft.VisualStudio.TestPlatform.Common.SettingsProvider;

/// <summary>
/// Manages the settings provider extensions.
/// </summary>
/// <remarks>
/// This is a non-shared instance because we want different settings provider instances to
/// be used for each run settings instance.
/// </remarks>
public class SettingsProviderExtensionManager
{
    private static SettingsProviderExtensionManager? s_settingsProviderExtensionManager;
    private static readonly object Synclock = new();

    /// <summary>
    /// The settings providers which are available.
    /// </summary>
    private readonly IEnumerable<LazyExtension<ISettingsProvider, ISettingsProviderCapabilities>> _settingsProviders;

    /// <summary>
    /// Used for logging errors.
    /// </summary>
    private readonly IMessageLogger _logger;

    /// <summary>
    /// Initializes with the settings providers.
    /// </summary>
    /// <remarks>
    /// The settings providers are imported as non-shared because we need different settings provider
    /// instances to be used for each run settings.
    /// </remarks>
    protected SettingsProviderExtensionManager(
        IEnumerable<LazyExtension<ISettingsProvider, ISettingsProviderCapabilities>> settingsProviders,
        IEnumerable<LazyExtension<ISettingsProvider, Dictionary<string, object>>> unfilteredSettingsProviders,
        IMessageLogger logger)
    {
        _settingsProviders = settingsProviders ?? throw new ArgumentNullException(nameof(settingsProviders));
        UnfilteredSettingsProviders = unfilteredSettingsProviders ?? throw new ArgumentNullException(nameof(unfilteredSettingsProviders));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        // Populate the map to avoid threading issues
        SettingsProvidersMap = new Dictionary<string, LazyExtension<ISettingsProvider, ISettingsProviderCapabilities>>();

        foreach (var settingsProvider in _settingsProviders)
        {
            var settingsName = settingsProvider.Metadata?.SettingsName;
            if (settingsName is null)
            {
                continue;
            }

            if (SettingsProvidersMap.ContainsKey(settingsName))
            {
                _logger.SendMessage(
                    TestMessageLevel.Error,
                    string.Format(
                        CultureInfo.CurrentCulture,
                        CommonResources.DuplicateSettingsName,
                        settingsName));
            }
            else
            {
                SettingsProvidersMap.Add(settingsName, settingsProvider);
            }
        }
    }

    /// <summary>
    /// Gets the Unfiltered list of settings providers.  Used for the /ListSettingsProviders command line argument.
    /// </summary>
    public IEnumerable<LazyExtension<ISettingsProvider, Dictionary<string, object>>> UnfilteredSettingsProviders { get; }

    /// <summary>
    /// Gets the map of settings name to settings provider.
    /// </summary>
    public Dictionary<string, LazyExtension<ISettingsProvider, ISettingsProviderCapabilities>> SettingsProvidersMap { get; }

    /// <summary>
    /// Creates an instance of the settings provider.
    /// </summary>
    /// <returns>Instance of the settings provider.</returns>
    public static SettingsProviderExtensionManager Create()
    {
        if (s_settingsProviderExtensionManager == null)
        {
            lock (Synclock)
            {
                if (s_settingsProviderExtensionManager == null)
                {

                    TestPluginManager.GetSpecificTestExtensions<TestSettingsProviderPluginInformation, ISettingsProvider, ISettingsProviderCapabilities, TestSettingsProviderMetadata>(
                            TestPlatformConstants.TestAdapterEndsWithPattern,
                            out var unfilteredTestExtensions,
                            out var testExtensions);

                    s_settingsProviderExtensionManager = new SettingsProviderExtensionManager(
                        testExtensions, unfilteredTestExtensions, TestSessionMessageLogger.Instance);
                }
            }
        }

        return s_settingsProviderExtensionManager;
    }

    /// <summary>
    /// Destroy the extension manager.
    /// </summary>
    public static void Destroy()
    {
        lock (Synclock)
        {
            s_settingsProviderExtensionManager = null;
        }
    }

    /// <summary>
    /// Load all the settings provider and fail on error
    /// </summary>
    /// <param name="shouldThrowOnError"> Indicates whether this method should throw on error. </param>
    public static void LoadAndInitializeAllExtensions(bool shouldThrowOnError)
    {
        var extensionManager = Create();

        try
        {
            foreach (var settingsProvider in extensionManager.SettingsProvidersMap)
            {
                // Note: - The below Verbose call should not be under IsVerboseEnabled check as we want to
                // call executor.Value even if logging is not enabled.
                EqtTrace.Verbose("SettingsProviderExtensionManager: Loading settings provider {0}", settingsProvider.Value.Value);
            }
        }
        catch (Exception ex)
        {
            EqtTrace.Error("SettingsProviderExtensionManager: LoadAndInitialize: Exception occurred while loading extensions {0}", ex);

            if (shouldThrowOnError)
            {
                throw;
            }
        }
    }

    /// <summary>
    /// Gets the settings with the provided name.
    /// </summary>
    /// <param name="settingsName">Name of the settings to get.</param>
    /// <returns>Settings provider with the provided name or null if one was not found.</returns>
    internal LazyExtension<ISettingsProvider, ISettingsProviderCapabilities>? GetSettingsProvider(string settingsName)
    {
        if (settingsName.IsNullOrWhiteSpace())
        {
            throw new ArgumentException(ObjectModelCommonResources.CannotBeNullOrEmpty, nameof(settingsName));
        }

        SettingsProvidersMap.TryGetValue(settingsName, out var settingsProvider);

        return settingsProvider;
    }

}

/// <summary>
/// Hold data about the Test settings provider.
/// </summary>
internal class TestSettingsProviderMetadata : ISettingsProviderCapabilities
{
    /// <summary>
    /// The constructor
    /// </summary>
    /// <param name="settingsName">Test settings name</param>
    public TestSettingsProviderMetadata(string settingsName)
    {
        SettingsName = settingsName;
    }

    /// <summary>
    /// Gets test settings name.
    /// </summary>
    public string SettingsName
    {
        get;
        private set;
    }
}
