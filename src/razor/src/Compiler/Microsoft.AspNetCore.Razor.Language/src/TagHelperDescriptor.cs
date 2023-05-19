// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.AspNetCore.Razor.Language;

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public abstract class TagHelperDescriptor : IEquatable<TagHelperDescriptor>
{
    private const int ContainsDiagnosticsBit = 1 << 0;
    private const int IsComponentFullyQualifiedNameMatchCacheSetBit = 1 << 1;
    private const int IsComponentFullyQualifiedNameMatchCacheBit = 1 << 2;
    private const int IsChildContentTagHelperCacheSetBit = 1 << 3;
    private const int IsChildContentTagHelperCacheBit = 1 << 4;
    private const int CaseSensitiveBit = 1 << 5;

    private int _flags;
    private int? _hashCode;
    private DocumentationObject _documentationObject;

    private IEnumerable<RazorDiagnostic> _allDiagnostics;
    private BoundAttributeDescriptor[] _editorRequiredAttributes;

    protected TagHelperDescriptor(string kind)
    {
        Kind = kind;
    }

    private bool HasFlag(int flag) => (_flags & flag) != 0;
    private void SetFlag(int toSet) => ThreadSafeFlagOperations.Set(ref _flags, toSet);
    private void ClearFlag(int toClear) => ThreadSafeFlagOperations.Clear(ref _flags, toClear);
    private void SetOrClearFlag(int toChange, bool value) => ThreadSafeFlagOperations.SetOrClear(ref _flags, toChange, value);

    public string Kind { get; }

    public string Name { get; protected set; }

    public IReadOnlyList<TagMatchingRuleDescriptor> TagMatchingRules { get; protected set; }

    public string AssemblyName { get; protected set; }

    public IReadOnlyList<BoundAttributeDescriptor> BoundAttributes { get; protected set; }

    public IReadOnlyList<AllowedChildTagDescriptor> AllowedChildTags { get; protected set; }

    public string Documentation
    {
        get => _documentationObject.GetText();
        protected set => _documentationObject = new(value);
    }

    internal DocumentationObject DocumentationObject
    {
        get => _documentationObject;
        set => _documentationObject = value;
    }

    public string DisplayName { get; protected set; }

    public string TagOutputHint { get; protected set; }

    public bool CaseSensitive
    {
        get => HasFlag(CaseSensitiveBit);
        protected set => SetOrClearFlag(CaseSensitiveBit, value);
    }

    public IReadOnlyList<RazorDiagnostic> Diagnostics
    {
        get => HasFlag(ContainsDiagnosticsBit)
            ? TagHelperDiagnostics.GetDiagnostics(this)
            : Array.Empty<RazorDiagnostic>();

        protected set
        {
            if (value?.Count > 0)
            {
                TagHelperDiagnostics.AddDiagnostics(this, value);
                SetFlag(ContainsDiagnosticsBit);
            }
            else if (HasFlag(ContainsDiagnosticsBit))
            {
                TagHelperDiagnostics.RemoveDiagnostics(this);
                ClearFlag(ContainsDiagnosticsBit);
            }
        }
    }

    public IReadOnlyDictionary<string, string> Metadata { get; protected set; }

    internal bool? IsComponentFullyQualifiedNameMatchCache
    {
        get => GetTriStateFlags(isSetFlag: IsComponentFullyQualifiedNameMatchCacheSetBit, isOnFlag: IsComponentFullyQualifiedNameMatchCacheBit);
        set => UpdateTriStateFlags(value, isSetFlag: IsComponentFullyQualifiedNameMatchCacheSetBit, isOnFlag: IsComponentFullyQualifiedNameMatchCacheBit);
    }

    internal bool? IsChildContentTagHelperCache
    {
        get => GetTriStateFlags(isSetFlag: IsChildContentTagHelperCacheSetBit, isOnFlag: IsChildContentTagHelperCacheBit);
        set => UpdateTriStateFlags(value, isSetFlag: IsChildContentTagHelperCacheSetBit, isOnFlag: IsChildContentTagHelperCacheBit);
    }

    private bool? GetTriStateFlags(int isSetFlag, int isOnFlag)
    {
        var flags = _flags;

        if ((flags & isSetFlag) == 0)
        {
            return null;
        }

        return (flags & isOnFlag) != 0;
    }

    private void UpdateTriStateFlags(bool? value, int isSetFlag, int isOnFlag)
    {
        switch (value)
        {
            case true:
                SetFlag(isSetFlag | isOnFlag);
                break;

            case false:
                ClearFlag(isOnFlag);
                SetFlag(isSetFlag);
                break;

            case null:
                ClearFlag(isSetFlag);
                break;
        }
    }

    internal BoundAttributeDescriptor[] EditorRequiredAttributes
    {
        get
        {
            _editorRequiredAttributes ??= GetEditorRequiredAttributes(BoundAttributes);
            return _editorRequiredAttributes;

            static BoundAttributeDescriptor[] GetEditorRequiredAttributes(IReadOnlyList<BoundAttributeDescriptor> attributes)
            {
                var count = attributes.Count;

                if (count == 0)
                {
                    return Array.Empty<BoundAttributeDescriptor>();
                }

                using var results = new PooledList<BoundAttributeDescriptor>();

                for (var i = 0; i < count; i++)
                {
                    if (attributes[i] is { IsEditorRequired: true } editorRequiredAttribute)
                    {
                        results.Add(editorRequiredAttribute);
                    }
                }

                return results.ToArray();
            }
        }
    }

    public bool HasErrors
    {
        get
        {
            if (!HasFlag(ContainsDiagnosticsBit))
            {
                return false;
            }

            var errors = Diagnostics.Any(diagnostic => diagnostic.Severity == RazorDiagnosticSeverity.Error);

            return errors;
        }
    }

    public virtual IEnumerable<RazorDiagnostic> GetAllDiagnostics()
    {
        if (_allDiagnostics == null)
        {
            using var diagnostics = new PooledList<RazorDiagnostic>();

            foreach (var allowedChildTag in AllowedChildTags)
            {
                diagnostics.AddRange(allowedChildTag.Diagnostics);
            }

            foreach (var attribute in BoundAttributes)
            {
                diagnostics.AddRange(attribute.Diagnostics);
            }

            // BUG?: Diagnostics on BoundAttributeParameterDescriptors are not collected here.
            // https://github.com/dotnet/razor/issues/8544

            foreach (var rule in TagMatchingRules)
            {
                diagnostics.AddRange(rule.GetAllDiagnostics());
            }

            diagnostics.AddRange(Diagnostics);

            _allDiagnostics = diagnostics.ToArray();
        }

        return _allDiagnostics;
    }

    public override string ToString()
    {
        return DisplayName ?? base.ToString();
    }

    public bool Equals(TagHelperDescriptor other)
    {
        return TagHelperDescriptorComparer.Default.Equals(this, other);
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as TagHelperDescriptor);
    }

    public override int GetHashCode()
    {
        // TagHelperDescriptors are immutable instances and it should be safe to cache it's hashes once.
        return _hashCode ??= TagHelperDescriptorComparer.Default.GetHashCode(this);
    }

    private string GetDebuggerDisplay()
    {
        return $"{DisplayName} - {string.Join(" | ", TagMatchingRules.Select(r => r.GetDebuggerDisplay()))}";
    }
}
