// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;

internal partial class TestProjectSnapshotManager
{
    public partial class Listener : IEnumerable<ProjectChangeEventArgs>, IDisposable
    {
        private readonly TestProjectSnapshotManager _projectManager;
        private readonly List<ProjectChangeEventArgs> _notifications;

        public Listener(TestProjectSnapshotManager projectManager)
        {
            _projectManager = projectManager;
            _projectManager.Changed += ProjectManager_Changed;

            _notifications = [];
        }

#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
        public void Dispose()
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
        {
            _projectManager.Changed -= ProjectManager_Changed;
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        public IEnumerator<ProjectChangeEventArgs> GetEnumerator()
        {
            foreach (var notification in _notifications)
            {
                yield return notification;
            }
        }

        private void ProjectManager_Changed(object? sender, ProjectChangeEventArgs e)
        {
            _notifications.Add(e);
        }
    }
}
