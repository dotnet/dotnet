// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using Microsoft.CodeAnalysis.Razor.Completion;

namespace Microsoft.CodeAnalysis.Remote.Razor.Completion;

[Export(typeof(ITagHelperCompletionService)), Shared]
internal sealed class OOPTagHelperCompletionService : TagHelperCompletionService
{
}
