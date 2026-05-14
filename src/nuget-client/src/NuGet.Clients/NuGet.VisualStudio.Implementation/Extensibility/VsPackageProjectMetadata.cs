// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.VisualStudio.Etw;

namespace NuGet.VisualStudio.Implementation.Extensibility
{
    internal class VsPackageProjectMetadata : IVsPackageProjectMetadata
    {
        private string _batchId;
        private string _projectName;

        public VsPackageProjectMetadata() : this(string.Empty, string.Empty)
        { }

        public VsPackageProjectMetadata(string id, string name)
        {
            _batchId = id ?? string.Empty;
            _projectName = name ?? string.Empty;
        }

        public string BatchId
        {
            get
            {
                const string eventName = nameof(IVsPackageProjectMetadata) + "." + nameof(BatchId);
                NuGetETW.ExtensibilityEventSource.Write(eventName, NuGetETW.InfoEventOptions);
                return _batchId;
            }
        }

        public string ProjectName
        {
            get
            {
                const string eventName = nameof(IVsPackageProjectMetadata) + "." + nameof(ProjectName);
                NuGetETW.ExtensibilityEventSource.Write(eventName, NuGetETW.InfoEventOptions);
                return _projectName;
            }
        }
    }
}
