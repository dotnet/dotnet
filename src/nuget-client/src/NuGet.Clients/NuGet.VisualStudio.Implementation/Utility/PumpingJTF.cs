// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.VisualStudio.Threading;

namespace NuGet.VisualStudio
{
    // Note that accessing DTE objects from a background thread is a bad thing. It violates VS threading rule #1 and does not switch to main thread
    // beforing accessing a method with STA requirement. This will RPC its way into main thread and might do work that is unrelated.
    // Since, the VS Extensibility API and IWizard methods are called from the UI thread and it should be synchronous, plus, powershell scripts
    // need to be executed on the pipeline execution thread, and has access to and in some cases, accesses EnvDTE.Project object;
    // we have to use this pumping JTF, instead of ThreadHelper.JoinableTaskFactory, in order to overcome the deadlock.
    // So, DO NOT use this class, unless it is absolutely necessary, because, generally, a deadlock is a clue to fix code to apply rule #1
    /// <summary>
    /// DO NOT use this class, unless, it is absolutely necessary
    /// This class is used to have a pumping JTF, in order, to not allow access to DTE objects from a background
    /// thread
    /// </summary>
    internal class PumpingJTF : DelegatingJoinableTaskFactory
    {
        public PumpingJTF(JoinableTaskFactory joinableTaskFactory)
            : base(joinableTaskFactory)
        {
        }

        // This override effectively keeps the filtered message pump running.
        // In other words, accessing DTE objects like EnvDTE.Project from a background thread, such as powershell pipeline execution thread
        // will not deadlock, when the UI thread is under this JTF's Run method.
        // Note that accessing DTE objects from a background thread is a bad thing. It violates VS threading rule #1 and does not switch to main thread
        // beforing accessing a method with STA requirement. This will RPC its way into main thread and might do work that is unrelated.
        // Since, the VS Extensibility API and IWizard methods are called from the UI thread and it should be synchronous, plus, powershell scripts
        // need to be executed on the pipeline execution thread, and has access to and in some cases, accesses EnvDTE.Project object;
        // we have to use this pumping JTF, instead of ThreadHelper.JoinableTaskFactory, in order to overcome the deadlock.
        // So, DO NOT use this class, unless it is absolutely necessary, because, generally, a deadlock is a clue to fix code to apply rule #1
        protected override void WaitSynchronously(Task task)
        {
            WaitSynchronouslyCore(task);
        }
    }
}
