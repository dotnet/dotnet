#pragma warning disable CA1034 // Nested types should not be visible
#pragma warning disable CA1052 // Static holder types should be static
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
#pragma warning disable IDE0039 // Use local function
#pragma warning disable IDE0040 // Add accessibility modifiers
#pragma warning disable IDE0046 // Convert to conditional expression
#pragma warning disable IDE0058 // Expression value is never used
#pragma warning disable IDE0161 // Convert to file-scoped namespace
#pragma warning disable IDE0270 // Use coalesce expression
#pragma warning disable IDE0290 // Use primary constructor

#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8600
#pragma warning disable CS8603
#pragma warning disable CS8622
#pragma warning disable CS8625
#endif

using System;
using System.Threading.Tasks;
using Xunit.Sdk;

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
		/// Verifies that an event is raised.
		/// </summary>
		/// <param name="attach">Code to attach the event handler</param>
		/// <param name="detach">Code to detach the event handler</param>
		/// <param name="testCode">A delegate to the code to be tested</param>
		/// <exception cref="RaisesException">Thrown when the expected event was not raised.</exception>
		public static void Raises(
			Action<Action> attach,
			Action<Action> detach,
			Action testCode)
		{
			if (!RaisesInternal(attach, detach, testCode))
				throw RaisesException.ForNoEvent();
		}

		/// <summary>
		/// Verifies that an event with the exact event args is raised.
		/// </summary>
		/// <typeparam name="T">The type of the event arguments to expect</typeparam>
		/// <param name="attach">Code to attach the event handler</param>
		/// <param name="detach">Code to detach the event handler</param>
		/// <param name="testCode">A delegate to the code to be tested</param>
		/// <returns>The event sender and arguments wrapped in an object</returns>
		/// <exception cref="RaisesException">Thrown when the expected event was not raised.</exception>
		public static RaisedEvent<T> Raises<T>(
			Action<Action<T>> attach,
			Action<Action<T>> detach,
			Action testCode)
		{
			var raisedEvent = RaisesInternal(attach, detach, testCode);

			if (raisedEvent == null)
				throw RaisesException.ForNoEvent(typeof(T));

			if (raisedEvent.Arguments != null && !raisedEvent.Arguments.GetType().Equals(typeof(T)))
				throw RaisesException.ForIncorrectType(typeof(T), raisedEvent.Arguments.GetType());

			return raisedEvent;
		}

		/// <summary>
		/// Verifies that an event with the exact event args is raised.
		/// </summary>
		/// <typeparam name="T">The type of the event arguments to expect</typeparam>
		/// <param name="attach">Code to attach the event handler</param>
		/// <param name="detach">Code to detach the event handler</param>
		/// <param name="testCode">A delegate to the code to be tested</param>
		/// <returns>The event sender and arguments wrapped in an object</returns>
		/// <exception cref="RaisesException">Thrown when the expected event was not raised.</exception>
		public static RaisedEvent<T> Raises<T>(
			Action<EventHandler<T>> attach,
			Action<EventHandler<T>> detach,
			Action testCode)
		{
			var raisedEvent = RaisesInternal(attach, detach, testCode);

			if (raisedEvent == null)
				throw RaisesException.ForNoEvent(typeof(T));

			if (raisedEvent.Arguments != null && !raisedEvent.Arguments.GetType().Equals(typeof(T)))
				throw RaisesException.ForIncorrectType(typeof(T), raisedEvent.Arguments.GetType());

			return raisedEvent;
		}

		/// <summary>
		/// Verifies that an event with the exact event args is raised.
		/// </summary>
		/// <typeparam name="T">The type of the event arguments to expect</typeparam>
		/// <param name="handler">Code returning the raised event</param>
		/// <param name="attach">Code to attach the event handler</param>
		/// <param name="detach">Code to detach the event handler</param>
		/// <param name="testCode">A delegate to the code to be tested</param>
		/// <returns>The event sender and arguments wrapped in an object</returns>
		/// <exception cref="RaisesException">Thrown when the expected event was not raised.</exception>
		public static RaisedEvent<T> Raises<T>(
#if XUNIT_NULLABLE
			Func<RaisedEvent<T>?> handler,
#else
			Func<RaisedEvent<T>> handler,
#endif
			Action attach,
			Action detach,
			Action testCode)
		{
			var raisedEvent = RaisesInternal(handler, attach, detach, testCode);

			if (raisedEvent == null)
				throw RaisesException.ForNoEvent(typeof(T));

			if (raisedEvent.Arguments != null && !raisedEvent.Arguments.GetType().Equals(typeof(T)))
				throw RaisesException.ForIncorrectType(typeof(T), raisedEvent.Arguments.GetType());

			return raisedEvent;
		}

		/// <summary>
		/// Verifies that an event is raised.
		/// </summary>
		/// <param name="attach">Code to attach the event handler</param>
		/// <param name="detach">Code to detach the event handler</param>
		/// <param name="testCode">A delegate to the code to be tested</param>
		/// <returns>The event sender and arguments wrapped in an object</returns>
		/// <exception cref="RaisesException">Thrown when the expected event was not raised.</exception>
		public static RaisedEvent<EventArgs> RaisesAny(
			Action<EventHandler> attach,
			Action<EventHandler> detach,
			Action testCode)
		{
			var raisedEvent = RaisesInternal(attach, detach, testCode);

			if (raisedEvent == null)
				throw RaisesAnyException.ForNoEvent(typeof(EventArgs));

			return raisedEvent;
		}

		/// <summary>
		/// Verifies that an event with the exact or a derived event args is raised.
		/// </summary>
		/// <typeparam name="T">The type of the event arguments to expect</typeparam>
		/// <param name="attach">Code to attach the event handler</param>
		/// <param name="detach">Code to detach the event handler</param>
		/// <param name="testCode">A delegate to the code to be tested</param>
		/// <returns>The event sender and arguments wrapped in an object</returns>
		/// <exception cref="RaisesException">Thrown when the expected event was not raised.</exception>
		public static RaisedEvent<T> RaisesAny<T>(
			Action<Action<T>> attach,
			Action<Action<T>> detach,
			Action testCode)
		{
			var raisedEvent = RaisesInternal(attach, detach, testCode);

			if (raisedEvent == null)
				throw RaisesAnyException.ForNoEvent(typeof(T));

			return raisedEvent;
		}

		/// <summary>
		/// Verifies that an event with the exact or a derived event args is raised.
		/// </summary>
		/// <typeparam name="T">The type of the event arguments to expect</typeparam>
		/// <param name="attach">Code to attach the event handler</param>
		/// <param name="detach">Code to detach the event handler</param>
		/// <param name="testCode">A delegate to the code to be tested</param>
		/// <returns>The event sender and arguments wrapped in an object</returns>
		/// <exception cref="RaisesException">Thrown when the expected event was not raised.</exception>
		public static RaisedEvent<T> RaisesAny<T>(
			Action<EventHandler<T>> attach,
			Action<EventHandler<T>> detach,
			Action testCode)
		{
			var raisedEvent = RaisesInternal(attach, detach, testCode);

			if (raisedEvent == null)
				throw RaisesAnyException.ForNoEvent(typeof(T));

			return raisedEvent;
		}

		/// <summary>
		/// Verifies that an event is raised.
		/// </summary>
		/// <param name="attach">Code to attach the event handler</param>
		/// <param name="detach">Code to detach the event handler</param>
		/// <param name="testCode">A delegate to the code to be tested</param>
		/// <returns>The event sender and arguments wrapped in an object</returns>
		/// <exception cref="RaisesException">Thrown when the expected event was not raised.</exception>
		public static async Task<RaisedEvent<EventArgs>> RaisesAnyAsync(
			Action<EventHandler> attach,
			Action<EventHandler> detach,
			Func<Task> testCode)
		{
			var raisedEvent = await RaisesAsyncInternal(attach, detach, testCode);

			if (raisedEvent == null)
				throw RaisesAnyException.ForNoEvent(typeof(EventArgs));

			return raisedEvent;
		}

		/// <summary>
		/// Verifies that an event with the exact or a derived event args is raised.
		/// </summary>
		/// <typeparam name="T">The type of the event arguments to expect</typeparam>
		/// <param name="attach">Code to attach the event handler</param>
		/// <param name="detach">Code to detach the event handler</param>
		/// <param name="testCode">A delegate to the code to be tested</param>
		/// <returns>The event sender and arguments wrapped in an object</returns>
		/// <exception cref="RaisesException">Thrown when the expected event was not raised.</exception>
		public static async Task<RaisedEvent<T>> RaisesAnyAsync<T>(
			Action<Action<T>> attach,
			Action<Action<T>> detach,
			Func<Task> testCode)
		{
			var raisedEvent = await RaisesAsyncInternal(attach, detach, testCode);

			if (raisedEvent == null)
				throw RaisesAnyException.ForNoEvent(typeof(T));

			return raisedEvent;
		}

		/// <summary>
		/// Verifies that an event with the exact or a derived event args is raised.
		/// </summary>
		/// <typeparam name="T">The type of the event arguments to expect</typeparam>
		/// <param name="attach">Code to attach the event handler</param>
		/// <param name="detach">Code to detach the event handler</param>
		/// <param name="testCode">A delegate to the code to be tested</param>
		/// <returns>The event sender and arguments wrapped in an object</returns>
		/// <exception cref="RaisesException">Thrown when the expected event was not raised.</exception>
		public static async Task<RaisedEvent<T>> RaisesAnyAsync<T>(
			Action<EventHandler<T>> attach,
			Action<EventHandler<T>> detach,
			Func<Task> testCode)
		{
			var raisedEvent = await RaisesAsyncInternal(attach, detach, testCode);

			if (raisedEvent == null)
				throw RaisesAnyException.ForNoEvent(typeof(T));

			return raisedEvent;
		}

		/// <summary>
		/// Verifies that an event is raised.
		/// </summary>
		/// <param name="attach">Code to attach the event handler</param>
		/// <param name="detach">Code to detach the event handler</param>
		/// <param name="testCode">A delegate to the code to be tested</param>
		/// <returns>The event sender and arguments wrapped in an object</returns>
		/// <exception cref="RaisesException">Thrown when the expected event was not raised.</exception>
		public static async Task RaisesAsync(
			Action<Action> attach,
			Action<Action> detach,
			Func<Task> testCode)
		{
			if (!await RaisesAsyncInternal(attach, detach, testCode))
				throw RaisesException.ForNoEvent();
		}

		/// <summary>
		/// Verifies that an event with the exact event args (and not a derived type) is raised.
		/// </summary>
		/// <typeparam name="T">The type of the event arguments to expect</typeparam>
		/// <param name="attach">Code to attach the event handler</param>
		/// <param name="detach">Code to detach the event handler</param>
		/// <param name="testCode">A delegate to the code to be tested</param>
		/// <returns>The event sender and arguments wrapped in an object</returns>
		/// <exception cref="RaisesException">Thrown when the expected event was not raised.</exception>
		public static async Task<RaisedEvent<T>> RaisesAsync<T>(
			Action<Action<T>> attach,
			Action<Action<T>> detach,
			Func<Task> testCode)
		{
			var raisedEvent = await RaisesAsyncInternal(attach, detach, testCode);

			if (raisedEvent == null)
				throw RaisesException.ForNoEvent(typeof(T));

			if (raisedEvent.Arguments != null && !raisedEvent.Arguments.GetType().Equals(typeof(T)))
				throw RaisesException.ForIncorrectType(typeof(T), raisedEvent.Arguments.GetType());

			return raisedEvent;
		}

		/// <summary>
		/// Verifies that an event with the exact event args (and not a derived type) is raised.
		/// </summary>
		/// <typeparam name="T">The type of the event arguments to expect</typeparam>
		/// <param name="attach">Code to attach the event handler</param>
		/// <param name="detach">Code to detach the event handler</param>
		/// <param name="testCode">A delegate to the code to be tested</param>
		/// <returns>The event sender and arguments wrapped in an object</returns>
		/// <exception cref="RaisesException">Thrown when the expected event was not raised.</exception>
		public static async Task<RaisedEvent<T>> RaisesAsync<T>(
			Action<EventHandler<T>> attach,
			Action<EventHandler<T>> detach,
			Func<Task> testCode)
		{
			var raisedEvent = await RaisesAsyncInternal(attach, detach, testCode);

			if (raisedEvent == null)
				throw RaisesException.ForNoEvent(typeof(T));

			if (raisedEvent.Arguments != null && !raisedEvent.Arguments.GetType().Equals(typeof(T)))
				throw RaisesException.ForIncorrectType(typeof(T), raisedEvent.Arguments.GetType());

			return raisedEvent;
		}

		// Helpers

		static bool RaisesInternal(
			Action<Action> attach,
			Action<Action> detach,
			Action testCode)
		{
			GuardArgumentNotNull(nameof(attach), attach);
			GuardArgumentNotNull(nameof(detach), detach);
			GuardArgumentNotNull(nameof(testCode), testCode);

			var result = false;
			Action handler = () => result = true;

			attach(handler);
			testCode();
			detach(handler);
			return result;
		}

#if XUNIT_NULLABLE
		static RaisedEvent<EventArgs>? RaisesInternal(
#else
		static RaisedEvent<EventArgs> RaisesInternal(
#endif
			Action<EventHandler> attach,
			Action<EventHandler> detach,
			Action testCode)
		{
			GuardArgumentNotNull(nameof(attach), attach);
			GuardArgumentNotNull(nameof(detach), detach);
			GuardArgumentNotNull(nameof(testCode), testCode);

			var raisedEvent = default(RaisedEvent<EventArgs>);
#if XUNIT_NULLABLE
			void handler(object? s, EventArgs args) => raisedEvent = new RaisedEvent<EventArgs>(s, args);
#else
			EventHandler handler = (object s, EventArgs args) => raisedEvent = new RaisedEvent<EventArgs>(s, args);
#endif
			attach(handler);
			testCode();
			detach(handler);
			return raisedEvent;
		}

#if XUNIT_NULLABLE
		static RaisedEvent<T>? RaisesInternal<T>(
#else
		static RaisedEvent<T> RaisesInternal<T>(
#endif
			Action<Action<T>> attach,
			Action<Action<T>> detach,
			Action testCode)
		{
			var raisedEvent = default(RaisedEvent<T>);
			Action<T> handler = (T args) => raisedEvent = new RaisedEvent<T>(args);

			return RaisesInternal(
				() => raisedEvent,
				() => attach(handler),
				() => detach(handler),
				testCode);
		}

#if XUNIT_NULLABLE
		static RaisedEvent<T>? RaisesInternal<T>(
#else
		static RaisedEvent<T> RaisesInternal<T>(
#endif
			Action<EventHandler<T>> attach,
			Action<EventHandler<T>> detach,
			Action testCode)
		{
			var raisedEvent = default(RaisedEvent<T>);
#if XUNIT_NULLABLE
			void handler(object? s, T args) => raisedEvent = new RaisedEvent<T>(s, args);
#else
			EventHandler<T> handler = (object s, T args) => raisedEvent = new RaisedEvent<T>(s, args);
#endif
			return RaisesInternal(
				() => raisedEvent,
				() => attach(handler),
				() => detach(handler),
				testCode);
		}

#if XUNIT_NULLABLE
		static RaisedEvent<T>? RaisesInternal<T>(
			Func<RaisedEvent<T>?> handler,
#else
		static RaisedEvent<T> RaisesInternal<T>(
			Func<RaisedEvent<T>> handler,
#endif
			Action attach,
			Action detach,
			Action testCode)
		{
			GuardArgumentNotNull(nameof(handler), handler);
			GuardArgumentNotNull(nameof(attach), attach);
			GuardArgumentNotNull(nameof(detach), detach);
			GuardArgumentNotNull(nameof(testCode), testCode);

			attach();
			testCode();
			detach();
			return handler();
		}

		static async Task<bool> RaisesAsyncInternal(
			Action<Action> attach,
			Action<Action> detach,
			Func<Task> testCode)
		{
			GuardArgumentNotNull(nameof(attach), attach);
			GuardArgumentNotNull(nameof(detach), detach);
			GuardArgumentNotNull(nameof(testCode), testCode);

			var result = false;
			Action handler = () => result = true;

			attach(handler);
			await testCode();
			detach(handler);
			return result;
		}

#if XUNIT_NULLABLE
		static async Task<RaisedEvent<EventArgs>?> RaisesAsyncInternal(
#else
		static async Task<RaisedEvent<EventArgs>> RaisesAsyncInternal(
#endif
			Action<EventHandler> attach,
			Action<EventHandler> detach,
			Func<Task> testCode)
		{
			GuardArgumentNotNull(nameof(attach), attach);
			GuardArgumentNotNull(nameof(detach), detach);
			GuardArgumentNotNull(nameof(testCode), testCode);

			var raisedEvent = default(RaisedEvent<EventArgs>);
#if XUNIT_NULLABLE
			void handler(object? s, EventArgs args) => raisedEvent = new RaisedEvent<EventArgs>(s, args);
#else
			EventHandler handler = (object s, EventArgs args) => raisedEvent = new RaisedEvent<EventArgs>(s, args);
#endif
			attach(handler);
			await testCode();
			detach(handler);
			return raisedEvent;
		}

#if XUNIT_NULLABLE
		static async Task<RaisedEvent<T>?> RaisesAsyncInternal<T>(
#else
		static async Task<RaisedEvent<T>> RaisesAsyncInternal<T>(
#endif
			Action<Action<T>> attach,
			Action<Action<T>> detach,
			Func<Task> testCode)
		{
			GuardArgumentNotNull(nameof(attach), attach);
			GuardArgumentNotNull(nameof(detach), detach);
			GuardArgumentNotNull(nameof(testCode), testCode);

			var raisedEvent = default(RaisedEvent<T>);
			Action<T> handler = (T args) => raisedEvent = new RaisedEvent<T>(args);

			attach(handler);
			await testCode();
			detach(handler);
			return raisedEvent;
		}

#if XUNIT_NULLABLE
		static async Task<RaisedEvent<T>?> RaisesAsyncInternal<T>(
#else
		static async Task<RaisedEvent<T>> RaisesAsyncInternal<T>(
#endif
			Action<EventHandler<T>> attach,
			Action<EventHandler<T>> detach,
			Func<Task> testCode)
		{
			GuardArgumentNotNull(nameof(attach), attach);
			GuardArgumentNotNull(nameof(detach), detach);
			GuardArgumentNotNull(nameof(testCode), testCode);

			var raisedEvent = default(RaisedEvent<T>);
#if XUNIT_NULLABLE
			void handler(object? s, T args) => raisedEvent = new RaisedEvent<T>(s, args);
#else
			EventHandler<T> handler = (object s, T args) => raisedEvent = new RaisedEvent<T>(s, args);
#endif
			attach(handler);
			await testCode();
			detach(handler);
			return raisedEvent;
		}

		/// <summary>
		/// Represents a raised event after the fact.
		/// </summary>
		/// <typeparam name="T">The type of the event arguments.</typeparam>
		public class RaisedEvent<T>
		{
			/// <summary>
			/// The sender of the event. When the event is recorded via <see cref="Action{T}"/> rather
			/// than <see cref="EventHandler{TEventArgs}"/>, this value will always be <c>null</c>,
			/// since there is no sender value when using actions.
			/// </summary>
#if XUNIT_NULLABLE
			public object? Sender { get; }
#else
			public object Sender { get; }
#endif

			/// <summary>
			/// The event arguments.
			/// </summary>
			public T Arguments { get; }

			/// <summary>
			/// Creates a new instance of the <see cref="RaisedEvent{T}" /> class.
			/// </summary>
			/// <param name="args">The event arguments</param>
			public RaisedEvent(T args) :
				this(null, args)
			{ }

			/// <summary>
			/// Creates a new instance of the <see cref="RaisedEvent{T}" /> class.
			/// </summary>
			/// <param name="sender">The sender of the event.</param>
			/// <param name="args">The event arguments</param>
			public RaisedEvent(
#if XUNIT_NULLABLE
				object? sender,
#else
				object sender,
#endif
				T args)
			{
				Sender = sender;
				Arguments = args;
			}
		}
	}
}
