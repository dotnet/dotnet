// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CommonLanguageServerProtocol.Framework;

namespace Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;

internal interface IRazorNotificationHandler<TRequestType> : INotificationHandler<TRequestType, RazorRequestContext>
{
}

internal interface IRazorNotificationHandler : INotificationHandler<RazorRequestContext>
{
}
