// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
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
        Assert.Equal(expected.ProjectConfigurationFileName, actual.ProjectConfigurationFileName);
        Assert.Equal(expected.CSharpVirtualDocumentSuffix, actual.CSharpVirtualDocumentSuffix);
        Assert.Equal(expected.HtmlVirtualDocumentSuffix, actual.HtmlVirtualDocumentSuffix);
        Assert.Equal(expected.SingleServerCompletionSupport, actual.SingleServerCompletionSupport);
        Assert.Equal(expected.SingleServerSupport, actual.SingleServerSupport);
        Assert.Equal(expected.ReturnCodeActionAndRenamePathsWithPrefixedSlash, actual.ReturnCodeActionAndRenamePathsWithPrefixedSlash);
    }

    [Fact]
    public void ProvideStringOption_OthersDefault()
    {
        var expected = new DefaultLanguageServerFeatureOptions();

        var projectRazorBinFilename = "project.razor.test.only.file.bin";
        var args = new[] { "--projectConfigurationFilename", projectRazorBinFilename };

        var actual = new ConfigurableLanguageServerFeatureOptions(args);

        Assert.Equal(projectRazorBinFilename, actual.ProjectConfigurationFileName);

        Assert.Equal(expected.SupportsFileManipulation, actual.SupportsFileManipulation);
        Assert.Equal(expected.CSharpVirtualDocumentSuffix, actual.CSharpVirtualDocumentSuffix);
        Assert.Equal(expected.HtmlVirtualDocumentSuffix, actual.HtmlVirtualDocumentSuffix);
        Assert.Equal(expected.SingleServerCompletionSupport, actual.SingleServerCompletionSupport);
        Assert.Equal(expected.SingleServerSupport, actual.SingleServerSupport);
        Assert.Equal(expected.ReturnCodeActionAndRenamePathsWithPrefixedSlash, actual.ReturnCodeActionAndRenamePathsWithPrefixedSlash);
    }

    [Fact]
    public void ProvideStringOption_IgnoresNoise()
    {
        var expected = new DefaultLanguageServerFeatureOptions();

        var projectRazorBinFilename = "project.razor.test.only.file.bin";
        var args = new[] { "noise", "--projectConfigurationFilename", projectRazorBinFilename, "ignore", "this" };

        var actual = new ConfigurableLanguageServerFeatureOptions(args);

        Assert.Equal(projectRazorBinFilename, actual.ProjectConfigurationFileName);

        Assert.Equal(expected.SupportsFileManipulation, actual.SupportsFileManipulation);
        Assert.Equal(expected.CSharpVirtualDocumentSuffix, actual.CSharpVirtualDocumentSuffix);
        Assert.Equal(expected.HtmlVirtualDocumentSuffix, actual.HtmlVirtualDocumentSuffix);
        Assert.Equal(expected.SingleServerCompletionSupport, actual.SingleServerCompletionSupport);
        Assert.Equal(expected.SingleServerSupport, actual.SingleServerSupport);
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
        Assert.Equal(expected.ProjectConfigurationFileName, actual.ProjectConfigurationFileName);
        Assert.Equal(expected.CSharpVirtualDocumentSuffix, actual.CSharpVirtualDocumentSuffix);
        Assert.Equal(expected.HtmlVirtualDocumentSuffix, actual.HtmlVirtualDocumentSuffix);
        Assert.Equal(expected.SingleServerCompletionSupport, actual.SingleServerCompletionSupport);
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
        Assert.Equal(expected.ProjectConfigurationFileName, actual.ProjectConfigurationFileName);
        Assert.Equal(expected.CSharpVirtualDocumentSuffix, actual.CSharpVirtualDocumentSuffix);
        Assert.Equal(expected.HtmlVirtualDocumentSuffix, actual.HtmlVirtualDocumentSuffix);
        Assert.Equal(expected.SingleServerCompletionSupport, actual.SingleServerCompletionSupport);
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
        Assert.Equal(expected.ProjectConfigurationFileName, actual.ProjectConfigurationFileName);
        Assert.Equal(expected.CSharpVirtualDocumentSuffix, actual.CSharpVirtualDocumentSuffix);
        Assert.Equal(expected.HtmlVirtualDocumentSuffix, actual.HtmlVirtualDocumentSuffix);
        Assert.Equal(expected.SingleServerCompletionSupport, actual.SingleServerCompletionSupport);
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

        Assert.Equal(expected.ProjectConfigurationFileName, actual.ProjectConfigurationFileName);
        Assert.Equal(expected.CSharpVirtualDocumentSuffix, actual.CSharpVirtualDocumentSuffix);
        Assert.Equal(expected.HtmlVirtualDocumentSuffix, actual.HtmlVirtualDocumentSuffix);
        Assert.Equal(expected.SingleServerCompletionSupport, actual.SingleServerCompletionSupport);
        Assert.Equal(expected.SingleServerSupport, actual.SingleServerSupport);
        Assert.Equal(expected.ReturnCodeActionAndRenamePathsWithPrefixedSlash, actual.ReturnCodeActionAndRenamePathsWithPrefixedSlash);
    }
}
