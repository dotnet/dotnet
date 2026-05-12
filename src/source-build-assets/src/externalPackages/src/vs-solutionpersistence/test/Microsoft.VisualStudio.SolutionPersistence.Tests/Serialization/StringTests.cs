// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using Microsoft.VisualStudio.SolutionPersistence.Serializer.Xml;

namespace Serialization;

/// <summary>
/// Tests related to processing strings.
/// </summary>
public sealed class StringTests
{
    private readonly StringTable stringTable = new StringTable().WithSolutionConstants();

    /// <summary>
    /// Make sure all keywords have a string representation.
    /// </summary>
    [Fact]
    public void KeywordStrings()
    {
        // Validate that if any new keywords are added to the enum, they are in the mapping tables.
        foreach (Keyword keyword in Enum.GetValues(typeof(Keyword)))
        {
            if (keyword is not Keyword.Unknown and not < 0 and not >= Keyword.MaxProp)
            {
                string keywordStr = keyword.ToXmlString();
                Assert.True(keywordStr is not null, $"Keyword {keyword} does not have a string representation.");
                Assert.True(this.stringTable.Contains(keywordStr), $"StringTable missing {keyword.ToXmlString()}");
            }
        }
    }

    /// <summary>
    /// Make sure all build type names have a string representation.
    /// </summary>
    [Fact]
    public void BuildTypeNamesTest()
    {
        foreach (FieldInfo fieldInfo in typeof(BuildTypeNames).GetFields(BindingFlags.NonPublic | BindingFlags.Static))
        {
            string value = fieldInfo.GetValue(null) as string ?? string.Empty;
            Assert.True(BuildTypeNames.TryGetKnown(value.AsSpan(), out string? str), $"BuildType lookup missing {value}");
            Assert.True(this.stringTable.Contains(str), $"StringTable missing {str}");
        }
    }

    /// <summary>
    /// Make sure all platform names have a string representation.
    /// </summary>
    [Fact]
    public void PlatformNamesTest()
    {
        foreach (FieldInfo fieldInfo in typeof(PlatformNames).GetFields(BindingFlags.NonPublic | BindingFlags.Static))
        {
            string value = fieldInfo.GetValue(null) as string ?? string.Empty;
            Assert.True(PlatformNames.TryGetKnown(value.AsSpan(), out string? str), $"Platform lookup missing {value}");
            Assert.True(this.stringTable.Contains(str), $"StringTable missing {str}");
        }
    }
}
