// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using NuGet.Common;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.ProjectModel.Test
{
    public class CacheFileFormatTests
    {
        [Fact]
        public void Read_WhenVersionIs1_ReadsCorrectly()
        {
            var logger = new TestLogger();

            var contents = $@"{{
  ""version"": ""1"",
  ""dgSpecHash"": ""LhkXQGGI+FQMy9dhLYjG5sWcHX3z/copzi4hjjBiY3Fotv0i7zQCikMZQ+rOKJ03gtx0hoHwIx5oKkM7sVHu7g=="",
  ""success"": true,
}}";
            CacheFile cacheFile = null;
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(contents)))
            {
                cacheFile = CacheFileFormat.Read(stream, logger, "emptyPath");
            }
            Assert.False(cacheFile.IsValid);
            Assert.True(cacheFile.Success);
            Assert.Equal("LhkXQGGI+FQMy9dhLYjG5sWcHX3z/copzi4hjjBiY3Fotv0i7zQCikMZQ+rOKJ03gtx0hoHwIx5oKkM7sVHu7g==", cacheFile.DgSpecHash);
            Assert.Equal(1, cacheFile.Version);

            Assert.Equal(0, logger.Errors);
            Assert.Equal(0, logger.Warnings);
        }

        [Fact]
        public void Read_WhenVersionIsCurrentVersion_ReadsCorrectly()
        {
            using (var workingDir = TestDirectory.Create())
            {
                var logger = new TestLogger();

                var projectFullPath = Path.Combine(workingDir, "EA11D9B8013142A6B40A81FD90F57EAA");
                var dgSpecHash = "LhkXQGGI+FQMy9dhLYjG5sWcHX3z/copzi4hjjBiY3Fotv0i7zQCikMZQ+rOKJ03gtx0hoHwIx5oKkM7sVHu7g==";
                var success = "true";

                var file1 = Path.Combine(workingDir, "7A329DF71DDD41F689C9AD876DDF79F6");
                var file2 = Path.Combine(workingDir, "C16089965CF84822A71D07580B29AF0E");

                File.WriteAllText(file1, string.Empty);

                var version = "2";

                var contents = $@"{{
  ""version"": {version},
  ""dgSpecHash"": ""{dgSpecHash}"",
  ""success"": {success},
  ""projectFilePath"": {JsonConvert.ToString(projectFullPath)},
  ""expectedPackageFiles"": [
    {JsonConvert.ToString(file1)},
    {JsonConvert.ToString(file2)}
  ],
  ""logs"": [
    {{
      ""code"": ""NU1000"",
      ""level"": ""Information"",
      ""message"": ""Test""
    }}
  ]
}}";
                CacheFile cacheFile = null;
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(contents)))
                {
                    cacheFile = CacheFileFormat.Read(stream, logger, "emptyPath");
                }

                Assert.True(cacheFile.IsValid);
                Assert.Equal(bool.Parse(success), cacheFile.Success);
                Assert.Equal(dgSpecHash, cacheFile.DgSpecHash);
                Assert.Equal(int.Parse(version), cacheFile.Version);

                Assert.Equal(projectFullPath, cacheFile.ProjectFilePath);
                Assert.Equal(1, cacheFile.LogMessages.Count);

                Assert.Equal(0, logger.Errors);
                Assert.Equal(0, logger.Warnings);
            }
        }

        [Fact]
        public void Write_WhenVersionIsCurrentVersion_WritesCorrectly()
        {
            using (var workingDir = TestDirectory.Create())
            {
                var projectFullPath = Path.Combine(workingDir, "E6E7F0F96EBE438887ED7D0B9FC88AFA");

                var file1 = Path.Combine(workingDir, "DA9707B5FCFB4DA8B8BB77AD527C778C");
                var file2 = Path.Combine(workingDir, "C78CE6D18C604A55BECD845F4F694A4B");

                var v = "2";
                var dgSpecHash = "LhkXQGGI+FQMy9dhLYjG5sWcHX3z/copzi4hjjBiY3Fotv0i7zQCikMZQ+rOKJ03gtx0hoHwIx5oKkM7sVHu7g==";
                var success = "true";
                var expected = $@"{{
  ""version"": {v},
  ""dgSpecHash"": ""{dgSpecHash}"",
  ""success"": {success},
  ""projectFilePath"": {JsonConvert.ToString(projectFullPath)},
  ""expectedPackageFiles"": [
    {JsonConvert.ToString(file1)},
    {JsonConvert.ToString(file2)}
  ],
  ""logs"": [
    {{
      ""code"": ""NU1000"",
      ""level"": ""Information"",
      ""message"": ""Test""
    }}
  ]
}}";

                var cacheFile = new CacheFile(dgSpecHash)
                {
                    Success = bool.Parse(success),
                    ProjectFilePath = projectFullPath,
                    ExpectedPackageFilePaths = new List<string>
                {
                    file1,
                    file2
                },
                    LogMessages = new List<IAssetsLogMessage>
                {
                    new AssetsLogMessage(LogLevel.Information, NuGetLogCode.NU1000, "Test")
                }
                };

                using (var stream = new MemoryStream())
                {
                    CacheFileFormat.Write(stream, cacheFile);
                    var actual = Encoding.UTF8.GetString(stream.ToArray());

                    Assert.Equal(expected, actual);
                }
            }
        }
    }
}
