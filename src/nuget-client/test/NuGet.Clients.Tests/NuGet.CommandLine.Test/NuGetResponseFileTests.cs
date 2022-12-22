#if IS_DESKTOP
// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Text;
using NuGet.Common;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.CommandLine.Test
{
    public class NuGetResponseFileTests
    {
        private const int TwoMegaBytesLength = 2048000;

        [WindowsNTFact]
        public void ResponseFileSingleArgFullPath()
        {
            using (var workingDirectory = new SimpleTestPathContext().WorkingDirectory)
            {
                // Arrange
                var responseFileArg1 = "/responseFileArg1";
                var responseFilePath = Path.Combine(workingDirectory, "responseFile1.rsp");
                var testArgs = new string[] { "/arg1", "/arg2", "@" + responseFilePath, "/arg3" };
                CreateResponseFile(responseFilePath, responseFileArg1);

                // Act
                var parsedArgs = ParseArgs(testArgs);

                // Assert
                Assert.Equal(testArgs[0], parsedArgs[0]);
                Assert.Equal(testArgs[1], parsedArgs[1]);
                Assert.Equal(responseFileArg1, parsedArgs[2]);
                Assert.Equal(testArgs[3], parsedArgs[3]);
            }
        }

        [WindowsNTFact]
        public void ResponseFileBlankLines()
        {
            using (var workingDirectory = new SimpleTestPathContext().WorkingDirectory)
            {
                // Arrange
                var testArgs = new string[] { "/arg1", "/arg2", "@responseFile1.rsp", "/arg3" };
                var responseFileArg1 = string.Format("/responseFileArg1{0}{0}{0}/responseFileArg2", Environment.NewLine);
                CreateResponseFile("responseFile1.rsp", responseFileArg1);

                // Act
                var parsedArgs = ParseArgs(testArgs);

                // Assert
                Assert.Equal(testArgs[0], parsedArgs[0]);
                Assert.Equal(testArgs[1], parsedArgs[1]);
                Assert.Equal("/responseFileArg1", parsedArgs[2]);
                Assert.Equal("/responseFileArg2", parsedArgs[3]);
                Assert.Equal(testArgs[3], parsedArgs[4]);
            }
        }

        [WindowsNTFact]
        public void NoResponseFile()
        {
            using (var workingDirectory = new SimpleTestPathContext().WorkingDirectory)
            {
                // Arrange
                var testArgs = new string[] { "/arg1", "/arg2", "/arg3" };

                // Act
                var parsedArgs = ParseArgs(testArgs);

                // Assert
                Assert.Equal(testArgs[0], parsedArgs[0]);
                Assert.Equal(testArgs[1], parsedArgs[1]);
                Assert.Equal(testArgs[2], parsedArgs[2]);
            }
        }

        [WindowsNTFact]
        public void NoArgs()
        {
            using (var workingDirectory = new SimpleTestPathContext().WorkingDirectory)
            {
                // Arrange
                var testArgs = new string[] { };

                // Act
                var parsedArgs = ParseArgs(testArgs);

                // Assert
                Assert.Equal(testArgs.Length, parsedArgs.Length);
            }
        }

        [WindowsNTFact]
        public void ResponseFileTooLarge()
        {
            using (var workingDirectory = new SimpleTestPathContext().WorkingDirectory)
            {
                // Arrange
                var testArgs = new string[] { "/arg1", "/arg2", "@responseFile1.rsp", "/arg3" };
                var responseFileContents = new StringBuilder();
                var tooLargeLength = TwoMegaBytesLength + 1;
                while (responseFileContents.Length < tooLargeLength)
                {
                    responseFileContents.Append("/responseFileArg ");
                }

                CreateResponseFile("responseFile1.rsp", responseFileContents.ToString());

                // Act
                string errorMessage = null;
                try
                {
                    var parsedArgs = ParseArgs(testArgs);
                }
                catch (ArgumentException e)
                {
                    errorMessage = e.Message;
                }

                // Assert
                Assert.Contains("Response file '@responseFile1.rsp' cannot be larger than 2mb", errorMessage);
            }
        }

        [WindowsNTFact]
        public void ResponseFileInvalid()
        {
            using (var workingDirectory = new SimpleTestPathContext().WorkingDirectory)
            {
                // Arrange
                var testArgs = new string[] { "/arg1", "/arg2", "@", "/arg3" };

                // Act
                string errorMessage = null;
                try
                {
                    var parsedArgs = ParseArgs(testArgs);
                }
                catch (ArgumentException e)
                {
                    errorMessage = e.Message;
                }

                // Assert
                Assert.Contains("Invalid response file, '@' does not exist", errorMessage);
            }
        }

        [WindowsNTFact]
        public void ResponseFileMultipleArgs()
        {
            using (var workingDirectory = new SimpleTestPathContext().WorkingDirectory)
            {
                // Arrange
                var testArgs = new string[] { "/arg1", "/arg2", "@responseFile1.rsp", "/arg3" };
                var responseFilePath = "responseFile1.rsp";
                var responseFileArg1 = "/responseFileArg1";
                var responseFileArg2 = "/responseFileArg2";
                var responseFileContent = string.Format("{0} {1}", responseFileArg1, responseFileArg2);
                CreateResponseFile(responseFilePath, responseFileContent);

                // Act
                var parsedArgs = ParseArgs(testArgs);

                // Assert
                Assert.Equal(testArgs[0], parsedArgs[0]);
                Assert.Equal(testArgs[1], parsedArgs[1]);
                Assert.Equal(responseFileArg1, parsedArgs[2]);
                Assert.Equal(responseFileArg2, parsedArgs[3]);
                Assert.Equal(testArgs[3], parsedArgs[4]);
            }
        }

        [WindowsNTFact]
        public void ResponseFileMultiLineArgs()
        {
            using (var workingDirectory = new SimpleTestPathContext().WorkingDirectory)
            {
                // Arrange
                var testArgs = new string[] { "/arg1", "/arg2", "@responseFile1.rsp", "/arg3" };
                var ResponseFilePath = "responseFile1.rsp";
                var ResponseFileArg1 = "/responseFileArg1";
                var ResponseFileArg2 = "/responseFileArg2";
                var ResponseFileArg3 = "/responseFileArg3";
                var responseFileContent = string.Format("{0}{1}{2} {3}", ResponseFileArg1, Environment.NewLine, ResponseFileArg2, ResponseFileArg3);
                CreateResponseFile(ResponseFilePath, responseFileContent);

                // Act
                var parsedArgs = ParseArgs(testArgs);

                // Assert
                Assert.Equal(testArgs[0], parsedArgs[0]);
                Assert.Equal(testArgs[1], parsedArgs[1]);
                Assert.Equal(ResponseFileArg1, parsedArgs[2]);
                Assert.Equal(ResponseFileArg2, parsedArgs[3]);
                Assert.Equal(ResponseFileArg3, parsedArgs[4]);
                Assert.Equal(testArgs[3], parsedArgs[5]);
            }
        }

        [WindowsNTFact]
        public void ResponseFileMultipleNested()
        {
            using (var workingDirectory = new SimpleTestPathContext().WorkingDirectory)
            {
                // Arrange
                var testArgs = new string[] { "/arg1", "/arg2", "@responseFile1.rsp", "/arg3" };
                var ResponseFile1Path = "responseFile1.rsp";
                var ResponseFileArg1 = "/responseFileArg1";
                var ResponseFileArg2 = "@responseFile2.rsp";
                var responseFileContent = string.Format("{0} {1}", ResponseFileArg1, ResponseFileArg2);
                CreateResponseFile(ResponseFile1Path, responseFileContent);

                var ResponseFile2Path = "responseFile2.rsp";
                var ResponseFile2Arg = "/responseFile2Arg";
                CreateResponseFile(ResponseFile2Path, ResponseFile2Arg);

                // Act
                var parsedArgs = ParseArgs(testArgs);

                // Assert
                Assert.Equal(testArgs[0], parsedArgs[0]);
                Assert.Equal(testArgs[1], parsedArgs[1]);
                Assert.Equal(ResponseFileArg1, parsedArgs[2]);
                Assert.Equal(ResponseFile2Arg, parsedArgs[3]);
                Assert.Equal(testArgs[3], parsedArgs[4]);
            }
        }

        [WindowsNTFact]
        public void ResponseFileInfiniteNestedRecursion()
        {
            using (var workingDirectory = new SimpleTestPathContext().WorkingDirectory)
            {
                // Arrange
                var testArgs = new string[] { "/arg1", "/arg2", "@responseFile1.rsp", "/arg3" };
                var ResponseFile1Path = "responseFile1.rsp";
                var ResponseFileArg1 = "/responseFileArg1";
                var ResponseFileArg2 = "@responseFile2.rsp";
                var responseFileContent = string.Format("{0} {1}", ResponseFileArg1, ResponseFileArg2);
                CreateResponseFile(ResponseFile1Path, responseFileContent);

                var ResponseFile2Path = "responseFile2.rsp";
                var ResponseFile2Arg = "@" + ResponseFile1Path;
                CreateResponseFile(ResponseFile2Path, ResponseFile2Arg);

                // Act
                string errorMessage = null;
                try
                {
                    var parsedArgs = ParseArgs(testArgs);
                }
                catch (ArgumentException e)
                {
                    errorMessage = e.Message;
                }

                // Assert
                Assert.Contains("No more than 3 nested response files are allowed", errorMessage);
            }
        }

        [WindowsNTFact]
        public void ResponseFileFullPath()
        {
            using (var workingDirectory = new SimpleTestPathContext().WorkingDirectory)
            {
                // Arrange
                var responseFileArg1 = "/responseFileArg1";
                var responseFilePath = Path.Combine(Environment.CurrentDirectory, "responseFile1.rsp");
                var testArgs = new string[] { "/arg1", "/arg2", "@" + responseFilePath, "/arg3" };
                CreateResponseFile(responseFilePath, responseFileArg1);

                // Act
                var parsedArgs = ParseArgs(testArgs);

                // Assert
                Assert.Equal(testArgs[0], parsedArgs[0]);
                Assert.Equal(testArgs[1], parsedArgs[1]);
                Assert.Equal(responseFileArg1, parsedArgs[2]);
                Assert.Equal(testArgs[3], parsedArgs[3]);
            }
        }

        [WindowsNTFact]
        public void WhiteSpaceArg()
        {
            using (var workingDirectory = new SimpleTestPathContext().WorkingDirectory)
            {
                // Arrange
                var argContent = " ";
                var testArgs = new string[] { argContent };

                // Act
                var parsedArgs = ParseArgs(testArgs);

                // Assert
                Assert.Equal(argContent, parsedArgs[0]);
                Assert.Equal(testArgs.Length, parsedArgs.Length);
            }
        }

        [WindowsNTFact]
        public void NullArg()
        {
            using (var workingDirectory = new SimpleTestPathContext().WorkingDirectory)
            {
                // Arrange
                string argContent = null;
                var testArgs = new string[] { argContent };

                // Act
                var parsedArgs = ParseArgs(testArgs);

                // Assert
                Assert.Equal(argContent, parsedArgs[0]);
                Assert.Equal(testArgs.Length, parsedArgs.Length);
            }
        }

        [WindowsNTFact]
        public void ResponseFileLargeArg()
        {
            using (var workingDirectory = new SimpleTestPathContext().WorkingDirectory)
            {
                // Arrange
                var testArgs = new string[] { "/arg1", "/arg2", "@responseFile1.rsp", "/arg3" };
                var responseFileArg1Builder = new StringBuilder();
                responseFileArg1Builder.Append("/");

                var largeLength = TwoMegaBytesLength - 1;
                while (responseFileArg1Builder.Length < largeLength)
                {
                    responseFileArg1Builder.Append("a");
                }

                var responseFileArg1 = responseFileArg1Builder.ToString();
                CreateResponseFile("responseFile1.rsp", responseFileArg1.ToString());

                // Act
                var parsedArgs = ParseArgs(testArgs);

                // Assert
                Assert.Equal(testArgs[0], parsedArgs[0]);
                Assert.Equal(testArgs[1], parsedArgs[1]);
                Assert.Equal(responseFileArg1, parsedArgs[2]);
                Assert.Equal(testArgs[3], parsedArgs[3]);
            }
        }

        private static void CreateResponseFile(string path, string contents)
        {
            File.WriteAllText(path, contents);
            Assert.True(File.Exists(path));
        }

        private static string[] ParseArgs(string[] args)
        {
            return CommandLineResponseFile.ParseArgsResponseFiles(args);
        }
    }
}
#endif
