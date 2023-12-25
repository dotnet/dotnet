// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class RazorLSPOptionsMonitorTest : ToolingTestBase
{
    private readonly IOptionsMonitorCache<RazorLSPOptions> _cache;

    public RazorLSPOptionsMonitorTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        var services = new ServiceCollection().AddOptions();
        _cache = services.BuildServiceProvider().GetRequiredService<IOptionsMonitorCache<RazorLSPOptions>>();
    }

    [Fact]
    public async Task UpdateAsync_Invokes_OnChangeRegistration()
    {
        // Arrange
        var expectedOptions = new RazorLSPOptions(EnableFormatting: false, AutoClosingTags: true, InsertSpaces: true, TabSize: 4, AutoShowCompletion: true, AutoListParams: true, FormatOnType: true, AutoInsertAttributeQuotes: true, ColorBackground: false, CommitElementsWithSpace: true);
        var configService = Mock.Of<IConfigurationSyncService>(
            f => f.GetLatestOptionsAsync(DisposalToken) == Task.FromResult(expectedOptions),
            MockBehavior.Strict);
        var optionsMonitor = new RazorLSPOptionsMonitor(configService, _cache, RazorLSPOptions.Default);
        var called = false;

        // Act & Assert
        optionsMonitor.OnChange(options =>
        {
            called = true;
            Assert.Same(expectedOptions, options);
        });

        await optionsMonitor.UpdateAsync(DisposalToken);
        Assert.True(called, "Registered callback was not called.");
    }

    [Fact]
    public async Task UpdateAsync_DoesNotInvoke_OnChangeRegistration_AfterDispose()
    {
        // Arrange
        var expectedOptions = new RazorLSPOptions(EnableFormatting: false, AutoClosingTags: true, InsertSpaces: true, TabSize: 4, AutoShowCompletion: true, AutoListParams: true, FormatOnType: true, AutoInsertAttributeQuotes: true, ColorBackground: false, CommitElementsWithSpace: true);
        var configService = Mock.Of<IConfigurationSyncService>(
            f => f.GetLatestOptionsAsync(DisposalToken) == Task.FromResult(expectedOptions),
            MockBehavior.Strict);
        var optionsMonitor = new RazorLSPOptionsMonitor(configService, _cache, RazorLSPOptions.Default);
        var called = false;
        var onChangeToken = optionsMonitor.OnChange(options => called = true);

        // Act 1
        await optionsMonitor.UpdateAsync(DisposalToken);

        // Assert 1
        Assert.True(called, "Registered callback was not called.");

        // Act 2
        called = false;
        onChangeToken.Dispose();
        await optionsMonitor.UpdateAsync(DisposalToken);

        // Assert 2
        Assert.False(called, "Registered callback called even after dispose.");
    }

    [Fact]
    public async Task UpdateAsync_ConfigReturnsNull_DoesNotInvoke_OnChangeRegistration()
    {
        // Arrange
        var configService = new Mock<IConfigurationSyncService>(MockBehavior.Strict).Object;
        Mock.Get(configService)
            .Setup(s => s.GetLatestOptionsAsync(DisposalToken))
            .ReturnsAsync(value: null);
        var optionsMonitor = new RazorLSPOptionsMonitor(configService, _cache, RazorLSPOptions.Default);
        var called = false;
        var onChangeToken = optionsMonitor.OnChange(options => called = true);

        // Act
        await optionsMonitor.UpdateAsync(DisposalToken);

        // Assert
        Assert.False(called, "Registered callback called even when GetLatestOptionsAsync() returns null.");
    }

    [Fact]
    public void InitializedOptionsAreCurrent()
    {
        // Arrange
        var expectedOptions = new RazorLSPOptions(EnableFormatting: false, AutoClosingTags: true, InsertSpaces: true, TabSize: 4, AutoShowCompletion: true, AutoListParams: true, FormatOnType: true, AutoInsertAttributeQuotes: true, ColorBackground: false, CommitElementsWithSpace: true);
        var configService = Mock.Of<IConfigurationSyncService>(
            f => f.GetLatestOptionsAsync(DisposalToken) == Task.FromResult(expectedOptions),
            MockBehavior.Strict);
        var optionsMonitor = new RazorLSPOptionsMonitor(configService, _cache, expectedOptions);

        // Act & Assert
        Assert.Same(expectedOptions, optionsMonitor.CurrentValue);
    }
}
