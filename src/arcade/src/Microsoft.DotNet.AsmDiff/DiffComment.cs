// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.AsmDiff
{
    public sealed class DiffComment
    {
        public DiffComment(string docId, string author, string text)
        {
            DocId = docId;
            Author = author;
            Text = text;
        }

        public string DocId { get; private set; }
        public string Author { get; private set; }
        public string Text { get; private set; }

    }
}
