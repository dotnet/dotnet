#if XUNIT_NULLABLE
#nullable enable
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Xunit.Sdk
{
	/// <summary>
	/// Exception thrown when two values are unexpectedly not equal.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	class EqualException : AssertActualExpectedException
	{
		static readonly Dictionary<char, string> Encodings = new Dictionary<char, string>
		{
			{ '\r', "\\r" },
			{ '\n', "\\n" },
			{ '\t', "\\t" },
			{ '\0', "\\0" }
		};

#if XUNIT_NULLABLE
		string? message;
#else
		string message;
#endif

		/// <summary>
		/// Creates a new instance of the <see cref="EqualException"/> class.
		/// </summary>
		/// <param name="expected">The expected object value</param>
		/// <param name="actual">The actual object value</param>
#if XUNIT_NULLABLE
		public EqualException(object? expected, object? actual)
#else
		public EqualException(object expected, object actual)
#endif
			: base(expected, actual, "Assert.Equal() Failure")
		{
			ActualIndex = -1;
			ExpectedIndex = -1;
		}

		/// <summary>
		/// Creates a new instance of the <see cref="EqualException"/> class for string comparisons.
		/// </summary>
		/// <param name="expected">The expected string value</param>
		/// <param name="actual">The actual string value</param>
		/// <param name="expectedIndex">The first index in the expected string where the strings differ</param>
		/// <param name="actualIndex">The first index in the actual string where the strings differ</param>
#if XUNIT_NULLABLE
		public EqualException(string? expected, string? actual, int expectedIndex, int actualIndex)
#else
		public EqualException(string expected, string actual, int expectedIndex, int actualIndex)
#endif
			: this(expected, actual, expectedIndex, actualIndex, null)
		{ }

#if XUNIT_NULLABLE
		EqualException(string? expected, string? actual, int expectedIndex, int actualIndex, int? pointerPosition)
#else
		EqualException(string expected, string actual, int expectedIndex, int actualIndex, int? pointerPosition)
#endif
			: base(expected, actual, "Assert.Equal() Failure")
		{
			ActualIndex = actualIndex;
			ExpectedIndex = expectedIndex;
			PointerPosition = pointerPosition;
		}

		/// <summary>
		/// Gets the index into the actual value where the values first differed.
		/// Returns -1 if the difference index points were not provided.
		/// </summary>
		public int ActualIndex { get; }

		/// <summary>
		/// Gets the index into the expected value where the values first differed.
		/// Returns -1 if the difference index points were not provided.
		/// </summary>
		public int ExpectedIndex { get; }

		/// <inheritdoc/>
		public override string Message
		{
			get
			{
				if (message == null)
					message = CreateMessage();

				return message;
			}
		}

		/// <summary>
		/// Gets the index of the difference between the IEunmerables when converted to a string.
		/// </summary>
		public int? PointerPosition { get; private set; }

		string CreateMessage()
		{
			if (ExpectedIndex == -1)
				return base.Message;

			var printedExpected = ShortenAndEncode(Expected, PointerPosition ?? ExpectedIndex, '↓', ExpectedIndex);
			var printedActual = ShortenAndEncode(Actual, PointerPosition ?? ActualIndex, '↑', ActualIndex);

			var sb = new StringBuilder();
			sb.Append(UserMessage);

			if (!string.IsNullOrWhiteSpace(printedExpected.Item2))
				sb.AppendFormat(
					CultureInfo.CurrentCulture,
					"{0}          {1}",
					Environment.NewLine,
					printedExpected.Item2
				);

			sb.AppendFormat(
				CultureInfo.CurrentCulture,
				"{0}Expected: {1}{0}Actual:   {2}",
				Environment.NewLine,
				printedExpected.Item1,
				printedActual.Item1
			);

			if (!string.IsNullOrWhiteSpace(printedActual.Item2))
				sb.AppendFormat(
					CultureInfo.CurrentCulture,
					"{0}          {1}",
					Environment.NewLine,
					printedActual.Item2
				);

			return sb.ToString();
		}

		/// <summary>
		/// Creates a new instance of the <see cref="EqualException"/> class for IEnumerable comparisons.
		/// </summary>
		/// <param name="expected">The expected object value</param>
		/// <param name="actual">The actual object value</param>
		/// <param name="mismatchIndex">The first index in the expected IEnumerable where the strings differ</param>
#if XUNIT_NULLABLE
		public static EqualException FromEnumerable(IEnumerable? expected, IEnumerable? actual, int mismatchIndex)
#else
		public static EqualException FromEnumerable(IEnumerable expected, IEnumerable actual, int mismatchIndex)
#endif
		{
			int? pointerPositionExpected;
			int? pointerPositionActual;

			var expectedText = ArgumentFormatter.Format(expected, out pointerPositionExpected, mismatchIndex);
			var actualText = ArgumentFormatter.Format(actual, out pointerPositionActual, mismatchIndex);
			var pointerPosition = (pointerPositionExpected ?? -1) > (pointerPositionActual ?? -1) ? pointerPositionExpected : pointerPositionActual;

			return new EqualException(expectedText, actualText, mismatchIndex, mismatchIndex, pointerPosition);
		}


#if XUNIT_NULLABLE
		static Tuple<string, string> ShortenAndEncode(string? value, int position, char pointer, int? index = null)
#else
		static Tuple<string, string> ShortenAndEncode(string value, int position, char pointer, int? index = null)
#endif
		{
			if (value == null)
				return Tuple.Create("(null)", "");

			index = index ?? position;

			var start = Math.Max(position - 20, 0);
			var end = Math.Min(position + 41, value.Length);
			var printedValue = new StringBuilder(100);
			var printedPointer = new StringBuilder(100);

			if (start > 0)
			{
				printedValue.Append("···");
				printedPointer.Append("   ");
			}

			for (var idx = start; idx < end; ++idx)
			{
				var c = value[idx];
				var paddingLength = 1;

#if XUNIT_NULLABLE
				string? encoding;
#else
				string encoding;
#endif

				if (Encodings.TryGetValue(c, out encoding))
				{
					printedValue.Append(encoding);
					paddingLength = encoding.Length;
				}
				else
					printedValue.Append(c);

				if (idx < position)
					printedPointer.Append(' ', paddingLength);
				else if (idx == position)
					printedPointer.AppendFormat("{0} (pos {1})", pointer, index);
			}

			if (value.Length == position)
				printedPointer.AppendFormat("{0} (pos {1})", pointer, index);

			if (end < value.Length)
				printedValue.Append("···");

			return Tuple.Create(printedValue.ToString(), printedPointer.ToString());
		}
	}
}
