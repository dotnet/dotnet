using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using NuGet.CommandLine;

namespace NuGet.Common
{
    /// <summary>
    /// Represents the solution loaded from a sln file. We use the internal class
    /// Microsoft.Build.Construction.SolutionParser to parse sln files.
    /// </summary>
    internal class Solution : MSBuildUser
    {
        public List<ProjectInSolution> Projects { get; private set; }

        public Solution(string solutionFileName, string msbuildPath)
        {
            if (string.IsNullOrEmpty(msbuildPath))
            {
                throw new ArgumentNullException(nameof(msbuildPath));
            }

            _msbuildDirectory = msbuildPath;

            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolve);

            try
            {
                var msbuildAssembly = Assembly.LoadFrom(
                    Path.Combine(msbuildPath, "Microsoft.Build.dll"));
                switch (msbuildAssembly.GetName().Version.Major)
                {
                    case 4:
                    case 12:
                        LoadSolutionWithMsbuild4or12(msbuildAssembly, solutionFileName);
                        break;

                    case 14:
                    case 15:
                        LoadSolutionWithMsbuild14(msbuildAssembly, solutionFileName);
                        break;

                    default:
                        throw new InvalidOperationException(string.Format(
                            CultureInfo.InvariantCulture,
                            LocalizedResourceManager.GetString(nameof(NuGet.CommandLine.NuGetResources.Error_UnsupportedMsbuild)),
                            msbuildAssembly.FullName));
                }
            }
            finally
            {
                AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(AssemblyResolve);
            }
        }

        // Load the solution file with msbuild 4 or msbuild 12. In this case,
        // the internal class SolutionParser is used to parse the solution file
        private void LoadSolutionWithMsbuild4or12(Assembly msbuildAssembly, string solutionFileName)
        {
            var solutionParserType = msbuildAssembly.GetType(
                "Microsoft.Build.Construction.SolutionParser",
                throwOnError: true);
            var solutionReaderProperty = solutionParserType.GetProperty(
                "SolutionReader",
                BindingFlags.NonPublic | BindingFlags.Instance);
            var parseSolutionMethod = solutionParserType.GetMethod(
                "ParseSolution",
                BindingFlags.NonPublic | BindingFlags.Instance);
            var projectsProperty = solutionParserType.GetProperty(
                "Projects",
                BindingFlags.NonPublic | BindingFlags.Instance);
            var solutionParser = solutionParserType.GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic,
                binder: null, types: Type.EmptyTypes, modifiers: null).Invoke(null);
            using (var streamReader = new StreamReader(solutionFileName))
            {
                solutionReaderProperty.SetValue(solutionParser, streamReader, index: null);
                try
                {
                    parseSolutionMethod.Invoke(solutionParser, parameters: null);
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException ?? ex;
                }
            }

            var solutionDirectory = Path.GetDirectoryName(solutionFileName);

            // load projects
            var projectInSolutionType = msbuildAssembly.GetType(
                "Microsoft.Build.Construction.ProjectInSolution",
                throwOnError: true);
            var relativePathProperty = projectInSolutionType.GetProperty(
                "RelativePath",
                BindingFlags.NonPublic | BindingFlags.Instance);
            var projectTypeProperty = projectInSolutionType.GetProperty(
                "ProjectType",
                BindingFlags.NonPublic | BindingFlags.Instance);
            var projects = new List<ProjectInSolution>();
            foreach (var proj in (object[])projectsProperty.GetValue(solutionParser, index: null))
            {
                var projectType = projectTypeProperty.GetValue(proj, index: null).ToString();
                var isSolutionFolder = projectType.Equals("SolutionFolder", StringComparison.OrdinalIgnoreCase);
                var relativePath = (string)relativePathProperty.GetValue(proj, index: null);
                var absolutePath = Path.Combine(solutionDirectory, relativePath);
                projects.Add(new ProjectInSolution(relativePath, absolutePath, isSolutionFolder));
            }
            Projects = projects;
        }

        // Load the solution file using the public class SolutionFile in msbuild 14
        private void LoadSolutionWithMsbuild14(Assembly msbuildAssembly, string solutionFileName)
        {
            var isSolutionFilter = solutionFileName.EndsWith(".slnf", StringComparison.OrdinalIgnoreCase);
            var solutionFileType = msbuildAssembly.GetType("Microsoft.Build.Construction.SolutionFile");
            var parseMethod = solutionFileType.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public);
            var projectShouldBuildMethod = isSolutionFilter ? solutionFileType.GetMethod("ProjectShouldBuild", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                ?? throw new InvalidOperationException(string.Format(
                    CultureInfo.InvariantCulture,
                    LocalizedResourceManager.GetString(nameof(NuGet.CommandLine.NuGetResources.Error_UnsupportedMsBuildForSolutionFilter)),
                    msbuildAssembly.FullName))
                : null;

            dynamic solutionFile = parseMethod.Invoke(null, new object[] { solutionFileName });

            // load projects
            var projects = new List<ProjectInSolution>();
            foreach (var project in solutionFile.ProjectsInOrder)
            {
                var projectType = project.ProjectType.ToString();
                var isSolutionFolder = projectType.Equals("SolutionFolder", StringComparison.OrdinalIgnoreCase);

                try
                {
                    var projectShouldBuild = !isSolutionFilter || projectShouldBuildMethod.Invoke(solutionFile, new object[] { project.RelativePath });
                    if (projectShouldBuild)
                    {
                        var relativePath = project.RelativePath.Replace('\\', Path.DirectorySeparatorChar);
                        var absolutePath = project.AbsolutePath;

                        projects.Add(new ProjectInSolution(relativePath, absolutePath, isSolutionFolder));
                    }
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException ?? ex;
                }
            }
            Projects = projects;
        }
    }
}
