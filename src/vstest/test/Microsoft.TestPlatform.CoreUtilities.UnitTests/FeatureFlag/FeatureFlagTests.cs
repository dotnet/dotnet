// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.CoreUtilities.UnitTests;

[TestClass]
public class FeatureFlagTests
{
    [TestMethod]
    public void SingletonAlwaysReturnsTheSameInstance()
    {
        Assert.IsTrue(ReferenceEquals(FeatureFlag.Instance, FeatureFlag.Instance));
    }
}
