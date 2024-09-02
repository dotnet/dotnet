// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Composition;
using Microsoft.CodeAnalysis.Razor.AutoInsert;

namespace Microsoft.CodeAnalysis.Remote.Razor.AutoInsert;

[Export(typeof(IAutoInsertService)), Shared]
[method: ImportingConstructor]
internal sealed class OOPAutoInsertService([ImportMany] IEnumerable<IOnAutoInsertProvider> providers) : AutoInsertService(providers)
{
}
