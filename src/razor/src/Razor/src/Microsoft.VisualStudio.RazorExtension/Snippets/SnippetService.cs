// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.Editor.Razor.Snippets;
using Microsoft.VisualStudio.ProjectSystem.VS;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.RazorExtension.Snippets;

internal class SnippetService
{
    private readonly JoinableTaskFactory _joinableTaskFactory;
    private readonly IAsyncServiceProvider _serviceProvider;
    private readonly SnippetCache _snippetCache;
    private IVsExpansionManager? _vsExpansionManager;

    private readonly object _cacheGuard = new();

    private static readonly Guid s_CSharpLanguageId = new("694dd9b6-b865-4c5b-ad85-86356e9c88dc");
    private static readonly Guid s_HtmlLanguageId = new("9bbfd173-9770-47dc-b191-651b7ff493cd");

    private static readonly Dictionary<Guid, ImmutableHashSet<string>> s_ignoredSnippets = new()
    {
        {
            // These are identified as the snippets that are provided by the C# Language Server. The list is found
            // in https://github.com/dotnet/roslyn/blob/8eb40e64564a2a8d44be2fde9b079605f1f10e0f/src/Features/LanguageServer/Protocol/Handler/InlineCompletions/InlineCompletionsHandler.cs#L41
            s_CSharpLanguageId,
            ImmutableHashSet.Create(
                "~", "Attribute", "checked", "class", "ctor", "cw", "do", "else", "enum", "equals", "Exception", "for", "foreach", "forr",
                "if", "indexer", "interface", "invoke", "iterator", "iterindex", "lock", "mbox", "namespace", "#if", "#region", "prop",
                "propfull", "propg", "sim", "struct", "svm", "switch", "try", "tryf", "unchecked", "unsafe", "using", "while")
        },
        {
            // Currently no HTML snippets are ignored
            s_HtmlLanguageId, ImmutableHashSet<string>.Empty
        }
    };

    public SnippetService(
        JoinableTaskFactory joinableTaskFactory,
        IAsyncServiceProvider serviceProvider,
        SnippetCache snippetCache)
    {
        _joinableTaskFactory = joinableTaskFactory;
        _serviceProvider = serviceProvider;
        _snippetCache = snippetCache;
        _ = _joinableTaskFactory.RunAsync(InitializeAsync);
    }

    private async Task InitializeAsync()
    {
        await _joinableTaskFactory.SwitchToMainThreadAsync();

        var textManager = (IVsTextManager2?)await _serviceProvider.GetServiceAsync(typeof(SVsTextManager)).ConfigureAwait(true);
        if (textManager is null)
        {
            return;
        }

        if (textManager.GetExpansionManager(out _vsExpansionManager) == VSConstants.S_OK)
        {
            // Call the asynchronous IExpansionManager API from a background thread
            await TaskScheduler.Default;
            await PopulateAsync().ConfigureAwait(false);
        }
    }

    private async Task PopulateAsync()
    {
        var csharpExpansionEnumerator = await GetExpansionEnumeratorAsync(s_CSharpLanguageId).ConfigureAwait(false);
        var htmlExpansionEnumerator = await GetExpansionEnumeratorAsync(s_HtmlLanguageId).ConfigureAwait(false);

        // The rest of the process requires being on the UI thread, see the explanation on
        // PopulateSnippetCacheFromExpansionEnumeration for details
        await _joinableTaskFactory.SwitchToMainThreadAsync();
        PopulateSnippetCacheFromExpansionEnumeration(
            (SnippetLanguage.CSharp, csharpExpansionEnumerator),
            (SnippetLanguage.Html, htmlExpansionEnumerator));
    }

    private Task<IVsExpansionEnumeration> GetExpansionEnumeratorAsync(Guid languageGuid)
    {
        _vsExpansionManager.AssumeNotNull();
        var expansionManager = (IExpansionManager)_vsExpansionManager;

        return expansionManager.EnumerateExpansionsAsync(
                languageGuid,
                0, // shortCutOnly
                Array.Empty<string>(), // types
                0, // countTypes
                1, // includeNULLTypes
                1 // includeDulicates: Allows snippets with the same title but different shortcuts
        );
    }

    /// <remarks>
    /// This method must be called on the UI thread because it eventually calls into
    /// IVsExpansionEnumeration.Next, which must be called on the UI thread due to an issue
    /// with how the call is marshalled.
    /// 
    /// The second parameter for IVsExpansionEnumeration.Next is defined like this:
    ///    [ComAliasName("Microsoft.VisualStudio.TextManager.Interop.VsExpansion")] IntPtr[] rgelt
    ///
    /// We pass a pointer for rgelt that we expect to be populated as the result. This
    /// eventually calls into the native CExpansionEnumeratorShim::Next method, which has the
    /// same contract of expecting a non-null rgelt that it can drop expansion data into. When
    /// we call from the UI thread, this transition from managed code to the
    /// CExpansionEnumeratorShim goes smoothly and everything works.
    ///
    /// When we call from a background thread, the COM marshaller has to move execution to the
    /// UI thread, and as part of this process it uses the interface as defined in the idl to
    /// set up the appropriate arguments to pass. The same parameter from the idl is defined as
    ///    [out, size_is(celt), length_is(*pceltFetched)] VsExpansion **rgelt
    ///
    /// Because rgelt is specified as an <c>out</c> parameter, the marshaller is discarding the
    /// pointer we passed and substituting the null reference. This then causes a null
    /// reference exception in the shim. Calling from the UI thread avoids this marshaller.
    /// </remarks>
    private void PopulateSnippetCacheFromExpansionEnumeration(params (SnippetLanguage language, IVsExpansionEnumeration expansionEnumerator)[] enumerators)
    {
        _joinableTaskFactory.Context.AssertUIThread();

        foreach (var (language, enumerator) in enumerators)
        {
            _snippetCache.Update(language, ExtractSnippetInfo(language, enumerator));
        }
    }

    private ImmutableArray<SnippetInfo> ExtractSnippetInfo(SnippetLanguage language, IVsExpansionEnumeration expansionEnumerator)
    {
        _joinableTaskFactory.Context.AssertUIThread();

        var snippetInfo = new VsExpansion();
        var pSnippetInfo = new IntPtr[1];

        try
        {
            // Allocate enough memory for one VSExpansion structure. This memory is filled in by the Next method.
            pSnippetInfo[0] = Marshal.AllocCoTaskMem(Marshal.SizeOf(snippetInfo));

            var langGuid = language == SnippetLanguage.CSharp
                ? s_CSharpLanguageId
                : s_HtmlLanguageId;

            var toIgnore = s_ignoredSnippets[langGuid];
            var result = expansionEnumerator.GetCount(out var count);
            if (result != HResult.OK)
            {
                return ImmutableArray<SnippetInfo>.Empty;
            }

            using var snippetListBuilder = new PooledArrayBuilder<SnippetInfo>();

            for (uint i = 0; i < count; i++)
            {
                result = expansionEnumerator.Next(1, pSnippetInfo, out var fetched);
                if (result != HResult.OK)
                {
                    continue;
                }

                if (fetched > 0)
                {
                    // Convert the returned blob of data into a structure that can be read in managed code.
                    snippetInfo = ConvertToVsExpansionAndFree(pSnippetInfo[0]);

                    if (!string.IsNullOrEmpty(snippetInfo.shortcut) && !toIgnore.Contains(snippetInfo.shortcut))
                    {
                        snippetListBuilder.Add(new SnippetInfo(snippetInfo.shortcut, snippetInfo.title, snippetInfo.description, snippetInfo.path, language));
                    }
                }
            }

            return snippetListBuilder.ToImmutable();
        }
        finally
        {
            Marshal.FreeCoTaskMem(pSnippetInfo[0]);
        }
    }

    private static VsExpansion ConvertToVsExpansionAndFree(IntPtr expansionPtr)
    {
        var buffer = (VsExpansionWithIntPtrs)Marshal.PtrToStructure(expansionPtr, typeof(VsExpansionWithIntPtrs));
        var expansion = new VsExpansion();

        ConvertToStringAndFree(ref buffer.DescriptionPtr, ref expansion.description);
        ConvertToStringAndFree(ref buffer.PathPtr, ref expansion.path);
        ConvertToStringAndFree(ref buffer.ShortcutPtr, ref expansion.shortcut);
        ConvertToStringAndFree(ref buffer.TitlePtr, ref expansion.title);

        return expansion;
    }

    private static void ConvertToStringAndFree(ref IntPtr ptr, ref string? str)
    {
        if (ptr != IntPtr.Zero)
        {
            str = Marshal.PtrToStringBSTR(ptr);
            Marshal.FreeBSTR(ptr);
            ptr = IntPtr.Zero;
        }
    }

    /// <summary>
    /// This structure is used to facilitate the interop calls with IVsExpansionEnumeration.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    private struct VsExpansionWithIntPtrs
    {
        public IntPtr PathPtr;
        public IntPtr TitlePtr;
        public IntPtr ShortcutPtr;
        public IntPtr DescriptionPtr;
    }
}
