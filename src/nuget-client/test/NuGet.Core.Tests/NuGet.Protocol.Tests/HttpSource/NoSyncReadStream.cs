// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class NoSyncReadStream : DownloadTimeoutStream
    {
        public NoSyncReadStream(Stream stream)
            : base("nosync", stream, TimeSpan.FromMinutes(1))
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            Assert.Fail("READ should not be called");
            throw new InvalidOperationException("test failed!! Read should not be called!");
        }
    }
}
