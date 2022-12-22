// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Xml;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;

/// <summary>
/// Interface for data collector add-ins
/// </summary>
public abstract class DataCollector : IDisposable
{
    /// <summary>
    /// Initializes the data collector
    /// </summary>
    /// <param name="configurationElement">
    /// The XML element containing configuration information for the data collector. Can be
    /// null if the add-in does not have any configuration information.
    /// </param>
    /// <param name="events">
    /// Object containing the execution events the data collector can register for
    /// </param>
    /// <param name="dataSink">The sink used by the data collector to send its data</param>
    /// <param name="logger">
    /// Used by the data collector to send warnings, errors, or other messages
    /// </param>
    /// <param name="environmentContext">Provides contextual information about the agent environment</param>
    public abstract void Initialize(
        XmlElement? configurationElement,
        DataCollectionEvents events,
        DataCollectionSink dataSink,
        DataCollectionLogger logger,
        DataCollectionEnvironmentContext? environmentContext
    );

    /// <summary>
    /// Disposes the data collector.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);

        // Suppress Finalize in case a subclass implements a finalizer.
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Called to perform cleanup when the instance is being disposed.
    /// </summary>
    /// <param name="disposing">True when being called from the Dispose method and false when being called during finalization.</param>
    protected virtual void Dispose(bool disposing)
    {
    }

}
