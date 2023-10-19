#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8603
#endif

using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Xunit
{
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class Assert
	{
		/// <summary>
		/// Records any exception which is thrown by the given code.
		/// </summary>
		/// <param name="testCode">The code which may thrown an exception.</param>
		/// <returns>Returns the exception that was thrown by the code; null, otherwise.</returns>
#if XUNIT_NULLABLE
		protected static Exception? RecordException(Action testCode)
#else
		protected static Exception RecordException(Action testCode)
#endif
		{
			GuardArgumentNotNull(nameof(testCode), testCode);

			try
			{
				testCode();
				return null;
			}
			catch (Exception ex)
			{
				return ex;
			}
		}

		/// <summary>
		/// Records any exception which is thrown by the given code that has
		/// a return value. Generally used for testing property accessors.
		/// </summary>
		/// <param name="testCode">The code which may thrown an exception.</param>
		/// <param name="asyncMethodName">The name of the async method the user should've called if they accidentally
		/// passed in an async function</param>
		/// <returns>Returns the exception that was thrown by the code; null, otherwise.</returns>
#if XUNIT_NULLABLE
		protected static Exception? RecordException(
			Func<object?> testCode,
#else
		protected static Exception RecordException(
			Func<object> testCode,
#endif
			string asyncMethodName)
		{
			GuardArgumentNotNull(nameof(testCode), testCode);

			var result = default(object);

			try
			{
				result = testCode();
			}
			catch (Exception ex)
			{
				return ex;
			}

#if XUNIT_VALUETASK
			if (result is Task || result is ValueTask)
#else
			if (result is Task)
#endif
				throw new InvalidOperationException($"You must call Assert.{asyncMethodName} when testing async code");

			return null;
		}

		/// <summary/>
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("You must call Assert.RecordExceptionAsync (and await the result) when testing async code.", true)]
		protected static Exception RecordException(Func<Task> testCode)
		{
			throw new NotImplementedException();
		}

#if XUNIT_VALUETASK
		/// <summary/>
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("You must call Assert.RecordExceptionAsync (and await the result) when testing async code.", true)]
		protected static Exception RecordException(Func<ValueTask> testCode)
		{
			throw new NotImplementedException();
		}

		/// <summary/>
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("You must call Assert.RecordExceptionAsync (and await the result) when testing async code.", true)]
		protected static Exception RecordException<T>(Func<ValueTask<T>> testCode)
		{
			throw new NotImplementedException();
		}
#endif

		/// <summary>
		/// Records any exception which is thrown by the given task.
		/// </summary>
		/// <param name="testCode">The task which may thrown an exception.</param>
		/// <returns>Returns the exception that was thrown by the code; null, otherwise.</returns>
#if XUNIT_NULLABLE
		protected static async Task<Exception?> RecordExceptionAsync(Func<Task> testCode)
#else
		protected static async Task<Exception> RecordExceptionAsync(Func<Task> testCode)
#endif
		{
			GuardArgumentNotNull(nameof(testCode), testCode);

			try
			{
				await testCode();
				return null;
			}
			catch (Exception ex)
			{
				return ex;
			}
		}

#if XUNIT_VALUETASK
		/// <summary>
		/// Records any exception which is thrown by the given task.
		/// </summary>
		/// <param name="testCode">The task which may thrown an exception.</param>
		/// <returns>Returns the exception that was thrown by the code; null, otherwise.</returns>
#if XUNIT_NULLABLE
		protected static async ValueTask<Exception?> RecordExceptionAsync(Func<ValueTask> testCode)
#else
		protected static async ValueTask<Exception> RecordExceptionAsync(Func<ValueTask> testCode)
#endif
		{
			GuardArgumentNotNull(nameof(testCode), testCode);

			try
			{
				await testCode();
				return null;
			}
			catch (Exception ex)
			{
				return ex;
			}
		}

		/// <summary>
		/// Records any exception which is thrown by the given task.
		/// </summary>
		/// <param name="testCode">The task which may thrown an exception.</param>
		/// <typeparam name="T">The type of the ValueTask return value.</typeparam>
		/// <returns>Returns the exception that was thrown by the code; null, otherwise.</returns>
#if XUNIT_NULLABLE
		protected static async ValueTask<Exception?> RecordExceptionAsync<T>(Func<ValueTask<T>> testCode)
#else
		protected static async ValueTask<Exception> RecordExceptionAsync<T>(Func<ValueTask<T>> testCode)
#endif
		{
			GuardArgumentNotNull(nameof(testCode), testCode);

			try
			{
				await testCode();
				return null;
			}
			catch (Exception ex)
			{
				return ex;
			}
		}
#endif
	}
}
