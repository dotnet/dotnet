// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
public class DiagFlagPatternTests
{
    // Positive cases — should match (diag flag present as standalone token)
    [TestMethod]
    [DataRow("/diag")]
    [DataRow("/diag:foo.txt")]
    [DataRow("/diag=foo.txt")]
    [DataRow("--diag")]
    [DataRow("--diag bar.txt")]
    [DataRow("-diag:x")]
    [DataRow("--DIAG")]
    [DataRow("/DIAG:path")]
    [DataRow("assembly.dll /diag:log.txt")]
    [DataRow("assembly.dll --diag log.txt --logger trx")]
    [DataRow("/diag:log.txt assembly.dll")]
    public void DiagFlagPattern_MatchesStandaloneDiagFlags(string arguments)
        => Assert.IsTrue(IntegrationTestBase.DiagFlagPattern.IsMatch(arguments), $"Expected a match for: {arguments}");

    // Negative cases — should NOT match
    [TestMethod]
    [DataRow("--logger:my-diagnostics.trx")]
    [DataRow("--diagnostic")]
    [DataRow("/diagnostics")]
    [DataRow("--no-diag-mode")]
    [DataRow("--logger:console;verbosity=normal")]
    [DataRow("nodiagsuffix")]
    [DataRow("assembly.dll --logger:my-diag.trx")]
    public void DiagFlagPattern_DoesNotMatchNonDiagTokens(string arguments)
        => Assert.IsFalse(IntegrationTestBase.DiagFlagPattern.IsMatch(arguments), $"Expected no match for: {arguments}");
}
