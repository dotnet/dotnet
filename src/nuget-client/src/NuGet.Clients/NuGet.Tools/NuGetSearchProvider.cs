// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace NuGetVSExtension
{
    [Guid("042C2B4B-C7F7-49DB-B7A2-402EB8DC7892")]
    public class NuGetSearchProvider : IVsSearchProvider
    {
        private IVsUIObject _searchResultsIcon;
        private readonly OleMenuCommand _managePackageDialogCommand;
        private readonly OleMenuCommand _managePackageForSolutionDialogCommand;
        private readonly OleMenuCommandService _menuCommandService;

        public NuGetSearchProvider(OleMenuCommandService menuCommandService, OleMenuCommand managePackageDialogCommand, OleMenuCommand managePackageForSolutionDialogCommand)
        {
            if (menuCommandService == null)
            {
                throw new ArgumentNullException(nameof(menuCommandService));
            }
            if (managePackageDialogCommand == null)
            {
                throw new ArgumentNullException(nameof(managePackageDialogCommand));
            }
            if (managePackageForSolutionDialogCommand == null)
            {
                throw new ArgumentNullException(nameof(managePackageForSolutionDialogCommand));
            }

            _menuCommandService = menuCommandService;
            _managePackageDialogCommand = managePackageDialogCommand;
            _managePackageForSolutionDialogCommand = managePackageForSolutionDialogCommand;
        }

        internal OleMenuCommandService MenuCommandService
        {
            get { return _menuCommandService; }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public IVsUIObject SearchResultsIcon
        {
            get
            {
                if (_searchResultsIcon == null)
                {
                    try
                    {
                        const string packUriFormat = "pack://application:,,,/{0};component/{1}";
                        string assemblyName = GetType().Assembly.GetName().Name;
                        var image = new BitmapImage(new Uri(String.Format(CultureInfo.InvariantCulture, packUriFormat, assemblyName, "Resources/nugetIcon.bmp")));
                        _searchResultsIcon = WpfPropertyValue.CreateIconObject(image);
                    }
                    catch (Exception)
                    {
                        // An exception is thrown because the icon which was expected to be embedded
                        // in the assembly could not be loaded. Do not block the search provider itself for the absence of icon
                        // Recommendation is to file a low pri bug for the same
                    }
                }
                return _searchResultsIcon;
            }
        }

        public Guid Category
        {
            get { return GetType().GUID; }
        }

        public IVsSearchTask CreateSearch(uint dwCookie, IVsSearchQuery pSearchQuery, IVsSearchProviderCallback pSearchCallback)
        {
            if (dwCookie == 0)
            {
                return null;
            }

            return new NuGetSearchTask(this, dwCookie, pSearchQuery, pSearchCallback, _managePackageDialogCommand, _managePackageForSolutionDialogCommand);
        }

        public IVsSearchItemResult CreateItemResult(string lpszPersistenceData)
        {
            // Disallow persistence of data for Most Recently Used Search Results
            return null;
        }

        public string DisplayText
        {
            get { return Resources.NuGetSearchProvider_DisplayText; }
        }

        public string Description
        {
            get { return Resources.NuGetSearchProvider_Description; }
        }

        public void ProvideSearchSettings(IVsUIDataSource pSearchOptions)
        {
        }

        public string Shortcut
        {
            get { return Resources.NuGetSearchProvider_CategoryShortcut; }
        }

        public string Tooltip
        {
            get { return null; }
        }
    }
}
