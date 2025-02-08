// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.VisualStudio.Razor.Discovery;

internal sealed partial class ProjectStateUpdater
{
    internal TestAccessor GetTestAccessor() => new(this);

    internal sealed class TestAccessor(ProjectStateUpdater instance)
    {
        public interface IUpdateItem
        {
            bool IsCompleted { get; }
            bool IsCancellationRequested { get; }
        }

        private sealed class UpdateItemWrapper(UpdateItem updateItem) : IUpdateItem
        {
            public bool IsCompleted => updateItem.IsCompleted;
            public bool IsCancellationRequested => updateItem.IsCancellationRequested;
        }

        public ImmutableArray<IUpdateItem> GetUpdates()
        {
            lock (instance._updates)
            {
                using var result = new PooledArrayBuilder<IUpdateItem>(capacity: instance._updates.Count);

                foreach (var (_, updateItem) in instance._updates)
                {
                    result.Add(new UpdateItemWrapper(updateItem));
                }

                return result.ToImmutable();
            }
        }

        /// <summary>
        /// Used in unit tests to ensure we can control when background work starts.
        /// </summary>
        public ManualResetEventSlim? BlockBackgroundWorkStart
        {
            get => instance._blockBackgroundWorkStart;
            set => instance._blockBackgroundWorkStart = value;
        }

        /// <summary>
        /// Used in unit tests to ensure we can know when background work finishes.
        /// </summary>
        public ManualResetEventSlim? NotifyBackgroundWorkCompleted
        {
            get => instance._notifyBackgroundWorkCompleted;
            set => instance._notifyBackgroundWorkCompleted = value;
        }
    }
}
