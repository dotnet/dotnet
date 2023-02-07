// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Moq;
using Test.Utility.VisualStudio;
using Xunit;

namespace NuGet.VisualStudio.Common.Test
{
    [Collection(MockedVS.Collection)]
    public abstract partial class OutputConsoleLoggerTests : IDisposable, IAsyncLifetime
    {
        private protected Action _onBuildBegin;
        private protected Action _afterClosing;
        private protected object _msBuildOutputVerbosity;

        private protected readonly Mock<IVisualStudioShell> _visualStudioShell = new Mock<IVisualStudioShell>();
        private protected readonly Mock<INuGetErrorList> _errorList = new Mock<INuGetErrorList>();
        private protected readonly OutputConsoleLogger _outputConsoleLogger;

        private protected readonly Mock<IOutputConsoleProvider> _outputConsoleProvider;
        private protected readonly Mock<IOutputConsole> _outputConsole;

        protected OutputConsoleLoggerTests(GlobalServiceProvider sp)
        {
            sp.Reset();

            _visualStudioShell.Setup(vss => vss.SubscribeToBuildBeginAsync(It.IsAny<Action>()))
                              .Returns(Task.CompletedTask)
                              .Callback((Action action) => { _onBuildBegin = action; });

            _visualStudioShell.Setup(vss => vss.SubscribeToAfterClosingAsync(It.IsAny<Action>()))
                              .Returns(Task.CompletedTask)
                              .Callback((Action action) => { _afterClosing = action; });

            _visualStudioShell.Setup(vss => vss.GetPropertyValueAsync("Environment", "ProjectsAndSolution", "MSBuildOutputVerbosity"))
                              .Returns(GetMSBuildOutputVerbosityAsync);

            var mockOutputConsoleUtility = OutputConsoleUtility.GetMock();
            _outputConsole = mockOutputConsoleUtility.mockIOutputConsole;
            _outputConsoleProvider = mockOutputConsoleUtility.mockIOutputConsoleProvider;

            _outputConsoleLogger = new OutputConsoleLogger(_visualStudioShell.Object, _outputConsoleProvider.Object, new Lazy<INuGetErrorList>(() => _errorList.Object));
        }

        /// <summary>
        /// Waits until the <see cref="OutputConsoleLogger._semaphore" /> has reset.
        /// </summary>
        private async Task WaitForInitializationAsync()
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                // Wait on a task that does nothing so we know when the semaphore's enqueued real work is complete.
                await _outputConsoleLogger._semaphore.ExecuteAsync(() => { return Task.CompletedTask; });
            });
        }

        private Task<object> GetMSBuildOutputVerbosityAsync()
        {
            return Task.FromResult(_msBuildOutputVerbosity);
        }

        void IDisposable.Dispose()
        {
            _outputConsoleLogger.Dispose();
        }

        public Task DisposeAsync() => Task.CompletedTask;

        public async Task InitializeAsync()
        {
            await WaitForInitializationAsync();
        }
    }
}
