// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.Helix.Client;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.DotNet.Helix.JobSender.Test
{
    public class AdHocPayloadTests
    {
        [Fact]
        public void MultipleFilesWithSameNameAreRefused()
        {
            var exception = Assert.Throws<ArgumentException>(() => new AdhocPayload(new[] { "a/b.txt", "a/c.txt", "d/b.txt" }));
            Assert.StartsWith("Names of files to upload have to be distinct. The following name repeats at least once: b.txt", exception.Message);
            Assert.Equal("files", exception.ParamName);
        }

        [Fact]
        public void MultipleFilesWithNamesSameUpToCaseAreStillRefused()
        {
            var exception = Assert.Throws<ArgumentException>(() => new AdhocPayload(new[] { "a/B.txt", "a/c.txt", "d/b.txt" }));
            Assert.StartsWith("Names of files to upload have to be distinct. The following name repeats at least once: b.txt", exception.Message);
            Assert.Equal("files", exception.ParamName);
        }

        [Fact]
        public void MultipleFilesWithDifferentNamesAreAccepted()
        {
            new AdhocPayload(new[] { "a/b.txt", "a/c.txt", "b/d.txt" });
        }
    }
}
