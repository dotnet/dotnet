// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Composition;
using Microsoft.AspNetCore.Razor.ProjectEngineHost;

namespace Microsoft.CodeAnalysis.Razor;

[Export(typeof(IFallbackProjectEngineFactory))]
internal class FallbackProjectEngineFactory : EmptyProjectEngineFactory, IFallbackProjectEngineFactory
{
}
