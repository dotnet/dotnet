// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectEngineHost.Serialization;
using Newtonsoft.Json;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks;

public abstract class TagHelperBenchmarkBase
{
    protected readonly byte[] TagHelperBuffer;

    protected readonly IReadOnlyList<TagHelperDescriptor> DefaultTagHelpers;
    protected readonly JsonSerializer DefaultSerializer;

    protected TagHelperBenchmarkBase()
    {
        TagHelperBuffer = Resources.GetResourceBytes("taghelpers.json");

        // Deserialize from json file.
        TagHelperDescriptorJsonConverter.DisableCachingForTesting = true;
        DefaultSerializer = new JsonSerializer();
        DefaultSerializer.Converters.Add(TagHelperDescriptorJsonConverter.Instance);

        using var stream = new MemoryStream(TagHelperBuffer);
        using var reader = new JsonTextReader(new StreamReader(stream));
        DefaultTagHelpers = DefaultSerializer.Deserialize<IReadOnlyList<TagHelperDescriptor>>(reader).AssumeNotNull();
        TagHelperDescriptorJsonConverter.DisableCachingForTesting = false;
    }
}
