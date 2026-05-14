// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;

namespace Microsoft.DotNet.SourceBuild.Tests;

public class TestScenario
{
    public DotNetActions Commands { get; }
    public bool NoHttps { get; set; } = Config.TargetRid.Contains("osx");
    public string ScenarioName { get; }
    public DotNetTemplate Template { get; }
    public Action<string>? Validate { get; }

    public TestScenario(string scenarioName, DotNetTemplate template, DotNetActions commands = DotNetActions.None, Action<string>? validate = null)
    {
        ScenarioName = scenarioName;
        Template = template;
        Commands = commands;
        Validate = validate;
    }

    internal void Execute(DotNetHelper dotNetHelper)
    {
        string projectName = $"{ScenarioName}_{Template}_CSharp";
        string customNewArgs = NoHttps ? "--no-https" : string.Empty;
        dotNetHelper.ExecuteNew(Template.GetName(), projectName, "C#", customArgs: customNewArgs);

        if (Commands.HasFlag(DotNetActions.Build))
        {
            dotNetHelper.ExecuteBuild(projectName);
        }
        if (Commands.HasFlag(DotNetActions.Run))
        {
            dotNetHelper.ExecuteRunWeb(projectName, Template);
        }
        if (Commands.HasFlag(DotNetActions.Publish))
        {
            dotNetHelper.ExecutePublish(projectName, Template);
        }
        if (Commands.HasFlag(DotNetActions.PublishSelfContained))
        {
            dotNetHelper.ExecutePublish(projectName, Template, selfContained: true, rid: Config.TargetRid);
        }

        string projectPath = Path.Combine(DotNetHelper.ProjectsDirectory, projectName);
        Validate?.Invoke(projectPath);
    }
}
