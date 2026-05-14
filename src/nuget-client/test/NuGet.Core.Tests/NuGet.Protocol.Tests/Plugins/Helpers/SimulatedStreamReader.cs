// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Text;

namespace NuGet.Protocol.Plugins.Tests
{
    internal sealed class SimulatedStreamReader : StreamReader
    {
        internal SimulatedStreamReader(Stream stream)
            : base(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, bufferSize: 4096, leaveOpen: true)
        {
        }
    }
}
