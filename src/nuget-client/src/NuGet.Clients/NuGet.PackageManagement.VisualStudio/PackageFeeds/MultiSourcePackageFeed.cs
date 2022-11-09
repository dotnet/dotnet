// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Indexing;
using NuGet.PackageManagement.Telemetry;
using NuGet.Protocol.Core.Types;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Internal.Contracts;
using NuGet.VisualStudio.Telemetry;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>
    /// Consolidated live sources package feed enumerating packages and aggregating search results.
    /// </summary>
    public sealed class MultiSourcePackageFeed : IPackageFeed
    {
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(5);
        private const int PageSize = 25;

        private readonly SourceRepository[] _sourceRepositories;
        private readonly INuGetUILogger _logger;
        private readonly INuGetTelemetryService _telemetryService;

        public bool IsMultiSource => _sourceRepositories.Length > 1;

        private class TelemetryState
        {
            private int _emittedFlag = 0;

            public TelemetryState(Guid parentId, int pageIndex)
            {
                OperationId = parentId;
                PageIndex = pageIndex;
                Duration = Stopwatch.StartNew();
            }

            public Guid OperationId { get; }
            public int PageIndex { get; }
            public Stopwatch Duration { get; }

            /// <summary>
            /// This telemetry state should be emitted exactly once. This property will return true the first time it
            /// is called, then false for every subsequent call.
            /// </summary>
            public bool ShouldEmit
            {
                get
                {
                    var value = Interlocked.CompareExchange(ref _emittedFlag, 1, 0);
                    return value == 0;
                }
            }

            public TelemetryState NextPage()
            {
                return new TelemetryState(OperationId, PageIndex + 1);
            }
        }

        private class AggregatedContinuationToken : ContinuationToken
        {
            public TelemetryState TelemetryState { get; set; }
            public string SearchString { get; set; }
            public IDictionary<string, ContinuationToken> SourceSearchCursors { get; set; } = new Dictionary<string, ContinuationToken>();
        }

        private class AggregatedRefreshToken : RefreshToken
        {
            public TelemetryState TelemetryState { get; set; }
            public string SearchString { get; set; }
            public IDictionary<string, Task<SearchResult<IPackageSearchMetadata>>> SearchTasks { get; set; }
            public IDictionary<string, LoadingStatus> SourceSearchStatus { get; set; }
        }

        public MultiSourcePackageFeed(
            IEnumerable<SourceRepository> sourceRepositories,
            INuGetUILogger logger,
            INuGetTelemetryService telemetryService)
        {
            if (sourceRepositories == null)
            {
                throw new ArgumentNullException(nameof(sourceRepositories));
            }

            if (!sourceRepositories.Any())
            {
                throw new ArgumentException("Collection of source repositories cannot be empty", nameof(sourceRepositories));
            }

            _sourceRepositories = sourceRepositories.ToArray();
            _telemetryService = telemetryService;
            _logger = logger;
        }

        public async Task<SearchResult<IPackageSearchMetadata>> SearchAsync(string searchText, SearchFilter filter, CancellationToken cancellationToken)
        {
            var searchOperationId = Guid.NewGuid();
            if (_telemetryService != null)
            {
                _telemetryService.EmitTelemetryEvent(new SearchTelemetryEvent(
                    searchOperationId,
                    searchText,
                    filter.IncludePrerelease));
            }

            SearchResult<IPackageSearchMetadata> result;
            using (var packageSourceTelemetry = new PackageSourceTelemetry(_sourceRepositories, searchOperationId, PackageSourceTelemetry.TelemetryAction.Search))
            {
                var searchTasks = TaskCombinators.ObserveErrorsAsync(
                    _sourceRepositories,
                    r => r.PackageSource.Name,
                    (r, t) => r.SearchAsync(searchText, filter, PageSize, t),
                    LogError,
                    cancellationToken);

                result = await WaitForCompletionOrBailOutAsync(
                    searchText,
                    searchTasks,
                    new TelemetryState(searchOperationId, pageIndex: 0),
                    cancellationToken);

                if (_telemetryService != null)
                {
                    await packageSourceTelemetry.SendTelemetryAsync();
                    var protocolDiagnosticTotals = packageSourceTelemetry.GetTotals();
                    _telemetryService.EmitTelemetryEvent(SourceTelemetry.GetSearchSourceSummaryEvent(
                        searchOperationId,
                        _sourceRepositories.Select(x => x.PackageSource),
                        protocolDiagnosticTotals));
                }
            }

            return result;
        }

        public async Task<SearchResult<IPackageSearchMetadata>> ContinueSearchAsync(ContinuationToken continuationToken, CancellationToken cancellationToken)
        {
            var searchToken = continuationToken as AggregatedContinuationToken;

            if (searchToken?.SourceSearchCursors == null)
            {
                throw new InvalidOperationException("Invalid token");
            }

            var searchTokens = _sourceRepositories
                .Join(searchToken.SourceSearchCursors,
                    r => r.PackageSource.Name,
                    c => c.Key,
                    (r, c) => new { Repository = r, NextToken = c.Value });

            var searchTasks = TaskCombinators.ObserveErrorsAsync(
                searchTokens,
                j => j.Repository.PackageSource.Name,
                (j, t) => j.Repository.SearchAsync(j.NextToken, PageSize, t),
                LogError,
                cancellationToken);

            return await WaitForCompletionOrBailOutAsync(
                searchToken.SearchString,
                searchTasks,
                searchToken.TelemetryState?.NextPage(),
                cancellationToken);
        }

        public async Task<SearchResult<IPackageSearchMetadata>> RefreshSearchAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
        {
            var searchToken = refreshToken as AggregatedRefreshToken;

            if (searchToken == null)
            {
                throw new InvalidOperationException("Invalid token");
            }

            return await WaitForCompletionOrBailOutAsync(
                searchToken.SearchString,
                searchToken.SearchTasks,
                searchToken.TelemetryState,
                cancellationToken);
        }

        private async Task<SearchResult<IPackageSearchMetadata>> WaitForCompletionOrBailOutAsync(
            string searchText,
            IDictionary<string, Task<SearchResult<IPackageSearchMetadata>>> searchTasks,
            TelemetryState telemetryState,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (searchTasks.Count == 0)
            {
                return SearchResult.Empty<IPackageSearchMetadata>();
            }

            var aggregatedTask = Task.WhenAll(searchTasks.Values);

            RefreshToken refreshToken = null;
            if (aggregatedTask != await Task.WhenAny(aggregatedTask, Task.Delay(DefaultTimeout)))
            {
                refreshToken = new AggregatedRefreshToken
                {
                    TelemetryState = telemetryState,
                    SearchString = searchText,
                    SearchTasks = searchTasks,
                    RetryAfter = DefaultTimeout
                };
            }

            var partitionedTasks = searchTasks
                .ToLookup(t => t.Value.Status == TaskStatus.RanToCompletion);

            var completedOnly = partitionedTasks[true];

            SearchResult<IPackageSearchMetadata> aggregated;
            IEnumerable<TimeSpan> timings = null;
            var timeAggregation = new Stopwatch();
            if (completedOnly.Any())
            {
                var results = await Task.WhenAll(completedOnly.Select(kv => kv.Value));
                timings = results.Select(e => e.Duration);
                timeAggregation.Start();
                aggregated = await AggregateSearchResultsAsync(searchText, results, telemetryState);
                timeAggregation.Stop();
            }
            else
            {
                timings = Enumerable.Empty<TimeSpan>();
                aggregated = SearchResult.Empty<IPackageSearchMetadata>();
            }

            aggregated.OperationId = telemetryState?.OperationId;
            aggregated.RefreshToken = refreshToken;

            var notCompleted = partitionedTasks[false];

            if (notCompleted.Any())
            {
                var statuses = notCompleted.ToDictionary(
                    kv => kv.Key,
                    kv => GetLoadingStatus(kv.Value.Status));

                foreach (var item in statuses)
                {
                    aggregated.SourceSearchStatus.Add(item);
                }

                var exceptions = notCompleted
                    .Where(kv => kv.Value.Exception != null)
                    .ToDictionary(
                        kv => kv.Key,
                        kv => (Exception)kv.Value.Exception);

                foreach (var item in exceptions)
                {
                    aggregated.SourceSearchException.Add(item);
                }
            }

            if (_telemetryService != null
                && aggregated.SourceSearchStatus != null
                && aggregated.SourceSearchStatus.Values != null
                && telemetryState != null)
            {
                var loadingStatus = aggregated.SourceSearchStatus.Values.Aggregate();
                if (loadingStatus != LoadingStatus.Loading
                    && telemetryState.ShouldEmit)
                {
                    telemetryState.Duration.Stop();
                    _telemetryService.EmitTelemetryEvent(new SearchPageTelemetryEvent(
                        telemetryState.OperationId,
                        telemetryState.PageIndex,
                        aggregated.Items?.Count ?? 0,
                        telemetryState.Duration.Elapsed,
                        timings,
                        timeAggregation.Elapsed,
                        loadingStatus));
                }
            }

            return aggregated;
        }

        private static LoadingStatus GetLoadingStatus(TaskStatus taskStatus)
        {
            switch (taskStatus)
            {
                case TaskStatus.Canceled:
                    return LoadingStatus.Cancelled;
                case TaskStatus.Created:
                case TaskStatus.RanToCompletion:
                case TaskStatus.Running:
                case TaskStatus.WaitingForActivation:
                case TaskStatus.WaitingForChildrenToComplete:
                case TaskStatus.WaitingToRun:
                    return LoadingStatus.Loading;
                case TaskStatus.Faulted:
                    return LoadingStatus.ErrorOccurred;
                default:
                    return LoadingStatus.Unknown;
            }
        }

        private async Task<SearchResult<IPackageSearchMetadata>> AggregateSearchResultsAsync(
            string searchText,
            IEnumerable<SearchResult<IPackageSearchMetadata>> results,
            TelemetryState telemetryState)
        {
            SearchResult<IPackageSearchMetadata> result;

            var nonEmptyResults = results.Where(r => r.Any()).ToArray();
            if (nonEmptyResults.Length == 0)
            {
                result = SearchResult.Empty<IPackageSearchMetadata>();
            }
            else if (nonEmptyResults.Length == 1)
            {
                result = SearchResult.FromItems(nonEmptyResults[0].Items);
            }
            else
            {
                var items = nonEmptyResults.Select(r => r.Items).ToArray();

                var indexer = new RelevanceSearchResultsIndexer();
                var aggregator = new SearchResultsAggregator(indexer, new PackageSearchMetadataSplicer());
                var aggregatedItems = await aggregator.AggregateAsync(
                    searchText, items);

                result = SearchResult.FromItems(aggregatedItems.ToArray());
                // set correct count of unmerged items
                result.RawItemsCount = items.Aggregate(0, (r, next) => r + next.Count);
            }

            result.SourceSearchStatus = results
                .SelectMany(r => r.SourceSearchStatus)
                .ToDictionary(kv => kv.Key, kv => kv.Value);

            var cursors = results
                .Where(r => r.NextToken != null)
                .ToDictionary(r => r.SourceSearchStatus.Single().Key, r => r.NextToken);

            if (cursors.Keys.Any())
            {
                result.NextToken = new AggregatedContinuationToken
                {
                    TelemetryState = telemetryState,
                    SearchString = searchText,
                    SourceSearchCursors = cursors
                };
            }

            return result;
        }

        private void LogError(Task task, object state)
        {
            if (_logger == null)
            {
                // observe the task exception when no UI logger provided.
                Trace.WriteLine(ExceptionUtilities.DisplayMessage(task.Exception));
                return;
            }

            // UI logger only can be engaged from the main thread
            NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                var errorMessage = ExceptionUtilities.DisplayMessage(task.Exception);
                _logger.Log(
                    new LogMessage(
                        LogLevel.Error,
                        $"[{state.ToString()}] {errorMessage}"));
            }).PostOnFailure(nameof(MultiSourcePackageFeed));
        }
    }
}
