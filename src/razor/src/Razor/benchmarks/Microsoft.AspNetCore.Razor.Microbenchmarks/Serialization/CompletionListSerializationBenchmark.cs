// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.IO;
using System.Text;
using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Legacy;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.LanguageServer.Completion;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Newtonsoft.Json;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks.Serialization;

public class CompletionListSerializationBenchmark
{
    private readonly byte[] _completionListBuffer;

    private readonly JsonSerializer _serializer;
    private readonly CompletionList _completionList;

    public CompletionListSerializationBenchmark()
    {
        var tagHelperFactsService = new TagHelperFactsService();
        var completionService = new LanguageServerTagHelperCompletionService(tagHelperFactsService);
        var htmlFactsService = new DefaultHtmlFactsService();
        var optionsMonitor = new BenchmarkOptionsMonitor<RazorLSPOptions>(RazorLSPOptions.Default);
        var tagHelperCompletionProvider = new TagHelperCompletionProvider(completionService, htmlFactsService, tagHelperFactsService, optionsMonitor);

        _serializer = JsonSerializer.Create();

        var documentContent = "<";
        var queryIndex = 1;
        _completionList = GenerateCompletionList(documentContent, queryIndex, tagHelperCompletionProvider);
        _completionListBuffer = GenerateBuffer(_completionList);
    }

    [Benchmark(Description = "Component Completion List Roundtrip Serialization")]
    public void ComponentElement_CompletionList_Serialization_RoundTrip()
    {
        // Serialize back to json.
        MemoryStream originalStream;
        using (originalStream = new MemoryStream())
        using (var writer = new StreamWriter(originalStream, Encoding.UTF8, bufferSize: 4096))
        {
            _serializer.Serialize(writer, _completionList);
        }

        CompletionList deserializedCompletions;
        var stream = new MemoryStream(originalStream.GetBuffer());
        using (stream)
        using (var reader = new JsonTextReader(new StreamReader(stream)))
        {
            deserializedCompletions = _serializer.Deserialize<CompletionList>(reader).AssumeNotNull();
        }
    }

    [Benchmark(Description = "Component Completion List Serialization")]
    public void ComponentElement_CompletionList_Serialization()
    {
        using var stream = new MemoryStream();
        using var writer = new StreamWriter(stream, Encoding.UTF8, bufferSize: 4096);
        _serializer.Serialize(writer, _completionList);
    }

    [Benchmark(Description = "Component Completion List Deserialization")]
    public void ComponentElement_CompletionList_Deserialization()
    {
        // Deserialize from json file.
        using var stream = new MemoryStream(_completionListBuffer);
        using var reader = new JsonTextReader(new StreamReader(stream));
        CompletionList deserializedCompletions;
        deserializedCompletions = _serializer.Deserialize<CompletionList>(reader).AssumeNotNull();
    }

    private CompletionList GenerateCompletionList(string documentContent, int queryIndex, TagHelperCompletionProvider componentCompletionProvider)
    {
        var sourceDocument = RazorSourceDocument.Create(documentContent, RazorSourceDocumentProperties.Default);
        var syntaxTree = RazorSyntaxTree.Parse(sourceDocument);
        var tagHelperDocumentContext = TagHelperDocumentContext.Create(prefix: string.Empty, CommonResources.LegacyTagHelpers);

        var queryableChange = new SourceChange(queryIndex, length: 0, newText: string.Empty);
        var owner = syntaxTree.Root.LocateOwner(queryableChange);
        var context = new RazorCompletionContext(queryIndex, owner, syntaxTree, tagHelperDocumentContext);

        var razorCompletionItems = componentCompletionProvider.GetCompletionItems(context);
        var completionList = RazorCompletionListProvider.CreateLSPCompletionList(
            razorCompletionItems,
            new VSInternalClientCapabilities()
            {
                TextDocument = new TextDocumentClientCapabilities()
                {
                    Completion = new VSInternalCompletionSetting()
                    {
                        CompletionItemKind = new CompletionItemKindSetting()
                        {
                            ValueSet = new[] { CompletionItemKind.TagHelper }
                        },
                        CompletionList = new VSInternalCompletionListSetting()
                        {
                            CommitCharacters = true,
                            Data = true,
                        }
                    }
                }
            });
        return completionList;
    }

    private byte[] GenerateBuffer(CompletionList completionList)
    {
        using var stream = new MemoryStream();
        using var writer = new StreamWriter(stream, Encoding.UTF8, bufferSize: 4096);
        _serializer.Serialize(writer, completionList);
        var buffer = stream.GetBuffer();

        return buffer;
    }
}
