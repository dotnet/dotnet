// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace NuGet.VisualStudio.Common.Test
{
    [Collection(nameof(TestJoinableTaskFactoryCollection))]
    public abstract partial class OutputConsoleLoggerTests : IDisposable
    {
        private protected Action _onBuildBegin;
        private protected Action _afterClosing;
        private protected object _msBuildOutputVerbosity;

        private protected readonly Mock<IVisualStudioShell> _visualStudioShell = new Mock<IVisualStudioShell>();
        private protected readonly Mock<IOutputConsoleProvider> _outputConsoleProvider = new Mock<IOutputConsoleProvider>();
        private protected readonly Mock<IOutputConsole> _outputConsole = new Mock<IOutputConsole>();
        private protected readonly Mock<INuGetErrorList> _errorList = new Mock<INuGetErrorList>();
        private protected readonly OutputConsoleLogger _outputConsoleLogger;

        protected OutputConsoleLoggerTests()
        {
            _visualStudioShell.Setup(vss => vss.SubscribeToBuildBeginAsync(It.IsAny<Action>()))
                              .Returns(Task.CompletedTask)
                              .Callback((Action action) => { _onBuildBegin = action; });

            _visualStudioShell.Setup(vss => vss.SubscribeToAfterClosingAsync(It.IsAny<Action>()))
                              .Returns(Task.CompletedTask)
                              .Callback((Action action) => { _afterClosing = action; });

            _visualStudioShell.Setup(vss => vss.GetPropertyValueAsync("Environment", "ProjectsAndSolution", "MSBuildOutputVerbosity"))
                              .Returns(GetMSBuildOutputVerbosityAsync);

            _outputConsoleProvider.Setup(ocp => ocp.CreatePackageManagerConsoleAsync())
                                  .Returns(Task.FromResult(_outputConsole.Object));

            _outputConsoleLogger = new OutputConsoleLogger(_visualStudioShell.Object, _outputConsoleProvider.Object, new Lazy<INuGetErrorList>(() => _errorList.Object));
        }

        private Task<object> GetMSBuildOutputVerbosityAsync()
        {
            return Task.FromResult(_msBuildOutputVerbosity);
        }

        void IDisposable.Dispose()
        {
            _outputConsoleLogger.Dispose();
        }
    }
}
