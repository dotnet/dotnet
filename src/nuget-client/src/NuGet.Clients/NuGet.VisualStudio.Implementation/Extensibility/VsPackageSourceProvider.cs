// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using NuGet.Protocol.Core.Types;
using NuGet.VisualStudio.Etw;
using NuGet.VisualStudio.Telemetry;

namespace NuGet.VisualStudio.Implementation.Extensibility
{
    [Export(typeof(IVsPackageSourceProvider))]
    public class VsPackageSourceProvider : IVsPackageSourceProvider
    {
        private readonly Configuration.IPackageSourceProvider _packageSourceProvider;
        private readonly INuGetTelemetryProvider _telemetryProvider;

        [ImportingConstructor]
        public VsPackageSourceProvider(ISourceRepositoryProvider sourceRepositoryProvider, INuGetTelemetryProvider telemetryProvider)
        {
            if (sourceRepositoryProvider == null)
            {
                throw new ArgumentNullException(nameof(sourceRepositoryProvider));
            }

            _packageSourceProvider = sourceRepositoryProvider.PackageSourceProvider;
            _telemetryProvider = telemetryProvider;

            _packageSourceProvider.PackageSourcesChanged += PackageSourcesChanged;
        }

        public IEnumerable<KeyValuePair<string, string>> GetSources(bool includeUnOfficial, bool includeDisabled)
        {
            const string eventName = nameof(IVsPackageSourceProvider) + "." + nameof(GetSources);
            using var _ = NuGetETW.ExtensibilityEventSource.StartStopEvent(eventName,
                new
                {
                    IncludeUnOfficial = includeUnOfficial,
                    IncludeDisabled = includeDisabled
                });

            try
            {
                var sources = new List<KeyValuePair<string, string>>();

                foreach (var source in _packageSourceProvider.LoadPackageSources())
                {
                    if ((IsOfficial(source) || includeUnOfficial)
                        && (source.IsEnabled || includeDisabled))
                    {
                        // Name -> Source Uri
                        var pair = new KeyValuePair<string, string>(source.Name, source.Source);
                        sources.Add(pair);
                    }
                }

                return sources;
            }
            catch (Exception ex) when (!IsExpected(ex))
            {
                _telemetryProvider.PostFault(ex, typeof(VsPackageSourceProvider).FullName);
                throw new InvalidOperationException(ex.Message, ex);
            }
        }

        private event EventHandler _sourcesChanged;
        const string SourcesChangedEventName = nameof(IVsPackageSourceProvider) + "." + nameof(SourcesChanged);
        public event EventHandler SourcesChanged
        {
            add
            {
                NuGetETW.ExtensibilityEventSource.Write(SourcesChangedEventName, NuGetETW.AddEventOptions);
                _sourcesChanged += value;
            }
            remove
            {
                NuGetETW.ExtensibilityEventSource.Write(SourcesChangedEventName, NuGetETW.RemoveEventOptions);
                _sourcesChanged -= value;
            }
        }

        private void PackageSourcesChanged(object sender, EventArgs e)
        {
            if (_sourcesChanged != null)
            {
                // No information is given in the event args, callers must re-request GetSources
                var eventArgs = new EventArgs();
                var delegates = _sourcesChanged.GetInvocationList();
                for (int i = 0; i < delegates.Length; i++)
                {
                    var handler = (EventHandler)delegates[i];
                    handler(this, eventArgs);
                }
            }
        }

        private static bool IsOfficial(Configuration.PackageSource source)
        {
            bool official = source.IsOfficial;

            // override the official flag if the domain is nuget.org
            if (source.Source.StartsWith("http://www.nuget.org/", StringComparison.OrdinalIgnoreCase)
                || source.Source.StartsWith("https://www.nuget.org/", StringComparison.OrdinalIgnoreCase)
                || source.Source.StartsWith("http://api.nuget.org/", StringComparison.OrdinalIgnoreCase)
                || source.Source.StartsWith("https://api.nuget.org/", StringComparison.OrdinalIgnoreCase))
            {
                official = true;
            }

            return official;
        }

        private static bool IsExpected(Exception ex)
        {
            return ex is ArgumentException
                || ex is ArgumentNullException
                || ex is InvalidDataException
                || ex is InvalidOperationException;
        }
    }
}
