// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;
using Xunit.Sdk;

namespace Microsoft.DotNet.XUnitExtensions.Tests;

public class SkipOnCIAttributeTests
{
    private static readonly string[] s_ciVariables = ["DOTNET_CI", "HELIX_WORKITEM_ROOT", "AGENT_OS"];

    [SkipOnCI("reason")]
    private static void AllPlatforms() { }

    [SkipOnCI("reason", TestPlatforms.Windows | TestPlatforms.Linux | TestPlatforms.OSX)]
    private static void CurrentPlatform() { }

    [SkipOnCI("reason", TestPlatforms.Browser)]
    private static void OtherPlatform() { }

    [Theory]
    [InlineData(nameof(AllPlatforms))]
    [InlineData(nameof(CurrentPlatform))]
    public void InCI_MatchingPlatform_ReturnsFailingTrait(string methodName)
    {
        var res = GetTraits(methodName, inCI: true);

        var trait = Assert.Single(res);
        Assert.Equal(XunitConstants.Category, trait.Key);
        Assert.Equal("failing", trait.Value);
    }

    [Fact]
    public void InCI_OtherPlatform_ReturnsNoTraits()
    {
        Assert.Empty(GetTraits(nameof(OtherPlatform), inCI: true));
    }

    [Theory]
    [InlineData(nameof(AllPlatforms))]
    [InlineData(nameof(CurrentPlatform))]
    [InlineData(nameof(OtherPlatform))]
    public void NotInCI_ReturnsNoTraits(string methodName)
    {
        Assert.Empty(GetTraits(methodName, inCI: false));
    }

    private static IReadOnlyList<KeyValuePair<string, string>> GetTraits(string methodName, bool inCI)
    {
        MethodInfo method = typeof(SkipOnCIAttributeTests).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static);
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

            return TraitHelper.GetTraits(method);
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
