// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.DotNet.ScenarioTests.SdkTemplateTests;

public class SdkTemplateTest
{
    public DotNetSdkActions Commands { get; }
    public DotNetLanguage Language { get; }
    public bool NoHttps { get => TargetRid.Contains("osx"); }
    public string TargetRid { get; set; }
    public string TargetArchitecture { get => TargetRid.Split('-')[1]; }
    public string ScenarioName { get; }
    public DotNetSdkTemplate Template { get; }

    public SdkTemplateTest(string scenarioName, DotNetLanguage language, string targetRid, DotNetSdkTemplate template, DotNetSdkActions commands = DotNetSdkActions.None)
    {
        ScenarioName = scenarioName;
        Template = template;
        Language = language;
        Commands = commands;
        TargetRid = targetRid;
    }

    internal void Execute(DotNetSdkHelper dotNetHelper, string testRoot, string? framework = null)
    {
        // Don't use the cli language name in the project name because it may contain '#': https://github.com/dotnet/roslyn/issues/51692
        string projectName = $"{ScenarioName}_{Template}_{Language}";
        string customNewArgs = Template.IsAspNetCore() && NoHttps ? "--no-https" : string.Empty;
        string projectDirectory = Path.Combine(testRoot, projectName);
        if (framework != null)
        {
            customNewArgs += framework;
            projectDirectory += "_" + framework.Split(' ')[1];
        }

        Directory.CreateDirectory(projectDirectory);

        dotNetHelper.ExecuteNew(Template.GetName(), projectName, projectDirectory, Language.ToCliName(), customArgs: customNewArgs);

        if (Commands.HasFlag(DotNetSdkActions.AddClassLibRef))
        {
            dotNetHelper.ExecuteAddClassReference(projectDirectory);
        }
        if (Commands.HasFlag(DotNetSdkActions.Build))
        {
            dotNetHelper.ExecuteBuild(projectDirectory);
        }
        if (Commands.HasFlag(DotNetSdkActions.Run))
        {
            if (Template.IsAspNetCore())
            {
                dotNetHelper.ExecuteRunWeb(projectDirectory);
            }
            else if (Template.isUIApp())
            {
                dotNetHelper.ExecuteRunUIApp(projectDirectory);
            }
            else
            {
                dotNetHelper.ExecuteRun(projectDirectory);
            }
        }
        if (Commands.HasFlag(DotNetSdkActions.Publish))
        {
            dotNetHelper.ExecutePublish(projectDirectory);
        }
        if (Commands.HasFlag(DotNetSdkActions.PublishComplex))
        {
            dotNetHelper.ExecutePublish(projectDirectory, selfContained: false);        
            dotNetHelper.ExecutePublish(projectDirectory, selfContained: true, TargetRid);
            dotNetHelper.ExecutePublish(projectDirectory, selfContained: true, $"linux-{TargetArchitecture}");
        }
        if (Commands.HasFlag(DotNetSdkActions.PublishR2R))
        {
            dotNetHelper.ExecutePublish(projectDirectory, selfContained: true, $"linux-{TargetArchitecture}", trimmed: true, readyToRun: true);
        }
        if (Commands.HasFlag(DotNetSdkActions.Test))
        {
            dotNetHelper.ExecuteTest(projectDirectory);
        }
    }
}
