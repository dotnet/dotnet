// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Arcade.Test.Common;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;

namespace Microsoft.DotNet.Build.Tasks.VisualStudio.UnitTests
{
    public class FinalizeInsertionVsixFileTests
    {
        [Fact]
        public void UpdateInstallationElement()
        {
            var task = new FinalizeInsertionVsixFile()
            {
                VsixFilePath = "x.vsix",
            };

            var manifest = @"<?xml version=""1.0"" encoding=""utf-8""?>
<PackageManifest Version=""2.0.0"" xmlns=""http://schemas.microsoft.com/developer/vsx-schema/2011"">
  <Installation Experimental=""true"" />
</PackageManifest>
";

            var manifestXml = XDocument.Load(new StringReader(manifest));

            task.UpdateInstallationElement(manifestXml);

            AssertEx.AssertEqualToleratingWhitespaceDifferences(@"
<PackageManifest Version=""2.0.0"" xmlns=""http://schemas.microsoft.com/developer/vsx-schema/2011"">
  <Installation SystemComponent=""true"" />
</PackageManifest>
", manifestXml.ToString());
        }

        [Fact]
        public void UpdateInstallationElement_BadXml()
        {
            var engine = new MockEngine { ContinueOnError = true };

            var task = new FinalizeInsertionVsixFile()
            {
                BuildEngine = engine,
                VsixFilePath = "x.vsix",
            };

            var manifest = @"<?xml version=""1.0"" encoding=""utf-8""?>
<PackageManifest xmlns=""http://schemas.microsoft.com/developer/vsx-schema/2011"">
</PackageManifest>
";

            var manifestXml = XDocument.Load(new StringReader(manifest));
            
            task.UpdateInstallationElement(manifestXml);

            AssertEx.Equal(new[] { "PackageManifest.Installation element not found in manifest of 'x.vsix'" }, engine.Errors.Select(e => e.Message));
        }

        [Fact]
        public void UpdateInstallationElement_ExperimentalNotSpecified()
        {
            var engine = new MockEngine { ContinueOnError = true };

            var task = new FinalizeInsertionVsixFile()
            {
                BuildEngine = engine,
                VsixFilePath = "x.vsix",
            };

            var manifest = @"<?xml version=""1.0"" encoding=""utf-8""?>
<PackageManifest Version=""2.0.0"" xmlns=""http://schemas.microsoft.com/developer/vsx-schema/2011"">
  <Installation/>
</PackageManifest>
";

            var manifestXml = XDocument.Load(new StringReader(manifest));

            task.UpdateInstallationElement(manifestXml);

            AssertEx.Equal(new[] { @"PackageManifest.Installation element of the manifest does not have Experimental=""true"": 'x.vsix'" }, engine.Warnings.Select(e => e.Message));

            AssertEx.AssertEqualToleratingWhitespaceDifferences(@"
<PackageManifest Version=""2.0.0"" xmlns=""http://schemas.microsoft.com/developer/vsx-schema/2011"">
  <Installation SystemComponent=""true"" />
</PackageManifest>
", manifestXml.ToString());
        }

        [Fact]
        public void UpdateInstallationElement_ExperimentalFalse()
        {
            var engine = new MockEngine { ContinueOnError = true };

            var task = new FinalizeInsertionVsixFile()
            {
                BuildEngine = engine,
                VsixFilePath = "x.vsix",
            };

            var manifest = @"<?xml version=""1.0"" encoding=""utf-8""?>
<PackageManifest Version=""2.0.0"" xmlns=""http://schemas.microsoft.com/developer/vsx-schema/2011"">
  <Installation Experimental=""false"" SystemComponent=""false""/>
</PackageManifest>
";

            var manifestXml = XDocument.Load(new StringReader(manifest));

            task.UpdateInstallationElement(manifestXml);

            AssertEx.Equal(new[] 
            {
                @"PackageManifest.Installation element of the manifest does not have Experimental=""true"": 'x.vsix'",
                @"PackageManifest.Installation element of the manifest specifies SystemComponent attribute: 'x.vsix'",
            }, engine.Warnings.Select(e => e.Message));

            AssertEx.AssertEqualToleratingWhitespaceDifferences(@"
<PackageManifest Version=""2.0.0"" xmlns=""http://schemas.microsoft.com/developer/vsx-schema/2011"">
  <Installation SystemComponent=""true"" />
</PackageManifest>
", manifestXml.ToString());
        }
    }
}
