// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.Logging;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.Test.Common.Logging;

internal sealed class TestOutputLoggerFactory(ITestOutputHelper output)
    : AbstractRazorLoggerFactory([new TestOutputLoggerProvider(output)])
{
}
