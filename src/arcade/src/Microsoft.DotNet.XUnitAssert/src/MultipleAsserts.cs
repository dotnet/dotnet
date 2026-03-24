#pragma warning disable CA1031 // Do not catch general exception types
#pragma warning disable CA1052 // Static holder types should be static
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task

#if XUNIT_NULLABLE
#nullable enable
#endif

using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace Xunit
{
	partial class Assert
	{
		/// <summary>
		/// Runs multiple checks, collecting the exceptions from each one, and then bundles all failures
		/// up into a single assertion failure.
		/// </summary>
		/// <param name="checks">The individual assertions to run, as actions.</param>
		public static void Multiple(params Action[] checks)
		{
			if (checks == null || checks.Length == 0)
				return;

			var exceptions = new List<Exception>();

			foreach (var check in checks)
				try
				{
					check();
				}
				catch (Exception ex)
				{
					exceptions.Add(ex);
				}

			if (exceptions.Count == 0)
				return;
			if (exceptions.Count == 1)
				ExceptionDispatchInfo.Capture(exceptions[0]).Throw();

			throw MultipleException.ForFailures(exceptions);
		}

		/// <summary>
		/// Asynchronously runs multiple checks, collecting the exceptions from each one, and then bundles all failures
		/// up into a single assertion failure.
		/// </summary>
		/// <param name="checks">The individual assertions to run, as async actions.</param>
		public static async Task MultipleAsync(params Func<Task>[] checks)
		{
			if (checks == null || checks.Length == 0)
				return;

			var exceptions = new List<Exception>();

			foreach (var check in checks)
				try
				{
					await check();
				}
				catch (Exception ex)
				{
					exceptions.Add(ex);
				}

			if (exceptions.Count == 0)
				return;
			if (exceptions.Count == 1)
				ExceptionDispatchInfo.Capture(exceptions[0]).Throw();

			throw MultipleException.ForFailuresAsync(exceptions);
		}
	}
}
