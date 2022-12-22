// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;

using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework.Utilities;
using Microsoft.VisualStudio.TestPlatform.Common.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Common.Logging;
using Microsoft.VisualStudio.TestPlatform.Common.SettingsProvider;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;

using CommonResources = Microsoft.VisualStudio.TestPlatform.Common.Resources.Resources;
using ObjectModelCommonResources = Microsoft.VisualStudio.TestPlatform.ObjectModel.Resources.CommonResources;

namespace Microsoft.VisualStudio.TestPlatform.Common;

/// <summary>
/// Used for loading settings for a run.
/// </summary>
public class RunSettings : IRunSettings
{
    /// <summary>
    /// Map of the settings names in the file to their associated settings provider.
    /// </summary>
    private readonly Dictionary<string, LazyExtension<ISettingsProvider, ISettingsProviderCapabilities>> _settings;

    /// <summary>
    /// Used to keep track if settings have been loaded.
    /// </summary>
    private bool _isSettingsLoaded;

    /// <summary>
    /// Initializes a new instance of the <see cref="RunSettings"/> class.
    /// </summary>
    public RunSettings()
    {
        _settings = new Dictionary<string, LazyExtension<ISettingsProvider, ISettingsProviderCapabilities>>();
    }

    /// <summary>
    /// Gets the settings in the form of Xml string.
    /// </summary>
    public string? SettingsXml { get; private set; }

    /// <summary>
    /// Get the settings for the provided settings name.
    /// </summary>
    /// <param name="settingsName">Name of the settings section to get.</param>
    /// <returns>The settings provider for the settings or null if one was not found.</returns>
    public ISettingsProvider? GetSettings(string? settingsName)
    {
        if (settingsName.IsNullOrWhiteSpace())
        {
            throw new ArgumentException(ObjectModelCommonResources.CannotBeNullOrEmpty, nameof(settingsName));
        }

        // Try and lookup the settings provider.
        _settings.TryGetValue(settingsName, out var provider);

        // If a provider was found, return it.
        return provider?.Value;
    }

    /// <summary>
    /// Load the settings from the provided xml string.
    /// </summary>
    /// <param name="settings">xml string</param>
    public void LoadSettingsXml(string settings)
    {
        if (StringUtilities.IsNullOrWhiteSpace(settings))
        {
            throw new ArgumentException(ObjectModelCommonResources.CannotBeNullOrEmpty, settings);
        }

        using var stringReader = new StringReader(settings);
        var reader = XmlReader.Create(stringReader, XmlRunSettingsUtilities.ReaderSettings);
        ValidateAndSaveSettings(reader);
    }

    /// <summary>
    /// Initialize settings providers with the settings xml.
    /// </summary>
    /// <param name="settings"> The settings xml string. </param>
    public void InitializeSettingsProviders(string settings)
    {
        using var stringReader = new StringReader(settings);
        var reader = XmlReader.Create(stringReader, XmlRunSettingsUtilities.ReaderSettings);
        ReadRunSettings(reader);
    }

    /// <summary>
    /// Validate the runsettings checking that it is well formed.
    /// This would throw XML exception on failure.
    /// </summary>
    /// <param name="reader">A xml reader instance.</param>
    private void ValidateAndSaveSettings(XmlReader reader)
    {
        try
        {
            var dom = new XmlDocument();
            dom.Load(reader);
            using var writer = new StringWriter(CultureInfo.InvariantCulture);
            dom.Save(writer);
            SettingsXml = writer.ToString();
        }
        catch (Exception e)
        {
            throw new SettingsException(
                string.Format(
                    CultureInfo.CurrentCulture,
                    CommonResources.RunSettingsParseError,
                    e.Message),
                e);
        }
    }

    /// <summary>
    /// Reads test run settings from XmlReader
    /// </summary>
    /// <param name="reader"></param>
    private void ReadRunSettings(XmlReader reader)
    {
        // If settings have already been loaded, throw.
        if (_isSettingsLoaded)
        {
            throw new InvalidOperationException(CommonResources.RunSettingsAlreadyLoaded);
        }

        _isSettingsLoaded = true;

        try
        {
            // Read to the root element.
            XmlReaderUtilities.ReadToRootNode(reader);

            // Read to the first section.
            reader.ReadToNextElement();

            // Lookup the settings provider for each of the elements.
            var settingsExtensionManager = SettingsProviderExtensionManager.Create();
            while (!reader.EOF)
            {
                LoadSection(reader, settingsExtensionManager);
                reader.SkipToNextElement();
            }
        }
        catch (SettingsException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new SettingsException(
                string.Format(
                    CultureInfo.CurrentCulture,
                    CommonResources.RunSettingsParseError,
                    e.Message),
                e);
        }
    }

    /// <summary>
    /// Loads the section for the current element of the reader.
    /// </summary>
    /// <param name="reader">Reader to load the section from.</param>
    /// <param name="settingsExtensionManager">Settings extension manager to get the provider from.</param>
    private void LoadSection(XmlReader reader, SettingsProviderExtensionManager settingsExtensionManager)
    {
        ValidateArg.NotNull(reader, nameof(reader));
        ValidateArg.NotNull(settingsExtensionManager, nameof(settingsExtensionManager));

        // Check for duplicate settings
        if (_settings.ContainsKey(reader.Name))
        {
            TestSessionMessageLogger.Instance.SendMessage(
                TestMessageLevel.Error,
                string.Format(CultureInfo.CurrentCulture, CommonResources.DuplicateSettingsProvided, reader.Name));

            return;
        }

        // Look up the section for this node.
        var provider = settingsExtensionManager.GetSettingsProvider(reader.Name);

        if (provider != null)
        {
            try
            {
                // Have the provider load the settings.
                provider.Value.Load(reader.ReadSubtree());
            }
            catch (Exception e)
            {
                // Setup to throw the exception when the section is requested.
                provider = CreateLazyThrower(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        CommonResources.SettingsProviderInitializationError,
                        provider.Metadata.SettingsName,
                        e.Message),
                    provider.Metadata,
                    e);
            }
        }
        else
        {
            // Setup to throw when this section is requested.
            var metadata = new TestSettingsProviderMetadata(reader.Name);

            var message = string.Format(
                CultureInfo.CurrentCulture,
                CommonResources.SettingsProviderNotFound,
                metadata.SettingsName);

            provider = CreateLazyThrower(message, metadata);
        }

        // Cache the provider instance so it can be looked up later when the section is requested.
        _settings.Add(provider.Metadata.SettingsName, provider);
    }

    /// <summary>
    /// Creates a lazy instance which will throw a SettingsException when the value property is accessed.
    /// </summary>
    /// <param name="message">Message for the exception.</param>
    /// <param name="metadata">Metadata to use for the lazy instance.</param>
    /// <param name="innerException">Inner exception to include in the exception which is thrown.</param>
    /// <returns>Lazy instance setup to throw when the value property is accessed.</returns>
    private static LazyExtension<ISettingsProvider, ISettingsProviderCapabilities> CreateLazyThrower(
        string message,
        ISettingsProviderCapabilities metadata,
        Exception? innerException = null)
    {
        return new LazyExtension<ISettingsProvider, ISettingsProviderCapabilities>(
            () => throw new SettingsException(message, innerException),
            metadata);
    }

}
