// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Completion;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.VisualStudio.Editor.Razor;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks.LanguageServer;

public class TagHelperCompletionBenchmark
{
    // These were gleaned by creating a Blazor Server app, opening Counter.razor, typing a '<' in the editor,
    // and breaking into LspTagHelperCompletionService.GetElementCompletions(...) in the debugger.
    private static readonly string[] s_existingElementCompletions =
    [
        "html", "head", "title", "base", "link", "meta", "style", "script", "noscript", "body", "section", "nav", "article", "aside",
        "h1", "h2", "h3", "h4", "h5", "h6", "header", "footer", "address", "p", "br", "pre", "dialog", "blockquote", "ol", "ul", "li",
        "dl", "dt", "dd", "a", "em", "strong", "small", "cite", "bdi", "q", "dfn", "abbr", "code", "var", "samp", "kbd", "sub", "sup",
        "i", "b", "u", "s", "mark", "progress", "meter", "time", "ruby", "rtc", "rt", "rp", "bdo", "span", "ins", "del", "figure",
        "img", "iframe", "embed", "object", "param", "details", "summary", "command", "menu", "menuitem", "legend", "div", "main",
        "template", "polymer-element", "ng-form", "ng-include", "ng-pluralize", "ng-switch", "ng-view", "source", "track", "audio",
        "video", "picture", "hr", "wbr", "form", "fieldset", "label", "input", "button", "select", "datalist", "optgroup", "option",
        "textarea", "keygen", "output", "canvas", "map", "area", "math", "svg", "table", "caption", "colgroup", "col", "thead", "tfoot",
        "tbody", "tr", "th", "td", "content", "shadow"
    ];

    [Benchmark]
    public object GetAttributeCompletions()
    {
        var tagHelperCompletionService = new TagHelperCompletionService();
        var context = new AttributeCompletionContext(
            TagHelperDocumentContext.Create(prefix: null, CommonResources.TelerikTagHelpers),
            existingCompletions: [],
            currentTagName: "PageTitle",
            currentAttributeName: null,
            attributes: [],
            currentParentTagName: "PageTitle",
            currentParentIsTagHelper: true,
            inHTMLSchema: HtmlFacts.IsHtmlTagName);

        return tagHelperCompletionService.GetAttributeCompletions(context);
    }

    [Benchmark]
    public object GetElementCompletions()
    {
        var tagHelperCompletionService = new TagHelperCompletionService();
        var context = new ElementCompletionContext(
            TagHelperDocumentContext.Create(prefix: null, CommonResources.TelerikTagHelpers),
            existingCompletions: s_existingElementCompletions,
            containingTagName: null,
            attributes: [],
            containingParentTagName: null,
            containingParentIsTagHelper: false,
            inHTMLSchema: HtmlFacts.IsHtmlTagName);

        return tagHelperCompletionService.GetElementCompletions(context);
    }
}
