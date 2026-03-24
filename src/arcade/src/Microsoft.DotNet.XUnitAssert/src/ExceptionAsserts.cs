#pragma warning disable CA1052 // Static holder types should be static
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task

#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8603
#pragma warning disable CS8604
#pragma warning disable CS8625
#endif

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace Xunit
{
	partial class Assert
	{
		/// <summary>
		/// Verifies that the exact exception is thrown (and not a derived exception type).
		/// </summary>
		/// <param name="exceptionType">The type of the exception expected to be thrown</param>
		/// <param name="testCode">A delegate to the code to be tested</param>
		/// <returns>The exception that was thrown, when successful</returns>
		public static Exception Throws(
			Type exceptionType,
			Action testCode) =>
				ThrowsImpl(exceptionType, RecordException(testCode));

		/// <summary>
		/// Verifies that the exact exception is thrown (and not a derived exception type).
		/// </summary>
		/// <param name="exceptionType">The type of the exception expected to be thrown</param>
		/// <param name="testCode">A delegate to the code to be tested</param>
		/// <param name="inspector">A function which inspects the exception to determine if it's
		/// valid or not. Returns <see langword="null"/> if the exception is valid, or a message if it's not.</param>
		/// <returns>The exception that was thrown, when successful</returns>
		public static Exception Throws(
			Type exceptionType,
			Action testCode,
#if XUNIT_NULLABLE
			Func<Exception, string?> inspector) =>
#else
			Func<Exception, string> inspector) =>
#endif
				ThrowsImpl(exceptionType, RecordException(testCode), inspector);

		/// <summary>
		/// Verifies that the exact exception is thrown (and not a derived exception type).
		/// Generally used to test property accessors.
		/// </summary>
		/// <param name="exceptionType">The type of the exception expected to be thrown</param>
		/// <param name="testCode">A delegate to the code to be tested</param>
		/// <returns>The exception that was thrown, when successful</returns>
		public static Exception Throws(
			Type exceptionType,
#if XUNIT_NULLABLE
			Func<object?> testCode) =>
#else
			Func<object> testCode) =>
#endif
				ThrowsImpl(exceptionType, RecordException(testCode, nameof(ThrowsAsync)));

		/// <summary>
		/// Verifies that the exact exception is thrown (and not a derived exception type).
		/// Generally used to test property accessors.
		/// </summary>
		/// <param name="exceptionType">The type of the exception expected to be thrown</param>
		/// <param name="testCode">A delegate to the code to be tested</param>
		/// <param name="inspector">A function which inspects the exception to determine if it's
		/// valid or not. Returns <see langword="null"/> if the exception is valid, or a message if it's not.</param>
		/// <returns>The exception that was thrown, when successful</returns>
		public static Exception Throws(
			Type exceptionType,
#if XUNIT_NULLABLE
			Func<object?> testCode,
			Func<Exception, string?> inspector) =>
#else
			Func<object> testCode,
			Func<Exception, string> inspector) =>
#endif
				ThrowsImpl(exceptionType, RecordException(testCode, nameof(ThrowsAsync)), inspector);

		/// <summary/>
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("You must call Assert.ThrowsAsync (and await the result) when testing async code.", true)]
		public static Exception Throws(
			Type exceptionType,
			Func<Task> testCode)
		{
			throw new NotSupportedException("You must call Assert.ThrowsAsync (and await the result) when testing async code.");
		}

		/// <summary/>
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("You must call Assert.ThrowsAsync (and await the result) when testing async code.", true)]
		public static Exception Throws(
			Type exceptionType,
			Func<Task> testCode,
#if XUNIT_NULLABLE
			Func<Exception, string?> inspector)
#else
			Func<Exception, string> inspector)
#endif
		{
			throw new NotSupportedException("You must call Assert.ThrowsAsync (and await the result) when testing async code.");
		}

		/// <summary>
		/// Verifies that the exact exception is thrown (and not a derived exception type).
		/// </summary>
		/// <typeparam name="T">The type of the exception expected to be thrown</typeparam>
		/// <param name="testCode">A delegate to the code to be tested</param>
		/// <returns>The exception that was thrown, when successful</returns>
		public static T Throws<T>(Action testCode)
			where T : Exception =>
				(T)ThrowsImpl(typeof(T), RecordException(testCode));

		/// <summary>
		/// Verifies that the exact exception is thrown (and not a derived exception type).
		/// </summary>
		/// <typeparam name="T">The type of the exception expected to be thrown</typeparam>
		/// <param name="testCode">A delegate to the code to be tested</param>
		/// <param name="inspector">A function which inspects the exception to determine if it's
		/// valid or not. Returns <see langword="null"/> if the exception is valid, or a message if it's not.</param>
		/// <returns>The exception that was thrown, when successful</returns>
		public static T Throws<T>(
			Action testCode,
#if XUNIT_NULLABLE
			Func<T, string?> inspector)
#else
			Func<T, string> inspector)
#endif
				where T : Exception =>
					(T)ThrowsImpl(typeof(T), RecordException(testCode), ex => inspector((T)ex));

		/// <summary>
		/// Verifies that the exact exception is thrown (and not a derived exception type).
		/// Generally used to test property accessors.
		/// </summary>
		/// <typeparam name="T">The type of the exception expected to be thrown</typeparam>
		/// <param name="testCode">A delegate to the code to be tested</param>
		/// <returns>The exception that was thrown, when successful</returns>
#if XUNIT_NULLABLE
		public static T Throws<T>(Func<object?> testCode)
#else
		public static T Throws<T>(Func<object> testCode)
#endif
			where T : Exception =>
				(T)ThrowsImpl(typeof(T), RecordException(testCode, nameof(ThrowsAsync)));

		/// <summary>
		/// Verifies that the exact exception is thrown (and not a derived exception type).
		/// Generally used to test property accessors.
		/// </summary>
		/// <typeparam name="T">The type of the exception expected to be thrown</typeparam>
		/// <param name="testCode">A delegate to the code to be tested</param>
		/// <param name="inspector">A function which inspects the exception to determine if it's
		/// valid or not. Returns <see langword="null"/> if the exception is valid, or a message if it's not.</param>
		/// <returns>The exception that was thrown, when successful</returns>
		public static T Throws<T>(
#if XUNIT_NULLABLE
			Func<object?> testCode,
			Func<T, string?> inspector)
#else
			Func<object> testCode,
			Func<T, string> inspector)
#endif
				where T : Exception =>
					(T)ThrowsImpl(typeof(T), RecordException(testCode, nameof(ThrowsAsync)), ex => inspector((T)ex));

		/// <summary/>
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("You must call Assert.ThrowsAsync<T> (and await the result) when testing async code.", true)]
		public static T Throws<T>(Func<Task> testCode)
			where T : Exception
		{
			throw new NotSupportedException("You must call Assert.ThrowsAsync<T> (and await the result) when testing async code.");
		}

		/// <summary/>
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("You must call Assert.ThrowsAsync<T> (and await the result) when testing async code.", true)]
		public static T Throws<T>(
			Func<Task> testCode,
#if XUNIT_NULLABLE
			Func<T, string?> inspector)
#else
			Func<T, string> inspector)
#endif
				where T : Exception
		{
			throw new NotSupportedException("You must call Assert.ThrowsAsync<T> (and await the result) when testing async code.");
		}

		/// <summary>
		/// Verifies that the exact exception is thrown (and not a derived exception type), where the exception
		/// derives from <see cref="ArgumentException"/> and has the given parameter name.
		/// </summary>
		/// <param name="paramName">The parameter name that is expected to be in the exception</param>
		/// <param name="testCode">A delegate to the code to be tested</param>
		/// <returns>The exception that was thrown, when successful</returns>
		public static T Throws<T>(
#if XUNIT_NULLABLE
			string? paramName,
#else
			string paramName,
#endif
			Action testCode)
				where T : ArgumentException
		{
			var ex = Throws<T>(testCode);

			if (paramName != ex.ParamName)
				throw ThrowsException.ForIncorrectParameterName(typeof(T), paramName, ex.ParamName);

			return ex;
		}

		/// <summary>
		/// Verifies that the exact exception is thrown (and not a derived exception type), where the exception
		/// derives from <see cref="ArgumentException"/> and has the given parameter name.
		/// </summary>
		/// <param name="paramName">The parameter name that is expected to be in the exception</param>
		/// <param name="testCode">A delegate to the code to be tested</param>
		/// <returns>The exception that was thrown, when successful</returns>
		public static T Throws<T>(
#if XUNIT_NULLABLE
			string? paramName,
			Func<object?> testCode)
#else
			string paramName,
			Func<object> testCode)
#endif
				where T : ArgumentException
		{
			var ex = Throws<T>(testCode);

			if (paramName != ex.ParamName)
				throw ThrowsException.ForIncorrectParameterName(typeof(T), paramName, ex.ParamName);

			return ex;
		}

		/// <summary/>
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("You must call Assert.ThrowsAsync<T> (and await the result) when testing async code.", true)]
		public static T Throws<T>(
#if XUNIT_NULLABLE
			string? paramName,
#else
			string paramName,
#endif
			Func<Task> testCode)
				where T : ArgumentException
		{
			throw new NotSupportedException("You must call Assert.ThrowsAsync<T> (and await the result) when testing async code.");
		}

		/// <summary>
		/// Verifies that the exact exception or a derived exception type is thrown.
		/// </summary>
		/// <typeparam name="T">The type of the exception expected to be thrown</typeparam>
		/// <param name="testCode">A delegate to the code to be tested</param>
		/// <returns>The exception that was thrown, when successful</returns>
		public static T ThrowsAny<T>(Action testCode)
			where T : Exception =>
				(T)ThrowsAnyImpl(typeof(T), RecordException(testCode));

		/// <summary>
		/// Verifies that the exact exception or a derived exception type is thrown.
		/// </summary>
		/// <typeparam name="T">The type of the exception expected to be thrown</typeparam>
		/// <param name="testCode">A delegate to the code to be tested</param>
		/// <param name="inspector">A function which inspects the exception to determine if it's
		/// valid or not. Returns <see langword="null"/> if the exception is valid, or a message if it's not.</param>
		/// <returns>The exception that was thrown, when successful</returns>
		public static T ThrowsAny<T>(
			Action testCode,
#if XUNIT_NULLABLE
			Func<T, string?> inspector)
#else
			Func<T, string> inspector)
#endif
				where T : Exception =>
					(T)ThrowsAnyImpl(typeof(T), RecordException(testCode), ex => inspector((T)ex));

		/// <summary>
		/// Verifies that the exact exception or a derived exception type is thrown.
		/// Generally used to test property accessors.
		/// </summary>
		/// <typeparam name="T">The type of the exception expected to be thrown</typeparam>
		/// <param name="testCode">A delegate to the code to be tested</param>
		/// <returns>The exception that was thrown, when successful</returns>
#if XUNIT_NULLABLE
		public static T ThrowsAny<T>(Func<object?> testCode)
#else
		public static T ThrowsAny<T>(Func<object> testCode)
#endif
			where T : Exception =>
				(T)ThrowsAnyImpl(typeof(T), RecordException(testCode, nameof(ThrowsAnyAsync)));

		/// <summary>
		/// Verifies that the exact exception or a derived exception type is thrown.
		/// Generally used to test property accessors.
		/// </summary>
		/// <typeparam name="T">The type of the exception expected to be thrown</typeparam>
		/// <param name="testCode">A delegate to the code to be tested</param>
		/// <param name="inspector">A function which inspects the exception to determine if it's
		/// valid or not. Returns <see langword="null"/> if the exception is valid, or a message if it's not.</param>
		/// <returns>The exception that was thrown, when successful</returns>
		public static T ThrowsAny<T>(
#if XUNIT_NULLABLE
			Func<object?> testCode,
			Func<T, string?> inspector)
#else
			Func<object> testCode,
			Func<T, string> inspector)
#endif
			where T : Exception =>
				(T)ThrowsAnyImpl(typeof(T), RecordException(testCode, nameof(ThrowsAnyAsync)), ex => inspector((T)ex));

		/// <summary/>
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("You must call Assert.ThrowsAnyAsync<T> (and await the result) when testing async code.", true)]
		public static T ThrowsAny<T>(Func<Task> testCode)
			where T : Exception
		{
			throw new NotSupportedException("You must call Assert.ThrowsAnyAsync<T> (and await the result) when testing async code.");
		}

		/// <summary/>
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("You must call Assert.ThrowsAnyAsync<T> (and await the result) when testing async code.", true)]
		public static T ThrowsAny<T>(
			Func<Task> testCode,
#if XUNIT_NULLABLE
			Func<T, string?> inspector)
#else
			Func<T, string> inspector)
#endif
				where T : Exception
		{
			throw new NotSupportedException("You must call Assert.ThrowsAnyAsync<T> (and await the result) when testing async code.");
		}

		/// <summary>
		/// Verifies that the exact exception or a derived exception type is thrown.
		/// </summary>
		/// <typeparam name="T">The type of the exception expected to be thrown</typeparam>
		/// <param name="testCode">A delegate to the task to be tested</param>
		/// <returns>The exception that was thrown, when successful</returns>
		public static async Task<T> ThrowsAnyAsync<T>(Func<Task> testCode)
			where T : Exception =>
				(T)ThrowsAnyImpl(typeof(T), await RecordExceptionAsync(testCode));

		/// <summary>
		/// Verifies that the exact exception or a derived exception type is thrown.
		/// </summary>
		/// <typeparam name="T">The type of the exception expected to be thrown</typeparam>
		/// <param name="testCode">A delegate to the task to be tested</param>
		/// <param name="inspector">A function which inspects the exception to determine if it's
		/// valid or not. Returns <see langword="null"/> if the exception is valid, or a message if it's not.</param>
		/// <returns>The exception that was thrown, when successful</returns>
		public static async Task<T> ThrowsAnyAsync<T>(
			Func<Task> testCode,
#if XUNIT_NULLABLE
			Func<T, string?> inspector)
#else
			Func<T, string> inspector)
#endif
				where T : Exception =>
					(T)ThrowsAnyImpl(typeof(T), await RecordExceptionAsync(testCode), ex => inspector((T)ex));

		/// <summary>
		/// Verifies that the exact exception is thrown (and not a derived exception type).
		/// </summary>
		/// <param name="exceptionType">The type of the exception expected to be thrown</param>
		/// <param name="testCode">A delegate to the task to be tested</param>
		/// <returns>The exception that was thrown, when successful</returns>
		public static async Task<Exception> ThrowsAsync(
			Type exceptionType,
			Func<Task> testCode) =>
				ThrowsImpl(exceptionType, await RecordExceptionAsync(testCode));

		/// <summary>
		/// Verifies that the exact exception is thrown (and not a derived exception type).
		/// </summary>
		/// <param name="exceptionType">The type of the exception expected to be thrown</param>
		/// <param name="testCode">A delegate to the task to be tested</param>
		/// <param name="inspector">A function which inspects the exception to determine if it's
		/// valid or not. Returns <see langword="null"/> if the exception is valid, or a message if it's not.</param>
		/// <returns>The exception that was thrown, when successful</returns>
		public static async Task<Exception> ThrowsAsync(
			Type exceptionType,
			Func<Task> testCode,
#if XUNIT_NULLABLE
			Func<Exception, string?> inspector) =>
#else
			Func<Exception, string> inspector) =>
#endif
				ThrowsImpl(exceptionType, await RecordExceptionAsync(testCode), inspector);

		/// <summary>
		/// Verifies that the exact exception is thrown (and not a derived exception type).
		/// </summary>
		/// <typeparam name="T">The type of the exception expected to be thrown</typeparam>
		/// <param name="testCode">A delegate to the task to be tested</param>
		/// <returns>The exception that was thrown, when successful</returns>
		public static async Task<T> ThrowsAsync<T>(Func<Task> testCode)
			where T : Exception =>
				(T)ThrowsImpl(typeof(T), await RecordExceptionAsync(testCode));

		/// <summary>
		/// Verifies that the exact exception is thrown (and not a derived exception type).
		/// </summary>
		/// <typeparam name="T">The type of the exception expected to be thrown</typeparam>
		/// <param name="testCode">A delegate to the task to be tested</param>
		/// <param name="inspector">A function which inspects the exception to determine if it's
		/// valid or not. Returns <see langword="null"/> if the exception is valid, or a message if it's not.</param>
		/// <returns>The exception that was thrown, when successful</returns>
		public static async Task<T> ThrowsAsync<T>(
			Func<Task> testCode,
#if XUNIT_NULLABLE
			Func<T, string?> inspector)
#else
			Func<T, string> inspector)
#endif
				where T : Exception =>
					(T)ThrowsImpl(typeof(T), await RecordExceptionAsync(testCode), ex => inspector((T)ex));

		/// <summary>
		/// Verifies that the exact exception is thrown (and not a derived exception type), where the exception
		/// derives from <see cref="ArgumentException"/> and has the given parameter name.
		/// </summary>
		/// <param name="paramName">The parameter name that is expected to be in the exception</param>
		/// <param name="testCode">A delegate to the task to be tested</param>
		/// <returns>The exception that was thrown, when successful</returns>
		public static async Task<T> ThrowsAsync<T>(
#if XUNIT_NULLABLE
			string? paramName,
#else
			string paramName,
#endif
			Func<Task> testCode)
				where T : ArgumentException
		{
			var ex = await ThrowsAsync<T>(testCode);

			if (paramName != ex.ParamName)
				throw ThrowsException.ForIncorrectParameterName(typeof(T), paramName, ex.ParamName);

			return ex;
		}

		static Exception ThrowsAnyImpl(
			Type exceptionType,
#if XUNIT_NULLABLE
			Exception? exception,
			Func<Exception, string?>? inspector = null)
#else
			Exception exception,
			Func<Exception, string> inspector = null)
#endif
		{
			GuardArgumentNotNull(nameof(exceptionType), exceptionType);

			if (exception == null)
				throw ThrowsAnyException.ForNoException(exceptionType);

			if (!exceptionType.IsAssignableFrom(exception.GetType()))
				throw ThrowsAnyException.ForIncorrectExceptionType(exceptionType, exception);

			var message = default(string);
			try
			{
				message = inspector?.Invoke(exception);
			}
			catch (Exception ex)
			{
				throw ThrowsAnyException.ForInspectorFailure("Exception thrown by inspector", ex);
			}

			if (message != null)
				throw ThrowsAnyException.ForInspectorFailure(message);

			return exception;
		}

		static Exception ThrowsImpl(
			Type exceptionType,
#if XUNIT_NULLABLE
			Exception? exception,
			Func<Exception, string?>? inspector = null)
#else
			Exception exception,
			Func<Exception, string> inspector = null)
#endif
		{
			GuardArgumentNotNull(nameof(exceptionType), exceptionType);

			if (exception == null)
				throw ThrowsException.ForNoException(exceptionType);

			if (!exceptionType.Equals(exception.GetType()))
				throw ThrowsException.ForIncorrectExceptionType(exceptionType, exception);

			var message = default(string);
			try
			{
				message = inspector?.Invoke(exception);
			}
			catch (Exception ex)
			{
				throw ThrowsException.ForInspectorFailure("Exception thrown by inspector", ex);
			}

			if (message != null)
				throw ThrowsException.ForInspectorFailure(message);

			return exception;
		}
	}
}
