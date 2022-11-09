// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell.TableControl;
using Microsoft.VisualStudio.Shell.TableManager;

using IAsyncServiceProvider = Microsoft.VisualStudio.Shell.IAsyncServiceProvider;
using Task = System.Threading.Tasks.Task;

namespace NuGet.VisualStudio.Common
{
    /// <summary>
    /// Add/Remove warnings/errors from the error list.
    /// This persists messages once they are added.
    /// </summary>
    [Export(typeof(INuGetErrorList))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public sealed class ErrorListTableDataSource : INuGetErrorList, ITableDataSource, IDisposable
    {
        private readonly object _initLockObj = new object();
        private readonly object _subscribeLockObj = new object();
        private readonly IAsyncServiceProvider _asyncServiceProvider = AsyncServiceProvider.GlobalProvider;
        private IReadOnlyList<TableSubscription> _subscriptions = new List<TableSubscription>();
        private readonly List<ErrorListTableEntry> _entries = new List<ErrorListTableEntry>();

        public string SourceTypeIdentifier => StandardTableDataSources.ErrorTableDataSource;

        public string Identifier => "NuGetRestoreManagerListTable";

        public string DisplayName => "NuGet_Restore_Manager_Table_Data_Source";

        private IErrorList _errorList;
        private ITableManager _tableManager;
        private bool _initialized;

        /// <summary>
        /// Internal, used by tests.
        /// </summary>
        public ErrorListTableDataSource(IErrorList errorList, ITableManager tableManager)
        {
            if (errorList == null)
            {
                throw new ArgumentNullException(nameof(errorList));
            }

            if (tableManager == null)
            {
                throw new ArgumentNullException(nameof(tableManager));
            }

            _initialized = true;
            _errorList = errorList;
            _tableManager = tableManager;
        }

        public ErrorListTableDataSource()
        {
        }


        /// <summary>
        /// This method is called by the TableManager during the call to EnsureInitialized().
        /// Locks should be careful to avoid a deadlock here due to this flow.
        /// </summary>
        public IDisposable Subscribe(ITableDataSink sink)
        {
            TableSubscription subscription = null;

            lock (_entries)
            {
                subscription = new TableSubscription(sink);

                if (_entries.Count > 0)
                {
                    subscription.RunWithLock((s) =>
                    {
                        // Add all existing entries to the new sink
                        s.AddEntries(_entries.ToList(), removeAllEntries: false);
                    });
                }
            }

            lock (_subscribeLockObj)
            {
                // Add valid subscriptions to the new list to start
                var updatedSubscriptions = new List<TableSubscription>(_subscriptions.Where(e => !e.Disposed))
                {
                    // Add the new subscription
                    subscription
                };

                // Apply changes
                _subscriptions = updatedSubscriptions;
            }

            return subscription;
        }

        /// <summary>
        /// Clear only nuget entries.
        /// </summary>
        public void ClearNuGetEntries()
        {
            if (_initialized)
            {
                lock (_entries)
                {
                    // Clear all entries
                    _entries.Clear();
                }

                var subscriptions = _subscriptions;
                foreach (var subscription in subscriptions)
                {
                    if (!subscription.Disposed)
                    {
                        subscription.RunWithLock((sink) =>
                        {
                            // Find all entries in the sink that belong to NuGet, this ensures we won't miss any.
                            var entries = _errorList.TableControl.Entries.Where(IsNuGetEntry).ToArray();

                            // Remove all found entries.
                            sink.RemoveEntries(entries);
                        });
                    }
                }
            }
        }

        /// <summary>
        /// Add error list entries
        /// </summary>
        public void AddNuGetEntries(params ErrorListTableEntry[] entries)
        {
            if (entries.Length > 0)
            {
                EnsureInitialized();

                lock (_entries)
                {
                    // Update the full set of entries
                    _entries.AddRange(entries);
                }

                // Add new entries to each sink
                var subscriptions = _subscriptions;
                foreach (var subscription in subscriptions)
                {
                    if (!subscription.Disposed)
                    {
                        subscription.RunWithLock((sink) =>
                        {
                            // Copy the list we pass off
                            // Add everything to the sink
                            sink.AddEntries(entries.ToList(), removeAllEntries: false);
                        });
                    }
                }
            }
        }

        /// <summary>
        /// Show error window if settings permit.
        /// </summary>
        public async Task BringToFrontIfSettingsPermitAsync()
        {
            EnsureInitialized();

            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            IVsShell vsShell = await _asyncServiceProvider.GetServiceAsync<IVsShell, IVsShell>(throwOnFailure: false);
            int getPropertyReturnCode = vsShell.GetProperty((int)__VSSPROPID.VSSPROPID_ShowTasklistOnBuildEnd, out object propertyShowTaskListOnBuildEnd);
            bool showErrorListOnBuildEnd = true;

            if (getPropertyReturnCode == VSConstants.S_OK)
            {
                if (bool.TryParse(propertyShowTaskListOnBuildEnd?.ToString(), out bool result))
                {
                    showErrorListOnBuildEnd = result;
                }
            }

            if (showErrorListOnBuildEnd)
            {
                // Give the error list focus.
                var vsErrorList = _errorList as IVsErrorList;
                vsErrorList?.BringToFront();
            }
        }

        // Lock before calling
        private void EnsureInitialized()
        {
            if (!_initialized)
            {
                // Double check around locking since this is called often.
                lock (_initLockObj)
                {
                    if (!_initialized)
                    {
                        NuGetUIThreadHelper.JoinableTaskFactory.Run(async () =>
                        {
                            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                            // Get the error list service from the UI thread
                            _errorList = await _asyncServiceProvider.GetServiceAsync<SVsErrorList, IErrorList>();
                            _tableManager = _errorList.TableControl.Manager;

                            _tableManager.AddSource(this);
                            _initialized = true;
                        });
                    }
                }
            }
        }

        /// <summary>
        /// Internal use only, return all entries.
        /// </summary>
        /// <returns></returns>
        public ErrorListTableEntry[] GetEntries()
        {
            lock (_entries)
            {
                return _entries.ToArray();
            }
        }

        private static bool IsNuGetEntry(ITableEntryHandle entry)
        {
            object sourceObj;
            return (entry != null
                && entry.TryGetValue(StandardTableColumnDefinitions.ErrorSource, out sourceObj)
                && StringComparer.Ordinal.Equals(ErrorListTableEntry.ErrorSouce, (sourceObj as string)));
        }

        public void Dispose()
        {
            if (_initialized)
            {
                // This does not need to be on the UI thread.
                _tableManager.RemoveSource(this);
            }
        }

        /// <summary>
        /// Holds an ITableDataSink and lock.
        /// </summary>
        private class TableSubscription : IDisposable
        {
            private readonly object _lockObj = new object();
            private ITableDataSink _sink;

            /// <summary>
            /// True if the TableManager has called Dipose on this.
            /// </summary>
            public bool Disposed { get; private set; }

            public TableSubscription(ITableDataSink sink)
            {
                _sink = sink;
            }

            public void RunWithLock(Action<ITableDataSink> action)
            {
                lock (_lockObj)
                {
                    if (!Disposed)
                    {
                        if (_sink != null)
                        {
                            action(_sink);
                        }
                    }
                }
            }

            /// <summary>
            /// When disposed we set the sink to null and later
            /// all null sinks will be cleared out of the list.
            /// Setting it to null also means that operations on this
            /// will end up as a noop.
            /// </summary>
            public void Dispose()
            {
                lock (_lockObj)
                {
                    _sink = null;
                    Disposed = true;
                }
            }
        }
    }
}
