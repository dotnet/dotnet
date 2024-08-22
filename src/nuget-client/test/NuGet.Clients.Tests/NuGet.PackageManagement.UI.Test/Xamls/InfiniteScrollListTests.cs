// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Microsoft.VisualStudio.Threading;
using Moq;
using NuGet.Common;
using NuGet.PackageManagement.VisualStudio;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Internal.Contracts;
using Xunit;
using Xunit.Abstractions;

namespace NuGet.PackageManagement.UI.Test
{
    [Collection(MockedVS.Collection)]
    public class InfiniteScrollListTests
    {
        private readonly ITestOutputHelper _output;

        public InfiniteScrollListTests(GlobalServiceProvider sp, ITestOutputHelper output)
        {
            sp.Reset();
            _output = output;
        }

        [WpfFact(Skip = "https://github.com/NuGet/Home/issues/10938")]
        public void Constructor_JoinableTaskFactoryIsNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new InfiniteScrollList(joinableTaskFactory: null));

            Assert.Equal("joinableTaskFactory", exception.ParamName);
        }

        [WpfFact(Skip = "https://github.com/NuGet/Home/issues/10938")]
        public void CheckBoxesEnabled_Initialized_DefaultIsFalse()
        {
            var list = new InfiniteScrollList();

            Assert.False(list.CheckBoxesEnabled);
        }

        [WpfFact(Skip = "https://github.com/NuGet/Home/issues/10938")]
        public void DataContext_Initialized_DefaultIsItems()
        {
            var list = new InfiniteScrollList();

            Assert.Same(list.DataContext, list.Items);
        }

        [WpfFact(Skip = "https://github.com/NuGet/Home/issues/10938")]
        public void IsSolution_Initialized_DefaultIsFalse()
        {
            var list = new InfiniteScrollList();

            Assert.False(list.IsSolution);
        }

        [WpfFact(Skip = "https://github.com/NuGet/Home/issues/10938")]
        public void Items_Initialized_DefaultIsEmpty()
        {
            var list = new InfiniteScrollList();

            Assert.Empty(list.Items);
        }

        [WpfFact(Skip = "https://github.com/NuGet/Home/issues/10938")]
        public void PackageItems_Initialized_DefaultIsEmpty()
        {
            var list = new InfiniteScrollList();

            Assert.Empty(list.PackageItems);
        }

        [WpfFact(Skip = "https://github.com/NuGet/Home/issues/10938")]
        public void SelectedPackageItem_Initialized_DefaultIsNull()
        {
            var list = new InfiniteScrollList();

            Assert.Null(list.SelectedPackageItem);
        }

        [WpfFact(Skip = "https://github.com/NuGet/Home/issues/10938")]
        public async Task LoadItems_LoaderIsNull_Throws()
        {
            var list = new InfiniteScrollList();

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                async () =>
                {
                    await list.LoadItemsAsync(
                        loader: null,
                        loadingMessage: "a",
                        logger: null,
                        searchResultTask: Task.FromResult<SearchResultContextInfo>(null),
                        token: CancellationToken.None);
                });

            Assert.Equal("loader", exception.ParamName);
        }

        [WpfTheory(Skip = "https://github.com/NuGet/Home/issues/10938")]
        [InlineData(null)]
        [InlineData("")]
        public async Task LoadItems_LoadingMessageIsNullOrEmpty_Throws(string loadingMessage)
        {
            var list = new InfiniteScrollList();

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                async () =>
                {
                    await list.LoadItemsAsync(
                        Mock.Of<IPackageItemLoader>(),
                        loadingMessage,
                        logger: null,
                        searchResultTask: Task.FromResult<SearchResultContextInfo>(null),
                        token: CancellationToken.None);
                });

            Assert.Equal("loadingMessage", exception.ParamName);
        }

        [WpfFact(Skip = "https://github.com/NuGet/Home/issues/10938")]
        public async Task LoadItems_SearchResultTaskIsNull_Throws()
        {
            var list = new InfiniteScrollList();

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                async () =>
                {
                    await list.LoadItemsAsync(
                        Mock.Of<IPackageItemLoader>(),
                        loadingMessage: "a",
                        logger: null,
                        searchResultTask: null,
                        token: CancellationToken.None);
                });

            Assert.Equal("searchResultTask", exception.ParamName);
        }

        [WpfFact(Skip = "https://github.com/NuGet/Home/issues/10938")]
        public async Task LoadItems_IfCancelled_Throws()
        {
            var list = new InfiniteScrollList();

            await Assert.ThrowsAsync<OperationCanceledException>(
                async () =>
                {
                    await list.LoadItemsAsync(
                        Mock.Of<IPackageItemLoader>(),
                        loadingMessage: "a",
                        logger: null,
                        searchResultTask: Task.FromResult<SearchResultContextInfo>(null),
                        token: new CancellationToken(canceled: true));
                });
        }

        [WpfFact(Skip = "https://github.com/NuGet/Home/issues/10938")]
        public async Task LoadItems_BeforeGettingCurrent_WaitsForInitialResults()
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var loader = new Mock<IPackageItemLoader>(MockBehavior.Strict);
            var state = new Mock<IItemLoaderState>();
            var hasWaited = false;

            loader.SetupGet(x => x.IsMultiSource)
                .Returns(true);
            loader.SetupGet(x => x.State)
                .Returns(state.Object);
            loader.Setup(x => x.UpdateStateAndReportAsync(
                    It.IsNotNull<SearchResultContextInfo>(),
                    It.IsNotNull<IProgress<IItemLoaderState>>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var loadingStatus = LoadingStatus.Loading;
            var loadingStatusCallCount = 0;

            state.Setup(x => x.LoadingStatus)
                .Returns(() => loadingStatus)
                .Callback(() =>
                    {
                        ++loadingStatusCallCount;

                        if (loadingStatusCallCount >= 2)
                        {
                            loadingStatus = LoadingStatus.NoItemsFound;
                            hasWaited = true;
                        }
                    });

            var itemsCount = 0;
            var itemsCountCallCount = 0;

            state.Setup(x => x.ItemsCount)
                .Returns(() => itemsCount)
                .Callback(() =>
                    {
                        ++itemsCountCallCount;

                        if (itemsCountCallCount >= 2)
                        {
                            itemsCount = 1;
                            hasWaited = true;
                        }
                    });

            loader.Setup(x => x.UpdateStateAsync(
                    It.IsNotNull<IProgress<IItemLoaderState>>(),
                    It.IsAny<CancellationToken>()))
                .Returns(() => Task.CompletedTask);

            var logger = new Mock<INuGetUILogger>();
            var searchResultTask = Task.FromResult(new SearchResultContextInfo());

            var list = new InfiniteScrollList();
            var taskCompletionSource = new TaskCompletionSource<string>();

            // Despite LoadItems(...) being a synchronous method, the method internally fires an asynchronous task.
            // We'll know when that task completes successfully when the LoadItemsCompleted event fires,
            // and to avoid infinite waits in exceptional cases, we'll interpret a call to reset as a failure.
            list.LoadItemsCompleted += (sender, args) => taskCompletionSource.TrySetResult(null);

            loader.Setup(x => x.Reset());
            logger.Setup(x => x.Log(It.Is<ILogMessage>(lm => lm.Level == LogLevel.Error && lm.Message != null)))
                  .Callback<ILogMessage>(
                    (logMessage) =>
                        {
                            taskCompletionSource.TrySetResult(logMessage.Message);
                        });
            loader.Setup(x => x.GetCurrent())
                .Returns(() =>
                {
                    if (!hasWaited)
                    {
                        taskCompletionSource.TrySetResult("GetCurrent() was called before waiting for initial results.");
                    }

                    return Enumerable.Empty<PackageItemViewModel>();
                });

            await list.LoadItemsAsync(
                loader.Object,
                loadingMessage: "a",
                logger: logger.Object,
                searchResultTask: searchResultTask,
                token: CancellationToken.None);

            var errorMessage = await taskCompletionSource.Task;

            Assert.Null(errorMessage);

            loader.Verify();
        }

        [WpfTheory(Skip = "https://github.com/NuGet/Home/issues/10938")]
        [MemberData(nameof(TestSearchMetadata))]
        public async Task LoadItemsAsync_LoadingStatusIndicator_InItemsCollectionIfEmptySearch(
            PackageSearchMetadataContextInfo[] searchItems,
            int expectedItems)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var loaderMock = new Mock<IPackageItemLoader>(MockBehavior.Strict);
            var stateMock = new Mock<IItemLoaderState>();
            var searchTask = Task.FromResult(new SearchResultContextInfo(searchItems, new Dictionary<string, LoadingStatus>(), true));
            var testLogger = new TestNuGetUILogger(_output);
            var tcs = new TaskCompletionSource<int>();
            var list = new InfiniteScrollList();
            var searchService = new Mock<INuGetSearchService>();

            var currentStatus = LoadingStatus.Loading;

            stateMock.Setup(x => x.LoadingStatus)
                .Returns(() => currentStatus);
            stateMock.Setup(x => x.ItemsCount)
                .Returns(() => searchItems.Length);
            loaderMock.SetupGet(x => x.State)
                .Returns(stateMock.Object);
            loaderMock.SetupGet(x => x.IsMultiSource)
                .Returns(false);
            loaderMock.Setup(x => x.UpdateStateAndReportAsync(
                    It.IsNotNull<SearchResultContextInfo>(),
                    It.IsNotNull<IProgress<IItemLoaderState>>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask)
                .Callback(() =>
                {
                    currentStatus = searchItems.Length > 0 ? LoadingStatus.Ready : LoadingStatus.NoItemsFound;
                });
            loaderMock.Setup(x => x.UpdateStateAsync(
                    It.IsNotNull<IProgress<IItemLoaderState>>(),
                    It.IsAny<CancellationToken>()))
                .Returns(() => Task.CompletedTask);
            loaderMock.Setup(x => x.GetCurrent())
                .Returns(() => searchItems.Select(x => new PackageItemViewModel(searchService.Object)));

            list.LoadItemsCompleted += (sender, args) =>
            {
                var lst = (InfiniteScrollList)sender;
                tcs.TrySetResult(lst.Items.Count);
                _output.WriteLine("3. After assert");
            };

            _output.WriteLine("1. Init act");
            await list.LoadItemsAsync(
                loader: loaderMock.Object,
                loadingMessage: "Test loading",
                logger: testLogger,
                searchResultTask: searchTask,
                token: CancellationToken.None);
            _output.WriteLine("2. End act");

            var finished = await tcs.Task;

            Assert.Equal(expectedItems, finished);
            _output.WriteLine("4. End of test");
        }

        public static IEnumerable<object[]> TestSearchMetadata()
        {
            var allData = new List<object[]>
            {
                new object[]{ new PackageSearchMetadataContextInfo[] {
                }, 1 }, // only loading indicator
                new object[]{ new PackageSearchMetadataContextInfo[] {
                    new PackageSearchMetadataContextInfo(),
                    new PackageSearchMetadataContextInfo(),
                    new PackageSearchMetadataContextInfo(),
                }, 3 }, // only search elements
            };

            return allData;
        }
    }
}
