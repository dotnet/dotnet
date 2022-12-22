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

using System.Collections.Generic;
using System.Diagnostics;

namespace RuntimeCommon
{
    /// <summary>
    /// Runs a list of TestRun, calling <see cref="TestRun.TestExecutor"/> <see cref="TestExecutor"/> passing <see cref="TestData"/>.
    /// </summary>
    public static class TestRunner
    {
        /// <summary>
        /// Calls: <see cref="TestRun.TestExecutor"/> passing <see cref="TestRunData"/>.
        /// </summary>
        public static void Run(TestConfig testConfig, IList<TestRun> testRuns, TestRunData testData)
        {
            var totalIterations = 0;
            testConfig.Logger.AppendLine("Runner,Version,Loops,Iterations,Total,Time(milliseconds),Per Second,% Above Minimum");
            for (int loop = 0; loop < testConfig.NumLoops; loop++)
            {
                RunTestsShuffled(testRuns, loop, testData, testConfig.NumIterations);
                totalIterations += testConfig.NumIterations;
            }

            int minCase = 0;
            int maxCase = 0;

            for (int i = 0; i < testRuns.Count; i++)
            {
                if (testRuns[i].TotalTime >= testRuns[maxCase].TotalTime)
                    maxCase = i;

                if (testRuns[i].TotalTime <= testRuns[minCase].TotalTime)
                    minCase = i;
            }

            for (int i = 0; i < testRuns.Count; i++)
            {
                var tokensPerSecond = string.Format("{0,12:F4}", testRuns[i].NumberOfIterationsRun / testRuns[i].TotalTime * 1000);
                var aboveMinimum = string.Format("{0,12:F2}", (testRuns[i].TotalTime / testRuns[minCase].TotalTime * 100) - 100);
                testConfig.Logger.AppendLine(
                    $"{testRuns[i].Name}," +
                    $"{testConfig.Version}," +
                    $"{testConfig.NumLoops}," +
                    $"{testConfig.NumIterations}," +
                    $"{testRuns[i].NumberOfIterationsRun}," +
                    $"{testRuns[i].TotalTime}," +
                    $"{tokensPerSecond}," +
                    $"{aboveMinimum}");
            }
        }

        /// <summary>
        /// Loops threw tests from <paramref name="start"/> for the number of runs.
        /// </summary>
        /// <param name="testRuns"></param>
        /// <param name="start"></param>
        /// <param name="runData"></param>
        /// <param name="numIterations"></param>
        private static void RunTestsShuffled(IList<TestRun> testRuns, int start, TestRunData testData, int numIterations)
        {
            for (int index = 0; index < testRuns.Count; index++)
            {
                var testRun = testRuns[(start + index) % testRuns.Count];
                testRun.TotalTime += ExecuteRun(testRun, testData, numIterations);
                testRun.NumberOfIterationsRun += numIterations;
            }
        }

        /// <summary>
        /// Executes the run and remembers the time.
        /// </summary>
        /// <param name="testRun"></param>
        /// <param name="testExceutor"></param>
        /// <param name="numIterations"></param>
        /// <returns></returns>
        private static double ExecuteRun(TestRun testRun, TestRunData testData, int numIterations)
        {
            Stopwatch sw = Stopwatch.StartNew();
            testRun.TestExecutor(testData);
            sw.Stop();
            return sw.Elapsed.TotalMilliseconds;
        }
    }
}
