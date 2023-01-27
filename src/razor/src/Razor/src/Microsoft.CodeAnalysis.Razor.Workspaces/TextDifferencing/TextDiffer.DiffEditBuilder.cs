// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.Razor.TextDifferencing;

internal abstract partial class TextDiffer
{
    protected readonly ref struct DiffEditBuilder
    {
        private readonly List<DiffEdit> _edits;

        public DiffEditBuilder(List<DiffEdit> edits)
        {
            _edits = edits ?? throw new ArgumentNullException(nameof(edits));
        }

        public void AddDelete(int position)
        {
            if (_edits.Count > 0 &&
                _edits[^1] is DiffEdit(DiffEditKind.Delete, var lastPosition, _, var lastLength) &&
                position == lastPosition)
            {
                _edits[^1] = DiffEdit.Delete(lastPosition, lastLength + 1);
            }
            else
            {
                _edits.Add(DiffEdit.Delete(position));
            }
        }

        public void AddInsert(int position, int newTextPosition)
        {
            if (_edits.Count > 0 &&
                _edits[^1] is DiffEdit(DiffEditKind.Insert, var lastPosition, var lastNewTextPosition, var lastLength) &&
                position == lastPosition &&
                newTextPosition == lastNewTextPosition + lastLength)
            {
                _edits[^1] = DiffEdit.Insert(lastPosition, lastNewTextPosition.GetValueOrDefault(), lastLength + 1);
            }
            else
            {
                _edits.Add(DiffEdit.Insert(position, newTextPosition));
            }
        }

        public List<DiffEdit>.Enumerator GetEnumerator()
            => _edits.GetEnumerator();
    }
}
