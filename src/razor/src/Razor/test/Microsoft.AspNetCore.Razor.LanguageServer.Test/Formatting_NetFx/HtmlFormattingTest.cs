// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

[Collection(HtmlFormattingCollection.Name)]
public class HtmlFormattingTest(FormattingTestContext context, HtmlFormattingFixture fixture, ITestOutputHelper testOutput)
    : FormattingTestBase(context, fixture.Service, testOutput), IClassFixture<FormattingTestContext>
{
    [FormattingTestFact]
    public async Task FormatsComponentTags()
    {
        var tagHelpers = GetComponents();
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
            tagHelpers: [.. tagHelpers]);
    }

    [FormattingTestFact]
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
            tagHelpers: [.. tagHelpers]);
    }

    [FormattingTestFact]
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
            tagHelpers: [.. tagHelpers]);
    }

    [FormattingTestFact]
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
            tagHelpers: [.. tagHelpers]);
    }

    [FormattingTestFact]
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
            tagHelpers: [.. tagHelpers]);
    }

    [FormattingTestFact]
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
            tagHelpers: [.. GetComponents()]);
    }

    [FormattingTestFact]
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
            tagHelpers: [.. GetComponents()]);
    }

    [FormattingTestFact(Skip = "Requires fix")]
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
                    }
                    """,
            tagHelpers: [.. GetComponents()]);
    }

    [FormattingTestFact]
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
            tagHelpers: [.. GetComponents()]);
    }

    [FormattingTestFact]
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
            tagHelpers: [.. GetComponents()]);
    }

    [FormattingTestFact]
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
            tagHelpers: [.. CreateTagHelpers()]);

        ImmutableArray<TagHelperDescriptor> CreateTagHelpers()
        {
            var select = """
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
                    """;
            var selectItem = """
                    @typeparam TValue
                    <option value="@StringValue">@ChildContent</option>

                    @code
                    {
                        [Parameter] public TValue Value { get; set; }
                        [Parameter] public RenderFragment ChildContent { get; set; }

                        protected string StringValue => Value?.ToString();
                    }
                    """;

            var selectComponent = CompileToCSharp("Select.razor", select, throwOnFailure: true, fileKind: RazorFileKind.Component);
            var selectItemComponent = CompileToCSharp("SelectItem.razor", selectItem, throwOnFailure: true, fileKind: RazorFileKind.Component);

            using var _ = ArrayBuilderPool<TagHelperDescriptor>.GetPooledObject(out var tagHelpers);
            tagHelpers.AddRange(selectComponent.CodeDocument.GetRequiredTagHelperContext().TagHelpers);
            tagHelpers.AddRange(selectItemComponent.CodeDocument.GetRequiredTagHelperContext().TagHelpers);

            return tagHelpers.ToImmutable();
        }
    }

    [FormattingTestFact]
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
            allowDiagnostics: true);
    }

    private TagHelperCollection GetComponents()
    {
        AdditionalSyntaxTrees.Add(Parse("""
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
                """));

        var generated = CompileToCSharp("Test.razor", string.Empty, throwOnFailure: false, fileKind: RazorFileKind.Component);

        return generated.CodeDocument.GetRequiredTagHelperContext().TagHelpers;
    }
}
