using System;
using System.Globalization;
using System.Reflection;

namespace NuGet.Common
{
    /// <summary>
    /// Represents a project in a sln file.
    /// </summary>
    internal class ProjectInSolution
    {
        /// <summary>
        /// The path of the project relative to the solution.
        /// </summary>
        public string RelativePath { get; private set; }

        /// <summary>
        /// Indicates if the project is a solution folder.
        /// </summary>
        public bool IsSolutionFolder { get; private set; }

        public ProjectInSolution(string relativePath, bool isSolutionFolder)
        {
            RelativePath = relativePath;
            IsSolutionFolder = isSolutionFolder;
        }
    }
}
