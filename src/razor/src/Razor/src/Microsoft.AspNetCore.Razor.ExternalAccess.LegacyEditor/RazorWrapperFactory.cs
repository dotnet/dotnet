// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.CodeAnalysis.Razor.Workspaces.Settings;
using Microsoft.VisualStudio.LegacyEditor.Razor;
using Microsoft.VisualStudio.LegacyEditor.Razor.Parsing;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor;

/// <summary>
///  Creates and caches wrappers for various Razor objects without directly exposing Razor types.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
internal static partial class RazorWrapperFactory
{
    private static readonly ConditionalWeakTable<object, object> s_objectToWrapperMap = new();

    [return: NotNullIfNotNull(nameof(obj))]
    private static TResult? Wrap<TInner, TWrapper, TResult>(object? obj, Func<TInner, TWrapper> createWrapper)
        where TInner : class
        where TResult : class
        where TWrapper : Wrapper<TInner>, TResult
    {
        var inner = CastObject<TInner>(obj);
        if (inner is null)
        {
            return null;
        }

        if (s_objectToWrapperMap.TryGetValue(inner, out var wrapperObj))
        {
            return (TResult)wrapperObj;
        }

        var wrapper = createWrapper(inner);
        s_objectToWrapperMap.Add(inner, wrapper);
        return wrapper;
    }

    private static ImmutableArray<TResult> WrapAll<TInner, TResult>(ImmutableArray<TInner> array, Func<TInner, TResult> createWrapper)
        where TInner : class
        where TResult : class
    {
        using var builder = new PooledArrayBuilder<TResult>(capacity: array.Length);

        foreach (var item in array)
        {
            builder.Add(createWrapper(item));
        }

        return builder.ToImmutableAndClear();
    }

    private static ImmutableArray<TResult> WrapAll<TInner, TResult>(IEnumerable<TInner> items, Func<TInner, TResult> createWrapper)
        where TInner : class
        where TResult : class
    {
        using var builder = new PooledArrayBuilder<TResult>();

        foreach (var item in items)
        {
            builder.Add(createWrapper(item));
        }

        return builder.ToImmutableAndClear();
    }

    private static ImmutableArray<TResult> InitializeArrayWithWrappedItems<TInner, TResult>(
        ref ImmutableArray<TResult> location,
        ImmutableArray<TInner> list,
        Func<TInner, TResult> createWrapper)
        where TInner : class
        where TResult : class
    {
        if (location.IsDefault)
        {
            ImmutableInterlocked.InterlockedInitialize(ref location, WrapAll(list, createWrapper));
        }

        return location;
    }

    private static ImmutableArray<TResult> InitializeArrayWithWrappedItems<TInner, TResult>(
        ref ImmutableArray<TResult> location,
        IEnumerable<TInner> list,
        Func<TInner, TResult> createWrapper)
        where TInner : class
        where TResult : class
    {
        if (location.IsDefault)
        {
            ImmutableInterlocked.InterlockedInitialize(ref location, WrapAll(list, createWrapper));
        }

        return location;
    }

    private static T Unwrap<T>(object obj)
        where T : class
        => ((Wrapper<T>)obj).Object;

    private static IRazorBoundAttributeDescriptor Wrap(BoundAttributeDescriptor obj) => WrapBoundAttributeDescriptor(obj);
    private static IRazorBoundAttributeParameterDescriptor Wrap(BoundAttributeParameterDescriptor obj) => WrapBoundAttributeParameterDescriptor(obj);
    private static IRazorDiagnostic Wrap(RazorDiagnostic obj) => WrapDiagnostic(obj);
    private static IRazorDocumentTracker Wrap(IVisualStudioDocumentTracker obj) => WrapDocumentTracker(obj);
    private static IRazorElementCompletionContext Wrap(ElementCompletionContext obj) => WrapElementCompletionContext(obj);
    private static IRazorParser Wrap(IVisualStudioRazorParser obj) => WrapParser(obj);
    private static IRazorRequiredAttributeDescriptor Wrap(RequiredAttributeDescriptor obj) => WrapRequiredAttributeDescriptor(obj);
    private static IRazorTagHelperDescriptor Wrap(TagHelperDescriptor obj) => WrapTagHelperDescriptor(obj);
    private static IRazorTagMatchingRuleDescriptor Wrap(TagMatchingRuleDescriptor obj) => WrapTagMatchingRuleDescriptor(obj);

    private static ElementCompletionContext Unwrap(IRazorElementCompletionContext obj) => Unwrap<ElementCompletionContext>(obj);
    private static TagHelperBinding Unwrap(IRazorTagHelperBinding obj) => Unwrap<TagHelperBinding>(obj);
    private static TagHelperDescriptor Unwrap(IRazorTagHelperDescriptor obj) => Unwrap<TagHelperDescriptor>(obj);
    private static TagHelperDocumentContext Unwrap(IRazorTagHelperDocumentContext obj) => Unwrap<TagHelperDocumentContext>(obj);

    internal static IRazorBoundAttributeDescriptor WrapBoundAttributeDescriptor(object obj)
        => Wrap<BoundAttributeDescriptor, BoundAttributeDescriptorWrapper, IRazorBoundAttributeDescriptor>(obj, static obj => new BoundAttributeDescriptorWrapper(obj));

    internal static IRazorBoundAttributeParameterDescriptor WrapBoundAttributeParameterDescriptor(object obj)
        => Wrap<BoundAttributeParameterDescriptor, BoundAttributeParameterDescriptorWrapper, IRazorBoundAttributeParameterDescriptor>(obj, static obj => new BoundAttributeParameterDescriptorWrapper(obj));

    internal static IRazorCodeDocument WrapCodeDocument(object obj)
        => Wrap<RazorCodeDocument, CodeDocumentWrapper, IRazorCodeDocument>(obj, static obj => new CodeDocumentWrapper(obj));

    internal static IRazorDiagnostic WrapDiagnostic(object obj)
        => Wrap<RazorDiagnostic, DiagnosticWrapper, IRazorDiagnostic>(obj, static obj => new DiagnosticWrapper(obj));

    internal static IRazorDocumentTracker WrapDocumentTracker(object obj)
        => Wrap<IVisualStudioDocumentTracker, DocumentTrackerWrapper, IRazorDocumentTracker>(obj, static obj => new DocumentTrackerWrapper(obj));

    internal static IRazorEditorFactoryService WrapEditorFactoryService(object obj)
        => Wrap<VisualStudio.LegacyEditor.Razor.IRazorEditorFactoryService, EditorFactoryServiceWrapper, IRazorEditorFactoryService>(obj, obj => new EditorFactoryServiceWrapper(obj));

    internal static IRazorEditorSettingsManager WrapClientSettingsManager(object obj)
        => Wrap<IClientSettingsManager, ClientSettingsManagerWrapper, IRazorEditorSettingsManager>(obj, static obj => new ClientSettingsManagerWrapper(obj));

    internal static IRazorElementCompletionContext WrapElementCompletionContext(object obj)
        => Wrap<ElementCompletionContext, ElementCompletionContextWrapper, IRazorElementCompletionContext>(obj, static obj => new ElementCompletionContextWrapper(obj));

    internal static IRazorParser WrapParser(object obj)
        => Wrap<IVisualStudioRazorParser, ParserWrapper, IRazorParser>(obj, static obj => new ParserWrapper(obj));

    internal static IRazorRequiredAttributeDescriptor WrapRequiredAttributeDescriptor(object obj)
        => Wrap<RequiredAttributeDescriptor, RequiredAttributeDescriptorWrapper, IRazorRequiredAttributeDescriptor>(obj, static obj => new RequiredAttributeDescriptorWrapper(obj));

    internal static IRazorTagHelperBinding WrapTagHelperBinding(object obj)
        => Wrap<TagHelperBinding, TagHelperBindingWrapper, IRazorTagHelperBinding>(obj, static obj => new TagHelperBindingWrapper(obj));

    internal static IRazorTagHelperDescriptor WrapTagHelperDescriptor(object obj)
        => Wrap<TagHelperDescriptor, TagHelperDescriptorWrapper, IRazorTagHelperDescriptor>(obj, static obj => new TagHelperDescriptorWrapper(obj));

    internal static IRazorTagHelperDocumentContext WrapTagHelperDocumentContext(object obj)
        => Wrap<TagHelperDocumentContext, TagHelperDocumentContextWrapper, IRazorTagHelperDocumentContext>(obj, static obj => new TagHelperDocumentContextWrapper(obj));

    internal static IRazorTagHelperCompletionService WrapTagHelperCompletionService(object obj)
        => Wrap<ITagHelperCompletionService, TagHelperCompletionServiceWrapper, IRazorTagHelperCompletionService>(obj, static obj => new TagHelperCompletionServiceWrapper(obj));

    internal static IRazorTagMatchingRuleDescriptor WrapTagMatchingRuleDescriptor(object obj)
        => Wrap<TagMatchingRuleDescriptor, TagMatchingRuleDescriptorWrapper, IRazorTagMatchingRuleDescriptor>(obj, static obj => new TagMatchingRuleDescriptorWrapper(obj));

    internal static IRazorTagHelperFactsService GetWrappedTagHelperFactsService()
        => TagHelperFactsServiceWrapper.Instance;

    internal static RazorSourceSpan ConvertSourceSpan(object obj)
    {
        var span = CastStruct<SourceSpan>(obj);

        return ConvertSourceSpan(span);
    }

    [return: NotNullIfNotNull(nameof(obj))]
    internal static RazorSourceChange? ConvertSourceChange(object? obj)
    {
        return CastObject<SourceChange>(obj) is SourceChange change
            ? new RazorSourceChange(
                Span: ConvertSourceSpan(change.Span),
                NewText: change.NewText)
            : null;
    }

    [return: NotNullIfNotNull(nameof(obj))]
    internal static RazorSourceMapping? ConvertSourceMapping(object? obj)
    {
        return CastObject<SourceMapping>(obj) is SourceMapping mapping
            ? new RazorSourceMapping(
                OriginalSpan: ConvertSourceSpan(mapping.OriginalSpan),
                GeneratedSpan: ConvertSourceSpan(mapping.GeneratedSpan))
            : null;
    }

    private static RazorSourceSpan ConvertSourceSpan(SourceSpan span)
        => new(
            FilePath: span.FilePath,
            AbsoluteIndex: span.AbsoluteIndex,
            LineIndex: span.LineIndex,
            CharacterIndex: span.CharacterIndex,
            Length: span.Length,
            LineCount: span.LineCount,
            EndCharacterIndex: span.EndCharacterIndex);

    [return: NotNullIfNotNull(nameof(obj))]
    private static T? CastObject<T>(object? obj)
        where T : class
    {
        return obj switch
        {
            T result => result,
            null => null,
            _ => throw new ArgumentException($"Expected {typeof(T).FullName}.", nameof(obj))
        };
    }

    private static T CastStruct<T>(object obj)
        where T : struct
    {
        return obj is T result
            ? result
            : throw new ArgumentException($"Expected {typeof(T).FullName}.", nameof(obj));
    }
}
