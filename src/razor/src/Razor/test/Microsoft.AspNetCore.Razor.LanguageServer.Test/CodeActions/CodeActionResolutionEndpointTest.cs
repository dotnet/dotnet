// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

public class CodeActionResolutionEndpointTest : LanguageServerTestBase
{
    public CodeActionResolutionEndpointTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public async Task Handle_Valid_RazorCodeAction_WithResolver()
    {
        // Arrange
        var codeActionEndpoint = new CodeActionResolutionEndpoint(
            new RazorCodeActionResolver[] {
                new MockRazorCodeActionResolver("Test"),
            },
            Array.Empty<CSharpCodeActionResolver>(),
            Array.Empty<HtmlCodeActionResolver>(),
            LoggerFactory);
        var requestParams = new RazorCodeActionResolutionParams()
        {
            Action = "Test",
            Language = LanguageServerConstants.CodeActions.Languages.Razor,
            Data = new AddUsingsCodeActionParams()
            {
                Namespace = "Test",
                Uri = new Uri("C:/path/to/Page.razor")
            }
        };
        var request = new CodeAction()
        {
            Title = "Valid request",
            Data = JToken.FromObject(requestParams)
        };
        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act
        var razorCodeAction = await codeActionEndpoint.HandleRequestAsync(request, requestContext, default);

        // Assert
        Assert.NotNull(razorCodeAction.Edit);
    }

    [Fact]
    public async Task Handle_Valid_CSharpCodeAction_WithResolver()
    {
        // Arrange
        var codeActionEndpoint = new CodeActionResolutionEndpoint(
            Array.Empty<RazorCodeActionResolver>(),
            new CSharpCodeActionResolver[] {
                new MockCSharpCodeActionResolver("Test"),
            },
            Array.Empty<HtmlCodeActionResolver>(),
            LoggerFactory);
        var requestParams = new RazorCodeActionResolutionParams()
        {
            Action = "Test",
            Language = LanguageServerConstants.CodeActions.Languages.CSharp,
            Data = JObject.FromObject(new CodeActionResolveParams()
            {
                RazorFileUri = new Uri("C:/path/to/Page.razor"),
            })
        };
        var request = new CodeAction()
        {
            Title = "Valid request",
            Data = JToken.FromObject(requestParams)
        };
        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act
        var razorCodeAction = await codeActionEndpoint.HandleRequestAsync(request, requestContext, default);

        // Assert
        Assert.NotNull(razorCodeAction.Edit);
    }

    [Fact]
    public async Task Handle_Valid_CSharpCodeAction_WithMultipleLanguageResolvers()
    {
        // Arrange
        var codeActionEndpoint = new CodeActionResolutionEndpoint(
            new RazorCodeActionResolver[] {
                new MockRazorCodeActionResolver("TestRazor"),
            },
            new CSharpCodeActionResolver[] {
                new MockCSharpCodeActionResolver("TestCSharp"),
            },
            Array.Empty<HtmlCodeActionResolver>(),
            LoggerFactory);
        var requestParams = new RazorCodeActionResolutionParams()
        {
            Action = "TestCSharp",
            Language = LanguageServerConstants.CodeActions.Languages.CSharp,
            Data = JObject.FromObject(new CodeActionResolveParams()
            {
                RazorFileUri = new Uri("C:/path/to/Page.razor"),
            })
        };
        var request = new CodeAction()
        {
            Title = "Valid request",
            Data = JToken.FromObject(requestParams)
        };
        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act
        var razorCodeAction = await codeActionEndpoint.HandleRequestAsync(request, requestContext, default);

        // Assert
        Assert.NotNull(razorCodeAction.Edit);
    }

    [Fact(Skip = "Debug.Fail fails in CI")]
    public async Task Handle_Valid_RazorCodeAction_WithoutResolver()
    {
        // Arrange
        var codeActionEndpoint = new CodeActionResolutionEndpoint(
            Array.Empty<RazorCodeActionResolver>(),
            Array.Empty<CSharpCodeActionResolver>(),
            Array.Empty<HtmlCodeActionResolver>(),
            LoggerFactory);
        var requestParams = new RazorCodeActionResolutionParams()
        {
            Action = "Test",
            Language = LanguageServerConstants.CodeActions.Languages.Razor,
            Data = new AddUsingsCodeActionParams()
            {
                Namespace = "Test",
                Uri = new Uri("C:/path/to/Page.razor")
            }
        };
        var request = new CodeAction()
        {
            Title = "Valid request",
            Data = JToken.FromObject(requestParams)
        };
        var requestContext = CreateRazorRequestContext(documentContext: null);

#if DEBUG
        // Act & Assert (Throws due to debug assert on no Razor.Test resolver)
        await Assert.ThrowsAnyAsync<Exception>(async () => await codeActionEndpoint.HandleRequestAsync(request, requestContext, default));
#else
        // Act
        var resolvedCodeAction = await codeActionEndpoint.HandleRequestAsync(request, requestContext, default);

        // Assert
        Assert.Null(resolvedCodeAction.Edit);
#endif
    }

    [Fact(Skip = "Debug.Fail fails in CI")]
    public async Task Handle_Valid_CSharpCodeAction_WithoutResolver()
    {
        // Arrange
        var codeActionEndpoint = new CodeActionResolutionEndpoint(
            Array.Empty<RazorCodeActionResolver>(),
            Array.Empty<CSharpCodeActionResolver>(),
            Array.Empty<HtmlCodeActionResolver>(),
            LoggerFactory);
        var requestParams = new RazorCodeActionResolutionParams()
        {
            Action = "Test",
            Language = LanguageServerConstants.CodeActions.Languages.CSharp,
            Data = JObject.FromObject(new CodeActionResolveParams()
            {
                RazorFileUri = new Uri("C:/path/to/Page.razor"),
            })
        };
        var request = new CodeAction()
        {
            Title = "Valid request",
            Data = JToken.FromObject(requestParams)
        };
        var requestContext = CreateRazorRequestContext(documentContext: null);

#if DEBUG
        // Act & Assert (Throws due to debug assert on no resolver registered for CSharp.Test)
        await Assert.ThrowsAnyAsync<Exception>(async () => await codeActionEndpoint.HandleRequestAsync(request, requestContext, default));
#else
        // Act
        var resolvedCodeAction = await codeActionEndpoint.HandleRequestAsync(request, requestContext, default);

        // Assert
        Assert.Null(resolvedCodeAction.Edit);
#endif
    }

    [Fact(Skip = "Debug.Fail fails in CI")]
    public async Task Handle_Valid_RazorCodeAction_WithCSharpResolver_ResolvesNull()
    {
        // Arrange
        var codeActionEndpoint = new CodeActionResolutionEndpoint(
            Array.Empty<RazorCodeActionResolver>(),
            new CSharpCodeActionResolver[] {
                new MockCSharpCodeActionResolver("Test"),
            },
            Array.Empty<HtmlCodeActionResolver>(),
            LoggerFactory);
        var requestParams = new RazorCodeActionResolutionParams()
        {
            Action = "Test",
            Language = LanguageServerConstants.CodeActions.Languages.Razor,
            Data = new AddUsingsCodeActionParams()
            {
                Namespace = "Test",
                Uri = new Uri("C:/path/to/Page.razor")
            }
        };
        var request = new CodeAction()
        {
            Title = "Valid request",
            Data = JToken.FromObject(requestParams)
        };
        var requestContext = CreateRazorRequestContext(documentContext: null);

#if DEBUG
        // Act & Assert (Throws due to debug assert on no resolver registered for Razor.Test)
        await Assert.ThrowsAnyAsync<Exception>(async () => await codeActionEndpoint.HandleRequestAsync(request, requestContext, default));
#else
        // Act
        var resolvedCodeAction = await codeActionEndpoint.HandleRequestAsync(request, requestContext, default);

        // Assert
        Assert.Null(resolvedCodeAction.Edit);
#endif
    }

    [Fact(Skip = "Debug.Fail fails in CI")]
    public async Task Handle_Valid_CSharpCodeAction_WithRazorResolver_ResolvesNull()
    {
        // Arrange
        var codeActionEndpoint = new CodeActionResolutionEndpoint(
            new RazorCodeActionResolver[] {
                new MockRazorCodeActionResolver("Test"),
            },
            Array.Empty<CSharpCodeActionResolver>(),
            Array.Empty<HtmlCodeActionResolver>(),
            LoggerFactory);
        var requestParams = new RazorCodeActionResolutionParams()
        {
            Action = "Test",
            Language = LanguageServerConstants.CodeActions.Languages.CSharp,
            Data = JObject.FromObject(new CodeActionResolveParams()
            {
                RazorFileUri = new Uri("C:/path/to/Page.razor"),
            })
        };
        var request = new CodeAction()
        {
            Title = "Valid request",
            Data = JToken.FromObject(requestParams)
        };
        var requestContext = CreateRazorRequestContext(documentContext: null);

#if DEBUG
        // Act & Assert (Throws due to debug asserts)
        await Assert.ThrowsAnyAsync<Exception>(async () => await codeActionEndpoint.HandleRequestAsync(request, requestContext, default));
#else
        // Act
        var resolvedCodeAction = await codeActionEndpoint.HandleRequestAsync(request, requestContext, default);

        // Assert
        Assert.Null(resolvedCodeAction.Edit);
#endif
    }

    [Fact]
    public async Task ResolveRazorCodeAction_ResolveMultipleRazorProviders_FirstMatches()
    {
        // Arrange
        var codeActionEndpoint = new CodeActionResolutionEndpoint(
                new RazorCodeActionResolver[] {
                    new MockRazorCodeActionResolver("A"),
                    new MockRazorNullCodeActionResolver("B"),
            },
            Array.Empty<CSharpCodeActionResolver>(),
            Array.Empty<HtmlCodeActionResolver>(),
            LoggerFactory);
        var codeAction = new CodeAction();
        var request = new RazorCodeActionResolutionParams()
        {
            Action = "A",
            Language = LanguageServerConstants.CodeActions.Languages.Razor,
            Data = JToken.FromObject(new AddUsingsCodeActionParams()
            {
                Namespace = "Test",
                Uri = new Uri("C:/path/to/Page.razor")
            }),
        };

        // Act
        var resolvedCodeAction = await codeActionEndpoint.ResolveRazorCodeActionAsync(codeAction, request, default);

        // Assert
        Assert.NotNull(resolvedCodeAction.Edit);
    }

    [Fact]
    public async Task ResolveRazorCodeAction_ResolveMultipleRazorProviders_SecondMatches()
    {
        // Arrange
        var codeActionEndpoint = new CodeActionResolutionEndpoint(
            new RazorCodeActionResolver[] {
                new MockRazorNullCodeActionResolver("A"),
                new MockRazorCodeActionResolver("B"),
            },
            Array.Empty<CSharpCodeActionResolver>(),
            Array.Empty<HtmlCodeActionResolver>(),
            LoggerFactory);
        var codeAction = new CodeAction();
        var request = new RazorCodeActionResolutionParams()
        {
            Action = "B",
            Language = LanguageServerConstants.CodeActions.Languages.Razor,
            Data = JToken.FromObject(new AddUsingsCodeActionParams()
            {
                Namespace = "Test",
                Uri = new Uri("C:/path/to/Page.razor")
            })
        };

        // Act
        var resolvedCodeAction = await codeActionEndpoint.ResolveRazorCodeActionAsync(codeAction, request, default);

        // Assert
        Assert.NotNull(resolvedCodeAction.Edit);
    }

    [Fact]
    public async Task ResolveCSharpCodeAction_ResolveMultipleCSharpProviders_FirstMatches()
    {
        // Arrange
        var codeActionEndpoint = new CodeActionResolutionEndpoint(
            Array.Empty<RazorCodeActionResolver>(),
            new CSharpCodeActionResolver[] {
                new MockCSharpCodeActionResolver("A"),
                new MockCSharpNullCodeActionResolver("B"),
            },
            Array.Empty<HtmlCodeActionResolver>(),
            LoggerFactory);
        var codeAction = new CodeAction();
        var request = new RazorCodeActionResolutionParams()
        {
            Action = "A",
            Language = LanguageServerConstants.CodeActions.Languages.CSharp,
            Data = JObject.FromObject(new CodeActionResolveParams()
            {
                RazorFileUri = new Uri("C:/path/to/Page.razor"),
            })
        };

        // Act
        var resolvedCodeAction = await codeActionEndpoint.ResolveCSharpCodeActionAsync(codeAction, request, default);

        // Assert
        Assert.NotNull(resolvedCodeAction.Edit);
    }

    [Fact]
    public async Task ResolveCSharpCodeAction_ResolveMultipleCSharpProviders_SecondMatches()
    {
        // Arrange
        var codeActionEndpoint = new CodeActionResolutionEndpoint(
            Array.Empty<RazorCodeActionResolver>(),
            new CSharpCodeActionResolver[] {
                new MockCSharpNullCodeActionResolver("A"),
                new MockCSharpCodeActionResolver("B"),
            },
            Array.Empty<HtmlCodeActionResolver>(),
            LoggerFactory);
        var codeAction = new CodeAction();
        var request = new RazorCodeActionResolutionParams()
        {
            Action = "B",
            Language = LanguageServerConstants.CodeActions.Languages.Razor,
            Data = JObject.FromObject(new CodeActionResolveParams()
            {
                RazorFileUri = new Uri("C:/path/to/Page.razor"),
            })
        };

        // Act
        var resolvedCodeAction = await codeActionEndpoint.ResolveCSharpCodeActionAsync(codeAction, request, default);

        // Assert
        Assert.NotNull(resolvedCodeAction.Edit);
    }

    [Fact]
    public async Task ResolveCSharpCodeAction_ResolveMultipleLanguageProviders()
    {
        // Arrange
        var codeActionEndpoint = new CodeActionResolutionEndpoint(
            new RazorCodeActionResolver[] {
                new MockRazorNullCodeActionResolver("A"),
                new MockRazorCodeActionResolver("B"),
            },
            new CSharpCodeActionResolver[] {
                new MockCSharpNullCodeActionResolver("C"),
                new MockCSharpCodeActionResolver("D"),
            },
            Array.Empty<HtmlCodeActionResolver>(),
            LoggerFactory);
        var codeAction = new CodeAction();
        var request = new RazorCodeActionResolutionParams()
        {
            Action = "D",
            Language = LanguageServerConstants.CodeActions.Languages.CSharp,
            Data = JObject.FromObject(new CodeActionResolveParams()
            {
                RazorFileUri = new Uri("C:/path/to/Page.razor"),
            })
        };

        // Act
        var resolvedCodeAction = await codeActionEndpoint.ResolveCSharpCodeActionAsync(codeAction, request, default);

        // Assert
        Assert.NotNull(resolvedCodeAction.Edit);
    }

    [Fact]
    public async Task Handle_ResolveEditBasedCodeActionCommand()
    {
        // Arrange
        var codeActionEndpoint = new CodeActionResolutionEndpoint(
            Array.Empty<RazorCodeActionResolver>(),
            new CSharpCodeActionResolver[] {
                new MockCSharpCodeActionResolver("Test"),
            },
            Array.Empty<HtmlCodeActionResolver>(),
            LoggerFactory);
        var requestParams = new RazorCodeActionResolutionParams()
        {
            Action = LanguageServerConstants.CodeActions.EditBasedCodeActionCommand,
            Language = LanguageServerConstants.CodeActions.Languages.Razor,
            Data = JToken.FromObject(new WorkspaceEdit())
        };

        var request = new CodeAction()
        {
            Title = "Valid request",
            Data = JToken.FromObject(requestParams)
        };
        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act
        var razorCodeAction = await codeActionEndpoint.HandleRequestAsync(request, requestContext, default);

        // Assert
        Assert.NotNull(razorCodeAction.Edit);
    }

    private class MockRazorCodeActionResolver : RazorCodeActionResolver
    {
        public override string Action { get; }

        internal MockRazorCodeActionResolver(string action)
        {
            Action = action;
        }

        public override Task<WorkspaceEdit?> ResolveAsync(JObject data, CancellationToken cancellationToken)
        {
            return Task.FromResult<WorkspaceEdit?>(new WorkspaceEdit());
        }
    }

    private class MockRazorNullCodeActionResolver : RazorCodeActionResolver
    {
        public override string Action { get; }

        internal MockRazorNullCodeActionResolver(string action)
        {
            Action = action;
        }

        public override Task<WorkspaceEdit?> ResolveAsync(JObject data, CancellationToken cancellationToken)
        {
            return Task.FromResult<WorkspaceEdit?>(null);
        }
    }

    private class MockCSharpCodeActionResolver : CSharpCodeActionResolver
    {
        public override string Action { get; }

        internal MockCSharpCodeActionResolver(string action)
            : base(Mock.Of<ClientNotifierServiceBase>(MockBehavior.Strict))
        {
            Action = action;
        }

        public override Task<CodeAction> ResolveAsync(CodeActionResolveParams csharpParams, CodeAction codeAction, CancellationToken cancellationToken)
        {
            codeAction.Edit = new WorkspaceEdit();
            return Task.FromResult(codeAction);
        }
    }

    private class MockCSharpNullCodeActionResolver : CSharpCodeActionResolver
    {
        public override string Action { get; }

        internal MockCSharpNullCodeActionResolver(string action)
            : base(Mock.Of<ClientNotifierServiceBase>(MockBehavior.Strict))
        {
            Action = action;
        }

        public override Task<CodeAction> ResolveAsync(CodeActionResolveParams csharpParams, CodeAction codeAction, CancellationToken cancellationToken)
        {
            // This is deliberately returning null when it's not supposed to, so that if this code action
            // is ever returned by a method, the test will fail
            return Task.FromResult<CodeAction>(null!);
        }
    }
}
