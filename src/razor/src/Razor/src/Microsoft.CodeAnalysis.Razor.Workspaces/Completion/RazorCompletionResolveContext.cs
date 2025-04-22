// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;

namespace Microsoft.CodeAnalysis.Razor.Completion;

internal record RazorCompletionResolveContext(string FilePath, ImmutableArray<RazorCompletionItem> CompletionItems) : ICompletionResolveContext;
