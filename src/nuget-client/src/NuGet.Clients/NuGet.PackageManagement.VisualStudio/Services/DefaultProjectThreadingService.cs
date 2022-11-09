// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Threading;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.VisualStudio
{
    [Export(typeof(IVsProjectThreadingService))]
    internal class DefaultProjectThreadingService : IVsProjectThreadingService
    {
        public JoinableTaskFactory JoinableTaskFactory => NuGetUIThreadHelper.JoinableTaskFactory;
    }
}
