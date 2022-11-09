// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NuGet.Protocol
{
    internal static class JsonUtility
    {
        /// <summary>
        /// JsonLoadSettings with line info and comments ignored.
        /// </summary>
        internal static readonly JsonLoadSettings DefaultLoadSettings = new JsonLoadSettings()
        {
            LineInfoHandling = LineInfoHandling.Ignore,
            CommentHandling = CommentHandling.Ignore
        };

        /// <summary>
        /// Load json from a file to a JObject using the default load settings.
        /// </summary>
        internal static JObject LoadJson(TextReader reader)
        {
            using (var jsonReader = new JsonTextReader(reader))
            {
                return LoadJson(jsonReader);
            }
        }

        /// <summary>
        /// Load json from a file to a JObject using the default load settings.
        /// </summary>
        internal static JObject LoadJson(JsonReader jsonReader)
        {
            return JObject.Load(jsonReader, DefaultLoadSettings);
        }
    }
}
