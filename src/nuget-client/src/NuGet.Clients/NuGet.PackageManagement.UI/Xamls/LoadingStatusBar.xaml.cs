using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualStudio.Shell;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// Interaction logic for LoadingStatusBar.xaml
    /// </summary>
    internal partial class LoadingStatusBar : UserControl
    {
        public LoadingStatusViewModel ViewModel => DataContext as LoadingStatusViewModel;

        #region ItemsLoaded

        public int ItemsLoaded
        {
            get { return (int)GetValue(ItemsLoadedProperty); }
            set { SetValue(ItemsLoadedProperty, value); }
        }

        public static readonly DependencyProperty ItemsLoadedProperty = DependencyProperty.Register(
            nameof(ItemsLoaded),
            typeof(int),
            typeof(LoadingStatusBar),
            new PropertyMetadata(0, OnItemsLoadedPropertyChanged));

        private static void OnItemsLoadedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LoadingStatusBar)d).ViewModel.ItemsLoaded = (int)e.NewValue;
        }

        #endregion ItemsLoaded

        #region ShowMoreResultsClick

        public static readonly RoutedEvent ShowMoreResultsClickEvent = EventManager.RegisterRoutedEvent(
            nameof(ShowMoreResultsClick),
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(LoadingStatusBar));

        public event RoutedEventHandler ShowMoreResultsClick
        {
            add { AddHandler(ShowMoreResultsClickEvent, value); }
            remove { RemoveHandler(ShowMoreResultsClickEvent, value); }
        }

        #endregion ShowMoreResultsClick

        #region DismissClick

        public static readonly RoutedEvent DismissClickEvent = EventManager.RegisterRoutedEvent(
            nameof(DismissClick),
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(LoadingStatusBar));

        public event RoutedEventHandler DismissClick
        {
            add { AddHandler(DismissClickEvent, value); }
            remove { RemoveHandler(DismissClickEvent, value); }
        }

        #endregion DismissClick

        public LoadingStatusBar()
        {
            InitializeComponent();
        }

        public void UpdateLoadingState(IItemLoaderState loaderState)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            ViewModel?.UpdateModel(loaderState);
        }

        public void Reset(string loadingMessage, bool isMultiSource)
        {
            DataContext = new LoadingStatusViewModel
            {
                LoadingMessage = loadingMessage,
                IsMultiSource = isMultiSource
            };
        }

        public void SetError()
        {
            DataContext = new LoadingStatusViewModel
            {
                PackageSearchStatus = PackageSearchStatus.ErrorOccurred
            };
        }

        public void SetCancelled()
        {
            DataContext = new LoadingStatusViewModel
            {
                PackageSearchStatus = PackageSearchStatus.Cancelled
            };
        }

        private void ShowMoreResultsButton_Click(object sender, RoutedEventArgs e) => RaiseEvent(new RoutedEventArgs(ShowMoreResultsClickEvent));

        private void DismissButton_Click(object sender, RoutedEventArgs e) => RaiseEvent(new RoutedEventArgs(DismissClickEvent));
    }
}
