// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Text.Json.Serialization;

namespace NuGet.CommandLine.XPlat
{
    internal class PackageSearchProblem
    {
        [JsonPropertyName("text")]
        public string Text { get; private set; }

        [JsonPropertyName("problemType")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PackageSearchProblemType ProblemType { get; }

        private PackageSearchProblem()
        { }

        internal PackageSearchProblem(PackageSearchProblemType packageSearchProblemType, string text)
        {
            ProblemType = packageSearchProblemType;
            Text = text;
        }

        public PackageSearchProblem(string text, PackageSearchProblemType problemType)
        {
            Text = text;
            ProblemType = problemType;
        }
    }
}
