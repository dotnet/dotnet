using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.DotNet.ScenarioTests.Common;

/// <summary>
/// Serves as a way to ferry test runner input (e.g. install locations)
/// to the tests
/// </summary>
public class ScenarioTestFixture
{
    public const string DotNetRootEnvironmentVariable = "SCENARIO_TESTS_DOTNETROOT";
    public const string SdkVersionEnvironmentVariable = "SCENARIO_TESTS_SDKVERSION";
    public const string TestRootEnvironmentVariable = "SCENARIO_TESTS_TESTROOT";
    public const string TargetRidEnvironmentVariable = "SCENARIO_TESTS_TARGETRID";
    public const string PortableRidEnvironmentVariable = "SCENARIO_TESTS_PORTABLERID";
    public const string BinlogDirEnvironmentVariable = "SCENARIO_TESTS_BINLOG_DIR";

    public string? SdkVersion { get; }

    public string DotNetRoot { get; }

    public string TestRoot { get; }

    public string TargetRid { get; }

    public string TargetArchitecture { get => TargetRid.Split('-').Last(); }

    public string? PortableRid { get; }

    public string? BinlogDir { get; }

    public ScenarioTestFixture()
    {
        string? dotnetRoot = Environment.GetEnvironmentVariable(DotNetRootEnvironmentVariable);
        string? testRoot = Environment.GetEnvironmentVariable(TestRootEnvironmentVariable);
        string? sdkVersion = Environment.GetEnvironmentVariable(SdkVersionEnvironmentVariable);
        string? targetRid = Environment.GetEnvironmentVariable(TargetRidEnvironmentVariable);
        string? portableRid = Environment.GetEnvironmentVariable(PortableRidEnvironmentVariable);
        string? binlogDir = Environment.GetEnvironmentVariable(BinlogDirEnvironmentVariable);

        if (string.IsNullOrEmpty(dotnetRoot) || string.IsNullOrEmpty(testRoot) || string.IsNullOrEmpty(targetRid))
        {
            throw new ArgumentException("Please specify SDK root,Test Root and Target Rid");
        }

        SdkVersion = sdkVersion;
        DotNetRoot = dotnetRoot;
        TestRoot = testRoot;
        TargetRid = targetRid;
        PortableRid = portableRid;
        BinlogDir = binlogDir;
    }
}
