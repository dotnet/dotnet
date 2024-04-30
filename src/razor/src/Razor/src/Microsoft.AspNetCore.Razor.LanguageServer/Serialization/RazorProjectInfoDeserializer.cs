// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.IO;
using Microsoft.AspNetCore.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Serialization;

internal sealed class RazorProjectInfoDeserializer : IRazorProjectInfoDeserializer
{
    public static readonly IRazorProjectInfoDeserializer Instance = new RazorProjectInfoDeserializer();

    private RazorProjectInfoDeserializer()
    {
    }

    public RazorProjectInfo? DeserializeFromString(string? base64String)
    {
        RazorProjectInfo? razorProjectInfo = null;

        // ProjectInfo will be null if project is being deleted and should be removed
        if (base64String is not null)
        {
            var projectInfoBytes = Convert.FromBase64String(base64String);
            using var stream = new MemoryStream(projectInfoBytes);
            razorProjectInfo = DeserializeFromStream(stream);
        }

        return razorProjectInfo;
    }

    public RazorProjectInfo? DeserializeFromFile(string filePath)
    {
        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);

        return DeserializeFromStream(stream);
    }

    public RazorProjectInfo? DeserializeFromStream(Stream stream)
    {
        try
        {
            return RazorProjectInfo.DeserializeFrom(stream);
        }
        catch
        {
            // Swallow deserialization exceptions. There's many reasons they can happen, all out of our control.
            return null;
        }
    }
}
