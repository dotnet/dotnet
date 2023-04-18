// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollector.InProcDataCollector;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.InProcDataCollector;

namespace Coverlet.Collector.DataCollection;

// This class MUST have the same full name as
// https://github.com/tonerdo/coverlet/blob/master/src/coverlet.collector/InProcDataCollection/CoverletInProcDataCollector.cs
// to mimic real behavior
public class CoverletInProcDataCollector : InProcDataCollection
{
    public void Initialize(IDataCollectionSink dataCollectionSink)
    {
        throw new NotImplementedException();
    }

    public void TestCaseEnd(TestCaseEndArgs testCaseEndArgs)
    {
        throw new NotImplementedException();
    }

    public void TestCaseStart(TestCaseStartArgs testCaseStartArgs)
    {
        throw new NotImplementedException();
    }

    public void TestSessionEnd(TestSessionEndArgs testSessionEndArgs)
    {
        throw new NotImplementedException();
    }

    public void TestSessionStart(TestSessionStartArgs testSessionStartArgs)
    {
        throw new NotImplementedException();
    }
}
