// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;

/// <summary>
/// Class used by data collectors to send data to up-stream components
/// </summary>
public abstract class DataCollectionSink
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DataCollectionSink"/> class.
    /// </summary>
    protected DataCollectionSink()
    {
    }

    /// <summary>
    /// Called when sending of a file has completed.
    /// </summary>
    public abstract event AsyncCompletedEventHandler? SendFileCompleted;

    /// <summary>
    /// Sends a file to up-stream components.
    /// </summary>
    /// <param name="context">The context in which the file is being sent.  Cannot be null.</param>
    /// <param name="path">the path to the file on the local file system</param>
    /// <param name="deleteFile">True to automatically have the file removed after sending it.</param>
    public void SendFileAsync(DataCollectionContext context, string path, bool deleteFile)
    {
        SendFileAsync(context, path, string.Empty, deleteFile);
    }

    /// <summary>
    /// Sends a file to up-stream components.
    /// </summary>
    /// <param name="context">The context in which the file is being sent.  Cannot be null.</param>
    /// <param name="path">the path to the file on the local file system</param>
    /// <param name="description">A short description of the data being sent.</param>
    /// <param name="deleteFile">True to automatically have the file removed after sending it.</param>
    public void SendFileAsync(DataCollectionContext context, string path, string description, bool deleteFile)
    {
        var fileInfo = new FileTransferInformation(context, path, deleteFile);
        fileInfo.Description = description;

        SendFileAsync(fileInfo);
    }

    /// <summary>
    /// Sends a file to up-stream components
    /// </summary>
    /// <param name="fileTransferInformation">Information about the file to be transferred.</param>
    public abstract void SendFileAsync(FileTransferInformation fileTransferInformation);

}
