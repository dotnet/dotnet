// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Windows.Input;

namespace NuGet.PackageManagement.UI
{
    public static class Commands
    {
        public static ICommand FocusOnSearchBox { get; } = new RoutedCommand();

        /// <summary>
        /// The parameter of this command is <see cref="PackageItemViewModel"/>
        /// </summary>
        public static ICommand UninstallPackageCommand { get; } = new RoutedCommand();

        /// <summary>
        /// The parameter of this command is <see cref="PackageItemViewModel"/>
        /// </summary>
        public static ICommand InstallPackageCommand { get; } = new RoutedCommand();

        // no parameters
        public static ICommand RestartSearchCommand { get; } = new RoutedCommand();

        // no parameters. Overridable by hosting app.
        public static ICommand ShowErrorsCommand { get; set; } = new RoutedCommand();

        /// <summary>
        /// Command parameter is search string
        /// </summary>
        public static ICommand SearchPackageCommand { get; set; } = new RoutedCommand();
    }
}
