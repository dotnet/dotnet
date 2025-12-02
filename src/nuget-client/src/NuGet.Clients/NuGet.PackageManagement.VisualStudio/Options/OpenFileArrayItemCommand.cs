// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Utilities.UnifiedSettings;
using NuGet.PackageManagement.VisualStudio.IDE;
using Resx = NuGet.PackageManagement.VisualStudio.Strings;

namespace NuGet.PackageManagement.VisualStudio.Options
{
    internal class OpenFileArrayItemCommand : IArrayItemCommand
    {
        private IDocumentOpener _documentOpener;

        public const string FILE_PATH = "filePath";

        public string Title => Resx.VSOptions_Button_Open;

        public string Description => "";

        public int DefaultActionPriority => 1;

        public OpenFileArrayItemCommand(IDocumentOpener documentOpener)
        {
            _documentOpener = documentOpener;
        }

        public void Invoke(IDictionary<string, object> arrayItemContent)
        {
            var path = arrayItemContent[FILE_PATH] as string;
            _documentOpener.OpenDocument(path);
        }

        /// <summary>
        /// Configuration Files provided by the NuGet SDK are files that exist.
        /// If configuration files are created or deleted while the IDE is open, a refresh is expected.
        /// Therefore, we can assume all of these files provided to the command exist, so we can keep the command enabled.
        /// </summary>
        public Task<bool> IsEnabledAsync(IDictionary<string, object> arrayItemContent, CancellationToken cancellationToken)
        {
#pragma warning disable VSTHRD003 // Avoid awaiting foreign Tasks
            return TaskResult.True;
#pragma warning restore VSTHRD003 // Avoid awaiting foreign Tasks
        }
    }
}
