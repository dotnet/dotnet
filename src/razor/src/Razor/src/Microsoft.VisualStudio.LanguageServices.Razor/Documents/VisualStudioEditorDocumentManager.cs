// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.Editor.Razor.Documents;

// Similar to the DocumentProvider in dotnet/Roslyn - but simplified quite a bit to remove
// concepts that we don't need. Responsible for providing data about text changes for documents
// and editor open/closed state.
internal class VisualStudioEditorDocumentManager : EditorDocumentManagerBase
{
    private readonly IVsEditorAdaptersFactoryService _editorAdaptersFactory;

    private readonly IVsRunningDocumentTable4 _runningDocumentTable;

    private readonly Dictionary<uint, List<DocumentKey>> _documentsByCookie;
    private readonly Dictionary<DocumentKey, uint> _cookiesByDocument;
    private bool _advised;

    public VisualStudioEditorDocumentManager(
        ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
        JoinableTaskContext joinableTaskContext,
        FileChangeTrackerFactory fileChangeTrackerFactory,
        IVsRunningDocumentTable runningDocumentTable,
        IVsEditorAdaptersFactoryService editorAdaptersFactory)
        : base(projectSnapshotManagerDispatcher, joinableTaskContext, fileChangeTrackerFactory)
    {
        if (runningDocumentTable is null)
        {
            throw new ArgumentNullException(nameof(runningDocumentTable));
        }

        if (editorAdaptersFactory is null)
        {
            throw new ArgumentNullException(nameof(editorAdaptersFactory));
        }

        if (fileChangeTrackerFactory is null)
        {
            throw new ArgumentNullException(nameof(fileChangeTrackerFactory));
        }

        _runningDocumentTable = (IVsRunningDocumentTable4)runningDocumentTable;
        _editorAdaptersFactory = editorAdaptersFactory;

        _documentsByCookie = new Dictionary<uint, List<DocumentKey>>();
        _cookiesByDocument = new Dictionary<DocumentKey, uint>();
    }

    protected override ITextBuffer? GetTextBufferForOpenDocument(string filePath)
    {
        if (filePath is null)
        {
            throw new ArgumentNullException(nameof(filePath));
        }

        JoinableTaskContext.AssertUIThread();

        EnsureDocumentTableAdvised();

        // Check if the document is already open and initialized, and associate a buffer if possible.
        uint cookie;
        if (_runningDocumentTable.IsMonikerValid(filePath) &&
            ((cookie = _runningDocumentTable.GetDocumentCookie(filePath)) != VSConstants.VSCOOKIE_NIL) &&
            (_runningDocumentTable.GetDocumentFlags(cookie) & (uint)_VSRDTFLAGS4.RDT_PendingInitialization) == 0)
        {
            // GetDocumentData requires the UI thread
            var documentData = _runningDocumentTable.GetDocumentData(cookie);

            var textBuffer = documentData is not VsTextBuffer vsTextBuffer
                ? null
                : _editorAdaptersFactory.GetDocumentBuffer(vsTextBuffer);
            return textBuffer;
        }

        return null;
    }

    protected override void OnDocumentOpened(EditorDocument document)
    {
        JoinableTaskContext.AssertUIThread();

        EnsureDocumentTableAdvised();

        var cookie = _runningDocumentTable.GetDocumentCookie(document.DocumentFilePath);
        if (cookie != VSConstants.VSCOOKIE_NIL)
        {
            TrackOpenDocument(cookie, new DocumentKey(document.ProjectKey, document.DocumentFilePath));
        }
    }

    protected override void OnDocumentClosed(EditorDocument document)
    {
        JoinableTaskContext.AssertUIThread();

        EnsureDocumentTableAdvised();

        var key = new DocumentKey(document.ProjectKey, document.DocumentFilePath);
        if (_cookiesByDocument.TryGetValue(key, out var cookie))
        {
            UntrackOpenDocument(cookie, key);
        }
    }

    public void DocumentOpened(uint cookie)
    {
        JoinableTaskContext.AssertUIThread();

        EnsureDocumentTableAdvised();

        lock (Lock)
        {
            // Casts avoid dynamic
            if ((object)_runningDocumentTable.GetDocumentData(cookie) is IVsTextBuffer vsTextBuffer)
            {
                var filePath = _runningDocumentTable.GetDocumentMoniker(cookie);
                if (!TryGetMatchingDocuments(filePath, out var documents))
                {
                    // This isn't a document that we're interesting in.
                    return;
                }

                var textBuffer = _editorAdaptersFactory.GetDataBuffer(vsTextBuffer);
                if (textBuffer is null)
                {
                    // The text buffer has not been created yet, register to be notified when it is.
                    VsTextBufferDataEventsSink.Subscribe(vsTextBuffer, () => BufferLoaded(vsTextBuffer, filePath));

                    return;
                }

                // It's possible that events could be fired out of order and that this is a rename.
                if (_documentsByCookie.ContainsKey(cookie))
                {
                    DocumentClosed(cookie, exceptFilePath: filePath);
                }

                BufferLoaded(textBuffer, filePath, documents);
            }
        }
    }

    public void BufferLoaded(IVsTextBuffer vsTextBuffer, string filePath)
    {
        JoinableTaskContext.AssertUIThread();

        EnsureDocumentTableAdvised();

        var textBuffer = _editorAdaptersFactory.GetDocumentBuffer(vsTextBuffer);
        if (textBuffer != null)
        {
            // We potentially waited for the editor to initialize on this code path, so requery
            // the documents.
            if (TryGetMatchingDocuments(filePath, out var documents))
            {
                BufferLoaded(textBuffer, filePath, documents);
            }
        }
    }

    public void BufferLoaded(ITextBuffer textBuffer, string filePath, EditorDocument[] documents)
    {
        JoinableTaskContext.AssertUIThread();

        EnsureDocumentTableAdvised();

        lock (Lock)
        {
            for (var i = 0; i < documents.Length; i++)
            {
                DocumentOpened(filePath, textBuffer);
            }
        }
    }

    public void DocumentClosed(uint cookie, string? exceptFilePath = null)
    {
        JoinableTaskContext.AssertUIThread();

        EnsureDocumentTableAdvised();

        lock (Lock)
        {
            if (!_documentsByCookie.TryGetValue(cookie, out var documents))
            {
                return;
            }

            // We have to deal with some complications here due to renames and event ordering and such.
            // We we might see multiple documents open for a cookie (due to linked files), but only one of them
            // has been renamed. In that case, we just process the change that we know about.
            var filePaths = new HashSet<string>(documents.Select(d => d.DocumentFilePath));

            // `Remove` can correctly handle the case when the incoming value is null without any exceptions.
            // The method is just not properly annotated for it,
            // so we can suppress the warning here
            filePaths.Remove(exceptFilePath!); 

            foreach (var filePath in filePaths)
            {
                DocumentClosed(filePath);
            }
        }
    }

    public void DocumentRenamed(uint cookie, string fromFilePath, string toFilePath)
    {
        JoinableTaskContext.AssertUIThread();

        EnsureDocumentTableAdvised();

        // Ignore changes is casing
        if (FilePathComparer.Instance.Equals(fromFilePath, toFilePath))
        {
            return;
        }

        lock (Lock)
        {
            // Treat a rename as a close + reopen.
            //
            // Due to ordering issues, we could see a partial rename. This is why we need to pass the new
            // file path here.
            DocumentClosed(cookie, exceptFilePath: toFilePath);
        }

        // Try to open any existing documents that match the new name.
        if ((_runningDocumentTable.GetDocumentFlags(cookie) & (uint)_VSRDTFLAGS4.RDT_PendingInitialization) == 0)
        {
            DocumentOpened(cookie);
        }
    }

    private void EnsureDocumentTableAdvised()
    {
        JoinableTaskContext.AssertUIThread();

        if (!_advised)
        {
            _advised = true;
            var hr = ((IVsRunningDocumentTable)_runningDocumentTable).AdviseRunningDocTableEvents(new RunningDocumentTableEventSink(this), out _);
            Marshal.ThrowExceptionForHR(hr);
        }
    }

    private void TrackOpenDocument(uint cookie, DocumentKey key)
    {
        if (!_documentsByCookie.TryGetValue(cookie, out var documents))
        {
            documents = new List<DocumentKey>();
            _documentsByCookie.Add(cookie, documents);
        }

        if (!documents.Contains(key))
        {
            documents.Add(key);
        }

        _cookiesByDocument[key] = cookie;
    }

    private void UntrackOpenDocument(uint cookie, DocumentKey key)
    {
        if (_documentsByCookie.TryGetValue(cookie, out var documents))
        {
            documents.Remove(key);

            if (documents.Count == 0)
            {
                _documentsByCookie.Remove(cookie);
            }
        }

        _cookiesByDocument.Remove(key);
    }
}
