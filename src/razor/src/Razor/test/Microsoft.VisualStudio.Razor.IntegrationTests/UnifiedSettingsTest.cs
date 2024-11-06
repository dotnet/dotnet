// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.IO;
using System.Reflection;
using System.Text.Json;
using Xunit;

namespace Microsoft.VisualStudio.Razor.IntegrationTests;

public class UnifiedSettingsTest
{
    [Fact]
    public void TestJsonIsValid()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "Microsoft.VisualStudio.Razor.IntegrationTests.razor.registration.json";

        using var stream = assembly.GetManifestResourceStream(resourceName);
        using var reader = new StreamReader(stream);
        var json = reader.ReadToEnd();

        Assert.False(string.IsNullOrEmpty(json));

        var options = new JsonDocumentOptions
        {
            CommentHandling = JsonCommentHandling.Skip
        };
        var document = JsonDocument.Parse(json, options);
        Assert.NotNull(document);
    }
}
