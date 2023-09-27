// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.NET.Sdk.Razor.SourceGenerators
{
    public partial class RazorSourceGenerator
    {
        /// <summary>
        ///  Helper class that joins together two lists of tag helpers to avoid allocating
        ///  a new array to copy them to.
        /// </summary>
        private sealed class AllTagHelpers : IReadOnlyList<TagHelperDescriptor>
        {
            private static readonly List<TagHelperDescriptor> s_emptyList = new();

            public static readonly AllTagHelpers Empty = new(
                tagHelpersFromCompilation: null,
                tagHelpersFromReferences: null);

            private readonly List<TagHelperDescriptor> _tagHelpersFromCompilation;
            private readonly List<TagHelperDescriptor> _tagHelpersFromReferences;

            private AllTagHelpers(
                List<TagHelperDescriptor>? tagHelpersFromCompilation,
                List<TagHelperDescriptor>? tagHelpersFromReferences)
            {
                _tagHelpersFromCompilation = tagHelpersFromCompilation ?? s_emptyList;
                _tagHelpersFromReferences = tagHelpersFromReferences ?? s_emptyList;
            }

            public static AllTagHelpers Create(
                List<TagHelperDescriptor>? tagHelpersFromCompilation,
                List<TagHelperDescriptor>? tagHelpersFromReferences)
            {
                return tagHelpersFromCompilation is null or [] && tagHelpersFromReferences is null or []
                    ? Empty
                    : new(tagHelpersFromCompilation, tagHelpersFromReferences);
            }

            public TagHelperDescriptor this[int index]
            {
                get
                {
                    if (index >= 0)
                    {
                        return index < _tagHelpersFromCompilation.Count
                            ? _tagHelpersFromCompilation[index]
                            : _tagHelpersFromReferences[index - _tagHelpersFromCompilation.Count];
                    }

                    throw new IndexOutOfRangeException();
                }
            }

            public int Count
                => _tagHelpersFromCompilation.Count + _tagHelpersFromReferences.Count;

            public IEnumerator<TagHelperDescriptor> GetEnumerator()
            {
                foreach (var tagHelper in _tagHelpersFromCompilation)
                {
                    yield return tagHelper;
                }

                foreach (var tagHelper in _tagHelpersFromReferences)
                {
                    yield return tagHelper;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
                => GetEnumerator();
        }
    }
}
