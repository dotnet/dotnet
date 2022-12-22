// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using Microsoft.AspNetCore.Razor.Test.Common;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LiveShare.Razor.Guest;

public class DefaultProxyAccessorTest : TestBase
{
    public DefaultProxyAccessorTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public void GetProjectHierarchyProxy_Caches()
    {
        // Arrange
        var proxy = Mock.Of<IProjectHierarchyProxy>(MockBehavior.Strict);
        var proxyAccessor = new TestProxyAccessor<IProjectHierarchyProxy>(proxy);

        // Act
        var proxy1 = proxyAccessor.GetProjectHierarchyProxy();
        var proxy2 = proxyAccessor.GetProjectHierarchyProxy();

        // Assert
        Assert.Same(proxy1, proxy2);
    }

    private class TestProxyAccessor<TTestProxy> : DefaultProxyAccessor where TTestProxy : class
    {
        private readonly TTestProxy _proxy;

        public TestProxyAccessor(TTestProxy proxy)
        {
            _proxy = proxy;
        }

        internal override TProxy CreateServiceProxy<TProxy>()
        {
            if (typeof(TProxy) == typeof(TTestProxy))
            {
                return _proxy as TProxy;
            }

            throw new InvalidOperationException("The proxy accessor was called with unexpected arguments.");
        }
    }
}
