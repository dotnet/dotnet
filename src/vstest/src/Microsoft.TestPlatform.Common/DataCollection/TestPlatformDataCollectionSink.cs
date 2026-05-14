// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;

using Microsoft.VisualStudio.TestPlatform.Common.DataCollector.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;

namespace Microsoft.VisualStudio.TestPlatform.Common.DataCollector;

/// <summary>
/// The test platform data collection sink.
/// </summary>
internal class TestPlatformDataCollectionSink : DataCollectionSink
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestPlatformDataCollectionSink"/> class.
    /// Creates a data collector sink for data transfer.
    /// </summary>
    /// <param name="attachmentManager">
    /// The attachment Manager.
    /// </param>
    /// <param name="dataCollectorConfig">
    /// Data collector info.
    /// </param>
    internal TestPlatformDataCollectionSink(IDataCollectionAttachmentManager attachmentManager, DataCollectorConfig dataCollectorConfig)
    {
        DataCollectorConfig = dataCollectorConfig ?? throw new ArgumentNullException(nameof(dataCollectorConfig));
        AttachmentManager = attachmentManager ?? throw new ArgumentNullException(nameof(attachmentManager));
    }

    /// <summary>
    /// Event handler for handling file transfer completed event.
    /// </summary>
    public override event AsyncCompletedEventHandler? SendFileCompleted;

    /// <summary>
    /// Gets or sets message sink to transfer collection message.
    /// </summary>
    private IDataCollectionAttachmentManager AttachmentManager
    {
        get; set;
    }

    /// <summary>
    /// Gets or sets dataCollector with which this data sink is associated.
    /// </summary>
    private DataCollectorConfig DataCollectorConfig
    {
        get; set;
    }

    /// <summary>
    /// Sends a file asynchronously.
    /// </summary>
    /// <param name="fileTransferInformation">Information about the file being transferred.</param>
    public override void SendFileAsync(FileTransferInformation fileTransferInformation)
    {
        ValidateArg.NotNull(fileTransferInformation, nameof(fileTransferInformation));
        TPDebug.Assert(DataCollectorConfig.TypeUri is not null, "DataCollectorConfig.TypeUri is null");
        AttachmentManager.AddAttachment(fileTransferInformation, SendFileCompleted, DataCollectorConfig.TypeUri, DataCollectorConfig.FriendlyName);
    }
}
