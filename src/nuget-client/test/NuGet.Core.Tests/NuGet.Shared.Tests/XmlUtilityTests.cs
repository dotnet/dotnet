// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Shared.Test
{
    public class XmlUtilityTests
    {
        [Fact]
        public void Load_FilePathIsNull_Throws()
        {
            //Act
            var exception = Assert.Throws<ArgumentNullException>(() => XmlUtility.Load(path: null));

            //Assert
            Assert.Equal("path", exception.ParamName);
        }

        [Fact]
        public void Load_FileWithSecureXml_Success()
        {
            using (var root = TestDirectory.Create())
            {
                //Arrange
                string path = Path.Combine(Path.GetDirectoryName(root), "packages.config");

                using (var writer = new StreamWriter(path))
                {
                    writer.Write(
@"<packages>
<package id=""x"" version=""1.1.0"" targetFramework=""net45"" /> 
<package id=""y"" version=""1.0.0"" targetFramework=""net45"" />
</packages>");
                }

                //Act
                XDocument doc = XmlUtility.Load(path);

                //Assert
                Assert.Equal("packages", doc.Root.Name);
                Assert.Equal(2, doc.Root.Elements().Count());
            }
        }

        [Fact]
        public void Load_FileWithInSecureXml_Throws()
        {
            using (var root = TestDirectory.Create())
            {
                //Arrange
                string path = Path.Combine(Path.GetDirectoryName(root), "packages.config");
                using (var writer = new StreamWriter(path))
                {
                    writer.Write(
@"<!DOCTYPE package [
   <!ENTITY greeting ""Hello"">
   <!ENTITY name ""NuGet Client "">
   <!ENTITY sayhello ""&greeting; &name;"">
]>
<packages>
    <package id=""&sayhello;"" version=""1.1.0"" targetFramework=""net45"" /> 
    <package id=""x"" version=""1.0.0"" targetFramework=""net45"" />
</packages>");
                }

                //Act & Assert
                Assert.Throws<XmlException>(() => XmlUtility.Load(path));
            }
        }

        [Fact]
        public void GetEncodedXMLName_ValidXmlElementNameMatchesInput_Success()
        {
            string validXml = "packageSources";

            string encodedXml = XmlUtility.GetEncodedXMLName(validXml);

            Assert.Equal(validXml, encodedXml);
        }

        [Fact]
        public void GetEncodedXMLName_EncodesInValidXmlElementName_Success()
        {
            string invalidXml = "package Sources";

            string encodedXml = XmlUtility.GetEncodedXMLName(invalidXml);

            Assert.NotEqual(invalidXml, encodedXml);
            Assert.Equal(encodedXml, "package_x0020_Sources");
        }

        [Fact]
        public void Load_FileWithPUACharactersInPath_Success()
        {
            using var root = TestDirectory.Create();

            //Arrange
            string path = Path.Combine(Path.GetDirectoryName(root), "U1[]U2[]U3[]", "packages.config");
            //Create a new directory under test working directory with PAU chars in the name
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            using (var writer = new StreamWriter(path))
            {
                writer.Write(
@"<packages>
<package id=""x"" version=""1.1.0"" targetFramework=""net45"" /> 
<package id=""y"" version=""1.0.0"" targetFramework=""net45"" />
</packages>");
            }

            //Act
            XDocument doc = XmlUtility.Load(path);

            //Assert
            Assert.Equal("packages", doc.Root.Name);
            Assert.Equal(2, doc.Root.Elements().Count());
        }
    }
}
