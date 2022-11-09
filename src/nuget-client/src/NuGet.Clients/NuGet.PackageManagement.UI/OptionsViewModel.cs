// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using NuGet.ProjectManagement;
using NuGet.Resolver;

namespace NuGet.PackageManagement.UI
{
    public class OptionsViewModel : INotifyPropertyChanged
    {
        public OptionsViewModel()
        {
            ShowPreviewWindow = true;
            ShowDeprecatedFrameworkWindow = true;
            CreateFileConflictActions();
            CreateDependencyBehaviors();
            ShowClassicOptions = true;
        }

        private void CreateFileConflictActions()
        {
            _fileConflicActions = new[]
                {
                    new FileConflictActionOptionItem(Resources.FileConflictAction_Prompt, FileConflictAction.PromptUser),
                    new FileConflictActionOptionItem(Resources.FileConflictAction_IgnoreAll, FileConflictAction.IgnoreAll),
                    new FileConflictActionOptionItem(Resources.FileConflictAction_OverwriteAll, FileConflictAction.OverwriteAll)
                };

            SelectedFileConflictAction = _fileConflicActions[0];
        }

        private void CreateDependencyBehaviors()
        {
            _dependencyBehaviors = new[]
                {
                    new DependencyBehaviorItem(Resources.DependencyBehavior_IgnoreDependencies, DependencyBehavior.Ignore),
                    new DependencyBehaviorItem(Resources.DependencyBehavior_Lowest, DependencyBehavior.Lowest),
                    new DependencyBehaviorItem(Resources.DependencyBehavior_HighestPatch, DependencyBehavior.HighestPatch),
                    new DependencyBehaviorItem(Resources.DependencyBehavior_HighestMinor, DependencyBehavior.HighestMinor),
                    new DependencyBehaviorItem(Resources.DependencyBehavior_Highest, DependencyBehavior.Highest)
                };
            SelectedDependencyBehavior = _dependencyBehaviors[1];
        }

        private FileConflictActionOptionItem[] _fileConflicActions;

        public IEnumerable<FileConflictActionOptionItem> FileConflictActions
        {
            get { return _fileConflicActions; }
        }

        private FileConflictActionOptionItem _selectedFileConflictAction;

        public FileConflictActionOptionItem SelectedFileConflictAction
        {
            get
            {
                return _selectedFileConflictAction;
            }
            set
            {
                if (_selectedFileConflictAction != value)
                {
                    _selectedFileConflictAction = value;
                    OnPropertyChanged(nameof(SelectedFileConflictAction));
                }
            }
        }

        private DependencyBehaviorItem[] _dependencyBehaviors;

        public IEnumerable<DependencyBehaviorItem> DependencyBehaviors
        {
            get { return _dependencyBehaviors; }
        }

        public event EventHandler SelectedChanged;

        private DependencyBehaviorItem _selectedDependencyBehavior;

        public DependencyBehaviorItem SelectedDependencyBehavior
        {
            get
            {
                return _selectedDependencyBehavior;
            }
            set
            {
                if (_selectedDependencyBehavior != value)
                {
                    _selectedDependencyBehavior = value;
                    if (SelectedChanged != null)
                    {
                        SelectedChanged(this, EventArgs.Empty);
                    }
                    OnPropertyChanged(nameof(SelectedDependencyBehavior));
                }
            }
        }

        private bool _showPreviewWindow;

        public bool ShowPreviewWindow
        {
            get
            {
                return _showPreviewWindow;
            }
            set
            {
                if (_showPreviewWindow != value)
                {
                    _showPreviewWindow = value;
                    OnPropertyChanged(nameof(ShowPreviewWindow));
                }
            }
        }

        private bool _showDeprecatedFrameworkWindow;

        public bool ShowDeprecatedFrameworkWindow
        {
            get
            {
                return _showDeprecatedFrameworkWindow;
            }
            set
            {
                if (_showDeprecatedFrameworkWindow != value)
                {
                    _showDeprecatedFrameworkWindow = value;
                    OnPropertyChanged(nameof(ShowDeprecatedFrameworkWindow));
                }
            }
        }

        private bool _removeDependencies;

        public bool RemoveDependencies
        {
            get
            {
                return _removeDependencies;
            }
            set
            {
                if (_removeDependencies != value)
                {
                    _removeDependencies = value;
                    OnPropertyChanged(nameof(RemoveDependencies));
                }
            }
        }

        private bool _forceRemove;

        public bool ForceRemove
        {
            get
            {
                return _forceRemove;
            }
            set
            {
                if (_forceRemove != value)
                {
                    _forceRemove = value;
                    OnPropertyChanged(nameof(ForceRemove));
                }
            }
        }

        private bool _showClassicOptions;

        /// <summary>
        /// Classic options include, file conflicts, dependency behavior, force remove, and remove dependencies.
        /// Classic options do not apply to build integrated projects.
        /// </summary>
        public bool ShowClassicOptions
        {
            get
            {
                return _showClassicOptions;
            }
            set
            {
                if (_showClassicOptions != value)
                {
                    _showClassicOptions = value;
                    OnPropertyChanged(nameof(ShowClassicOptions));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
