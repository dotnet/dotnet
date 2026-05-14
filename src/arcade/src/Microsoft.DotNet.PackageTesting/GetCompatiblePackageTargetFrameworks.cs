// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.Build.Tasks;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.DotNet.PackageTesting
{
    public class GetCompatiblePackageTargetFrameworks : BuildTask
    {
        private static List<NuGetFramework> allTargetFrameworks = new();
        private static Dictionary<NuGetFramework, HashSet<NuGetFramework>> packageTfmMapping = new();

        [Required]
        public string[] PackagePaths { get; set; }

        [Required]
        public string SupportedTestFrameworks { get; set; }

        [Output]
        public ITaskItem[] TestProjects { get; set; }

        public override bool Execute()
        {
            List<ITaskItem> testProjects = new();

            try
            {
                Initialize(SupportedTestFrameworks);
                string minDotnetTargetFramework = allTargetFrameworks.Where(t => t.Framework == ".NETCoreApp")
                    .OrderBy(t => t.Version)
                    .FirstOrDefault()?
                    .GetShortFolderName();

                foreach (var packagePath in PackagePaths)
                {
                    Package package = NupkgParser.CreatePackageObject(packagePath);

                    IEnumerable<NuGetFramework> testFrameworks = GetTestFrameworks(package, minDotnetTargetFramework);
                    testProjects.AddRange(CreateItemFromTestFramework(package.PackageId, package.Version, testFrameworks));
                }

                // Removing empty items.
                TestProjects = testProjects.Where(tfm => tfm.ItemSpec != "").ToArray();
            }
            catch (Exception e)
            {
                Log.LogErrorFromException(e, showStackTrace: false);
            }

            return !Log.HasLoggedErrors;
        }

        public static IEnumerable<NuGetFramework> GetTestFrameworks(Package package, string minDotnetTargetFramework)
        {
            List<NuGetFramework> frameworksToTest= new();
            IEnumerable<NuGetFramework> packageTargetFrameworks = package.FrameworksInPackage;

            // Testing the package installation on all tfms linked with package targetframeworks.
            foreach (var item in packageTargetFrameworks)
            {
                if (packageTfmMapping.ContainsKey(item))
                {
                    frameworksToTest.AddRange(packageTfmMapping[item]);
                }

                // Adding the frameworks in the packages to the test matrix.
                frameworksToTest.Add(item);
            }

            if (!string.IsNullOrEmpty(minDotnetTargetFramework) && frameworksToTest.Any(t => t.Framework == ".NETStandard"))
            {
                frameworksToTest.Add(NuGetFramework.Parse(minDotnetTargetFramework));
            }

            return frameworksToTest.Where(tfm => allTargetFrameworks.Contains(tfm)).Distinct();
        }

        public static void Initialize(string targetFrameworks)
        {
            // Defining the set of known frameworks that we care to test
            foreach (var tfm in targetFrameworks.Split(';'))
            {
                allTargetFrameworks.Add(NuGetFramework.Parse(tfm));
            }

            // creating a map framework in package => frameworks to test based on default compatibilty mapping.
            foreach (var item in DefaultFrameworkMappings.Instance.CompatibilityMappings)
            {
                NuGetFramework forwardTfm = item.SupportedFrameworkRange.Max;
                NuGetFramework reverseTfm = item.TargetFrameworkRange.Min;
                if (packageTfmMapping.ContainsKey(forwardTfm))
                {
                    packageTfmMapping[forwardTfm].Add(reverseTfm);
                }
                else
                {
                    packageTfmMapping.Add(forwardTfm, new HashSet<NuGetFramework> { reverseTfm });
                }
            }
        }

        private static List<ITaskItem> CreateItemFromTestFramework(string packageId, string version, IEnumerable<NuGetFramework> testFrameworks)
        {
            List<ITaskItem> testprojects = new();
            foreach (var framework in testFrameworks)
            {
                TaskItem supportedPackage = new(packageId);
                supportedPackage.SetMetadata("Version", version);
                supportedPackage.SetMetadata("TargetFramework", framework.ToString());
                supportedPackage.SetMetadata("TargetFrameworkShort", framework.GetShortFolderName());
                testprojects.Add(supportedPackage);
            }

            return testprojects;
        }
    }
}
