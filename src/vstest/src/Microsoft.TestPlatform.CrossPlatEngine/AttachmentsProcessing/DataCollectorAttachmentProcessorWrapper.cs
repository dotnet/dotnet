// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETFRAMEWORK

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

using Microsoft.VisualStudio.TestPlatform.Common.DataCollector;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.TestRunAttachmentsProcessing;

/// <summary>
/// This class is the "container" for the real IDataCollectorAttachmentProcessor implementation.
/// It tries to load the extension and it receives calls from the DataCollectorAttachmentProcessorAppDomain that
/// acts as a proxy for the main AppDomain(the runner one).
/// </summary>
internal sealed class DataCollectorAttachmentProcessorRemoteWrapper : MarshalByRefObject, IDisposable
{
    private readonly AnonymousPipeServerStream _pipeServerStream = new(PipeDirection.Out, HandleInheritability.None);
    private readonly object _pipeClientLock = new();
    private readonly string _pipeShutdownMessagePrefix;

    private IDataCollectorAttachmentProcessor? _dataCollectorAttachmentProcessorInstance;

    private CancellationTokenSource? _processAttachmentCts;

    public string? AssemblyQualifiedName { get; private set; }

    public string? FriendlyName { get; private set; }

    public bool LoadSucceded { get; private set; }

    public bool HasAttachmentProcessor { get; private set; }

    public DataCollectorAttachmentProcessorRemoteWrapper(string pipeShutdownMessagePrefix)
    {
        _pipeShutdownMessagePrefix = pipeShutdownMessagePrefix ?? throw new ArgumentNullException(nameof(pipeShutdownMessagePrefix));
    }

    public string GetClientHandleAsString() => _pipeServerStream.GetClientHandleAsString();

    public bool SupportsIncrementalProcessing => _dataCollectorAttachmentProcessorInstance?.SupportsIncrementalProcessing == true;

    public Uri[]? GetExtensionUris() => _dataCollectorAttachmentProcessorInstance?.GetExtensionUris()?.ToArray();

    public string ProcessAttachment(
        string configurationElement,
        string attachments)
    {
        var doc = new XmlDocument();
        doc.LoadXml(configurationElement);
        AttachmentSet[] attachmentSets = JsonDataSerializer.Instance.Deserialize<AttachmentSet[]>(attachments)!;
        SynchronousProgress progress = new(Report);
        _processAttachmentCts = new CancellationTokenSource();

        ICollection<AttachmentSet> attachmentsResult =
            Task.Run(async () => await _dataCollectorAttachmentProcessorInstance!.ProcessAttachmentSetsAsync(
            doc.DocumentElement,
            attachmentSets,
            progress,
            new MessageLogger(this, nameof(ProcessAttachment)),
            _processAttachmentCts.Token))
            // We cannot marshal Task so we need to block the thread until the end of the processing
            .ConfigureAwait(false).GetAwaiter().GetResult();

        return JsonDataSerializer.Instance.Serialize(attachmentsResult.ToArray());
    }

    public void CancelProcessAttachment() => _processAttachmentCts?.Cancel();

    public bool LoadExtension(string filePath, Uri dataCollectorUri)
    {
        var dataCollectorExtensionManager = DataCollectorExtensionManager.Create(filePath, true, new MessageLogger(this, nameof(LoadExtension)));
        var dataCollectorExtension = dataCollectorExtensionManager.TryGetTestExtension(dataCollectorUri);
        if (dataCollectorExtension is null || dataCollectorExtension.Metadata.HasAttachmentProcessor == false)
        {
            TraceInfo($"DataCollectorAttachmentsProcessorsFactory: DataCollectorExtension not found for uri '{dataCollectorUri}'");
            return false;
        }

        TPDebug.Assert(dataCollectorExtension.TestPluginInfo is not null, "dataCollectorExtension.TestPluginInfo is null");
        Type attachmentProcessorType = ((DataCollectorConfig)dataCollectorExtension.TestPluginInfo).AttachmentsProcessorType!;
        try
        {
            _dataCollectorAttachmentProcessorInstance = TestPluginManager.CreateTestExtension<IDataCollectorAttachmentProcessor>(attachmentProcessorType);
            AssemblyQualifiedName = attachmentProcessorType.AssemblyQualifiedName;
            FriendlyName = dataCollectorExtension.Metadata.FriendlyName;
            LoadSucceded = true;
            HasAttachmentProcessor = true;
            TraceInfo($"DataCollectorAttachmentProcessorWrapper.LoadExtension: Creation of collector attachment processor '{attachmentProcessorType.AssemblyQualifiedName}' from file '{filePath}' succeded");
            return true;
        }
        catch (Exception ex)
        {
            TraceError($"DataCollectorAttachmentProcessorWrapper.LoadExtension: Failed during the creation of data collector attachment processor '{attachmentProcessorType.AssemblyQualifiedName}'\n{ex}");
            SendMessage(nameof(LoadExtension), TestMessageLevel.Error, $"DataCollectorAttachmentProcessorWrapper.LoadExtension: Failed during the creation of data collector attachment processor '{attachmentProcessorType.AssemblyQualifiedName}'\n{ex}");
        }

        return false;
    }

    private void TraceError(string message) => Trace(AppDomainPipeMessagePrefix.EqtTraceError, message);

    private void TraceInfo(string message) => Trace(AppDomainPipeMessagePrefix.EqtTraceInfo, message);

    private void Trace(string traceType, string message)
    {
        lock (_pipeClientLock)
        {
            WriteToPipe($"{traceType}|{message}");
        }
    }

    private void Report(int value)
    {
        lock (_pipeClientLock)
        {
            WriteToPipe($"{AppDomainPipeMessagePrefix.Report}|{value}");
        }
    }

    private void SendMessage(string origin, TestMessageLevel messageLevel, string message)
    {
        lock (_pipeClientLock)
        {
            WriteToPipe($"{origin}.TestMessageLevel.{messageLevel}|{message}");
        }
    }

    private void WriteToPipe(string message)
    {
        using StreamWriter sw = new(_pipeServerStream, Encoding.Default, 1024, true);
        sw.AutoFlush = true;
        // We want to keep the protocol very simple and text message oriented.
        // On the read side we do ReadLine() to simplify the parsing and
        // for this reason we remove the \n to null terminator and we'll aggregate on client side.
        sw.WriteLine(message.Replace(Environment.NewLine, "\0").Replace("\n", "\0"));
        _pipeServerStream.Flush();
        _pipeServerStream.WaitForPipeDrain();
    }

    class MessageLogger : IMessageLogger
    {
        private readonly string _name;
        private readonly DataCollectorAttachmentProcessorRemoteWrapper _wrapper;

        public MessageLogger(DataCollectorAttachmentProcessorRemoteWrapper wrapper, string name)
        {
            _wrapper = wrapper ?? throw new ArgumentNullException(nameof(wrapper));
            _name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public void SendMessage(TestMessageLevel testMessageLevel, string message)
            => _wrapper.SendMessage(_name, testMessageLevel, message);
    }

    class SynchronousProgress : IProgress<int>
    {
        private readonly Action<int> _report;

        public SynchronousProgress(Action<int> report) => _report = report ?? throw new ArgumentNullException(nameof(report));

        public void Report(int value) => _report(value);
    }

    public void Dispose()
    {
        _processAttachmentCts?.Dispose();
        // Send shutdown message to gracefully close the client.
        WriteToPipe($"{_pipeShutdownMessagePrefix}_{AppDomain.CurrentDomain.FriendlyName}");
        _pipeServerStream.Dispose();
        (_dataCollectorAttachmentProcessorInstance as IDisposable)?.Dispose();
    }
}

#endif
