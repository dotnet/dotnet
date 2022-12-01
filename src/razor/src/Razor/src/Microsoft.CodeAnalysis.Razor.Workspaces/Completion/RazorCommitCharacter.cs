// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Razor.Completion;

internal sealed record RazorCommitCharacter(string Character, bool Insert = true)
{
    public static IReadOnlyList<RazorCommitCharacter> FromArray(IReadOnlyList<string> characters) => FromArray(characters, insert: true);

    public static IReadOnlyList<RazorCommitCharacter> FromArray(IReadOnlyList<string> characters, bool insert)
    {
        var converted = new RazorCommitCharacter[characters.Count];

        for (var i = 0; i < characters.Count; i++)
        {
            converted[i] = new RazorCommitCharacter(characters[i], insert);
        }

        return converted;
    }
}
