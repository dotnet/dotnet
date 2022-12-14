// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Xunit;

namespace System.IO.Tests
{
    public class FileStream_Seek : FileSystemTest
    {
        [Theory]
        [InlineData(0)]
        [InlineData(10)]
        public void SeekAppendModifyThrows(int bufferSize)
        {
            string fileName = GetTestFilePath();
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                fs.Write(TestBuffer, 0, TestBuffer.Length);
            }

            using (FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.Read, bufferSize))
            {
                long length = fs.Length;
                Assert.Throws<IOException>(() => fs.Seek(length - 1, SeekOrigin.Begin));
                Assert.Equal(length, fs.Position);
                Assert.Throws<IOException>(() => fs.Seek(-1, SeekOrigin.Current));
                Assert.Equal(length, fs.Position);
                Assert.Throws<IOException>(() => fs.Seek(-1, SeekOrigin.End));
                Assert.Equal(length, fs.Position);

                Assert.Throws<IOException>(() => fs.Seek(0, SeekOrigin.Begin));
                Assert.Equal(length, fs.Position);
                Assert.Throws<IOException>(() => fs.Seek(-length, SeekOrigin.Current));
                Assert.Equal(length, fs.Position);
                Assert.Throws<IOException>(() => fs.Seek(-length, SeekOrigin.End));
                Assert.Equal(length, fs.Position);

                Assert.Throws<IOException>(() => fs.Position = length - 1);
                Assert.Equal(length, fs.Position);

                fs.Write(TestBuffer);
                Assert.Equal(length, fs.Seek(length, SeekOrigin.Begin));
            }
        }
    }
}
