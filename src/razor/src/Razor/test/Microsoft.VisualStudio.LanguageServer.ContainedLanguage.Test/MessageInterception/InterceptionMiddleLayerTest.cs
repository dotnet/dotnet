// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage.MessageInterception;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.WebTools.Languages.Shared.VS.Test.LanguageServer.MiddleLayerProviders;

public class InterceptionMiddleLayerTest : TestBase
{
    public InterceptionMiddleLayerTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public void Ctor_NullInterceptorManager_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new InterceptionMiddleLayer(null!, "test"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Ctor_EmptyLanguageName_Throws(string languageName)
    {
        var fakeInterceptorManager = Mock.Of<InterceptorManager>(MockBehavior.Strict);
        Assert.Throws<ArgumentException>(() => new InterceptionMiddleLayer(fakeInterceptorManager, languageName));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void CanHandle_DelegatesToInterceptionManager(bool value)
    {
        var fakeInterceptorManager = Mock.Of<InterceptorManager>(MockBehavior.Strict);
        Mock.Get(fakeInterceptorManager).Setup(x => x.HasInterceptor("testMessage", "testLanguage"))
                                        .Returns(value);
        var sut = new InterceptionMiddleLayer(fakeInterceptorManager, "testLanguage");

        var result = sut.CanHandle("testMessage");

        Assert.Equal(value, result);
    }

    [Fact]
    public async Task HandleNotificationAsync_IfInterceptorReturnsNull_DoesNotSendNotification()
    {
        var fakeInterceptorManager = Mock.Of<InterceptorManager>(MockBehavior.Strict);
        Mock.Get(fakeInterceptorManager)
            .Setup(x => x.HasInterceptor("testMethod", "testLanguage"))
            .Returns(true);
        Mock.Get(fakeInterceptorManager)
            .Setup(x => x.ProcessInterceptorsAsync(
                "testMethod",
                It.IsAny<JToken>(),
                "testLanguage",
                CancellationToken.None))
            .ReturnsAsync(value: null);
        var token = JToken.Parse("{}");
        var sut = new InterceptionMiddleLayer(fakeInterceptorManager, "testLanguage");
        var sentNotification = false;

        await sut.HandleNotificationAsync("testMethod", token, (_) =>
        {
            sentNotification = true;
            return Task.CompletedTask;
        });

        Assert.False(sentNotification);
    }

    [Fact]
    public async Task HandleNotificationAsync_IfInterceptorReturnsToken_SendsNotificationWithToken()
    {
        var token = JToken.Parse("{}");
        var expected = JToken.Parse("\"expected\"");
        JToken? actual = null;
        var fakeInterceptorManager = Mock.Of<InterceptorManager>(MockBehavior.Strict);
        Mock.Get(fakeInterceptorManager)
            .Setup(x => x.HasInterceptor("testMethod", "testLanguage"))
            .Returns(true);
        Mock.Get(fakeInterceptorManager)
            .Setup(x => x.ProcessInterceptorsAsync(
                "testMethod",
                It.IsAny<JToken>(),
                "testLanguage",
                CancellationToken.None))
            .ReturnsAsync(expected);
        var sut = new InterceptionMiddleLayer(fakeInterceptorManager, "testLanguage");

        await sut.HandleNotificationAsync("testMethod", token, (t) =>
        {
            actual = t;
            return Task.CompletedTask;
        });

        Assert.Equal(expected, actual);
    }
}
