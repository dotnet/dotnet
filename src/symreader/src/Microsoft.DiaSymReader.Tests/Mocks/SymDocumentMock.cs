// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using System;
using System.Runtime.InteropServices;

namespace Microsoft.DiaSymReader.UnitTests
{
    internal class SymDocumentMock : ISymUnmanagedDocument
    {
        public virtual int GetUrl(int bufferLength, out int count, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0), Out] char[] url) => throw new NotImplementedException();
        public virtual int GetDocumentType(ref Guid documentType) => throw new NotImplementedException();
        public virtual int GetLanguage(ref Guid language) => throw new NotImplementedException();
        public virtual int GetLanguageVendor(ref Guid vendor) => throw new NotImplementedException();
        public virtual int GetChecksumAlgorithmId(ref Guid algorithm) => throw new NotImplementedException();
        public virtual int GetChecksum(int bufferLength, out int count, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0), Out] byte[] checksum) => throw new NotImplementedException();
        public virtual int FindClosestLine(int line, out int closestLine) => throw new NotImplementedException();
        public virtual int HasEmbeddedSource([MarshalAs(UnmanagedType.Bool)] out bool value) => throw new NotImplementedException();
        public virtual int GetSourceLength(out int length) => throw new NotImplementedException();
        public virtual int GetSourceRange(int startLine, int startColumn, int endLine, int endColumn, int bufferLength, out int count, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4), Out] byte[] source) => throw new NotImplementedException();
    }
}
