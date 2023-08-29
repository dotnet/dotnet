using Microsoft.DotNet.ScenarioTests.Common;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.DotNet.ScenarioTests.SdkTemplateTests;

public class SdkTemplateTests : IClassFixture<ScenarioTestFixture>
{
    ScenarioTestFixture _scenarioTestInput;
    ITestOutputHelper _testOutputHelper;
    DotNetSdkHelper _sdkHelper;

    public SdkTemplateTests(ScenarioTestFixture testInput, ITestOutputHelper outputHelper)
    {
        if (string.IsNullOrEmpty(testInput.DotNetRoot))
        {
            throw new ArgumentException("sdk root must be set for sdk tests");
        }

        _scenarioTestInput = testInput;
        _testOutputHelper = outputHelper;
        _sdkHelper = new DotNetSdkHelper(outputHelper, _scenarioTestInput.DotNetRoot, _scenarioTestInput.SdkVersion);
    }

    [Theory]
    [MemberData(nameof(GetLanguages))]
    public void VerifyConsoleTemplateComplex(DotNetLanguage language)
    {
        var newTest = new SdkTemplateTest(
            nameof(SdkTemplateTests) + "Complex", language, _scenarioTestInput.TargetRid, DotNetSdkTemplate.Console,
            DotNetSdkActions.Build | DotNetSdkActions.Run | DotNetSdkActions.PublishComplex | DotNetSdkActions.PublishR2R);
        newTest.Execute(_sdkHelper, _scenarioTestInput.TestRoot);
    }

    [Theory]
    [MemberData(nameof(GetLanguages))]
    [Trait("Category", "Offline")]
    public void VerifyConsoleTemplate(DotNetLanguage language)
    {
        var newTest = new SdkTemplateTest(
            nameof(SdkTemplateTests), language, _scenarioTestInput.TargetRid, DotNetSdkTemplate.Console,
            DotNetSdkActions.Build | DotNetSdkActions.Run | DotNetSdkActions.Publish);
        newTest.Execute(_sdkHelper, _scenarioTestInput.TestRoot);
    }

    [Theory]
    [MemberData(nameof(GetLanguages))]
    [Trait("Category", "Offline")]
    public void VerifyClasslibTemplate(DotNetLanguage language)
    {
        var newTest = new SdkTemplateTest(
            nameof(SdkTemplateTests), language, _scenarioTestInput.TargetRid, DotNetSdkTemplate.ClassLib,
            DotNetSdkActions.Build | DotNetSdkActions.Publish);
        newTest.Execute(_sdkHelper, _scenarioTestInput.TestRoot);
    }

    [Theory]
    [MemberData(nameof(GetLanguages))]
    public void VerifyXUnitTemplate(DotNetLanguage language)
    {
        var newTest = new SdkTemplateTest(
            nameof(SdkTemplateTests), language, _scenarioTestInput.TargetRid, DotNetSdkTemplate.XUnit,
            DotNetSdkActions.Test);
        newTest.Execute(_sdkHelper, _scenarioTestInput.TestRoot);
    }

    [Theory]
    [MemberData(nameof(GetLanguages))]
    public void VerifyNUnitTemplate(DotNetLanguage language)
    {
        var newTest = new SdkTemplateTest(
            nameof(SdkTemplateTests), language, _scenarioTestInput.TargetRid, DotNetSdkTemplate.NUnit,
            DotNetSdkActions.Test);
        newTest.Execute(_sdkHelper, _scenarioTestInput.TestRoot);
    }

    [Theory]
    [MemberData(nameof(GetLanguages))]
    public void VerifyMSTestTemplate(DotNetLanguage language)
    {
        var newTest = new SdkTemplateTest(
            nameof(SdkTemplateTests), language, _scenarioTestInput.TargetRid, DotNetSdkTemplate.MSTest,
            DotNetSdkActions.Test);
        newTest.Execute(_sdkHelper, _scenarioTestInput.TestRoot);
    }
    
    [Theory]
    [InlineData(DotNetLanguage.CSharp)]
    [InlineData(DotNetLanguage.VB)]
    [Trait("Category", "Offline")]
    [Trait("SkipIfPlatform", "LINUX")]
    [Trait("SkipIfPlatform", "OSX")]
    public void VerifyWPFTemplate(DotNetLanguage language)
    {
        var newTest = new SdkTemplateTest(
            nameof(SdkTemplateTest), language, _scenarioTestInput.TargetRid, DotNetSdkTemplate.Wpf,
            DotNetSdkActions.Test | DotNetSdkActions.Run | DotNetSdkActions.Publish);
        newTest.Execute(_sdkHelper, _scenarioTestInput.TestRoot);
    }
    
    [Fact]
    [Trait("Category", "Offline")]
    [Trait("SkipIfPlatform", "LINUX")]
    [Trait("SkipIfPlatform", "OSX")]
    public void VerifyWebAppTemplate()
    {
        var newTest = new SdkTemplateTest(
            nameof(SdkTemplateTest), DotNetLanguage.CSharp, _scenarioTestInput.TargetRid, DotNetSdkTemplate.WebApp,
            DotNetSdkActions.Test | DotNetSdkActions.Run | DotNetSdkActions.Publish);
        newTest.Execute(_sdkHelper, _scenarioTestInput.TestRoot);
    }
    
    [Theory]
    [InlineData(DotNetLanguage.CSharp)]
    [InlineData(DotNetLanguage.VB)]
    [Trait("Category", "Offline")]
    [Trait("SkipIfPlatform", "LINUX")]
    [Trait("SkipIfPlatform", "OSX")]
    public void VerifyWinformsTemplate(DotNetLanguage language)
    {
        var newTest = new SdkTemplateTest(
            nameof(SdkTemplateTest), language, _scenarioTestInput.TargetRid, DotNetSdkTemplate.Winforms,
            DotNetSdkActions.Test | DotNetSdkActions.Run | DotNetSdkActions.Publish);
        newTest.Execute(_sdkHelper, _scenarioTestInput.TestRoot);
    }
    
    [Theory]
    [InlineData(DotNetLanguage.CSharp)]
    [InlineData(DotNetLanguage.VB)]
    [Trait("Category", "Offline")]
    [Trait("SkipIfPlatform", "OSX")]
    public void VerifyReferenceInConsoleTemplate(DotNetLanguage language)
    {
        var referenceTest = new SdkTemplateTest(
            nameof(SdkTemplateTest), language, _scenarioTestInput.TargetRid, DotNetSdkTemplate.ClassLib,
            DotNetSdkActions.Build);
        referenceTest.Execute(_sdkHelper, _scenarioTestInput.TestRoot);
        var newTest = new SdkTemplateTest(
            nameof(SdkTemplateTest), language, _scenarioTestInput.TargetRid, DotNetSdkTemplate.Console,
            DotNetSdkActions.AddClassLibRef | DotNetSdkActions.Build | DotNetSdkActions.Test |
            DotNetSdkActions.Run | DotNetSdkActions.Publish);
        newTest.Execute(_sdkHelper, _scenarioTestInput.TestRoot);
    }
    
    /*
    [Theory]
    [MemberData(nameof(GetAllLanguagesWithDownlevelFrameworks))]
    [Trait("Category", "Downlevel")]
    public void VerifyDownlevelFrameworksInConsoleTemplate(DotNetLanguage language, string frameworks)
    {
        var newTest = new SdkTemplateTest(
            nameof(SdkTemplateTests), language, _scenarioTestInput.TargetRid, DotNetSdkTemplate.Console,
            DotNetSdkActions.Build | DotNetSdkActions.Run | DotNetSdkActions.PublishComplex | DotNetSdkActions.PublishR2R);
        newTest.Execute(_sdkHelper, _scenarioTestInput.TestRoot, framework: "-f " + frameworks);
    }

    [Theory]
    [InlineData("net6.0")]
    [InlineData("net7.0")]
    [Trait("Category", "Downlevel")]
    public void VerifyDownLevelFrameworksInWebAppTemplate(string frameworks)
    {
        var newTest = new SdkTemplateTest(
            nameof(SdkTemplateTest), DotNetLanguage.CSharp, _scenarioTestInput.TargetRid, DotNetSdkTemplate.WebApp,
            DotNetSdkActions.Test | DotNetSdkActions.Run | DotNetSdkActions.PublishComplex);
        newTest.Execute(_sdkHelper, _scenarioTestInput.TestRoot, framework: "-f " + frameworks);
    }*/
    
    private static IEnumerable<object[]> GetLanguages() => Enum.GetValues<DotNetLanguage>().Select(lang => new object[] { lang });

    /*
    private static string[] AllDownLevelFrameworks = new string[] { "net6.0", "net7.0" };

    private static IEnumerable<object[]> GetAllLanguagesWithDownlevelFrameworks() 
        => Enum.GetValues<DotNetLanguage>().
        SelectMany(lang => AllDownLevelFrameworks.Select(tfm => new object[] { lang, tfm }));*/
}
