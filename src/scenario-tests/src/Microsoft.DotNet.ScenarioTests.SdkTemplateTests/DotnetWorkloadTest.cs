// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.DotNet.ScenarioTests.SdkTemplateTests;

internal class DotnetWorkloadTest
{
    public DotNetSdkActions Commands { get; }
    public DotNetLanguage Language { get; }
    public bool NoHttps { get => TargetRid.Contains("osx"); }
    public string TargetRid { get; set; }
    public string TargetArchitecture { get => TargetRid.Split('-').Last(); }
    public string ScenarioName { get; }

    public DotnetWorkloadTest(string scenarioName, string targetRid, DotNetSdkActions commands = DotNetSdkActions.None)
    {
        ScenarioName = scenarioName;
        Commands = commands;
        TargetRid = targetRid;
    }

    internal void Execute(DotNetSdkHelper dotNetHelper, string testRoot, string workloadID)
    {
        string projectName = $"{ScenarioName}_Workload_{Commands.ToString()}";
        string projectDirectory = Path.Combine(testRoot, projectName);

        Directory.CreateDirectory(projectDirectory);

        if (Commands.HasFlag(DotNetSdkActions.FullWorkloadTest))
        {
            string originalSource = "";
            //running workload list before install to see if present from another source
            originalSource = dotNetHelper.ExecuteWorkloadList(projectDirectory, workloadID, false, firstRun: true);
            dotNetHelper.ExecuteWorkloadInstall(projectDirectory, workloadID);
            dotNetHelper.ExecuteWorkloadList(projectDirectory, workloadID, true, originalSource);
            dotNetHelper.ExecuteWorkloadUninstall(projectDirectory, workloadID);
            dotNetHelper.ExecuteWorkloadList(projectDirectory, workloadID, false, originalSource);
        }
        if (Commands.HasFlag(DotNetSdkActions.WorkloadInstall))
        {
            dotNetHelper.ExecuteWorkloadInstall(projectDirectory, workloadID);
        }
        if (Commands.HasFlag(DotNetSdkActions.WorkloadUninstall))
        {
            dotNetHelper.ExecuteWorkloadUninstall(projectDirectory, workloadID);
        }
    }
}
