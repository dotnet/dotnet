// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Mvc.Razor.Extensions.Version1_X;

internal class RazorExtensionsDiagnosticFactory
{
    private const string DiagnosticPrefix = "RZ";

    internal static readonly RazorDiagnosticDescriptor ViewComponent_CannotFindMethod =
        new($"{DiagnosticPrefix}3900",
            ViewComponentResources.ViewComponent_CannotFindMethod,
            RazorDiagnosticSeverity.Error);

    public static RazorDiagnostic CreateViewComponent_CannotFindMethod(string tagHelperType)
        => RazorDiagnostic.Create(
            ViewComponent_CannotFindMethod,
            new SourceSpan(SourceLocation.Undefined, contentLength: 0),
            ViewComponentTypes.SyncMethodName,
            ViewComponentTypes.AsyncMethodName,
            tagHelperType);

    internal static readonly RazorDiagnosticDescriptor ViewComponent_AmbiguousMethods =
        new($"{DiagnosticPrefix}3901",
            ViewComponentResources.ViewComponent_AmbiguousMethods,
            RazorDiagnosticSeverity.Error);

    public static RazorDiagnostic CreateViewComponent_AmbiguousMethods(string tagHelperType)
        => RazorDiagnostic.Create(
            ViewComponent_AmbiguousMethods,
            new SourceSpan(SourceLocation.Undefined, contentLength: 0),
            tagHelperType,
            ViewComponentTypes.SyncMethodName,
            ViewComponentTypes.AsyncMethodName);

    internal static readonly RazorDiagnosticDescriptor ViewComponent_AsyncMethod_ShouldReturnTask =
        new($"{DiagnosticPrefix}3902",
            ViewComponentResources.ViewComponent_AsyncMethod_ShouldReturnTask,
            RazorDiagnosticSeverity.Error);

    public static RazorDiagnostic CreateViewComponent_AsyncMethod_ShouldReturnTask(string tagHelperType)
        => RazorDiagnostic.Create(
            ViewComponent_AsyncMethod_ShouldReturnTask,
            new SourceSpan(SourceLocation.Undefined, contentLength: 0),
            ViewComponentTypes.AsyncMethodName,
            tagHelperType,
            nameof(Task));

    internal static readonly RazorDiagnosticDescriptor ViewComponent_SyncMethod_ShouldReturnValue =
        new($"{DiagnosticPrefix}3903",
            ViewComponentResources.ViewComponent_SyncMethod_ShouldReturnValue,
            RazorDiagnosticSeverity.Error);

    public static RazorDiagnostic CreateViewComponent_SyncMethod_ShouldReturnValue(string tagHelperType)
        => RazorDiagnostic.Create(
            ViewComponent_SyncMethod_ShouldReturnValue,
            new SourceSpan(SourceLocation.Undefined, contentLength: 0),
            ViewComponentTypes.SyncMethodName,
            tagHelperType);

    internal static readonly RazorDiagnosticDescriptor ViewComponent_SyncMethod_CannotReturnTask =
        new($"{DiagnosticPrefix}3904",
            ViewComponentResources.ViewComponent_SyncMethod_CannotReturnTask,
            RazorDiagnosticSeverity.Error);

    public static RazorDiagnostic CreateViewComponent_SyncMethod_CannotReturnTask(string tagHelperType)
        => RazorDiagnostic.Create(
            ViewComponent_SyncMethod_CannotReturnTask,
            new SourceSpan(SourceLocation.Undefined, contentLength: 0),
            ViewComponentTypes.SyncMethodName,
            tagHelperType,
            nameof(Task));
}
