// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Utilities.UnifiedSettings;
using NuGet.Configuration;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.VisualStudio.Options
{
    [Guid("ACE317DA-8399-4DA4-9828-107E02244D45")]
    public class PackageSourceMappingPage : NuGetExternalSettingsProvider
    {
        internal const string MonikerPackageSourceMapping = "packageSourceMapping";
        internal const string MonikerSourceNameEnum = "packageSourceMapping.sourceName";
        internal const string MonikerPackageId = "packageId";
        internal const string MonikerSourceNames = "sourceName";

        public override event EventHandler<EnumSettingChoicesChangedEventArgs>? EnumSettingChoicesChanged;

        private readonly IPackageSourceProvider _packageSourceProvider;
        internal readonly PackageSourceMappingProvider _packageSourceMappingProvider;

        public PackageSourceMappingPage(VSSettings vsSettings, IPackageSourceProvider packageSourceProvider, PackageSourceMappingProvider packageSourceMappingProvider)
            : base(vsSettings)
        {
            _packageSourceProvider = packageSourceProvider ?? throw new ArgumentNullException(nameof(packageSourceProvider));
            _packageSourceMappingProvider = packageSourceMappingProvider ?? throw new ArgumentNullException(nameof(packageSourceMappingProvider));
        }

        internal override void VsSettings_SettingsChanged(object sender, EventArgs e)
        {
            // Possible values for Package Sources need to be refreshed, so tell Unified Settings that they have changed.
            EnumSettingChoicesChanged?.Invoke(this, new EnumSettingChoicesChangedEventArgs(MonikerSourceNameEnum));

            base.VsSettings_SettingsChanged(sender, e);
        }

        public override async Task<ExternalSettingOperationResult<T>> GetValueAsync<T>(string moniker, CancellationToken cancellationToken)
        {
            if (moniker == MonikerPackageSourceMapping)
            {
                IReadOnlyList<PackageSourceMappingSourceItem> packageSourceMappingItems = await Task.Run(
                    () => _packageSourceMappingProvider.GetPackageSourceMappingItems(),
                    cancellationToken);

                Dictionary<string, List<PackageSourceContextInfo>> packageSourceMappingDictionary = PackageSourceMappingUtility.CreatePackageSourceMappingDictionary(packageSourceMappingItems);
                ImmutableSortedDictionary<string, List<PackageSourceContextInfo>> sortedPackageSourceMappingDictionary
                    = packageSourceMappingDictionary.OrderBy(mapping => mapping.Key, StringComparer.OrdinalIgnoreCase).ToImmutableSortedDictionary();
                return GetValuePackageSourceMappings<T>(sortedPackageSourceMappingDictionary);
            }

            // Shouldn't happen as these are monikers we declared in registration.json.
            throw new InvalidOperationException();
        }

        public override async Task<ExternalSettingOperationResult> SetValueAsync<T>(string moniker, T value, CancellationToken cancellationToken)
        {
            var packageSourceMappingList = value as IReadOnlyList<IDictionary<string, object>>;
            if (packageSourceMappingList is null)
            {
                throw new InvalidOperationException();
            }

            try
            {
                // Stop listening to setting changes while saving.
                _suppressSettingValuesChanged = true;
                if (moniker == MonikerPackageSourceMapping)
                {
                    return await Task.Run(
                        () => SavePackageSourceMappings(packageSourceMappingList, cancellationToken),
                        cancellationToken);
                }
            }
            finally
            {
                // Resume listening to setting changes after saving.
                _suppressSettingValuesChanged = false;
            }

            // Shouldn't happen as these are monikers we declared in registration.json.
            throw new InvalidOperationException();
        }

        private ExternalSettingOperationResult SavePackageSourceMappings(IReadOnlyList<IDictionary<string, object>> packageSourceMappingList, CancellationToken cancellationToken)
        {
            ExternalSettingOperationResult result;

            try
            {
                List<(string, IEnumerable<string>)> packagePatternToSources = new List<(string, IEnumerable<string>)>(capacity: packageSourceMappingList.Count);

                IReadOnlyList<PackageSource> existingPackageSources = _packageSourceProvider.LoadPackageSources().ToList().AsReadOnly();
                IReadOnlyList<PackageSourceMappingSourceItem> originalPackageSourceMappings = _packageSourceMappingProvider.GetPackageSourceMappingItems();

                foreach (Dictionary<string, object> packageSourceMappingDictionary in packageSourceMappingList)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    string packageIdPattern = packageSourceMappingDictionary[MonikerPackageId].ToString();
                    var sourceObjects = (IEnumerable<object>)packageSourceMappingDictionary[MonikerSourceNames];
                    List<string> sources = sourceObjects.Select(sourceObject => sourceObject.ToString()).ToList();
                    packagePatternToSources.Add((packageIdPattern, sources));
                }

                var sourceNamesToPackagePatterns = new Dictionary<string, List<PackagePatternItem>>();

                List<PackageSourceMappingSourceItem> packageSourceMappingSourceItems =
                    PackageSourceMappingUtility.ConvertPackageIdAndSourcesToSourceMappingSourceItems(sourceNamesToPackagePatterns, packagePatternToSources);

                _packageSourceMappingProvider.SavePackageSourceMappings(packageSourceMappingSourceItems);

                result = ExternalSettingOperationResult.Success.Instance;
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex) when (!(ex is OperationCanceledException && cancellationToken.IsCancellationRequested))
#pragma warning restore CA1031 // Do not catch general exception types
            {
                result = CreateSettingErrorResult(ex.Message, isTransient: true);
                ActivityLog.LogError(ExceptionHelper.LogEntrySource, ex.ToString());
            }

            return result;
        }

        public override async Task<ExternalSettingOperationResult<IReadOnlyList<EnumChoice>>> GetEnumChoicesAsync(string enumSettingMoniker, CancellationToken cancellationToken)
        {
            if (enumSettingMoniker == MonikerSourceNameEnum)
            {
                List<PackageSource> packageSources = await Task.Run(() => _packageSourceProvider.LoadPackageSources().ToList());

                // Source names should be unique, but in case validation was bypassed and multiple were added, de-duplicate them for the options shown for source mappings.
                List<string> sourceNames = packageSources
                    .Select(packageSource => packageSource.Name)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                List<EnumChoice> enumChoices = sourceNames
                    .Select(sourceName => new EnumChoice(Moniker: sourceName, Title: sourceName))
                    .ToList();

                return ExternalSettingOperationResult.SuccessResult((IReadOnlyList<EnumChoice>)enumChoices.AsReadOnly());
            }

            return await base.GetEnumChoicesAsync(enumSettingMoniker, cancellationToken);
        }

        private static ExternalSettingOperationResult<T> GetValuePackageSourceMappings<T>(ImmutableSortedDictionary<string, List<PackageSourceContextInfo>> packageSourceMappingsDictionary)
        {
            ExternalSettingOperationResult<T> result;

            try
            {
                var packageSourceMappingsList = new List<Dictionary<string, object>>(capacity: packageSourceMappingsDictionary.Count);

                // Each list item is represented by a dictionary, which in this case will have a single key-value pair for ConfigPath.
                foreach (KeyValuePair<string, List<PackageSourceContextInfo>> packageSourceMapping in packageSourceMappingsDictionary)
                {
                    string packageIdOrPattern = packageSourceMapping.Key;
                    List<PackageSourceContextInfo> packageSources = packageSourceMapping.Value;
                    List<string> packageSourceNames = new(packageSources.Count);
                    packageSourceNames.AddRange(packageSources.Select(source => source.Name));

                    var dict = new Dictionary<string, object>(capacity: 2)
                    {
                        { MonikerPackageId, packageIdOrPattern },
                        { MonikerSourceNames, packageSourceNames },
                    };

                    packageSourceMappingsList.Add(dict);
                }


                T castedPackageSources = (T)(object)packageSourceMappingsList;
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
