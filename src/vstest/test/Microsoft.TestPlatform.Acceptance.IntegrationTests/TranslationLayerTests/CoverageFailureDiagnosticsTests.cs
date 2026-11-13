// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests.TranslationLayerTests;

[TestClass]
public class CoverageFailureDiagnosticsTests
{
    [TestMethod]
    public void ProcessorContextShouldExcludeUnrelatedTraceAndPreserveExceptionContext()
    {
        var context = CoverageFailureDiagnostics.SelectProcessorContext(new[]
        {
            "TpTrace Verbose: unrelated payload",
            "TpTrace Error: TestRunAttachmentsProcessingManager: merge failed",
            "System.IO.IOException: report locked",
            "   at Merge()",
            "TpTrace Verbose: unrelated next event",
            "not part of the exception",
        }).ToArray();

        CollectionAssert.AreEqual(new[]
        {
            "TpTrace Error: TestRunAttachmentsProcessingManager: merge failed",
            "System.IO.IOException: report locked",
            "   at Merge()",
        }, context);
    }

    [TestMethod]
    public void ProcessorContextShouldBeBoundedAndRedactSecretsBeforeTruncating()
    {
        var line = "TestRunAttachmentsProcessingManager: {\"Secret\":\"" + new string('s', 3000) + "\",\"Path\":\"report.coverage\"}";
        var context = CoverageFailureDiagnostics.SelectProcessorContext(Enumerable.Repeat(line, 50)).ToArray();

        Assert.AreEqual(40, context.Length);
        Assert.IsTrue(context.All(l => l.Contains("<redacted>") && l.Contains("report.coverage")));
        Assert.IsFalse(context.Any(l => l.Contains(new string('s', 20))));
        Assert.IsTrue(CoverageFailureDiagnostics.RedactAndLimit(new string('x', 3000)).Length < 2100);
    }

    [TestMethod]
    public void MissingProcessorLogShouldBeReportedWithoutThrowing()
    {
        var missing = Path.Combine(AppContext.BaseDirectory, $"missing-{Guid.NewGuid():N}.log");

        var diagnostic = CoverageFailureDiagnostics.ReadProcessorContext(missing).Single();

        StringAssert.Contains(diagnostic, "diagnostics unavailable");
        StringAssert.Contains(diagnostic, missing);
    }
}
