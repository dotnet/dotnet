// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

[Export(typeof(CompletionTriggerAndCommitCharacters))]
[method: ImportingConstructor]
internal sealed class CohostCompletionTriggerAndCommitCharacters(LanguageServerFeatureOptions languageServerFeatureOptions) : CompletionTriggerAndCommitCharacters(languageServerFeatureOptions);
