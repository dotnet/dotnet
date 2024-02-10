// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.CodeAnalysis.Razor.Completion;

internal interface ITagHelperCompletionService
{
    AttributeCompletionResult GetAttributeCompletions(AttributeCompletionContext completionContext);
    ElementCompletionResult GetElementCompletions(ElementCompletionContext completionContext);
}
