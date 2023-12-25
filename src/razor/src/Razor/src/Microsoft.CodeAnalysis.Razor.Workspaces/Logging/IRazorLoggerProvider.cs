// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;

namespace Microsoft.CodeAnalysis.Razor.Logging;

// Very very light wrapper for ILoggerProvider, so that we're not MEF importing general use types
internal interface IRazorLoggerProvider : ILoggerProvider
{
}
