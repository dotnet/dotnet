// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.VisualStudio.Internal.Contracts;
using Xunit;

namespace NuGet.PackageManagement.UI.Test
{
    public class NuGetSolutionManagerServiceWrapperTests : IDisposable
    {
        private readonly NuGetSolutionManagerServiceWrapper _wrapper;

        public NuGetSolutionManagerServiceWrapperTests()
        {
            _wrapper = new NuGetSolutionManagerServiceWrapper();
        }

        public void Dispose()
        {
            _wrapper.Dispose();
        }

        [Fact]
        public void Service_Always_ReturnsNonNullValue()
        {
            Assert.NotNull(_wrapper.Service);

            using (_wrapper.Swap(newService: null))
            {
                Assert.NotNull(_wrapper.Service);
            }
        }

        [Fact]
        public void Swap_WhenNewServiceIsNull_ReturnsPreviousService()
        {
            using (INuGetSolutionManagerService previousService = _wrapper.Swap(newService: null))
            {
                Assert.NotNull(previousService);
            }
        }

        [Fact]
        public void Swap_WhenNewServiceIsNonNull_ReturnsPreviousService()
        {
            using (INuGetSolutionManagerService expectedResult = _wrapper.Service)
            using (INuGetSolutionManagerService actualResult = _wrapper.Swap(new TestNuGetSolutionManagerService()))
            {
                Assert.Same(expectedResult, actualResult);
            }
        }

        [Fact]
        public void AfterNuGetCacheUpdated_Always_ForwardsEvent()
        {
            var service = new TestNuGetSolutionManagerService();

            using (_wrapper.Swap(service))
            {
                const string path = "a";
                var eventRaised = false;

                _wrapper.AfterNuGetCacheUpdated += (sender, e) =>
                {
                    Assert.Equal(path, e);

                    eventRaised = true;
                };

                service.RaiseAfterNuGetCacheUpdated(path);

                Assert.True(eventRaised);
            }
        }

        [Fact]
        public void AfterProjectRenamed_Always_ForwardsEvent()
        {
            var service = new TestNuGetSolutionManagerService();

            using (_wrapper.Swap(service))
            {
                var project = Mock.Of<IProjectContextInfo>();
                var eventRaised = false;

                _wrapper.AfterProjectRenamed += (sender, e) =>
                {
                    Assert.Same(project, e);

                    eventRaised = true;
                };

                service.RaiseAfterProjectRenamed(project);

                Assert.True(eventRaised);
            }
        }

        [Fact]
        public void ProjectAdded_Always_ForwardsEvent()
        {
            var service = new TestNuGetSolutionManagerService();

            using (_wrapper.Swap(service))
            {
                var project = Mock.Of<IProjectContextInfo>();
                var eventRaised = false;

                _wrapper.ProjectAdded += (sender, e) =>
                {
                    Assert.Same(project, e);

                    eventRaised = true;
                };

                service.RaiseProjectAdded(project);

                Assert.True(eventRaised);
            }
        }

        [Fact]
        public void ProjectRemoved_Always_ForwardsEvent()
        {
            var service = new TestNuGetSolutionManagerService();

            using (_wrapper.Swap(service))
            {
                var project = Mock.Of<IProjectContextInfo>();
                var eventRaised = false;

                _wrapper.ProjectRemoved += (sender, e) =>
                {
                    Assert.Same(project, e);

                    eventRaised = true;
                };

                service.RaiseProjectRemoved(project);

                Assert.True(eventRaised);
            }
        }

        [Fact]
        public void ProjectRenamed_Always_ForwardsEvent()
        {
            var service = new TestNuGetSolutionManagerService();

            using (_wrapper.Swap(service))
            {
                var project = Mock.Of<IProjectContextInfo>();
                var eventRaised = false;

                _wrapper.ProjectRenamed += (sender, e) =>
                {
                    Assert.Same(project, e);

                    eventRaised = true;
                };

                service.RaiseProjectRenamed(project);

                Assert.True(eventRaised);
            }
        }

        [Fact]
        public void ProjectUpdated_Always_ForwardsEvent()
        {
            var service = new TestNuGetSolutionManagerService();

            using (_wrapper.Swap(service))
            {
                var project = Mock.Of<IProjectContextInfo>();
                var eventRaised = false;

                _wrapper.ProjectUpdated += (sender, e) =>
                {
                    Assert.Same(project, e);

                    eventRaised = true;
                };

                service.RaiseProjectUpdated(project);

                Assert.True(eventRaised);
            }
        }

        [Fact]
        public async Task GetSolutionDirectoryAsync_Always_ReturnsSolutionDirectory()
        {
            var service = new TestNuGetSolutionManagerService();

            using (_wrapper.Swap(service))
            {
                const string expectedResult = "a";

                service.SolutionDirectory = expectedResult;

                string actualResult = await _wrapper.GetSolutionDirectoryAsync(CancellationToken.None);

                Assert.Equal(expectedResult, actualResult);
            }
        }

        private sealed class TestNuGetSolutionManagerService : INuGetSolutionManagerService
        {
            public event EventHandler<string> AfterNuGetCacheUpdated;
            public event EventHandler<IProjectContextInfo> AfterProjectRenamed;
            public event EventHandler<IProjectContextInfo> ProjectAdded;
            public event EventHandler<IProjectContextInfo> ProjectRemoved;
            public event EventHandler<IProjectContextInfo> ProjectRenamed;
            public event EventHandler<IProjectContextInfo> ProjectUpdated;

            internal string SolutionDirectory { get; set; }

            public void Dispose()
            {
            }

            public ValueTask<string> GetSolutionDirectoryAsync(CancellationToken cancellationToken)
            {
                return new ValueTask<string>(SolutionDirectory);
            }

            internal void RaiseAfterNuGetCacheUpdated(string path)
            {
                AfterNuGetCacheUpdated?.Invoke(this, path);
            }

            internal void RaiseAfterProjectRenamed(IProjectContextInfo project)
            {
                AfterProjectRenamed?.Invoke(this, project);
            }

            internal void RaiseProjectAdded(IProjectContextInfo project)
            {
                ProjectAdded?.Invoke(this, project);
            }

            internal void RaiseProjectRemoved(IProjectContextInfo project)
            {
                ProjectRemoved?.Invoke(this, project);
            }

            internal void RaiseProjectRenamed(IProjectContextInfo project)
            {
                ProjectRenamed?.Invoke(this, project);
            }

            internal void RaiseProjectUpdated(IProjectContextInfo project)
            {
                ProjectUpdated?.Invoke(this, project);
            }
        }
    }
}
