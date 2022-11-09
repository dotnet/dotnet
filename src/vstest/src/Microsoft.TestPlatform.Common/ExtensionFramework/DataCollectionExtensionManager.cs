// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.Common.DataCollector;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework.Utilities;
using Microsoft.VisualStudio.TestPlatform.Common.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;

/// <summary>
/// Manages loading and provides access to data collector extensions implementing the
/// DataCollector interface.
/// </summary>
internal class DataCollectorExtensionManager : TestExtensionManager<ObjectModel.DataCollection.DataCollector, IDataCollectorCapabilities>
{
    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="unfilteredTestExtensions">
    /// The unfiltered Test Extensions.
    /// </param>
    /// <param name="testExtensions">
    /// The test Extensions.
    /// </param>
    /// <param name="logger">
    /// The logger.
    /// </param>
    /// <remarks>
    /// The constructor is not public because the factory method should be used to get instances of this class.
    /// </remarks>
    protected DataCollectorExtensionManager(
        IEnumerable<LazyExtension<ObjectModel.DataCollection.DataCollector, Dictionary<string, object>>> unfilteredTestExtensions,
        IEnumerable<LazyExtension<ObjectModel.DataCollection.DataCollector, IDataCollectorCapabilities>> testExtensions,
        IMessageLogger logger)
        : base(unfilteredTestExtensions, testExtensions, logger)
    {
    }

    /// <summary>
    /// Gets an instance of the DataCollectorExtensionManager.
    /// </summary>
    /// <param name="messageLogger">
    /// The message Logger.
    /// </param>
    /// <returns>
    /// The DataCollectorExtensionManager.
    /// </returns>
    public static DataCollectorExtensionManager Create(IMessageLogger messageLogger)
    {
        TestPluginManager.GetSpecificTestExtensions<DataCollectorConfig, ObjectModel.DataCollection.DataCollector, IDataCollectorCapabilities, DataCollectorMetadata>(
            TestPlatformConstants.DataCollectorEndsWithPattern,
            out var unfilteredTestExtensions,
            out var filteredTestExtensions);

        return new DataCollectorExtensionManager(unfilteredTestExtensions, filteredTestExtensions, messageLogger);
    }

    /// <summary>
    /// Gets an instance of the DataCollectorExtensionManager.
    /// </summary>
    /// <param name="extensionAssemblyFilePath">
    /// File path that contains data collectors to load.
    /// </param>
    /// <param name="skipCache">
    /// Skip the extensions cache.
    /// </param>
    /// <param name="messageLogger">
    /// The message Logger.
    /// </param>
    /// <returns>
    /// The DataCollectorExtensionManager.
    /// </returns>
    public static DataCollectorExtensionManager Create(string extensionAssemblyFilePath, bool skipCache, IMessageLogger messageLogger)
    {
        TestPluginManager.GetTestExtensions<DataCollectorConfig, ObjectModel.DataCollection.DataCollector, IDataCollectorCapabilities, DataCollectorMetadata>(
            extensionAssemblyFilePath,
            out var unfilteredTestExtensions,
            out var filteredTestExtensions,
            skipCache);

        return new DataCollectorExtensionManager(unfilteredTestExtensions, filteredTestExtensions, messageLogger);
    }
}

/// <summary>
/// Hold data about the Data Collector.
/// </summary>
public class DataCollectorMetadata : IDataCollectorCapabilities
{
    /// <summary>
    /// Constructor for DataCollectorMetadata
    /// </summary>
    /// <param name="extension">
    /// Uri identifying the data collector.
    /// </param>
    /// <param name="friendlyName">
    /// The friendly Name.
    /// </param>
    public DataCollectorMetadata(string extension, string friendlyName)
        : this(extension, friendlyName, false)
    { }

    /// <summary>
    /// Constructor for DataCollectorMetadata
    /// </summary>
    /// <param name="extension">
    /// Uri identifying the data collector.
    /// </param>
    /// <param name="friendlyName">
    /// The friendly Name.
    /// <param name="hasAttachmentProcessor">
    /// Indicates if the current data collector registers an attachment processor
    /// </param>
    public DataCollectorMetadata(string extension, string friendlyName, bool hasAttachmentProcessor)
    {
        ExtensionUri = extension;
        FriendlyName = friendlyName;
        HasAttachmentProcessor = hasAttachmentProcessor;
    }

    /// <summary>
    /// Gets Uri identifying the data collector.
    /// </summary>
    public string ExtensionUri
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets Friendly Name identifying the data collector.
    /// </summary>
    public string FriendlyName
    {
        get;
        private set;
    }

    /// <summary>
    /// Check if the data collector has got attachment processor registered
    /// </summary>
    public bool HasAttachmentProcessor
    {
        get;
        private set;
    }
}
