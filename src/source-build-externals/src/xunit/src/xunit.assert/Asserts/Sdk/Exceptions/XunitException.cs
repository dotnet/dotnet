#if XUNIT_NULLABLE
#nullable enable
#endif

using System;

namespace Xunit.Sdk
{
	/// <summary>
	/// The base assert exception class
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	class XunitException : Exception, IAssertionException
	{
#if XUNIT_NULLABLE
		readonly string? stackTrace;
#else
		readonly string stackTrace;
#endif

		/// <summary>
		/// Initializes a new instance of the <see cref="XunitException"/> class.
		/// </summary>
		public XunitException() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="XunitException"/> class.
		/// </summary>
		/// <param name="userMessage">The user message to be displayed</param>
#if XUNIT_NULLABLE
		public XunitException(string? userMessage)
			: this(userMessage, (Exception?)null) { }
#else
		public XunitException(string userMessage)
			: this(userMessage, (Exception)null) { }
#endif

		/// <summary>
		/// Initializes a new instance of the <see cref="XunitException"/> class.
		/// </summary>
		/// <param name="userMessage">The user message to be displayed</param>
		/// <param name="innerException">The inner exception</param>
#if XUNIT_NULLABLE
		public XunitException(string? userMessage, Exception? innerException)
#else
		public XunitException(string userMessage, Exception innerException)
#endif
			: base(userMessage, innerException)
		{
			UserMessage = userMessage;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="XunitException"/> class.
		/// </summary>
		/// <param name="userMessage">The user message to be displayed</param>
		/// <param name="stackTrace">The stack trace to be displayed</param>
#if XUNIT_NULLABLE
		protected XunitException(string? userMessage, string? stackTrace)
#else
		protected XunitException(string userMessage, string stackTrace)
#endif
			: this(userMessage)
		{
			this.stackTrace = stackTrace;
		}

		/// <summary>
		/// Gets a string representation of the frames on the call stack at the time the current exception was thrown.
		/// </summary>
		/// <returns>A string that describes the contents of the call stack, with the most recent method call appearing first.</returns>
#if XUNIT_NULLABLE
		public override string? StackTrace => stackTrace ?? base.StackTrace;
#else
		public override string StackTrace => stackTrace ?? base.StackTrace;
#endif

		/// <summary>
		/// Gets the user message
		/// </summary>
#if XUNIT_NULLABLE
		public string? UserMessage { get; protected set; }
#else
		public string UserMessage { get; protected set; }
#endif

		/// <inheritdoc/>
		public override string ToString()
		{
			var className = GetType().ToString();
			var message = Message;
			var result = default(string);

			if (message == null || message.Length <= 0)
				result = className;
			else
				result = string.Format("{0}: {1}", className, message);

			var stackTrace = StackTrace;
			if (stackTrace != null)
				result = result + Environment.NewLine + stackTrace;

			return result;
		}
	}
}
