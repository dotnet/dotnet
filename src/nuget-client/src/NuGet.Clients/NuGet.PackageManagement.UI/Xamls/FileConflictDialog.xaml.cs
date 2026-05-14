// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualStudio.PlatformUI;
using NuGet.ProjectManagement;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// Interaction logic for FileConflictDialog.xaml
    /// </summary>
    public partial class FileConflictDialog : DialogWindow
    {
        public FileConflictDialog()
        {
            InitializeComponent();
        }

        public string Question
        {
            get { return QuestionText.Text; }
            set { QuestionText.Text = value; }
        }

        public FileConflictAction UserSelection { get; private set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var tagValue = (string)button.Tag;

            UserSelection = (FileConflictAction)Enum.Parse(typeof(FileConflictAction), tagValue);

            DialogResult = true;
        }
    }
}
