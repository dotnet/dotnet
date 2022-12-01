// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.IO;
using System.Linq;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Newtonsoft.Json;
using Xunit;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Refactoring.Test;

public class PlatformAgnosticClientCapabilitiesJsonConverterTest
{
    [Fact]
    public void ReadJson_ReadsValues()
    {
        // Arrange
        // Note this is a small subset of the actual ClientCapabilities provided
        // for use in basic validations.
        var rawJson = @"{
  ""workspace"": {
    ""applyEdit"": true,
    ""workspaceEdit"": {
      ""documentChanges"": true
    }
  },
  ""textDocument"": {
    ""_vs_onAutoInsert"": {
      ""dynamicRegistration"": false
    },
    ""synchronization"": {
      ""willSave"": false,
      ""willSaveWaitUntil"": false,
      ""didSave"": true,
      ""dynamicRegistration"": false
    },
    ""completion"": {
      ""completionItem"": {
        ""snippetSupport"": false,
        ""commitCharactersSupport"": true
      },
      ""completionItemKind"": {
        ""valueSet"": [
          3
        ]
      },
      ""contextSupport"": false,
      ""dynamicRegistration"": false
    },
    ""hover"": {
      ""contentFormat"": [
        ""plaintext""
      ],
      ""dynamicRegistration"": false
    },
    ""signatureHelp"": {
      ""signatureInformation"": {
        ""documentationFormat"": [
          ""plaintext""
        ]
      },
      ""contextSupport"": true,
      ""dynamicRegistration"": false
    },
    ""codeAction"": {
      ""codeActionLiteralSupport"": {
        ""codeActionKind"": {
          ""valueSet"": [
            ""refactor.extract""
          ]
        }
      },
      ""dynamicRegistration"": false
    }
  }
}";
        var stringReader = new StringReader(rawJson);

        // Act
        var capabilities = JsonSerializer.CreateDefault().Deserialize<VSInternalClientCapabilities>(new JsonTextReader(stringReader));

        // Assert
        Assert.True(capabilities.Workspace.ApplyEdit);
        Assert.Equal(MarkupKind.PlainText, capabilities.TextDocument.Hover.ContentFormat.First());
        Assert.Equal(CompletionItemKind.Function, capabilities.TextDocument.Completion.CompletionItemKind.ValueSet.First());
        Assert.Equal(MarkupKind.PlainText, capabilities.TextDocument.SignatureHelp.SignatureInformation.DocumentationFormat.First());
        Assert.Equal(CodeActionKind.RefactorExtract, capabilities.TextDocument.CodeAction.CodeActionLiteralSupport.CodeActionKind.ValueSet.First());
    }
}
