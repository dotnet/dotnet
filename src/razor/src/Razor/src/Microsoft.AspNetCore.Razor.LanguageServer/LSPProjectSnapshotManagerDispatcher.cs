// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class LSPProjectSnapshotManagerDispatcher(IErrorReporter errorReporter)
    : ProjectSnapshotManagerDispatcher(errorReporter)
{
}
