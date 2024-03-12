namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    /// <summary>
    /// Base implementation of DiagnosticSource listener. 
    /// Takes care of managing subscriptions to multiple sources and their events.
    /// </summary>
    /// <typeparamref name="TContext">The type of processing context for given diagnostic source.</typeparamref>
    internal abstract class DiagnosticSourceListenerBase<TContext> : IObserver<DiagnosticListener>, IDisposable
    {
        protected static readonly ConcurrentDictionary<DiagnosticListener, ActiveSubsciptionManager> SubscriptionManagers =
            new ConcurrentDictionary<DiagnosticListener, ActiveSubsciptionManager>();
    
        protected readonly TelemetryClient Client;
        protected readonly TelemetryConfiguration Configuration;

        private readonly ConcurrentQueue<IDisposable> individualSubscriptions = new ConcurrentQueue<IDisposable>();
        private readonly ConcurrentQueue<IndividualDiagnosticSourceListener> individualListeners = new ConcurrentQueue<IndividualDiagnosticSourceListener>();

        private IDisposable listenerSubscription;

        /// <summary>
        /// Creates DiagnosticSourceListenerBase. To finish the initialization and subscribe to all enabled sources,
        /// call <see cref="Subscribe"/>.
        /// </summary>
        /// <param name="configuration"><see cref="TelemetryConfiguration"/> instance.
        /// The listener tracks dependency calls and uses configuration to construct <see cref="TelemetryClient"/>.</param>
        protected DiagnosticSourceListenerBase(TelemetryConfiguration configuration)
        {
            this.Configuration = configuration;
            this.Client = new TelemetryClient(configuration);
        }

        /// <summary>
        /// Subscribes the listener to all enabled sources. This method must be called
        /// to enable dependency calls collection.
        /// </summary>
        public void Subscribe()
        {
            if (this.listenerSubscription != null)
            {
                return;
            }

            try
            {
                this.listenerSubscription = DiagnosticListener.AllListeners.Subscribe(this);
            }
            catch (Exception ex)
            {
                DependencyCollectorEventSource.Log.DiagnosticSourceListenerFailedToSubscribe(this.GetType().Name, ex.ToInvariantString());
            }
        }

        /// <summary>
        /// This method gets called once for each existing DiagnosticListener when this
        /// DiagnosticListener is added to the list of DiagnosticListeners
        /// (<see cref="System.Diagnostics.DiagnosticListener.AllListeners"/>). This method will
        /// also be called for each subsequent DiagnosticListener that is added to the list of
        /// DiagnosticListeners.
        /// <seealso cref="IObserver{T}.OnNext(T)"/>
        /// </summary>
        /// <param name="value">The DiagnosticListener that exists when this listener was added to
        /// the list, or a DiagnosticListener that got added after this listener was added.</param>
        public virtual void OnNext(DiagnosticListener value)
        {
            if (value == null || !this.IsSourceEnabled(value))
            {
                return;
            }

            var eventHandler = this.GetEventHandler(value.Name);
            var manager = SubscriptionManagers.GetOrAdd(value, k => new ActiveSubsciptionManager());

            var individualListener = new IndividualDiagnosticSourceListener(
                value, 
                eventHandler, 
                this, 
                this.GetListenerContext(value),
                manager);

            manager.Attach(individualListener);

            IDisposable subscription = value.Subscribe(
                individualListener,
                (evnt, input1, input2) => this.IsActivityEnabled(evnt, individualListener.Context) && eventHandler.IsEventEnabled(evnt, input1, input2));

            this.individualSubscriptions.Enqueue(subscription);
            this.individualListeners.Enqueue(individualListener);
        }

        /// <summary>
        /// Notifies the observer that the provider has finished sending push-based notifications.
        /// <seealso cref="IObserver{T}.OnCompleted()"/>
        /// </summary>
        public void OnCompleted()
        {
        }

        /// <summary>
        /// Notifies the observer that the provider has experienced an error condition.
        /// <seealso cref="IObserver{T}.OnError(Exception)"/>
        /// </summary>
        /// <param name="error">An object that provides additional information about the error.</param>
        public void OnError(Exception error)
        {
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Checks if the diagnostic source is enabled by this listener.
        /// </summary>
        /// <param name="diagnosticListener">The diagnostic source.</param>
        /// <returns><code>true</code> if Diagnostic Source is enabled.</returns>
        internal abstract bool IsSourceEnabled(DiagnosticListener diagnosticListener);

        /// <summary>
        /// Checks if the Activity is enabled by configuration based on the event name.
        /// </summary>
        /// <param name="evnt">The event name.</param>
        /// <param name="context">The diagnostic source-specific context (<see cref="GetListenerContext(DiagnosticListener)"/>).</param>
        /// <returns><code>true</code> if Diagnostic Source Activity and corresponding event are enabled.</returns>
        internal abstract bool IsActivityEnabled(string evnt, TContext context);

        /// <summary>
        /// Gets diagnostic source-specific context for processing events from that source.
        /// </summary>
        /// <param name="diagnosticListener">The diagnostic source.</param>
        /// <returns>The context.</returns>
        protected virtual TContext GetListenerContext(DiagnosticListener diagnosticListener)
        {
            return default(TContext);
        }

        /// <summary>
        /// Gets event handler for specific diagnostic source.
        /// </summary>
        /// <param name="diagnosticListenerName">The diagnostic source name.</param>
        /// <returns>Event handler.</returns>
        protected abstract IDiagnosticEventHandler GetEventHandler(string diagnosticListenerName);

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                while (this.individualListeners.TryDequeue(out var individualListener))
                {
                    individualListener.Dispose();
                }

                while (this.individualSubscriptions.TryDequeue(out var individualSubscription))
                {
                    individualSubscription.Dispose();
                }

                this.listenerSubscription?.Dispose();
            }
        }

        /// <summary>
        /// Event listener for a single Diagnostic Source.
        /// </summary>
        internal sealed class IndividualDiagnosticSourceListener : IObserver<KeyValuePair<string, object>>, IDisposable
        {
            internal readonly TContext Context;
            private readonly DiagnosticListener diagnosticListener;
            private readonly IDiagnosticEventHandler eventHandler;
            private readonly DiagnosticSourceListenerBase<TContext> telemetryDiagnosticSourceListener;
            private readonly ActiveSubsciptionManager subscriptionManager;

            internal IndividualDiagnosticSourceListener(
                DiagnosticListener diagnosticListener,
                IDiagnosticEventHandler eventHandler,
                DiagnosticSourceListenerBase<TContext> telemetryDiagnosticSourceListener,
                TContext context,
                ActiveSubsciptionManager subscriptionManager)
            {
                this.diagnosticListener = diagnosticListener;
                this.eventHandler = eventHandler;
                this.Context = context;
                this.telemetryDiagnosticSourceListener = telemetryDiagnosticSourceListener;
                this.subscriptionManager = subscriptionManager;
                this.subscriptionManager.Attach(this);
            }

            public void OnNext(KeyValuePair<string, object> evnt)
            {
                // It's possible to host multiple apps (ASP.NET Core or generic hosts) in the same process
                // Each of this apps has it's own DependencyTrackingModule and corresponding listener for specific source.
                // We should ignore events for all of them except one
                if (!this.subscriptionManager.IsActive(this))
                {
                    DependencyCollectorEventSource.Log.NotActiveListenerNoTracking(evnt.Key, Activity.Current?.Id);
                    return;
                }

                // while we provide IsEnabled callback during subscription, it does not gurantee events will not be fired
                // In case of multiple subscribers, it's enough for one to reply true to IsEnabled.
                // I.e. check for if activity is not disabled and particular handler wants to receive the event.
                if (this.telemetryDiagnosticSourceListener.IsActivityEnabled(evnt.Key, this.Context) && this.eventHandler.IsEventEnabled(evnt.Key, null, null))
                {
                    Activity currentActivity = Activity.Current;
                    if (currentActivity == null)
                    {
                        DependencyCollectorEventSource.Log.CurrentActivityIsNull(evnt.Key);
                        return;
                    }

                    DependencyCollectorEventSource.Log.TelemetryDiagnosticSourceListenerEvent(evnt.Key, currentActivity.Id);

                    try
                    {
                        this.eventHandler.OnEvent(evnt, this.diagnosticListener);
                    }
                    catch (Exception ex)
                    {
                        DependencyCollectorEventSource.Log.TelemetryDiagnosticSourceCallbackException(evnt.Key, ex.ToInvariantString());
                    }
                }
            }

            /// <summary>
            /// Notifies the observer that the provider has finished sending push-based notifications.
            /// <seealso cref="IObserver{T}.OnCompleted()"/>
            /// </summary>
            public void OnCompleted()
            {
            }

            /// <summary>
            /// Notifies the observer that the provider has experienced an error condition.
            /// <seealso cref="IObserver{T}.OnError(Exception)"/>
            /// </summary>
            /// <param name="error">An object that provides additional information about the error.</param>
            public void OnError(Exception error)
            {
            }

            public void Dispose()
            {
                this.subscriptionManager?.Detach(this);
            }
        }
    }
}