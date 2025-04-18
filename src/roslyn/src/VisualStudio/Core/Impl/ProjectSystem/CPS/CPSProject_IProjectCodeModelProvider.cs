﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable disable

using System;
using Microsoft.VisualStudio.LanguageServices.Implementation.CodeModel;
using Microsoft.VisualStudio.LanguageServices.Implementation.ProjectSystem.Extensions;

namespace Microsoft.VisualStudio.LanguageServices.Implementation.ProjectSystem.CPS;

internal sealed partial class CPSProject
{
    public EnvDTE.CodeModel GetCodeModel(EnvDTE.Project parent)
        => _projectCodeModel.GetOrCreateRootCodeModel(parent);

    public EnvDTE.FileCodeModel GetFileCodeModel(EnvDTE.ProjectItem item)
    {
        if (!item.TryGetFullPath(out var filePath))
        {
            return null;
        }

        return _projectCodeModel.GetOrCreateFileCodeModel(filePath, item);
    }

    private sealed class CPSCodeModelInstanceFactory : ICodeModelInstanceFactory
    {
        private readonly CPSProject _project;

        public CPSCodeModelInstanceFactory(CPSProject project)
            => _project = project;

        EnvDTE.FileCodeModel ICodeModelInstanceFactory.TryCreateFileCodeModelThroughProjectSystem(string filePath)
        {
            var projectItem = GetProjectItem(filePath);
            if (projectItem == null)
            {
                return null;
            }

            return _project._projectCodeModel.GetOrCreateFileCodeModel(filePath, projectItem);
        }

        private EnvDTE.ProjectItem GetProjectItem(string filePath)
        {
            var dteProject = _project._visualStudioWorkspace.TryGetDTEProject(_project._projectSystemProject.Id);
            if (dteProject == null)
            {
                return null;
            }

            return dteProject.FindItemByPath(filePath, StringComparer.OrdinalIgnoreCase);
        }
    }
}
