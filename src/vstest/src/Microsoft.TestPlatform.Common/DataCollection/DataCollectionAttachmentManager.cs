// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.Common.DataCollector.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;

namespace Microsoft.VisualStudio.TestPlatform.Common.DataCollector;

/// <summary>
/// Manages file transfer from data collector to test runner service.
///
/// Events are handled sequentially so it's not possible have parallel AddAttachment/GetAttachments for the same DataCollectionContext.
/// DataCollectionContext can be a session context(session start/end) or a test case context(test case start/end).
///
/// We have two type of events that will fire a collection of files "session end" and "test case end".
/// File are sent and copied/moved in parallel using async tasks, for these reason we need to use an async structure ConcurrentDictionary
/// to be able to handle parallel test case start/end events(if host run tests in parallel).
///
/// We could avoid to use ConcurrentDictionary for the list of the attachment sets of a specific DataCollectionContext, but
/// we don't know how the user will implement the datacollector and they could send file out of events(wrong usage, no more expected sequential access AddAttachment->GetAttachments),
/// so we prefer protect every collection. This not means that outcome will be "always correct"(file attached in a correct way) but at least we avoid exceptions.
/// </summary>
internal class DataCollectionAttachmentManager : IDataCollectionAttachmentManager, IDisposable
{
    private readonly object _attachmentTaskLock = new();

    /// <summary>
    /// Default results directory to be used when user didn't specify.
    /// </summary>
    private const string DefaultOutputDirectoryName = "TestResults";

    /// <summary>
    /// Logger for data collection messages
    /// </summary>
    private IMessageSink? _messageSink;

    /// <summary>
    /// Attachment transfer tasks associated with a given datacollection context.
    /// </summary>
    private readonly ConcurrentDictionary<DataCollectionContext, List<Task>> _attachmentTasks;

    /// <summary>
    /// Use to cancel attachment transfers if test run is canceled.
    /// </summary>
    private readonly CancellationTokenSource _cancellationTokenSource;

    /// <summary>
    /// File helper instance.
    /// </summary>
    private readonly IFileHelper _fileHelper;

    /// <summary>
    /// Initializes a new instance of the <see cref="DataCollectionAttachmentManager"/> class.
    /// </summary>
    public DataCollectionAttachmentManager()
        : this(new TestPlatform.Utilities.Helpers.FileHelper())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataCollectionAttachmentManager"/> class.
    /// </summary>
    /// <param name="fileHelper">File helper instance.</param>
    protected DataCollectionAttachmentManager(IFileHelper fileHelper)
    {
        _fileHelper = fileHelper;
        _cancellationTokenSource = new CancellationTokenSource();
        _attachmentTasks = new ConcurrentDictionary<DataCollectionContext, List<Task>>();
        AttachmentSets = new ConcurrentDictionary<DataCollectionContext, ConcurrentDictionary<Uri, AttachmentSet>>();
    }

    /// <summary>
    /// Gets the session output directory.
    /// </summary>
    internal string? SessionOutputDirectory { get; private set; }

    /// <summary>
    /// Gets the attachment sets for the given datacollection context.
    /// </summary>
    internal ConcurrentDictionary<DataCollectionContext, ConcurrentDictionary<Uri, AttachmentSet>> AttachmentSets
    {
        get; private set;
    }
    /// <inheritdoc/>
    public void Initialize(SessionId id, string outputDirectory, IMessageSink messageSink)
    {
        ValidateArg.NotNull(id, nameof(id));
        ValidateArg.NotNull(messageSink, nameof(messageSink));

        _messageSink = messageSink;

        if (outputDirectory.IsNullOrEmpty())
        {
            SessionOutputDirectory = Path.Combine(Path.GetTempPath(), DefaultOutputDirectoryName, id.Id.ToString());
        }
        else
        {
            // Create a session specific directory under base output directory.
            var expandedOutputDirectory = Environment.ExpandEnvironmentVariables(outputDirectory);
            var absolutePath = Path.GetFullPath(expandedOutputDirectory);
            SessionOutputDirectory = Path.Combine(absolutePath, id.Id.ToString());
        }

        try
        {
            // Create the output directory if it doesn't exist.
            if (!Directory.Exists(SessionOutputDirectory))
            {
                Directory.CreateDirectory(SessionOutputDirectory);
            }
        }
        catch (UnauthorizedAccessException accessException)
        {
            string accessDeniedMessage = string.Format(CultureInfo.CurrentCulture, Resources.Resources.AccessDenied, accessException.Message);
            ConsoleOutput.Instance.Error(false, accessDeniedMessage);
            throw;
        }

    }

    /// <inheritdoc/>
    public List<AttachmentSet> GetAttachments(DataCollectionContext dataCollectionContext)
    {
        try
        {
            if (_attachmentTasks.TryGetValue(dataCollectionContext, out var tasks))
            {
                Task.WhenAll(tasks.ToArray()).Wait();
            }
        }
        catch (Exception ex)
        {
            EqtTrace.Error("DataCollectionAttachmentManager.GetAttachments: Fail to get attachments: {0} ", ex);
        }

        List<AttachmentSet> attachments = new();

        if (AttachmentSets.TryGetValue(dataCollectionContext, out var uriAttachmentSetMap))
        {
            attachments = uriAttachmentSetMap.Values.ToList();
            _attachmentTasks.TryRemove(dataCollectionContext, out _);
            AttachmentSets.TryRemove(dataCollectionContext, out _);
        }

        return attachments;
    }

    /// <inheritdoc/>
    public void AddAttachment(FileTransferInformation fileTransferInfo, AsyncCompletedEventHandler? sendFileCompletedCallback, Uri uri, string friendlyName)
    {
        ValidateArg.NotNull(fileTransferInfo, nameof(fileTransferInfo));

        if (SessionOutputDirectory.IsNullOrEmpty())
        {
            EqtTrace.Error("DataCollectionAttachmentManager.AddAttachment: Initialize not invoked.");
            return;
        }

        if (!AttachmentSets.ContainsKey(fileTransferInfo.Context))
        {
            var uriAttachmentSetMap = new ConcurrentDictionary<Uri, AttachmentSet>();
            AttachmentSets.TryAdd(fileTransferInfo.Context, uriAttachmentSetMap);
            _attachmentTasks.TryAdd(fileTransferInfo.Context, new List<Task>());
        }

        if (!AttachmentSets[fileTransferInfo.Context].ContainsKey(uri))
        {
            AttachmentSets[fileTransferInfo.Context].TryAdd(uri, new AttachmentSet(uri, friendlyName));
        }

        AddNewFileTransfer(fileTransferInfo, sendFileCompletedCallback, uri, friendlyName);
    }

    /// <inheritdoc/>
    public void Cancel()
    {
        _cancellationTokenSource.Cancel();
    }

    /// <summary>
    /// Sanity checks on CopyRequestData
    /// </summary>
    /// <param name="fileTransferInfo">
    /// The file Transfer Info.
    /// </param>
    /// <param name="localFilePath">
    /// The local File Path.
    /// </param>
    private static void Validate(FileTransferInformation fileTransferInfo, string localFilePath)
    {
        if (!File.Exists(fileTransferInfo.FileName))
        {
            throw new FileNotFoundException(
                string.Format(
                    CultureInfo.InvariantCulture,
                    "Could not find source file '{0}'.",
                    fileTransferInfo.FileName));
        }

        var directoryName = Path.GetDirectoryName(localFilePath);

        if (!Directory.Exists(directoryName))
        {
            Directory.CreateDirectory(directoryName!);
        }
        else if (File.Exists(localFilePath))
        {
            File.Delete(localFilePath);
        }
    }

    /// <summary>
    /// Add a new file transfer (either copy/move) request.
    /// </summary>
    /// <param name="fileTransferInfo">
    /// The file Transfer Info.
    /// </param>
    /// <param name="sendFileCompletedCallback">
    /// The send File Completed Callback.
    /// </param>
    /// <param name="uri">
    /// The uri.
    /// </param>
    /// <param name="friendlyName">
    /// The friendly Name.
    /// </param>
    private void AddNewFileTransfer(FileTransferInformation fileTransferInfo, AsyncCompletedEventHandler? sendFileCompletedCallback, Uri uri, string friendlyName)
    {
        var context = fileTransferInfo.Context;
        TPDebug.Assert(
            context != null,
            "DataCollectionManager.AddNewFileTransfer: FileDataHeaderMessage with null context.");

        var testCaseId = fileTransferInfo.Context.HasTestCase
            ? fileTransferInfo.Context.TestExecId.Id.ToString()
            : string.Empty;

        TPDebug.Assert(SessionOutputDirectory is not null, "SessionOutputDirectory is null.");
        var directoryPath = Path.Combine(
            SessionOutputDirectory,
            testCaseId);
        var localFilePath = Path.Combine(directoryPath, Path.GetFileName(fileTransferInfo.FileName));

        var task = Task.Factory.StartNew(
            () =>
            {
                Validate(fileTransferInfo, localFilePath);

                if (_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    _cancellationTokenSource.Token.ThrowIfCancellationRequested();
                }

                try
                {
                    if (fileTransferInfo.PerformCleanup)
                    {
                        EqtTrace.Info("DataCollectionAttachmentManager.AddNewFileTransfer: Moving file {0} to {1}", fileTransferInfo.FileName, localFilePath);

                        _fileHelper.MoveFile(fileTransferInfo.FileName, localFilePath);

                        EqtTrace.Info("DataCollectionAttachmentManager.AddNewFileTransfer: Moved file {0} to {1}", fileTransferInfo.FileName, localFilePath);
                    }
                    else
                    {
                        EqtTrace.Info("DataCollectionAttachmentManager.AddNewFileTransfer: Copying file {0} to {1}", fileTransferInfo.FileName, localFilePath);

                        _fileHelper.CopyFile(fileTransferInfo.FileName, localFilePath);

                        EqtTrace.Info("DataCollectionAttachmentManager.AddNewFileTransfer: Copied file {0} to {1}", fileTransferInfo.FileName, localFilePath);
                    }
                }
                catch (Exception ex)
                {
                    LogError(
                        ex.ToString(),
                        uri,
                        friendlyName,
                        GuidPolyfill.Parse(testCaseId, CultureInfo.InvariantCulture)
                        );

                    throw;
                }
            },
            _cancellationTokenSource.Token);

        var continuationTask = task.ContinueWith(
            (t) =>
            {
                try
                {
                    if (t.Exception == null)
                    {
                        lock (_attachmentTaskLock)
                        {
                            AttachmentSets[fileTransferInfo.Context][uri].Attachments.Add(UriDataAttachment.CreateFrom(localFilePath, fileTransferInfo.Description));
                        }
                    }

                    sendFileCompletedCallback?.SafeInvoke(this, new AsyncCompletedEventArgs(t.Exception, false, fileTransferInfo.UserToken), "DataCollectionManager.AddNewFileTransfer");
                }
                catch (Exception e)
                {
                    EqtTrace.Error(
                        "DataCollectionAttachmentManager.TriggerCallBack: Error occurred while raising the file transfer completed callback for {0}. Error: {1}",
                        localFilePath,
                        e.ToString());
                }
            },
            _cancellationTokenSource.Token);

        _attachmentTasks[fileTransferInfo.Context].Add(continuationTask);
    }

    /// <summary>
    /// Logs an error message.
    /// </summary>
    /// <param name="errorMessage">
    /// The error message.
    /// </param>
    /// <param name="collectorUri">
    /// The collector uri.
    /// </param>
    /// <param name="collectorFriendlyName">
    /// The collector friendly name.
    /// </param>
    /// <param name="testCaseId">
    /// Id of testCase if available, null otherwise.
    /// </param>
    private void LogError(string errorMessage, Uri collectorUri, string collectorFriendlyName, Guid testCaseId)
    {
        var args = new DataCollectionMessageEventArgs(TestMessageLevel.Error, errorMessage)
        {
            Uri = collectorUri,
            FriendlyName = collectorFriendlyName
        };

        if (!testCaseId.Equals(Guid.Empty))
        {
            args.TestCaseId = testCaseId;
        }

        _messageSink?.SendMessage(args);
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _cancellationTokenSource.Dispose();
        }
    }
}
