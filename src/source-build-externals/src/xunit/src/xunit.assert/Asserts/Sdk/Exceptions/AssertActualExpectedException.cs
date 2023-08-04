#if XUNIT_NULLABLE
#nullable enable

using System.Diagnostics.CodeAnalysis;
#endif

using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Xunit.Sdk
{
	/// <summary>
	/// Base class for exceptions that have actual and expected values
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	class AssertActualExpectedException : XunitException
	{
		/// <summary>
		/// Creates a new instance of the <see href="AssertActualExpectedException"/> class.
		/// </summary>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The actual value</param>
		/// <param name="userMessage">The user message to be shown</param>
		/// <param name="expectedTitle">The title to use for the expected value (defaults to "Expected")</param>
		/// <param name="actualTitle">The title to use for the actual value (defaults to "Actual")</param>
#if XUNIT_NULLABLE
		public AssertActualExpectedException(
			object? expected,
			object? actual,
			string? userMessage,
			string? expectedTitle = null,
			string? actualTitle = null) :
#else
		public AssertActualExpectedException(
			object expected,
			object actual,
			string userMessage,
			string expectedTitle = null,
			string actualTitle = null) :
#endif
				this(expected, actual, userMessage, expectedTitle, actualTitle, null)
		{ }

		/// <summary>
		/// Creates a new instance of the <see href="AssertActualExpectedException"/> class.
		/// </summary>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The actual value</param>
		/// <param name="userMessage">The user message to be shown</param>
		/// <param name="expectedTitle">The title to use for the expected value (defaults to "Expected")</param>
		/// <param name="actualTitle">The title to use for the actual value (defaults to "Actual")</param>
		/// <param name="innerException">The inner exception.</param>
#if XUNIT_NULLABLE
		public AssertActualExpectedException(
			object? expected,
			object? actual,
			string? userMessage,
			string? expectedTitle,
			string? actualTitle,
			Exception? innerException) :
#else
		public AssertActualExpectedException(
			object expected,
			object actual,
			string userMessage,
			string expectedTitle,
			string actualTitle,
			Exception innerException) :
#endif
				base(userMessage, innerException)
		{
			Actual = ConvertToString(actual);
			ActualTitle = actualTitle ?? "Actual";
			Expected = ConvertToString(expected);
			ExpectedTitle = expectedTitle ?? "Expected";

			if (actual != null &&
				expected != null &&
				Actual == Expected &&
				actual.GetType() != expected.GetType())
			{
				Actual += string.Format(CultureInfo.CurrentCulture, " ({0})", actual.GetType().FullName);
				Expected += string.Format(CultureInfo.CurrentCulture, " ({0})", expected.GetType().FullName);
			}
		}

		/// <summary>
		/// Gets the actual value.
		/// </summary>
#if XUNIT_NULLABLE
		public string? Actual { get; }
#else
		public string Actual { get; }
#endif

		/// <summary>
		/// Gets the title used for the actual value.
		/// </summary>
		public string ActualTitle { get; }

		/// <summary>
		/// Gets the expected value.
		/// </summary>
#if XUNIT_NULLABLE
		public string? Expected { get; }
#else
		public string Expected { get; }
#endif

		/// <summary>
		/// Gets the title used for the expected value.
		/// </summary>
		public string ExpectedTitle { get; }

		/// <summary>
		/// Gets a message that describes the current exception. Includes the expected and actual values.
		/// </summary>
		/// <returns>The error message that explains the reason for the exception, or an empty string("").</returns>
		/// <filterpriority>1</filterpriority>
		public override string Message
		{
			get
			{
				var titleLength = Math.Max(ExpectedTitle.Length, ActualTitle.Length) + 2;  // + the colon and space
				var formattedExpectedTitle = (ExpectedTitle + ":").PadRight(titleLength);
				var formattedActualTitle = (ActualTitle + ":").PadRight(titleLength);
				var indentedNewLine = Environment.NewLine + " ".PadRight(titleLength);

				return string.Format(
					CultureInfo.CurrentCulture,
					"{0}{5}{1}{2}{5}{3}{4}",
					base.Message,
					formattedExpectedTitle,
					(Expected ?? "(null)").Replace(Environment.NewLine, indentedNewLine),
					formattedActualTitle,
					(Actual ?? "(null)").Replace(Environment.NewLine, indentedNewLine),
					Environment.NewLine
				);
			}
		}

		static string ConvertToSimpleTypeName(TypeInfo typeInfo)
		{
			if (!typeInfo.IsGenericType)
				return typeInfo.Name;

			var simpleNames = typeInfo.GenericTypeArguments.Select(type => ConvertToSimpleTypeName(type.GetTypeInfo()));
			var backTickIdx = typeInfo.Name.IndexOf('`');
			if (backTickIdx < 0)
				backTickIdx = typeInfo.Name.Length;  // F# doesn't use backticks for generic type names

			return string.Format("{0}<{1}>", typeInfo.Name.Substring(0, backTickIdx), string.Join(", ", simpleNames));
		}

#if XUNIT_NULLABLE
		[return: NotNullIfNotNull("value")]
		static string? ConvertToString(object? value)
#else
		static string ConvertToString(object value)
#endif
		{
			if (value == null)
				return null;

			var stringValue = value as string;
			if (stringValue != null)
				return stringValue;

			var formattedValue = ArgumentFormatter.Format(value);
			if (value is IEnumerable)
				formattedValue = string.Format("{0} {1}", ConvertToSimpleTypeName(value.GetType().GetTypeInfo()), formattedValue);

			return formattedValue;
		}
	}
}
