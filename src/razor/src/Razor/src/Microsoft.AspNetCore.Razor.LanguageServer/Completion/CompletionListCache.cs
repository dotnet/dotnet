// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion;

internal sealed class CompletionListCache
{
    private record struct Slot(
        int Id,
        VSInternalCompletionList CompletionList,
        object? Context,
        bool Used = true);

    // Internal for testing
    internal const int MaxCacheSize = 10;

    private readonly object _accessLock = new();

    // This is used as a circular buffer.
    private readonly Slot[] _items = new Slot[MaxCacheSize];

    private int _nextIndex;
    private int _nextId;

    public int Add(VSInternalCompletionList completionList, object? context)
    {
        if (completionList is null)
        {
            throw new ArgumentNullException(nameof(completionList));
        }

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

    public bool TryGet(int id, out (VSInternalCompletionList CompletionList, object? Context) result)
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

                if (!slot.Used)
                {
                    break;
                }

                if (slot.Id == id)
                {
                    result = (slot.CompletionList, slot.Context);
                    return true;
                }

                count--;
            }

            // A cache entry associated with the given id was not found.
            result = default;
            return false;
        }
    }
}
