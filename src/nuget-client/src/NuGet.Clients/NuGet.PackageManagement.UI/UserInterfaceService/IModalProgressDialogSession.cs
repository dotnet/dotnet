// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;

namespace NuGet.PackageManagement.UI
{
    public interface IModalProgressDialogSession : IDisposable
    {
        IProgress<ProgressDialogData> Progress { get; }

        CancellationToken UserCancellationToken { get; }
    }
}
