// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using Microsoft;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// Tracks a bag of distinct disposable objects which will be disposed when the bag itself is disposed.
    /// </summary>
    internal sealed class DisposableBag : IDisposable, IEnumerable
    {
        /// <summary>
        /// The set of disposable blocks. If <see langword="null" />, then this disposable bag has been disposed.
        /// </summary>
        private ImmutableHashSet<IDisposable?>? _disposables = ImmutableHashSet.Create<IDisposable?>();

        /// <summary>
        /// Disposes of all contained disposable items.
        /// </summary>
        public void Dispose()
        {
            ImmutableHashSet<IDisposable?>? disposables = Interlocked.Exchange(ref _disposables, null);

            if (disposables != null)
            {
                foreach (IDisposable? item in disposables)
                {
                    item?.Dispose();
                }
            }
        }

        /// <summary>
        /// Adds an object to this bag, to be disposed when the bag itself is disposed.
        /// </summary>
        /// <remarks>
        /// If this disposable bag has already been disposed, <paramref name="disposable" /> will be disposed immediately.
        /// </remarks>
        /// <param name="disposable">The value to be included in this disposable bag.</param>
        public void Add(IDisposable? disposable)
        {
            if (disposable == null)
            {
                return;
            }

            bool shouldDisposeArgument = false;

            ImmutableInterlocked.Update(
                ref _disposables,
                (set, item) =>
                {
                    if (set == null)
                    {
                        shouldDisposeArgument = true;
                        return null!;
                    }

                    return set.Add(item);
                },
                disposable);

            if (shouldDisposeArgument)
            {
                disposable.Dispose();
            }
        }

        /// <summary>
        /// Adds objects to this bag, to each be disposed when the bag itself is disposed.
        /// </summary>
        public void Add(IEnumerable<IDisposable?> disposables)
        {
            Requires.NotNull(disposables, nameof(disposables));

            foreach (IDisposable? disposable in disposables)
            {
                Add(disposable);
            }
        }

        /// <summary>
        /// Removes an object from the bag. If done before the bag is disposed, this will prevent
        /// <paramref name="disposable"/> from being disposed along with the bag itself.
        /// </summary>
        /// <param name="disposable">The object to remove.</param>
        public void Remove(IDisposable? disposable)
        {
            if (disposable == null)
            {
                return;
            }

            ImmutableInterlocked.Update(
                ref _disposables,
                (set, item) => set?.Remove(item),
                disposable);
        }

        /// <summary>
        /// Implemented only to allow collection initialization of this type.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator() => throw new NotSupportedException();
    }
}
