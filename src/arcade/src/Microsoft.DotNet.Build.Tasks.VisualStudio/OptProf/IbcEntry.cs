// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Microsoft.DotNet.Build.Tasks.VisualStudio
{
    internal struct IbcEntry
    {
        private const string TechnologyName = "IBC";
        private const string VSInstallationRootVar = "%VisualStudio.InstallationUnderTest.Path%";
        private const string DefaultNgenApplication = VSInstallationRootVar + "\\Common7\\IDE\\vsn.exe";

        public readonly string EntryName;
        public readonly string RelativeInstallationPath;
        public readonly string InstrumentationArguments;
        public readonly string RelativeDirectoryPath;

        public IbcEntry(string entryName, string relativeInstallationPath, string relativeDirectoryPath, string ngenApplicationPath)
        {
            string commandLineArg(string name, string value) => $"/{name}:\"{value}\"";

            EntryName = entryName;
            RelativeInstallationPath = relativeInstallationPath;
            RelativeDirectoryPath = relativeDirectoryPath;
            InstrumentationArguments = commandLineArg("ExeConfig", ngenApplicationPath);
        }

        public JObject ToJson()
            => new JObject(
                new JProperty("Technology", TechnologyName),
                new JProperty("RelativeInstallationPath", RelativeInstallationPath),
                new JProperty("InstrumentationArguments", InstrumentationArguments));


        public static IEnumerable<IbcEntry> GetEntriesFromAssembly(AssemblyOptProfTraining assembly)
        {
            foreach (var args in assembly.InstrumentationArguments)
            {
                yield return new IbcEntry(
                    assembly.Assembly,
                    relativeInstallationPath: args.RelativeInstallationFolder.Replace("/", "\\") + $"\\{assembly.Assembly}",
                    relativeDirectoryPath: "",
                    ngenApplicationPath: Path.Combine(VSInstallationRootVar, args.InstrumentationExecutable.Replace("/", "\\")));
            }
        }

        public static IEnumerable<IbcEntry> GetEntriesFromVsixJsonManifest(JObject json)
        {
            bool isNgened(JToken file)
                => file["ngen"] != null || file["ngenPriority"] != null || file["ngenArchitecture"] != null || file["ngenApplication"] != null;

            bool isPEFile(string filePath)
            {
                string ext = Path.GetExtension(filePath);
                return StringComparer.OrdinalIgnoreCase.Equals(ext, ".dll") ||
                       StringComparer.OrdinalIgnoreCase.Equals(ext, ".exe");
            }

            bool isSatelliteAssembly(string filePath)
                => Path.GetFileNameWithoutExtension(filePath).EndsWith(".resources");

            bool includeInIbcTraining(JToken file, string fileName)
                => isNgened(file) && isPEFile(fileName) && !isSatelliteAssembly(fileName);

            string replacePrefix(string path, string prefix, string replacement)
                => path.StartsWith(prefix, StringComparison.OrdinalIgnoreCase) ? replacement + path.Substring(prefix.Length) : path;

            if (json["extensionDir"] != null)
            {
                var extensionDir = replacePrefix((string)json["extensionDir"], "[installdir]\\", "");
                return from file in (JArray)json["files"]
                       let fileName = (string)file["fileName"]
                       where includeInIbcTraining(file, fileName)
                       let filePath = $"{extensionDir}\\{fileName.TrimStart('/').Replace("/", "\\")}"
                       select new IbcEntry(fileName, filePath, relativeDirectoryPath: Path.GetDirectoryName(fileName), DefaultNgenApplication);
            }
            else
            {
                return from file in (JArray)json["files"]
                       let fileName = (string)file["fileName"]
                       let ngenApplication = (string)file["ngenApplication"]
                       where includeInIbcTraining(file, fileName)
                       let filePath = fileName.Replace("/Contents/", string.Empty).Replace("/", "\\")
                       select new IbcEntry(
                           fileName,
                           filePath,
                           relativeDirectoryPath: "",
                           (ngenApplication != null) ? replacePrefix(ngenApplication, "[installdir]", VSInstallationRootVar) : DefaultNgenApplication);
            }
        }
    }
}
