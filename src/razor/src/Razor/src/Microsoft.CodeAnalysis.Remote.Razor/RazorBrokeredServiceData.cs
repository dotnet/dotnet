// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.VisualStudio.Composition;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal sealed record class RazorBrokeredServiceData(
    ExportProvider? ExportProvider,
    ILoggerFactory? LoggerFactory,
    IRazorBrokeredServiceInterceptor? Interceptor);
