// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.DotNet.ScenarioTests.Common;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.DotNet.ScenarioTests.SdkTemplateTests;

/// <summary>
/// Tests that validate template instantiation works correctly across different locales.
/// These tests verify that the .NET CLI and templates function properly when 
/// DOTNET_CLI_UI_LANGUAGE is set to various supported cultures.
/// </summary>
public class LocalizedTemplateTests : IClassFixture<ScenarioTestFixture>
{
    private readonly ScenarioTestFixture _scenarioTestInput;
    private readonly DotNetSdkHelper _sdkHelper;

    /// <summary>
    /// Supported .NET CLI locales based on SDK's localization support.
    /// </summary>
    private static readonly string[] SupportedCultures = new[]
    {
        "en-US",  // English (United States)
        "de-DE",  // German (Germany)
        "es-ES",  // Spanish (Spain)
        "fr-FR",  // French (France)
        "it-IT",  // Italian (Italy)
        "ja-JP",  // Japanese (Japan)
        "ko-KR",  // Korean (Korea)
        "pt-BR",  // Portuguese (Brazil)
        "ru-RU",  // Russian (Russia)
        "tr-TR",  // Turkish (Turkey)
        "zh-CN",  // Chinese (Simplified, China)
        "zh-TW",  // Chinese (Traditional, Taiwan)
    };

    /// <summary>
    /// All supported programming languages in .NET.
    /// </summary>
    private static readonly DotNetLanguage[] AllLanguages = Enum.GetValues<DotNetLanguage>();

    public LocalizedTemplateTests(ScenarioTestFixture testInput, ITestOutputHelper outputHelper)
    {
        if (string.IsNullOrEmpty(testInput.DotNetRoot))
        {
            throw new ArgumentException("sdk root must be set for sdk tests");
        }

        _scenarioTestInput = testInput;
        _sdkHelper = new DotNetSdkHelper(outputHelper, _scenarioTestInput.DotNetRoot, _scenarioTestInput.SdkVersion, _scenarioTestInput.BinlogDir);
    }

    [Theory]
    [MemberData(nameof(GetLocaleAndTemplateData))]
    [Trait("Category", "Offline")]
    public void VerifyTemplateInstantiationInLocale(string culture, DotNetSdkTemplate template, DotNetLanguage language)
    {
        // Don't use the cli language name in the project name because it may contain '#': https://github.com/dotnet/roslyn/issues/51692
        string projectName = $"{nameof(LocalizedTemplateTests)}_{template}_{language}_{SanitizeCultureName(culture)}";
        string projectDirectory = Path.Combine(_scenarioTestInput.TestRoot, projectName);
        
        Directory.CreateDirectory(projectDirectory);

        // Instantiate the template with the specified culture
        _sdkHelper.ExecuteNew(
            template.GetName(), 
            projectName, 
            projectDirectory, 
            language.ToCliName(),
            customArgs: null,
            culture: culture);

        // Verify the template was created successfully by building it
        _sdkHelper.ExecuteBuild(projectDirectory, culture: culture);
    }

    [Theory]
    [MemberData(nameof(GetCoreTemplatesWithLocales))]
    [Trait("Category", "Offline")]
    public void VerifyConsoleTemplateWithBuildAndRun(string culture, DotNetLanguage language)
    {
        string projectName = $"{nameof(LocalizedTemplateTests)}_ConsoleRun_{language}_{SanitizeCultureName(culture)}";
        string projectDirectory = Path.Combine(_scenarioTestInput.TestRoot, projectName);
        
        Directory.CreateDirectory(projectDirectory);

        // Instantiate console template with the specified culture
        _sdkHelper.ExecuteNew(
            DotNetSdkTemplate.Console.GetName(), 
            projectName, 
            projectDirectory, 
            language.ToCliName(),
            customArgs: null,
            culture: culture);

        // Build the project
        _sdkHelper.ExecuteBuild(projectDirectory, culture: culture);

        // Run the project to verify it executes successfully
        _sdkHelper.ExecuteRun(projectDirectory, frameworks: null, culture: culture);
    }

    /// <summary>
    /// Gets test data combining locales with core templates (console, classlib) and ASP.NET Core templates.
    /// This provides broad coverage of template instantiation across different cultures.
    /// ASP.NET Core templates (Web, Mvc, WebApi, Razor) are included as they don't come directly from the SDK.
    /// </summary>
    public static IEnumerable<object[]> GetLocaleAndTemplateData()
    {
        var coreTemplates = new[] {
            DotNetSdkTemplate.Console, 
            DotNetSdkTemplate.ClassLib,
            DotNetSdkTemplate.Web,
            DotNetSdkTemplate.Mvc,
            DotNetSdkTemplate.Razor
        };

        // WebApi requires portable assets
        if (!ScenarioTestFixture.IsCategoryExcluded("RequiresPortableAssets"))
        {
            coreTemplates.Add(DotNetSdkTemplate.WebApi);
        }

        var languages = new[] { DotNetLanguage.CSharp }; // Start with C# for broad locale coverage

        foreach (var culture in SupportedCultures)
        {
            foreach (var template in coreTemplates)
            {
                foreach (var language in languages)
                {
                    yield return new object[] { culture, template, language };
                }
            }
        }
    }

    /// <summary>
    /// Gets test data for console templates with all languages across supported locales.
    /// This provides more focused testing of the most common template with deeper language coverage.
    /// </summary>
    public static IEnumerable<object[]> GetCoreTemplatesWithLocales()
    {
        foreach (var culture in SupportedCultures)
        {
            foreach (var language in AllLanguages)
            {
                yield return new object[] { culture, language };
            }
        }
    }

    /// <summary>
    /// Sanitizes culture name for use in directory/project names by replacing hyphens with underscores.
    /// </summary>
    private static string SanitizeCultureName(string culture)
    {
        return culture.Replace("-", "_");
    }
}
