// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.SolutionRestoreManager
{
    internal abstract class RestoreOperationProgressUI : IAsyncDisposable
    {
        protected static readonly AsyncLocal<RestoreOperationProgressUI> _instance = new AsyncLocal<RestoreOperationProgressUI>();

        public static RestoreOperationProgressUI Current
        {
            get
            {
                return _instance.Value;
            }
            set
            {
                _instance.Value = value;
            }
        }

        public CancellationToken UserCancellationToken { get; protected set; } = CancellationToken.None;

        public abstract Task ReportProgressAsync(
            string progressMessage,
            uint currentStep = 0,
            uint totalSteps = 0);

        public IDisposable RegisterUserCancellationAction(Action callback)
        {
            return UserCancellationToken.Register(callback);
        }

        public abstract ValueTask DisposeAsync();
    }
}
