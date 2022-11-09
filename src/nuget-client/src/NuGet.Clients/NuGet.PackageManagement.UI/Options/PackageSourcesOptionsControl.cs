// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft;
using Microsoft.ServiceHub.Framework;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using NuGet.Configuration;
using NuGet.PackageManagement.UI;
using NuGet.PackageManagement.VisualStudio;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Common;
using NuGet.VisualStudio.Internal.Contracts;
using NuGet.VisualStudio.Telemetry;
using IAsyncServiceProvider = Microsoft.VisualStudio.Shell.IAsyncServiceProvider;
using GelUtilities = Microsoft.Internal.VisualStudio.PlatformUI.Utilities;
using Task = System.Threading.Tasks.Task;
using Microsoft.VisualStudio.Imaging.Interop;
using Microsoft.VisualStudio.Imaging;

namespace NuGet.Options
{
    /// <summary>
    /// Represents the Tools - Options - Package Manager dialog
    /// </summary>
    /// <remarks>
    /// The code in this class assumes that while the dialog is open, noone is modifying the
    /// VSPackageSourceProvider directly.
    /// Otherwise, we have a problem with synchronization with the package source provider.
    /// </remarks>
    public partial class PackageSourcesOptionsControl : UserControl
    {
        private BindingSource _packageSources;
        private BindingSource _machineWidepackageSources;
        private readonly IAsyncServiceProvider _asyncServiceProvider;
        private bool _initialized;
        private IReadOnlyList<PackageSourceContextInfo> _originalPackageSources;
#pragma warning disable ISB001 // Dispose of proxies, disposed in disposing event or in ClearSettings
        private INuGetSourcesService _nugetSourcesService; // Store proxy object in case the dialog is up and we lose connection we wont grab the local proxy and try to save to that
#pragma warning restore ISB001 // Dispose of proxies, disposed in disposing event or in ClearSettings

        private static IVsImageService2 ImageService
        {
            get
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                return (IVsImageService2)Package.GetGlobalService(typeof(SVsImageService));
            }
        }

        public static Image WarningIcon
        {
            get
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                ImageAttributes attributes = new ImageAttributes
                {
                    StructSize = Marshal.SizeOf(typeof(ImageAttributes)),
                    ImageType = (uint)_UIImageType.IT_Bitmap,
                    Format = (uint)_UIDataFormat.DF_WinForms,
                    LogicalWidth = 16,
                    LogicalHeight = 16,
                    Flags = (uint)_ImageAttributesFlags.IAF_RequiredFlags
                };

                IVsUIObject uIObj = ImageService.GetImage(KnownMonikers.StatusWarning, attributes);

                return (Image)GelUtilities.GetObjectData(uIObj);
            }
        }

        public PackageSourcesOptionsControl(IAsyncServiceProvider asyncServiceProvider)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            InitializeComponent();

            _asyncServiceProvider = asyncServiceProvider;

            SetupEventHandlers();

            UpdateDPI();

            HttpWarningIcon.Image = WarningIcon;
            HttpWarning.Text = Resources.Warning_NewHTTPSource_VSOptions;
        }

        private void UpdateDPI()
        {
            var imgs = images16px;
            if (addButton.Height > 72)
            {
                imgs = images64px;
            }
            else if (addButton.Height > 40)
            {
                imgs = images32px;
            }

            addButton.ImageList = imgs;
            removeButton.ImageList = imgs;
        }

        private void SetupEventHandlers()
        {
            Disposed += PackageSourcesOptionsControl_Disposed;
            NewPackageName.TextChanged += (o, e) => UpdateUI();
            NewPackageSource.TextChanged += (o, e) => UpdateUI();
            NewPackageName.Focus();
            UpdateUI();
        }

        private void UpdateUI()
        {
            // It is only allowed for 1 of the listboxes to be selected at any time or neither
            // Never MUST both the listboxes be selected
            Debug.Assert(PackageSourcesListBox.SelectedItem == null || MachineWidePackageSourcesListBox.SelectedItem == null);

            var selectedSource = (PackageSourceContextInfo)PackageSourcesListBox.SelectedItem;
            var selectedMachineSource = (PackageSourceContextInfo)MachineWidePackageSourcesListBox.SelectedItem;

            if (selectedMachineSource != null)
            {
                // This block corresponds to MachineWidePackageSourcesListBox
                addButton.Enabled = false;
                removeButton.Enabled = false;
                BrowseButton.Enabled = false;
                updateButton.Enabled = false;

                NewPackageName.ReadOnly = NewPackageSource.ReadOnly = true;
            }
            else
            {
                bool allowEditing = selectedSource != null;

                BrowseButton.Enabled = updateButton.Enabled = removeButton.Enabled = allowEditing;
                NewPackageName.ReadOnly = NewPackageSource.ReadOnly = !allowEditing;

                // Always enable addButton for PackageSourceListBox
                addButton.Enabled = true;
            }
        }

        internal async Task InitializeOnActivatedAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (_initialized)
                {
                    return;
                }

                _initialized = true;

                IServiceBrokerProvider serviceBrokerProvider = await ServiceLocator.GetComponentModelServiceAsync<IServiceBrokerProvider>();
                IServiceBroker serviceBroker = await serviceBrokerProvider.GetAsync();

#pragma warning disable ISB001 // Dispose of proxies, disposed in disposing event or in ClearSettings
                _nugetSourcesService = await serviceBroker.GetProxyAsync<INuGetSourcesService>(
                    NuGetServices.SourceProviderService,
                    cancellationToken: cancellationToken);
#pragma warning restore ISB001 // Dispose of proxies, disposed in disposing event or in ClearSettings
                Assumes.NotNull(_nugetSourcesService);

                // get packages sources
                _originalPackageSources = await _nugetSourcesService.GetPackageSourcesAsync(cancellationToken);
                // packageSources and machineWidePackageSources are deep cloned when created, no need to worry about re-querying for sources to diff changes
                var allPackageSources = _originalPackageSources;
                var packageSources = allPackageSources.Where(ps => !ps.IsMachineWide).ToList();
                var machineWidePackageSources = allPackageSources.Where(ps => ps.IsMachineWide).ToList();

                // bind to the package sources, excluding Aggregate
                _packageSources = new BindingSource(packageSources.Select(ps => ps.Clone()).ToList(), null);
                _packageSources.CurrentChanged += OnSelectedPackageSourceChanged;
                PackageSourcesListBox.GotFocus += PackageSourcesListBox_GotFocus;
                PackageSourcesListBox.DataSource = _packageSources;
                ResetItemsCheckedState(PackageSourcesListBox, _packageSources);

                if (machineWidePackageSources.Count > 0)
                {
                    _machineWidepackageSources = new BindingSource(machineWidePackageSources.Select(ps => ps.Clone()).ToList(), null);
                    _machineWidepackageSources.CurrentChanged += OnSelectedMachineWidePackageSourceChanged;
                    MachineWidePackageSourcesListBox.GotFocus += MachineWidePackageSourcesListBox_GotFocus;
                    MachineWidePackageSourcesListBox.DataSource = _machineWidepackageSources;
                    ResetItemsCheckedState(MachineWidePackageSourcesListBox, _machineWidepackageSources);
                }
                else
                {
                    MachineWidePackageSourcesListBox.Visible = MachineWideSourcesLabel.Visible = false;
                }

                OnSelectedPackageSourceChanged(null, EventArgs.Empty);
            }
            // Thrown during creating or saving NuGet.Config.
            catch (NuGetConfigurationException ex)
            {
                MessageHelper.ShowErrorMessage(ex.Message, Resources.ErrorDialogBoxTitle);
            }
            // Thrown if no nuget.config found.
            catch (InvalidOperationException ex)
            {
                MessageHelper.ShowErrorMessage(ex.Message, Resources.ErrorDialogBoxTitle);
            }
            catch (UnauthorizedAccessException)
            {
                MessageHelper.ShowErrorMessage(Resources.ShowError_ConfigUnauthorizedAccess, Resources.ErrorDialogBoxTitle);
            }
            // Unknown exception.
            catch (Exception ex)
            {
                MessageHelper.ShowErrorMessage(Resources.ShowError_SettingActivatedFailed, Resources.ErrorDialogBoxTitle);
                ActivityLog.LogError(NuGetUI.LogEntrySource, ex.ToString());
            }
        }

        private void ResetItemsCheckedState(PackageSourceCheckedListBox checkedListBox, BindingSource bindingSource)
        {
            var list = (IList<PackageSourceContextInfo>)bindingSource.List;

            for (int i = 0; i < list.Count; i++)
            {
                var isEnabled = list[i].IsEnabled;
                checkedListBox.SetItemChecked(i, isEnabled);
            }
        }

        private void MachineWidePackageSourcesListBox_GotFocus(object sender, EventArgs e)
        {
            if (MachineWidePackageSourcesListBox.SelectedItem == null)
            {
                MachineWidePackageSourcesListBox.SelectedItem = _machineWidepackageSources.Current;
            }
            OnSelectedMachineWidePackageSourceChanged(sender, null);
        }

        private void PackageSourcesListBox_GotFocus(object sender, EventArgs e)
        {
            if (PackageSourcesListBox.SelectedItem == null &&
                _packageSources != null)
            {
                PackageSourcesListBox.SelectedItem = _packageSources.Current;
            }
            OnSelectedPackageSourceChanged(sender, null);
        }

        /// <summary>
        /// Persist the package sources, which was add/removed via the Options page, to the VS Settings store.
        /// This gets called when users click OK button.
        /// </summary>
        internal async Task<bool> ApplyChangedSettingsAsync(CancellationToken cancellationToken)
        {
            // if user presses Enter after filling in Name/Source but doesn't click Update
            // the options will be closed without adding the source, try adding before closing
            // Only apply if nothing was updated or the update was successfull
            var result = TryUpdateSource();
            if (result != TryUpdateSourceResults.NotUpdated &&
                result != TryUpdateSourceResults.Unchanged &&
                result != TryUpdateSourceResults.Successful)
            {
                return false;
            }

            // get package sources as ordered list
            List<PackageSourceContextInfo> packageSources = PackageSourcesListBox.Items.Cast<PackageSourceContextInfo>().ToList();
            packageSources.AddRange(MachineWidePackageSourcesListBox.Items.Cast<PackageSourceContextInfo>().ToList());

            try
            {
                if (SourcesChanged(_originalPackageSources, packageSources))
                {
                    await _nugetSourcesService.SavePackageSourceContextInfosAsync(packageSources, cancellationToken);
                }
            }
            // Thrown during creating or saving NuGet.Config.
            catch (NuGetConfigurationException ex)
            {
                MessageHelper.ShowErrorMessage(ex.Message, Resources.ErrorDialogBoxTitle);
                return false;
            }
            // Thrown if no nuget.config found.
            catch (InvalidOperationException ex)
            {
                MessageHelper.ShowErrorMessage(ex.Message, Resources.ErrorDialogBoxTitle);
                return false;
            }
            catch (UnauthorizedAccessException)
            {
                MessageHelper.ShowErrorMessage(Resources.ShowError_ConfigUnauthorizedAccess, Resources.ErrorDialogBoxTitle);
                return false;
            }
            // Unknown exception.
            catch (Exception ex)
            {
                MessageHelper.ShowErrorMessage(Resources.ShowError_ApplySettingFailed, Resources.ErrorDialogBoxTitle);
                ActivityLog.LogError(NuGetUI.LogEntrySource, ex.ToString());
                return false;
            }

            // find the enabled package source
            return true;
        }

        // Returns true if there are no changes between existingSources and packageSources.
        private static bool SourcesChanged(IReadOnlyList<PackageSourceContextInfo> existingSources, IReadOnlyList<PackageSourceContextInfo> packageSources)
        {
            if (existingSources.Count != packageSources.Count)
            {
                return true;
            }

            for (int i = 0; i < existingSources.Count; ++i)
            {
                if (!existingSources[i].Equals(packageSources[i]) ||
                     existingSources[i].IsEnabled != packageSources[i].IsEnabled)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// This gets called when users close the Options dialog
        /// </summary>
        internal void ClearSettings()
        {
            // clear this flag so that we will set up the bindings again when the option page is activated next time
            _initialized = false;

            _nugetSourcesService?.Dispose();
            _nugetSourcesService = null;
            _packageSources = null;
            ClearNameSource();
            UpdateUI();
        }

        private void OnRemoveButtonClick(object sender, EventArgs e)
        {
            if (PackageSourcesListBox.SelectedItem == null)
            {
                return;
            }
            _packageSources.Remove(PackageSourcesListBox.SelectedItem);

            // changing _packageSources appears to clear all the checked items, so now we have to reset all the checkbox states
            ResetItemsCheckedState(PackageSourcesListBox, _packageSources);

            UpdateUI();
        }

        private void OnAddButtonClick(object sender, EventArgs e)
        {
            if (_packageSources == null)
            {
                return;
            }

            _packageSources.Add(CreateNewPackageSource());

            // changing _packageSources appears to clear all the checked items, so now we have to reset all the checkbox states
            ResetItemsCheckedState(PackageSourcesListBox, _packageSources);

            // auto-select the newly-added item
            PackageSourcesListBox.SelectedIndex = PackageSourcesListBox.Items.Count - 1;
            UpdateUI();
        }

        private PackageSourceContextInfo CreateNewPackageSource()
        {
            var sourcesList = (IEnumerable<PackageSourceContextInfo>)_packageSources.List;
            for (int i = 0; ; i++)
            {
                var newName = i == 0 ? "Package source" : "Package source " + i;
                var newSource = i == 0 ? "https://packagesource" : "https://packagesource" + i;
                var packageSource = new PackageSourceContextInfo(newSource, newName);
                if (sourcesList.All(ps => !ps.Equals(packageSource)))
                {
                    return packageSource;
                }
            }
        }

        private void OnUpdateButtonClick(object sender, EventArgs e)
        {
            TryUpdateSourceResults result = TryUpdateSource();
            if (result == TryUpdateSourceResults.NotUpdated)
            {
                MessageHelper.ShowWarningMessage(Resources.ShowWarning_NameAndSourceRequired, Resources.ShowWarning_Title);
                SelectAndFocus(NewPackageName);
            }
        }

        private TryUpdateSourceResults TryUpdateSource()
        {
            var name = NewPackageName.Text.Trim();
            var source = NewPackageSource.Text.Trim();
            if (string.IsNullOrWhiteSpace(name)
                && string.IsNullOrWhiteSpace(source))
            {
                return TryUpdateSourceResults.NotUpdated;
            }

            // validate name
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageHelper.ShowWarningMessage(Resources.ShowWarning_NameRequired, Resources.ShowWarning_Title);
                SelectAndFocus(NewPackageName);
                return TryUpdateSourceResults.InvalidSource;
            }

            // validate source
            if (string.IsNullOrWhiteSpace(source))
            {
                MessageHelper.ShowWarningMessage(Resources.ShowWarning_SourceRequried, Resources.ShowWarning_Title);
                SelectAndFocus(NewPackageSource);
                return TryUpdateSourceResults.InvalidSource;
            }

            if (!(Common.PathValidator.IsValidLocalPath(source) || Common.PathValidator.IsValidUncPath(source) || Common.PathValidator.IsValidUrl(source)))
            {
                MessageHelper.ShowWarningMessage(Resources.ShowWarning_InvalidSource, Resources.ShowWarning_Title);
                SelectAndFocus(NewPackageSource);
                return TryUpdateSourceResults.InvalidSource;
            }

            var selectedPackageSource = (PackageSourceContextInfo)PackageSourcesListBox.SelectedItem;
            if (selectedPackageSource == null)
            {
                return TryUpdateSourceResults.NotUpdated;
            }

            if (selectedPackageSource.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase) && selectedPackageSource.Source.Equals(source, StringComparison.OrdinalIgnoreCase))
            {
                return TryUpdateSourceResults.Unchanged;
            }

            var sourcesList = (IEnumerable<PackageSourceContextInfo>)_packageSources.List;

            // check to see if name has already been added
            // also make sure it's not the same as the aggregate source ('All')
            bool hasName = sourcesList.Any(ps => ps != selectedPackageSource && string.Equals(name, ps.Name, StringComparison.CurrentCultureIgnoreCase));
            if (hasName)
            {
                MessageHelper.ShowWarningMessage(Resources.ShowWarning_UniqueName, Resources.ShowWarning_Title);
                SelectAndFocus(NewPackageName);
                return TryUpdateSourceResults.SourceConflicted;
            }

            // check to see if source has already been added
            bool hasSource = sourcesList.Any(ps => ps != selectedPackageSource &&
                                                   string.Equals(PathValidator.GetCanonicalPath(source),
                                                                 PathValidator.GetCanonicalPath(ps.Source),
                                                                 StringComparison.OrdinalIgnoreCase));
            if (hasSource)
            {
                MessageHelper.ShowWarningMessage(Resources.ShowWarning_UniqueSource, Resources.ShowWarning_Title);
                SelectAndFocus(NewPackageSource);
                return TryUpdateSourceResults.SourceConflicted;
            }

            selectedPackageSource.Name = name;
            selectedPackageSource.Source = source;
            _packageSources.ResetCurrentItem();

            return TryUpdateSourceResults.Successful;
        }

        private static void SelectAndFocus(TextBox textBox)
        {
            textBox.Focus();
            textBox.SelectAll();
        }

        private void ClearNameSource()
        {
            NewPackageName.Text = string.Empty;
            NewPackageSource.Text = string.Empty;
            NewPackageName.Focus();
        }

        private void PackageSourcesContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var currentListBox = PackageSourcesContextMenu.SourceControl as ListBox;
            if (currentListBox != null
                && currentListBox.SelectedItem != null
                && e.ClickedItem == CopyPackageSourceStripMenuItem)
            {
                CopySelectedItem((PackageSourceContextInfo)currentListBox.SelectedItem);
            }
        }

        private void PackageSourcesListBox_KeyUp(object sender, KeyEventArgs e)
        {
            var currentListBox = (PackageSourceCheckedListBox)sender;
            if (e.KeyCode == Keys.C && e.Control)
            {
                CopySelectedItem((PackageSourceContextInfo)currentListBox.SelectedItem);
                e.Handled = true;
            }
        }

        private void PackageSourcesListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var checkedListBox = sender as CheckedListBox;
            if (checkedListBox == null)
            {
                return;
            }

            if (e.Index < 0 || e.Index >= checkedListBox.Items.Count)
            {
                return;
            }

            bool isEnabled = e.NewValue == CheckState.Checked;
            var packageSource = (PackageSourceContextInfo)checkedListBox.Items[e.Index];
            packageSource.IsEnabled = isEnabled;
        }

        private Rectangle GetCheckBoxRectangleForListBoxItem(PackageSourceCheckedListBox currentListBox, int itemIndex)
        {
            const int edgeMargin = 8;

            var itemRectangle = currentListBox.GetItemRectangle(itemIndex);

            // this is the bound of the checkbox
            var checkBoxRectangle = new Rectangle(
                itemRectangle.Left + edgeMargin + 2,
                itemRectangle.Top + edgeMargin,
                currentListBox.CheckBoxSize.Width,
                currentListBox.CheckBoxSize.Height);

            return checkBoxRectangle;
        }

        private static void CopySelectedItem(PackageSourceContextInfo selectedPackageSource)
        {
            Clipboard.Clear();
            Clipboard.SetText(selectedPackageSource.Source);
        }

        private void PackageSourcesListBox_MouseMove(object sender, MouseEventArgs e)
        {
            var currentListBox = (ListBox)sender;
            int index = currentListBox.IndexFromPoint(e.X, e.Y);

            if (index >= 0
                && index < currentListBox.Items.Count
                && e.Y <= currentListBox.PreferredHeight)
            {
                var source = (PackageSourceContextInfo)currentListBox.Items[index];
                string newToolTip = !string.IsNullOrEmpty(source.Description) ?
                    source.Description :
                    source.Source;
                string currentToolTip = packageListToolTip.GetToolTip(currentListBox);
                if (currentToolTip != newToolTip)
                {
                    packageListToolTip.SetToolTip(currentListBox, newToolTip);
                }
            }
            else
            {
                packageListToolTip.SetToolTip(currentListBox, null);
                packageListToolTip.Hide(currentListBox);
            }
        }

        private void OnSelectedPackageSourceChanged(object sender, EventArgs e)
        {
            MachineWidePackageSourcesListBox.ClearSelected();
            UpdateUI();

            UpdateTextBoxes((PackageSourceContextInfo)_packageSources.Current);
        }

        private void OnSelectedMachineWidePackageSourceChanged(object sender, EventArgs e)
        {
            PackageSourcesListBox.ClearSelected();
            UpdateUI();

            UpdateTextBoxes((PackageSourceContextInfo)_machineWidepackageSources.Current);
        }

        private void UpdateTextBoxes(PackageSourceContextInfo packageSource)
        {
            if (packageSource != null)
            {
                NewPackageName.Text = packageSource.Name;
                NewPackageSource.Text = packageSource.Source;
            }
            else
            {
                NewPackageName.Text = string.Empty;
                NewPackageSource.Text = string.Empty;
            }
        }

        private void OnBrowseButtonClicked(object sender, EventArgs args)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(OnBrowseButtonClickedAsync).PostOnFailure(nameof(PackageSourcesOptionsControl), nameof(OnBrowseButtonClicked));
        }

        private async Task OnBrowseButtonClickedAsync()
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            const int MaxDirectoryLength = 1000;

            //const int BIF_RETURNONLYFSDIRS = 0x00000001;   // For finding a folder to start document searching.
            const int BIF_BROWSEINCLUDEURLS = 0x00000080; // Allow URLs to be displayed or entered.

            var uiShell = await _asyncServiceProvider.GetServiceAsync<SVsUIShell, IVsUIShell2>();
            Assumes.Present(uiShell);
            var rgch = new char[MaxDirectoryLength + 1];

            // allocate a buffer in unmanaged memory for file name (string)
            var bufferPtr = Marshal.AllocCoTaskMem((rgch.Length + 1) * 2);
            // copy initial path to bufferPtr
            Marshal.Copy(rgch, 0, bufferPtr, rgch.Length);

            var pBrowse = new VSBROWSEINFOW[1];
            pBrowse[0] = new VSBROWSEINFOW
            {
                lStructSize = (uint)Marshal.SizeOf(pBrowse[0]),
                dwFlags = BIF_BROWSEINCLUDEURLS,
                pwzDlgTitle = Resources.BrowseFolderDialogDescription,
                nMaxDirName = MaxDirectoryLength,
                hwndOwner = Handle,
                pwzDirName = bufferPtr,
                pwzInitialDir = DetermineInitialDirectory()
            };

            var browseInfo = new VSNSEBROWSEINFOW[1] { new VSNSEBROWSEINFOW() };

            var ret = uiShell.GetDirectoryViaBrowseDlgEx(pBrowse, "", Resources.BrowseFolderDialogSelectButton, "", browseInfo);
            if (ret == VSConstants.S_OK)
            {
                var pathPtr = pBrowse[0].pwzDirName;
                var path = Marshal.PtrToStringAuto(pathPtr);
                NewPackageSource.Text = path;

                // if the package name text box is empty, we fill it with the selected folder's name
                if (string.IsNullOrEmpty(NewPackageName.Text))
                {
                    NewPackageName.Text = Path.GetFileName(path);
                }
            }
        }

        private string DetermineInitialDirectory()
        {
            // determine the inital directory to show in the folder dialog
            var initialDir = NewPackageSource.Text;

            if (IsPathRootedSafe(initialDir)
                && Directory.Exists(initialDir))
            {
                return initialDir;
            }

            var selectedItem = (PackageSourceContextInfo)PackageSourcesListBox.SelectedItem;
            if (selectedItem != null)
            {
                initialDir = selectedItem.Source;
                if (IsPathRootedSafe(initialDir))
                {
                    return initialDir;
                }
            }

            // fallback to MyDocuments folder
            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        private void PackageSourcesOptionsControl_Disposed(object sender, EventArgs e)
        {
            Disposed -= PackageSourcesOptionsControl_Disposed;
            _nugetSourcesService?.Dispose();
            _nugetSourcesService = null;
            _packageSources?.Dispose();
            _machineWidepackageSources?.Dispose();
        }

        private static bool IsPathRootedSafe(string path)
        {
            // Check to make sure path does not contain any invalid chars.
            // Otherwise, Path.IsPathRooted() will throw an ArgumentException.
            return path.IndexOfAny(Path.GetInvalidPathChars()) == -1 && Path.IsPathRooted(path);
        }

        private void NewPackageSource_TextChanged(object sender, EventArgs e)
        {
            var source = new PackageSource(NewPackageSource.Text.Trim(), NewPackageName.Text.Trim());

            // Warn if the source is http, support for this will be removed
            if (source.IsHttp && !source.IsHttps)
            {
                HttpWarning.Visible = true;
                HttpWarningIcon.Visible = true;
            }
            else
            {
                HttpWarning.Visible = false;
                HttpWarningIcon.Visible = false;
            }
        }
    }

    internal enum TryUpdateSourceResults
    {
        NotUpdated = 0,
        Successful = 1,
        InvalidSource = 2,
        SourceConflicted = 3,
        Unchanged = 4
    }
}
