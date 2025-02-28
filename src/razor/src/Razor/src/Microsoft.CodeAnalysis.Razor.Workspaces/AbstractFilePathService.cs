// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Razor.Workspaces;

internal abstract class AbstractFilePathService(LanguageServerFeatureOptions languageServerFeatureOptions) : IFilePathService
{
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions = languageServerFeatureOptions;

    public string GetRazorCSharpFilePath(ProjectKey projectKey, string razorFilePath)
        => GetGeneratedFilePath(projectKey, razorFilePath, _languageServerFeatureOptions.CSharpVirtualDocumentSuffix);

    public virtual Uri GetRazorDocumentUri(Uri virtualDocumentUri)
    {
        var uriPath = virtualDocumentUri.AbsoluteUri;
        var razorFilePath = GetRazorFilePath(uriPath);
        var uri = new Uri(razorFilePath, UriKind.Absolute);
        return uri;
    }

    public virtual bool IsVirtualCSharpFile(Uri uri)
        => CheckIfFileUriAndExtensionMatch(uri, _languageServerFeatureOptions.CSharpVirtualDocumentSuffix);

    public bool IsVirtualHtmlFile(Uri uri)
        => CheckIfFileUriAndExtensionMatch(uri, _languageServerFeatureOptions.HtmlVirtualDocumentSuffix);

    public bool IsVirtualDocumentUri(Uri uri)
        => IsVirtualCSharpFile(uri) || IsVirtualHtmlFile(uri);

    private static bool CheckIfFileUriAndExtensionMatch(Uri uri, string extension)
        => uri.GetAbsoluteOrUNCPath()?.EndsWith(extension, StringComparison.Ordinal) ?? false;

    private string GetRazorFilePath(string filePath)
    {
        var trimIndex = filePath.LastIndexOf(_languageServerFeatureOptions.HtmlVirtualDocumentSuffix);

        // We don't check for C# in cohosting, as it will throw, and people might call this method on any
        // random path.
        if (trimIndex == -1 && !_languageServerFeatureOptions.UseRazorCohostServer)
        {
            trimIndex = filePath.LastIndexOf(_languageServerFeatureOptions.CSharpVirtualDocumentSuffix);

            if (trimIndex == -1)
            {
                // Not a C# file, and we wouldn't have got here if it was Html, so nothing left to do
                return filePath;
            }

            // If this is a C# generated file, and we're including the project suffix, then filename will be
            // <Page>.razor.<project slug><c# suffix>
            if (_languageServerFeatureOptions.IncludeProjectKeyInGeneratedFilePath)
            {
                // We can remove the project key easily, by just looking for the last '.'. The project
                // slug itself cannot a '.', enforced by the assert below in GetProjectSuffix
                trimIndex = filePath.LastIndexOf('.', trimIndex - 1);
                Debug.Assert(trimIndex != -1, "There was no project element to the generated file name?");
            }
        }

        if (trimIndex != -1)
        {
            return filePath[..trimIndex];
        }

        return filePath;
    }

    private string GetGeneratedFilePath(ProjectKey projectKey, string razorFilePath, string suffix)
    {
        var projectSuffix = GetProjectSuffix(projectKey);

        return razorFilePath + projectSuffix + suffix;
    }

    private string GetProjectSuffix(ProjectKey projectKey)
    {
        if (!_languageServerFeatureOptions.IncludeProjectKeyInGeneratedFilePath)
        {
            return string.Empty;
        }

        // If there is no project key, we still want to generate something as otherwise the GetRazorFilePath method
        // would end up unnecessarily overcomplicated
        if (projectKey.IsUnknown)
        {
            return ".p";
        }

        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var crypto = sha256.ComputeHash(Encoding.Unicode.GetBytes(projectKey.Id));
        var projectToken = Convert.ToBase64String(crypto, 0, 12).Replace('/', '_').Replace('+', '-');

        Debug.Assert(!projectToken.Contains("."), "Project token can't contain a dot or the GetRazorFilePath method will fail.");
        return "." + projectToken;
    }

    internal TestAccessor GetTestAccessor() => new(this);

    internal readonly struct TestAccessor(AbstractFilePathService instance)
    {
        internal string GetRazorFilePath(string filePath) => instance.GetRazorFilePath(filePath);
    }
}
