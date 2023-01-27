// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Microsoft.AspNetCore.Razor.Language;

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public abstract class TagMatchingRuleDescriptor : IEquatable<TagMatchingRuleDescriptor>
{
    private int? _hashCode;
    private IEnumerable<RazorDiagnostic> _allDiagnostics;

    public string TagName { get; protected set; }

    public IReadOnlyList<RequiredAttributeDescriptor> Attributes { get; protected set; }

    public string ParentTag { get; protected set; }

    public TagStructure TagStructure { get; protected set; }

    public bool CaseSensitive { get; protected set; }

    public IReadOnlyList<RazorDiagnostic> Diagnostics { get; protected set; }


    public bool HasErrors
    {
        get
        {
            var allDiagnostics = GetAllDiagnostics();
            var errors = allDiagnostics.Any(diagnostic => diagnostic.Severity == RazorDiagnosticSeverity.Error);

            return errors;
        }
    }

    public virtual IEnumerable<RazorDiagnostic> GetAllDiagnostics()
    {
        if (_allDiagnostics == null)
        {
            var attributeDiagnostics = Attributes.SelectMany(attribute => attribute.Diagnostics);
            var combinedDiagnostics = Diagnostics.Concat(attributeDiagnostics);
            _allDiagnostics = combinedDiagnostics.ToArray();
        }

        return _allDiagnostics;
    }

    public bool Equals(TagMatchingRuleDescriptor other)
    {
        return TagMatchingRuleDescriptorComparer.Default.Equals(this, other);
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as TagMatchingRuleDescriptor);
    }

    public override int GetHashCode()
    {
        _hashCode ??= TagMatchingRuleDescriptorComparer.Default.GetHashCode(this);
        return _hashCode.Value;
    }

    internal string GetDebuggerDisplay()
    {
        var tagName = TagName ?? "*";
        tagName += TagStructure == TagStructure.WithoutEndTag ? "/" : "";
        return $"{TagName ?? "*"}[{string.Join(", ", Attributes.Select(a => DescribeAttribute(a)))}]";
        static string DescribeAttribute(RequiredAttributeDescriptor attribute)
        {
            var name = attribute.Name switch
            {
                null => "*",
                var prefix when attribute.NameComparison == RequiredAttributeDescriptor.NameComparisonMode.PrefixMatch => $"^{prefix}",
                var full => full,
            };

            var value = attribute.Value switch
            {
                null => "",
                var prefix when attribute.ValueComparison == RequiredAttributeDescriptor.ValueComparisonMode.PrefixMatch => $"^={prefix}",
                var suffix when attribute.ValueComparison == RequiredAttributeDescriptor.ValueComparisonMode.SuffixMatch => $"$={suffix}",
                var full => $"={full}",
            };
            return name + value;
        }
    }
}
