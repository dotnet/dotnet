// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using FluentAssertions;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Moq;
using Xunit;

namespace NuGet.VisualStudio.Common.Test
{

    public partial class OutputConsoleLoggerTests
    {
        public class Constructor : OutputConsoleLoggerTests
        {
            public Constructor(GlobalServiceProvider sp)
                : base(sp)
            { }

            [Fact]
            public void When_null_visualStudioShell_is_passed_ArgumentNullException_is_thrown()
            {
                var exception = Assert.Throws<ArgumentNullException>(() => new OutputConsoleLogger(visualStudioShell: null, _outputConsoleProvider.Object, new Lazy<INuGetErrorList>(() => _errorList.Object)));
                exception.ParamName.Should().Be("visualStudioShell");
            }

            [Fact]
            public void When_null_consoleProvider_is_passed_ArgumentNullException_is_thrown()
            {
                var exception = Assert.Throws<ArgumentNullException>(() => new OutputConsoleLogger(_visualStudioShell.Object, consoleProvider: null, new Lazy<INuGetErrorList>(() => _errorList.Object)));
                exception.ParamName.Should().Be("consoleProvider");
            }

            [Fact]
            public void When_null_errorList_is_passed_ArgumentNullException_is_thrown()
            {
                var exception = Assert.Throws<ArgumentNullException>(() => new OutputConsoleLogger(_visualStudioShell.Object, _outputConsoleProvider.Object, errorList: null));
                exception.ParamName.Should().Be("errorList");
            }

            [Fact]
            public void Subscribes_to_BuildBegin_events()
            {
                _visualStudioShell.Verify(vss => vss.SubscribeToBuildBeginAsync(It.IsAny<Action>()), Times.Exactly(1));
            }

            [Fact]
            public void BuildBegin_event_clears_error_list_entries()
            {
                _onBuildBegin();
                _errorList.Verify(el => el.ClearNuGetEntries(), Times.Exactly(1));
            }

            [Fact]
            public void Subscribes_to_AfterClosing_events()
            {
                _visualStudioShell.Verify(vss => vss.SubscribeToAfterClosingAsync(It.IsAny<Action>()), Times.Exactly(1));
            }

            [Fact]
            public void AfterClosing_event_clears_error_list_entries()
            {
                _afterClosing();
                _errorList.Verify(eltds => eltds.ClearNuGetEntries(), Times.Exactly(1));
            }

            [Fact]
            public void Creates_package_manager_console()
            {
                _outputConsoleProvider.Verify(ocp => ocp.CreatePackageManagerConsoleAsync(), Times.Exactly(1));
            }
        }
    }
}
