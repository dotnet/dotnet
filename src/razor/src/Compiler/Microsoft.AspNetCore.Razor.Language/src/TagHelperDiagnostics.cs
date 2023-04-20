// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Microsoft.AspNetCore.Razor.Language;

internal static class TagHelperDiagnostics
{
    private static readonly ConditionalWeakTable<object, IReadOnlyList<RazorDiagnostic>> s_diagnosticsTable = new();

    private static void AddDiagnosticsToTable(object obj, IReadOnlyList<RazorDiagnostic> diagnostics)
    {
        if (diagnostics.Count > 0)
        {
            s_diagnosticsTable.Add(obj, diagnostics);
        }
    }

    private static IReadOnlyList<RazorDiagnostic> GetDiagnosticsFromTable(object obj)
        => s_diagnosticsTable.TryGetValue(obj, out var diagnostics)
            ? diagnostics
            : Array.Empty<RazorDiagnostic>();

    private static void RemoveDiagnosticsFromTable(object obj)
        => s_diagnosticsTable.Remove(obj);

    public static void AddDiagnostics(AllowedChildTagDescriptor descriptor, IReadOnlyList<RazorDiagnostic> diagnostics)
        => AddDiagnosticsToTable(descriptor, diagnostics);

    public static void AddDiagnostics(BoundAttributeDescriptor descriptor, IReadOnlyList<RazorDiagnostic> diagnostics)
        => AddDiagnosticsToTable(descriptor, diagnostics);

    public static void AddDiagnostics(BoundAttributeParameterDescriptor descriptor, IReadOnlyList<RazorDiagnostic> diagnostics)
        => AddDiagnosticsToTable(descriptor, diagnostics);

    public static void AddDiagnostics(RequiredAttributeDescriptor descriptor, IReadOnlyList<RazorDiagnostic> diagnostics)
        => AddDiagnosticsToTable(descriptor, diagnostics);

    public static void AddDiagnostics(TagHelperDescriptor descriptor, IReadOnlyList<RazorDiagnostic> diagnostics)
        => AddDiagnosticsToTable(descriptor, diagnostics);

    public static void AddDiagnostics(TagMatchingRuleDescriptor descriptor, IReadOnlyList<RazorDiagnostic> diagnostics)
        => AddDiagnosticsToTable(descriptor, diagnostics);

    public static IReadOnlyList<RazorDiagnostic> GetDiagnostics(AllowedChildTagDescriptor descriptor)
        => GetDiagnosticsFromTable(descriptor);

    public static IReadOnlyList<RazorDiagnostic> GetDiagnostics(BoundAttributeDescriptor descriptor)
        => GetDiagnosticsFromTable(descriptor);

    public static IReadOnlyList<RazorDiagnostic> GetDiagnostics(BoundAttributeParameterDescriptor descriptor)
        => GetDiagnosticsFromTable(descriptor);

    public static IReadOnlyList<RazorDiagnostic> GetDiagnostics(RequiredAttributeDescriptor descriptor)
        => GetDiagnosticsFromTable(descriptor);

    public static IReadOnlyList<RazorDiagnostic> GetDiagnostics(TagHelperDescriptor descriptor)
        => GetDiagnosticsFromTable(descriptor);

    public static IReadOnlyList<RazorDiagnostic> GetDiagnostics(TagMatchingRuleDescriptor descriptor)
        => GetDiagnosticsFromTable(descriptor);

    public static void RemoveDiagnostics(AllowedChildTagDescriptor descriptor)
        => RemoveDiagnosticsFromTable(descriptor);

    public static void RemoveDiagnostics(BoundAttributeDescriptor descriptor)
        => RemoveDiagnosticsFromTable(descriptor);

    public static void RemoveDiagnostics(BoundAttributeParameterDescriptor descriptor)
        => RemoveDiagnosticsFromTable(descriptor);

    public static void RemoveDiagnostics(RequiredAttributeDescriptor descriptor)
        => RemoveDiagnosticsFromTable(descriptor);

    public static void RemoveDiagnostics(TagHelperDescriptor descriptor)
        => RemoveDiagnosticsFromTable(descriptor);

    public static void RemoveDiagnostics(TagMatchingRuleDescriptor descriptor)
        => RemoveDiagnosticsFromTable(descriptor);
}
