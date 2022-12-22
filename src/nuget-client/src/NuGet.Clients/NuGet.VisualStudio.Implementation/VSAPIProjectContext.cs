// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Xml.Linq;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.ProjectManagement;

namespace NuGet.VisualStudio
{
    internal sealed class VSAPIProjectContext : IMSBuildNuGetProjectContext
    {
        private Guid _operationID;

        public VSAPIProjectContext()
            : this(false, false)
        {
        }

        public VSAPIProjectContext(bool skipAssemblyReferences, bool bindingRedirectsDisabled)
        {
            SourceControlManagerProvider = ServiceLocator.GetComponentModelService<ISourceControlManagerProvider>();
            SkipAssemblyReferences = skipAssemblyReferences;
            BindingRedirectsDisabled = bindingRedirectsDisabled;
        }

        public void Log(MessageLevel level, string message, params object[] args)
        {
            // No logging needed when using the API
        }

        public void Log(ILogMessage message)
        {
            // No logging needed when using the API
        }

        public FileConflictAction ResolveFileConflict(string message)
        {
            return FileConflictAction.IgnoreAll;
        }

        public PackageExtractionContext PackageExtractionContext { get; set; }

        public ISourceControlManagerProvider SourceControlManagerProvider { get; }

        public ExecutionContext ExecutionContext => null;

        public bool SkipAssemblyReferences { get; }

        public bool BindingRedirectsDisabled { get; }

        public bool SkipBindingRedirects { get; set; }

        public XDocument OriginalPackagesConfig { get; set; }

        public void ReportError(string message)
        {
            // no-op
            Debug.Fail(message);
        }

        public void ReportError(ILogMessage message)
        {
            // no-op
            Debug.Fail(message.FormatWithCode());
        }

        public NuGetActionType ActionType { get; set; }

        public Guid OperationId
        {
            get
            {
                if (_operationID == Guid.Empty)
                {
                    _operationID = Guid.NewGuid();
                }
                return _operationID;
            }
            set
            {
                _operationID = value;
            }
        }
    }
}
