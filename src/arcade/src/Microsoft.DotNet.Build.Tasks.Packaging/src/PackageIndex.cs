// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NuGet.Frameworks;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Collections;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;

namespace Microsoft.DotNet.Build.Tasks.Packaging
{
    public class PackageIndex
    {
        static JsonSerializer s_serializer = CreateSerializer();

        static ConcurrentDictionary<string, PackageIndex> s_indexCache = new ConcurrentDictionary<string, PackageIndex>();

        public SortedDictionary<string, PackageInfo> Packages { get; set; } = new SortedDictionary<string, PackageInfo>();

        public SortedDictionary<string, string> ModulesToPackages { get; set; } = new SortedDictionary<string, string>();

        public MetaPackages MetaPackages { get; set; } = new MetaPackages();

        public string PreRelease { get; set; }

        [JsonIgnore]
        public HashSet<string> IndexSources { get; set; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public static PackageIndex Load(IEnumerable<string> packageIndexFiles)
        {
            string indexKey = String.Join("|",
                packageIndexFiles.Select(packageIndexFile => new FileInfo(packageIndexFile))
                                 .Select(packageIndexFileInfo => $"{packageIndexFileInfo.FullName}:{packageIndexFileInfo.Length}:{packageIndexFileInfo.LastWriteTimeUtc.Ticks}"));

            PackageIndex result = null;

            if (s_indexCache.TryGetValue(indexKey, out result))
            {
                return result;
            }

            foreach(var packageIndexFile in packageIndexFiles)
            {
                if (result == null)
                {
                    result = Load(packageIndexFile);
                }
                else
                {
                    result.Merge(packageIndexFile);
                }
            }

            s_indexCache[indexKey] = result;

            return result;
        }

        public static PackageIndex Load(string packageIndexFile)
        {
            using (var file = File.OpenText(packageIndexFile))
            using (var jsonTextReader = new JsonTextReader(file))
            {
                var result = s_serializer.Deserialize<PackageIndex>(jsonTextReader);
                result.IndexSources.Add(Path.GetFullPath(packageIndexFile));
                return result;
            }
        }

        public void Save(string path)
        {
            string directory = Path.GetDirectoryName(path);
            if (!String.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (var file = File.CreateText(path))
            {
                s_serializer.Serialize(file, this);
            }
        }

        private static JsonSerializer CreateSerializer()
        {
            var serializer = new JsonSerializer();
            serializer.StringEscapeHandling = StringEscapeHandling.EscapeNonAscii;
            serializer.Formatting = Formatting.Indented;
            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.DefaultValueHandling = DefaultValueHandling.Ignore;

            serializer.Converters.Add(new VersionConverter());
            serializer.Converters.Add(new InboxFrameworksConverter());
            serializer.Converters.Add(new MetaPackagesConverter());

            return serializer;
        }

        /// <summary>
        /// Merges an index into the currently loaded index if not already merged.
        /// </summary>
        /// <param name="otherIndexFile"></param>
        public void Merge(string otherIndexFile)
        {
            if (!IndexSources.Contains(otherIndexFile))
            {
                Merge(Load(otherIndexFile));
            }
        }

        /// <summary>
        /// Merges a list of indexes into the currently loaded index if not already merged.
        /// </summary>
        /// <param name="otherIndexFiles"></param>
        public void Merge(IEnumerable<string> otherIndexFiles)
        {
            foreach(var otherIndexFile in otherIndexFiles)
            {
                Merge(otherIndexFile);
            }
        }

        /// <summary>
        /// Merges another packageIndex into this package index.  For any overlapping
        /// data 'other' has precedence.
        /// </summary>
        /// <param name="other"></param>
        public void Merge(PackageIndex other)
        {
            if (other.IndexSources.IsSubsetOf(IndexSources))
            {
                return;
            }

            // if pre-release is set on this index and different than the other
            // move pre-release to individual infos
            if (PreRelease != null && !PreRelease.Equals(other.PreRelease))
            {
                foreach(var info in Packages.Values)
                {
                    if (info.PreRelease == null)
                    {
                        info.PreRelease = PreRelease;
                    }
                }

                PreRelease = null;
            }

            foreach(var otherPackage in other.Packages)
            {
                var otherInfo = otherPackage.Value;
                PackageInfo info;

                if (Packages.TryGetValue(otherPackage.Key, out info))
                {
                    info.Merge(otherInfo);
                }
                else
                {
                    Packages[otherPackage.Key] = info = otherInfo;

                    // if pre-release is set on the other index and doesn't match the value of the info, set it
                    if (other.PreRelease != null && !other.PreRelease.Equals(info.PreRelease))
                    {
                        info.PreRelease = other.PreRelease;
                    }
                }
            }

            foreach(var otherModuleToPackage in other.ModulesToPackages)
            {
                ModulesToPackages[otherModuleToPackage.Key] = otherModuleToPackage.Value;
            }

            MetaPackages.Merge(other.MetaPackages);

            foreach(var otherIndexSource in other.IndexSources)
            {
                IndexSources.Add(otherIndexSource);
            }
        }

        public void MergeInboxFromLayout(NuGetFramework framework, string layoutDirectory, bool addPackages = true)
        {
            foreach(var file in Directory.EnumerateFiles(layoutDirectory, "*.dll", SearchOption.AllDirectories))
            {
                var assemblyName = Path.GetFileNameWithoutExtension(file);
                var assemblyVersion = VersionUtility.GetAssemblyVersion(file);

                TryAddInbox(assemblyName, framework, assemblyVersion, addPackages);
            }
        }

        public void MergeFrameworkLists(string frameworkListDirectory, bool addPackages = true)
        {
            foreach (string frameworkDir in Directory.EnumerateDirectories(frameworkListDirectory))
            {
                string targetFrameworkMoniker = Path.GetFileName(frameworkDir);
                NuGetFramework framework = NuGetFramework.Parse(targetFrameworkMoniker);
                foreach (string frameworkListPath in Directory.EnumerateFiles(frameworkDir, "*.xml"))
                {
                    MergeFrameworkList(framework, frameworkListPath, addPackages);
                }
            }
        }

        public void MergeFrameworkList(NuGetFramework framework, string frameworkListPath, bool addPackages = true)
        {
            XDocument frameworkList = XDocument.Load(frameworkListPath);
            foreach (var file in frameworkList.Element("FileList").Elements("File"))
            {
                string assemblyName = file.Attribute("AssemblyName").Value;
                var versionAttribute = file.Attribute("Version");
                Version supportedVersion = (versionAttribute != null) ? new Version(versionAttribute.Value) : VersionUtility.MaxVersion;

                TryAddInbox(assemblyName, framework, supportedVersion, addPackages);
            }
        }

        private void TryAddInbox(string assemblyName, NuGetFramework framework, Version version, bool addPackages = true)
        {
            PackageInfo info;
            if (Packages.TryGetValue(assemblyName, out info))
            {
                info.InboxOn.AddInboxVersion(framework, version);
            }
            else if (addPackages)
            {
                Packages[assemblyName] = info = new PackageInfo();
                info.InboxOn.AddInboxVersion(framework, version);
            }
        }

        // helper functions
        public bool TryGetBaseLineVersion(string packageId, out Version baseLineVersion)
        {
            PackageInfo info;
            baseLineVersion = null;

            if (Packages.TryGetValue(packageId, out info))
            {
                baseLineVersion = info.BaselineVersion;
            }

            return baseLineVersion != null;
        }

        public IEnumerable<NuGetFramework> GetAlllInboxFrameworks()
        {
            // very inefficient, included for legacy reasons.
            return Packages.Values.SelectMany(info => info.InboxOn.GetInboxFrameworks()).Distinct().ToArray();
        }

        public IEnumerable<NuGetFramework> GetInboxFrameworks(string assemblyName)
        {
            IEnumerable<NuGetFramework> inboxFrameworks = null;
            PackageInfo info;

            if (Packages.TryGetValue(assemblyName, out info))
            {
                inboxFrameworks = info.InboxOn.GetInboxFrameworks();
            }

            return inboxFrameworks;
        }

        public IEnumerable<NuGetFramework> GetInboxFrameworks(string assemblyName, string assemblyVersionString)
        {
            Version assemblyVersion = FrameworkUtilities.Ensure4PartVersion(assemblyVersionString);

            return GetInboxFrameworks(assemblyName, assemblyVersion);
        }

        public IEnumerable<NuGetFramework> GetInboxFrameworks(string assemblyName, Version assemblyVersion)
        {
            IEnumerable<NuGetFramework> inboxFrameworks = null;
            PackageInfo info;

            if (Packages.TryGetValue(assemblyName, out info))
            {
                inboxFrameworks = info.InboxOn.GetInboxVersions().Where(p => p.Value >= assemblyVersion).Select(p => p.Key);
            }

            return inboxFrameworks;
        }

        public bool IsInbox(string assemblyName, NuGetFramework framework, string assemblyVersionString)
        {
            Version assemblyVersion = FrameworkUtilities.Ensure4PartVersion(assemblyVersionString);

            return IsInbox(assemblyName, framework, assemblyVersion);
        }

        public bool IsInbox(string assemblyName, NuGetFramework framework, Version assemblyVersion)
        {
            PackageInfo info;
            bool isInbox = false;

            if (Packages.TryGetValue(assemblyName, out info))
            {
                isInbox = info.InboxOn.IsInbox(framework, assemblyVersion);
            }

            return isInbox;
        }

        public bool IsStable(string packageId, Version packageVersion)
        {
            PackageInfo info;
            bool isStable = false;

            if (Packages.TryGetValue(packageId, out info))
            {
                isStable = info.StableVersions.Contains(packageVersion);
            }

            return isStable;
        }

        public string GetPreRelease(string packageId)
        {
            PackageInfo info;
            string preRelease = null;

            if (Packages.TryGetValue(packageId, out info))
            {
                preRelease = info.PreRelease;
            }

            if (preRelease == null)
            {
                preRelease = PreRelease;
            }

            return preRelease;
        }

        public Version GetPackageVersionForAssemblyVersion(string packageId, Version assemblyVersion)
        {
            PackageInfo info;
            Version packageVersion = null;

            if (assemblyVersion != null)
            {
                if (Packages.TryGetValue(packageId, out info))
                {
                    packageVersion = info.GetPackageVersionForAssemblyVersion(assemblyVersion);
                }
                else
                {
                    // if not found assume 1:1 with assembly version
                    packageVersion = VersionUtility.As3PartVersion(assemblyVersion);
                }
            }

            return packageVersion;
        }
    }

    public class PackageInfo
    {
        public HashSet<Version> StableVersions { get; set; } = new HashSet<Version>();

        public bool ShouldSerializeStableVersions() { return StableVersions.Count > 0; }

        /// <summary>
        /// Minimum version to use when referencing this package.
        /// </summary>
        public Version BaselineVersion { get; set; }

        /// <summary>
        /// Mapping of frameworks which contain this package inbox (or in a framework package)
        /// </summary>
        public InboxFrameworks InboxOn { get; set; } = new InboxFrameworks();

        public bool ShouldSerializeInboxFrameworkAssemblyVersions() { return InboxOn.Count > 0; }

        /// <summary>
        /// Mapping of assembly version to package version which (first) contains that assembly version.
        /// </summary>
        public SortedDictionary<Version, Version> AssemblyVersionInPackageVersion { get; set; } = new SortedDictionary<Version, Version>();
        
        public bool ShouldSerializeAssemblyVersionInPackageVersion() { return AssemblyVersionInPackageVersion.Count > 0; }

        public string PreRelease { get; set; }

        public void Merge(PackageInfo other)
        {
            StableVersions.UnionWith(other.StableVersions);

            if (other.BaselineVersion != null && BaselineVersion == null)
            {
                BaselineVersion = other.BaselineVersion;
            }

            if (other.PreRelease != null && PreRelease == null)
            {
                PreRelease = other.PreRelease;
            }

            foreach(var inboxOnPair in other.InboxOn.GetInboxVersions())
            {
                InboxOn.AddInboxVersion(inboxOnPair.Key, inboxOnPair.Value);
            }

            foreach (var assemblyVersionInPackage in other.AssemblyVersionInPackageVersion)
            {
                Version otherAssemblyVersion = assemblyVersionInPackage.Key;
                Version otherPackageVersion = assemblyVersionInPackage.Value;

                AddAssemblyVersionInPackage(otherAssemblyVersion, otherPackageVersion);
            }
        }

        public void AddAssemblyVersionsInPackage(IEnumerable<Version> assemblyVersions, Version packageVersion)
        {
            foreach (var assemblyVersion in assemblyVersions)
            {
                AddAssemblyVersionInPackage(assemblyVersion, packageVersion);
            }
        }

        public void AddAssemblyVersionInPackage(Version assemblyVersion, Version packageVersion)
        {
            Version existingPackageVersion;
            if (AssemblyVersionInPackageVersion.TryGetValue(assemblyVersion, out existingPackageVersion))
            {
                bool existingStable = StableVersions.Contains(existingPackageVersion);
                bool updateStable = StableVersions.Contains(packageVersion);
                
                // always prefer a stable package over unstable package
                // use the highest unstable package version
                // use the lowest stable package version
                if ((updateStable && !existingStable) || // update to stable from unstable
                    (updateStable && existingStable && packageVersion < existingPackageVersion) || // update to lower stable
                    (!updateStable && !existingStable && packageVersion > existingPackageVersion)) // update to higher non-stable version
                {
                    AssemblyVersionInPackageVersion[assemblyVersion] = packageVersion;
                }
            }
            else
            {
                AssemblyVersionInPackageVersion[assemblyVersion] = packageVersion;
            }
        }
        public Version GetPackageVersionForAssemblyVersion(NuGetFramework framework, Version assemblyVersion)
        {
            Version packageVersion = null;

            if (assemblyVersion != null)
            {
                // prefer an explicit mapping
                if (!AssemblyVersionInPackageVersion.TryGetValue(assemblyVersion, out packageVersion))
                {
                    // if not found assume 1:1 with assembly version
                    packageVersion = VersionUtility.As3PartVersion(assemblyVersion);
                }
            }

            return packageVersion;
        }

        public Version GetPackageVersionForAssemblyVersion(Version assemblyVersion)
        {
            Version packageVersion = null;

            if (assemblyVersion != null)
            {
                // prefer an explicit mapping
                if (!AssemblyVersionInPackageVersion.TryGetValue(assemblyVersion, out packageVersion))
                {
                    // if not found assume 1:1 with assembly version
                    packageVersion = VersionUtility.As3PartVersion(assemblyVersion);
                }
            }

            return packageVersion;
        }
    }

    public class InboxFrameworks
    {
        private SortedDictionary<string, SortedDictionary<Version, Version>> inboxVersions = new SortedDictionary<string, SortedDictionary<Version, Version>>();

        public int Count { get { return inboxVersions.Sum(m => m.Value.Count); } }

        public void AddInboxVersion(NuGetFramework framework, Version assemblyVersion)
        {
            var normalizedFramework = NormalizeFramework(framework);
            var frameworkKey = GetFrameworkKey(normalizedFramework);
            var frameworkVersion = normalizedFramework.Version;

            SortedDictionary<Version, Version> mappings;
            if (!inboxVersions.TryGetValue(frameworkKey, out mappings))
            {
                inboxVersions[frameworkKey] = mappings = new SortedDictionary<Version, Version>();
            }
            
            if (assemblyVersion == null)
            {
                // explicitly not supported.
                mappings[normalizedFramework.Version] = assemblyVersion;
            }
            else
            {
                List<Version> redundantMappings = new List<Version>();

                var addMapping = true;
                foreach(var mapping in mappings)
                {
                    var existingFrameworkVersion = mapping.Key;
                    var existingAssemblyVersion = mapping.Value;

                    if (existingFrameworkVersion <= frameworkVersion)
                    {
                        if (existingAssemblyVersion != null && existingAssemblyVersion >= assemblyVersion)
                        {
                            // lower or same framework already maps this or higher version, don't add it
                            addMapping = false;
                        }
                    }
                    else
                    {
                        if (existingAssemblyVersion <= assemblyVersion)
                        {
                            // higher framework version with an equal or lower assembly version, remove it.
                            redundantMappings.Add(mapping.Key);
                        }
                    }
                }

                if (addMapping)
                {
                    mappings[normalizedFramework.Version] = assemblyVersion;
                }

                foreach(var redundantMapping in redundantMappings)
                {
                    mappings.Remove(redundantMapping);
                }
            }


        }

        public IEnumerable<NuGetFramework> GetInboxFrameworks()
        {
            foreach (var framework in inboxVersions)
            {
                foreach (var frameworkVersion in framework.Value.Keys)
                {
                    var fx = FromFrameworkKeyAndVersion(framework.Key, frameworkVersion);

                    yield return fx;
                }
            }
        }

        public IEnumerable<KeyValuePair<NuGetFramework, Version>> GetInboxVersions()
        {
            foreach (var framework in inboxVersions)
            {
                foreach (var frameworkInboxVersionPair in framework.Value)
                {
                    var frameworkVersion = frameworkInboxVersionPair.Key;
                    var assemblyVersion = frameworkInboxVersionPair.Value;

                    var fx = FromFrameworkKeyAndVersion(framework.Key, frameworkVersion);

                    yield return new KeyValuePair<NuGetFramework, Version>(fx, assemblyVersion);
                }
            }
        }

        public bool IsAnyVersionInbox(NuGetFramework framework)
        {
            var normalizedFramework = NormalizeFramework(framework);
            var key = GetFrameworkKey(normalizedFramework);

            SortedDictionary<Version, Version> mappings;
            if (!inboxVersions.TryGetValue(key, out mappings))
            {
                // no inbox info for this framework
                return false;
            }

            return mappings.Keys.Any(fxVer => fxVer <= framework.Version);
        }

        public bool IsInbox(NuGetFramework framework, Version assemblyVersion, bool permitRevisions = false)
        {
            var normalizedFramework = NormalizeFramework(framework);
            var key = GetFrameworkKey(normalizedFramework);

            SortedDictionary<Version, Version> mappings;
            if (!inboxVersions.TryGetValue(key, out mappings))
            {
                // no inbox info for this framework
                return false;
            }

            Version assemblyVersionInbox;
            if (mappings.TryGetValue(normalizedFramework.Version, out assemblyVersionInbox))
            {
                if (assemblyVersionInbox == null)
                {
                    // null entry means it's explicitly not inbox
                    return false;
                }

                if (assemblyVersionInbox == VersionUtility.MaxVersion)
                {
                    return true;
                }

                // inbox if explicit entry is greater than or equal to current
                return permitRevisions ? 
                    VersionUtility.IsCompatibleApiVersion(assemblyVersionInbox, assemblyVersion) : 
                    assemblyVersionInbox >= assemblyVersion;
            }

            // find nearest
            var compatibleMapping = mappings.LastOrDefault(m => m.Key < normalizedFramework.Version);
            if (compatibleMapping.Key == null || compatibleMapping.Value == null)
            {
                // either no compatible mapping, or compatible mapping explicitly not inbox
                return false;
            }

            if (compatibleMapping.Value == VersionUtility.MaxVersion)
            {
                return true;
            }

            // inbox if compatible entry is greater than or equal to current
            return permitRevisions ?
                    VersionUtility.IsCompatibleApiVersion(compatibleMapping.Value, assemblyVersion) :
                    compatibleMapping.Value >= assemblyVersion;
        }


        private static NuGetFramework NormalizeFramework(NuGetFramework framework)
        {
            if (framework == FrameworkConstants.CommonFrameworks.NetCore50)
            {
                // normalize netcore50 -> UAP10.
                // this permits us to model that netcore50/uap10 should not inherit inbox state from netcore4*
                return FrameworkConstants.CommonFrameworks.UAP10;
            }
            else if (framework == FrameworkConstants.CommonFrameworks.NetCore45)
            {
                return FrameworkConstants.CommonFrameworks.Win8;
            }
            else if (framework == FrameworkConstants.CommonFrameworks.NetCore451)
            {
                return FrameworkConstants.CommonFrameworks.Win81;
            }

            return framework;
        }

        private static string GetFrameworkKey(NuGetFramework framework)
        {
            if (!String.IsNullOrEmpty(framework.Profile))
            {
                return framework.Framework + "," + framework.Profile;
            }
            return framework.Framework;
        }

        private static NuGetFramework FromFrameworkKeyAndVersion(string key, Version version)
        {
            var parts = key.Split(',');

            if (parts.Length > 1)
            {
                return new NuGetFramework(parts[0], version, parts[1]);
            }
            else
            {
                return new NuGetFramework(key, version);
            }
        }
    }

    public class InboxFrameworksConverter : JsonConverter
    {
        private const string AnyVersion = "Any";
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(InboxFrameworks);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jobj = JObject.Load(reader);

            var result = new InboxFrameworks();
            foreach (var property in jobj.Properties())
            {
                var versionString = property.Value.ToString();
                var version = versionString.Equals(AnyVersion, StringComparison.OrdinalIgnoreCase) ? VersionUtility.MaxVersion : new Version(versionString);
                result.AddInboxVersion(NuGetFramework.Parse(property.Name), version);
            }

            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else if (value is InboxFrameworks)
            {
                writer.WriteStartObject();

                foreach(var frameworkPair in ((InboxFrameworks)value).GetInboxVersions())
                {
                    var shortName = frameworkPair.Key.GetShortFolderName();
                    var assemblyVersion = frameworkPair.Value;
                    var assemblyVersionString = assemblyVersion == VersionUtility.MaxVersion ? AnyVersion : assemblyVersion.ToString();
                    writer.WritePropertyName(shortName);
                    writer.WriteValue(assemblyVersionString);
                }

                writer.WriteEndObject();
            }
            else
            {
                throw new JsonSerializationException($"Expected {nameof(InboxFrameworks)} but got {value.GetType()}");
            }
        }
    }

    public class MetaPackages
    {
        private Dictionary<string, string> packageToMetaPackage = new Dictionary<string, string>();

        public void AddMetaPackageMapping(string packageId, string metaPackageId)
        {
            string existingMetaPackage;

            if (packageToMetaPackage.TryGetValue(packageId, out existingMetaPackage))
            {
                if (existingMetaPackage != metaPackageId)
                {
                    throw new InvalidOperationException($"Package {packageId} cannot be mapped to {metaPackageId} because it is already mapped to {existingMetaPackage}.");
                }
            }
            else
            {
                packageToMetaPackage.Add(packageId, metaPackageId);
            }
        }

        public string GetMetaPackageId(string packageId)
        {
            string metaPackageId;

            packageToMetaPackage.TryGetValue(packageId, out metaPackageId);

            return metaPackageId;
        }

        public void Merge(MetaPackages other)
        {
            foreach(var metaPackage in other.packageToMetaPackage)
            {
                // only merge a meta-package definition if it wasn't defined
                if (!packageToMetaPackage.ContainsKey(metaPackage.Key))
                {
                    packageToMetaPackage.Add(metaPackage.Key, metaPackage.Value);
                }
            }
        }

        internal IEnumerable<IGrouping<string, string>> GetMetaPackageGrouping()
        {
            return packageToMetaPackage.GroupBy(p => p.Value, p => p.Key);
        }
    }

    public class MetaPackagesConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(MetaPackages);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jobj = JObject.Load(reader);

            var result = new MetaPackages();
            foreach (var property in jobj.Properties())
            {
                if (property.Value == null || property.Value.Type == JTokenType.Null)
                {
                    continue;
                }

                var metaPackageId = property.Name;
                
                var metaPackageArray = property.Value as JArray;

                if (metaPackageArray == null)
                {
                    throw new JsonSerializationException($"Expected array for property {metaPackageId}");
                }

                foreach(var package in metaPackageArray)
                {
                    result.AddMetaPackageMapping(package.ToString(), metaPackageId);
                }
            }

            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else if (value is MetaPackages)
            {
                writer.WriteStartObject();
                foreach(var metaPackage in ((MetaPackages)value).GetMetaPackageGrouping())
                {
                    writer.WritePropertyName(metaPackage.Key);
                    writer.WriteStartArray();
                    foreach(var package in metaPackage)
                    {
                        writer.WriteValue(package);
                    }
                    writer.WriteEndArray();
                }

                writer.WriteEndObject();
            }
            else
            {
                throw new JsonSerializationException($"Expected {nameof(MetaPackages)} but got {value.GetType()}");
            }
        }
    }

}
