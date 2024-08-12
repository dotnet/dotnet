// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Packaging.Core;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class RepositoryMetadataTests
    {
        [Fact]
        public void RepositoryMetadataDefaultConstructor()
        {
            var metadata = new RepositoryMetadata();

            Assert.Equal(string.Empty, metadata.Type);
            Assert.Equal(string.Empty, metadata.Url);
            Assert.Equal(string.Empty, metadata.Branch);
            Assert.Equal(string.Empty, metadata.Commit);
        }

        [Theory]
        [MemberData(nameof(EqualsRepositoryMetadataData))]
        public void RepositoryMetadataEquals(RepositoryMetadata metadata1, RepositoryMetadata metadata2, bool equals)
        {
            Assert.Equal(equals, metadata1 == metadata2);
            Assert.NotEqual(equals, metadata1 != metadata2);

            if (metadata1 != null)
            {
                Assert.Equal(equals, metadata1.Equals(metadata2));
            }
            if (metadata1 != null && metadata2 != null)
            {
                Assert.Equal(equals, metadata1.GetHashCode() == metadata2.GetHashCode());
            }
        }

        public static TheoryData<RepositoryMetadata, RepositoryMetadata, bool> EqualsRepositoryMetadataData
        {
            get
            {
                var template = new RepositoryMetadata("type", "url", "branch", "commit");

                return new TheoryData<RepositoryMetadata, RepositoryMetadata, bool>
                {
                    { template, template, true },
                    { template, null, false },
                    { null, template, false },
                    { new RepositoryMetadata(), new RepositoryMetadata(), true },
                    { new RepositoryMetadata("type", "url", "branch", "commit"), template, true },

                    { new RepositoryMetadata("TYPE", "url", "branch", "commit"), template, true },
                    { new RepositoryMetadata("type", "URL", "branch", "commit"), template, false },
                    { new RepositoryMetadata("type", "url", "BRANCH", "commit"), template, false },
                    { new RepositoryMetadata("type", "url", "branch", "COMMIT"), template, false },

                    { new RepositoryMetadata("faketype", "url", "branch", "commit"), template, false },
                    { new RepositoryMetadata("type", "fakeurl", "branch", "commit"), template, false },
                    { new RepositoryMetadata("type", "url", "fakebranch", "commit"), template, false },
                    { new RepositoryMetadata("type", "url", "branch", "fakecommit"), template, false },

                    { new RepositoryMetadata(null, "url", "branch", "commit"), template, false },
                    { new RepositoryMetadata("type", null, "branch", "commit"), template, false },
                    { new RepositoryMetadata("type", "url", null, "commit"), template, false },
                    { new RepositoryMetadata("type", "url", "branch", null), template, false },
                };
            }
        }
    }
}
