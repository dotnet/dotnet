// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.LanguageServer.ColorPresentation;
using Microsoft.CommonLanguageServerProtocol.Framework;

namespace Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;

[LanguageServerEndpoint(ColorPresentationEndpoint.ColorPresentationMethodName)]
internal interface IColorPresentationEndpoint : IRazorRequestHandler<ColorPresentationParams, ColorPresentation.ColorPresentation[]>
{
}
