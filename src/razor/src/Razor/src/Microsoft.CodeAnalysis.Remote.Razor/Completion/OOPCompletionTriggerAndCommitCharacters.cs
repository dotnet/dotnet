// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.CodeAnalysis.Remote.Razor.Completion;

[Export(typeof(CompletionTriggerAndCommitCharacters)), Shared]
[method: ImportingConstructor]
internal sealed class OOPCompletionTriggerAndCommitCharacters(LanguageServerFeatureOptions options)
    : CompletionTriggerAndCommitCharacters(options);
