// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.SolutionPersistence.Serializer.Xml;

namespace Serialization;

/// <summary>
/// Tests related to processing strings.
/// </summary>
public sealed class StringTests
{
    /// <summary>
    /// Make sure all keywords have a string representation.
    /// </summary>
    [Fact]
    public void KeywordStrings()
    {
        // Validate that if any new keywords are added to the enum, they are in the mapping tables.
        foreach (Keyword keyword in Enum.GetValues(typeof(Keyword)))
        {
            if (keyword is not Keyword.Unknown and not < 0 and not >= Keyword.MaxProp &&
                keyword.ToXmlString() is null)
            {
                Assert.Fail($"Keyword {keyword} does not have a string representation.");
            }
        }
    }
}
