// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace API.Test
{
    public static class VSProjectHelper
    {
        public static object NewProject(
            string templatePath,
            string outputPath,
            string templateName,
            string projectName,
            string solutionFolderName)
        {
            Utils.ThrowStringArgException(templatePath, nameof(templatePath));
            Utils.ThrowStringArgException(outputPath, nameof(outputPath));
            Utils.ThrowStringArgException(templateName, nameof(templateName));
            // projectName can be null or empty
            // solutionFolderName can be null or empty

            var name = projectName;
            if (string.IsNullOrEmpty(name))
            {
                var id = Utils.GetNewGUID();
                name = templateName + "_" + id;
            }

            return ThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                return await NewProjectAsync(
                    templatePath,
                    outputPath,
                    templateName,
                    name,
                    solutionFolderName);
            });
        }

        private static async Task<object> NewProjectAsync(
            string templatePath,
            string outputPath,
            string templateName,
            string projectName,
            string solutionFolderName)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            await VSSolutionHelper.EnsureSolutionAsync(outputPath);

            string projectTemplateFilePath = null;

            var dte = ServiceLocator.GetDTE();
            var dte2 = (DTE2)dte;
            var solution2 = dte2.Solution as Solution2;
            Project solutionFolderProject = null;

            dynamic newProject = null;

            projectTemplateFilePath = await GetProjectTemplateFilePathAsync(solution2, templateName, templatePath);

            var solutionDir = Path.GetDirectoryName(solution2.FullName);

            string destPath = null;
            if (string.IsNullOrEmpty(solutionFolderName))
            {
                destPath = Path.Combine(solutionDir, projectName);
            }
            else
            {
                destPath = Path.Combine(solutionDir, Path.Combine(solutionFolderName, projectName));
            }

            var window = dte2.ActiveWindow as Window2;

            solutionFolderProject = await CreateProjectFromTemplateAsync(
                solution2,
                solutionFolderName,
                projectTemplateFilePath,
                destPath,
                projectName);

            await CloseOpenDocumentsAsync(dte2);

            await Activatex86ConfigurationsAsync(dte2);

            window.SetFocus();

            if (solutionFolderProject != null)
            {
                newProject = await VSSolutionHelper.GetProjectAsync(solutionFolderProject, projectName);
            }
            else
            {
                newProject = await VSSolutionHelper.GetProjectAsync(solution2, projectName);
            }

            if (newProject == null)
            {
                throw new InvalidOperationException(
                    "Could not create new project or could not locate newly created project");
            }

            return newProject;
        }

        private static async Task<Project> CreateProjectFromTemplateAsync(
            Solution2 solution2,
            string solutionFolderName,
            string projectTemplateFilePath,
            string destPath,
            string projectName)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            if (string.IsNullOrEmpty(solutionFolderName))
            {
                solution2.AddFromTemplate(projectTemplateFilePath, destPath, projectName, Exclusive: false);
                return null;
            }
            else
            {
                var solutionFolderProject
                    = await VSSolutionHelper.GetSolutionFolderProjectAsync(solution2, solutionFolderName);

                var solutionFolder = (SolutionFolder)solutionFolderProject.Object;
                solutionFolder.AddFromTemplate(projectTemplateFilePath, destPath, projectName);

                return solutionFolderProject;
            }
        }

        private static async Task<string> GetProjectTemplateFilePathAsync(
            Solution2 solution2,
            string templateName,
            string templatePath)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            string projectTemplatePath = null;
            string projectTemplateFilePath = null;

            if (templateName.Equals("DNXClassLibrary", StringComparison.Ordinal)
                || templateName.Equals("DNXConsoleApp", StringComparison.Ordinal))
            {
                projectTemplatePath = templateName + ".vstemplate|FrameworkVersion=4.5";
                var lang = "CSharp/Web";

                projectTemplateFilePath = solution2.GetProjectItemTemplate(projectTemplatePath, lang);
            }
            else
            {
                projectTemplatePath = Path.Combine(templatePath, templateName + ".zip");

                var projectTemplateFiles = Directory.GetFiles(projectTemplatePath, "*.vstemplate");
                Debug.Assert(projectTemplateFiles.Length > 0);
                projectTemplateFilePath = projectTemplateFiles[0];
            }

            return projectTemplateFilePath;
        }

        private static async Task CloseOpenDocumentsAsync(DTE2 dte2)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            foreach (var document in dte2.Documents)
            {
                try
                {
                    ((Document)document).Close();
                }
                catch { }
            }
        }

        private static async Task Activatex86ConfigurationsAsync(DTE2 dte2)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            foreach (var config in dte2.Solution.SolutionBuild.SolutionConfigurations)
            {
                var solutionConfiguration = config as SolutionConfiguration2;
                if (solutionConfiguration.PlatformName.Equals("x86", StringComparison.Ordinal))
                {
                    solutionConfiguration.Activate();
                }
            }
        }
    }
}
