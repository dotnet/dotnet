// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if JSONSERIALIZATION_PROJECTSYSTEM
using System.IO;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.Serialization.Json;

internal static partial class JsonDataConvert
{
    public static void Serialize(RazorProjectInfo value, TextWriter writer, bool indented = false)
        => SerializeObject(value, writer, indented, ObjectWriters.WriteProperties);

    public static string Serialize(RazorProjectInfo value, bool indented = false)
        => SerializeObject(value, indented, ObjectWriters.WriteProperties);

    public static void SerializeToFile(RazorProjectInfo value, string filePath, bool indented = false)
    {
        using var writer = new StreamWriter(filePath);
        SerializeObject(value, writer, indented, ObjectWriters.WriteProperties);
    }

    public static RazorProjectInfo DeserializeProjectInfo(TextReader reader)
        => DeserializeNonNullObject(reader, ObjectReaders.ReadProjectInfoFromProperties);

    public static RazorProjectInfo DeserializeProjectInfo(string json)
        => DeserializeNonNullObject(json, ObjectReaders.ReadProjectInfoFromProperties);

    public static RazorProjectInfo DeserializeProjectInfo(byte[] bytes)
        => DeserializeNonNullObject(bytes, ObjectReaders.ReadProjectInfoFromProperties);
}
#endif
