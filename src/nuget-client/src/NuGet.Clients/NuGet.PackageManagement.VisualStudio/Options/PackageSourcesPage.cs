// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Utilities.UnifiedSettings;
using NuGet.Configuration;

namespace NuGet.PackageManagement.VisualStudio.Options
{
    [Guid("15C605EC-4FD7-446B-BA4A-75ECF0C0B2D0")]
    public class PackageSourcesPage : NuGetExternalSettingsProvider
    {
        internal const string MonikerPackageSources = "packageSources";
        internal const string MonikerMachineWideSources = "machineWidePackageSources";
        internal const string MonikerPackageSourceId = "packageSourceId"; // Unique identifier for the package source
        internal const string MonikerSourceName = "sourceName";
        internal const string MonikerSourceUrl = "sourceUrl";
        internal const string MonikerIsEnabled = "isEnabled";
        internal const string MonikerAllowInsecureConnections = "allowInsecureConnections";

        private IPackageSourceProvider _packageSourceProvider;

        public PackageSourcesPage(VSSettings vsSettings, IPackageSourceProvider packageSourceProvider)
            : base(vsSettings)
        {
            _packageSourceProvider = packageSourceProvider ?? throw new ArgumentNullException(nameof(packageSourceProvider));
        }

        private List<PackageSource> LoadPackageSources(bool isMachineWide)
        {
            IEnumerable<PackageSource> all = _packageSourceProvider.LoadPackageSources();
            List<PackageSource> filteredPackageSources = all
                .Where(packageSource => packageSource.IsMachineWide == isMachineWide).ToList();
            return filteredPackageSources;
        }

        public override async Task<ExternalSettingOperationResult<T>> GetValueAsync<T>(string moniker, CancellationToken cancellationToken)
        {
            switch (moniker)
            {
                case MonikerPackageSources:
                    var packageSources = await Task.Run(
                        () => LoadPackageSources(isMachineWide: false),
                        cancellationToken);

                    return GetValuePackageSources<T>(packageSources);

                case MonikerMachineWideSources:
                    var machineWidePackageSources = await Task.Run(
                        () => LoadPackageSources(isMachineWide: true),
                        cancellationToken);

                    return GetValuePackageSources<T>(machineWidePackageSources);

                default: break;
            }

            // Shouldn't happen as these are monikers we declared in registration.json.
            throw new InvalidOperationException();
        }

        public override async Task<ExternalSettingOperationResult> SetValueAsync<T>(string moniker, T value, CancellationToken cancellationToken)
        {
            var packageSourcesList = value as IReadOnlyList<IDictionary<string, object>>;
            if (packageSourcesList is null)
            {
                throw new InvalidOperationException();
            }

            bool hasAnyHiddenPropertyChanged = false;

            try
            {
                // Stop listening to setting changes while saving.
                _suppressSettingValuesChanged = true;

                switch (moniker)
                {
                    case MonikerPackageSources:
                        return await Task.Run(
                            () =>
                            {
                                (ExternalSettingOperationResult result, bool hasAnyHiddenPropertyChanged) savePackageSourcesResult = SavePackageSources(packageSourcesList, cancellationToken);
                                hasAnyHiddenPropertyChanged = savePackageSourcesResult.hasAnyHiddenPropertyChanged;
                                return savePackageSourcesResult.result;
                            },
                            cancellationToken);

                    case MonikerMachineWideSources:
                        return await Task.Run(
                            () => SetIsEnabledOnMachineWidePackageSources(packageSourcesList, cancellationToken),
                            cancellationToken);

                    default:
                        throw new InvalidOperationException();
                }
            }
            finally
            {
                // Resume listening to setting changes after saving.
                _suppressSettingValuesChanged = false;

                if (hasAnyHiddenPropertyChanged)
                {
                    VsSettings_SettingsChanged(this, EventArgs.Empty);
                }
            }
        }

        private ExternalSettingOperationResult SetIsEnabledOnMachineWidePackageSources(
            IReadOnlyList<IDictionary<string, object>> packageSourcesList,
            CancellationToken cancellationToken)
        {
            ExternalSettingOperationResult result;

            try
            {
                var machineWidePackageSources = LoadPackageSources(isMachineWide: true);

                foreach (PackageSource originalMachineWideSource in machineWidePackageSources)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    string originalPackageSourceName = originalMachineWideSource.Name;
                    IDictionary<string, object> targetPackageSource = packageSourcesList
                        .Single(packageSourceDictionary =>
                            packageSourceDictionary[MonikerSourceName].ToString() == originalPackageSourceName);

                    bool originalIsEnabled = originalMachineWideSource.IsEnabled;
                    bool targetIsEnabled = (bool)targetPackageSource[MonikerIsEnabled];

                    if (originalIsEnabled != targetIsEnabled)
                    {
                        if (targetIsEnabled)
                        {
                            _packageSourceProvider.EnablePackageSource(originalPackageSourceName);
                        }
                        else
                        {
                            _packageSourceProvider.DisablePackageSource(originalPackageSourceName);
                        }
                    }
                }

                result = ExternalSettingOperationResult.Success.Instance;
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex) when (!(ex is OperationCanceledException && cancellationToken.IsCancellationRequested))
#pragma warning restore CA1031 // Do not catch general exception types
            {
                result = CreateSettingErrorResult(ex.Message + " ('" + MonikerMachineWideSources + "')", isTransient: true);
            }

            return result;
        }

        private (ExternalSettingOperationResult result, bool hasAnyHiddenPropertyChanged) SavePackageSources(
            IReadOnlyList<IDictionary<string, object>> packageSourceDictionaryList,
            CancellationToken cancellationToken)
        {
            bool hasAnyHiddenPropertyChanged = false;
            ExternalSettingOperationResult result;

            try
            {
                List<PackageSource> packageSources = new List<PackageSource>(capacity: packageSourceDictionaryList.Count);
                List<PackageSource> existingPackageSources = LoadPackageSources(isMachineWide: false);
                bool hasAnyPackageSourceNameChanged = false;

                foreach (Dictionary<string, object> packageSourceDictionary in packageSourceDictionaryList)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    string name = packageSourceDictionary[MonikerSourceName].ToString();
                    string lookupName;

                    // Package Sources that were pre-existing in the NuGet.Config when GetValueAsync was called will have a Package ID.
                    if (packageSourceDictionary.TryGetValue(MonikerPackageSourceId, out object packageSourceIdObj))
                    {
                        lookupName = packageSourceIdObj.ToString();

                        if (!string.Equals(lookupName, name, StringComparison.CurrentCultureIgnoreCase))
                        {
                            // Changing the ID needs to refresh Unified Settings since the ID is a hidden property.
                            hasAnyPackageSourceNameChanged = true;
                        }
                    }
                    else // Newly added Package Sources will not have a Package ID yet.
                    {
                        lookupName = name;
                    }

                    string source = packageSourceDictionary[MonikerSourceUrl].ToString();
                    bool isEnabled = (bool)packageSourceDictionary[MonikerIsEnabled];
                    bool allowInsecureConnections = (bool)packageSourceDictionary[MonikerAllowInsecureConnections];

                    PackageSource packageSource =
                        PackageSourceValidator.FindExistingOrCreate(
                            lookupName,
                            source,
                            name,
                            isEnabled,
                            allowInsecureConnections,
                            existingPackageSources);

                    packageSources.Add(packageSource);
                }

                // Throw any validation errors before saving.
                PackageSourceValidator.ValidateUniquenessOrThrow(packageSources);

                _packageSourceProvider.SavePackageSources(packageSources);

                hasAnyHiddenPropertyChanged = hasAnyPackageSourceNameChanged;

                result = ExternalSettingOperationResult.Success.Instance;
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex) when (!(ex is OperationCanceledException && cancellationToken.IsCancellationRequested))
#pragma warning restore CA1031 // Do not catch general exception types
            {
                result = CreateSettingErrorResult(ex.Message, isTransient: true);
                ActivityLog.LogError(ExceptionHelper.LogEntrySource, ex.ToString());
            }

            return (result, hasAnyHiddenPropertyChanged);
        }

        private static ExternalSettingOperationResult<T> GetValuePackageSources<T>(List<PackageSource> packageSources)
        {
            ExternalSettingOperationResult<T> result;

            try
            {
                var packageSourcesList = new List<Dictionary<string, object>>(capacity: packageSources.Count);

                // Each list item is represented by a dictionary, which in this case will have a single key-value pair for ConfigPath.
                foreach (PackageSource packageSource in packageSources)
                {
                    var dict = new Dictionary<string, object>(capacity: 5)
                    {
                        { MonikerPackageSourceId, packageSource.Name }, // Use the package source name as a unique identifier
                        { MonikerSourceName, packageSource.Name },
                        { MonikerSourceUrl, packageSource.SourceUri }, // Throws if Source is an invalid URI
                        { MonikerIsEnabled, packageSource.IsEnabled },
                        { MonikerAllowInsecureConnections, packageSource.AllowInsecureConnections }
                    };

                    packageSourcesList.Add(dict);
                }

                T castedPackageSources = (T)(object)packageSourcesList;
                result = ExternalSettingOperationResult.SuccessResult(castedPackageSources);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                var userErrorMessage = Strings.Error_NuGetConfig_InvalidState + " " + ex.Message;
                result = CreateSettingErrorResult<T>(userErrorMessage);

                var logErrorMessage = Strings.Error_NuGetConfig_InvalidState + " " + ex.ToString();
                ActivityLog.LogError(ExceptionHelper.LogEntrySource, logErrorMessage);
            }

            return result;
        }
    }
}
