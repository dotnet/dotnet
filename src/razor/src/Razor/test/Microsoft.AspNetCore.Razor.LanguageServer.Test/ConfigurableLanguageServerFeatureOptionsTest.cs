// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Xunit;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class ConfigurableLanguageServerFeatureOptionsTest
{
    [Fact]
    public void NoArgs_AllDefault()
    {
        var expected = new DefaultLanguageServerFeatureOptions();

        var actual = new ConfigurableLanguageServerFeatureOptions(Array.Empty<string>());

        Assert.Equal(expected.SupportsFileManipulation, actual.SupportsFileManipulation);
        Assert.Equal(expected.CSharpVirtualDocumentSuffix, actual.CSharpVirtualDocumentSuffix);
        Assert.Equal(expected.HtmlVirtualDocumentSuffix, actual.HtmlVirtualDocumentSuffix);
        Assert.Equal(expected.SingleServerSupport, actual.SingleServerSupport);
        Assert.Equal(expected.ReturnCodeActionAndRenamePathsWithPrefixedSlash, actual.ReturnCodeActionAndRenamePathsWithPrefixedSlash);
    }

    [Fact]
    public void ProvideStringOption_OthersDefault()
    {
        var expected = new DefaultLanguageServerFeatureOptions();

        var singleServerSupport = !expected.SingleServerSupport;
        var args = new[] { "--singleServerSupport", singleServerSupport.ToString() };

        var actual = new ConfigurableLanguageServerFeatureOptions(args);

        Assert.Equal(expected.SupportsFileManipulation, actual.SupportsFileManipulation);
        Assert.Equal(expected.CSharpVirtualDocumentSuffix, actual.CSharpVirtualDocumentSuffix);
        Assert.Equal(expected.HtmlVirtualDocumentSuffix, actual.HtmlVirtualDocumentSuffix);
        Assert.Equal(singleServerSupport, actual.SingleServerSupport);
        Assert.Equal(expected.ReturnCodeActionAndRenamePathsWithPrefixedSlash, actual.ReturnCodeActionAndRenamePathsWithPrefixedSlash);
    }

    [Fact]
    public void ProvideStringOption_IgnoresNoise()
    {
        var expected = new DefaultLanguageServerFeatureOptions();

        var singleServerSupport = !expected.SingleServerSupport;
        var args = new[] { "--singleServerSupport", singleServerSupport.ToString(), "ignore", "this" };

        var actual = new ConfigurableLanguageServerFeatureOptions(args);

        Assert.Equal(expected.SupportsFileManipulation, actual.SupportsFileManipulation);
        Assert.Equal(expected.CSharpVirtualDocumentSuffix, actual.CSharpVirtualDocumentSuffix);
        Assert.Equal(expected.HtmlVirtualDocumentSuffix, actual.HtmlVirtualDocumentSuffix);
        Assert.Equal(singleServerSupport, actual.SingleServerSupport);
        Assert.Equal(expected.ReturnCodeActionAndRenamePathsWithPrefixedSlash, actual.ReturnCodeActionAndRenamePathsWithPrefixedSlash);
    }

    [Fact]
    public void ProvideBoolOption_LastArgument()
    {
        var expected = new DefaultLanguageServerFeatureOptions();

        var args = new[] { "hoo", "goo", "--singleServerSupport" };

        var actual = new ConfigurableLanguageServerFeatureOptions(args);

        // If the default ever changes, this test would be invalid
        Assert.False(expected.SingleServerSupport);
        Assert.True(actual.SingleServerSupport);

        Assert.Equal(expected.SupportsFileManipulation, actual.SupportsFileManipulation);
        Assert.Equal(expected.CSharpVirtualDocumentSuffix, actual.CSharpVirtualDocumentSuffix);
        Assert.Equal(expected.HtmlVirtualDocumentSuffix, actual.HtmlVirtualDocumentSuffix);
        Assert.Equal(expected.ReturnCodeActionAndRenamePathsWithPrefixedSlash, actual.ReturnCodeActionAndRenamePathsWithPrefixedSlash);
    }

    [Fact]
    public void ProvideBoolOption_SingleArgument()
    {
        var expected = new DefaultLanguageServerFeatureOptions();

        var args = new[] { "--singleServerSupport", "--otherOption", "false" };

        var actual = new ConfigurableLanguageServerFeatureOptions(args);

        // If the default ever changes, this test would be invalid
        Assert.False(expected.SingleServerSupport);
        Assert.True(actual.SingleServerSupport);

        Assert.Equal(expected.SupportsFileManipulation, actual.SupportsFileManipulation);
        Assert.Equal(expected.CSharpVirtualDocumentSuffix, actual.CSharpVirtualDocumentSuffix);
        Assert.Equal(expected.HtmlVirtualDocumentSuffix, actual.HtmlVirtualDocumentSuffix);
        Assert.Equal(expected.ReturnCodeActionAndRenamePathsWithPrefixedSlash, actual.ReturnCodeActionAndRenamePathsWithPrefixedSlash);
    }

    [Fact]
    public void ProvideBoolOption_ExplicitTrue()
    {
        var expected = new DefaultLanguageServerFeatureOptions();

        var args = new[] { "--singleServerSupport", "true", "false" };

        var actual = new ConfigurableLanguageServerFeatureOptions(args);

        // If the default ever changes, this test would be invalid
        Assert.False(expected.SingleServerSupport);
        Assert.True(actual.SingleServerSupport);

        Assert.Equal(expected.SupportsFileManipulation, actual.SupportsFileManipulation);
        Assert.Equal(expected.CSharpVirtualDocumentSuffix, actual.CSharpVirtualDocumentSuffix);
        Assert.Equal(expected.HtmlVirtualDocumentSuffix, actual.HtmlVirtualDocumentSuffix);
        Assert.Equal(expected.ReturnCodeActionAndRenamePathsWithPrefixedSlash, actual.ReturnCodeActionAndRenamePathsWithPrefixedSlash);
    }

    [Fact]
    public void ProvideBoolOption_ExplicitFalse()
    {
        var expected = new DefaultLanguageServerFeatureOptions();

        var args = new[] { "--supportsFileManipulation", "false", "true" };

        var actual = new ConfigurableLanguageServerFeatureOptions(args);

        // If the default ever changes, this test would be invalid
        Assert.True(expected.SupportsFileManipulation);
        Assert.False(actual.SupportsFileManipulation);

        Assert.Equal(expected.CSharpVirtualDocumentSuffix, actual.CSharpVirtualDocumentSuffix);
        Assert.Equal(expected.HtmlVirtualDocumentSuffix, actual.HtmlVirtualDocumentSuffix);
        Assert.Equal(expected.SingleServerSupport, actual.SingleServerSupport);
        Assert.Equal(expected.ReturnCodeActionAndRenamePathsWithPrefixedSlash, actual.ReturnCodeActionAndRenamePathsWithPrefixedSlash);
    }
}
