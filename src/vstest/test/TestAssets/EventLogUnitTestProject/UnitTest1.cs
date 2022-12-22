// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventLogUnitTestProject;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestMethod1()
    {
        EventLog.WriteEntry("Application", "Application", EventLogEntryType.Error, 110);
        EventLog.WriteEntry("Application", "Application", EventLogEntryType.Error, 111);
        EventLog.WriteEntry("Application", "Application", EventLogEntryType.Error, 112);
    }

    [TestMethod]
    public void TestMethod2()
    {
        EventLog.WriteEntry("Application", "Application", EventLogEntryType.Error, 220);
        EventLog.WriteEntry("Application", "Application", EventLogEntryType.Error, 221);
        EventLog.WriteEntry("Application", "Application", EventLogEntryType.Error, 222);
        EventLog.WriteEntry("Application", "Application", EventLogEntryType.Error, 223);
    }

    [TestMethod]
    public void TestMethod3()
    {
        EventLog.WriteEntry("Application", "Application", EventLogEntryType.Error, 330);
        EventLog.WriteEntry("Application", "Application", EventLogEntryType.Error, 331);
        EventLog.WriteEntry("Application", "Application", EventLogEntryType.Error, 332);
    }
}
