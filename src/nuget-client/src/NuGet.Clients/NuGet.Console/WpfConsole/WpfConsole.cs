// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Media;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;
using NuGet.PackageManagement;
using NuGet.PackageManagement.VisualStudio;
using NuGet.VisualStudio;
using EditorDefGuidList = Microsoft.VisualStudio.Editor.DefGuidList;
using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;
using Task = System.Threading.Tasks.Task;

namespace NuGetConsole.Implementation.Console
{
    internal interface IPrivateWpfConsole : IWpfConsole
    {
        SnapshotPoint? InputLineStart { get; }
        InputHistory InputHistory { get; }
        void BeginInputLine();
        SnapshotSpan? EndInputLine(bool isEcho);
    }

    [SuppressMessage(
        "Microsoft.Maintainability",
        "CA1506:AvoidExcessiveClassCoupling",
        Justification = "We don't have resources to refactor this class.")]
    internal class WpfConsole : ObjectWithFactory<WpfConsoleService>, IDisposable
    {
        private readonly IPrivateConsoleStatus _consoleStatus;
        private IVsTextBuffer _bufferAdapter;
        private int _consoleWidth = -1;
        private bool _isConsoleWidthExplicitlySet;
        private IContentType _contentType;
        private int _currentHistoryInputIndex;
        private IPrivateConsoleDispatcher _dispatcher;
        private IList<string> _historyInputs;
        private IHost _host;
        private InputHistory _inputHistory;
        private SnapshotPoint? _inputLineStart;
        private PrivateMarshaler _marshaler;
        private uint _pdwCookieForStatusBar;
        private IReadOnlyRegion _readOnlyRegionBegin;
        private IReadOnlyRegion _readOnlyRegionBody;
        private IVsTextView _view;
        private IVsStatusbar _vsStatusBar;
        private IWpfTextView _wpfTextView;
        private bool _startedWritingOutput;
        private List<Tuple<string, Color?, Color?>> _outputCache = new List<Tuple<string, Color?, Color?>>();

        public WpfConsole(
            WpfConsoleService factory,
            IServiceProvider sp,
            IPrivateConsoleStatus consoleStatus,
            string contentTypeName,
            string hostName)
            : base(factory)
        {
            UtilityMethods.ThrowIfArgumentNull(sp);

            _consoleStatus = consoleStatus;
            ServiceProvider = sp;
            ContentTypeName = contentTypeName;
            HostName = hostName;
        }

        private IServiceProvider ServiceProvider { get; set; }
        public string ContentTypeName { get; private set; }
        public string HostName { get; private set; }

        public IPrivateConsoleDispatcher Dispatcher
        {
            get
            {
                if (_dispatcher == null)
                {
                    _dispatcher = new ConsoleDispatcher(Marshaler);
                }
                return _dispatcher;
            }
        }

        public IVsUIShell VsUIShell
        {
            get { return ServiceProvider.GetService<IVsUIShell>(typeof(SVsUIShell)); }
        }

        private IVsStatusbar VsStatusBar
        {
            get
            {
                return NuGetUIThreadHelper.JoinableTaskFactory.Run(GetVsStatusBarAsync);
            }
        }

        private async Task<IVsStatusbar> GetVsStatusBarAsync()
        {
            if (_vsStatusBar == null)
            {
                _vsStatusBar = await AsyncServiceProvider.GlobalProvider.GetServiceAsync<IVsStatusbar, IVsStatusbar>(throwOnFailure: false);
            }
            return _vsStatusBar;
        }

        private IOleServiceProvider OleServiceProvider
        {
            get { return ServiceProvider.GetService<IOleServiceProvider>(typeof(IOleServiceProvider)); }
        }

        private IContentType ContentType
        {
            get
            {
                if (_contentType == null)
                {
                    _contentType = Factory.ContentTypeRegistryService.GetContentType(ContentTypeName);
                    if (_contentType == null)
                    {
                        _contentType = Factory.ContentTypeRegistryService.AddContentType(
                            ContentTypeName, new[] { "text" });
                    }
                }

                return _contentType;
            }
        }

        private IVsTextBuffer VsTextBuffer
        {
            get
            {
                if (_bufferAdapter == null)
                {
                    // make sure we only create text editor after StartWritingOutput() is called.
                    Debug.Assert(_startedWritingOutput);

                    _bufferAdapter = Factory.VsEditorAdaptersFactoryService.CreateVsTextBufferAdapter(
                        OleServiceProvider, ContentType);
                    _bufferAdapter.InitializeContent(string.Empty, 0);
                }

                return _bufferAdapter;
            }
        }

        public IWpfTextView WpfTextView
        {
            get
            {
                if (_wpfTextView == null)
                {
                    // make sure we only create text editor after StartWritingOutput() is called.
                    Debug.Assert(_startedWritingOutput);
                    _wpfTextView = Factory.VsEditorAdaptersFactoryService.GetWpfTextView(VsTextView);
                }

                return _wpfTextView;
            }
        }

        private IWpfTextViewHost WpfTextViewHost
        {
            get
            {
                var userData = VsTextView as IVsUserData;
                object data;
                Guid guidIWpfTextViewHost = EditorDefGuidList.guidIWpfTextViewHost;
                userData.GetData(ref guidIWpfTextViewHost, out data);
                var wpfTextViewHost = data as IWpfTextViewHost;

                return wpfTextViewHost;
            }
        }

        /// <summary>
        /// Get current input line start point (updated to current WpfTextView's text snapshot).
        /// </summary>
        public SnapshotPoint? InputLineStart
        {
            get
            {
                if (_inputLineStart != null)
                {
                    ITextSnapshot snapshot = WpfTextView.TextSnapshot;
                    if (_inputLineStart.Value.Snapshot != snapshot)
                    {
                        _inputLineStart = _inputLineStart.Value.TranslateTo(snapshot, PointTrackingMode.Negative);
                    }
                }
                return _inputLineStart;
            }
        }

        public SnapshotSpan InputLineExtent
        {
            get { return GetInputLineExtent(); }
        }

        /// <summary>
        /// Get the snapshot extent from InputLineStart to END. Normally this console expects
        /// one line only on InputLine. However in some cases multiple lines could appear, e.g.
        /// when a DTE event handler writes to the console. This scenario is not fully supported,
        /// but it is better to clean up nicely with ESC/ArrowUp/Return.
        /// </summary>
        public SnapshotSpan AllInputExtent
        {
            get
            {
                SnapshotPoint start = InputLineStart.Value;
                return new SnapshotSpan(start, start.Snapshot.GetEnd());
            }
        }

        public string InputLineText
        {
            get { return InputLineExtent.GetText(); }
        }

        private PrivateMarshaler Marshaler
        {
            get
            {
                if (_marshaler == null)
                {
                    _marshaler = new PrivateMarshaler(this);
                }
                return _marshaler;
            }
        }

        public IWpfConsole MarshaledConsole
        {
            get { return Marshaler; }
        }

        public IHost Host
        {
            get { return _host; }
            set
            {
                if (_host != null)
                {
                    throw new InvalidOperationException();
                }
                _host = value;
            }
        }

        public int ConsoleWidth
        {
            get
            {
                if (_consoleWidth < 0)
                {
                    ITextViewMargin leftMargin = WpfTextViewHost.GetTextViewMargin(PredefinedMarginNames.Left);
                    ITextViewMargin rightMargin = WpfTextViewHost.GetTextViewMargin(PredefinedMarginNames.Right);

                    double marginSize = 0.0;
                    if (leftMargin != null
                        && leftMargin.Enabled)
                    {
                        marginSize += leftMargin.MarginSize;
                    }
                    if (rightMargin != null
                        && rightMargin.Enabled)
                    {
                        marginSize += rightMargin.MarginSize;
                    }

                    var n = (int)((WpfTextView.ViewportWidth - marginSize) / WpfTextView.FormattedLineSource.ColumnWidth);
                    _consoleWidth = Math.Max(80, n); // Larger of 80 or n
                }
                return _consoleWidth;
            }
        }

        private InputHistory InputHistory
        {
            get
            {
                if (_inputHistory == null)
                {
                    _inputHistory = new InputHistory();
                }
                return _inputHistory;
            }
        }

        public IVsTextView VsTextView
        {
            get
            {
                if (_view == null)
                {
                    var textViewRoleSet = Factory.TextEditorFactoryService.CreateTextViewRoleSet(
                        PredefinedTextViewRoles.Interactive,
                        PredefinedTextViewRoles.Editable,
                        PredefinedTextViewRoles.Analyzable,
                        PredefinedTextViewRoles.Zoomable);

                    _view = Factory.VsEditorAdaptersFactoryService.CreateVsTextViewAdapter(OleServiceProvider, textViewRoleSet);
                    _view.Initialize(
                        VsTextBuffer as IVsTextLines,
                        IntPtr.Zero,
                        (uint)(TextViewInitFlags.VIF_HSCROLL | TextViewInitFlags.VIF_VSCROLL) |
                        (uint)TextViewInitFlags3.VIF_NO_HWND_SUPPORT,
                        null);

                    // Set font and color
                    var propCategoryContainer = _view as IVsTextEditorPropertyCategoryContainer;
                    if (propCategoryContainer != null)
                    {
                        IVsTextEditorPropertyContainer propContainer;
                        Guid guidPropCategory = EditorDefGuidList.guidEditPropCategoryViewMasterSettings;
                        int hr = propCategoryContainer.GetPropertyCategory(ref guidPropCategory, out propContainer);
                        if (hr == 0)
                        {
                            propContainer.SetProperty(VSEDITPROPID.VSEDITPROPID_ViewGeneral_FontCategory,
                                GuidList.guidPackageManagerConsoleFontAndColorCategory);
                            propContainer.SetProperty(VSEDITPROPID.VSEDITPROPID_ViewGeneral_ColorCategory,
                                GuidList.guidPackageManagerConsoleFontAndColorCategory);
                        }
                    }

                    // add myself as IConsole
                    WpfTextView.TextBuffer.Properties.AddProperty(typeof(IConsole), this);

                    // Initial mark readonly region. Must call Start() to start accepting inputs.
                    SetReadOnlyRegionType(ReadOnlyRegionType.All);

                    // Set some EditorOptions: -DragDropEditing, +WordWrap
                    IEditorOptions editorOptions = Factory.EditorOptionsFactoryService.GetOptions(WpfTextView);
                    editorOptions.SetOptionValue(DefaultTextViewOptions.DragDropEditingId, false);
                    editorOptions.SetOptionValue(DefaultTextViewOptions.WordWrapStyleId, WordWrapStyles.WordWrap);

                    // Reset console width when needed
                    WpfTextView.ViewportWidthChanged += (sender, e) => ResetConsoleWidth();
                    WpfTextView.ZoomLevelChanged += (sender, e) => ResetConsoleWidth();

                    // Create my Command Filter
                    _ = new WpfConsoleKeyProcessor(this);
                }

                return _view;
            }
        }

        public object Content
        {
            get { return WpfTextViewHost.HostControl; }
        }

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            try
            {
                Dispose(true);
            }
            finally
            {
                GC.SuppressFinalize(this);
            }
        }

        #endregion

        public event EventHandler<NuGetEventArgs<Tuple<SnapshotSpan, Color?, Color?>>> NewColorSpan;
        public event EventHandler ConsoleCleared;

        private void SetReadOnlyRegionType(ReadOnlyRegionType value)
        {
            if (!_startedWritingOutput)
            {
                return;
            }

            ITextBuffer buffer = WpfTextView.TextBuffer;
            ITextSnapshot snapshot = buffer.CurrentSnapshot;

            using (IReadOnlyRegionEdit edit = buffer.CreateReadOnlyRegionEdit())
            {
                edit.ClearReadOnlyRegion(ref _readOnlyRegionBegin);
                edit.ClearReadOnlyRegion(ref _readOnlyRegionBody);

                switch (value)
                {
                    case ReadOnlyRegionType.BeginAndBody:
                        if (snapshot.Length > 0)
                        {
                            _readOnlyRegionBegin = edit.CreateReadOnlyRegion(new Span(0, 0),
                                SpanTrackingMode.EdgeExclusive,
                                EdgeInsertionMode.Deny);
                            _readOnlyRegionBody = edit.CreateReadOnlyRegion(new Span(0, snapshot.Length));
                        }
                        break;

                    case ReadOnlyRegionType.All:
                        _readOnlyRegionBody = edit.CreateReadOnlyRegion(new Span(0, snapshot.Length),
                            SpanTrackingMode.EdgeExclusive,
                            EdgeInsertionMode.Deny);
                        break;
                }

                edit.Apply();
            }
        }

        public SnapshotSpan GetInputLineExtent(int start = 0, int length = -1)
        {
            SnapshotPoint beginPoint = InputLineStart.Value + start;
            return length >= 0
                ? new SnapshotSpan(beginPoint, length)
                : new SnapshotSpan(beginPoint, beginPoint.GetContainingLine().End);
        }

        public void BeginInputLine()
        {
            if (!_startedWritingOutput)
            {
                return;
            }

            if (_inputLineStart == null)
            {
                SetReadOnlyRegionType(ReadOnlyRegionType.BeginAndBody);
                _inputLineStart = WpfTextView.TextSnapshot.GetEnd();
            }
        }

        public SnapshotSpan? EndInputLine(bool isEcho = false)
        {
            if (!_startedWritingOutput)
            {
                return null;
            }

            // Reset history navigation upon end of a command line
            ResetNavigateHistory();

            if (_inputLineStart != null)
            {
                SnapshotSpan inputSpan = InputLineExtent;

                _inputLineStart = null;
                SetReadOnlyRegionType(ReadOnlyRegionType.All);
                if (!isEcho)
                {
                    Dispatcher.PostInputLine(new InputLine(inputSpan));
                }

                return inputSpan;
            }

            return null;
        }

        private void ResetConsoleWidth()
        {
            if (!_isConsoleWidthExplicitlySet)
            {
                _consoleWidth = -1;
            }
        }

        public void SetConsoleWidth(int consoleWidth)
        {
            _consoleWidth = consoleWidth;
            _isConsoleWidthExplicitlySet = true;
        }

        public void Write(string text)
        {
            if (!_startedWritingOutput)
            {
                _outputCache.Add(Tuple.Create<string, Color?, Color?>(text, null, null));
                return;
            }

            if (_inputLineStart == null) // If not in input mode, need unlock to enable output
            {
                SetReadOnlyRegionType(ReadOnlyRegionType.None);
            }

            // Append text to editor buffer
            ITextBuffer textBuffer = WpfTextView.TextBuffer;
            textBuffer.Insert(textBuffer.CurrentSnapshot.Length, text);

            // Ensure caret visible (scroll)
            WpfTextView.Caret.EnsureVisible();

            if (_inputLineStart == null) // If not in input mode, need lock again
            {
                SetReadOnlyRegionType(ReadOnlyRegionType.All);
            }
        }

        public void WriteLine(string text)
        {
            // If append \n only, text becomes 1 line when copied to notepad.
            Write(text + Environment.NewLine);
        }

        public void WriteBackspace()
        {
            if (_inputLineStart == null) // If not in input mode, need unlock to enable output
            {
                SetReadOnlyRegionType(ReadOnlyRegionType.None);
            }

            // Delete last character from input buffer.
            ITextBuffer textBuffer = WpfTextView.TextBuffer;
            if (textBuffer.CurrentSnapshot.Length > 0)
            {
                textBuffer.Delete(new Span(textBuffer.CurrentSnapshot.Length - 1, 1));
            }

            // Ensure caret visible (scroll)
            WpfTextView.Caret.EnsureVisible();

            if (_inputLineStart == null) // If not in input mode, need lock again
            {
                SetReadOnlyRegionType(ReadOnlyRegionType.All);
            }
        }

        public void Write(string text, Color? foreground, Color? background)
        {
            if (!_startedWritingOutput)
            {
                _outputCache.Add(Tuple.Create(text, foreground, background));
                return;
            }

            int begin = WpfTextView.TextSnapshot.Length;
            Write(text);
            int end = WpfTextView.TextSnapshot.Length;

            if (foreground != null
                || background != null)
            {
                var span = new SnapshotSpan(WpfTextView.TextSnapshot, begin, end - begin);
                NewColorSpan.Raise(this, Tuple.Create(span, foreground, background));
            }
        }

        public void StartWritingOutput()
        {
            _startedWritingOutput = true;
            FlushOutput();
        }

        private void FlushOutput()
        {
            foreach (var tuple in _outputCache)
            {
                Write(tuple.Item1, tuple.Item2, tuple.Item3);
            }

            _outputCache.Clear();
            _outputCache = null;
        }

        private void ResetNavigateHistory()
        {
            _historyInputs = null;
            _currentHistoryInputIndex = -1;
        }

        public void NavigateHistory(int offset)
        {
            if (_historyInputs == null)
            {
                _historyInputs = InputHistory.History;
                if (_historyInputs == null)
                {
                    _historyInputs = Array.Empty<string>();
                }

                _currentHistoryInputIndex = _historyInputs.Count;
            }

            int index = _currentHistoryInputIndex + offset;
            if (index >= -1
                && index <= _historyInputs.Count)
            {
                _currentHistoryInputIndex = index;
                string input = (index >= 0 && index < _historyInputs.Count)
                    ? _historyInputs[_currentHistoryInputIndex]
                    : string.Empty;

                // Replace all text after InputLineStart with new text
                WpfTextView.TextBuffer.Replace(AllInputExtent, input);
                WpfTextView.Caret.EnsureVisible();
            }
        }

        private async Task WriteProgressAsync(string operation, int percentComplete)
        {
            if (operation == null)
            {
                throw new ArgumentNullException(nameof(operation));
            }

            if (percentComplete < 0)
            {
                percentComplete = 0;
            }

            if (percentComplete > 100)
            {
                percentComplete = 100;
            }

            if (percentComplete == 100)
            {
                await HideProgressAsync();
            }
            else
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                var vsStatusBar = await GetVsStatusBarAsync();
                vsStatusBar.Progress(
                    ref _pdwCookieForStatusBar,
                    1 /* in progress */,
                    operation,
                    (uint)percentComplete,
                    (uint)100);
            }
        }

        private void HideProgress()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            VsStatusBar.Progress(
                ref _pdwCookieForStatusBar,
                0 /* completed */,
                string.Empty,
                (uint)100,
                (uint)100);
        }

        private async Task HideProgressAsync()
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            var vsStatusBar = await GetVsStatusBarAsync();
            vsStatusBar.Progress(
                ref _pdwCookieForStatusBar,
                0 /* completed */,
                string.Empty,
                (uint)100,
                (uint)100);
        }

        public void SetExecutionMode(bool isExecuting)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            _consoleStatus.SetBusyState(isExecuting);

            if (!isExecuting)
            {
                HideProgress();

                VsUIShell.UpdateCommandUI(0 /* false = update UI asynchronously */);
            }
        }

        public void Activate()
        {
        }

        public void Clear()
        {
            if (!_startedWritingOutput)
            {
                _outputCache.Clear();
                return;
            }

            SetReadOnlyRegionType(ReadOnlyRegionType.None);

            ITextBuffer textBuffer = WpfTextView.TextBuffer;
            textBuffer.Delete(new Span(0, textBuffer.CurrentSnapshot.Length));

            // Dispose existing incompleted input line
            _inputLineStart = null;

            // Raise event
            ConsoleCleared.Raise(this);
        }

        public void ClearConsole()
        {
            if (_inputLineStart != null)
            {
                Dispatcher.ClearConsole();
            }
        }

        [SuppressMessage(
            "Microsoft.Design",
            "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "We don't want to crash VS when it exits.")]
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_bufferAdapter != null)
                {
                    NuGetUIThreadHelper.JoinableTaskFactory.Run(async () =>
                    {
                        await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                        var docData = _bufferAdapter as IVsPersistDocData;
                        if (docData != null)
                        {
                            try
                            {
                                docData.Close();
                            }
                            catch (Exception exception)
                            {
                                ExceptionHelper.WriteErrorToActivityLog(exception);
                            }

                            _bufferAdapter = null;
                        }
                    });
                }

                var disposable = _dispatcher as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }

                if (_view is not null)
                {
                    _view.CloseView();
                    _view = null;
                }
            }
        }

        ~WpfConsole()
        {
            Dispose(false);
        }

        #region Nested type: PrivateMarshaler

        private class PrivateMarshaler : Marshaler<WpfConsole>, IPrivateWpfConsole
        {
            public PrivateMarshaler(WpfConsole impl)
                : base(impl)
            {
            }

            #region IPrivateWpfConsole

            public SnapshotPoint? InputLineStart
            {
                get { return Invoke(() => _impl.InputLineStart); }
            }

            public void BeginInputLine()
            {
                Invoke(() => _impl.BeginInputLine());
            }

            public SnapshotSpan? EndInputLine(bool isEcho)
            {
                return Invoke(() => _impl.EndInputLine(isEcho));
            }

            public InputHistory InputHistory
            {
                get { return Invoke(() => _impl.InputHistory); }
            }

            #endregion

            #region IWpfConsole

            public IHost Host
            {
                get { return Invoke(() => _impl.Host); }
                set { Invoke(() => { _impl.Host = value; }); }
            }

            public IConsoleDispatcher Dispatcher
            {
                get { return Invoke(() => _impl.Dispatcher); }
            }

            public int ConsoleWidth
            {
                get { return Invoke(() => _impl.ConsoleWidth); }
            }

            public async Task WriteAsync(string text)
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                _impl.Write(text);
            }

            public async Task WriteLineAsync(string text)
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                _impl.WriteLine(text);
            }

            public Task WriteLineAsync(string format, params object[] args)
            {
                return WriteLineAsync(string.Format(CultureInfo.CurrentCulture, format, args));
            }

            public async Task WriteBackspaceAsync()
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                _impl.WriteBackspace();
            }

            public async Task WriteAsync(string text, Color? foreground, Color? background)
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                _impl.Write(text, foreground, background);
            }

            public async Task ActivateAsync()
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                _impl.Activate();
            }

            public async Task ClearAsync()
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                _impl.Clear();
            }

            public void SetConsoleWidth(int consoleWidth)
            {
                Invoke(() => _impl.SetConsoleWidth(consoleWidth));
            }

            public void SetExecutionMode(bool isExecuting)
            {
                Invoke(() =>
                {
                    ThreadHelper.ThrowIfNotOnUIThread();

                    _impl.SetExecutionMode(isExecuting);
                });
            }

            public object Content
            {
                get { return Invoke(() => _impl.Content); }
            }

            public Task WriteProgressAsync(string operation, int percentComplete)
            {
                return _impl.WriteProgressAsync(operation, percentComplete);
            }

            public object VsTextView
            {
                get { return Invoke(() => _impl.VsTextView); }
            }

            public bool ShowDisclaimerHeader
            {
                get { return true; }
            }

            public void StartWritingOutput()
            {
                Invoke(_impl.StartWritingOutput);
            }

            #endregion

            public void Dispose()
            {
                _impl.Dispose(disposing: true);
            }
        }

        #endregion

        #region Nested type: ReadOnlyRegionType

        private enum ReadOnlyRegionType
        {
            /// <summary>
            /// No ReadOnly region. The whole text buffer allows edit.
            /// </summary>
            None,

            /// <summary>
            /// Begin and body are ReadOnly. Only allows edit at the end.
            /// </summary>
            BeginAndBody,

            /// <summary>
            /// The whole text buffer is ReadOnly. Does not allow any edit.
            /// </summary>
            All
        };

        #endregion
    }
}
