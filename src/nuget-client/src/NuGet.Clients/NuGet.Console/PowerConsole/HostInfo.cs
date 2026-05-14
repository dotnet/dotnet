// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;

namespace NuGetConsole.Implementation.PowerConsole
{
    /// <summary>
    /// Represents a host with extra info.
    /// </summary>
    internal class HostInfo : ObjectWithFactory<PowerConsoleWindow>, IDisposable
    {
        private readonly Lazy<IHostProvider, IHostMetadata> _hostProvider;

        public HostInfo(PowerConsoleWindow factory, Lazy<IHostProvider, IHostMetadata> hostProvider)
            : base(factory)
        {
            UtilityMethods.ThrowIfArgumentNull(hostProvider);
            this._hostProvider = hostProvider;
        }

        /// <summary>
        /// Get the HostName attribute value of this host.
        /// </summary>
        public string HostName
        {
            get { return _hostProvider.Metadata.HostName; }
        }

        private IWpfConsole _wpfConsole;

        /// <summary>
        /// Get/create the console for this host. If not already created, this
        /// actually creates the (console, host) pair.
        /// Note: Creating the console is handled by this package and mostly will
        /// succeed. However, creating the host could be from other packages and
        /// fail. In that case, this console is already created and can be used
        /// subsequently in limited ways, such as displaying an error message.
        /// </summary>
        public IWpfConsole WpfConsole
        {
            get
            {
                if (_wpfConsole == null)
                {
                    _wpfConsole = Factory.WpfConsoleService.CreateConsole(
                        Factory.ServiceProvider, PowerConsoleWindow.ContentType, HostName);
                    _wpfConsole.Host = _hostProvider.Value.CreateHost(@async: true);
                }
                return _wpfConsole;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                var disposable = _wpfConsole as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }

        void IDisposable.Dispose()
        {
            try
            {
                Dispose(true);
            }
            finally
            {
                GC.SuppressFinalize(this);
            }
        }

        ~HostInfo()
        {
            Dispose(false);
        }
    }
}
