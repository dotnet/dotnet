// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Remote.Razor;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

[CollectionDefinition(nameof(CohostRenameEndpointTest), DisableParallelization = true)]
[Collection(nameof(CohostRenameEndpointTest))]
public class CohostRenameEndpointTest(ITestOutputHelper testOutputHelper) : CohostEndpointTestBase(testOutputHelper)
{
    [Fact]
    public Task CSharp_SameFile()
        => VerifyRenamesAsync(
            input: """
                This is a Razor document.

                <h1>@MyMethod()</h1>

                @code
                {
                    public string MyMe$$thod()
                    {
                        return $"Hi from {nameof(MyMethod)}";
                    }
                }

                The end.
                """,
            newName: "CallThisFunction",
            expected: """
                This is a Razor document.
                
                <h1>@CallThisFunction()</h1>
                
                @code
                {
                    public string CallThisFunction()
                    {
                        return $"Hi from {nameof(CallThisFunction)}";
                    }
                }
                
                The end.
                """);

    [Fact]
    public Task CSharp_WithOtherFile()
        => VerifyRenamesAsync(
            input: """
                This is a Razor document.

                <h1>@_other.MyMethod()</h1>

                @code
                {
                    private OtherClass _other;

                    public string MyMethod()
                    {
                        _other.MyMet$$hod();
                        return $"Hi from {nameof(OtherClass.MyMethod)}";
                    }
                }

                The end.
                """,
            additionalFiles: [
                (FilePath("OtherFile.cs"), """
                    public class OtherClass
                    {
                        public void MyMethod()
                        {
                        }
                    }
                    """)
            ],
            newName: "CallThisFunction",
            expected: """
                This is a Razor document.
                
                <h1>@_other.CallThisFunction()</h1>
                
                @code
                {
                    private OtherClass _other;
                
                    public string MyMethod()
                    {
                        _other.CallThisFunction();
                        return $"Hi from {nameof(OtherClass.CallThisFunction)}";
                    }
                }
                
                The end.
                """,
            additionalExpectedFiles: [
                (FileUri("OtherFile.cs"), """
                    public class OtherClass
                    {
                        public void CallThisFunction()
                        {
                        }
                    }
                    """)
            ]);

    [Fact]
    public Task CSharp_Inherits()
       => VerifyRenamesAsync(
           input: """
                @inherits MyComponent$$Base

                This is a Razor document.

                The end.
                """,
           additionalFiles: [
               (FilePath("OtherFile.cs"), """
                    using Microsoft.AspNetCore.Components;

                    public class MyComponentBase : ComponentBase
                    {
                    }
                    """)
           ],
           newName: "OtherName",
           expected: """
                @inherits OtherName

                This is a Razor document.
                
                The end.
                """,
           additionalExpectedFiles: [
               (FileUri("OtherFile.cs"), """
                    using Microsoft.AspNetCore.Components;

                    public class OtherName : ComponentBase
                    {
                    }
                    """)
           ]);

    [Fact]
    public Task CSharp_Model()
        => VerifyRenamesAsync(
            input: """
                @model MyMod$$el

                This is a Razor document.

                The end.
                """,
            additionalFiles: [
                (FilePath("OtherFile.cs"), """
                    public class MyModel
                    {
                    }
                    """)
            ],
            newName: "OtherModel",
            expected: """
                @model OtherModel

                This is a Razor document.
                
                The end.
                """,
            additionalExpectedFiles: [
                (FileUri("OtherFile.cs"), """
                    public class OtherModel
                    {
                    }
                    """)
            ],
            fileKind: RazorFileKind.Legacy);

    [Fact]
    public Task CSharp_Implements()
        => VerifyRenamesAsync(
            input: """
                @implements MyInter$$face

                This is a Razor document.

                The end.
                """,
            additionalFiles: [
                (FilePath("OtherFile.cs"), """
                    public interface MyInterface
                    {
                    }
                    """)
            ],
            newName: "IMyFace",
            expected: """
                @implements IMyFace

                This is a Razor document.
                
                The end.
                """,
            additionalExpectedFiles: [
                (FileUri("OtherFile.cs"), """
                    public interface IMyFace
                    {
                    }
                    """)
            ]);

    [Fact]
    public Task CSharp_TypeParam()
       => VerifyRenamesAsync(
           input: """
                @typeparam TItem where TItem : MyInter$$face

                This is a Razor document.

                The end.
                """,
           additionalFiles: [
               (FilePath("OtherFile.cs"), """
                    public interface MyInterface
                    {
                    }
                    """)
           ],
           newName: "IMyFace",
           expected: """
                @typeparam TItem where TItem : IMyFace

                This is a Razor document.
                
                The end.
                """,
           additionalExpectedFiles: [
               (FileUri("OtherFile.cs"), """
                    public interface IMyFace
                    {
                    }
                    """)
           ]);

    [Fact]
    public Task CSharp_Attribute()
       => VerifyRenamesAsync(
           input: """
                @attribute [HasPa$$nts]

                This is a Razor document.

                The end.
                """,
           additionalFiles: [
               (FilePath("OtherFile.cs"), """
                    public class HasPantsAttribute : Attribute
                    {
                    }
                    """)
           ],
           newName: "HasJacketAttribute",
           expected: """
                @attribute [HasJacket]

                This is a Razor document.
                
                The end.
                """,
           additionalExpectedFiles: [
               (FileUri("OtherFile.cs"), """
                    public class HasJacketAttribute : Attribute
                    {
                    }
                    """)
           ]);

    [Fact]
    public Task CSharp_Attribute_FullName()
       => VerifyRenamesAsync(
           input: """
                @attribute [HasPa$$ntsAttribute]

                This is a Razor document.

                The end.
                """,
           additionalFiles: [
               (FilePath("OtherFile.cs"), """
                    public class HasPantsAttribute : Attribute
                    {
                    }
                    """)
           ],
           newName: "HasJacketAttribute",
           expected: """
                @attribute [HasJacketAttribute]

                This is a Razor document.
                
                The end.
                """,
           additionalExpectedFiles: [
               (FileUri("OtherFile.cs"), """
                    public class HasJacketAttribute : Attribute
                    {
                    }
                    """)
           ]);

    [Fact]
    public Task Component_ExistingFile()
     => VerifyRenamesAsync(
         input: $"""
                This is a Razor document.

                <Comp$$onent />

                The end.
                """,
         additionalFiles: [
             (FilePath("Component.razor"), ""),
             (FilePath("DifferentName.razor"), "")
         ],
         newName: "DifferentName",
         expected: "");

    [Theory]
    [InlineData("$$Component")]
    [InlineData("Com$$ponent")]
    [InlineData("Component$$")]
    public Task Component_StartTag(string startTag)
        => VerifyRenamesAsync(
            input: $"""
                This is a Razor document.

                <Component />

                <div>
                    <{startTag} />
                    <Component>
                    </Component>
                    <div>
                        <Component />
                        <Component>
                        </Component>
                    </div>
                </div>

                The end.
                """,
            additionalFiles: [
                (FilePath("Component.razor"), "")
            ],
            newName: "DifferentName",
            expected: """
                This is a Razor document.

                <DifferentName />
                
                <div>
                    <DifferentName />
                    <DifferentName>
                    </DifferentName>
                    <div>
                        <DifferentName />
                        <DifferentName>
                        </DifferentName>
                    </div>
                </div>

                The end.
                """,
            additionalExpectedFiles:
                [(FileUri("DifferentName.razor"), "")]);

    [Theory]
    [InlineData("$$My.Foo.Component")]
    [InlineData("M$$y.Foo.Component")]
    [InlineData("My$$.Foo.Component")]
    [InlineData("My.$$Foo.Component")]
    [InlineData("My.F$$oo.Component")]
    [InlineData("My.Foo$$.Component")]
    [InlineData("My.Foo.$$Component")]
    [InlineData("My.Foo.Com$$ponent")]
    [InlineData("My.Foo.Component$$")]
    public Task Component_StartTag_FullyQualified(string startTag)
        => VerifyRenamesAsync(
            input: $"""
                This is a Razor document.

                <My.Foo.Component />
                <My.Foo.Component></My.Foo.Component>
                <My.Foo.Component>
                </My.Foo.Component>

                <div>
                    <{startTag} />
                    <My.Foo.Component></My.Foo.Component>
                    <My.Foo.Component>
                    </My.Foo.Component>
                    <div>
                        <My.Foo.Component />
                        <My.Foo.Component></My.Foo.Component>
                        <My.Foo.Component>
                        </My.Foo.Component>
                    </div>
                </div>

                The end.
                """,
            additionalFiles: [
                (FilePath("Component.razor"), """
                    @namespace My.Foo
                    """)
            ],
            newName: "DifferentName",
            expected: """
                This is a Razor document.

                <My.Foo.DifferentName />
                <My.Foo.DifferentName></My.Foo.DifferentName>
                <My.Foo.DifferentName>
                </My.Foo.DifferentName>
                
                <div>
                    <My.Foo.DifferentName />
                    <My.Foo.DifferentName></My.Foo.DifferentName>
                    <My.Foo.DifferentName>
                    </My.Foo.DifferentName>
                    <div>
                        <My.Foo.DifferentName />
                        <My.Foo.DifferentName></My.Foo.DifferentName>
                        <My.Foo.DifferentName>
                        </My.Foo.DifferentName>
                    </div>
                </div>

                The end.
                """,
            additionalExpectedFiles:
                [(FileUri("DifferentName.razor"), """
                    @namespace My.Foo
                    """)]);

    [Theory]
    [InlineData("$$Component")]
    [InlineData("Com$$ponent")]
    [InlineData("Component$$")]
    public Task Component_EndTag(string endTag)
        => VerifyRenamesAsync(
            input: $"""
                This is a Razor document.

                <Component />

                <div>
                    <Component />
                    <Component>
                    </Component>
                    <div>
                        <Component />
                        <Component>
                        </{endTag}>
                    </div>
                </div>

                The end.
                """,
            additionalFiles: [
                (FilePath("Component.razor"), "")
            ],
            newName: "DifferentName",
            expected: """
                This is a Razor document.

                <DifferentName />

                <div>
                    <DifferentName />
                    <DifferentName>
                    </DifferentName>
                    <div>
                        <DifferentName />
                        <DifferentName>
                        </DifferentName>
                    </div>
                </div>

                The end.
                """,
            additionalExpectedFiles:
                [(FileUri("DifferentName.razor"), "")]);

    [Fact]
    public Task Component_Attribute()
        => VerifyRenamesAsync(
            input: $"""
                This is a Razor document.

                <Component Tit$$le="Hello1" />

                <div>
                    <Component Title="Hello2" />
                    <Component Title="Hello3">
                    </Component>
                    <div>
                        <Component Title="Hello4"/>
                        <Component Title="Hello5">
                        </Component>
                    </div>
                </div>

                The end.
                """,
            additionalFiles: [
                (FilePath("Component.razor"), """
                    <div></div>

                    @code {
                        [Parameter]
                        public string Title { get; set; }
                    }

                    """)
            ],
            newName: "Name",
            expected: """
                This is a Razor document.
                
                <Component Name="Hello1" />
                
                <div>
                    <Component Name="Hello2" />
                    <Component Name="Hello3">
                    </Component>
                    <div>
                        <Component Name="Hello4"/>
                        <Component Name="Hello5">
                        </Component>
                    </div>
                </div>
                
                The end.
                """,
             additionalExpectedFiles: [
                (FileUri("Component.razor"), """
                    <div></div>

                    @code {
                        [Parameter]
                        public string Name { get; set; }
                    }

                    """)
            ]);

    [Fact]
    public Task Mvc()
        => VerifyRenamesAsync(
            input: """
                This is a Razor document.

                <Com$$ponent />

                The end.
                """,
            additionalFiles: [
                (FilePath("Component.razor"), "")
            ],
            newName: "DifferentName",
            expected: "",
            fileKind: RazorFileKind.Legacy);

    [Fact]
    public Task Component_WithContent()
      => VerifyRenamesAsync(
          input: $"""
                This is a Razor document.

                <Component>Hello</Compon$$ent>
                <Component>
                    Hello
                </Component>

                The end.
                """,
          additionalFiles: [
              (FilePath("Component.razor"), "")
          ],
          newName: "DifferentName",
          expected: """
                This is a Razor document.

                <DifferentName>Hello</DifferentName>
                <DifferentName>
                    Hello
                </DifferentName>

                The end.
                """,
          additionalExpectedFiles:
                [(FileUri("DifferentName.razor"), "")]);

    [Fact]
    public Task Component_WithContent_FullyQualified()
      => VerifyRenamesAsync(
          input: $"""
                This is a Razor document.

                <My.Namespace.Component>Hello</My.Namespace.Compon$$ent>
                <My.Namespace.Component>
                    Hello
                </My.Namespace.Component>

                The end.
                """,
          additionalFiles: [
              (FilePath("Component.razor"), """
                    @namespace My.Namespace
                    """)
          ],
          newName: "DifferentName",
          expected: """
                This is a Razor document.

                <My.Namespace.DifferentName>Hello</My.Namespace.DifferentName>
                <My.Namespace.DifferentName>
                    Hello
                </My.Namespace.DifferentName>

                The end.
                """,
          additionalExpectedFiles:
                [(FileUri("DifferentName.razor"), """
                    @namespace My.Namespace
                    """)]);

    [Fact]
    public Task Component_WithOtherFile()
        => VerifyRenamesAsync(
            input: $"""
                This is a Razor document.

                <Comp$$onent />
                <Component></Component>
                <Component>
                </Component>

                The end.
                """,
            additionalFiles: [
                (FilePath("Component.razor"), ""),
                (FilePath("OtherComponent.razor"), """
                    <Component />
                    <Component></Component>
                    <Component>
                    </Component>
                    """)
            ],
            newName: "DifferentName",
            expected: """
                This is a Razor document.

                <DifferentName />
                <DifferentName></DifferentName>
                <DifferentName>
                </DifferentName>

                The end.
                """,
            additionalExpectedFiles: [
                (FileUri("DifferentName.razor"), ""),
                (FileUri("OtherComponent.razor"), """
                    <DifferentName />
                    <DifferentName></DifferentName>
                    <DifferentName>
                    </DifferentName>
                    """)
            ]);

    [Fact]
    public Task Component_FullyQualified()
        => VerifyRenamesAsync(
            input: $"""
                This is a Razor document.

                <My.Namespace.Comp$$onent />
                <My.Namespace.Component></My.Namespace.Component>
                <My.Namespace.Component>
                </My.Namespace.Component>

                The end.
                """,
            additionalFiles: [
                (FilePath("Component.razor"), """
                    @namespace My.Namespace
                    """)
            ],
            newName: "DifferentName",
            expected: """
                This is a Razor document.

                <My.Namespace.DifferentName />
                <My.Namespace.DifferentName></My.Namespace.DifferentName>
                <My.Namespace.DifferentName>
                </My.Namespace.DifferentName>

                The end.
                """,
            additionalExpectedFiles:
                [(FileUri("DifferentName.razor"), """
                    @namespace My.Namespace
                    """)]);

    [Fact]
    public Task Component_WithOtherFile_FullyQualified()
        => VerifyRenamesAsync(
            input: $"""
                This is a Razor document.

                <My.Namespace.Comp$$onent />
                <My.Namespace.Component></My.Namespace.Component>
                <My.Namespace.Component>
                </My.Namespace.Component>

                The end.
                """,
            additionalFiles: [
                (FilePath("Component.razor"), """
                    @namespace My.Namespace
                    """),
                (FilePath("OtherComponent.razor"), """
                    <My.Namespace.Component />
                    <My.Namespace.Component></My.Namespace.Component>
                    <My.Namespace.Component>
                    </My.Namespace.Component>
                    """)
            ],
            newName: "DifferentName",
            expected: """
                This is a Razor document.

                <My.Namespace.DifferentName />
                <My.Namespace.DifferentName></My.Namespace.DifferentName>
                <My.Namespace.DifferentName>
                </My.Namespace.DifferentName>

                The end.
                """,
            additionalExpectedFiles: [
                (FileUri("DifferentName.razor"), """
                    @namespace My.Namespace
                    """),
                (FileUri("OtherComponent.razor"), """
                    <My.Namespace.DifferentName />
                    <My.Namespace.DifferentName></My.Namespace.DifferentName>
                    <My.Namespace.DifferentName>
                    </My.Namespace.DifferentName>
                    """)
            ]);

    [Fact]
    public Task Component_OwnFile()
        => VerifyRenamesAsync(
            input: $"""
                This is a Razor document.

                <Fi$$le1 />
                <File1></File1>
                <File1>
                </File1>

                The end.
                """,
            newName: "ABetterName",
            expected: """
                This is a Razor document.

                <ABetterName />
                <ABetterName></ABetterName>
                <ABetterName>
                </ABetterName>

                The end.
                """,
            newFileUri: FileUri("ABetterName.razor"));

    [Fact]
    public Task Component_WithOtherFile_OwnFile()
    => VerifyRenamesAsync(
        input: $"""
                This is a Razor document.

                <Comp$$onent />
                <Component></Component>
                <Component>
                </Component>

                The end.
                """,
        additionalFiles: [
            (FilePath("Component.razor"), """
                <Component />
                <Component></Component>
                <Component>
                </Component>
                """),
            (FilePath("OtherComponent.razor"), """
                <Component />
                <Component></Component>
                <Component>
                </Component>
                """)
        ],
        newName: "DifferentName",
        expected: """
                This is a Razor document.

                <DifferentName />
                <DifferentName></DifferentName>
                <DifferentName>
                </DifferentName>

                The end.
                """,
        additionalExpectedFiles: [
            (FileUri("DifferentName.razor"), """
                <DifferentName />
                <DifferentName></DifferentName>
                <DifferentName>
                </DifferentName>
                """),
            (FileUri("OtherComponent.razor"), """
                <DifferentName />
                <DifferentName></DifferentName>
                <DifferentName>
                </DifferentName>
                """)
        ]);

    [Fact]
    public Task Component_OwnFile_WithCss()
        => VerifyRenamesAsync(
            input: $"""
                This is a Razor document.

                <Fi$$le1 />
                <File1></File1>
                <File1>
                </File1>

                The end.
                """,
            additionalFiles: [
                (FilePath("File1.razor.css"), "")
            ],
            newName: "ABetterName",
            expected: """
                This is a Razor document.

                <ABetterName />
                <ABetterName></ABetterName>
                <ABetterName>
                </ABetterName>

                The end.
                """,
            newFileUri: FileUri("ABetterName.razor"),
            additionalExpectedFiles: [
                (FileUri("ABetterName.razor.css"), "")]);

    [Fact]
    public Task Component_OwnFile_WithCodeBehind()
        => VerifyRenamesAsync(
            input: $"""
                This is a Razor document.

                <Fi$$le1 />
                <File1></File1>
                <File1>
                </File1>

                The end.
                """,
            additionalFiles: [
                (FilePath("File1.razor.cs"), """
                    namespace SomeProject;

                    // This class name should change, but we don't support that yet
                    public partial class File1
                    {
                    }
                    """)
            ],
            newName: "ABetterName",
            expected: """
                This is a Razor document.

                <ABetterName />
                <ABetterName></ABetterName>
                <ABetterName>
                </ABetterName>

                The end.
                """,
            newFileUri: FileUri("ABetterName.razor"),
            additionalExpectedFiles: [
                (FileUri("ABetterName.razor.cs"), """
                namespace SomeProject;

                // This class name should change, but we don't support that yet
                public partial class File1
                {
                }
                """)]);

    [Fact]
    public Task Component_WithOtherFile_WithCss()
        => VerifyRenamesAsync(
            input: $"""
                This is a Razor document.

                <Comp$$onent />
                <Component></Component>
                <Component>
                </Component>

                The end.
                """,
            additionalFiles: [
                (FilePath("Component.razor.css"), ""),
                (FilePath("Component.razor"), """
                    <Component />
                    <Component></Component>
                    <Component>
                    </Component>
                    """)
            ],
            newName: "DifferentName",
            expected: """
                This is a Razor document.

                <DifferentName />
                <DifferentName></DifferentName>
                <DifferentName>
                </DifferentName>

                The end.
                """,
            additionalExpectedFiles: [
                (FileUri("DifferentName.razor.css"), ""),
                (FileUri("DifferentName.razor"), """
                    <DifferentName />
                    <DifferentName></DifferentName>
                    <DifferentName>
                    </DifferentName>
                    """),
            ]);

    [Fact]
    public Task Component_WithOtherFile_WithCodeBehindAndCss()
        => VerifyRenamesAsync(
            input: $"""
                This is a Razor document.

                <Comp$$onent />
                <Component></Component>
                <Component>
                </Component>

                The end.
                """,
            additionalFiles: [
                (FilePath("Component.razor.css"), ""),
                (FilePath("Component.razor.cs"), """
                    namespace SomeProject;

                    // This class name should change, but we don't support that yet
                    public partial class Component
                    {
                    }
                    """),
                (FilePath("Component.razor"), """
                    <Component />
                    <Component></Component>
                    <Component>
                    </Component>
                    """)
            ],
            newName: "DifferentName",
            expected: """
                This is a Razor document.

                <DifferentName />
                <DifferentName></DifferentName>
                <DifferentName>
                </DifferentName>

                The end.
                """,
            additionalExpectedFiles: [
                (FileUri("DifferentName.razor.css"), ""),
                (FileUri("DifferentName.razor.cs"), """
                    namespace SomeProject;

                    // This class name should change, but we don't support that yet
                    public partial class Component
                    {
                    }
                    """),
                (FileUri("DifferentName.razor"), """
                    <DifferentName />
                    <DifferentName></DifferentName>
                    <DifferentName>
                    </DifferentName>
                    """),
            ]);

    private async Task VerifyRenamesAsync(
        string input,
        string newName,
        string expected,
        RazorFileKind? fileKind = null,
        Uri? newFileUri = null,
        (string fileName, string contents)[]? additionalFiles = null,
        (Uri fileUri, string contents)[]? additionalExpectedFiles = null)
    {
        TestFileMarkupParser.GetPosition(input, out var source, out var cursorPosition);
        var document = CreateProjectAndRazorDocument(source, fileKind, additionalFiles: additionalFiles);
        var inputText = await document.GetTextAsync(DisposalToken);
        var position = inputText.GetPosition(cursorPosition);

        var requestInvoker = new TestHtmlRequestInvoker([(Methods.TextDocumentRenameName, (object?)null)]);

        var fileSystem = (RemoteFileSystem)OOPExportProvider.GetExportedValue<IFileSystem>();
        fileSystem.GetTestAccessor().SetFileSystem(new TestFileSystem(additionalFiles));

        var endpoint = new CohostRenameEndpoint(IncompatibleProjectService, RemoteServiceInvoker, requestInvoker);

        var renameParams = new RenameParams
        {
            Position = position,
            TextDocument = new TextDocumentIdentifier { DocumentUri = document.CreateDocumentUri() },
            NewName = newName,
        };

        var result = await endpoint.GetTestAccessor().HandleRequestAsync(renameParams, document, DisposalToken);

        if (expected.Length == 0)
        {
            Assert.Null(result);
            return;
        }

        Assert.NotNull(result);

        var documentUri = newFileUri ?? document.CreateUri();
        var expectedChanges = (additionalExpectedFiles ?? []).Concat([(documentUri, expected)]);
        await result.AssertWorkspaceEditAsync(document.Project.Solution, expectedChanges, DisposalToken);
    }
}
