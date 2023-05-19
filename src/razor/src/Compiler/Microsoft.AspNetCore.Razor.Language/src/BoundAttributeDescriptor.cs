// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language.Components;

namespace Microsoft.AspNetCore.Razor.Language;

/// <summary>
/// A metadata class describing a tag helper attribute.
/// </summary>
public abstract class BoundAttributeDescriptor : IEquatable<BoundAttributeDescriptor>
{
    private const int ContainsDiagnosticsBit = 1 << 0;
    private const int IsDirectiveAttributeComputedBit = 1 << 1;
    private const int IsDirectiveAttributeBit = 1 << 2;
    private const int IsIndexerStringPropertyBit = 1 << 3;
    private const int IsIndexerBooleanPropertyBit = 1 << 4;
    private const int IsEnumBit = 1 << 5;
    private const int IsStringPropertyBit = 1 << 6;
    private const int IsBooleanPropertyBit = 1 << 7;
    private const int IsEditorRequiredBit = 1 << 8;
    private const int HasIndexerBit = 1 << 9;
    private const int CaseSensitiveBit = 1 << 10;

    private int _flags;
    private DocumentationObject _documentationObject;

    private bool HasFlag(int flag) => (_flags & flag) != 0;
    private void SetFlag(int toSet) => ThreadSafeFlagOperations.Set(ref _flags, toSet);
    private void ClearFlag(int toClear) => ThreadSafeFlagOperations.Clear(ref _flags, toClear);
    private void SetOrClearFlag(int toChange, bool value) => ThreadSafeFlagOperations.SetOrClear(ref _flags, toChange, value);

    protected BoundAttributeDescriptor(string kind)
    {
        Kind = kind;
    }

    public string Kind { get; }

    public bool IsIndexerStringProperty
    {
        get => HasFlag(IsIndexerStringPropertyBit);
        protected set => SetOrClearFlag(IsIndexerStringPropertyBit, value);
    }

    public bool IsIndexerBooleanProperty
    {
        get => HasFlag(IsIndexerBooleanPropertyBit);
        protected set => SetOrClearFlag(IsIndexerBooleanPropertyBit, value);
    }

    public bool IsEnum
    {
        get => HasFlag(IsEnumBit);
        protected set => SetOrClearFlag(IsEnumBit, value);
    }

    public bool IsStringProperty
    {
        get => HasFlag(IsStringPropertyBit);
        protected set => SetOrClearFlag(IsStringPropertyBit, value);
    }

    public bool IsBooleanProperty
    {
        get => HasFlag(IsBooleanPropertyBit);
        protected set => SetOrClearFlag(IsBooleanPropertyBit, value);
    }

    internal bool IsEditorRequired
    {
        get => HasFlag(IsEditorRequiredBit);
        private protected set => SetOrClearFlag(IsEditorRequiredBit, value);
    }

    public string Name { get; protected set; }

    public string IndexerNamePrefix { get; protected set; }

    public string TypeName { get; protected set; }

    public string IndexerTypeName { get; protected set; }

    public bool HasIndexer
    {
        get => HasFlag(HasIndexerBit);
        protected set => SetOrClearFlag(HasIndexerBit, value);
    }

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

    public bool CaseSensitive
    {
        get => HasFlag(CaseSensitiveBit);
        protected set => SetOrClearFlag(CaseSensitiveBit, value);
    }

    public bool IsDirectiveAttribute
    {
        get
        {
            if (!HasFlag(IsDirectiveAttributeComputedBit))
            {
                // If we haven't computed this value yet, compute it by checking the metadata.
                var isDirectiveAttribute = Metadata.TryGetValue(ComponentMetadata.Common.DirectiveAttribute, out var value) && value == bool.TrueString;
                if (isDirectiveAttribute)
                {
                    SetFlag(IsDirectiveAttributeBit | IsDirectiveAttributeComputedBit);
                }
                else
                {
                    ClearFlag(IsDirectiveAttributeBit);
                    SetFlag(IsDirectiveAttributeComputedBit);
                }
            }

            return HasFlag(IsDirectiveAttributeBit);
        }
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

    public virtual IReadOnlyList<BoundAttributeParameterDescriptor> BoundAttributeParameters { get; protected set; }

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

    public override string ToString()
    {
        return DisplayName ?? base.ToString();
    }

    public bool Equals(BoundAttributeDescriptor other)
    {
        return BoundAttributeDescriptorComparer.Default.Equals(this, other);
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as BoundAttributeDescriptor);
    }

    public override int GetHashCode()
    {
        return BoundAttributeDescriptorComparer.Default.GetHashCode(this);
    }
}
