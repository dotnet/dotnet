// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.CompilerServices;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>
    /// Defines a contract for an awaiter managing asynchronous access to a lock.
    /// </summary>
    public abstract class AsyncLockAwaiter : INotifyCompletion
    {
        public abstract bool IsCompleted { get; }

        public abstract IDisposable GetResult();

        public abstract void OnCompleted(Action continuation);

        public abstract void Release();
    }
}
