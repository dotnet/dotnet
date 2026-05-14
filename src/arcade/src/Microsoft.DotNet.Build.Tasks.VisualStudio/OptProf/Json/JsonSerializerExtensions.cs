// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Newtonsoft.Json;
using System.IO;

namespace Microsoft.DotNet.Build.Tasks.VisualStudio
{
    internal static class JsonSerializerExtensions
    {
        public static T Deserialize<T>(this JsonSerializer serializer, TextReader reader) 
            => (T)serializer.Deserialize(reader, typeof(T));
    }
}
