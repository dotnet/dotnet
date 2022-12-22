// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.IO;

namespace Microsoft.VisualStudio.Razor.IntegrationTests;

public static class RazorProjectConstants
{
    internal const string BlazorProjectName = "BlazorProject";

    private static readonly string s_pagesDir = Path.Combine("Pages");
    private static readonly string s_sharedDir = Path.Combine("Shared");
    internal static readonly string FetchDataRazorFile = Path.Combine(s_pagesDir, "FetchData.razor");
    internal static readonly string CounterRazorFile = Path.Combine(s_pagesDir, "Counter.razor");
    internal static readonly string IndexRazorFile = Path.Combine(s_pagesDir, "Index.razor");
    // Temporarily don't use this because of a startup issue with creating new files
    //internal static readonly string ModifiedIndexRazorFile = Path.Combine(s_pagesDir, "ModifiedIndex.razor");
    internal static readonly string SemanticTokensFile = Path.Combine(s_pagesDir, "SemanticTokens.razor");
    internal static readonly string MainLayoutFile = Path.Combine(s_sharedDir, "MainLayout.razor");
    internal static readonly string ErrorCshtmlFile = Path.Combine(s_pagesDir, "Error.cshtml");
    internal static readonly string ImportsRazorFile = "_Imports.razor";

    internal static readonly string IndexPageContent = @"@page ""/""

<PageTitle>Index</PageTitle>

<h1>Hello, world!</h1>

Welcome to your new app.

<SurveyPrompt Title=""How is Blazor working for you?"" />";

    internal static readonly string MainLayoutContent = @"@inherits LayoutComponentBase

<PageTitle>BlazorApp</PageTitle>

<div class=""page"">
    <div class=""sidebar"">
        <NavMenu />
    </div>

    <main>
        <div class=""top-row px-4"">
            <a href=""https://docs.microsoft.com/aspnet/"" target=""_blank"">About</a>
        </div>

        <article class=""content px-4"">
            @Body
        </article>
    </main>
</div>
";
}
