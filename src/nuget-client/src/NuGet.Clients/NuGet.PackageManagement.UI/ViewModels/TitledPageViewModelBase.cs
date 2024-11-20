namespace NuGet.PackageManagement.UI.ViewModels
{
    public class TitledPageViewModelBase : ViewModelBase
    {
        private string _title;
        public string Title
        {
            get => _title;
            protected set => SetAndRaisePropertyChanged(ref _title, value);
        }

        private bool _isVisible;
        public bool IsVisible
        {
            get => _isVisible;
            protected set => SetAndRaisePropertyChanged(ref _isVisible, value);
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
