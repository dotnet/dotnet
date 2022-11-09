// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using NuGet.Common;

namespace NuGet.Configuration
{
    /// <summary>
    /// Concrete implementation of ISettings to support NuGet Settings
    /// Wrapper for computed settings from given settings files
    /// </summary>
    public class Settings : ISettings
    {
        /// <summary>
        /// Default file name for a settings file is 'NuGet.config'
        /// Also, the user level setting file at '%APPDATA%\NuGet' always uses this name
        /// </summary>
        public static readonly string DefaultSettingsFileName = "NuGet.Config";

        /// <summary>
        /// NuGet config names with casing ordered by precedence.
        /// </summary>
        public static readonly string[] OrderedSettingsFileNames =
            PathUtility.IsFileSystemCaseInsensitive ?
            new[] { DefaultSettingsFileName } :
            new[]
            {
                "nuget.config", // preferred style
                "NuGet.config", // Alternative
                DefaultSettingsFileName  // NuGet v2 style
            };

        public static readonly string[] SupportedMachineWideConfigExtension =
            RuntimeEnvironmentHelper.IsWindows ?
            new[] { "*.config" } :
            new[] { "*.Config", "*.config" };

        private readonly Dictionary<string, VirtualSettingSection> _computedSections;

        public SettingSection GetSection(string sectionName)
        {
            if (_computedSections.TryGetValue(sectionName, out var section))
            {
                return section.Clone() as SettingSection;
            }

            return null;
        }

        public void AddOrUpdate(string sectionName, SettingItem item)
        {
            if (string.IsNullOrEmpty(sectionName))
            {
                throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(sectionName));
            }

            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            // Operation is an update
            if (_computedSections.TryGetValue(sectionName, out var section) && section.Items.Contains(item))
            {
                // An update could not be possible here because the operation might be
                // in a machine wide config. If so then we want to add the item to
                // the output config.
                if (section.Update(item))
                {
                    return;
                }
            }

            // Operation is an add
            var outputSettingsFile = GetOutputSettingFileForSection(sectionName);
            if (outputSettingsFile == null)
            {
                throw new InvalidOperationException(Resources.NoWritteableConfig);
            }

            AddOrUpdate(outputSettingsFile, sectionName, item);
        }

        internal void AddOrUpdate(SettingsFile settingsFile, string sectionName, SettingItem item)
        {
            if (string.IsNullOrEmpty(sectionName))
            {
                throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(sectionName));
            }

            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var currentSettings = Priority.Last(f => f.Equals(settingsFile));
            if (settingsFile.IsMachineWide || (currentSettings?.IsMachineWide ?? false))
            {
                throw new InvalidOperationException(Resources.CannotUpdateMachineWide);
            }

            if (settingsFile.IsReadOnly || (currentSettings?.IsReadOnly ?? false))
            {
                throw new InvalidOperationException(Resources.CannotUpdateReadOnlyConfig);
            }

            if (currentSettings == null)
            {
                SettingsFiles.Add(settingsFile);
            }

            // If it is an update this will take care of it and modify the underlaying object, which is also referenced by _computedSections.
            settingsFile.AddOrUpdate(sectionName, item);

            // AddOrUpdate should have created this section, therefore this should always exist.
            settingsFile.TryGetSection(sectionName, out var settingFileSection);

            // If it is an add we have to manually add it to the _computedSections.
            var computedSectionExists = _computedSections.TryGetValue(sectionName, out var section);
            if (computedSectionExists && !section.Items.Contains(item))
            {
                var existingItem = settingFileSection.Items.First(i => i.Equals(item));
                section.Add(existingItem);
            }
            else if (!computedSectionExists)
            {
                _computedSections.Add(sectionName,
                    new VirtualSettingSection(settingFileSection));
            }
        }

        public void Remove(string sectionName, SettingItem item)
        {
            if (string.IsNullOrEmpty(sectionName))
            {
                throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(sectionName));
            }

            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (!_computedSections.TryGetValue(sectionName, out var section))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.SectionDoesNotExist, sectionName));
            }

            if (!section.Items.Contains(item))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.ItemDoesNotExist, sectionName));
            }

            section.Remove(item);

            if (section.IsEmpty())
            {
                _computedSections.Remove(sectionName);
            }
        }

        public event EventHandler SettingsChanged = delegate { };

        public Settings(string root)
            : this(new List<SettingsFile> { new SettingsFile(root) }) { }

        public Settings(string root, string fileName)
            : this(new List<SettingsFile> { new SettingsFile(root, fileName) }) { }

        public Settings(string root, string fileName, bool isMachineWide)
            : this(new List<SettingsFile>() { new SettingsFile(root, fileName, isMachineWide, isReadOnly: false) })
        {
        }

        /// <summary>
        /// All the SettingsFiles represent by this settings object.
        /// The ordering is important, closest to furthest from the user.
        /// </summary>
        private IList<SettingsFile> SettingsFiles { get; }

        /// <summary>
        /// Create a settings object.
        /// The settings files need to be ordered from closest to furthest from the user.
        /// </summary>
        /// <param name="settingsFiles"></param>
        internal Settings(IList<SettingsFile> settingsFiles)
        {
            SettingsFiles = settingsFiles ?? throw new ArgumentNullException(nameof(settingsFiles));

            var computedSections = new Dictionary<string, VirtualSettingSection>();

            // They come in priority order, closest to furthest
            // reverse merge them, so the closest ones apply.
            for (int i = settingsFiles.Count - 1; i >= 0; i--)
            {
                settingsFiles[i].MergeSectionsInto(computedSections);
            }

            _computedSections = computedSections;
        }

        private SettingsFile GetOutputSettingFileForSection(string sectionName)
        {
            // Search for the furthest from the user that can be written
            // to that is not clearing the ones before it on the hierarchy
            var writteableSettingsFiles = Priority.Where(f => !f.IsReadOnly);

            var clearedSections = writteableSettingsFiles.Select(f =>
            {
                if (f.TryGetSection(sectionName, out var section))
                {
                    return section;
                }
                return null;
            }).Where(s => s != null && s.Items.Contains(new ClearItem()));

            if (clearedSections.Any())
            {
                return clearedSections.First().Origin;
            }

            // if none have a clear tag, default to furthest from the user
            return writteableSettingsFiles.LastOrDefault();
        }

        /// <summary>
        /// Enumerates the sequence of <see cref="SettingsFile"/> instances
        /// ordered from closer to user to furthest
        /// </summary>
        internal IEnumerable<SettingsFile> Priority => SettingsFiles;

        public void SaveToDisk()
        {
            foreach (var settingsFile in Priority)
            {
                settingsFile.SaveToDisk();
            }

            SettingsChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Load default settings based on a directory.
        /// This includes machine wide settings.
        /// </summary>
        public static ISettings LoadDefaultSettings(string root)
        {
            return LoadSettings(
                root,
                configFileName: null,
                machineWideSettings: new XPlatMachineWideSetting(),
                loadUserWideSettings: true,
                useTestingGlobalPath: false);
        }

        /// <summary>
        /// Loads user settings from the NuGet configuration files. The method walks the directory
        /// tree in <paramref name="root" /> up to its root, and reads each NuGet.config file
        /// it finds in the directories. It then reads the user specific settings,
        /// which is file <paramref name="configFileName" />
        /// in <paramref name="root" /> if <paramref name="configFileName" /> is not null,
        /// If <paramref name="configFileName" /> is null, the user specific settings file is
        /// %AppData%\NuGet\NuGet.config.
        /// After that, the machine wide settings files are added.
        /// </summary>
        /// <remarks>
        /// For example, if <paramref name="root" /> is c:\dir1\dir2, <paramref name="configFileName" />
        /// is "userConfig.file", the files loaded are (in the order that they are loaded):
        /// c:\dir1\dir2\nuget.config
        /// c:\dir1\nuget.config
        /// c:\nuget.config
        /// c:\dir1\dir2\userConfig.file
        /// machine wide settings (e.g. c:\programdata\NuGet\Config\*.config)
        /// </remarks>
        /// <param name="root">
        /// The file system to walk to find configuration files.
        /// Can be null.
        /// </param>
        /// <param name="configFileName">The user specified configuration file.</param>
        /// <param name="machineWideSettings">
        /// The machine wide settings. If it's not null, the
        /// settings files in the machine wide settings are added after the user specific
        /// config file.
        /// </param>
        /// <returns>The settings object loaded.</returns>
        public static ISettings LoadDefaultSettings(
            string root,
            string configFileName,
            IMachineWideSettings machineWideSettings)
        {
            return LoadDefaultSettings(root, configFileName, machineWideSettings, settingsLoadingContext: null);
        }

        /// <summary>
        /// Loads user settings from the NuGet configuration files. The method walks the directory
        /// tree in <paramref name="root" /> up to its root, and reads each NuGet.config file
        /// it finds in the directories. It then reads the user specific settings,
        /// which is file <paramref name="configFileName" />
        /// in <paramref name="root" /> if <paramref name="configFileName" /> is not null,
        /// If <paramref name="configFileName" /> is null, the user specific settings file is
        /// %AppData%\NuGet\NuGet.config.
        /// After that, the machine wide settings files are added.
        /// </summary>
        /// <remarks>
        /// For example, if <paramref name="root" /> is c:\dir1\dir2, <paramref name="configFileName" />
        /// is "userConfig.file", the files loaded are (in the order that they are loaded):
        /// c:\dir1\dir2\nuget.config
        /// c:\dir1\nuget.config
        /// c:\nuget.config
        /// c:\dir1\dir2\userConfig.file
        /// machine wide settings (e.g. c:\programdata\NuGet\Config\*.config)
        /// </remarks>
        /// <param name="root">
        /// The file system to walk to find configuration files.
        /// Can be null.
        /// </param>
        /// <param name="configFileName">The user specified configuration file.</param>
        /// <param name="machineWideSettings">
        /// The machine wide settings. If it's not null, the
        /// settings files in the machine wide settings are added after the user specific
        /// config file.
        /// </param>
        /// <param name="settingsLoadingContext">A <see cref="SettingsLoadingContext" /> object to use when loading the settings.</param>
        /// <returns>The settings object loaded.</returns>
        public static ISettings LoadDefaultSettings(
            string root,
            string configFileName,
            IMachineWideSettings machineWideSettings,
            SettingsLoadingContext settingsLoadingContext)
        {
            return LoadSettings(
                root,
                configFileName,
                machineWideSettings,
                loadUserWideSettings: true,
                useTestingGlobalPath: false,
                settingsLoadingContext);
        }

        /// <summary>
        /// Loads Specific NuGet.Config file. The method only loads specific config file 
        /// which is file <paramref name="configFileName"/>from <paramref name="root"/>.
        /// </summary>
        public static ISettings LoadSpecificSettings(string root, string configFileName)
        {
            if (string.IsNullOrEmpty(configFileName))
            {
                throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(configFileName));
            }

            return LoadSettings(
                root,
                configFileName,
                machineWideSettings: null,
                loadUserWideSettings: true,
                useTestingGlobalPath: false);
        }

        public static ISettings LoadImmutableSettingsGivenConfigPaths(IList<string> configFilePaths, SettingsLoadingContext settingsLoadingContext)
        {
            if (configFilePaths == null || configFilePaths.Count == 0)
            {
                return NullSettings.Instance;
            }
            var settings = new List<SettingsFile>();

            foreach (var configFilePath in configFilePaths)
            {
                settings.Add(settingsLoadingContext.GetOrCreateSettingsFile(configFilePath, isReadOnly: true));
            }

            return new ImmutableSettings(LoadSettingsGivenSettingsFiles(settings));
        }

        public static ISettings LoadSettingsGivenConfigPaths(IList<string> configFilePaths)
        {
            var settings = new List<SettingsFile>();
            if (configFilePaths == null || configFilePaths.Count == 0)
            {
                return NullSettings.Instance;
            }

            foreach (var configFile in configFilePaths)
            {
                var file = new FileInfo(configFile);
                settings.Add(new SettingsFile(file.DirectoryName, file.Name));
            }

            return LoadSettingsGivenSettingsFiles(settings);
        }


        private static ISettings LoadSettingsGivenSettingsFiles(List<SettingsFile> settings)
        {
            return LoadSettingsForSpecificConfigs(
                            settings.First().DirectoryPath,
                            settings.First().FileName,
                            validSettingFiles: settings,
                            machineWideSettings: null,
                            loadUserWideSettings: false,
                            useTestingGlobalPath: false);
        }


        /// <summary>
        /// For internal use only
        /// </summary>
        internal static ISettings LoadSettings(
            string root,
            string configFileName,
            IMachineWideSettings machineWideSettings,
            bool loadUserWideSettings,
            bool useTestingGlobalPath,
            SettingsLoadingContext settingsLoadingContext = null)
        {
            // Walk up the tree to find a config file; also look in .nuget subdirectories
            // If a configFile is passed, don't walk up the tree. Only use that single config file.
            var validSettingFiles = new List<SettingsFile>();
            if (root != null && string.IsNullOrEmpty(configFileName))
            {
                validSettingFiles.AddRange(
                    GetSettingsFilesFullPath(root)
                        .Select(f =>
                        {
                            if (settingsLoadingContext == null)
                            {
                                return ReadSettings(root, f);
                            }
                            return settingsLoadingContext.GetOrCreateSettingsFile(f);
                        })
                        .Where(f => f != null));
            }

            return LoadSettingsForSpecificConfigs(
                root,
                configFileName,
                validSettingFiles,
                machineWideSettings,
                loadUserWideSettings,
                useTestingGlobalPath);
        }

        /// <summary>
        /// For internal use only.
        /// Finds and loads all configuration files within <paramref name="root" />.
        /// Does not load configuration files outside of <paramref name="root" />.
        /// </summary>
        internal static ISettings LoadSettings(
            DirectoryInfo root,
            IMachineWideSettings machineWideSettings,
            bool loadUserWideSettings,
            bool useTestingGlobalPath,
            SettingsLoadingContext settingsLoadingContext = null)
        {
            var validSettingFiles = new List<SettingsFile>();
            var comparer = PathUtility.GetStringComparisonBasedOnOS();
            var settingsFiles = root.GetFileSystemInfos("*.*", SearchOption.AllDirectories)
                .OfType<FileInfo>()
                .OrderByDescending(file => file.FullName.Count(c => c == Path.DirectorySeparatorChar))
                .Where(file => OrderedSettingsFileNames.Any(fileName => fileName.Equals(file.Name, comparer)));

            validSettingFiles.AddRange(
                settingsFiles
                    .Select(file => ReadSettings(file.DirectoryName, file.FullName, settingsLoadingContext: settingsLoadingContext))
                    .Where(file => file != null));

            return LoadSettingsForSpecificConfigs(
                root.FullName,
                configFileName: null,
                validSettingFiles,
                machineWideSettings,
                loadUserWideSettings,
                useTestingGlobalPath,
                settingsLoadingContext);
        }

        private static ISettings LoadSettingsForSpecificConfigs(
            string root,
            string configFileName,
            List<SettingsFile> validSettingFiles,
            IMachineWideSettings machineWideSettings,
            bool loadUserWideSettings,
            bool useTestingGlobalPath,
            SettingsLoadingContext settingsLoadingContext = null)
        {
            if (loadUserWideSettings)
            {
                validSettingFiles.AddRange(LoadUserSpecificSettings(root, configFileName, useTestingGlobalPath, settingsLoadingContext));
            }

            if (machineWideSettings != null && machineWideSettings.Settings is Settings mwSettings && string.IsNullOrEmpty(configFileName))
            {
                // Priority gives you the settings file in the order you want to start reading them
                var files = mwSettings.Priority.Select(
                    s => ReadSettings(s.DirectoryPath, s.FileName, s.IsMachineWide, settingsLoadingContext: settingsLoadingContext));

                validSettingFiles.AddRange(files);
            }

            if (validSettingFiles?.Any() != true)
            {
                // This means we've failed to load all config files and also failed to load or create the one in %AppData%
                // Work Item 1531: If the config file is malformed and the constructor throws, NuGet fails to load in VS.
                // Returning a null instance prevents us from silently failing and also from picking up the wrong config
                return NullSettings.Instance;
            }

            // Create a settings object with the linked list head. Typically, it's either the config file in %ProgramData%\NuGet\Config,
            // or the user wide config (%APPDATA%\NuGet\nuget.config) if there are no machine
            // wide config files. The head file is the one we want to read first, while the user wide config
            // is the one that we want to write to.
            // TODO: add UI to allow specifying which one to write to
            return new Settings(settingsFiles: validSettingFiles);
        }

        /// <summary>
        /// Load the user specific settings
        /// </summary>
        /// <param name="root"></param>
        /// <param name="configFileName"></param>
        /// <param name="useTestingGlobalPath"></param>
        /// <param name="settingsLoadingContext"></param>
        /// <returns></returns>
        internal static IEnumerable<SettingsFile> LoadUserSpecificSettings(
            string root,
            string configFileName,
            bool useTestingGlobalPath,
            SettingsLoadingContext settingsLoadingContext = null)
        {
            // Path.Combine is performed with root so it should not be null
            // However, it is legal for it be empty in this method
            var rootDirectory = root ?? string.Empty;

            if (configFileName == null)
            {
                string userSettingsDir = GetUserSettingsDirectory(rootDirectory, useTestingGlobalPath);
                if (userSettingsDir == null)
                {
                    yield break;
                }

                var defaultSettingsFilePath = Path.Combine(userSettingsDir, DefaultSettingsFileName);

                // ReadSettings will try to create the default config file if it doesn't exist
                SettingsFile userSpecificSettings = ReadSettings(rootDirectory, defaultSettingsFilePath, settingsLoadingContext: settingsLoadingContext);

                yield return userSpecificSettings;

                // For backwards compatibility, we first return default user specific the non-default configs and then the additional files from the nested `config` directory
                var additionalConfigurationPath = GetAdditionalUserWideConfigurationDirectory(userSettingsDir);
                foreach (var file in FileSystemUtility
                    .GetFilesRelativeToRoot(root: additionalConfigurationPath, filters: SupportedMachineWideConfigExtension, searchOption: SearchOption.TopDirectoryOnly)
                    .OrderBy(e => e, PathUtility.GetStringComparerBasedOnOS()))
                {
                    if (!PathUtility.GetStringComparerBasedOnOS().Equals(DefaultSettingsFileName, file))
                    {
                        var settings = ReadSettings(additionalConfigurationPath, file, isMachineWideSettings: false, isAdditionalUserWideConfig: true);
                        if (settings != null)
                        {
                            yield return settings;
                        }
                    }
                }
            }
            else
            {
                if (!FileSystemUtility.DoesFileExistIn(rootDirectory, configFileName))
                {
                    var message = string.Format(CultureInfo.CurrentCulture,
                        Resources.FileDoesNotExist,
                        Path.Combine(rootDirectory, configFileName));
                    throw new InvalidOperationException(message);
                }

                yield return ReadSettings(rootDirectory, configFileName, settingsLoadingContext: settingsLoadingContext);
            }
        }

        private static string GetUserSettingsDirectory(string rootDirectory, bool useTestingGlobalPath)
        {
            return useTestingGlobalPath
                ? Path.Combine(rootDirectory, "TestingGlobalPath")
                : NuGetEnvironment.GetFolderPath(NuGetFolderPath.UserSettingsDirectory);
        }

        private static string GetAdditionalUserWideConfigurationDirectory(string userSettingsDirectory)
        {
            return Path.Combine(userSettingsDirectory, ConfigurationConstants.Config);
        }

        /// <summary>
        /// Loads the machine wide settings.
        /// </summary>
        /// <remarks>
        /// For example, if <paramref name="paths" /> is {"IDE", "Version", "SKU" }, then
        /// the files loaded are (in the order that they are loaded):
        /// %programdata%\NuGet\Config\IDE\Version\SKU\*.config
        /// %programdata%\NuGet\Config\IDE\Version\*.config
        /// %programdata%\NuGet\Config\IDE\*.config
        /// %programdata%\NuGet\Config\*.config
        /// </remarks>
        /// <param name="root">The file system in which the settings files are read.</param>
        /// <param name="paths">The additional paths under which to look for settings files.</param>
        /// <returns>The list of settings read.</returns>
        public static ISettings LoadMachineWideSettings(
            string root,
            params string[] paths)
        {
            if (string.IsNullOrEmpty(root))
            {
                throw new ArgumentException("root cannot be null or empty");
            }

            var settingFiles = new List<SettingsFile>();
            var combinedPath = Path.Combine(paths);

            while (true)
            {
                // load setting files in directory
                foreach (var file in FileSystemUtility.GetFilesRelativeToRoot(root, combinedPath, SupportedMachineWideConfigExtension, SearchOption.TopDirectoryOnly))
                {
                    var settings = ReadSettings(root, file, isMachineWideSettings: true);
                    if (settings != null)
                    {
                        settingFiles.Add(settings);
                    }
                }

                if (combinedPath.Length == 0)
                {
                    break;
                }

                var index = combinedPath.LastIndexOf(Path.DirectorySeparatorChar);
                if (index < 0)
                {
                    index = 0;
                }
                combinedPath = combinedPath.Substring(0, index);
            }

            if (settingFiles.Any())
            {
                return new Settings(settingFiles);
            }

            return NullSettings.Instance;
        }

        public static string ApplyEnvironmentTransform(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            return Environment.ExpandEnvironmentVariables(value);
        }

        public static Tuple<string, string> GetFileNameAndItsRoot(string root, string settingsPath)
        {
            string fileName = null;
            string directory = null;

            if (Path.IsPathRooted(settingsPath))
            {
                fileName = Path.GetFileName(settingsPath);
                directory = Path.GetDirectoryName(settingsPath);
            }
            else if (!FileSystemUtility.IsPathAFile(settingsPath))
            {
                var fullPath = Path.Combine(root ?? string.Empty, settingsPath);
                fileName = Path.GetFileName(fullPath);
                directory = Path.GetDirectoryName(fullPath);
            }
            else
            {
                fileName = settingsPath;
                directory = root;
            }

            return new Tuple<string, string>(fileName, directory);
        }

        /// <summary>
        /// Get a list of all the paths of the settings files used as part of this settings object
        /// </summary>
        public IList<string> GetConfigFilePaths()
        {
            return Priority.Select(config => config.ConfigFilePath).ToList();
        }

        /// <summary>
        /// Get a list of all the roots of the settings files used as part of this settings object
        /// </summary>
        public IList<string> GetConfigRoots()
        {
            return Priority.Select(config => config.DirectoryPath).Distinct().ToList();
        }

        internal static string ResolvePathFromOrigin(string originDirectoryPath, string originFilePath, string path)
        {
            if (Uri.TryCreate(path, UriKind.Relative, out var _) &&
                !string.IsNullOrEmpty(originDirectoryPath) &&
                !string.IsNullOrEmpty(originFilePath))
            {
                return ResolveRelativePath(originDirectoryPath, originFilePath, path);
            }

            return path;
        }

        private static string ResolveRelativePath(string originDirectoryPath, string originFilePath, string path)
        {
            if (string.IsNullOrEmpty(originDirectoryPath) || string.IsNullOrEmpty(originFilePath))
            {
                return null;
            }

            if (string.IsNullOrEmpty(path))
            {
                return path;
            }

            var rawPath = Path.Combine(originDirectoryPath, ResolvePath(Path.GetDirectoryName(originFilePath), path));
            var normalizedPath = new DirectoryInfo(rawPath).FullName;
            return normalizedPath;
        }

        private static string ResolvePath(string configDirectory, string value)
        {
            // Three cases for when Path.IsRooted(value) is true:
            // 1- C:\folder\file
            // 2- \\share\folder\file
            // 3- \folder\file
            // In the first two cases, we want to honor the fully qualified path
            // In the last case, we want to return X:\folder\file with X: drive where config file is located.
            // However, Path.Combine(path1, path2) always returns path2 when Path.IsRooted(path2) == true (which is current case)
            string root;

            try
            {
                root = Path.GetPathRoot(value);
            }
            catch (ArgumentException ex)
            {
                throw new NuGetConfigurationException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.ShowError_ConfigHasInvalidPackageSource, NuGetLogCode.NU1006, value, ex.Message),
                    ex);
            }

            // this corresponds to 3rd case
            if (root != null
                && root.Length == 1
                && (root[0] == Path.DirectorySeparatorChar || value[0] == Path.AltDirectorySeparatorChar))
            {
                return Path.Combine(Path.GetPathRoot(configDirectory), value.Substring(1));
            }
            return Path.Combine(configDirectory, value);
        }

        private static SettingsFile ReadSettings(string settingsRoot, string settingsPath, bool isMachineWideSettings = false, bool isAdditionalUserWideConfig = false, SettingsLoadingContext settingsLoadingContext = null)
        {
            try
            {
                if (settingsLoadingContext != null)
                {
                    return settingsLoadingContext.GetOrCreateSettingsFile(settingsPath, isMachineWideSettings, isAdditionalUserWideConfig);
                }

                var tuple = GetFileNameAndItsRoot(settingsRoot, settingsPath);
                var filename = tuple.Item1;
                var root = tuple.Item2;
                return new SettingsFile(root, filename, isMachineWideSettings, isAdditionalUserWideConfig);
            }
            catch (XmlException)
            {
                return null;
            }
        }

        /// <remarks>
        /// Order is most significant (e.g. applied last) to least significant (applied first)
        /// ex:
        /// c:\someLocation\nuget.config
        /// c:\nuget.config
        /// </remarks>
        private static IEnumerable<string> GetSettingsFilesFullPath(string root)
        {
            // for dirs obtained by walking up the tree, only consider setting files that already exist.
            // otherwise we'd end up creating them.
            foreach (var dir in GetSettingsFilePaths(root))
            {
                var fileName = GetSettingsFileNameFromDir(dir);
                if (fileName != null)
                {
                    yield return fileName;
                }
            }

            yield break;
        }

        /// <summary>
        /// Checks for each possible casing of nuget.config in the directory. The first match is
        /// returned. If there are no nuget.config files null is returned.
        /// </summary>
        /// <remarks>For windows <see cref="OrderedSettingsFileNames"/> contains a single casing since
        /// the file system is case insensitive.</remarks>
        private static string GetSettingsFileNameFromDir(string directory)
        {
            foreach (var nugetConfigCasing in OrderedSettingsFileNames)
            {
                var file = Path.Combine(directory, nugetConfigCasing);
                if (File.Exists(file))
                {
                    return file;
                }
            }

            return null;
        }

        private static IEnumerable<string> GetSettingsFilePaths(string root)
        {
            while (root != null)
            {
                yield return root;
                root = Path.GetDirectoryName(root);
            }

            yield break;
        }
    }
}
