using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NuGet.Common
{
    public static class ProjectHelper
    {
        private static readonly HashSet<string> _supportedProjectExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) {
            ".csproj",
            ".vbproj",
            ".fsproj",
            ".btproj",
            ".vcxproj",
            ".jsproj",
            ".wixproj",
            ".nuproj",
            ".nfproj",
        };

        public static HashSet<string> SupportedProjectExtensions
        {
            get
            {
                return _supportedProjectExtensions;
            }
        }

        public static bool TryGetProjectFile(string directory, out string projectFile)
        {
            projectFile = null;
            var candidates = GetProjectFiles(directory).ToArray();
            if (candidates.Length == 1)
            {
                projectFile = candidates[0];
            }

            return !String.IsNullOrEmpty(projectFile);
        }

        public static IEnumerable<string> GetProjectFiles(string directory)
        {
            return _supportedProjectExtensions.SelectMany(x => Directory.GetFiles(directory, "*" + x));
        }


        public static string GetSolutionDir(string projectDirectory)
        {
            string path = projectDirectory;

            // Only look 4 folders up to find the solution directory
            const int maxDepth = 5;
            int depth = 0;
            do
            {
                if (SolutionFileExists(path))
                {
                    return path;
                }

                path = Path.GetDirectoryName(path);

                depth++;
                //When you get to c:\, the parent path is null.
            } while (depth < maxDepth && path != null);

            return null;
        }

        private static bool SolutionFileExists(string path)
        {
            return Directory.GetFiles(path, "*.sln").Any();
        }
    }
}
