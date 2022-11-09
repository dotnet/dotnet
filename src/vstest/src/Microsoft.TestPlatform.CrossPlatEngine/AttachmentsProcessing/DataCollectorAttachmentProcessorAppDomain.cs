// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETFRAMEWORK

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.TestRunAttachmentsProcessing;

/// <summary>
/// This class is a proxy implementation of IDataCollectorAttachmentProcessor.
/// We cannot load extension directly inside the runner in design mode because we're locking files
/// and in some scenario build or publish can fail.
///
/// DataCollectorAttachmentProcessorAppDomain creates DataCollectorAttachmentProcessorRemoteWrapper in a
/// custom domain.
///
/// IDataCollectorAttachmentProcessor needs to communicate back some information like, report percentage state
/// of the processing, send messages through the IMessageLogger etc...so we have a full duplex communication.
///
/// For this reason we use an anonymous pipe to "listen" to the events from the real implementation and we forward
/// the information to the caller.
/// </summary>
internal class DataCollectorAttachmentProcessorAppDomain : IDataCollectorAttachmentProcessor, IDisposable
{
    private readonly string _pipeShutdownMessagePrefix = Guid.NewGuid().ToString();
    private readonly DataCollectorAttachmentProcessorRemoteWrapper _wrapper;
    private readonly InvokedDataCollector _invokedDataCollector;
    private readonly AppDomain _appDomain;
    private readonly IMessageLogger? _dataCollectorAttachmentsProcessorsLogger;
    private readonly Task _pipeServerReadTask;
    private readonly AnonymousPipeClientStream _pipeClientStream;

    public bool LoadSucceded { get; private set; }
    public string? AssemblyQualifiedName => _wrapper.AssemblyQualifiedName;
    public string? FriendlyName => _wrapper.FriendlyName;
    private IMessageLogger? _processAttachmentSetsLogger;
    private IProgress<int>? _progressReporter;

    public DataCollectorAttachmentProcessorAppDomain(InvokedDataCollector invokedDataCollector, IMessageLogger? dataCollectorAttachmentsProcessorsLogger)
    {
        _invokedDataCollector = invokedDataCollector ?? throw new ArgumentNullException(nameof(invokedDataCollector));
        _appDomain = AppDomain.CreateDomain(invokedDataCollector.Uri.ToString());
        _dataCollectorAttachmentsProcessorsLogger = dataCollectorAttachmentsProcessorsLogger;
        _wrapper = (DataCollectorAttachmentProcessorRemoteWrapper)_appDomain.CreateInstanceFromAndUnwrap(
            typeof(DataCollectorAttachmentProcessorRemoteWrapper).Assembly.Location,
            typeof(DataCollectorAttachmentProcessorRemoteWrapper).FullName,
            false,
            BindingFlags.Default,
            null,
            new[] { _pipeShutdownMessagePrefix },
            null,
            null);

        _pipeClientStream = new AnonymousPipeClientStream(PipeDirection.In, _wrapper.GetClientHandleAsString());
        _pipeServerReadTask = Task.Run(() => PipeReaderTask());

        EqtTrace.Verbose($"DataCollectorAttachmentProcessorAppDomain.ctor: AppDomain '{_appDomain.FriendlyName}' created to host assembly '{invokedDataCollector.FilePath}'");

        InitExtension();
    }

    private void InitExtension()
    {
        try
        {
            LoadSucceded = _wrapper.LoadExtension(_invokedDataCollector.FilePath, _invokedDataCollector.Uri);
            EqtTrace.Verbose($"DataCollectorAttachmentProcessorAppDomain.ctor: Extension '{_invokedDataCollector.Uri}' loaded. LoadSucceded: {LoadSucceded} AssemblyQualifiedName: '{AssemblyQualifiedName}' HasAttachmentProcessor: '{HasAttachmentProcessor}' FriendlyName: '{FriendlyName}'");
        }
        catch (Exception ex)
        {
            EqtTrace.Error($"DataCollectorAttachmentProcessorAppDomain.InitExtension: Exception during extension initialization\n{ex}");
        }
    }

    private void PipeReaderTask()
    {
        try
        {
            using StreamReader sr = new(_pipeClientStream, Encoding.Default, false, 1024, true);
            while (_pipeClientStream?.IsConnected == true)
            {
                try
                {
                    string messagePayload = sr.ReadLine().Replace("\0", Environment.NewLine);

                    if (messagePayload.StartsWith(_pipeShutdownMessagePrefix))
                    {
                        EqtTrace.Info($"DataCollectorAttachmentProcessorAppDomain.PipeReaderTask: Shutdown message received, message: {messagePayload}");
                        return;
                    }

                    string prefix = messagePayload.Substring(0, messagePayload.IndexOf('|'));
                    string message = messagePayload.Substring(messagePayload.IndexOf('|') + 1);

                    switch (prefix)
                    {
                        case AppDomainPipeMessagePrefix.EqtTraceError: EqtTrace.Error(message); break;
                        case AppDomainPipeMessagePrefix.EqtTraceInfo: EqtTrace.Info(message); break;
                        case AppDomainPipeMessagePrefix.LoadExtensionTestMessageLevelInformational:
                        case AppDomainPipeMessagePrefix.LoadExtensionTestMessageLevelWarning:
                        case AppDomainPipeMessagePrefix.LoadExtensionTestMessageLevelError:
                            _dataCollectorAttachmentsProcessorsLogger?
                                .SendMessage((TestMessageLevel)Enum.Parse(typeof(TestMessageLevel), prefix.Substring(prefix.LastIndexOf('.') + 1), false), message);
                            break;
                        case AppDomainPipeMessagePrefix.ProcessAttachmentTestMessageLevelInformational:
                        case AppDomainPipeMessagePrefix.ProcessAttachmentTestMessageLevelWarning:
                        case AppDomainPipeMessagePrefix.ProcessAttachmentTestMessageLevelError:
                            _processAttachmentSetsLogger?
                                .SendMessage((TestMessageLevel)Enum.Parse(typeof(TestMessageLevel), prefix.Substring(prefix.LastIndexOf('.') + 1), false), message);
                            break;
                        case AppDomainPipeMessagePrefix.Report:
                            _progressReporter?.Report(int.Parse(message, CultureInfo.CurrentCulture));
                            break;
                        default:
                            EqtTrace.Error($"DataCollectorAttachmentProcessorAppDomain:PipeReaderTask: Unknown message: {message}");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    EqtTrace.Error($"DataCollectorAttachmentProcessorAppDomain.PipeReaderTask: Exception during the pipe reading, Pipe connected: {_pipeClientStream.IsConnected}\n{ex}");
                }
            }

            EqtTrace.Info($"DataCollectorAttachmentProcessorAppDomain.PipeReaderTask: Exiting from the pipe read loop.");
        }
        catch (Exception ex)
        {
            EqtTrace.Error($"DataCollectorAttachmentProcessorAppDomain.PipeReaderTask: Exception on stream reader for the pipe reading\n{ex}");
        }
    }

    public bool HasAttachmentProcessor => _wrapper.HasAttachmentProcessor;

    public bool SupportsIncrementalProcessing => _wrapper.SupportsIncrementalProcessing;

    public IEnumerable<Uri>? GetExtensionUris() => _wrapper?.GetExtensionUris();

    public async Task<ICollection<AttachmentSet>> ProcessAttachmentSetsAsync(XmlElement configurationElement, ICollection<AttachmentSet> attachments, IProgress<int> progressReporter, IMessageLogger logger, CancellationToken cancellationToken)
    {
        // We register the cancellation and we call cancel inside the AppDomain
        cancellationToken.Register(() => _wrapper.CancelProcessAttachment());
        _processAttachmentSetsLogger = logger;
        _progressReporter = progressReporter;
        var result = await Task.Run(() => _wrapper.ProcessAttachment(configurationElement.OuterXml, JsonDataSerializer.Instance.Serialize(attachments.ToArray()))).ConfigureAwait(false);
        return JsonDataSerializer.Instance.Deserialize<AttachmentSet[]>(result)!;
    }

    public void Dispose()
    {
        _wrapper.Dispose();

        string appDomainName = _appDomain.FriendlyName;
        AppDomain.Unload(_appDomain);
        EqtTrace.Verbose($"DataCollectorAttachmentProcessorAppDomain.Dispose: Unloaded AppDomain '{appDomainName}'");

        if (_pipeServerReadTask?.Wait(TimeSpan.FromSeconds(30)) == false)
        {
            EqtTrace.Error($"DataCollectorAttachmentProcessorAppDomain.Dispose: PipeReaderTask timeout expired");
        }

        // We don't need to close the pipe handle because we're communicating with an in-process pipe and the same handle is closed by AppDomain.Unload(_appDomain);
        // Disposing here will fail for invalid handle during the release but we call it to avoid the GC cleanup inside the finalizer thread
        // where it fails the same.
        //
        // We could also suppress the finalizers
        // GC.SuppressFinalize(_pipeClientStream);
        // GC.SuppressFinalize(_pipeClientStream.SafePipeHandle);
        // but doing so mean relying to an implementation detail,
        // for instance if some changes are done and some other object finalizer will be added;
        // this will run on .NET Framework and it's unexpected but we prefer to rely on the documented semantic:
        // "if I call dispose no finalizers will be called for unmanaged resources hold by this object".
        try
        {
            _pipeClientStream?.Dispose();
        }
        catch
        { }
    }
}

internal static class AppDomainPipeMessagePrefix
{
    public const string EqtTraceError = "EqtTrace.Error";
    public const string EqtTraceInfo = "EqtTrace.Info";
    public const string Report = "Report";
    public const string LoadExtensionTestMessageLevelInformational = "LoadExtension.TestMessageLevel.Informational";
    public const string LoadExtensionTestMessageLevelWarning = "﻿LoadExtension.TestMessageLevel.Warning";
    public const string LoadExtensionTestMessageLevelError = "LoadExtension.TestMessageLevel.Error";
    public const string ProcessAttachmentTestMessageLevelInformational = "ProcessAttachment.TestMessageLevel.Informational";
    public const string ProcessAttachmentTestMessageLevelWarning = "ProcessAttachment.TestMessageLevel.Warning";
    public const string ProcessAttachmentTestMessageLevelError = "ProcessAttachment.TestMessageLevel.Error";
}

#endif
