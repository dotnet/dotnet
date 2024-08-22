// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using NuGet.Packaging.Core;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class HttpStreamValidationTests
    {
        private const string Uri = "http://example/foo/bar";

        [Fact]
        public void HttpStreamValidation_ValidateJObject_RejectsIncompleteJsonObjects()
        {
            // Arrange
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(@"
                {
                    ""foo"": 1,
                    ""bar"": 2"));

            // Act & Assert
            var actual = Assert.Throws<InvalidDataException>(() =>
            {
                HttpStreamValidation.ValidateJObject(Uri, stream);
            });

            Assert.IsType<JsonReaderException>(actual.InnerException);
        }

        [Fact]
        public void HttpStreamValidation_ValidateJObject_RejectsBrokenJsonObjects()
        {
            // Arrange
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(@"
                {
                    ""foo"": 1,
                    ""bar"" []"));

            // Act & Assert
            var actual = Assert.Throws<InvalidDataException>(() =>
            {
                HttpStreamValidation.ValidateJObject(Uri, stream);
            });

            Assert.IsType<JsonReaderException>(actual.InnerException);
        }

        [Fact]
        public void HttpStreamValidation_ValidateJObject_RejectsJsonArray()
        {
            // Arrange
            var stream = new MemoryStream(Encoding.UTF8.GetBytes("[1, 2]"));

            // Act & Assert
            var actual = Assert.Throws<InvalidDataException>(() =>
            {
                HttpStreamValidation.ValidateJObject(Uri, stream);
            });

            Assert.IsType<JsonReaderException>(actual.InnerException);
        }

        [Fact]
        public void HttpStreamValidation_ValidateJObject_AcceptsMinimal()
        {
            // Arrange
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(@"
                {
                    ""foo"": 1,
                    ""bar"": 2
                }"));

            // Act & Assert
            HttpStreamValidation.ValidateJObject(Uri, stream);
        }

        [Fact]
        public void HttpStreamValidation_ValidateNupkg_RejectsCompletelyInvalidZipNupkg()
        {
            // Arrange
            using (var stream = new MemoryStream(Encoding.ASCII.GetBytes("not a zip!")))
            {
                // Act & Assert
                var actual = Assert.Throws<InvalidDataException>(() =>
                {
                    HttpStreamValidation.ValidateNupkg(
                        Uri,
                        stream);
                });

                Assert.IsType<InvalidDataException>(actual.InnerException);
            }
        }

        [Fact]
        public void HttpStreamValidation_ValidateNupkg_RejectsNupkgWithBadPrefix()
        {
            // Arrange
            using (var zipStream = new MemoryStream())
            using (var stream = new MemoryStream())
            {
                using (var zip = new ZipArchive(zipStream, ZipArchiveMode.Create, leaveOpen: true))
                {
                    zip.AddEntry("package.nuspec", new byte[0]);
                }

                zipStream.Seek(0, SeekOrigin.Begin);

                var badPrefix = Encoding.ASCII.GetBytes("bad!!!");
                stream.Write(badPrefix, 0, badPrefix.Length);
                zipStream.CopyTo(stream);
                stream.Seek(0, SeekOrigin.Begin);

                // Act & Assert
                var actual = Assert.Throws<InvalidDataException>(() =>
                {
                    HttpStreamValidation.ValidateNupkg(
                        Uri,
                        stream);
                });

                Assert.IsType<InvalidDataException>(actual.InnerException);
            }
        }

        [Fact]
        public void HttpStreamValidation_ValidateNupkg_RejectsPartialNupkg()
        {
            // Arrange
            using (var zipStream = new MemoryStream())
            using (var stream = new MemoryStream())
            {
                using (var zip = new ZipArchive(zipStream, ZipArchiveMode.Create, leaveOpen: true))
                {
                    zip.AddEntry("package.nuspec", new byte[0]);
                }

                zipStream.Seek(0, SeekOrigin.Begin);
                var partialLength = (int)zipStream.Length - 1;
                stream.Write(zipStream.ToArray(), 0, partialLength);
                stream.Seek(0, SeekOrigin.Begin);

                // Act & Assert
                var actual = Assert.Throws<InvalidDataException>(() =>
                {
                    HttpStreamValidation.ValidateNupkg(
                        Uri,
                        stream);
                });

                Assert.IsType<InvalidDataException>(actual.InnerException);
            }
        }

        [Fact]
        public void HttpStreamValidation_ValidateNupkg_RejectsMissingNuspec()
        {
            // Arrange
            using (var zipStream = new MemoryStream())
            {
                using (var zip = new ZipArchive(zipStream, ZipArchiveMode.Create, leaveOpen: true))
                {
                    zip.AddEntry("foo.bar", new byte[0]);
                }

                zipStream.Seek(0, SeekOrigin.Begin);

                // Act & Assert
                var actual = Assert.Throws<InvalidDataException>(() =>
                {
                    HttpStreamValidation.ValidateNupkg(
                        Uri,
                        zipStream);
                });

                Assert.IsType<PackagingException>(actual.InnerException);
            }
        }

        [Fact]
        public void HttpStreamValidation_ValidateNupkg_RejectsBrokenNuspec()
        {
            // Arrange
            using (var stream = new MemoryStream())
            {
                using (var zip = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true))
                {
                    zip.AddEntry("package.nuspec", new byte[0]);
                }

                stream.Seek(0, SeekOrigin.Begin);

                // Act & Assert
                var actual = Assert.Throws<InvalidDataException>(() =>
                {
                    HttpStreamValidation.ValidateNupkg(
                        Uri,
                        stream);
                });

                Assert.IsType<XmlException>(actual.InnerException);
            }
        }

        [Fact]
        public void HttpStreamValidation_ValidateNupkg_AcceptsMinimalNupkg()
        {
            // Arrange
            using (var stream = new MemoryStream())
            {
                using (var zip = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true))
                {
                    zip.AddEntry("package.nuspec", @"<?xml version=""1.0"" encoding=""utf-8""?>
<package>
</package>");
                }

                stream.Seek(0, SeekOrigin.Begin);

                // Act & Assert
                HttpStreamValidation.ValidateNupkg(
                    Uri,
                    stream);
            }
        }

        [Fact]
        public void HttpStreamValidation_ValidateXml_RejectsBroken()
        {
            // Arrange
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(@"<?xml version=""1.0"" encoding=""utf-8""?>
                <entry>
                </ent
                ")))
            {
                // Act & Assert
                var actual = Assert.Throws<InvalidDataException>(() =>
                {
                    HttpStreamValidation.ValidateXml(
                        Uri,
                        stream);
                });

                Assert.IsType<XmlException>(actual.InnerException);
            }
        }

        [Fact]
        public void HttpStreamValidation_ValidateXml_RejectsIncompleteBroken()
        {
            // Arrange
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(@"<?xml version=""1.0"" encoding=""utf-8""?>
                <entry>
                <another>
                ")))
            {
                // Act & Assert
                var actual = Assert.Throws<InvalidDataException>(() =>
                {
                    HttpStreamValidation.ValidateXml(
                        Uri,
                        stream);
                });

                Assert.IsType<XmlException>(actual.InnerException);
            }
        }

        [Fact]
        public void HttpStreamValidation_ValidateXml_AcceptsMinimal()
        {
            // Arrange
            using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(@"<?xml version=""1.0"" encoding=""utf-8""?>
                <entry>
                </entry>
                ")))
            {
                // Act & Assert
                HttpStreamValidation.ValidateXml(
                    Uri,
                    stream);
            }
        }
    }
}
