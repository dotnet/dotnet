// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using FluentAssertions;
using Xunit;

namespace NuGet.Build.Tasks.Console.Test
{
    public class ProgramTests
    {
        /// <summary>
        /// Verifies that <see cref="Program.TryDeserializeGlobalProperties(TextWriter, BinaryReader, out Dictionary{string, string})" /> correctly deserializes from a stream.
        /// </summary>
        [Fact]
        public void TryDeserializeGlobalProperties_WhenEncodingUsed_PropertiesAreDeserialized()
        {
            var expectedGlobalProperties = new Dictionary<string, string>
            {
                ["Property1"] = "Value1",
                ["Property2"] = "  Value2  "
            };

            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            StaticGraphRestoreTaskBase.WriteGlobalProperties(writer, expectedGlobalProperties);

            var errors = new StringBuilder();

            using var errorWriter = new StringWriter(errors);

            stream.Position = 0;

            using var reader = new BinaryReader(stream, Encoding.UTF8, leaveOpen: true);

            bool result = Program.TryDeserializeGlobalProperties(errorWriter, reader, out Dictionary<string, string> actualGlobalProperties);

            result.Should().BeTrue();

            actualGlobalProperties.Should().BeEquivalentTo(expectedGlobalProperties);
        }

        /// <summary>
        /// Verifies that <see cref="Program.TryDeserializeGlobalProperties(TextWriter, BinaryReader, out Dictionary{string, string})" /> returns <see langword="false" /> and logs an error if the stream contains an unsupported integer.
        /// </summary>
        /// <param name="count">The number of dictionary items to attempt to deserialize.</param>
        [Theory]
        [InlineData(-100)] // An integer that is negative
        [InlineData(int.MaxValue)] // An integer that is too big
        public void TryDeserializeGlobalProperties_WhenInvalidDictionaryLength_ReturnsFalse(int count)
        {
            byte[] bytes = BitConverter.GetBytes(count);

            using var stream = GetStreamWithBytes(bytes);

            int expectedLength = BitConverter.ToInt32(bytes, bytes.Length - 4);

            VerifyTryDeserializeGlobalPropertiesError(stream, Strings.Error_StaticGraphRestoreArgumentsParsingFailedUnexpectedIntegerValue, expectedLength);
        }

        /// <summary>
        /// Verifies that <see cref="Program.TryDeserializeGlobalProperties(TextWriter, BinaryReader, out Dictionary{string, string})" /> returns <see langword="false" /> and logs an error if reading the stream causes an exception to be thrown.
        /// </summary>
        /// <param name="throwOnReadCount">Indicates for which call to Stream.Read() should throw, the first, second, or third.</param>
        /// <param name="bytes">An array of bytes to use as the stream.</param>
        [Theory]
        [InlineData(1)] // Throw on first Read()
        [InlineData(2)] // Throw on second Read()
        public void TryDeserializeGlobalProperties_WhenStreamReadThrows_ReturnsFalse(int throwOnReadCount)
        {
            var expectedGlobalProperties = new Dictionary<string, string>
            {
                ["Property1"] = "Value1",
                ["Property2"] = "  Value2  "
            };

            Exception exception = new InvalidOperationException();

            using var stream = new StreamThatThrowsAnException(BitConverter.GetBytes(1), throwOnReadCount, exception);

            VerifyTryDeserializeGlobalPropertiesErrorStartsWith(stream, Strings.Error_StaticGraphRestoreArgumentsParsingFailedExceptionReadingStream, exception.Message, string.Empty);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        [InlineData(10)]
        public void TryParseArguments_WhenArgumentCountIncorrect_LogsError(int argumentCount)
        {
            var errors = new StringBuilder();

            using var errorWriter = new StringWriter(errors);

            string[] args = new string[argumentCount];

            bool result = Program.TryParseArguments(args, () => null, errorWriter, out (Dictionary<string, string> Options, FileInfo MSBuildExeFilePath, string EntryProjectFilePath, Dictionary<string, string> MSBuildGlobalProperties) arguments);

            result.Should().BeFalse();

            errors.ToString().Trim().Should().Be(string.Format(CultureInfo.CurrentCulture, Strings.Error_StaticGraphRestoreArgumentParsingFailedInvalidNumberOfArguments, argumentCount));
        }

        private static Stream GetStreamWithBytes(params byte[] bytes)
        {
            MemoryStream stream = new MemoryStream();

            using BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);

            writer.Write(bytes);

            stream.Flush();

            stream.Position = 0;

            return stream;
        }

        private void VerifyTryDeserializeGlobalPropertiesError(Stream stream, string expectedError, params object[] args)
        {
            var errors = new StringBuilder();

            using var errorWriter = new StringWriter(errors);

            using var reader = new BinaryReader(stream, Encoding.UTF8, leaveOpen: true);

            bool result = Program.TryDeserializeGlobalProperties(errorWriter, reader, out Dictionary<string, string> actualGlobalProperties);

            result.Should().BeFalse();

            actualGlobalProperties.Should().BeNull();

            errors.ToString().Trim().Should().Be(string.Format(CultureInfo.CurrentCulture, expectedError, args));
        }

        private void VerifyTryDeserializeGlobalPropertiesErrorStartsWith(Stream stream, string expectedError, params object[] args)
        {
            var errors = new StringBuilder();

            using var errorWriter = new StringWriter(errors);

            using var reader = new BinaryReader(stream, Encoding.UTF8, leaveOpen: true);

            bool result = Program.TryDeserializeGlobalProperties(errorWriter, reader, out Dictionary<string, string> actualGlobalProperties);

            result.Should().BeFalse();

            actualGlobalProperties.Should().BeNull();

            errors.ToString().Trim().Should().StartWith(string.Format(CultureInfo.CurrentCulture, expectedError, args));
        }

        private class StreamThatThrowsAnException : Stream
        {
            private readonly byte[] _buffer;
            private readonly Exception _exceptionToThrow;
            private readonly long _length;
            private readonly int _throwOnReadCount;
            private int _readCount = 0;

            public StreamThatThrowsAnException(byte[] buffer, int throwOnReadCount, Exception exception)
            {
                _throwOnReadCount = throwOnReadCount;
                _buffer = buffer;
                _length = buffer.LongLength;
                _exceptionToThrow = exception;
            }

            public override bool CanRead => true;

            public override bool CanSeek => false;

            public override bool CanWrite => false;

            public override long Length => _length;

            public override long Position { get; set; }

            public override void Flush()
            {
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                if (++_readCount >= _throwOnReadCount)
                {
                    throw _exceptionToThrow;
                }

                int readCount = 0;

                for (int i = 0; i < count; i++)
                {
                    if (Position >= _length)
                    {
                        break;
                    }

                    buffer[i + offset] = _buffer[Position++];

                    readCount++;
                }

                return readCount;
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                throw new NotSupportedException();
            }

            public override void SetLength(long value)
            {
                throw new NotSupportedException();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                throw new NotSupportedException();
            }
        }
    }
}
