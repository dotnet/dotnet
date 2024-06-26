// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using Microsoft.CodeAnalysis.Razor.FoldingRanges;

namespace Microsoft.CodeAnalysis.Remote.Razor.FoldingRanges;

[Shared]
[Export(typeof(IRazorFoldingRangeProvider))]
internal sealed class RemoteRazorCodeBlockFoldingProvider : RazorCodeBlockFoldingProvider;

[Shared]
[Export(typeof(IRazorFoldingRangeProvider))]
internal sealed class RemoteRazorCSharpStatementFoldingProvider : RazorCSharpStatementFoldingProvider;

[Shared]
[Export(typeof(IRazorFoldingRangeProvider))]
internal sealed class RemoteRazorCSharpStatementKeywordFoldingProvider : RazorCSharpStatementKeywordFoldingProvider;

[Shared]
[Export(typeof(IRazorFoldingRangeProvider))]
internal sealed class RemoteSectionDirectiveFoldingProvider : SectionDirectiveFoldingProvider;

[Shared]
[Export(typeof(IRazorFoldingRangeProvider))]
internal sealed class RemoteUsingsFoldingRangeProvider : UsingsFoldingRangeProvider;
