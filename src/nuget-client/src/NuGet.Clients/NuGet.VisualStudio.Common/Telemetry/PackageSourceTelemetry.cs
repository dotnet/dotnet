// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Protocol.Events;

namespace NuGet.VisualStudio.Telemetry
{
    public sealed class PackageSourceTelemetry : IDisposable
    {
        private readonly IReadOnlyDictionary<string, Data> _data;
        private readonly IDictionary<string, SourceRepository> _sources;
        private readonly Guid _parentId;
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, string>> _resourceStringTable;
        private readonly string _actionName;
        private readonly PackageSourceMapping _packageSourceMappingConfiguration;

        internal const string EventName = "PackageSourceDiagnostics";

        public enum TelemetryAction
        {
            Unknown = 0,
            Restore,
            Search
        }

        public PackageSourceTelemetry(IEnumerable<SourceRepository> sources, Guid parentId, TelemetryAction action, PackageSourceMapping packageSourceMappingConfiguration)
            : this(sources, parentId, action)
        {
            _packageSourceMappingConfiguration = packageSourceMappingConfiguration;
        }

        public PackageSourceTelemetry(IEnumerable<SourceRepository> sources, Guid parentId, TelemetryAction action)
        {
            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            // Multiple sources can use the same feed url. We can't know which one protocol events come from, so choose any.
            _sources = new Dictionary<string, SourceRepository>();
            foreach (var source in sources)
            {
                _sources[source.PackageSource.Source] = source;
            }

            var data = new Dictionary<string, Data>(_sources.Count);
            foreach (var source in _sources.Keys)
            {
                data[source] = new Data();
            }
            _data = data;

            _resourceStringTable = new ConcurrentDictionary<string, ConcurrentDictionary<string, string>>();
            ProtocolDiagnostics.HttpEvent += ProtocolDiagnostics_HttpEvent;
            ProtocolDiagnostics.ResourceEvent += ProtocolDiagnostics_ResourceEvent;
            ProtocolDiagnostics.NupkgCopiedEvent += ProtocolDiagnostics_NupkgCopiedEvent;
            _parentId = parentId;
            _actionName = GetActionName(action);
        }

        private static string GetActionName(TelemetryAction action)
        {
            switch (action)
            {
                case TelemetryAction.Restore:
                case TelemetryAction.Search:
                    return action.ToString();

                default:
                    throw new ArgumentException("Unknown value of " + nameof(TelemetryAction), nameof(action));
            }
        }

        private void ProtocolDiagnostics_ResourceEvent(ProtocolDiagnosticResourceEvent pdEvent)
        {
            AddResourceData(pdEvent, _data, _resourceStringTable);
        }

        internal static void AddResourceData(
            ProtocolDiagnosticResourceEvent pdEvent,
            IReadOnlyDictionary<string, Data> allData,
            ConcurrentDictionary<string, ConcurrentDictionary<string, string>> resourceStringTable)
        {
            if (!allData.TryGetValue(pdEvent.Source, out Data data))
            {
                return;
            }

            var resourceMethodNameTable = resourceStringTable.GetOrAdd(pdEvent.ResourceType, t => new ConcurrentDictionary<string, string>());
            var resourceTypeAndMethod = resourceMethodNameTable.GetOrAdd(pdEvent.Method, m => pdEvent.ResourceType + "." + m);

            lock (data._lock)
            {
                if (data.Resources.TryGetValue(resourceTypeAndMethod, out var t))
                {
                    data.Resources[resourceTypeAndMethod] = (t.count + 1, t.duration + pdEvent.Duration);
                }
                else
                {
                    data.Resources[resourceTypeAndMethod] = (1, pdEvent.Duration);
                }
            }
        }

        private void ProtocolDiagnostics_HttpEvent(ProtocolDiagnosticHttpEvent pdEvent)
        {
            AddHttpData(pdEvent, _data);
        }

        internal static void AddHttpData(ProtocolDiagnosticHttpEvent pdEvent, IReadOnlyDictionary<string, Data> allData)
        {
            if (!allData.TryGetValue(pdEvent.Source, out Data data))
            {
                return;
            }

            lock (data._lock)
            {
                var httpData = data.Http;
                httpData.Requests++;
                httpData.TotalDuration += pdEvent.EventDuration;
                // If any one event header duration is null, we want the HttpData value to be null,
                // since the request count would otherwise be incorrect. C# nullable does this automatically for us.
                httpData.HeaderDuration += pdEvent.HeaderDuration;

                if (pdEvent.IsSuccess)
                {
                    httpData.Successful++;
                }

                if (pdEvent.IsRetry)
                {
                    httpData.Retries++;
                }

                if (pdEvent.IsCancelled)
                {
                    httpData.Cancelled++;
                }

                if (pdEvent.IsLastAttempt && !pdEvent.IsSuccess && !pdEvent.IsCancelled)
                {
                    httpData.Failed++;
                }

                if (pdEvent.Bytes > 0)
                {
                    httpData.TotalBytes += pdEvent.Bytes;
                }

                if (pdEvent.HttpStatusCode.HasValue)
                {
                    if (!httpData.StatusCodes.TryGetValue(pdEvent.HttpStatusCode.Value, out var count))
                    {
                        count = 0;
                    }
                    httpData.StatusCodes[pdEvent.HttpStatusCode.Value] = count + 1;
                }
            }
        }

        private void ProtocolDiagnostics_NupkgCopiedEvent(ProtocolDiagnosticNupkgCopiedEvent ncEvent)
        {
            AddNupkgCopiedData(ncEvent, _data);
        }

        internal static void AddNupkgCopiedData(ProtocolDiagnosticNupkgCopiedEvent ncEvent, IReadOnlyDictionary<string, Data> allData)
        {
            if (!allData.TryGetValue(ncEvent.Source, out Data data))
            {
                return;
            }

            lock (data._lock)
            {
                data.NupkgCount++;
                data.NupkgSize += ncEvent.FileSize;
            }
        }

        public void Dispose()
        {
            ProtocolDiagnostics.HttpEvent -= ProtocolDiagnostics_HttpEvent;
            ProtocolDiagnostics.ResourceEvent -= ProtocolDiagnostics_ResourceEvent;
            ProtocolDiagnostics.NupkgCopiedEvent -= ProtocolDiagnostics_NupkgCopiedEvent;
        }

        public async Task SendTelemetryAsync()
        {
            var parentId = _parentId.ToString();
            foreach (var kvp in _data)
            {
                Data data = kvp.Value;
                string source = kvp.Key;
                if (!_sources.TryGetValue(kvp.Key, out SourceRepository sourceRepository))
                {
                    // Should not be possible. This is just defensive programming to avoid an exception being thrown in case I'm wrong.
                    sourceRepository = new SourceRepository(new PackageSource(source), Repository.Provider.GetCoreV3());
                }

                var telemetry = await ToTelemetryAsync(data, sourceRepository, parentId, _actionName, _packageSourceMappingConfiguration);

                if (telemetry != null)
                {
                    TelemetryActivity.EmitTelemetryEvent(telemetry);
                }
            }
        }

        internal static async Task<TelemetryEvent> ToTelemetryAsync(Data data, SourceRepository sourceRepository, string parentId, string actionName, PackageSourceMapping packageSourceMappingConfiguration)
        {
            if (data.Resources.Count == 0)
            {
                return null;
            }

            FeedType feedType = await sourceRepository.GetFeedType(CancellationToken.None);

            TelemetryEvent telemetry;
            lock (data._lock)
            {
                bool isPackageSourceMappingEnabled = packageSourceMappingConfiguration?.IsEnabled ?? false;

                telemetry = new TelemetryEvent(EventName,
                    new Dictionary<string, object>()
                    {
                    { PropertyNames.ParentId, parentId },
                    { PropertyNames.Action, actionName },
                    { PropertyNames.PackageSourceMapping.IsMappingEnabled, isPackageSourceMappingEnabled }
                    });

                AddSourceProperties(telemetry, sourceRepository, feedType);
                telemetry[PropertyNames.Duration.Total] = data.Resources.Values.Sum(r => r.duration.TotalMilliseconds);
                telemetry[PropertyNames.Nupkgs.Copied] = data.NupkgCount;
                telemetry[PropertyNames.Nupkgs.Bytes] = data.NupkgSize;
                AddResourceProperties(telemetry, data.Resources);

                if (data.Http.Requests > 0)
                {
                    AddHttpProperties(telemetry, data.Http);
                }
            }

            return telemetry;
        }

        private static void AddSourceProperties(TelemetryEvent telemetry, SourceRepository sourceRepository, FeedType feedType)
        {
            telemetry.AddPiiData(PropertyNames.Source.Url, sourceRepository.PackageSource.Source);

            telemetry[PropertyNames.Source.Type] = feedType;

            var msFeed = GetMsFeed(sourceRepository.PackageSource);
            if (msFeed != null)
            {
                telemetry[PropertyNames.Source.MSFeed] = msFeed;
            }
        }

        private static void AddResourceProperties(TelemetryEvent telemetry, Dictionary<string, (int count, TimeSpan duration)> resources)
        {
            telemetry[PropertyNames.Resources.Calls] = resources.Values.Sum(r => r.count);
            telemetry.ComplexData[PropertyNames.Resources.Details] = ToResourceDetailsTelemetry(resources);
        }

        private static void AddHttpProperties(TelemetryEvent telemetry, HttpData data)
        {
            telemetry[PropertyNames.Http.Requests] = data.Requests;
            telemetry[PropertyNames.Http.Successful] = data.Successful;
            telemetry[PropertyNames.Http.Retries] = data.Retries;
            telemetry[PropertyNames.Http.Cancelled] = data.Cancelled;
            telemetry[PropertyNames.Http.Failed] = data.Failed;
            telemetry[PropertyNames.Http.Bytes] = data.TotalBytes;
            telemetry[PropertyNames.Http.Duration.Total] = data.TotalDuration.TotalMilliseconds;

            if (data.HeaderDuration != null)
            {
                telemetry[PropertyNames.Http.Duration.Header] = data.HeaderDuration.Value.TotalMilliseconds;
            }

            if (data.StatusCodes.Count > 0)
            {
                telemetry.ComplexData[PropertyNames.Http.StatusCodes] = ToStatusCodeTelemetry(data.StatusCodes);
            }
        }

        private static TelemetryEvent ToResourceDetailsTelemetry(Dictionary<string, (int count, TimeSpan duration)> resources)
        {
            var subevent = new TelemetryEvent(eventName: null);

            foreach (var resource in resources)
            {
                var details = new TelemetryEvent(eventName: null);
                details["count"] = resource.Value.count;
                details["duration"] = resource.Value.duration.TotalMilliseconds;

                subevent.ComplexData[resource.Key] = details;
            }

            return subevent;
        }

        private static TelemetryEvent ToStatusCodeTelemetry(Dictionary<int, int> statusCodes)
        {
            var subevent = new TelemetryEvent(eventName: null);

            foreach (var pair in statusCodes)
            {
                subevent[pair.Key.ToString(CultureInfo.CurrentCulture)] = pair.Value;
            }

            return subevent;
        }

        private static string GetMsFeed(PackageSource source)
        {
            if (source.IsHttp)
            {
                if (UriUtility.IsNuGetOrg(source.Source))
                {
                    return "nuget.org";
                }
                else if (TelemetryUtility.IsAzureArtifacts(source))
                {
                    return "Azure DevOps";
                }
                else if (TelemetryUtility.IsGitHub(source))
                {
                    return "GitHub";
                }
            }
            else if (source.IsLocal)
            {
                if (TelemetryUtility.IsVsOfflineFeed(source))
                {
                    return "VS Offline";
                }
            }

            return null;
        }

        public Totals GetTotals()
        {
            return GetTotals(_data);
        }

        internal static Totals GetTotals(IReadOnlyDictionary<string, Data> data)
        {
            int requests = 0;
            long bytes = 0;
            TimeSpan duration = TimeSpan.Zero;

            foreach (var source in data)
            {
                lock (source.Value._lock)
                {
                    foreach (var resource in source.Value.Resources.Values)
                    {
                        requests += resource.count;
                        duration += resource.duration;
                    }

                    bytes += source.Value.NupkgSize;
                }
            }

            return new Totals(requests, bytes, duration);
        }

        public class Totals
        {
            public Totals(int requests, long bytes, TimeSpan duration)
            {
                Requests = requests;
                Bytes = bytes;
                Duration = duration;
            }

            public int Requests { get; }
            public long Bytes { get; }
            public TimeSpan Duration { get; }
        }

        internal class Data
        {
            internal object _lock;
            internal Dictionary<string, (int count, TimeSpan duration)> Resources { get; }
            internal HttpData Http { get; }
            internal int NupkgCount { get; set; }
            internal long NupkgSize { get; set; }

            internal Data()
            {
                _lock = new object();
                Resources = new Dictionary<string, (int count, TimeSpan duration)>();
                Http = new HttpData();
            }
        }

        internal class HttpData
        {
            public int Requests;
            public TimeSpan TotalDuration;
            public TimeSpan? HeaderDuration;
            public long TotalBytes;
            public readonly Dictionary<int, int> StatusCodes = new Dictionary<int, int>();
            public int Successful;
            public int Retries;
            public int Cancelled;
            public int Failed;

            public HttpData()
            {
                HeaderDuration = TimeSpan.Zero;
            }
        }

        internal static class PropertyNames
        {
            internal const string ParentId = "parentid";
            internal const string Action = "action";

            internal static class Source
            {
                internal const string Url = "source.url";
                internal const string Type = "source.type";
                internal const string MSFeed = "source.msfeed";
            }

            internal static class Duration
            {
                internal const string Total = "duration.total";
            }

            internal static class Nupkgs
            {
                internal const string Copied = "nupkgs.copied";
                internal const string Bytes = "nupkgs.bytes";
            }

            internal static class Resources
            {
                internal const string Calls = "resources.calls";
                internal const string Details = "resources.details";
            }

            internal static class Http
            {
                internal const string Requests = "http.requests";
                internal const string Successful = "http.successful";
                internal const string Retries = "http.retries";
                internal const string Cancelled = "http.cancelled";
                internal const string Failed = "http.failed";
                internal const string Bytes = "http.bytes";
                internal const string StatusCodes = "http.statuscodes";

                internal static class Duration
                {
                    internal const string Total = "http.duration.total";
                    internal const string Header = "http.duration.header";
                }
            }

            internal static class PackageSourceMapping
            {
                internal const string IsMappingEnabled = "PackageSourceMapping.IsMappingEnabled";
            }
        }
    }
}
