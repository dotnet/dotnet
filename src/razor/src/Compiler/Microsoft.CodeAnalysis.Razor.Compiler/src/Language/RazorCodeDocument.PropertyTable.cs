// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Razor.Language.Intermediate;

namespace Microsoft.AspNetCore.Razor.Language;

public sealed partial class RazorCodeDocument
{
    /// <summary>
    ///  Represents a set of mutable values associated with a <see cref="RazorCodeDocument"/>.
    /// </summary>
    private readonly struct PropertyTable()
    {
        // To add a mutable value, increase Size by 1 and add a new property below.
        // Use a Property<T> for reference types or a BoxedProperty<T> for value types.

        private const int Size = 10;

        private readonly object?[] _values = new object?[Size];

        public Property<TagHelperCollection> TagHelpers => new(_values, 0);
        public Property<TagHelperCollection> ReferencedTagHelpers => new(_values, 1);
        public Property<RazorSyntaxTree> PreTagHelperSyntaxTree => new(_values, 2);
        public Property<RazorSyntaxTree> SyntaxTree => new(_values, 3);
        public BoxedProperty<ImmutableArray<RazorSyntaxTree>> ImportSyntaxTrees => new(_values, 4);
        public Property<TagHelperDocumentContext> TagHelperContext => new(_values, 5);
        public Property<DocumentIntermediateNode> DocumentNode => new(_values, 6);
        public Property<RazorCSharpDocument> CSharpDocument => new(_values, 7);
        public Property<RazorHtmlDocument> HtmlDocument => new(_values, 8);
        public BoxedProperty<(string name, SourceSpan? span)> NamespaceInfo => new(_values, 9);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Do not use. Present to support the legacy editor", error: false)]
        public PropertyTable Clone()
        {
            var clone = new PropertyTable();
            Array.Copy(_values, clone._values, Size);

            return clone;
        }

        /// <summary>
        ///  Provides access to a specific slot within an array for a given reference type.
        /// </summary>
        /// <param name="values">The array of values.</param>
        /// <param name="index">The index within <paramref name="values"/> to access.</param>
        /// <remarks>
        ///  A <see langword="null"/> value in the slot indicates that the value is not present.
        /// </remarks>
        public readonly ref struct Property<T>(object?[] values, int index)
            where T : class
        {
            // We can use a ref field to access the array slot directly on modern .NET.
            // On NetFx, we index into the array for each access.
#if NET
            private readonly ref object? _value = ref values[index];
#endif

            public T? Value
#if NET
                => (T?)_value;
#else
                => (T?)values[index];
#endif

            public void SetValue(T? value)
#if NET
                => _value = value;
#else
                => values[index] = value;
#endif

            public bool TryGetValue([NotNullWhen(true)] out T? result)
            {
                result = Value;
                return result is not null;
            }

            public T RequiredValue
                => Value.AssumeNotNull();
        }

        /// <summary>
        ///  Provides access to a specific slot within an array for a given value type.
        ///  A <see cref="StrongBox{T}"/> is employed to avoid boxing and unboxing the value.
        /// </summary>
        /// <param name="values">The array of values.</param>
        /// <param name="index">The index within <paramref name="values"/> to access.</param>
        public readonly ref struct BoxedProperty<T>(object?[] values, int index)
            where T : struct
        {
            private readonly Property<StrongBox<T>> _box = new(values, index);

            public T? Value => _box.Value?.Value;

            public bool TryGetValue(out T result)
            {
                if (_box.TryGetValue(out var box))
                {
                    result = box.Value;
                    return true;
                }

                result = default;
                return false;
            }

            public void SetValue(T value)
            {
                if (_box.TryGetValue(out var box))
                {
                    // If we've already created a StrongBox, just update the value.
                    box.Value = value;
                }
                else
                {
                    // Otherwise, create a new StrongBox.
                    box = new(value);
                    _box.SetValue(box);
                }
            }
        }
    }
}
