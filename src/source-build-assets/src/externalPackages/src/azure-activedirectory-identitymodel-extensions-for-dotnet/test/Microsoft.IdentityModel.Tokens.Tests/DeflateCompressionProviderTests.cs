// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Text;
using Xunit;

namespace Microsoft.IdentityModel.Tokens.Tests
{
    public class DeflateCompressionProviderTests
    {
        [Fact]
        public void DecompressLargePayload_ShouldSucceed()
        {
            var largeData = CreateLargeTestData(50000);

            var provider = new DeflateCompressionProvider();
            provider.MaximumDeflateSize = 10000000;
            var compressedData = provider.Compress(Encoding.UTF8.GetBytes(largeData));
            var decompressedData = provider.Decompress(compressedData);
            var result = Encoding.UTF8.GetString(decompressedData);

            Assert.Equal(largeData, result);
        }

        [Fact]
        public void DecompressLargePayload_ExceedsMaximumSize_ShouldThrow()
        {
            var data = CreateLargeTestData(1000);

            var provider = new DeflateCompressionProvider();
            provider.MaximumDeflateSize = 100; // Very small limit

            var compressedData = provider.Compress(Encoding.UTF8.GetBytes(data));

            var exception = Assert.Throws<SecurityTokenDecompressionFailedException>(() => provider.Decompress(compressedData));
            Assert.Contains("IDX10816", exception.Message);
        }

        private string CreateLargeTestData(int itemCount)
        {
            var sb = new StringBuilder();
            sb.Append('[');
            for (int i = 0; i < itemCount; i++)
            {
                if (i > 0) sb.Append(',');
                sb.Append($"{{\"Prop1\":\"TestValue{i}\",\"Prop2\":{i},\"Prop3\":\"SomeAdditionalDataToMakeItLarger{i}\"}}");
            }
            sb.Append(']');
            return sb.ToString();
        }
    }
}
