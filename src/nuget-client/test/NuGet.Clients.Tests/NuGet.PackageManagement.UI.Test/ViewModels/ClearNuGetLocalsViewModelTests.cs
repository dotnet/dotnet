// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.Sdk.TestFramework;
using NuGet.PackageManagement.UI.ViewModels;
using Xunit;

namespace NuGet.PackageManagement.UI.Test.ViewModels
{
    [Collection(MockedVS.Collection)]
    public class ClearNuGetLocalsViewModelTests
    {
        private readonly string _exceptionMessage = "Test: Error in " + nameof(Execute_TaskFailedWithError_PropertiesUpdated);
        public ClearNuGetLocalsViewModelTests(GlobalServiceProvider sp)
        {
            sp.Reset();
        }

        [Fact]
        public void Constructor_WithNullClearNuGetLocalsCommandExecute_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ClearNuGetLocalsViewModel(clearNuGetLocalsCommandExecute: null!));
        }

        [Fact]
        public async Task Execute_TaskCompletedSuccessfully_PropertiesUpdated()
        {
            ClearNuGetLocalsViewModel viewModel = new ClearNuGetLocalsViewModel(clearNuGetLocalsCommandExecute: ClearNuGetLocalsCommandSuccessOnExecute);
            await viewModel.Execute();
            viewModel.IsCommandComplete.Should().BeTrue();
            viewModel.CommandCompleteText.Should().Contain("NuGet storage cleared at");
        }

        [Fact]
        public async Task Execute_TaskFailedWithError_PropertiesUpdated()
        {
            ClearNuGetLocalsViewModel viewModel = new ClearNuGetLocalsViewModel(clearNuGetLocalsCommandExecute: ClearNuGetLocalsCommandErrorOnExecute);
            await viewModel.Execute();
            viewModel.IsCommandComplete.Should().BeTrue();
            viewModel.CommandCompleteText.Should().Contain("NuGet storage clear failed at");
            viewModel.CommandCompleteText.Should().Contain(_exceptionMessage);
        }

        private Task ClearNuGetLocalsCommandSuccessOnExecute()
        {
            return Task.CompletedTask;
        }

        private Task ClearNuGetLocalsCommandErrorOnExecute()
        {
            throw new ApplicationException(_exceptionMessage);
        }
    }
}
