using Mono.Cecil.PE;
using NUnit.Framework;

namespace Mono.Cecil.Tests {
	public class ByteBufferTests {
		[Test]
		public void TestLargeIntegerCompressed ()
		{
			var testee = new ByteBuffer ();
			testee.WriteCompressedInt32 (-9076);
			testee.position = 0;
			var result = testee.ReadCompressedInt32 ();
			Assert.AreEqual (-9076, result);
		}

		// Round-trip tests pass on big-endian even with swapped bytes.
		// Assert the ECMA-335 II.23.2 encoding (always big-endian in the blob).
		[Test]
		public void CompressedInt32BytesAreBigEndian ()
		{
			AssertCompressedInt32Bytes (-100, 0xBF, 0x39);
			AssertCompressedInt32Bytes (-9076, 0xDF, 0xFF, 0xB9, 0x19);
		}

		static void AssertCompressedInt32Bytes (int value, params byte [] expected)
		{
			var testee = new ByteBuffer ();
			testee.WriteCompressedInt32 (value);
			Assert.AreEqual (expected.Length, testee.length);
			for (int i = 0; i < expected.Length; i++)
				Assert.AreEqual (expected [i], testee.buffer [i]);
		}

		// Round-trips WriteCompressedInt32/ReadCompressedInt32 across every encoding-width
		// boundary of the ECMA-335 compressed signed integer format (1/2/4-byte forms)
		[TestCase (0)]
		[TestCase (1)]
		[TestCase (-1)]
		[TestCase (63)]
		[TestCase (-64)]
		[TestCase (64)]
		[TestCase (-65)]
		[TestCase (8191)]
		[TestCase (-8192)]
		[TestCase (8192)]
		[TestCase (-8193)]
		[TestCase (-8269)]
		[TestCase (268435455)]
		[TestCase (-268435456)]
		public void CompressedInt32RoundTripsAcrossEncodingWidths (int value)
		{
			var testee = new ByteBuffer ();
			testee.WriteCompressedInt32 (value);
			testee.position = 0;
			var result = testee.ReadCompressedInt32 ();
			Assert.AreEqual (value, result);
		}
	}
}
