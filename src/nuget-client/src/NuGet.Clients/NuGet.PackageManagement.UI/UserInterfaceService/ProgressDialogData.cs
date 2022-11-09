// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.PackageManagement.UI
{
    public class ProgressDialogData
    {
        public string WaitMessage;
        public string ProgressText;
        public bool IsCancelable;
        public int CurrentStep;
        public int TotalSteps;

        public ProgressDialogData(string waitMessage, string progressText = null, bool isCancelable = false)
        {
            WaitMessage = waitMessage;
            ProgressText = progressText;
            IsCancelable = isCancelable;
            CurrentStep = 0;
            TotalSteps = 0;
        }

        public ProgressDialogData(string waitMessage, string progressText, bool isCancelable, int currentStep, int totalSteps)
        {
            WaitMessage = waitMessage;
            ProgressText = progressText;
            IsCancelable = isCancelable;
            CurrentStep = currentStep;
            TotalSteps = totalSteps;
        }
    }

    public interface IProgressDialogData
    {
        string WaitMessage { get; }
        string ProgressText { get; }
        bool IsCancelable { get; }
        int CurrentStep { get; }
        int TotalSteps { get; }
    }
}
