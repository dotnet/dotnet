// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.Test.Common;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.AutoInsert;

public class CloseTextTagOnAutoInsertProviderTest(ITestOutputHelper testOutput) : RazorOnAutoInsertProviderTestBase(testOutput)
{
    [Fact]
    public void OnTypeCloseAngle_ClosesTextTag()
    {
        RunAutoInsertTest(
input: @"
@{
    <text>$$
}
",
expected: @"
@{
    <text>$0</text>
}
");
    }

    [Fact]
    public void OnTypeCloseAngle_OutsideRazorBlock_DoesNotCloseTextTag()
    {
        RunAutoInsertTest(
input: @"
    <text>$$
",
expected: @"
    <text>
");
    }

    internal override IOnAutoInsertProvider CreateProvider()
    {
        var configService = StrictMock.Of<IConfigurationSyncService>();
        var optionsMonitor = new RazorLSPOptionsMonitor(configService, RazorLSPOptions.Default);

        var provider = new CloseTextTagOnAutoInsertProvider(optionsMonitor);
        return provider;
    }
}
