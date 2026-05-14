// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.Deployment.DotNet.Releases.Tests
{
    public class ProductCollectionTests : TestBase
    {
        [Fact]
        public async Task ItReturnsAllSupportPhases()
        {
            ProductCollection products = await ProductCollection.GetFromFileAsync(@"data/releases-index.json", false);
            IEnumerable<SupportPhase> supportPhases = products.GetSupportPhases();

            Assert.Equal(4, supportPhases.Count());
            Assert.Contains(SupportPhase.Active, supportPhases);
            Assert.Contains(SupportPhase.EOL, supportPhases);
            Assert.Contains(SupportPhase.Maintenance, supportPhases);
            Assert.Contains(SupportPhase.GoLive, supportPhases);
        }

        [Fact]
        public async Task ItThrowsIfPathIsNull()
        {
            Func<Task> f = async () => await ProductCollection.GetFromFileAsync((string)null, false); ;

            _ = await Assert.ThrowsAsync<ArgumentNullException>(f); 
        }

        [Fact]
        public async Task ItThrowsIfPathIsEmpty()
        {
            Func<Task> f = async () => await ProductCollection.GetFromFileAsync("", false); 

            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(f);
            Assert.Equal($"Value cannot be empty. (Parameter 'path')", exception.Message);
        }

        [Fact]
        public async Task ItThrowsIfFileDoesNotExitAndCannotBeDownloaded()
        {
            Func<Task> f = async () => await ProductCollection.GetFromFileAsync("data.json", false);

            FileNotFoundException exception = await Assert.ThrowsAsync<FileNotFoundException>(f);

            Assert.Equal("Could not find the specified file: data.json", exception.Message);
        }

        [Fact]
        public async Task ItThrowsIfReleasesUriIsNull()
        {
            Func<Task> f = async () => await ProductCollection.GetAsync((string)null);

            ArgumentNullException exception = await Assert.ThrowsAsync<ArgumentNullException>(f);
        }

        [Fact]
        public async Task ItThrowsIfReleasesUriIsEmpty()
        {
            Func<Task> f = async () => await ProductCollection.GetAsync("");

            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(f);
            Assert.Equal($"Value cannot be empty. (Parameter 'releasesIndexUri')", exception.Message);
        }
    }
}
