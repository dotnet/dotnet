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

        [Fact]
        public void Read_WhenJsonIsInvalid_LogsWarning()
        {
            var logger = new TestLogger();
            var contents = "{invalid json}";

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(contents)))
            {
                var cacheFile = CacheFileFormat.Read(stream, logger, "invalidPath");
                Assert.False(cacheFile.IsValid);
                Assert.Equal(1, logger.Warnings);
            }
        }

        [Fact]
        public void Read_WhenProjectFilePathIsMissing_ReturnsNullForProjectFilePath()
        {
            // Arrange
            var logger = new TestLogger();
            var contents = $@"{{
""version"": 2,
""dgSpecHash"": ""hash"",
""success"": true
}}";

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(contents)))
            {
                //Act
                var cacheFile = CacheFileFormat.Read(stream, logger, "path");

                // Assert
                Assert.Null(cacheFile.ProjectFilePath);
            }
        }

        [Fact]
        public void Write_WhenLogMessagesAreEmpty_DoesNotWriteLogsProperty()
        {
            // Arrange
            var cacheFile = new CacheFile("hash")
            {
                Version = 2,
                Success = true,
                ProjectFilePath = "somePath",
                ExpectedPackageFilePaths = new List<string>()
            };

            using (var stream = new MemoryStream())
            {
                //Act
                CacheFileFormat.Write(stream, cacheFile);
                var actual = Encoding.UTF8.GetString(stream.ToArray());

                // Assert
                Assert.DoesNotContain("\"logs\"", actual);
            }
        }

        [Fact]
        public void Write_WhenLogsAndExpectedPackageFilePathsPresent_WritesLogsAndExpectedPackageFilePaths()
        {
            var cacheFile = new CacheFile("hash")
            {
                Version = 2,
                Success = true,
                ProjectFilePath = "somePath",
                ExpectedPackageFilePaths = new List<string> { "file1", "file2" },
                LogMessages = new List<IAssetsLogMessage>
                {
                    new AssetsLogMessage(LogLevel.Warning, NuGetLogCode.NU1001, "Warning message")
                }
            };

            using (var stream = new MemoryStream())
            {
                CacheFileFormat.Write(stream, cacheFile);
                var actual = Encoding.UTF8.GetString(stream.ToArray());

                Assert.Contains("\"version\": 2", actual);
                Assert.Contains("\"dgSpecHash\": \"hash\"", actual);
                Assert.Contains("\"projectFilePath\": \"somePath\"", actual);
                Assert.Contains("\"expectedPackageFiles\": [", actual);
                Assert.Contains("file2", actual);
                Assert.Contains("file1", actual);
                Assert.Contains("\"logs\": [", actual);
                Assert.Contains("Warning message", actual);
            }
        }

        [Fact]
        public void Read_WhenJsonIsEmpty_LogsWarning()
        {
            // Arrange
            var logger = new TestLogger();
            var contents = "";

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(contents)))
            {
                // Act
                var cacheFile = CacheFileFormat.Read(stream, logger, "emptyPath");

                // Assert
                Assert.False(cacheFile.IsValid);
                Assert.Equal(1, logger.Warnings);
            }
        }

        [Theory]
        [MemberData(nameof(GetCacheFileTestData))]
        public void WriteRead_ValidValues_RoundTripTest(
        int version,
        string dgSpecHash,
        bool success,
        string projectFilePath,
        string[] expectedPackageFilePaths,
        List<IAssetsLogMessage> logMessages)
        {
            // Arrange
            var originalCacheFile = new CacheFile(dgSpecHash)
            {
                Version = version,
                Success = success,
                ProjectFilePath = projectFilePath,
                ExpectedPackageFilePaths = expectedPackageFilePaths != null ? new List<string>(expectedPackageFilePaths) : null,
                LogMessages = logMessages
            };

            string json;

            // Act - Serialize
            using (var stream = new MemoryStream())
            {
                CacheFileFormat.Write(stream, originalCacheFile);
                json = Encoding.UTF8.GetString(stream.ToArray());
            }

            // Assert - Serializer
            Assert.Contains($"\"version\": {version}", json);
            Assert.Contains($"\"dgSpecHash\": \"{dgSpecHash}\"", json);
            Assert.Contains($"\"success\": {success.ToString().ToLower()}", json);

            if (projectFilePath != null)
            {
                Assert.Contains($"\"projectFilePath\": \"{projectFilePath}\"", json);
            }
            else
            {
                Assert.DoesNotContain("\"projectFilePath\"", json);
            }

            if (expectedPackageFilePaths != null)
            {
                Assert.Contains("\"expectedPackageFiles\":", json);
                foreach (var path in expectedPackageFilePaths)
                {
                    Assert.Contains($"\"{path}\"", json);
                }
            }
            else
            {
                Assert.DoesNotContain("\"expectedPackageFiles\"", json);
            }

            if (logMessages != null)
            {
                Assert.Contains("\"logs\":", json);
                foreach (var log in logMessages)
                {
                    Assert.Contains($"\"message\": \"{log.Message}\"", json);
                }
            }
            else
            {
                Assert.DoesNotContain("\"logs\"", json);
            }

            // Act - Deserialize
            using (var deserializationStream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                var logger = new TestLogger();
                var deserializedCacheFile = CacheFileFormat.Read(deserializationStream, logger, "testPath");

                // Assert - Deserializer
                Assert.Equal(version, deserializedCacheFile.Version);
                Assert.Equal(dgSpecHash, deserializedCacheFile.DgSpecHash);
                Assert.Equal(success, deserializedCacheFile.Success);
                Assert.Equal(projectFilePath, deserializedCacheFile.ProjectFilePath);

                if (expectedPackageFilePaths != null)
                {
                    Assert.NotNull(deserializedCacheFile.ExpectedPackageFilePaths);
                    Assert.Equal(expectedPackageFilePaths.Length, deserializedCacheFile.ExpectedPackageFilePaths.Count);
                    Assert.Equal(expectedPackageFilePaths, deserializedCacheFile.ExpectedPackageFilePaths);
                }
                else
                {
                    Assert.Null(deserializedCacheFile.ExpectedPackageFilePaths);
                }

                if (logMessages != null)
                {
                    Assert.NotNull(deserializedCacheFile.LogMessages);
                    Assert.Equal(logMessages.Count, deserializedCacheFile.LogMessages.Count);
                    for (int i = 0; i < logMessages.Count; i++)
                    {
                        Assert.Equal(logMessages[i].Message, deserializedCacheFile.LogMessages[i].Message);
                        Assert.Equal(logMessages[i].Code, deserializedCacheFile.LogMessages[i].Code);
                        Assert.Equal(logMessages[i].Level, deserializedCacheFile.LogMessages[i].Level);
                    }
                }
                else
                {
                    Assert.Null(deserializedCacheFile.LogMessages);
                }
            }
        }

        public static IEnumerable<object[]> GetCacheFileTestData()
        {
            yield return new object[]
            {
                2,
                "hash1",
                true,
                "projectPath1",
                new string[] { "package1", "package2" },
                new List<IAssetsLogMessage>
                {
                    new AssetsLogMessage(LogLevel.Information, NuGetLogCode.NU1000, "Log message 1"),
                    new AssetsLogMessage(LogLevel.Warning, NuGetLogCode.NU1002, "Log message 2")
                }
            };

            yield return new object[]
            {
                2,
                "hash2",
                false,
                "projectPath2",
                new string[] { "package3" },
                new List<IAssetsLogMessage>
                {
                    new AssetsLogMessage(LogLevel.Error, NuGetLogCode.NU3000, "failure message")
                }
            };

            yield return new object[]
            {
                2,
                "hash3",
                true,
                null,
                new string[] { },
                null
            };

            yield return new object[]
            {
                2,
                "hash4",
                false,
                "projectPath4",
                null,
                new List<IAssetsLogMessage>()
            };
        }
    }
}
