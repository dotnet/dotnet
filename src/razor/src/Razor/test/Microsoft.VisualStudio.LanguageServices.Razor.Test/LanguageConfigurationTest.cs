// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Text.RegularExpressions;
using EnvDTE;
using Microsoft.CodeAnalysis.Testing;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LanguageServices.Razor.Test;

public class LanguageConfigurationTest
{
    private readonly ITestOutputHelper _output;

    public LanguageConfigurationTest(ITestOutputHelper output)
    {
        _output = output;
    }

    [Theory]
    [InlineData("""<div>$$""")]
    [InlineData("""<div>$$</div>""")]
    [InlineData("""<div class="hello">$$""")]
    [InlineData("""<div class="hello">$$</div>""")]
    [InlineData("""<div class="@(() => true)">$$""")]
    [InlineData("""<div class="@(() => true)">$$</div>""")]
    [InlineData("""<PropertyColumn Value="() => true" >$$""")]
    [InlineData("""<PropertyColumn Value="() => true" >$$</PropertyColumn>""")]
    public void OnEnter_WillIndent(string input)
    {
        TestFileMarkupParser.GetPosition(input, out input, out var position);

        Assert.True(WillIndent(input, position));
    }

    [Theory]
    [InlineData("""<input>$$""")]
    [InlineData("""<input />$$""")]
    [InlineData("""<PropertyColumn Value="() => true" />$$""")]
    [InlineData("""<PropertyColumn />$$""")]
    public void OnEnter_WontIndent(string input)
    {
        TestFileMarkupParser.GetPosition(input, out input, out var position);

        Assert.False(WillIndent(input, position));
    }

    public bool WillIndent(string input, int position)
    {
        var dir = Environment.CurrentDirectory;
        dir = dir.Substring(0, dir.IndexOf("artifacts"));
        var langConfigFile = Path.Combine(dir, @"src\Razor\src\Microsoft.VisualStudio.RazorExtension", "language-configuration.json");
        var langConfig = JObject.Parse(File.ReadAllText(langConfigFile));

        var onEnterRules = langConfig["onEnterRules"]!;
        foreach (var rule in onEnterRules)
        {
            var beforePattern = rule.Value<string>("beforeText");
            var afterPattern = rule.Value<string>("afterText");

            var before = input.Substring(0, position);
            var after = input.Substring(position);

            if (Regex.IsMatch(before, beforePattern))
            {
                _output.WriteLine("Matched beforeText pattern: " + beforePattern);
                if (afterPattern is null)
                {
                    _output.WriteLine("No afterText pattern found. Match!");
                    return true;
                }
                else if (Regex.IsMatch(after, afterPattern))
                {
                    _output.WriteLine("Matched afterText pattern: " + afterPattern);
                    _output.WriteLine("Match!");
                    return true;
                }

                _output.WriteLine("No match on afterText pattern.");
            }
        }

        _output.WriteLine("No match on any pattern.");
        return false;
    }
}
