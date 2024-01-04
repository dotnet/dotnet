// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.SwaggerGenerator
{
    public class CodeFile
    {
        public CodeFile(string path, string contents)
        {
            Path = path;
            Contents = contents;
        }

        public string Path { get; }
        public string Contents { get; }

        public void Deconstruct(out string path, out string contents)
        {
            path = Path;
            contents = Contents;
        }
    }
}
