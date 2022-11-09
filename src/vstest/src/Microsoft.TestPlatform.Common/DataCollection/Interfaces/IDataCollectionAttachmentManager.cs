// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;

namespace Microsoft.VisualStudio.TestPlatform.Common.DataCollector.Interfaces;

/// <summary>
/// The DataCollectionAttachmentManager Interface.
/// </summary>
internal interface IDataCollectionAttachmentManager
{
    /// <summary>
    /// Initializes session, output directory and data collection log.
    /// </summary>
    /// <param name="id">
    /// The id.
    /// </param>
    /// <param name="outputDirectory">
    /// The output directory.
    /// </param>
    /// <param name="messageSink">
    /// The message Sink.
    /// </param>
    void Initialize(SessionId id, string outputDirectory, IMessageSink messageSink);

    /// <summary>
    /// Gets attachment sets associated with given collection context.
    /// </summary>
    /// <param name="dataCollectionContext">
    /// The data collection context.
    /// </param>
    /// <returns>
    /// The <see cref="List"/>.
    /// </returns>
    List<AttachmentSet> GetAttachments(DataCollectionContext dataCollectionContext);

    /// <summary>
    /// Adds new attachment to current context
    /// </summary>
    /// <param name="fileTransferInfo">
    /// The file Transfer Info.
    /// </param>
    /// <param name="sendFileCompletedCallback">
    /// The send File Completed Callback.
    /// </param>
    /// <param name="dataCollectorUri">
    /// Uri of the data collector.
    /// </param>
    /// <param name="friendlyName">
    /// The friendly Name.
    /// </param>
    void AddAttachment(FileTransferInformation fileTransferInfo, AsyncCompletedEventHandler? sendFileCompletedCallback, Uri dataCollectorUri, string friendlyName);

    /// <summary>
    /// Stops processing further transfer requests as test run is canceled.
    /// </summary>
    void Cancel();
}
