// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.AspNetCore.Razor.TextDifferencing;

internal abstract partial class TextDiffer
{
    protected readonly struct DiffEdit
    {
        public DiffEditKind Kind { get; }
        public int Position { get; }
        public int? NewTextPosition { get; }
        public int Length { get; }

        private DiffEdit(DiffEditKind kind, int position, int? newTextPosition, int length)
        {
            Kind = kind;
            Position = position;
            NewTextPosition = newTextPosition;
            Length = length;
        }

        public override string ToString()
        {
            using var _ = StringBuilderPool.GetPooledObject(out var builder);

            builder.Append($"{Kind}: Position = {Position}");

            if (NewTextPosition is int newTextPosition)
            {
                builder.Append($", NewTextPosition = {newTextPosition}");
            }

            builder.Append($", Length = {Length}");

            return builder.ToString();
        }

        public void Deconstruct(out DiffEditKind kind, out int position, out int? newTextPosition, out int length)
            => (kind, position, newTextPosition, length) = (Kind, Position, NewTextPosition, Length);

        public static DiffEdit Insert(int position, int newTextPosition, int length = 1)
            => new(DiffEditKind.Insert, position, newTextPosition, length);

        public static DiffEdit Delete(int position, int length = 1)
            => new(DiffEditKind.Delete, position, newTextPosition: null, length);
    }
}
