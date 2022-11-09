using System;

namespace Microsoft.DotNet.SwaggerGenerator.Modeler
{
    public class Disposable : IDisposable
    {
        private readonly Action _onDispose;

        private Disposable(Action onDispose)
        {
            _onDispose = onDispose;
        }

        public void Dispose()
        {
            _onDispose?.Invoke();
        }

        public static IDisposable Create(Action onDispose)
        {
            return new Disposable(onDispose);
        }
    }
}
