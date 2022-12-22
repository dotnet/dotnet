// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace NuGetVSExtension
{
    public class NuGetStaticSearchResult : IVsSearchItemResult
    {
        private const string TabProvider = " /searchin:online";
        private readonly string _searchText;
        private readonly OleMenuCommand _supportedManagePackageCommand;
        private readonly NuGetSearchProvider _searchProvider;

        public NuGetStaticSearchResult(string searchText, NuGetSearchProvider provider, OleMenuCommand supportedManagePackageCommand)
        {
            if (searchText == null)
            {
                throw new ArgumentNullException(nameof(searchText));
            }
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }
            if (supportedManagePackageCommand == null)
            {
                throw new ArgumentNullException(nameof(supportedManagePackageCommand));
            }

            if (searchText.StartsWith(provider.Shortcut + " ", StringComparison.Ordinal))
            {
                searchText = searchText.Substring(provider.Shortcut.Length);
            }

            _searchText = searchText;
            _supportedManagePackageCommand = supportedManagePackageCommand;

            DisplayText = String.Format(CultureInfo.CurrentCulture,
                Resources.NuGetStaticResult_DisplayText, searchText);
            _searchProvider = provider;
        }

        public string Description
        {
            get { return null; }
        }

        public string DisplayText { get; private set; }

        public IVsUIObject Icon
        {
            get { return _searchProvider.SearchResultsIcon; }
        }

        [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Justification = "Just to make TeamCity build happy. We don't see any FxCop issue when built locally.")]
        public void InvokeAction()
        {
            _supportedManagePackageCommand.Invoke(_searchText + TabProvider);
        }

        public string PersistenceData
        {
            get { return null; }
        }

        public IVsSearchProvider SearchProvider
        {
            get { return _searchProvider; }
        }

        public string Tooltip
        {
            get { return null; }
        }
    }
}
