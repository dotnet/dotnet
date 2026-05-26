// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Xunit;

namespace NuGet.PackageManagement.UI.Test
{
    [Collection(MockedVS.Collection)]
    public class InfiniteScrollListTests
    {
        public InfiniteScrollListTests(GlobalServiceProvider sp)
        {
            sp.Reset();
        }

        [WpfFact(Skip = "https://github.com/NuGet/Home/issues/10938")]
        public void Constructor_JoinableTaskFactoryIsNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new InfiniteScrollList(joinableTaskFactory: null));

            Assert.Equal("joinableTaskFactory", exception.ParamName);
        }

        [WpfFact(Skip = "https://github.com/NuGet/Home/issues/10938")]
        public void DataContext_Initialized_DefaultIsItems()
        {
            var list = new InfiniteScrollList();

            Assert.Same(list.DataContext, list.ViewModel.Items);
        }
    }
}
