// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.Serialization;

internal static partial class ObjectReaders
{
    private record struct ConfigurationData(string ConfigurationName, RazorLanguageVersion LanguageVersion, RazorExtension[] Extensions)
    {
        public static readonly PropertyMap<ConfigurationData> PropertyMap = new(
            (nameof(ConfigurationName), ReadConfigurationName),
            (nameof(LanguageVersion), ReadLanguageVersion),
            (nameof(Extensions), ReadExtensions));

        private static void ReadConfigurationName(JsonDataReader reader, ref ConfigurationData data)
            => data.ConfigurationName = reader.ReadNonNullString();

        private static void ReadLanguageVersion(JsonDataReader reader, ref ConfigurationData data)
        {
            string languageVersionValue;

            if (reader.IsObjectStart)
            {
                LanguageVersionData versionData = default;
                reader.ReadObjectData(ref versionData, LanguageVersionData.PropertyMap);
                languageVersionValue = $"{versionData.Major}.{versionData.Minor}";
            }
            else
            {
                languageVersionValue = reader.ReadNonNullString();
            }

            data.LanguageVersion = RazorLanguageVersion.TryParse(languageVersionValue, out var languageVersion)
                ? languageVersion
                : RazorLanguageVersion.Version_2_1;
        }

        private static void ReadExtensions(JsonDataReader reader, ref ConfigurationData data)
            => data.Extensions = reader.ReadArray(static r => r.ReadNonNullObject(ReadExtensionFromProperties))
                ?? Array.Empty<RazorExtension>();
    }
}
