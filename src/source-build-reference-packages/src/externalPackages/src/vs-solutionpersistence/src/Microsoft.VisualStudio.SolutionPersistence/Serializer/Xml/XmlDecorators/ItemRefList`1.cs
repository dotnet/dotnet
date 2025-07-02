// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using Microsoft.VisualStudio.SolutionPersistence.Model;
using Microsoft.VisualStudio.SolutionPersistence.Utilities;

namespace Microsoft.VisualStudio.SolutionPersistence.Serializer.Xml.XmlDecorators;

/// <summary>
/// This provides a list of decorators that are referenced by a unique identifier.
/// This is used to cache unique items from the Xml DOM so they can be quickly referenced.
/// The list is a type of dictionary where the ItemRef is the key.
/// </summary>
/// <typeparam name="T">The decorator type this represents.</typeparam>
/// <param name="ignoreCase">Should this consider keys with different cases the same.</param>
[DebuggerDisplay("{items?.Count} Items, {invalidItems?.Count} Invalid Items")]
internal readonly struct ItemRefList<T>(bool ignoreCase)
    where T : XmlDecorator, IItemRefDecorator
{
    private readonly Lictionary<string, T> items = new Lictionary<string, T>(0, ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);

    public ItemRefList()
        : this(ignoreCase: false)
    {
    }

    internal readonly bool IgnoreCase { get; } = ignoreCase;

    internal readonly int ItemsCount => this.items.Count;

    internal readonly void Add(T item)
    {
        // Missing Name attribute.
        if (!item.IsValid() || item.ItemRef is null)
        {
            throw SolutionException.Create(string.Format(Errors.InvalidItemRef_Args2, item.ItemRefAttribute, item.ElementName), item, SolutionErrorType.InvalidItemRef);
        }
        else
        {
            if (!this.items.TryAdd(item.ItemRef, item))
            {
                // Duplicate Name attribute.
                throw SolutionException.Create(string.Format(Errors.DuplicateItemRef_Args2, item.ItemRef, item.ElementName), item, SolutionErrorType.DuplicateItemRef);
            }
        }
    }

    internal readonly T? FirstOrDefault() => this.items.Count > 0 ? this.items[0] : null;

    // Finds the item that would be immediately after the given item ref.
    internal readonly bool TryFindNext(string itemRef, out T? item)
    {
        return this.items.TryFindNext(itemRef, out item);
    }

    internal readonly void Remove(T item)
    {
        _ = this.items.Remove(item.ItemRef);
    }

    internal readonly EnumForwarder GetItems()
    {
        return new EnumForwarder(this);
    }

    internal ref struct EnumForwarder(ItemRefList<T> me)
    {
        public readonly ItemsEnumerator GetEnumerator() => new ItemsEnumerator(me.items.GetEnumerator());
    }

    internal ref struct ItemsEnumerator(List<KeyValuePair<string, T>>.Enumerator enumerator)
    {
        public T Current => enumerator.Current.Value;

        public bool MoveNext() => enumerator.MoveNext();
    }

    private sealed class OrdinalComparer : IComparer<T>
    {
        internal static readonly OrdinalComparer Instance = new OrdinalComparer();

        public int Compare(T? x, T? y) => StringComparer.Ordinal.Compare(x?.ItemRef, y?.ItemRef);
    }

    private sealed class OrdinalIgnoreCaseComparer : IComparer<T>
    {
        internal static readonly OrdinalIgnoreCaseComparer Instance = new OrdinalIgnoreCaseComparer();

        public int Compare(T? x, T? y) => StringComparer.OrdinalIgnoreCase.Compare(x?.ItemRef, y?.ItemRef);
    }
}
