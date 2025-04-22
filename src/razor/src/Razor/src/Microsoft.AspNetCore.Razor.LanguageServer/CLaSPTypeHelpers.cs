// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Reflection;
using Microsoft.CommonLanguageServerProtocol.Framework;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

/// <summary>
///  Useful helpers for accessing CLaSP types and performing reflection.
///  These are intended to only be used for testing to avoid type ambiguities
///  that occur because CLaSP is both compiled into the Razor language server
///  and is referenced as metadata through Microsoft.CodeAnalysis.LanguageServer.Protocol.
/// </summary>
internal static class CLaSPTypeHelpers
{
    public static Type IMethodHandlerType = typeof(IMethodHandler);

    public static LanguageServerEndpointAttribute? GetLanguageServerEnpointAttribute(Type type)
        => type.GetCustomAttribute<LanguageServerEndpointAttribute>();
}
