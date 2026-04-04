// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Xunit;

namespace OpenTelemetry.Sampler.AWS.Tests;

public class TestMatcher
{
    [Theory]
    [InlineData(null, "*")]
    [InlineData("", "*")]
    [InlineData("HelloWorld", "*")]
    [InlineData("HelloWorld", "HelloWorld")]
    [InlineData("HelloWorld", "Hello*")]
    [InlineData("HelloWorld", "*World")]
    [InlineData("HelloWorld", "?ello*")]
    [InlineData("HelloWorld", "Hell?W*d")]
    [InlineData("Hello.World", "*.World")]
    [InlineData("Bye.World", "*.World")]
    public void TestWildcardMatch(string? input, string pattern)
    {
        Assert.True(Matcher.WildcardMatch(input, pattern));
    }

    [Theory]
    [InlineData(null, "Hello*")]
    [InlineData("HelloWorld", null)]
    public void TestWildcardDoesNotMatch(string? input, string? pattern)
    {
        Assert.False(Matcher.WildcardMatch(input, pattern));
    }

    [Fact]
    public void TestAttributeMatching()
    {
        var tags = new List<KeyValuePair<string, object?>>
        {
            new("dog", "bark"),
            new("cat", "meow"),
            new("cow", "mooo"),
        };

        var ruleAttributes = new Dictionary<string, string>
        {
            { "dog", "bar?" },
            { "cow", "mooo" },
        };

        Assert.True(Matcher.AttributeMatch(tags, ruleAttributes));
    }

    [Fact]
    public void TestAttributeMatchingWithoutRuleAttributes()
    {
        var tags = new List<KeyValuePair<string, object?>>
        {
            new("dog", "bark"),
            new("cat", "meow"),
            new("cow", "mooo"),
        };

        var ruleAttributes = new Dictionary<string, string>();

        Assert.True(Matcher.AttributeMatch(tags, ruleAttributes));
    }

    [Fact]
    public void TestAttributeMatchingWithoutSpanTags()
    {
        var ruleAttributes = new Dictionary<string, string>
        {
            { "dog", "bar?" },
            { "cow", "mooo" },
        };

        Assert.False(Matcher.AttributeMatch([], ruleAttributes));
        Assert.False(Matcher.AttributeMatch(null, ruleAttributes));
    }
}
