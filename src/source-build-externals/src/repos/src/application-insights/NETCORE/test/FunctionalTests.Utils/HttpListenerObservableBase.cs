namespace FunctionalTests.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Reactive.Linq;
    using System.Reactive.Threading.Tasks;
    using System.Threading.Tasks;
    using Xunit.Abstractions;

    public abstract class HttpListenerObservableBase<T> : IObservable<T>, IDisposable
    {
        private readonly HttpListener listener;
        private IObservable<T> stream;
        private ITestOutputHelper output;

        public HttpListenerObservableBase(string url, ITestOutputHelper output)
        {
            this.output = output;

            this.output.WriteLine(string.Format("{0}: HttpListenerObservableBase Constructor. Url is: {1}", DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"), url));
            this.listener = new HttpListener();            
            this.listener.Prefixes.Add(url);
        }

        public void Start()
        {
            OnStart();
            if (this.stream != null)
            {
                this.output.WriteLine(string.Format("{0}: HttpListenerObservableBase Start. Stream is not null. Stopping", DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt")));
                this.Stop();
                this.output.WriteLine(string.Format("{0}: HttpListenerObservableBase Start. Stream is not null. Stop completed", DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt")));
            }

            if (!this.listener.IsListening)
            {
                this.output.WriteLine(string.Format("{0}: HttpListenerObservableBase Start. Listener not already listening. Starting to listen", DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt")));
                this.listener.Start();
                this.output.WriteLine(string.Format("{0}: HttpListenerObservableBase Start. Listener not already listening. Started listening", DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt")));
            }

            this.stream = this.CreateStream();
        }

        protected virtual void OnStart()
        { }

        public void Stop()
        {
            this.Dispose();
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (this.stream == null)
            {
                throw new InvalidOperationException("Call HttpListenerObservable.Start before subscribing to the stream");
            }

            return this.stream
                .Subscribe(observer);
        }

        public void Dispose()
        {
            if (listener != null && listener.IsListening)
            {
                // listener.Stop() is not required as Close does shutdown the listener.
                // Stop() followed by Close() throws error in non-windows.
                listener.Close();
                this.stream = null;
            }
        }

        private IObservable<T> CreateStream()
        {
            return Observable
                .Create<T>
                (obs =>
                    Task.Factory.FromAsync(
                        (a, c) => this.listener.BeginGetContext(a, c),
                        ar => this.listener.EndGetContext(ar),
                        null)
                        .ToObservable()
                        .SelectMany(this.CreateNewItemsFromContext)
                        .Subscribe(obs)
                )
              .Repeat()
              .Publish()
              .RefCount();
        }

        protected abstract IEnumerable<T> CreateNewItemsFromContext(HttpListenerContext context);
    }
}
