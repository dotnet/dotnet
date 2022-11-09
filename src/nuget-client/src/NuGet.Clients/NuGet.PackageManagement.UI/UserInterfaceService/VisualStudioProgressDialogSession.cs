// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using Microsoft.VisualStudio.Shell;

namespace NuGet.PackageManagement.UI
{
    internal class VisualStudioProgressDialogSession : IModalProgressDialogSession
    {
        private readonly Action _dispose;
        private bool _isDisposed = false;

        public VisualStudioProgressDialogSession(ThreadedWaitDialogHelper.Session session)
        {
            UserCancellationToken = session.UserCancellationToken;
            _dispose = session.Dispose;
            Progress = new VisualStudioDialogProgress(session.Progress);
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _dispose();
                _isDisposed = true;
            }
        }

        public IProgress<ProgressDialogData> Progress { get; }
        public CancellationToken UserCancellationToken { get; }
    }
}
