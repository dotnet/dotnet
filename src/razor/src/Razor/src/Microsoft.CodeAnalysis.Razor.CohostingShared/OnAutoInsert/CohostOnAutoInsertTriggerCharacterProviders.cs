// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis.Razor.AutoInsert;

namespace Microsoft.VisualStudio.LanguageServices.Razor.LanguageClient.Cohost;

// These are needed to we can get auto-insert trigger character collection
// during registration of CohostOnAutoInsertProvider without using a remote service

[Export(typeof(IOnAutoInsertTriggerCharacterProvider))]
internal sealed class CohostAutoClosingTagOnAutoInsertTriggerCharacterProvider : AutoClosingTagOnAutoInsertProvider;

[Export(typeof(IOnAutoInsertTriggerCharacterProvider))]
internal sealed class CohostCloseTextTagOnAutoInsertTriggerCharacterProvider : CloseTextTagOnAutoInsertProvider;
