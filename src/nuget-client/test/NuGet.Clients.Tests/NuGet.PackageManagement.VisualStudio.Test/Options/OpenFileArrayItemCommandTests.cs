// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Moq;
using NuGet.PackageManagement.VisualStudio.IDE;
using NuGet.PackageManagement.VisualStudio.Options;
using Xunit;

namespace NuGet.PackageManagement.VisualStudio.Test.Options
{
    [Collection(MockedVS.Collection)]
    public class OpenFileArrayItemCommandTests : MockedVSCollectionTests
    {
        private readonly Mock<IDocumentOpener> _documentOpener;
        private readonly OpenFileArrayItemCommand _service;

        public OpenFileArrayItemCommandTests(GlobalServiceProvider globalServiceProvider)
            : base(globalServiceProvider)
        {
            globalServiceProvider.Reset();
            _documentOpener = new Mock<IDocumentOpener>();
            _service = new OpenFileArrayItemCommand(_documentOpener.Object);
        }

        [Fact]
        public async Task IsEnabledAsync_IsAlways_TrueAsync()
        {
            // Arrange
            var anyFilePathDictionary = ImmutableDictionary<string, object>.Empty;

            // Act
            var result = await _service.IsEnabledAsync(anyFilePathDictionary, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Invoke_WhenDictionaryIsMissingFilePathKey_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var dictionaryFilePaths = new Dictionary<string, object>
            {
                { "someKey", "somePath" }
            };

            // Act
            Action act = () => _service.Invoke(dictionaryFilePaths);

            // Assert
            act.Should().Throw<KeyNotFoundException>(because: OpenFileArrayItemCommand.FILE_PATH + " was not found in the provided dictionary");
        }

        [Fact]
        public void Invoke_WhenFilePathDoesNotExist_ShouldThrowFileNotFoundException()
        {
            // Arrange
            _documentOpener.Setup(mock => mock.OpenDocument(It.IsAny<string>())).Throws<FileNotFoundException>();
            var pathDoesNotExist = "pathDoesNotExist/NuGet.Config";
            var dictionaryFilePaths = new Dictionary<string, object>
            {
                { OpenFileArrayItemCommand.FILE_PATH, pathDoesNotExist }
            };

            // Act
            Action act = () => _service.Invoke(dictionaryFilePaths);

            // Assert
            act.Should().Throw<FileNotFoundException>(because: "DocumentOpener was configured to throw 'FileNotFoundException' which should be an uncaught exception.");
        }
    }
}
