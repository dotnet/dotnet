#pragma warning disable IDE0057 // Use range operator
#pragma warning disable IDE0090 // Use 'new(...)'
#pragma warning disable IDE0305 // Simplify collection initialization

#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8603
#pragma warning disable CS8625
#pragma warning disable CS8767
#endif

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Xunit.Sdk;

#if XUNIT_NULLABLE
using System.Diagnostics.CodeAnalysis;
#endif

#if NET8_0_OR_GREATER
using System.Threading.Tasks;
#endif

namespace Xunit.Internal
{
	/// <summary>
	/// INTERNAL CLASS. DO NOT USE.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	static partial class AssertHelper
	{
		static readonly Dictionary<char, string> encodings = new Dictionary<char, string>
		{
			{ '\0', @"\0" },  // Null
			{ '\a', @"\a" },  // Alert
			{ '\b', @"\b" },  // Backspace
			{ '\f', @"\f" },  // Form feed
			{ '\n', @"\n" },  // New line
			{ '\r', @"\r" },  // Carriage return
			{ '\t', @"\t" },  // Horizontal tab
			{ '\v', @"\v" },  // Vertical tab
			{ '\\', @"\\" },  // Backslash
		};

		internal static (int start, int end) GetStartEndForString(
#if XUNIT_NULLABLE
			string? value,
#else
			string value,
#endif
			int index)
		{
			if (value is null)
				return (0, 0);

			if (ArgumentFormatter.MaxStringLength == int.MaxValue)
				return (0, value.Length);

			var halfMaxLength = ArgumentFormatter.MaxStringLength / 2;
			var start = Math.Max(index - halfMaxLength, 0);
			var end = Math.Min(start + ArgumentFormatter.MaxStringLength, value.Length);
			start = Math.Max(end - ArgumentFormatter.MaxStringLength, 0);

			return (start, end);
		}

		internal static bool IsCompilerGenerated(Type type) =>
			type.CustomAttributes.Any(a => a.AttributeType.FullName == "System.Runtime.CompilerServices.CompilerGeneratedAttribute");

		/// <summary/>
		public static IReadOnlyList<(string Prefix, string Member)> ParseExclusionExpressions(params string[] exclusionExpressions)
		{
			var result = new List<(string Prefix, string Member)>();

			foreach (var expression in exclusionExpressions ?? throw new ArgumentNullException(nameof(exclusionExpressions)))
			{
				if (expression is null || expression.Length is 0)
					throw new ArgumentException("Null/empty expressions are not valid.", nameof(exclusionExpressions));

				var lastDotIdx = expression.LastIndexOf('.');
				if (lastDotIdx == 0)
					throw new ArgumentException(
						string.Format(
							CultureInfo.CurrentCulture,
							"Expression '{0}' is not valid. Expressions may not start with a period.",
							expression
						),
						nameof(exclusionExpressions)
					);

				if (lastDotIdx == expression.Length - 1)
					throw new ArgumentException(
						string.Format(
							CultureInfo.CurrentCulture,
							"Expression '{0}' is not valid. Expressions may not end with a period.",
							expression
						),
						nameof(exclusionExpressions)
					);

				if (lastDotIdx < 0)
					result.Add((string.Empty, expression));
				else
					result.Add((expression.Substring(0, lastDotIdx), expression.Substring(lastDotIdx + 1)));
			}

			return result;
		}

		/// <summary/>
		public static IReadOnlyList<(string Prefix, string Member)> ParseExclusionExpressions(params LambdaExpression[] exclusionExpressions)
		{
			var result = new List<(string Prefix, string Member)>();

			foreach (var expression in exclusionExpressions ?? throw new ArgumentNullException(nameof(exclusionExpressions)))
			{
				if (expression is null)
					throw new ArgumentException("Null expression is not valid.", nameof(exclusionExpressions));

				var memberExpression = default(MemberExpression);

				// The incoming expressions are T => object?, so any boxed struct starts with a conversion
				if (expression.Body.NodeType == ExpressionType.Convert && expression.Body is UnaryExpression unaryExpression)
					memberExpression = unaryExpression.Operand as MemberExpression;
				else
					memberExpression = expression.Body as MemberExpression;

				if (memberExpression is null)
					throw new ArgumentException(
						string.Format(
							CultureInfo.CurrentCulture,
							"Expression '{0}' is not supported. Only property or field expressions from the lambda parameter are supported.",
							expression
						),
						nameof(exclusionExpressions)
					);

				var pieces = new LinkedList<string>();

				while (true)
				{
					pieces.AddFirst(memberExpression.Member.Name);

					if (memberExpression.Expression?.NodeType == ExpressionType.Parameter)
						break;

					memberExpression = memberExpression.Expression as MemberExpression;

					if (memberExpression is null)
						throw new ArgumentException(
							string.Format(
								CultureInfo.CurrentCulture,
								"Expression '{0}' is not supported. Only property or field expressions from the lambda parameter are supported.",
								expression
							),
							nameof(exclusionExpressions)
						);
				}

				if (pieces.Last is null)
					continue;

				var member = pieces.Last.Value;
				pieces.RemoveLast();

				var prefix = string.Join(".", pieces.ToArray());
				result.Add((prefix, member));
			}

			return result;
		}

		internal static string ShortenAndEncodeString(
#if XUNIT_NULLABLE
			string? value,
#else
			string value,
#endif
			int index,
			out int pointerIndent)
		{
			var (start, end) = GetStartEndForString(value, index);

			return ShortenString(value, start, end, index, out pointerIndent);
		}

#if XUNIT_NULLABLE
		internal static string ShortenAndEncodeString(string? value) =>
#else
		internal static string ShortenAndEncodeString(string value) =>
#endif
			ShortenAndEncodeString(value, 0, out var _);

#if XUNIT_NULLABLE
		internal static string ShortenAndEncodeStringEnd(string? value) =>
#else
		internal static string ShortenAndEncodeStringEnd(string value) =>
#endif
			ShortenAndEncodeString(value, (value?.Length - 1) ?? 0, out var _);

		internal static string ShortenString(
#if XUNIT_NULLABLE
			string? value,
#else
			string value,
#endif
			int start,
			int end,
			int index,
			out int pointerIndent)
		{
			if (value == null)
			{
				pointerIndent = -1;
				return "null";
			}

			// Set the initial buffer to include the possibility of quotes and ellipses, plus a few extra
			// characters for encoding before needing reallocation.
			var printedValue = new StringBuilder(end - start + 10);
			pointerIndent = 0;

			if (start > 0)
			{
				printedValue.Append(ArgumentFormatter.Ellipsis);
				pointerIndent += 3;
			}

			printedValue.Append('\"');
			pointerIndent++;

			for (var idx = start; idx < end; ++idx)
			{
				var c = value[idx];
				var paddingLength = 1;

				if (encodings.TryGetValue(c, out var encoding))
				{
					printedValue.Append(encoding);
					paddingLength = encoding.Length;
				}
				else
					printedValue.Append(c);

				if (idx < index)
					pointerIndent += paddingLength;
			}

			printedValue.Append('\"');

			if (end < value.Length)
				printedValue.Append(ArgumentFormatter.Ellipsis);

			return printedValue.ToString();
		}

#if NET8_0_OR_GREATER

#if XUNIT_NULLABLE
		[return: NotNullIfNotNull(nameof(data))]
		internal static IEnumerable<T>? ToEnumerable<T>(IAsyncEnumerable<T>? data) =>
#else
		internal static IEnumerable<T> ToEnumerable<T>(IAsyncEnumerable<T> data) =>
#endif
			data == null ? null : ToEnumerableImpl(data);

		static IEnumerable<T> ToEnumerableImpl<T>(IAsyncEnumerable<T> data)
		{
			var enumerator = data.GetAsyncEnumerator();

			try
			{
				while (WaitForValueTask(enumerator.MoveNextAsync()))
					yield return enumerator.Current;
			}
			finally
			{
				WaitForValueTask(enumerator.DisposeAsync());
			}
		}

		static void WaitForValueTask(ValueTask valueTask)
		{
			var valueTaskAwaiter = valueTask.GetAwaiter();
			if (valueTaskAwaiter.IsCompleted)
				return;

			// Let the task complete on a thread pool thread while we block the main thread
			Task.Run(valueTask.AsTask).GetAwaiter().GetResult();
		}

		static T WaitForValueTask<T>(ValueTask<T> valueTask)
		{
			var valueTaskAwaiter = valueTask.GetAwaiter();
			if (valueTaskAwaiter.IsCompleted)
				return valueTaskAwaiter.GetResult();

			// Let the task complete on a thread pool thread while we block the main thread
			return Task.Run(valueTask.AsTask).GetAwaiter().GetResult();
		}

#endif  // NET8_0_OR_GREATER
	}
}
