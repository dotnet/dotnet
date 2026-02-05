// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Threading.Tasks;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.UI.ViewModels
{
    public class ClearNuGetLocalsViewModel : ViewModelBase
    {
        private bool _isCommandComplete;
        private string? _commandCompleteText;
        private readonly Func<Task> _clearNuGetLocalsCommandExecute;
        private bool _isExecuting;
        private bool _hasError;

        public ClearNuGetLocalsViewModel(Func<Task> clearNuGetLocalsCommandExecute)
        {
            _clearNuGetLocalsCommandExecute = clearNuGetLocalsCommandExecute ?? throw new ArgumentNullException(nameof(clearNuGetLocalsCommandExecute));
        }

        public bool HasError
        {
            get
            {
                return _hasError;
            }
            set
            {
                SetAndRaisePropertyChanged(ref _hasError, value);
            }
        }


        public bool IsCommandComplete
        {
            get
            {
                return _isCommandComplete;
            }
            set
            {
                SetAndRaisePropertyChanged(ref _isCommandComplete, value);
            }
        }

        public string? CommandCompleteText
        {
            get
            {
                return _commandCompleteText;
            }
            set
            {
                SetAndRaisePropertyChanged(ref _commandCompleteText, value);
            }
        }

        internal Task Execute()
        {
            if (_isExecuting)
            {
                return Task.CompletedTask;
            }

            IsCommandComplete = false;
            CommandCompleteText = string.Empty;
            _isExecuting = true;

            var task = ExecuteBackgroundWorkAsync()
                .ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        string message = string.Format(
                            CultureInfo.CurrentCulture,
                            Resources.ShowMessage_LocalsCommandFailure,
                            DateTime.Now.ToString(Resources.Culture),
                            task.Exception.InnerException.Message);

                        OnCommandComplete(message, hasError: true);
                    }
                    else
                    {
                        string message = string.Format(
                            CultureInfo.CurrentCulture,
                            Resources.ShowMessage_LocalsCommandSuccess,
                            DateTime.Now.ToString(Resources.Culture));

                        OnCommandComplete(message, hasError: false);
                    }
                },
                TaskScheduler.FromCurrentSynchronizationContext());

            return task;
        }

        private async Task ExecuteBackgroundWorkAsync()
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                await _clearNuGetLocalsCommandExecute();
            });
        }

        private void OnCommandComplete(string message, bool hasError)
        {
            _isExecuting = false;
            CommandCompleteText = message;
            IsCommandComplete = true;
            HasError = hasError;
        }
    }
}
