// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Reflection;
using System.Text;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

namespace Microsoft.IdentityModel.Tokens.UrlEncoding.Tests
{
    public class Base64UrlEncoderTests
    {
        [Theory, MemberData(nameof(EncodeTestCases), DisableDiscoveryEnumeration = true)]
        public void EncodeTests(Base64UrlEncoderTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader("EncodeTests", theoryData);
            string encoderString = null;
            string encoderBytes = null;
            string encoderBytesUsingOffset = null;
            string encoderBytesUsingSpan = null;

            try
            {
                // to get to error code in Base64UrlEncoding, we need to skip Encoder
                if (!theoryData.EncodingOnly)
                {
                    encoderString = Base64UrlEncoder.Encode(theoryData.Json);
                    encoderBytes = Base64UrlEncoder.Encode(theoryData.Bytes);
                    encoderBytesUsingOffset = Base64UrlEncoder.Encode(theoryData.OffsetBytes, theoryData.Offset, theoryData.Length);
                    int encodedCharsCount = Base64UrlEncoder.Encode(theoryData.OffsetBytes.AsSpan<byte>().Slice(theoryData.Offset, theoryData.Length), theoryData.Chars.AsSpan<char>());
                    encoderBytesUsingSpan = new string(theoryData.Chars, 0, encodedCharsCount);
                }

                string encodingString = Base64UrlEncoding.Encode(theoryData.Bytes);
                string encodingBytesUsingOffset = Base64UrlEncoding.Encode(theoryData.OffsetBytes, theoryData.Offset, theoryData.Length);
                byte[] decodedBytes = theoryData.Bytes?.Length == 0 ? Array.Empty<byte>() : Base64UrlEncoding.Decode(encodingString);
                const string extraPadding = "EXTRAPADDING";
                byte[] decodedBytes2 = theoryData.Bytes?.Length == 0 ? Array.Empty<byte>() : Base64UrlEncoding.Decode(extraPadding + encodingString + extraPadding, extraPadding.Length, encodingString.Length);

                theoryData.ExpectedException.ProcessNoException(context);

                if (!theoryData.EncodingOnly)
                {
                    IdentityComparer.AreStringsEqual(encoderString, encoderBytes, "encoderString", "encoderBytes", context);
                    IdentityComparer.AreStringsEqual(encoderBytesUsingOffset, encoderBytes, "encoderBytesUsingOffset", "encoderBytes", context);
                    IdentityComparer.AreStringsEqual(encoderBytesUsingSpan, encoderBytes, "encoderBytesUsingSpan", "encoderBytes", context);
                    IdentityComparer.AreStringsEqual(encodingString, encoderBytes, "encodingString", "encoderBytes", context);
                }

                IdentityComparer.AreStringsEqual(encodingBytesUsingOffset, encodingString, "encodingBytesUsingOffset", "encodingString", context);
                IdentityComparer.AreStringsEqual(theoryData.ExpectedValue, encodingString, "theoryData.ExpectedValue", "encodingString", context);
                IdentityComparer.AreEqual(theoryData.Bytes, decodedBytes, context);
                IdentityComparer.AreEqual(theoryData.Bytes, decodedBytes2, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<Base64UrlEncoderTheoryData> EncodeTestCases
        {
            get
            {
                TheoryData<Base64UrlEncoderTheoryData> theoryData = new TheoryData<Base64UrlEncoderTheoryData>();

                // These values are sourced from https://datatracker.ietf.org/doc/html/rfc7519#section-6.1
                string json = $@"{{""alg"":""none""}}";
                string expectedValue = "eyJhbGciOiJub25lIn0";
                byte[] utf8Bytes = Encoding.UTF8.GetBytes(json);

                theoryData.Add(new Base64UrlEncoderTheoryData("Header_Offset_0")
                {
                    Bytes = utf8Bytes,
                    Chars = new char[1024],
                    ExpectedValue = expectedValue,
                    Json = json,
                    Length = utf8Bytes.Length,
                    Offset = 0,
                    OffsetBytes = utf8Bytes,
                    OffsetLength = utf8Bytes.Length
                });

                // NOTE the spec performs the encoding over the \r\n and space ' '.
                json = "{\"iss\":\"joe\",\r\n \"exp\":1300819380,\r\n \"http://example.com/is_root\":true}";
                expectedValue = "eyJpc3MiOiJqb2UiLA0KICJleHAiOjEzMDA4MTkzODAsDQogImh0dHA6Ly9leGFtcGxlLmNvbS9pc19yb290Ijp0cnVlfQ";
                utf8Bytes = Encoding.UTF8.GetBytes(json);

                theoryData.Add(new Base64UrlEncoderTheoryData("Payload_Offset_0")
                {
                    Bytes = utf8Bytes,
                    Chars = new char[1024],
                    ExpectedValue = expectedValue,
                    Json = json,
                    Length = utf8Bytes.Length,
                    Offset = 0,
                    OffsetBytes = utf8Bytes,
                    OffsetLength = utf8Bytes.Length
                });

                byte[] utf8BytesOffset = new byte[utf8Bytes.Length * 2];
                int count = Encoding.UTF8.GetBytes(json, 0, json.Length, utf8BytesOffset, 5);
                theoryData.Add(new Base64UrlEncoderTheoryData("Payload_Offset_5")
                {
                    Bytes = utf8Bytes,
                    Chars = new char[1024],
                    ExpectedValue = expectedValue,
                    Json = json,
                    Length = count,
                    Offset = 5,
                    OffsetBytes = utf8BytesOffset,
                    OffsetLength = count
                });

                theoryData.Add(new Base64UrlEncoderTheoryData("JsonNULL")
                {
                    Bytes = utf8Bytes,
                    Chars = new char[1024],
                    ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                    ExpectedValue = expectedValue,
                    Json = null,
                    Length = count,
                    Offset = 5,
                    OffsetBytes = utf8BytesOffset,
                    OffsetLength = count
                });

                theoryData.Add(new Base64UrlEncoderTheoryData("BytesNULL")
                {
                    Bytes = null,
                    Chars = new char[1024],
                    ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                    ExpectedValue = expectedValue,
                    Json = json,
                    Length = count,
                    Offset = 5,
                    OffsetBytes = utf8BytesOffset,
                    OffsetLength = count
                });

                theoryData.Add(new Base64UrlEncoderTheoryData("OffsetBytesNULL")
                {
                    Bytes = utf8Bytes,
                    Chars = new char[1024],
                    ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                    ExpectedValue = expectedValue,
                    Json = json,
                    Length = count,
                    Offset = 5,
                    OffsetBytes = null,
                    OffsetLength = count
                });

                theoryData.Add(new Base64UrlEncoderTheoryData("Length_Negative")
                {
                    Bytes = utf8Bytes,
                    Chars = new char[1024],
                    ExpectedException = ExpectedException.ArgumentOutOfRangeException("IDX10716:"),
                    ExpectedValue = expectedValue,
                    Json = json,
                    Length = -1,
                    Offset = 5,
                    OffsetBytes = utf8BytesOffset,
                    OffsetLength = 5
                });

                theoryData.Add(new Base64UrlEncoderTheoryData("Length_Zero")
                {
                    Bytes = new byte[0],
                    Chars = new char[1024],
                    ExpectedValue = string.Empty,
                    Json = string.Empty,
                    Length = 0,
                    Offset = 0,
                    OffsetBytes = new byte[0],
                    OffsetLength = 0
                });

                theoryData.Add(new Base64UrlEncoderTheoryData("Bytes_Zero")
                {
                    Bytes = new byte[0],
                    Chars = new char[1024],
                    ExpectedValue = string.Empty,
                    Json = string.Empty,
                    Length = 0,
                    Offset = 0,
                    OffsetBytes = new byte[0],
                    OffsetLength = 0
                });

                theoryData.Add(new Base64UrlEncoderTheoryData("Input_LessThan_Offset_Length")
                {
                    Bytes = utf8Bytes,
                    Chars = new char[1024],
                    ExpectedException = ExpectedException.ArgumentOutOfRangeException("IDX10717:"),
                    ExpectedValue = expectedValue,
                    Json = json,
                    Length = count,
                    Offset = utf8BytesOffset.Length,
                    OffsetBytes = utf8BytesOffset,
                    OffsetLength = utf8BytesOffset.Length
                });

                theoryData.Add(new Base64UrlEncoderTheoryData("BytesNULL_Encoding")
                {
                    Bytes = null,
                    Chars = new char[1024],
                    ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                    ExpectedValue = expectedValue,
                    Length = count,
                    Json = json,
                    Offset = 5,
                    OffsetBytes = utf8BytesOffset,
                    OffsetLength = count,
                    EncodingOnly = true
                });

                theoryData.Add(new Base64UrlEncoderTheoryData("OffsetBytesNULL_Encoding")
                {
                    Bytes = utf8Bytes,
                    Chars = new char[1024],
                    ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                    ExpectedValue = expectedValue,
                    Json = json,
                    Length = count,
                    Offset = 5,
                    OffsetBytes = null,
                    OffsetLength = count,
                    EncodingOnly = true
                });

                theoryData.Add(new Base64UrlEncoderTheoryData("Offset_Negative_Encoding")
                {
                    Bytes = utf8Bytes,
                    Chars = new char[1024],
                    ExpectedException = ExpectedException.ArgumentOutOfRangeException("IDX10716:"),
                    ExpectedValue = expectedValue,
                    Json = json,
                    Length = count,
                    Offset = -1,
                    OffsetBytes = utf8BytesOffset,
                    OffsetLength = count,
                    EncodingOnly = true
                });

                theoryData.Add(new Base64UrlEncoderTheoryData("Length_Negative_Encoding")
                {
                    Bytes = utf8Bytes,
                    Chars = new char[1024],
                    ExpectedException = ExpectedException.ArgumentOutOfRangeException("IDX10716:"),
                    ExpectedValue = expectedValue,
                    Json = json,
                    Length = -1,
                    Offset = 5,
                    OffsetBytes = utf8BytesOffset,
                    OffsetLength = -1,
                    EncodingOnly = true
                });

                theoryData.Add(new Base64UrlEncoderTheoryData("Length_Zero_Encoding")
                {
                    Bytes = new byte[0],
                    Chars = new char[1024],
                    ExpectedValue = string.Empty,
                    Json = string.Empty,
                    Length = 0,
                    Offset = 0,
                    OffsetBytes = new byte[0],
                    OffsetLength = 0
                });

                theoryData.Add(new Base64UrlEncoderTheoryData("Bytes_Zero_Encoding")
                {
                    Bytes = new byte[0],
                    Chars = new char[1024],
                    ExpectedValue = string.Empty,
                    Json = string.Empty,
                    Length = 0,
                    Offset = 0,
                    OffsetBytes = new byte[0],
                    OffsetLength = 0,
                    EncodingOnly = true
                });


                return theoryData;
            }
        }

        public class Base64UrlEncoderTheoryData : TheoryDataBase
        {
            public Base64UrlEncoderTheoryData(string testId) : base(testId) { }

            public byte[] Bytes { get; set; }

            public char[] Chars { get; set; }

            public string ExpectedValue { get; set; }

            public string Json { get; set; }

            public int Length { get; set; }

            public int Offset { get; set; }

            public byte[] OffsetBytes { get; set; }

            public int OffsetLength { get; set; }

            public bool EncodingOnly { get; set; }
        }

        [Fact]
        public void ValidateAndGetOutputSizeTests_InvalidInput_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => Base64UrlEncoding.ValidateAndGetOutputSize(string.Empty.AsSpan(), 0, 0));
            Assert.Throws<ArgumentException>(() => Base64UrlEncoding.ValidateAndGetOutputSize("abc".AsSpan(), -1, 3));
            Assert.Throws<ArgumentException>(() => Base64UrlEncoding.ValidateAndGetOutputSize("abc".AsSpan(), 0, -1));
            Assert.Throws<ArgumentException>(() => Base64UrlEncoding.ValidateAndGetOutputSize("abc".AsSpan(), 0, 4));

            IdentityModelEventSource.ShowPII = true;
            var ex = Assert.Throws<FormatException>(() => Base64UrlEncoding.ValidateAndGetOutputSize("abcde".AsSpan(), 0, 5));
            Assert.Equal(string.Format(LogMessages.IDX10400, "abcde"), ex.Message);

            IdentityModelEventSource.ShowPII = false;
            ex = Assert.Throws<FormatException>(() => Base64UrlEncoding.ValidateAndGetOutputSize("abcde".AsSpan(), 0, 5));
            Assert.Equal(string.Format(LogMessages.IDX10400, string.Format(IdentityModelEventSource.HiddenPIIString, "abcde".GetType())), ex.Message);
        }

        [Fact]
        public void ValidateAndGetOutputSizeTests_ValidInput_ReturnsSize()
        {
            int actualOutputSize = Base64UrlEncoding.ValidateAndGetOutputSize("abc".AsSpan(), 0, 0);
            Assert.Equal(0, actualOutputSize);

            actualOutputSize = Base64UrlEncoding.ValidateAndGetOutputSize("abcd".AsSpan(), 0, 4);
            Assert.Equal(3, actualOutputSize);

            actualOutputSize = Base64UrlEncoding.ValidateAndGetOutputSize("abc=".AsSpan(), 0, 4);
            Assert.Equal(2, actualOutputSize);
        }

        [Fact]
        public void EncodeDecode_InvalidParameters_ThrowsExceptionTests()
        {
            Assert.Throws<ArgumentNullException>(static () => Base64UrlEncoding.Decode(null));
            Assert.Throws<ArgumentNullException>(static () => Base64UrlEncoding.Decode(null, 0, 0));
            Assert.Throws<ArgumentNullException>(static () => Base64UrlEncoding.Encode(null));
            Assert.Throws<ArgumentNullException>(static () => Base64UrlEncoding.Encode(null, 0, 0));
            Assert.Throws<ArgumentNullException>(static () => Base64UrlEncoding.Decode<object>("abc", 0, 0, null));
            Assert.Throws<ArgumentNullException>(static () => Base64UrlEncoding.Decode<object, object>("abc", 0, 0, null, null));
            Assert.Throws<ArgumentNullException>(static () => Base64UrlEncoding.Decode<object, object, object, object>(null, 0, 0, null, null, null, null));
        }

        [Fact]
        public void Base64UrlEncoder_PublicApiIsNotChanged()
        {
            // Public APIs
            string encoded1 = Base64UrlEncoder.Encode("test");
            string encoded2 = Base64UrlEncoder.Encode(new byte[] { 1, 2, 3 });
            string encoded3 = Base64UrlEncoder.Encode(new byte[] { 1, 2, 3 }, 0, 3);
            char[] charBuffer = new char[10];
            int charsWritten = Base64UrlEncoder.Encode(new ReadOnlySpan<byte>(new byte[] { 1, 2, 3 }), new Span<char>(charBuffer));
            byte[] decodedBytes = Base64UrlEncoder.DecodeBytes("dGVzdA");
            string decodedString = Base64UrlEncoder.Decode("dGVzdA");

            // Internal APIs
            byte[] decodedBytesFromSpan = Base64UrlEncoder.Decode("test".AsSpan());
            Span<byte> output = stackalloc byte[10];
#if NET6_0_OR_GREATER
            int bytesWritten = Base64UrlEncoder.Decode("test".AsSpan(), output);
#else
            Base64UrlEncoder.Decode("test".AsSpan(), output);
            var method = typeof(Base64UrlEncoder).GetMethod(
                "Decode",
                BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public,
                null,
                new[] { typeof(ReadOnlySpan<char>), typeof(Span<byte>), },
                null
            );
            Assert.Equal(typeof(void), method.ReturnType);
#endif
        }

        [Fact]
        public void Base64UrlEncoding_PublicApiIsNotChanged()
        {
            // Public APIs
            byte[] decoded1 = Base64UrlEncoding.Decode("test");
            byte[] decoded2 = Base64UrlEncoding.Decode("test", 0, 4);
            string genericDecoded1 = Base64UrlEncoding.Decode<string>("test", 0, 4, (bytes, len) => "test");
            string genericDecoded2 = Base64UrlEncoding.Decode<string, int>("test", 0, 4, 1, (bytes, len, x) => "test");
            string genericDecoded3 = Base64UrlEncoding.Decode<string, int, int, int>("test", 0, 4, 1, 2, 3, (bytes, len, x, y, z) => "test");
            string encoded1 = Base64UrlEncoding.Encode(new byte[] { 1, 2, 3 });
            string encoded2 = Base64UrlEncoding.Encode(new byte[] { 1, 2, 3 }, 0, 3);

            // Internal APIs
            int outputSize = Base64UrlEncoding.ValidateAndGetOutputSize("test".AsSpan(), 0, 4);
            byte[] output = new byte[10];
            Base64UrlEncoding.Decode("test".AsSpan(), 0, 4, output);
            var method = typeof(Base64UrlEncoding).GetMethod(
                "Decode",
                BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public,
                null,
                new[] { typeof(ReadOnlySpan<char>), typeof(int), typeof(int), typeof(byte[]) },
                null
            );
            Assert.Equal(typeof(void), method.ReturnType);
        }
    }
}
