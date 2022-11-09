// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.Common;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;

/// <summary>
/// Creates and return a list of available attachments processor
/// </summary>
internal interface IDataCollectorAttachmentsProcessorsFactory
{
    /// <summary>
    /// Creates and return a list of available attachments processor
    /// </summary>
    /// <param name="invokedDataCollector">List of invoked data collectors</param>
    /// <param name="logger">Message logger</param>
    /// <returns>List of attachments processors</returns>
    DataCollectorAttachmentProcessor[] Create(InvokedDataCollector[]? invokedDataCollectors, IMessageLogger logger);
}

/// <summary>
/// Registered data collector attachment processor
/// </summary>
internal class DataCollectorAttachmentProcessor : IDisposable
{
    /// <summary>
    /// Data collector FriendlyName
    /// </summary>
    public string FriendlyName { get; private set; }

    /// <summary>
    /// Data collector attachment processor instance
    /// </summary>
    public IDataCollectorAttachmentProcessor DataCollectorAttachmentProcessorInstance { get; private set; }

    public DataCollectorAttachmentProcessor(string friendlyName, IDataCollectorAttachmentProcessor dataCollectorAttachmentProcessor)
    {
        FriendlyName = friendlyName.IsNullOrEmpty()
            ? throw new ArgumentException($"'{nameof(friendlyName)}' cannot be null or empty.", nameof(friendlyName))
            : friendlyName;
        DataCollectorAttachmentProcessorInstance = dataCollectorAttachmentProcessor ?? throw new ArgumentNullException(nameof(dataCollectorAttachmentProcessor));
    }

    public void Dispose()
    {
        (DataCollectorAttachmentProcessorInstance as IDisposable)?.Dispose();
    }
}
