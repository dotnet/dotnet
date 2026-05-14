// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Text;

namespace NuGet.Protocol.Plugins.Tests
{
    internal sealed class SimulatedStreamWriter : StreamWriter
    {
        internal SimulatedStreamWriter(Stream stream)
            : base(stream, Encoding.UTF8, bufferSize: 4096, leaveOpen: true)
        {
            AutoFlush = true;
        }
    }
}
