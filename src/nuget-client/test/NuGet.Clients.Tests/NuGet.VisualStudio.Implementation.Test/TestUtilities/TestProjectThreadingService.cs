// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.VisualStudio.Threading;
using NuGet.PackageManagement.VisualStudio;

namespace NuGet.VisualStudio.Implementation.Test.TestUtilities
{
    internal class TestProjectThreadingService : IVsProjectThreadingService
    {
        private readonly JoinableTaskContext _context;

        public TestProjectThreadingService()
        {
#pragma warning disable VSSDK005 // Avoid instantiating JoinableTaskContext
            _context = new JoinableTaskContext();
#pragma warning restore VSSDK005 // Avoid instantiating JoinableTaskContext
        }

        public JoinableTaskFactory JoinableTaskFactory => _context.Factory;
    }
}
