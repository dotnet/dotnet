// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.DiaSymReader.Tools.UnitTests
{
    public unsafe class InteropUtilitiesTests
    {
        [Fact]
        public void CopyQualifiedTypeName()
        {
            InteropUtilities.CopyQualifiedTypeName(null, 0, null, "", "");
            InteropUtilities.CopyQualifiedTypeName(null, 0, null, "Alpha", "Beta");

            var buffer = new char[12];

            void ClearBuffer()
            {
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer[i] = 'x';
                }
            }

            fixed (char* bufferPtr = &buffer[0])
            {
                int length;

                ClearBuffer();

                InteropUtilities.CopyQualifiedTypeName(null, 0, &length, "", "");
                Assert.Equal(0, length);
                length = -1;

                InteropUtilities.CopyQualifiedTypeName(null, 0, &length, null, "");
                Assert.Equal(0, length);
                length = -1;

                InteropUtilities.CopyQualifiedTypeName(null, 0, &length, null, "Z");
                Assert.Equal(1, length);
                length = -1;

                InteropUtilities.CopyQualifiedTypeName(null, 0, &length, "", "B");
                Assert.Equal(1, length);
                length = -1;

                InteropUtilities.CopyQualifiedTypeName(null, 0, &length, "A", "B");
                Assert.Equal(3, length);
                length = -1;

                InteropUtilities.CopyQualifiedTypeName(null, 0, &length, "Alpha", "Beta");
                Assert.Equal(10, length);
                length = -1;

                InteropUtilities.CopyQualifiedTypeName(bufferPtr + 1, buffer.Length - 1, &length, "", "");
                AssertEx.Equal(new char[] { 'x', '\0', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x' }, buffer);
                Assert.Equal(0, length);

                ClearBuffer();

                InteropUtilities.CopyQualifiedTypeName(bufferPtr + 1, buffer.Length - 1, &length, null, "Z");
                AssertEx.Equal(new char[] { 'x', 'Z', '\0', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x' }, buffer);
                Assert.Equal(1, length);

                ClearBuffer();

                InteropUtilities.CopyQualifiedTypeName(bufferPtr + 1, buffer.Length - 1, &length, "", "B");
                AssertEx.Equal(new char[] { 'x', 'B', '\0', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x' }, buffer);
                Assert.Equal(1, length);

                ClearBuffer();

                InteropUtilities.CopyQualifiedTypeName(bufferPtr + 1, buffer.Length - 1, &length, "A", "B");
                AssertEx.Equal(new char[]  { 'x', 'A', '.', 'B', '\0', 'x', 'x', 'x', 'x', 'x', 'x', 'x' }, buffer);
                Assert.Equal(3, length);

                ClearBuffer();

                InteropUtilities.CopyQualifiedTypeName(bufferPtr, 11, &length, "Alpha", "Beta");
                AssertEx.Equal(new char[] { 'A', 'l', 'p', 'h', 'a', '.', 'B', 'e', 't', 'a', '\0', 'x' }, buffer);
                Assert.Equal(10, length);
                ClearBuffer();

                InteropUtilities.CopyQualifiedTypeName(bufferPtr, 10, &length, "Alpha", "Beta");
                AssertEx.Equal(new char[] { 'A', 'l', 'p', 'h', 'a', '.', 'B', 'e', 't', '\0', 'x', 'x' }, buffer);
                Assert.Equal(9, length);
                ClearBuffer();

                InteropUtilities.CopyQualifiedTypeName(bufferPtr, 0, &length, "Alpha", "Beta");
                AssertEx.Equal(new char[] { 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x' }, buffer);
                Assert.Equal(0, length);
                ClearBuffer();

                InteropUtilities.CopyQualifiedTypeName(bufferPtr, 1, &length, "A", "B");
                AssertEx.Equal(new char[] { '\0', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x' }, buffer);
                Assert.Equal(0, length);
                ClearBuffer();

                InteropUtilities.CopyQualifiedTypeName(bufferPtr, 2, &length, "A", "B");
                AssertEx.Equal(new char[] { 'A', '\0', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x' }, buffer);
                Assert.Equal(1, length);
                ClearBuffer();

                InteropUtilities.CopyQualifiedTypeName(bufferPtr, 3, &length, "A", "B");
                AssertEx.Equal(new char[] { 'A', '.', '\0', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x' }, buffer);
                Assert.Equal(2, length);
                ClearBuffer();

                InteropUtilities.CopyQualifiedTypeName(bufferPtr, 4, &length, "A", "B");
                AssertEx.Equal(new char[] { 'A', '.', 'B', '\0', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x' }, buffer);
                Assert.Equal(3, length);
                ClearBuffer();

                InteropUtilities.CopyQualifiedTypeName(bufferPtr, 5, &length, "A", "B");
                AssertEx.Equal(new char[] { 'A', '.', 'B', '\0', 'x', 'x', 'x', 'x', 'x', 'x', 'x', 'x' }, buffer);
                Assert.Equal(3, length);
                ClearBuffer();
            }
        }
    }
}
