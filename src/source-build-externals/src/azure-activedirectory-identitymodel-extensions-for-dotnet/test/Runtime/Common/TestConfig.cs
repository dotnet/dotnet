//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace RuntimeCommon
{
    /// <summary>
    /// Stores basic information about the current performance test.
    /// </summary>
    public class TestConfig
    {
        /// <summary>
        /// Parses the command line args to retrieve the desired number of loops and iterations for the performance test.
        /// </summary>
        public static TestConfig ParseArgs(string[] args)
        {
            var testConfig = new TestConfig();
            if (args == null || args.Length < 2)
                return testConfig;

            var currentArg = 0;
            var displayArgs = false;
            for (int arg = 0; arg < args.Length; arg += 2)
            {
                if (args[currentArg].ToLower() == "-l")
                {
                    if (Int32.TryParse(args[currentArg + 1], out int numLoops))
                        testConfig.NumLoops = numLoops;
                }
                else if (args[currentArg].ToLower() == "-i")
                {
                    if (Int32.TryParse(args[currentArg + 1], out int numIterations))
                        testConfig.NumIterations = numIterations;
                }
                else if (args[currentArg].ToLower() == "-d")
                {
                    testConfig.LogDirectory = args[currentArg + 1];
                }
                else if ((args[currentArg].ToLower() == "-h") || (args[currentArg].ToLower() == @"\?"))
                {
                    displayArgs = true;
                }
                else
                {
                    testConfig.Logger.AppendLine($"Unknown Parameter: {args[currentArg]}");
                    displayArgs = true;
                }

                currentArg += 2;
            }

            if (displayArgs)
            {
                testConfig.Logger.AppendLine("");
                testConfig.Logger.AppendLine("*******************************************************");
                testConfig.Logger.AppendLine($"Args: -l <numLoops>");
                testConfig.Logger.AppendLine($"Args: -i <numIterations>");
                testConfig.Logger.AppendLine($"Args: -d <directory for logs>");
                testConfig.Logger.AppendLine($"Args: -h help");
                testConfig.Logger.AppendLine($"Args: /? help");
                testConfig.Logger.AppendLine("");
                testConfig.Logger.AppendLine("*******************************************************");
            }

            return testConfig;
        }

        /// <summary>
        /// Prepares the specified test cases for testing.
        /// </summary>
        public static IList<TestRun> SetupTestRuns(List<TestExecutor> testExecutors)
        {
            var perfTestCases = new List<TestRun>();
            for (int i = 0; i < testExecutors.Count; i++)
            {
                perfTestCases.Add(new TestRun
                {
                    NumberOfIterationsRun = 0,
                    TotalTime = 0,
                    Name = testExecutors[i].Method.Name,
                    TestExecutor = testExecutors[i]
                });
            }

            return perfTestCases;
        }

        public bool CacheSignatureProviders { get; set; } = true;

        public string LogDirectory { get; set; } = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\..\TestResults";

        public TestLogger Logger { get; set; } = new TestLogger();

        public string Version { get; set; }

        public int NumLoops { get; set; } = 10;

        public int NumIterations { get; set; } = 10;

        public override string ToString()
        {
            return $"Name: {Version}, NumIterations: '{NumIterations}', NumLoops: '{NumLoops}'.";
        }
    }
}
