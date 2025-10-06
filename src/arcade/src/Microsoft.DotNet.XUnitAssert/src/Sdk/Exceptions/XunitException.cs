#pragma warning disable CA1032 // Implement standard exception constructors
#pragma warning disable IDE0040 // Add accessibility modifiers
#pragma warning disable IDE0090 // Use 'new(...)'
#pragma warning disable IDE0161 // Convert to file-scoped namespace
#pragma warning disable IDE0290 // Use primary constructor

#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8625
#endif

using System;
using System.Globalization;

namespace Xunit.Sdk
{
	/// <summary>
	/// The base assert exception class. It marks itself with <see cref="IAssertionException"/> which is how
	/// the framework differentiates between assertion fails and general exceptions.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class XunitException : Exception, IAssertionException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="XunitException"/> class.
		/// </summary>
		/// <param name="userMessage">The user message to be displayed</param>
		public XunitException(
#if XUNIT_NULLABLE
			string? userMessage) :
#else
			string userMessage) :
#endif
				this(userMessage, null)
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="XunitException"/> class.
		/// </summary>
		/// <param name="userMessage">The user message to be displayed</param>
		/// <param name="innerException">The inner exception</param>
		public XunitException(
#if XUNIT_NULLABLE
			string? userMessage,
			Exception? innerException) :
#else
			string userMessage,
			Exception innerException) :
#endif
				base(userMessage, innerException)
		{ }

		/// <inheritdoc/>
		public override string ToString()
		{
			var className = GetType().ToString();
			var message = Message;
			var result =
				message == null || message.Length <= 0
					? className
					: string.Format(CultureInfo.CurrentCulture, "{0}: {1}", className, message);

			var stackTrace = StackTrace;
			if (stackTrace != null)
				result = string.Format(CultureInfo.CurrentCulture, "{0}{1}{2}", result, Environment.NewLine, stackTrace);

			return result;
		}
	}
}
