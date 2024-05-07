// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.PackageManagement.UI.ViewModels;
using Xunit;

namespace NuGet.PackageManagement.UI.Test.ViewModels
{
    public class KnownOwnerViewModelTests
    {
        [Fact]
        public void Constructor_WithNullKnownOwner_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new KnownOwnerViewModel(knownOwner: null));
        }
    }
}
