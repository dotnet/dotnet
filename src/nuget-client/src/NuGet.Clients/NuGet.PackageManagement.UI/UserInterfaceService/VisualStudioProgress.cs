// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.VisualStudio.Shell;

namespace NuGet.PackageManagement.UI
{
    internal class VisualStudioDialogProgress : IProgress<ProgressDialogData>
    {
        private readonly IProgress<ThreadedWaitDialogProgressData> _progress;

        public VisualStudioDialogProgress(IProgress<ThreadedWaitDialogProgressData> progress)
        {
            _progress = progress;
        }

        public void Report(ProgressDialogData value)
        {
            _progress.Report(new ThreadedWaitDialogProgressData(value.WaitMessage, value.ProgressText, null, value.IsCancelable, value.CurrentStep,
                value.TotalSteps));
        }
    }
}
