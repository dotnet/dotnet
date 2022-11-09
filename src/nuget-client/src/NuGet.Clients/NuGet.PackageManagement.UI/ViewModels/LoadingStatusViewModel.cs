// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using NuGet.ProjectManagement;
using NuGet.VisualStudio.Internal.Contracts;
using Resx = NuGet.PackageManagement.UI;

namespace NuGet.PackageManagement.UI
{
    internal class LoadingStatusViewModel : DependencyObject
    {
        public LoadingStatusViewModel()
        {
            UpdateModel();
        }

        public void UpdateModel(IItemLoaderState loaderState)
        {
            PackageSearchStatus = Convert(loaderState.LoadingStatus);
            ItemsFound = loaderState.ItemsCount;

            var convertedList = new System.Collections.SortedList(StringComparer.OrdinalIgnoreCase);
            foreach (var kv in loaderState.SourceLoadingStatus)
            {
                convertedList.Add(kv.Key, Convert(kv.Value));
            }
            SourceLoadingStatus = convertedList;
        }

        private void UpdateModel()
        {
            // order is important!
            CoerceValue(HasMoreItemsProperty);
            CoerceValue(MessageLevelProperty);
            CoerceValue(MoreItemsLinkTextProperty);
            CoerceValue(FailedSourcesProperty);
            CoerceValue(LoadingSourcesProperty);
        }

        #region SourceLoadingStatus

        public System.Collections.IDictionary SourceLoadingStatus
        {
            get { return (System.Collections.IDictionary)GetValue(SourceLoadingStatusProperty); }
            set { SetValue(SourceLoadingStatusProperty, value); }
        }

        public static readonly DependencyProperty SourceLoadingStatusProperty = DependencyProperty.Register(
            nameof(SourceLoadingStatus),
            typeof(System.Collections.IDictionary),
            typeof(LoadingStatusViewModel),
            new PropertyMetadata(null, OnSourceLoadingStatusPropertyChanged));

        private static void OnSourceLoadingStatusPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LoadingStatusViewModel)d).UpdateModel();
        }

        #endregion SourceLoadingStatus

        #region IsMultiSource

        public bool IsMultiSource
        {
            get { return ((bool)GetValue(IsMultiSourceProperty)); }
            set { SetValue(IsMultiSourceProperty, value); }
        }

        public static readonly DependencyProperty IsMultiSourceProperty = DependencyProperty.Register(
            nameof(IsMultiSource),
            typeof(bool),
            typeof(LoadingStatusViewModel),
            new PropertyMetadata(false, OnIsMultiSourcePropertyChanged));

        private static void OnIsMultiSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LoadingStatusViewModel)d).UpdateModel();
        }

        #endregion IsMultiSource

        #region LoadingMessage

        public string LoadingMessage
        {
            get { return (string)GetValue(LoadingMessageProperty); }
            set { SetValue(LoadingMessageProperty, value); }
        }

        public static readonly DependencyProperty LoadingMessageProperty = DependencyProperty.Register(
            nameof(LoadingMessage),
            typeof(string),
            typeof(LoadingStatusViewModel),
            new PropertyMetadata(string.Empty, OnLoadingMessagePropertyChanged));

        private static void OnLoadingMessagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LoadingStatusViewModel)d).UpdateModel();
        }

        #endregion LoadingMessage

        #region PackageSearchStatus

        public PackageSearchStatus PackageSearchStatus
        {
            get { return (PackageSearchStatus)GetValue(PackageSearchStatusProperty); }
            set { SetValue(PackageSearchStatusProperty, value); }
        }

        public static readonly DependencyProperty PackageSearchStatusProperty = DependencyProperty.Register(
            nameof(PackageSearchStatus),
            typeof(PackageSearchStatus),
            typeof(LoadingStatusViewModel),
            new PropertyMetadata(PackageSearchStatus.Unknown, OnPackageSearchStatusPropertyChanged));

        private static void OnPackageSearchStatusPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LoadingStatusViewModel)d).UpdateModel();
        }

        #endregion PackageSearchStatus

        #region MessageLevel

        public MessageLevel MessageLevel
        {
            get { return ((MessageLevel)GetValue(MessageLevelProperty)); }
            private set { SetValue(MessageLevelProperty, value); }
        }

        public static readonly DependencyProperty MessageLevelProperty = DependencyProperty.RegisterReadOnly(
            nameof(MessageLevel),
            typeof(MessageLevel),
            typeof(LoadingStatusViewModel),
            new PropertyMetadata(MessageLevel.Info, null, OnCoerceMessageLevel))
            .DependencyProperty;

        private static object OnCoerceMessageLevel(DependencyObject d, object baseValue)
        {
            var vm = (LoadingStatusViewModel)d;
            switch (vm.PackageSearchStatus)
            {
                case PackageSearchStatus.Loading:
                    return vm.ItemsFound == 0 ? MessageLevel.Info : MessageLevel.Warning;

                case PackageSearchStatus.Cancelled:
                case PackageSearchStatus.ErrorOccurred:
                    return MessageLevel.Error;

                case PackageSearchStatus.PackagesFound:
                    return !vm.HasMoreItems ? MessageLevel.Info : MessageLevel.Warning;

                case PackageSearchStatus.NoPackagesFound:
                case PackageSearchStatus.Unknown:
                default:
                    return MessageLevel.Info;
            }
        }

        #endregion MessageLevel

        #region ItemsFound

        public int ItemsFound
        {
            get { return (int)GetValue(ItemsFoundProperty); }
            set { SetValue(ItemsFoundProperty, value); }
        }

        public static readonly DependencyProperty ItemsFoundProperty = DependencyProperty.Register(
            nameof(ItemsFound),
            typeof(int),
            typeof(LoadingStatusViewModel),
            new PropertyMetadata(0, OnItemsFoundPropertyChanged));

        private static void OnItemsFoundPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LoadingStatusViewModel)d).UpdateModel();
        }

        #endregion ItemsFound

        #region ItemsLoaded

        public int ItemsLoaded
        {
            get { return (int)GetValue(ItemsLoadedProperty); }
            set { SetValue(ItemsLoadedProperty, value); }
        }

        public static readonly DependencyProperty ItemsLoadedProperty = DependencyProperty.Register(
            nameof(ItemsLoaded),
            typeof(int),
            typeof(LoadingStatusViewModel),
            new PropertyMetadata(0, OnItemsLoadedPropertyChanged));

        private static void OnItemsLoadedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LoadingStatusViewModel)d).UpdateModel();
        }

        #endregion ItemsLoaded

        #region HasMoreItems

        public bool HasMoreItems
        {
            get { return ((bool)GetValue(HasMoreItemsProperty)); }
            private set { SetValue(HasMoreItemsProperty, value); }
        }

        public static readonly DependencyProperty HasMoreItemsProperty = DependencyProperty.RegisterReadOnly(
            nameof(HasMoreItems),
            typeof(bool),
            typeof(LoadingStatusViewModel),
            new PropertyMetadata(false, null, OnCoerceHasMoreItems))
            .DependencyProperty;

        private static object OnCoerceHasMoreItems(DependencyObject d, object baseValue)
        {
            var vm = (LoadingStatusViewModel)d;
            return vm.ItemsLoaded > 0 && vm.ItemsFound > vm.ItemsLoaded;
        }

        #endregion HasMoreItems

        #region MoreItemsLinkText

        public string MoreItemsLinkText
        {
            get { return ((string)GetValue(MoreItemsLinkTextProperty)); }
            private set { SetValue(MoreItemsLinkTextProperty, value); }
        }

        public static readonly DependencyProperty MoreItemsLinkTextProperty = DependencyProperty.RegisterReadOnly(
            nameof(MoreItemsLinkText),
            typeof(string),
            typeof(LoadingStatusViewModel),
            new PropertyMetadata(string.Empty, null, OnCoerceMoreItemsLinkText))
            .DependencyProperty;

        private static object OnCoerceMoreItemsLinkText(DependencyObject d, object baseValue)
        {
            var vm = (LoadingStatusViewModel)d;
            return string.Format(
                CultureInfo.CurrentCulture,
                Resx.Resources.Button_ShowMoreResults,
                vm.ItemsFound - vm.ItemsLoaded);
        }

        #endregion MoreItemsLinkText

        #region FailedSources

        public string[] FailedSources
        {
            get { return (string[])GetValue(FailedSourcesProperty); }
            private set { SetValue(FailedSourcesProperty, value); }
        }

        public static readonly DependencyProperty FailedSourcesProperty = DependencyProperty.RegisterReadOnly(
            nameof(FailedSources),
            typeof(string[]),
            typeof(LoadingStatusViewModel),
            new PropertyMetadata(null, null, OnCoerceFailedSources))
            .DependencyProperty;

        private static object OnCoerceFailedSources(DependencyObject d, object baseValue)
        {
            var vm = (LoadingStatusViewModel)d;
            if (vm.SourceLoadingStatus == null)
            {
                return null;
            }

            var sourceLoadingStatus = vm.SourceLoadingStatus
                .Cast<System.Collections.DictionaryEntry>()
                .ToLookup(e => (PackageSearchStatus)e.Value);

            return sourceLoadingStatus[PackageSearchStatus.ErrorOccurred]
                .Select(e => (string)e.Key)
                .ToArray();
        }

        #endregion FailedSources

        #region LoadingSources

        public string[] LoadingSources
        {
            get { return (string[])GetValue(LoadingSourcesProperty); }
            private set { SetValue(LoadingSourcesProperty, value); }
        }

        public static readonly DependencyProperty LoadingSourcesProperty = DependencyProperty.RegisterReadOnly(
            nameof(LoadingSources),
            typeof(string[]),
            typeof(LoadingStatusViewModel),
            new PropertyMetadata(null, null, OnCoerceLoadingSources))
            .DependencyProperty;

        private static object OnCoerceLoadingSources(DependencyObject d, object baseValue)
        {
            var vm = (LoadingStatusViewModel)d;
            if (vm.SourceLoadingStatus == null)
            {
                return null;
            }

            var sourceLoadingStatus = vm.SourceLoadingStatus
                .Cast<System.Collections.DictionaryEntry>()
                .ToLookup(e => (PackageSearchStatus)e.Value);

            return sourceLoadingStatus[PackageSearchStatus.Loading]
                .Select(e => (string)e.Key)
                .ToArray();
        }

        #endregion LoadingSources

        private static PackageSearchStatus Convert(LoadingStatus status)
        {
            switch (status)
            {
                case LoadingStatus.Cancelled:
                    return PackageSearchStatus.Cancelled;
                case LoadingStatus.ErrorOccurred:
                    return PackageSearchStatus.ErrorOccurred;
                case LoadingStatus.Loading:
                    return PackageSearchStatus.Loading;
                case LoadingStatus.NoItemsFound:
                    return PackageSearchStatus.NoPackagesFound;
                case LoadingStatus.NoMoreItems:
                case LoadingStatus.Ready:
                    return PackageSearchStatus.PackagesFound;
                case LoadingStatus.Unknown:
                default:
                    return PackageSearchStatus.Unknown;
            }
        }
    }
}
