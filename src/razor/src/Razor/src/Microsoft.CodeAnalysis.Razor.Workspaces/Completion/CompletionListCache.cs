// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Completion;

internal class CompletionListCache
{
    private record struct Slot(int Id, VSInternalCompletionList CompletionList, ICompletionResolveContext Context);

    // Internal for testing
    internal const int MaxCacheSize = 10;

    private readonly object _accessLock = new();

    // This is used as a circular buffer.
    private readonly Slot[] _items = new Slot[MaxCacheSize];

    private int _nextIndex;
    private int _nextId;

    public int Add(VSInternalCompletionList completionList, ICompletionResolveContext context)
    {
        lock (_accessLock)
        {
            var index = _nextIndex++;
            var id = _nextId++;

            _items[index] = new Slot(id, completionList, context);

            // _nextIndex should always point to the index where we'll access the next element
            // in the circular buffer. Here, we check to see if it is after the last index.
            // If it is, we change it to the first index to properly "wrap around" the array.
            if (_nextIndex == MaxCacheSize)
            {
                _nextIndex = 0;
            }

            // Return generated id so the completion list can be retrieved later.
            return id;
        }
    }

    public bool TryGet(int id, [NotNullWhen(true)] out VSInternalCompletionList? completionList, [NotNullWhen(true)] out ICompletionResolveContext? context)
    {
        lock (_accessLock)
        {
            var index = _nextIndex;
            var count = MaxCacheSize;

            // Search back to front because the items in the back are the most recently added
            // which are most frequently accessed.
            while (count > 0)
            {
                index--;

                // If we're before the first index in the array, switch to the last index to
                // "wrap around" the array.
                if (index < 0)
                {
                    index = MaxCacheSize - 1;
                }

                var slot = _items[index];

                // CompletionList is annotated as non-nullable, but we are allocating an array of 10 items for our cache, so initially
                // those array entries will be default. By checking for null here, we detect if we're hitting an unused part of the array
                // so stop looping.
                if (slot.CompletionList is null)
                {
                    break;
                }

                if (slot.Id == id)
                {
                    completionList = slot.CompletionList;
                    context = slot.Context;
                    return true;
                }

                count--;
            }

            // A cache entry associated with the given id was not found.
            completionList = null;
            context = null;
            return false;
        }
    }
}
