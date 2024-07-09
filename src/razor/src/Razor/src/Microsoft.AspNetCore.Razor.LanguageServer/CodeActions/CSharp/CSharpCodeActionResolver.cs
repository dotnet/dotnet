// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

internal abstract class CSharpCodeActionResolver(IClientConnection clientConnection) : BaseDelegatedCodeActionResolver(clientConnection)
{
}
