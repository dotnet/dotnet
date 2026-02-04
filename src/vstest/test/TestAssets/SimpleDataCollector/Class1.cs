// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollector.InProcDataCollector;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.InProcDataCollector;

namespace SimpleDataCollector
{
    /// <summary>
    /// The ti listener tests.
    /// </summary>
    public class SimpleDataCollector : InProcDataCollection
    {
        private readonly string _fileName;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleDataCollector"/> class.
        /// </summary>
        public SimpleDataCollector()
        {
            _fileName = Path.Combine(Path.GetTempPath(), "inproctest.txt");
        }

        public void Initialize(IDataCollectionSink dataCollectionSink)
        {
            // Do Nothing
        }

        /// <summary>
        /// The test session start.
        /// </summary>
        /// <param name="testSessionStartArgs">
        /// The test session start args.
        /// </param>
        public void TestSessionStart(TestSessionStartArgs testSessionStartArgs)
        {
            Console.WriteLine(testSessionStartArgs.Configuration);
            File.WriteAllText(_fileName, "TestSessionStart : " + testSessionStartArgs.Configuration + "\r\n");
            var appDomainFilePath = Environment.GetEnvironmentVariable("TEST_ASSET_APPDOMAIN_COLLECTOR_PATH") ?? Path.Combine(Path.GetTempPath(), "appdomain_datacollector.txt");
            File.WriteAllText(appDomainFilePath, "AppDomain FriendlyName: " + AppDomain.CurrentDomain.FriendlyName);
        }

        /// <summary>
        /// The test case start.
        /// </summary>
        /// <param name="testCaseStartArgs">
        /// The test case start args.
        /// </param>
        public void TestCaseStart(TestCaseStartArgs testCaseStartArgs)
        {
            Console.WriteLine(
                "TestCase Name : {0}, TestCase ID:{1}",
                testCaseStartArgs.TestCase.DisplayName,
                testCaseStartArgs.TestCase.Id);
            File.AppendAllText(_fileName, "TestCaseStart : " + testCaseStartArgs.TestCase.DisplayName + "\r\n");
        }

        /// <summary>
        /// The test case end.
        /// </summary>
        /// <param name="testCaseEndArgs">
        /// The test case end args.
        /// </param>
        public void TestCaseEnd(TestCaseEndArgs testCaseEndArgs)
        {
            Console.WriteLine("TestCase Name:{0}, TestCase ID:{1}, OutCome:{2}", testCaseEndArgs.DataCollectionContext.TestCase.DisplayName, testCaseEndArgs.DataCollectionContext.TestCase.Id, testCaseEndArgs.TestOutcome);
            File.AppendAllText(_fileName, "TestCaseEnd : " + testCaseEndArgs.DataCollectionContext.TestCase.DisplayName + "\r\n");
        }

        /// <summary>
        /// The test session end.
        /// </summary>
        /// <param name="testSessionEndArgs">
        /// The test session end args.
        /// </param>
        public void TestSessionEnd(TestSessionEndArgs testSessionEndArgs)
        {
            Console.WriteLine("TestSession Ended");
            File.AppendAllText(_fileName, "TestSessionEnd");
        }
    }
}
