// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Microsoft.Build.Utilities;
using Microsoft.Arcade.Test.Common;
using Xunit;

namespace Microsoft.DotNet.Arcade.Sdk.Tests
{
    public sealed class GenerateSourcePackageSourceLinkTargetsFileTests 
    {
        [Fact]
        public void GetOutputFileContent()
        {
            string NormalizePath(string path) =>
                path.Replace('\\', Path.DirectorySeparatorChar);

            var task = new GenerateSourcePackageSourceLinkTargetsFile
            {
                ProjectDirectory = NormalizePath(@"C:\temp\A\B\C\D\E\F"),
                PackageId = "My.Package",
                SourceRoots = new TaskItem[] 
                {
                    new TaskItem(NormalizePath(@"C:\temp\A\"), new Dictionary<string, string> { { "SourceLinkUrl", "http://A-git/commitsha/*" } }),
                    new TaskItem(NormalizePath(@"C:\temp\A\B\")),
                    new TaskItem(NormalizePath(@"C:\temp\A\B\C\"), new Dictionary<string, string> { { "SourceLinkUrl", "http://C-git/commitsha/*?var=value" } }),
                    new TaskItem(NormalizePath(@"C:\temp\A\B\C\D\")),
                },
                OutputPath = "xxx",
            };

            string content = task.GetOutputFileContent();
            AssertEx.AssertEqualToleratingWhitespaceDifferences(@"<?xml version=""1.0"" encoding=""utf-8""?>
<Project xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
  <Target Name=""_AddSourcePackageSourceRoot_CFB3FCB48DA6C1861F924045FCA162F513465D35"" BeforeTargets=""InitializeSourceControlInformation"">
    <ItemGroup>
      <_PackageCompileItems Remove=""@(_PackageCompileItems)""/>
      <_PackageCompileItems Include=""%(Compile.RootDir)%(Compile.Directory)"" Condition=""'%(Compile.NuGetPackageId)' == 'My.Package'"" />
    </ItemGroup>
    <PropertyGroup>
      <_PackageCompileItem>@(_PackageCompileItems)</_PackageCompileItem>
      <_PackageCompileItem Condition=""'$(_PackageCompileItem)' != ''"">$(_PackageCompileItem.Split(';')[0])</_PackageCompileItem>
    </PropertyGroup>
    <ItemGroup>
      <SourceRoot Include=""$([System.Text.RegularExpressions.Regex]::Match($(_PackageCompileItem), '^$([System.Text.RegularExpressions.Regex]::Escape($([System.IO.Path]::GetFullPath('$(MSBuildThisFileDirectory)../contentFiles/'))))([^\\/]+[\\/][^\\/]+[\\/])'))"" 
                  SourceLinkUrl=""http://C-git/commitsha/D/E/F/*?var=value""/>
    </ItemGroup>
  </Target>
</Project>
", content);
            XDocument.Load(new StringReader(content));
        }
    }
}
