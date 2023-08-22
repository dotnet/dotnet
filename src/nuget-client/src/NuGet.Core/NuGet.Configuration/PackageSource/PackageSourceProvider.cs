// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using NuGet.Common;

namespace NuGet.Configuration
{
    public class PackageSourceProvider : IPackageSourceProvider
    {
        public ISettings Settings { get; private set; }

        internal const int MaxSupportedProtocolVersion = 3;
        private readonly IEnumerable<PackageSource> _configurationDefaultSources;

        public PackageSourceProvider(
          ISettings settings)
#pragma warning disable CS0618 // Type or member is obsolete
            : this(settings, ConfigurationDefaults.Instance.DefaultPackageSources, enablePackageSourcesChangedEvent: true)
#pragma warning restore CS0618 // Type or member is obsolete
        {
        }

        [Obsolete("https://github.com/NuGet/Home/issues/8479")]
        public PackageSourceProvider(
          ISettings settings,
          bool enablePackageSourcesChangedEvent)
            : this(settings, ConfigurationDefaults.Instance.DefaultPackageSources, enablePackageSourcesChangedEvent)
        {
        }

        public PackageSourceProvider(
            ISettings settings,
            IEnumerable<PackageSource> configurationDefaultSources)
#pragma warning disable CS0618 // Type or member is obsolete
            : this(settings, configurationDefaultSources, enablePackageSourcesChangedEvent: true)
#pragma warning restore CS0618 // Type or member is obsolete
        {
        }

        [Obsolete("https://github.com/NuGet/Home/issues/8479")]
        public PackageSourceProvider(
            ISettings settings,
            IEnumerable<PackageSource> configurationDefaultSources,
            bool enablePackageSourcesChangedEvent)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            if (enablePackageSourcesChangedEvent)
            {
                Settings.SettingsChanged += (_, __) => { OnPackageSourcesChanged(); };
            }
            _configurationDefaultSources = LoadConfigurationDefaultSources(configurationDefaultSources);
        }

        private static IEnumerable<PackageSource> LoadConfigurationDefaultSources(IEnumerable<PackageSource> configurationDefaultSources)
        {
#if !IS_CORECLR
            // Global default NuGet source doesn't make sense on Mono
            if (RuntimeEnvironmentHelper.IsMono)
            {
                return Enumerable.Empty<PackageSource>();
            }
#endif
            var packageSourceLookup = new Dictionary<string, IndexedPackageSource>(StringComparer.OrdinalIgnoreCase);
            var packageIndex = 0;

            foreach (var packageSource in configurationDefaultSources)
            {
                packageIndex = AddOrUpdateIndexedSource(packageSourceLookup, packageIndex, packageSource, packageSource.Name);
            }

            return packageSourceLookup.Values
                .OrderBy(source => source.Index)
                .Select(source => source.PackageSource);
        }

        private static Dictionary<string, IndexedPackageSource> LoadPackageSourceLookup(bool byName, ISettings settings)
        {
            var packageSourcesSection = settings.GetSection(ConfigurationConstants.PackageSources);
            var sourcesItems = packageSourcesSection?.Items.OfType<SourceItem>();

            // Order the list so that the closer to the user appear first
            IList<string> configFilePaths = settings.GetConfigFilePaths();
            var sources = sourcesItems?.OrderBy(i => configFilePaths.IndexOf(i.Origin?.ConfigFilePath)); //lower index => higher priority => closer to user.
            // get list of disabled packages
            var disabledSourcesSection = settings.GetSection(ConfigurationConstants.DisabledPackageSources);
            var disabledSourcesSettings = disabledSourcesSection?.Items.OfType<AddItem>();

            var disabledSources = new HashSet<string>(disabledSourcesSettings?.GroupBy(setting => setting.Key).Select(group => group.First().Key) ?? Enumerable.Empty<string>());
            var packageSourceLookup = new Dictionary<string, IndexedPackageSource>(StringComparer.OrdinalIgnoreCase);

            if (sources != null)
            {
                var packageIndex = 0;

                foreach (var setting in sources)
                {
                    var name = setting.Key;
                    var isEnabled = !disabledSources.Contains(name);
                    var packageSource = ReadPackageSource(setting, isEnabled, settings);

                    packageIndex = AddOrUpdateIndexedSource(packageSourceLookup, packageIndex, packageSource, byName ? packageSource.Name : packageSource.Source);
                }
            }
            return packageSourceLookup;
        }

        private static Dictionary<string, IndexedPackageSource> LoadPackageSourceLookupByName(ISettings settings)
        {
            return LoadPackageSourceLookup(byName: true, settings);
        }

        private Dictionary<string, IndexedPackageSource> LoadPackageSourceLookupBySource()
        {
            return LoadPackageSourceLookup(byName: false, Settings);
        }

        /// <summary>
        /// Returns PackageSources specified in the config file merged with any default sources specified in the
        /// constructor.
        /// </summary>
        public IEnumerable<PackageSource> LoadPackageSources()
        {
            return LoadPackageSources(Settings, _configurationDefaultSources);
        }

        /// <summary>
        /// Returns PackageSources if specified in the settings object, combined with the default sources from the default configuration.
        /// </summary>
        public static IEnumerable<PackageSource> LoadPackageSources(ISettings settings)
        {
            return LoadPackageSources(settings, ConfigurationDefaults.Instance.DefaultPackageSources);
        }

        private static List<PackageSource> LoadPackageSources(ISettings settings, IEnumerable<PackageSource> defaultPackageSources)
        {
            var loadedPackageSources = LoadPackageSourceLookupByName(settings).Values
                .OrderBy(source => source.Index)
                .Select(source => source.PackageSource)
                .ToList();

            if (defaultPackageSources != null && defaultPackageSources.Any())
            {
                AddDefaultPackageSources(loadedPackageSources, defaultPackageSources);
            }

            return loadedPackageSources;
        }

        private static void AddDefaultPackageSources(List<PackageSource> loadedPackageSources, IEnumerable<PackageSource> defaultPackageSources)
        {
            var defaultPackageSourcesToBeAdded = new List<PackageSource>();

            foreach (var packageSource in defaultPackageSources)
            {
                var sourceMatching = loadedPackageSources.Any(p => p.Source.Equals(packageSource.Source, StringComparison.OrdinalIgnoreCase));
                var feedNameMatching = loadedPackageSources.Any(p => p.Name.Equals(packageSource.Name, StringComparison.OrdinalIgnoreCase));

                if (!sourceMatching && !feedNameMatching)
                {
                    defaultPackageSourcesToBeAdded.Add(packageSource);
                }
            }

            var defaultSourcesInsertIndex = loadedPackageSources.FindIndex(source => source.IsMachineWide);

            if (defaultSourcesInsertIndex == -1)
            {
                defaultSourcesInsertIndex = loadedPackageSources.Count;
            }

            loadedPackageSources.InsertRange(defaultSourcesInsertIndex, defaultPackageSourcesToBeAdded);
        }

        private static PackageSource ReadPackageSource(SourceItem setting, bool isEnabled, ISettings settings)
        {
            var name = setting.Key;
            var packageSource = new PackageSource(setting.GetValueAsPath(), name, isEnabled)
            {
                IsMachineWide = setting.Origin?.IsMachineWide ?? false,
                MaxHttpRequestsPerSource = SettingsUtility.GetMaxHttpRequest(settings)
            };

            var credentials = ReadCredential(name, settings);
            if (credentials != null)
            {
                packageSource.Credentials = credentials;
            }

            var clientCertificateProvider = new ClientCertificateProvider(settings);
            var clientCertificateItem = clientCertificateProvider.GetClientCertificate(name);
            if (clientCertificateItem != null)
            {
                packageSource.ClientCertificates = clientCertificateItem.Search().ToList();
            }

            packageSource.ProtocolVersion = ReadProtocolVersion(setting);
            packageSource.AllowInsecureConnections = ReadAllowInsecureConnections(setting);

            return packageSource;
        }

        private static int ReadProtocolVersion(SourceItem setting)
        {
            if (int.TryParse(setting.ProtocolVersion, out var protocolVersion))
            {
                return protocolVersion;
            }

            return PackageSource.DefaultProtocolVersion;
        }

        private static bool ReadAllowInsecureConnections(SourceItem setting)
        {
            if (bool.TryParse(setting.AllowInsecureConnections, out var allowInsecureConnections))
            {
                return allowInsecureConnections;
            }

            return PackageSource.DefaultAllowInsecureConnections;
        }

        private static int AddOrUpdateIndexedSource(
            Dictionary<string, IndexedPackageSource> packageSourceLookup,
            int packageIndex,
            PackageSource packageSource,
            string lookupKey)
        {
            if (!packageSourceLookup.TryGetValue(lookupKey, out var previouslyAddedSource))
            {
                packageSourceLookup[lookupKey] = new IndexedPackageSource
                {
                    PackageSource = packageSource,
                    Index = packageIndex++
                };
            }
            else if (previouslyAddedSource.PackageSource.ProtocolVersion < packageSource.ProtocolVersion
                     &&
                     packageSource.ProtocolVersion <= MaxSupportedProtocolVersion)
            {
                // Pick the package source with the highest supported protocol version
                previouslyAddedSource.PackageSource = packageSource;
            }

            return packageIndex;
        }

        private static PackageSourceCredential ReadCredential(string sourceName, ISettings settings)
        {
            var environmentCredentials = ReadCredentialFromEnvironment(sourceName);

            if (environmentCredentials != null)
            {
                return environmentCredentials;
            }

            var credentialsSection = settings.GetSection(ConfigurationConstants.CredentialsSectionName);
            var credentialsItem = credentialsSection?.Items.OfType<CredentialsItem>().FirstOrDefault(s => string.Equals(s.ElementName, sourceName, StringComparison.Ordinal));

            if (credentialsItem != null && !credentialsItem.IsEmpty())
            {
                return new PackageSourceCredential(
                    sourceName,
                    credentialsItem.Username,
                    credentialsItem.Password,
                    credentialsItem.IsPasswordClearText,
                    credentialsItem.ValidAuthenticationTypes);
            }

            return null;
        }

        private static PackageSourceCredential ReadCredentialFromEnvironment(string sourceName)
        {
            var rawCredentials = Environment.GetEnvironmentVariable("NuGetPackageSourceCredentials_" + sourceName);
            if (string.IsNullOrEmpty(rawCredentials))
            {
                return null;
            }

            var match = Regex.Match(rawCredentials.Trim(), @"^Username=(?<user>.*?);\s*Password=(?<pass>.*?)(?:;ValidAuthenticationTypes=(?<authTypes>.*?))?$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            if (!match.Success)
            {
                return null;
            }

            return new PackageSourceCredential(
                sourceName,
                match.Groups["user"].Value,
                match.Groups["pass"].Value,
                isPasswordClearText: true,
                validAuthenticationTypesText: match.Groups["authTypes"].Value);
        }

        private PackageSource GetPackageSource(string key, Dictionary<string, IndexedPackageSource> sourcesLookup)
        {
            if (sourcesLookup.TryGetValue(key, out var indexedPackageSource))
            {
                return indexedPackageSource.PackageSource;
            }

            if (_configurationDefaultSources != null && _configurationDefaultSources.Any())
            {
                var loadedPackageSources = sourcesLookup.Values
                    .OrderBy(source => source.Index)
                    .Select(source => source.PackageSource)
                    .ToList();

                foreach (var packageSource in _configurationDefaultSources)
                {
                    var isSourceMatch = loadedPackageSources.Any(p => p.Source.Equals(packageSource.Source, StringComparison.OrdinalIgnoreCase));
                    var isFeedNameMatch = loadedPackageSources.Any(p => p.Name.Equals(packageSource.Name, StringComparison.OrdinalIgnoreCase));

                    if (isSourceMatch || isFeedNameMatch)
                    {
                        return packageSource;
                    }
                }
            }

            return null;
        }

        public PackageSource GetPackageSourceByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(name));
            }

            return GetPackageSource(name, LoadPackageSourceLookupByName(Settings));
        }

        public HashSet<string> GetPackageSourceNamesMatchingNamePrefix(string namePrefix)
        {
            var names = new HashSet<string>();

            IEnumerable<PackageSource> packageSources = LoadPackageSources();
            foreach (PackageSource packageSource in packageSources)
            {
                if (packageSource.Name.StartsWith(namePrefix, StringComparison.OrdinalIgnoreCase))
                {
                    names.Add(packageSource.Name);
                }
            }

            return names;
        }

        public PackageSource GetPackageSourceBySource(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(source));
            }

            return GetPackageSource(source, LoadPackageSourceLookupBySource());
        }

        public void RemovePackageSource(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(name));
            }

            var isDirty = false;
            RemovePackageSource(name, shouldSkipSave: false, isDirty: ref isDirty);
        }

        private void RemovePackageSource(string name, bool shouldSkipSave, ref bool isDirty)
        {
            // get list of sources
            var packageSourcesSection = Settings.GetSection(ConfigurationConstants.PackageSources);
            var sourcesSettings = packageSourcesSection?.Items.OfType<SourceItem>();

            // get list of credentials for sources
            var sourceCredentialsSection = Settings.GetSection(ConfigurationConstants.CredentialsSectionName);
            var sourceCredentialsSettings = sourceCredentialsSection?.Items.OfType<CredentialsItem>();

            var sourcesToRemove = sourcesSettings?.Where(s => string.Equals(s.Key, name, StringComparison.OrdinalIgnoreCase));
            var credentialsToRemove = sourceCredentialsSettings?.Where(s => string.Equals(s.ElementName, name, StringComparison.OrdinalIgnoreCase));

            if (sourcesToRemove != null)
            {
                foreach (var source in sourcesToRemove)
                {
                    try
                    {
                        Settings.Remove(ConfigurationConstants.PackageSources, source);
                        isDirty = true;
                    }
                    catch { }
                }
            }

            RemoveDisabledSource(name, shouldSkipSave: true, isDirty: ref isDirty);

            if (credentialsToRemove != null)
            {
                foreach (var credentials in credentialsToRemove)
                {
                    try
                    {
                        Settings.Remove(ConfigurationConstants.CredentialsSectionName, credentials);
                        isDirty = true;
                    }
                    catch { }
                }
            }

            if (!shouldSkipSave && isDirty)
            {
                Settings.SaveToDisk();
                OnPackageSourcesChanged();
                isDirty = false;
            }
        }

        [Obsolete("DisablePackageSource(PackageSource source) is deprecated. Please use DisablePackageSource(string name) instead.")]
        public void DisablePackageSource(PackageSource source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var isDirty = false;
            AddDisabledSource(source.Name, shouldSkipSave: false, isDirty: ref isDirty);
        }

        public void DisablePackageSource(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(name));
            }

            var isDirty = false;
            AddDisabledSource(name, shouldSkipSave: false, isDirty: ref isDirty);
        }

        private void AddDisabledSource(string name, bool shouldSkipSave, ref bool isDirty)
        {
            var settingsLookup = GetExistingSettingsLookup();
            var addedInSameFileAsCurrentSource = false;

            if (settingsLookup.TryGetValue(name, out var sourceSetting))
            {
                try
                {
                    if (sourceSetting.Origin != null)
                    {
                        (Settings as Settings).AddOrUpdate(sourceSetting.Origin, ConfigurationConstants.DisabledPackageSources, new AddItem(name, "true"));
                        isDirty = true;
                        addedInSameFileAsCurrentSource = true;
                    }
                }
                // We ignore any errors since this means the current source file could not be edited
                catch { }
            }

            if (!addedInSameFileAsCurrentSource)
            {
                Settings.AddOrUpdate(ConfigurationConstants.DisabledPackageSources, new AddItem(name, "true"));
                isDirty = true;
            }

            if (!shouldSkipSave && isDirty)
            {
                Settings.SaveToDisk();
                OnPackageSourcesChanged();
                isDirty = false;
            }
        }

        public void EnablePackageSource(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(name));
            }

            var isDirty = false;
            RemoveDisabledSource(name, shouldSkipSave: false, isDirty: ref isDirty);
        }

        private void RemoveDisabledSource(string name, bool shouldSkipSave, ref bool isDirty)
        {
            // get list of disabled sources
            var disabledSourcesSection = Settings.GetSection(ConfigurationConstants.DisabledPackageSources);
            var disabledSourcesSettings = disabledSourcesSection?.Items.OfType<AddItem>();

            var disableSourcesToRemove = disabledSourcesSettings?.Where(s => string.Equals(s.Key, name, StringComparison.OrdinalIgnoreCase));

            if (disableSourcesToRemove != null)
            {
                foreach (var disabledSource in disableSourcesToRemove)
                {
                    Settings.Remove(ConfigurationConstants.DisabledPackageSources, disabledSource);
                    isDirty = true;
                }
            }

            if (!shouldSkipSave && isDirty)
            {
                Settings.SaveToDisk();
                OnPackageSourcesChanged();
                isDirty = false;
            }
        }

        public void UpdatePackageSource(PackageSource source, bool updateCredentials, bool updateEnabled)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var packageSources = GetExistingSettingsLookup();
            packageSources.TryGetValue(source.Name, out var sourceToUpdate);

            if (sourceToUpdate != null)
            {
                AddItem disabledSourceItem = null;
                CredentialsItem credentialsSettingsItem = null;

                if (updateEnabled)
                {
                    // get list of disabled packages
                    var disabledSourcesSection = Settings.GetSection(ConfigurationConstants.DisabledPackageSources);
                    disabledSourceItem = disabledSourcesSection?.GetFirstItemWithAttribute<AddItem>(ConfigurationConstants.KeyAttribute, sourceToUpdate.ElementName);
                }

                if (updateCredentials)
                {
                    // get list of credentials for sources
                    var credentialsSection = Settings.GetSection(ConfigurationConstants.CredentialsSectionName);
                    credentialsSettingsItem = credentialsSection?.Items.OfType<CredentialsItem>().Where(s => string.Equals(s.ElementName, sourceToUpdate.Key, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                }

                var oldPackageSource = ReadPackageSource(sourceToUpdate, disabledSourceItem == null, Settings);
                var isDirty = false;

                UpdatePackageSource(
                    source,
                    oldPackageSource,
                    disabledSourceItem,
                    credentialsSettingsItem,
                    updateEnabled,
                    updateCredentials,
                    shouldSkipSave: false,
                    isDirty: ref isDirty);
            }
        }

        private void UpdatePackageSource(
            PackageSource newSource,
            PackageSource existingSource,
            AddItem existingDisabledSourceItem,
            CredentialsItem existingCredentialsItem,
            bool updateEnabled,
            bool updateCredentials,
            bool shouldSkipSave,
            ref bool isDirty)
        {
            if (string.Equals(newSource.Name, existingSource.Name, StringComparison.OrdinalIgnoreCase))
            {
                if ((!string.Equals(newSource.Source, existingSource.Source, StringComparison.OrdinalIgnoreCase) ||
                    newSource.ProtocolVersion != existingSource.ProtocolVersion) && newSource.IsPersistable)
                {
                    Settings.AddOrUpdate(ConfigurationConstants.PackageSources, newSource.AsSourceItem());
                    isDirty = true;
                }

                if (updateEnabled)
                {
                    if (newSource.IsEnabled && existingDisabledSourceItem != null)
                    {
                        Settings.Remove(ConfigurationConstants.DisabledPackageSources, existingDisabledSourceItem);
                        isDirty = true;
                    }

                    if (!newSource.IsEnabled && existingDisabledSourceItem == null)
                    {
                        AddDisabledSource(newSource.Name, shouldSkipSave: true, isDirty: ref isDirty);
                    }
                }

                if (updateCredentials && newSource.Credentials != existingSource.Credentials)
                {
                    if (existingCredentialsItem != null)
                    {
                        if (newSource.Credentials == null)
                        {
                            Settings.Remove(ConfigurationConstants.CredentialsSectionName, existingCredentialsItem);
                            isDirty = true;
                        }
                        else
                        {
                            Settings.AddOrUpdate(ConfigurationConstants.CredentialsSectionName, newSource.Credentials.AsCredentialsItem());
                            isDirty = true;
                        }
                    }
                    else if (newSource.Credentials != null && newSource.Credentials.IsValid())
                    {
                        Settings.AddOrUpdate(ConfigurationConstants.CredentialsSectionName, newSource.Credentials.AsCredentialsItem());
                        isDirty = true;
                    }
                }

                if (!shouldSkipSave && isDirty)
                {
                    Settings.SaveToDisk();
                    OnPackageSourcesChanged();
                    isDirty = false;
                }
            }
        }

        public void AddPackageSource(PackageSource source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var isDirty = false;
            AddPackageSource(source, shouldSkipSave: false, isDirty: ref isDirty);
        }

        private void AddPackageSource(PackageSource source, bool shouldSkipSave, ref bool isDirty)
        {
            if (source.IsPersistable)
            {
                Settings.AddOrUpdate(ConfigurationConstants.PackageSources, source.AsSourceItem());
                isDirty = true;
            }

            if (source.IsEnabled)
            {
                RemoveDisabledSource(source.Name, shouldSkipSave: true, isDirty: ref isDirty);
            }
            else
            {
                AddDisabledSource(source.Name, shouldSkipSave: true, isDirty: ref isDirty);
            }

            if (source.Credentials != null && source.Credentials.IsValid())
            {
                Settings.AddOrUpdate(ConfigurationConstants.CredentialsSectionName, source.Credentials.AsCredentialsItem());
                isDirty = true;
            }

            if (!shouldSkipSave && isDirty)
            {
                Settings.SaveToDisk();
                OnPackageSourcesChanged();
                isDirty = false;
            }
        }

        public void SavePackageSources(IEnumerable<PackageSource> sources)
        {
            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            var isDirty = false;
            var existingSettingsLookup = GetExistingSettingsLookup();

            var disabledSourcesSection = Settings.GetSection(ConfigurationConstants.DisabledPackageSources);
            var existingDisabledSources = disabledSourcesSection?.Items.OfType<AddItem>();
            Dictionary<string, AddItem> existingDisabledSourcesLookup = null;

            try
            {
                existingDisabledSourcesLookup = existingDisabledSources?.ToDictionary(setting => setting.Key, StringComparer.OrdinalIgnoreCase);
            }
            catch (ArgumentException e)
            {
                AddItem duplicatedKey = existingDisabledSources
                    .GroupBy(s => s.Key, StringComparer.OrdinalIgnoreCase)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.First())
                    .First();
                throw new NuGetConfigurationException(string.Format(CultureInfo.CurrentCulture, Resources.ShowError_ConfigDuplicateDisabledSources, duplicatedKey.Key, duplicatedKey.Origin.ConfigFilePath), e);
            }

            var credentialsSection = Settings.GetSection(ConfigurationConstants.CredentialsSectionName);
            var existingCredentials = credentialsSection?.Items.OfType<CredentialsItem>();
            var existingCredentialsLookup = existingCredentials?.ToDictionary(setting => setting.ElementName, StringComparer.OrdinalIgnoreCase);

            foreach (var source in sources)
            {
                AddItem existingDisabledSourceItem = null;
                SourceItem existingSourceItem = null;
                CredentialsItem existingCredentialsItem = null;

                var existingSourceIsEnabled = existingDisabledSourcesLookup == null || existingDisabledSourcesLookup.TryGetValue(source.Name, out existingDisabledSourceItem);

                if (existingSettingsLookup != null &&
                    existingSettingsLookup.TryGetValue(source.Name, out existingSourceItem))
                {
                    var oldPackageSource = ReadPackageSource(existingSourceItem, existingSourceIsEnabled, Settings);

                    existingCredentialsLookup?.TryGetValue(source.Name, out existingCredentialsItem);

                    UpdatePackageSource(
                        source,
                        oldPackageSource,
                        existingDisabledSourceItem,
                        existingCredentialsItem,
                        updateEnabled: true,
                        updateCredentials: true,
                        shouldSkipSave: true,
                        isDirty: ref isDirty);
                }
                else
                {
                    AddPackageSource(source, shouldSkipSave: true, isDirty: ref isDirty);
                }

                if (existingSourceItem != null)
                {
                    existingSettingsLookup.Remove(source.Name);
                }
            }

            if (existingSettingsLookup != null)
            {
                // get list of credentials for sources
                var sourceCredentialsSection = Settings.GetSection(ConfigurationConstants.CredentialsSectionName);
                var sourceCredentialsSettings = sourceCredentialsSection?.Items.OfType<CredentialsItem>();
                var existingsourceCredentialsLookup = sourceCredentialsSettings?.ToDictionary(setting => setting.ElementName, StringComparer.OrdinalIgnoreCase);

                foreach (var sourceItem in existingSettingsLookup)
                {
                    if (existingDisabledSourcesLookup != null && existingDisabledSourcesLookup.TryGetValue(sourceItem.Value.Key, out var existingDisabledSourceItem))
                    {
                        Settings.Remove(ConfigurationConstants.DisabledPackageSources, existingDisabledSourceItem);
                        isDirty = true;
                    }

                    if (existingsourceCredentialsLookup != null && existingsourceCredentialsLookup.TryGetValue(sourceItem.Value.Key, out var existingSourceCredentialItem))
                    {
                        Settings.Remove(ConfigurationConstants.CredentialsSectionName, existingSourceCredentialItem);
                        isDirty = true;
                    }

                    Settings.Remove(ConfigurationConstants.PackageSources, sourceItem.Value);
                    isDirty = true;
                }
            }

            if (isDirty)
            {
                Settings.SaveToDisk();
                OnPackageSourcesChanged();
                isDirty = false;
            }
        }

        private Dictionary<string, SourceItem> GetExistingSettingsLookup()
        {
            var sourcesSection = Settings.GetSection(ConfigurationConstants.PackageSources);
            var existingSettings = sourcesSection?.Items.OfType<SourceItem>().Where(
                c => !(c.Origin == null || c.Origin.IsReadOnly || c.Origin.IsMachineWide))
                .ToList();

            var existingSettingsLookup = new Dictionary<string, SourceItem>(StringComparer.OrdinalIgnoreCase);
            if (existingSettings != null)
            {
                foreach (var setting in existingSettings)
                {
                    if (existingSettingsLookup.TryGetValue(setting.Key, out var previouslyAddedSetting) &&
                        ReadProtocolVersion(previouslyAddedSetting) < ReadProtocolVersion(setting) &&
                        ReadProtocolVersion(setting) <= MaxSupportedProtocolVersion)
                    {
                        existingSettingsLookup.Remove(setting.Key);
                    }

                    existingSettingsLookup[setting.Key] = setting;
                }
            }

            return existingSettingsLookup;
        }

        /// <summary>
        /// Fires event PackageSourcesChanged
        /// </summary>
        private void OnPackageSourcesChanged()
        {
            PackageSourcesChanged?.Invoke(this, EventArgs.Empty);
        }

        public string DefaultPushSource
        {
            get
            {
                var source = SettingsUtility.GetDefaultPushSource(Settings);

                if (string.IsNullOrEmpty(source))
                {
                    source = ConfigurationDefaults.Instance.DefaultPushSource;
                }

                return source;
            }
        }

        public bool IsPackageSourceEnabled(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(name));
            }

            var disabledSources = Settings.GetSection(ConfigurationConstants.DisabledPackageSources);
            var value = disabledSources?.GetFirstItemWithAttribute<AddItem>(ConfigurationConstants.KeyAttribute, name);

            // It doesn't matter what value it is.
            // As long as the package source name is persisted in the <disabledPackageSources> section, the source is disabled.
            return value == null;
        }

        [Obsolete("IsPackageSourceEnabled(PackageSource source) is deprecated. Please use IsPackageSourceEnabled(string name) instead.")]
        public bool IsPackageSourceEnabled(PackageSource source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return IsPackageSourceEnabled(source.Name);
        }

        /// <summary>
        /// Gets the name of the ActivePackageSource from NuGet.Config
        /// </summary>
        public string ActivePackageSourceName
        {
            get
            {
                var activeSourceSection = Settings.GetSection(ConfigurationConstants.ActivePackageSourceSectionName);
                return activeSourceSection?.Items.OfType<AddItem>().FirstOrDefault()?.Key;
            }
        }

        /// <summary>
        /// Saves the <paramref name="source" /> as the active source.
        /// </summary>
        /// <param name="source"></param>
        public void SaveActivePackageSource(PackageSource source)
        {
            try
            {
                var activePackageSourceSection = Settings.GetSection(ConfigurationConstants.ActivePackageSourceSectionName);

                if (activePackageSourceSection != null)
                {
                    foreach (var activePackageSource in activePackageSourceSection.Items)
                    {
                        Settings.Remove(ConfigurationConstants.ActivePackageSourceSectionName, activePackageSource);
                    }
                }

                Settings.AddOrUpdate(ConfigurationConstants.ActivePackageSourceSectionName,
                        new AddItem(source.Name, source.Source));

                Settings.SaveToDisk();
            }
            catch (Exception)
            {
                // we want to ignore all errors here.
            }
        }

        private class IndexedPackageSource
        {
            public int Index { get; set; }

            public PackageSource PackageSource { get; set; }
        }

        public event EventHandler PackageSourcesChanged;
    }
}
