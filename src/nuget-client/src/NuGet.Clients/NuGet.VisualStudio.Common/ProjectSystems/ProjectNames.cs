// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// Represents project names in the solution manager.
    /// </summary>
    public class ProjectNames : IEquatable<ProjectNames>
    {
        /// <summary>The full path and filename of the project.</summary>
        public string FullName { get; }

        /// <summary>The relative path and filename from the solution to the project.</summary>
        /// <remarks>If the solution is in <c>c:\sln\solution.sln</c> and the project is in <c>c:\sln\project\project.csproj</c>, this value will be <c>project\project.csproj</c>.</remarks>
        public string UniqueName { get; }

        /// <summary>The project's name</summary>
        /// <remarks>Generally this is the project's filename with the extension removed.</remarks>
        public string ShortName { get; }

        /// <summary>The "human readable" unique name.</summary>
        /// <remarks>Generally, it's the unique name with the project's extension removed.</remarks>
        public string CustomUniqueName { get; }

        /// <summary>The project GUID</summary>
        public string ProjectId { get; }

        public ProjectNames(
            string fullName,
            string uniqueName,
            string shortName,
            string customUniqueName,
            string projectId)
        {
            if (string.IsNullOrEmpty(fullName))
            {
                throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(fullName));
            }

            if (string.IsNullOrEmpty(uniqueName))
            {
                throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(uniqueName));
            }

            if (string.IsNullOrEmpty(shortName))
            {
                throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(shortName));
            }

            if (string.IsNullOrEmpty(customUniqueName))
            {
                throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(customUniqueName));
            }

            if (string.IsNullOrEmpty(projectId))
            {
                throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(projectId));
            }

            if (!Guid.TryParse(projectId, out var projectGuid) || projectGuid == Guid.Empty)
            {
                throw new ArgumentException(Resources.Argument_Invalid_GUID, nameof(projectId));
            }

            FullName = fullName;
            UniqueName = uniqueName;
            ShortName = shortName;
            CustomUniqueName = customUniqueName;
            ProjectId = projectGuid.ToString(); // normalize the project id, just in case.
        }

        /// <summary>
        /// Factory method initializing instance of <see cref="ProjectNames"/> with values retrieved from a DTE project.
        /// </summary>
        /// <param name="dteProject">DTE project to get project names for.</param>
        /// <returns>New instance of <see cref="ProjectNames"/>.</returns>
        public static async Task<ProjectNames> FromDTEProjectAsync(EnvDTE.Project dteProject, SVsSolution vsSolution)
        {
            Assumes.Present(dteProject);
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            string fullname = dteProject.FullName;
            string uniqueName = dteProject.GetUniqueName();
            string shortName = dteProject.GetName();
            string customUniqueName = await dteProject.GetCustomUniqueNameAsync();
            string projectId = GetProjectGuid(dteProject, vsSolution);

            return new ProjectNames(
                fullName: fullname,
                uniqueName: uniqueName,
                shortName: shortName,
                customUniqueName: customUniqueName,
                projectId: projectId);
        }

        private static string GetProjectGuid(EnvDTE.Project dteProject, SVsSolution vsSolution)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            // Each project system needs to implement its own DTE project implementation. Hence, custom project types
            // have the potential to not implement everything. Therefore, we need to try multiple ways to maximise the
            // chance we find one way that actually works.

            if (TryGetProjectGuidFromUniqueName(dteProject, (IVsSolution2)vsSolution, out Guid guid))
            {
                return guid.ToString();
            }

            if (TryGetProjectGuidFromFullName(dteProject, (IVsSolution5)vsSolution, out guid))
            {
                return guid.ToString();
            }

            throw new ArgumentException("Unable to find project guid");

            bool TryGetProjectGuidFromUniqueName(EnvDTE.Project project, IVsSolution2 vsSolution2, out Guid projectGuid)
            {
                try
                {
                    var uniqueName = project.UniqueName;
                    if (string.IsNullOrEmpty(uniqueName))
                    {
                        projectGuid = default(Guid);
                        return false;
                    }

                    ErrorHandler.ThrowOnFailure(vsSolution2.GetProjectOfUniqueName(uniqueName, out IVsHierarchy hierarchy));
                    ErrorHandler.ThrowOnFailure(vsSolution2.GetGuidOfProject(hierarchy, out projectGuid));

                    return true;
                }
                catch
                {
                    projectGuid = default(Guid);
                    return false;
                }
            }

            bool TryGetProjectGuidFromFullName(EnvDTE.Project project, IVsSolution5 vsSolution5, out Guid projectGuid)
            {
                try
                {
                    var fullName = project.FullName;
                    if (string.IsNullOrEmpty(fullName))
                    {
                        fullName = project.FileName;
                        if (string.IsNullOrEmpty(fullName))
                        {
                            projectGuid = default(Guid);
                            return false;
                        }
                    }

                    projectGuid = vsSolution5.GetGuidOfProjectFile(fullName);
                    return true;
                }
                catch
                {
                    projectGuid = default(Guid);
                    return false;
                }
            }
        }

        /// <summary>
        /// Factory method initializing instance of <see cref="ProjectNames"/> with values retrieved from <see cref="IVsSolution2"/> and <see cref="IVsHierarchy"/>.
        /// </summary>
        /// <param name="fullPath">Full path to the project file</param>
        /// <param name="vsSolution2">Instance of <see cref="IVsSolution2"/></param>
        /// <param name="cancellationToken">The cancellation token to cancel operation</param>
        /// <returns></returns>
        public static async Task<ProjectNames> FromIVsSolution2(string fullPath, IVsSolution2 vsSolution2, IVsHierarchy project, CancellationToken cancellationToken)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            ErrorHandler.ThrowOnFailure(vsSolution2.GetGuidOfProject(project, out Guid guid));
            ErrorHandler.ThrowOnFailure(vsSolution2.GetUniqueNameOfProject(project, out string uniqueName));
            ErrorHandler.ThrowOnFailure(project.GetProperty((uint)VSConstants.VSITEMID.Root, (int)__VSHPROPID.VSHPROPID_Name, out object projectNameObject));
            string shortName = (string)projectNameObject;
            string customUniqueName = GetCustomUniqueName(project);

            var projectNames = new ProjectNames(
                fullName: fullPath,
                uniqueName: uniqueName,
                shortName: shortName,
                customUniqueName: customUniqueName,
                projectId: guid.ToString());

            return projectNames;
        }

        public static async Task<ProjectNames> FromIVsSolution2(string fullPath, IVsSolution2 vsSolution2, CancellationToken cancellationToken)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            ErrorHandler.ThrowOnFailure(vsSolution2.GetProjectOfUniqueName(fullPath, out IVsHierarchy project));
            return await FromIVsSolution2(fullPath, vsSolution2, project, cancellationToken);
        }

        private static string GetCustomUniqueName(IVsHierarchy project)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var solutionPath = new Stack<string>();

            IVsHierarchy currentNode = project;
            while (true)
            {
                var result = currentNode.GetProperty((uint)VSConstants.VSITEMID.Root, (int)__VSHPROPID.VSHPROPID_ParentHierarchy, out object newNode);
                if (result == VSConstants.E_NOTIMPL)
                {
                    // E_NOTIMPL means the property doesn't exist, which means that the current node is the solution itself. We don't use that name.
                    break;
                }
                else if (result != VSConstants.S_OK)
                {
                    ErrorHandler.ThrowOnFailure(result);
                }

                ErrorHandler.ThrowOnFailure(currentNode.GetProperty((uint)VSConstants.VSITEMID.Root, (int)__VSHPROPID.VSHPROPID_Name, out object hierarchyName));
                solutionPath.Push((string)hierarchyName);

                currentNode = (IVsHierarchy)newNode;
            }

            var customUniqueName = string.Join("\\", solutionPath);
            return customUniqueName;
        }

        /// <summary>
        /// Two projects are equal if they share the same FullNames.
        /// </summary>
        public bool Equals(ProjectNames other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return StringComparer.OrdinalIgnoreCase.Equals(FullName, other.FullName);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ProjectNames);
        }

        public override int GetHashCode()
        {
            return FullName != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(FullName) : 0;
        }

        public static bool operator ==(ProjectNames left, ProjectNames right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ProjectNames left, ProjectNames right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return UniqueName;
        }
    }
}
