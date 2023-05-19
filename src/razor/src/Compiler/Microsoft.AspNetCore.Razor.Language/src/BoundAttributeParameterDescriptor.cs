// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.AspNetCore.Razor.Language;

public abstract class BoundAttributeParameterDescriptor : IEquatable<BoundAttributeParameterDescriptor>
{
    private const int ContainsDiagnosticsBit = 1 << 0;
    private const int IsEnumBit = 1 << 1;
    private const int IsStringPropertyBit = 1 << 2;
    private const int IsBooleanPropertyBit = 1 << 3;
    private const int CaseSensitiveBit = 1 << 4;

    private int _flags;
    private DocumentationObject _documentationObject;

    private bool HasFlag(int flag) => (_flags & flag) != 0;
    private void SetFlag(int toSet) => ThreadSafeFlagOperations.Set(ref _flags, toSet);
    private void ClearFlag(int toClear) => ThreadSafeFlagOperations.Clear(ref _flags, toClear);
    private void SetOrClearFlag(int toChange, bool value) => ThreadSafeFlagOperations.SetOrClear(ref _flags, toChange, value);

    protected BoundAttributeParameterDescriptor(string kind)
    {
        Kind = kind;
    }

    public string Kind { get; }

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

    public string Name { get; protected set; }

    public string TypeName { get; protected set; }

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

    public bool Equals(BoundAttributeParameterDescriptor other)
    {
        return BoundAttributeParameterDescriptorComparer.Default.Equals(this, other);
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as BoundAttributeParameterDescriptor);
    }

    public override int GetHashCode()
    {
        return BoundAttributeParameterDescriptorComparer.Default.GetHashCode(this);
    }
}
