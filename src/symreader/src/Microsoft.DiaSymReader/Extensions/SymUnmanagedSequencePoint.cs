// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using System.Diagnostics;

namespace Microsoft.DiaSymReader
{
    [DebuggerDisplay("{GetDebuggerDisplay(),nq}")]
    public struct SymUnmanagedSequencePoint
    {
        public readonly int Offset;
        public readonly ISymUnmanagedDocument Document;
        public readonly int StartLine;
        public readonly int StartColumn;
        public readonly int EndLine;
        public readonly int EndColumn;

        public bool IsHidden => StartLine == 0xfeefee;

        public SymUnmanagedSequencePoint(
            int offset,
            ISymUnmanagedDocument document,
            int startLine,
            int startColumn,
            int endLine,
            int endColumn)
        {
            this.Offset = offset;
            this.Document = document;
            this.StartLine = startLine;
            this.StartColumn = startColumn;
            this.EndLine = endLine;
            this.EndColumn = endColumn;
        }

        private string GetDebuggerDisplay()
        {
            return $"SequencePoint: Offset = {Offset:x4}, Range = ({StartLine}, {StartColumn})..({EndLine}, {EndColumn})";
        }
    }
}
