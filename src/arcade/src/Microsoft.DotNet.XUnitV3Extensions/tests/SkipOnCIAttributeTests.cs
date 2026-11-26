// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Sdk;

namespace Microsoft.DotNet.XUnitExtensions.Tests;

public class SkipOnCIAttributeTests
{
    private const TestPlatforms CurrentPlatform = TestPlatforms.Windows | TestPlatforms.Linux | TestPlatforms.OSX;
    private static readonly string[] s_ciVariables = ["DOTNET_CI", "HELIX_WORKITEM_ROOT", "AGENT_OS"];

    [Fact]
    public void SingleArgument_AppliesToAllPlatforms()
    {
        Assert.Equal(TestPlatforms.Any, new SkipOnCIAttribute("reason").Platforms);
    }

    [Theory]
    [InlineData(TestPlatforms.Any)]
    [InlineData(CurrentPlatform)]
    public void InCI_MatchingPlatform_ReturnsFailingTrait(TestPlatforms platforms)
    {
        var res = GetTraits(new SkipOnCIAttribute("reason", platforms), inCI: true);

        var trait = Assert.Single(res);
        Assert.Equal(XunitConstants.Category, trait.Key);
        Assert.Equal("failing", trait.Value);
    }

    [Fact]
    public void InCI_OtherPlatform_ReturnsNoTraits()
    {
        Assert.Empty(GetTraits(new SkipOnCIAttribute("reason", TestPlatforms.Browser), inCI: true));
    }

    [Theory]
    [InlineData(TestPlatforms.Any)]
    [InlineData(CurrentPlatform)]
    [InlineData(TestPlatforms.Browser)]
    public void NotInCI_ReturnsNoTraits(TestPlatforms platforms)
    {
        Assert.Empty(GetTraits(new SkipOnCIAttribute("reason", platforms), inCI: false));
    }

    private static IReadOnlyCollection<KeyValuePair<string, string>> GetTraits(SkipOnCIAttribute attribute, bool inCI)
    {
        string[] saved = Array.ConvertAll(s_ciVariables, Environment.GetEnvironmentVariable);
        try
        {
            foreach (string name in s_ciVariables)
            {
                Environment.SetEnvironmentVariable(name, null);
            }

            if (inCI)
            {
                Environment.SetEnvironmentVariable("DOTNET_CI", "true");
            }

            return attribute.GetTraits();
        }
        finally
        {
            for (int i = 0; i < s_ciVariables.Length; i++)
            {
                Environment.SetEnvironmentVariable(s_ciVariables[i], saved[i]);
            }
        }
    }
}
