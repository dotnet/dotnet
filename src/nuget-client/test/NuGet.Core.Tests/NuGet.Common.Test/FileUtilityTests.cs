// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Common.Test
{
    public class FileUtilityTests
    {
        [Fact]
        public void FileUtility_Replace_BasicSuccess()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                // Arrange
                var dest = Path.Combine(testDirectory, "b");

                Action<string> action = (path) =>
                {
                    File.WriteAllText(path, "a");
                };

                // Act
                FileUtility.Replace(action, dest);

                // Assert
                Assert.True(File.Exists(dest));
                Assert.Equal(1, Directory.GetFiles(testDirectory).Length);
            }
        }

        [Fact]
        public void FileUtility_Replace_AlreadyExistsSuccess()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                // Arrange
                var dest = Path.Combine(testDirectory, "b");
                File.WriteAllText(dest, "b");

                Action<string> action = (path) =>
                {
                    File.WriteAllText(path, "a");
                };

                // Act
                FileUtility.Replace(action, dest);

                // Assert
                Assert.True(File.Exists(dest));
                Assert.Equal(1, Directory.GetFiles(testDirectory).Length);
                Assert.Equal("a", File.ReadAllText(dest));
            }
        }

        [Fact]
        public void FileUtility_Replace_Failure()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                // Arrange
                var dest = Path.Combine(testDirectory, "b");

                Action<string> action = (path) =>
                {
                    throw new Exception();
                };

                Exception? exception = null;

                // Act
                try
                {
                    FileUtility.Replace(action, dest);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                // Assert
                Assert.False(File.Exists(dest));
                Assert.Equal(0, Directory.GetFiles(testDirectory).Length);
                Assert.NotNull(exception);
            }
        }

        [Fact]
        public async Task FileUtility_ReplaceWithLock_BasicSuccess()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                // Arrange
                var dest = Path.Combine(testDirectory, "b");

                Action<string> action = (path) =>
                {
                    File.WriteAllText(path, "a");
                };

                // Act
                await FileUtility.ReplaceWithLock(action, dest);

                // Assert
                Assert.True(File.Exists(dest));
                Assert.Equal(1, Directory.GetFiles(testDirectory).Length);
            }
        }

        [Fact]
        public async Task FileUtility_DeleteWithLock_BasicSuccess()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                // Arrange
                var dest = Path.Combine(testDirectory, "b");
                File.WriteAllText(dest, "a");

                // Act
                await FileUtility.DeleteWithLock(dest);

                // Assert
                Assert.False(File.Exists(dest));
                Assert.Equal(0, Directory.GetFiles(testDirectory).Length);
            }
        }

        [Fact]
        public void FileUtility_MoveBasicSuccess()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                // Arrange
                var orig = Path.Combine(testDirectory, "a");
                var dest = Path.Combine(testDirectory, "b");

                File.WriteAllText(orig, "a");

                // Act
                FileUtility.Move(orig, dest);

                // Assert
                Assert.True(File.Exists(dest));
                Assert.False(File.Exists(orig));
            }
        }

        [Fact]
        public void FileUtility_MoveBasicFail()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                // Arrange
                var orig = Path.Combine(testDirectory, "a");
                var dest = Path.Combine(testDirectory, "b");

                File.WriteAllText(orig, "a");
                File.WriteAllText(dest, "a");

                using (var stream = File.OpenWrite(dest))
                {
                    // Act & Assert
                    Assert.Throws(typeof(IOException), () =>
                        FileUtility.Move(orig, dest));
                }
            }
        }

        [Fact]
        public void FileUtility_DeleteBasicSuccess()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                // Arrange
                var path = Path.Combine(testDirectory, "a");

                File.WriteAllText(path, "a");

                // Act
                FileUtility.Delete(path);

                // Assert
                Assert.False(File.Exists(path));
            }
        }

        [Fact]
        public void FileUtility_DeleteBasicFail()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                // Arrange
                var path = Path.Combine(testDirectory, "a");

                File.WriteAllText(path, "a");

                using (var stream = File.OpenWrite(path))
                {
                    // Act & Assert
                    if (RuntimeEnvironmentHelper.IsWindows)
                    {
                        Assert.Throws(typeof(IOException), () =>
                            FileUtility.Delete(path));
                    }
                    else
                    {
                        // Linux and OSX will delete the file without an error
                        FileUtility.Delete(path);
                        Assert.False(File.Exists(path));
                    }
                }
            }
        }

        [Fact]
        public async Task FileUtility_ReplaceAsync_BasicSuccess()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                // Arrange
                var dest = Path.Combine(testDirectory, "b");

                // Act
                await FileUtility.ReplaceAsync(async (path) =>
                {
                    using (var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        using (var writer = new StreamWriter(stream))
                        {
                            await writer.WriteAsync("a");
                        }
                    }
                }, dest);

                // Assert
                Assert.True(File.Exists(dest));
                Assert.Equal("a", File.ReadAllText(dest));
            }
        }

        [Fact]
        public async Task FileUtility_ReplaceAsync_AlreadyExistsSuccess()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                // Arrange
                var dest = Path.Combine(testDirectory, "b");
                File.WriteAllText(dest, "b");

                // Act
                await FileUtility.ReplaceAsync(async (path) =>
                {
                    using (var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        using (var writer = new StreamWriter(stream))
                        {
                            await writer.WriteAsync("a");
                        }
                    }
                }, dest);

                // Assert
                Assert.True(File.Exists(dest));
                Assert.Equal("a", File.ReadAllText(dest));
            }
        }

        [Fact]
        public void FileUtility_SafeRead_Success()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                // Arrange
                var dest = Path.Combine(testDirectory, "b");
                File.WriteAllText(dest, "b");

                // Act
                var fileData = FileUtility.SafeRead(dest, (stream, path) =>
                {
                    using (var reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                });

                // Assert
                Assert.True(File.Exists(dest));
                Assert.Equal("b", fileData);
            }
        }

        [Fact]
        public async Task FileUtility_SafeReadAsync_Success()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                // Arrange
                var dest = Path.Combine(testDirectory, "b");
                File.WriteAllText(dest, "b");

                // Act
                var fileData = await FileUtility.SafeReadAsync(dest, async (stream, path) =>
                {
                    using (var reader = new StreamReader(stream))
                    {
                        return await reader.ReadToEndAsync();
                    }
                });

                // Assert
                Assert.True(File.Exists(dest));
                Assert.Equal("b", fileData);
            }
        }

        [Fact]
        public async Task FileUtility_SafeReadAsync_MultipleReadStreams()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                // Arrange
                var dest = Path.Combine(testDirectory, "b");
                File.WriteAllText(dest, "b");

                var firstRead = string.Empty;

                // Act
                var fileData = await FileUtility.SafeReadAsync(dest, async (stream, path) =>
                {
                    using (var reader1 = new StreamReader(stream))
                    {
                        firstRead = await reader1.ReadToEndAsync();

                        using (var stream2 = new FileStream(dest, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
                        {
                            using (var reader2 = new StreamReader(stream2))
                            {
                                return await reader2.ReadToEndAsync();
                            }
                        }
                    }
                });

                // Assert
                Assert.True(File.Exists(dest));
                Assert.Equal("b", fileData);
                Assert.Equal(firstRead, fileData);
            }
        }

        [Fact]
        public async Task FileUtility_SafeReadAsync_Failure()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                // Arrange
                var dest = Path.Combine(testDirectory, "b");

                Exception? exception = null;

                // Act
                try
                {
                    var fileData = await FileUtility.SafeReadAsync(dest, async (stream, path) =>
                    {
                        if (File.Exists(dest))
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                return await reader.ReadToEndAsync();
                            }
                        }
                        else
                        {
                            throw new Exception();
                        }
                    });
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                // Assert
                Assert.False(File.Exists(dest));
                Assert.NotNull(exception);
            }
        }
    }
}
