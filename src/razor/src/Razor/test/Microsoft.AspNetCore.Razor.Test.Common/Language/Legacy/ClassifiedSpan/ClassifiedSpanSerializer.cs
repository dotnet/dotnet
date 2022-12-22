// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.IO;

namespace Microsoft.AspNetCore.Razor.Language.Legacy;

internal class ClassifiedSpanSerializer
{
    internal static string Serialize(RazorSyntaxTree syntaxTree)
    {
        using (var writer = new StringWriter())
        {
            var visitor = new ClassifiedSpanWriter(writer, syntaxTree);
            visitor.Visit();

            return writer.ToString();
        }
    }
}
