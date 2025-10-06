// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Microsoft.CodeAnalysis.Razor.Completion;

internal class MarkupTransitionCompletionDescription : CompletionDescription
{
    public override string Description { get; }

    public MarkupTransitionCompletionDescription(string description)
    {
        if (description is null)
        {
            throw new ArgumentNullException(nameof(description));
        }

        Description = description;
    }
}
