<<<<<<< ours
﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Test.Common;
<<<<<<<< HEAD:src/razor/src/Razor/test/Microsoft.AspNetCore.Razor.LanguageServer.Test/Formatting/HtmlFormattingTest.cs
========
using Microsoft.VisualStudio.Razor.LanguageClient.Cohost.Formatting;
>>>>>>>> darc/forward/e449a2e-4ada078:src/razor/src/Razor/test/Microsoft.CodeAnalysis.Razor.CohostingShared.Test/Formatting/MoreHtmlFormattingTest.cs
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

<<<<<<<< HEAD:src/razor/src/Razor/test/Microsoft.AspNetCore.Razor.LanguageServer.Test/Formatting/HtmlFormattingTest.cs
public class HtmlFormattingTest(ITestOutputHelper testOutput) : DocumentFormattingTestBase(testOutput)
========
public class MoreHtmlFormattingTest(ITestOutputHelper testOutput) : DocumentFormattingTestBase(testOutput)
>>>>>>>> darc/forward/e449a2e-4ada078:src/razor/src/Razor/test/Microsoft.CodeAnalysis.Razor.CohostingShared.Test/Formatting/MoreHtmlFormattingTest.cs
{
    [Fact]
    public async Task FormatsComponentTags()
    {
        await RunFormattingTestAsync(
            input: """
                       <PageTitle>
                        @if(true){
                            <p>@DateTime.Now</p>
                    }
                    </PageTitle>

                        <GridTable>
                        @foreach (var row in rows){
                            <GridRow @onclick="SelectRow(row)">
                            @foreach (var cell in row){
                        <GridCell>@cell</GridCell>}</GridRow>
                        }
                    </GridTable>
                    """,
            expected: """
                    <PageTitle>
                        @if (true)
                        {
                            <p>@DateTime.Now</p>
                        }
                    </PageTitle>

                    <GridTable>
                        @foreach (var row in rows)
                        {
                            <GridRow @onclick="SelectRow(row)">
                                @foreach (var cell in row)
                                {
                                    <GridCell>@cell</GridCell>
                                }
                            </GridRow>
                        }
                    </GridTable>
                    """,
            htmlFormatted: """
                    <PageTitle>
                        @if(true){
                        <p>@DateTime.Now</p>
                        }
                    </PageTitle>

                    <GridTable>
                        @foreach (var row in rows){
                        <GridRow @onclick="SelectRow(row)">
                            @foreach (var cell in row){
                            <GridCell>@cell</GridCell>}
                        </GridRow>
                        }
                    </GridTable>
                    """,
<<<<<<<< HEAD:src/razor/src/Razor/test/Microsoft.AspNetCore.Razor.LanguageServer.Test/Formatting/HtmlFormattingTest.cs
            tagHelpers: [.. tagHelpers]);
========
            additionalFiles: [(FilePath("Components.cs"), GetComponents())]);
>>>>>>>> darc/forward/e449a2e-4ada078:src/razor/src/Razor/test/Microsoft.CodeAnalysis.Razor.CohostingShared.Test/Formatting/MoreHtmlFormattingTest.cs
    }

    [Fact]
    public async Task FormatsComponentTag_WithImplicitExpression()
    {
        var tagHelpers = GetComponents();
        await RunFormattingTestAsync(
            input: """
                        <GridTable>
                            <GridRow >
                        <GridCell>@cell</GridCell>
                    <GridCell>cell</GridCell>
                        </GridRow>
                    </GridTable>
                    """,
            expected: """
                    <GridTable>
                        <GridRow>
                            <GridCell>@cell</GridCell>
                            <GridCell>cell</GridCell>
                        </GridRow>
                    </GridTable>
                    """,
            htmlFormatted: """
                    <GridTable>
                        <GridRow>
                            <GridCell>@cell</GridCell>
                            <GridCell>cell</GridCell>
                        </GridRow>
                    </GridTable>
                    """,
<<<<<<<< HEAD:src/razor/src/Razor/test/Microsoft.AspNetCore.Razor.LanguageServer.Test/Formatting/HtmlFormattingTest.cs
            tagHelpers: [.. tagHelpers]);
========
            additionalFiles: [(FilePath("Components.cs"), GetComponents())]);
>>>>>>>> darc/forward/e449a2e-4ada078:src/razor/src/Razor/test/Microsoft.CodeAnalysis.Razor.CohostingShared.Test/Formatting/MoreHtmlFormattingTest.cs
    }

    [Fact]
    public async Task FormatsComponentTag_WithExplicitExpression()
    {
        var tagHelpers = GetComponents();
        await RunFormattingTestAsync(
            input: """
                        <GridTable>
                            <GridRow >
                        <GridCell>@(cell)</GridCell>
                        </GridRow>
                    </GridTable>
                    """,
            expected: """
                    <GridTable>
                        <GridRow>
                            <GridCell>@(cell)</GridCell>
                        </GridRow>
                    </GridTable>
                    """,
            htmlFormatted: """
                    <GridTable>
                        <GridRow>
                            <GridCell>@(cell)</GridCell>
                        </GridRow>
                    </GridTable>
                    """,
<<<<<<<< HEAD:src/razor/src/Razor/test/Microsoft.AspNetCore.Razor.LanguageServer.Test/Formatting/HtmlFormattingTest.cs
            tagHelpers: [.. tagHelpers]);
========
            additionalFiles: [(FilePath("Components.cs"), GetComponents())]);
>>>>>>>> darc/forward/e449a2e-4ada078:src/razor/src/Razor/test/Microsoft.CodeAnalysis.Razor.CohostingShared.Test/Formatting/MoreHtmlFormattingTest.cs
    }

    [Fact]
    public async Task FormatsComponentTag_WithExplicitExpression_FormatsInside()
    {
        var tagHelpers = GetComponents();
        await RunFormattingTestAsync(
            input: """
                        <GridTable>
                            <GridRow >
                        <GridCell>@(""  +    "")</GridCell>
                        </GridRow>
                    </GridTable>
                    """,
            expected: """
                    <GridTable>
                        <GridRow>
                            <GridCell>@("" + "")</GridCell>
                        </GridRow>
                    </GridTable>
                    """,
            htmlFormatted: """
                    <GridTable>
                        <GridRow>
                            <GridCell>@(""  +    "")</GridCell>
                        </GridRow>
                    </GridTable>
                    """,
<<<<<<<< HEAD:src/razor/src/Razor/test/Microsoft.AspNetCore.Razor.LanguageServer.Test/Formatting/HtmlFormattingTest.cs
            tagHelpers: [.. tagHelpers]);
========
            additionalFiles: [(FilePath("Components.cs"), GetComponents())]);
>>>>>>>> darc/forward/e449a2e-4ada078:src/razor/src/Razor/test/Microsoft.CodeAnalysis.Razor.CohostingShared.Test/Formatting/MoreHtmlFormattingTest.cs
    }

    [Fact]
    public async Task FormatsComponentTag_WithExplicitExpression_MovesStart()
    {
        var tagHelpers = GetComponents();
        await RunFormattingTestAsync(
            input: """
                        <GridTable>
                            <GridRow >
                        <GridCell>
                        @(""  +    "")
                        </GridCell>
                        </GridRow>
                    </GridTable>
                    """,
            expected: """
                    <GridTable>
                        <GridRow>
                            <GridCell>
                                @("" + "")
                            </GridCell>
                        </GridRow>
                    </GridTable>
                    """,
            htmlFormatted: """
                    <GridTable>
                        <GridRow>
                            <GridCell>
                                @(""  +    "")
                            </GridCell>
                        </GridRow>
                    </GridTable>
                    """,
<<<<<<<< HEAD:src/razor/src/Razor/test/Microsoft.AspNetCore.Razor.LanguageServer.Test/Formatting/HtmlFormattingTest.cs
            tagHelpers: [.. tagHelpers]);
========
            additionalFiles: [(FilePath("Components.cs"), GetComponents())]);
>>>>>>>> darc/forward/e449a2e-4ada078:src/razor/src/Razor/test/Microsoft.CodeAnalysis.Razor.CohostingShared.Test/Formatting/MoreHtmlFormattingTest.cs
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/aspnetcore/issues/30382")]
    public async Task FormatNestedComponents2()
    {
        await RunFormattingTestAsync(
            input: """
                    <GridTable>
                    <ChildContent>
                    <GridRow>
                    <ChildContent>
                    <GridCell>
                    <ChildContent>
                    <strong></strong>
                    @if (true)
                    {
                    <strong></strong>
                    }
                    <strong></strong>
                    </ChildContent>
                    </GridCell>
                    </ChildContent>
                    </GridRow>
                    </ChildContent>
                    </GridTable>
                    """,
            expected: """
                    <GridTable>
                        <ChildContent>
                            <GridRow>
                                <ChildContent>
                                    <GridCell>
                                        <ChildContent>
                                            <strong></strong>
                                            @if (true)
                                            {
                                                <strong></strong>
                                            }
                                            <strong></strong>
                                        </ChildContent>
                                    </GridCell>
                                </ChildContent>
                            </GridRow>
                        </ChildContent>
                    </GridTable>
                    """,
            htmlFormatted: """
                    <GridTable>
                        <ChildContent>
                            <GridRow>
                                <ChildContent>
                                    <GridCell>
                                        <ChildContent>
                                            <strong></strong>
                                            @if (true)
                                            {
                                            <strong></strong>
                                            }
                                            <strong></strong>
                                        </ChildContent>
                                    </GridCell>
                                </ChildContent>
                            </GridRow>
                        </ChildContent>
                    </GridTable>
                    """,
<<<<<<<< HEAD:src/razor/src/Razor/test/Microsoft.AspNetCore.Razor.LanguageServer.Test/Formatting/HtmlFormattingTest.cs
            tagHelpers: [.. GetComponents()]);
========
            additionalFiles: [(FilePath("Components.cs"), GetComponents())]);
>>>>>>>> darc/forward/e449a2e-4ada078:src/razor/src/Razor/test/Microsoft.CodeAnalysis.Razor.CohostingShared.Test/Formatting/MoreHtmlFormattingTest.cs
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor/issues/8227")]
    public async Task FormatNestedComponents3()
    {
        await RunFormattingTestAsync(
            input: """
                    @if (true)
                    {
                        <Component1 Id="comp1"
                                Caption="Title" />
                    <Component1 Id="comp2"
                                Caption="Title">
                                <Frag>
                    <Component1 Id="comp3"
                                Caption="Title" />
                                </Frag>
                                </Component1>
                    }

                    @if (true)
                    {
                        <a_really_long_tag_name Id="comp1"
                                Caption="Title" />
                    <a_really_long_tag_name Id="comp2"
                                Caption="Title">
                                <a_really_long_tag_name>
                    <a_really_long_tag_name Id="comp3"
                                Caption="Title" />
                                </a_really_long_tag_name>
                                </a_really_long_tag_name>
                    }
                    """,
            expected: """
                    @if (true)
                    {
                        <Component1 Id="comp1"
                                    Caption="Title" />
                        <Component1 Id="comp2"
                                    Caption="Title">
                            <Frag>
                                <Component1 Id="comp3"
                                            Caption="Title" />
                            </Frag>
                        </Component1>
                    }

                    @if (true)
                    {
                        <a_really_long_tag_name Id="comp1"
                                                Caption="Title" />
                        <a_really_long_tag_name Id="comp2"
                                                Caption="Title">
                            <a_really_long_tag_name>
                                <a_really_long_tag_name Id="comp3"
                                                        Caption="Title" />
                            </a_really_long_tag_name>
                        </a_really_long_tag_name>
                    }
                    """,
            htmlFormatted: """
                    @if (true)
                    {
                    <Component1 Id="comp1"
                                Caption="Title" />
                    <Component1 Id="comp2"
                                Caption="Title">
                        <Frag>
                            <Component1 Id="comp3"
                                        Caption="Title" />
                        </Frag>
                    </Component1>
                    }

                    @if (true)
                    {
                    <a_really_long_tag_name Id="comp1"
                                            Caption="Title" />
                    <a_really_long_tag_name Id="comp2"
                                            Caption="Title">
                        <a_really_long_tag_name>
                            <a_really_long_tag_name Id="comp3"
                                                    Caption="Title" />
                        </a_really_long_tag_name>
                    </a_really_long_tag_name>
                    }
                    """,
<<<<<<<< HEAD:src/razor/src/Razor/test/Microsoft.AspNetCore.Razor.LanguageServer.Test/Formatting/HtmlFormattingTest.cs
            tagHelpers: [.. GetComponents()]);
========
            additionalFiles: [(FilePath("Components.cs"), GetComponents())]);
>>>>>>>> darc/forward/e449a2e-4ada078:src/razor/src/Razor/test/Microsoft.CodeAnalysis.Razor.CohostingShared.Test/Formatting/MoreHtmlFormattingTest.cs
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor/issues/8228")]
    public async Task FormatNestedComponents4()
    {
        await RunFormattingTestAsync(
            input: """
                    @{
                        RenderFragment fragment =
                          @<Component1 Id="Comp1"
                                     Caption="Title">
                        </Component1>;
                    }
                    """,
            expected: """
                    @{
                        RenderFragment fragment =
                          @<Component1 Id="Comp1"
                                       Caption="Title">
                          </Component1>;
<<<<<<<< HEAD:src/razor/src/Razor/test/Microsoft.AspNetCore.Razor.LanguageServer.Test/Formatting/HtmlFormattingTest.cs
                    }
                    """,
            htmlFormatted: """
                    @{
                        RenderFragment fragment =
                          @<Component1 Id="Comp1"
                                       Caption="Title">
                    </Component1>;
========
>>>>>>>> darc/forward/e449a2e-4ada078:src/razor/src/Razor/test/Microsoft.CodeAnalysis.Razor.CohostingShared.Test/Formatting/MoreHtmlFormattingTest.cs
                    }
                    """,
            htmlFormatted: """
                    @{
                        RenderFragment fragment =
                          @<Component1 Id="Comp1"
                                       Caption="Title">
                    </Component1>;
                    }
                    """,
            additionalFiles: [(FilePath("Components.cs"), GetComponents())]);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor/issues/8229")]
    public async Task FormatNestedComponents5()
    {
        await RunFormattingTestAsync(
            input: """
                    <Component1>
                        @{
                            RenderFragment fragment =
                            @<Component1 Id="Comp1"
                                     Caption="Title">
                            </Component1>;
                        }
                    </Component1>
                    """,
            expected: """
                    <Component1>
                        @{
                            RenderFragment fragment =
                            @<Component1 Id="Comp1"
                                         Caption="Title">
                            </Component1>;
                        }
                    </Component1>
                    """,
            htmlFormatted: """
                    <Component1>
                        @{
                        RenderFragment fragment =
                        @<Component1 Id="Comp1"
                                     Caption="Title">
                        </Component1>;
                        }
                    </Component1>
                    """,
<<<<<<<< HEAD:src/razor/src/Razor/test/Microsoft.AspNetCore.Razor.LanguageServer.Test/Formatting/HtmlFormattingTest.cs
            tagHelpers: [.. GetComponents()]);
========
            additionalFiles: [(FilePath("Components.cs"), GetComponents())]);
>>>>>>>> darc/forward/e449a2e-4ada078:src/razor/src/Razor/test/Microsoft.CodeAnalysis.Razor.CohostingShared.Test/Formatting/MoreHtmlFormattingTest.cs
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/aspnetcore/issues/30382")]
    public async Task FormatNestedComponents2_Range()
    {
        await RunFormattingTestAsync(
            input: """
                    <GridTable>
                    <ChildContent>
                    <GridRow>
                    <ChildContent>
                    <GridCell>
                    <ChildContent>
                    <strong></strong>
                    @if (true)
                    {
                    [|<strong></strong>|]
                    }
                    <strong></strong>
                    </ChildContent>
                    </GridCell>
                    </ChildContent>
                    </GridRow>
                    </ChildContent>
                    </GridTable>
                    """,
            expected: """
                    <GridTable>
                    <ChildContent>
                    <GridRow>
                    <ChildContent>
                    <GridCell>
                    <ChildContent>
                    <strong></strong>
                    @if (true)
                    {
                                                <strong></strong>
                    }
                    <strong></strong>
                    </ChildContent>
                    </GridCell>
                    </ChildContent>
                    </GridRow>
                    </ChildContent>
                    </GridTable>
                    """,
            htmlFormatted: """
                    <GridTable>
                        <ChildContent>
                            <GridRow>
                                <ChildContent>
                                    <GridCell>
                                        <ChildContent>
                                            <strong></strong>
                                            @if (true)
                                            {
                                            <strong></strong>
                                            }
                                            <strong></strong>
                                        </ChildContent>
                                    </GridCell>
                                </ChildContent>
                            </GridRow>
                        </ChildContent>
                    </GridTable>
                    """,
<<<<<<<< HEAD:src/razor/src/Razor/test/Microsoft.AspNetCore.Razor.LanguageServer.Test/Formatting/HtmlFormattingTest.cs
            tagHelpers: [.. GetComponents()]);
========
            additionalFiles: [(FilePath("Components.cs"), GetComponents())]);
>>>>>>>> darc/forward/e449a2e-4ada078:src/razor/src/Razor/test/Microsoft.CodeAnalysis.Razor.CohostingShared.Test/Formatting/MoreHtmlFormattingTest.cs
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor/issues/6211")]
    public async Task FormatCascadingValueWithCascadingTypeParameter()
    {
        await RunFormattingTestAsync(
            input: """

                <div>
                    @foreach ( var i in new int[] { 1, 23 } )
                    {
                        <div></div>
                    }
                </div>
                <Select TValue="string">
                    @foreach ( var i in new int[] { 1, 23 } )
                    {
                        <SelectItem Value="@i">@i</SelectItem>
                    }
                </Select>
                """,
            expected: """

<<<<<<<< HEAD:src/razor/src/Razor/test/Microsoft.AspNetCore.Razor.LanguageServer.Test/Formatting/HtmlFormattingTest.cs
                    <div>
                        @foreach (var i in new int[] { 1, 23 })
                        {
                            <div></div>
                        }
                    </div>
                    <Select TValue="string">
                        @foreach (var i in new int[] { 1, 23 })
                        {
                            <SelectItem Value="@i">@i</SelectItem>
                        }
                    </Select>
                    """,
            htmlFormatted: """

                    <div>
                        @foreach ( var i in new int[] { 1, 23 } )
                        {
                        <div></div>
                        }
                    </div>
                    <Select TValue="string">
                        @foreach ( var i in new int[] { 1, 23 } )
                        {
                        <SelectItem Value="@i">@i</SelectItem>
                        }
                    </Select>
                    """,
            tagHelpers: [.. CreateTagHelpers()]);
========
                <div>
                    @foreach (var i in new int[] { 1, 23 })
                    {
                        <div></div>
                    }
                </div>
                <Select TValue="string">
                    @foreach (var i in new int[] { 1, 23 })
                    {
                        <SelectItem Value="@i">@i</SelectItem>
                    }
                </Select>
                """,
            htmlFormatted: """
>>>>>>>> darc/forward/e449a2e-4ada078:src/razor/src/Razor/test/Microsoft.CodeAnalysis.Razor.CohostingShared.Test/Formatting/MoreHtmlFormattingTest.cs

                <div>
                    @foreach ( var i in new int[] { 1, 23 } )
                    {
                    <div></div>
                    }
                </div>
                <Select TValue="string">
                    @foreach ( var i in new int[] { 1, 23 } )
                    {
                    <SelectItem Value="@i">@i</SelectItem>
                    }
                </Select>
                """,
            additionalFiles: [
                (FilePath("Select.razor"), """
                    @typeparam TValue
                    @attribute [CascadingTypeParameter(nameof(TValue))]
                    <CascadingValue Value="@this" IsFixed>
                        <select>
                            @ChildContent
                        </select>
                    </CascadingValue>

                    @code
                    {
                        [Parameter] public TValue SelectedValue { get; set; }
                    }
                    """),
                (FilePath("SelectItem.razor"), """
                    @typeparam TValue
                    <option value="@StringValue">@ChildContent</option>

                    @code
                    {
                        [Parameter] public TValue Value { get; set; }
                        [Parameter] public RenderFragment ChildContent { get; set; }

                        protected string StringValue => Value?.ToString();
                    }
                    """)]);
    }

    [Fact]
    public async Task PreprocessorDirectives()
    {
        await RunFormattingTestAsync(
            input: """
                <div Model="SomeModel">
                <div />
                @{
                #if DEBUG
                }
                 <div />
                @{
                #endif
                }
                </div>

                @code {
                    private object SomeModel {get;set;}
                }
                """,
            expected: """
                    <div Model="SomeModel">
                        <div />
                        @{
                    #if DEBUG
                    }
                     <div />
                    @{
                    #endif

                        }
                    </div>

                    @code {
                        private object SomeModel { get; set; }
                    }
                    """,
            htmlFormatted: """
                <div Model="SomeModel">
                    <div />
                    @{
                    #if DEBUG
                    }
                    <div />
                    @{
                    #endif
                    }
                </div>

                @code {
                    private object SomeModel {get;set;}
                }
                """,
            allowDiagnostics: true);
    }

    private string GetComponents() => """
        using Microsoft.AspNetCore.Components;
        namespace Test
        {
            public class GridTable : ComponentBase
            {
                [Parameter]
                public RenderFragment ChildContent { get; set; }
            }

            public class GridRow : ComponentBase
            {
                [Parameter]
                public RenderFragment ChildContent { get; set; }
            }

            public class GridCell : ComponentBase
            {
                [Parameter]
                public RenderFragment ChildContent { get; set; }
            }

            public class Component1 : ComponentBase
            {
                [Parameter]
                public string Id { get; set; }

                [Parameter]
                public string Caption { get; set; }

                [Parameter]
                public RenderFragment Frag {get;set;}
            }
        }
        """;
}
=======
PLEASE READ

Please remove this file during conflict resolution in your PR.
This file has been reverted (removed) in the source repository but the PR branch
does not have the file yet as it's based on an older commit. This means the file is
not getting removed in the PR due to the other conflicts.
>>>>>>> theirs
