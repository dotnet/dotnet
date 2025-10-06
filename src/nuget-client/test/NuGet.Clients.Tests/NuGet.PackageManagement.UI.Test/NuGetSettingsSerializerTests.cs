// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Xunit;

namespace NuGet.PackageManagement.UI.Test
{
    public class NuGetSettingsSerializerTests
    {
        [Fact]
        public void NuGetSettings_WhenSerializing_FieldCountIsExpected()
        {
            IReadOnlyList<FieldInfo> fields = GetSerializableFields<NuGetSettings>();

            Assert.Equal(0, fields.Count);
        }

        [Fact]
        public void NuGetSettings_WhenSerializing_PropertyCountIsExpected()
        {
            IReadOnlyList<PropertyInfo> properties = GetSerializableProperties<NuGetSettings>();

            Assert.Equal(1, properties.Count);
        }

        [Fact]
        public void UserSettings_WhenSerializing_FieldCountIsExpected()
        {
            IReadOnlyList<FieldInfo> fields = GetSerializableFields<UserSettings>();

            Assert.Equal(0, fields.Count);
        }

        [Fact]
        public void UserSettings_WhenSerializing_PropertyCountIsExpected()
        {
            IReadOnlyList<PropertyInfo> properties = GetSerializableProperties<UserSettings>();

            Assert.Equal(13, properties.Count);
        }

        [Fact]
        public void Serialization_WhenDeserializing_Succeeds()
        {
            NuGetSettings expectedSettings = CreateNuGetSettings();

            var serializer = new NuGetSettingsSerializer();

            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, expectedSettings);

                Assert.NotEqual(0, stream.Length);

                stream.Seek(offset: 0, loc: SeekOrigin.Begin);

                NuGetSettings actualSettings = serializer.Deserialize(stream);

                Assert.NotSame(expectedSettings, actualSettings);
                AssertAreEquivalent(expectedSettings, actualSettings);
            }
        }

        private static void AssertAreEquivalent(NuGetSettings expectedSettings, NuGetSettings actualSettings)
        {
            Assert.Equal(expectedSettings.WindowSettings.Count, actualSettings.WindowSettings.Count);

            IReadOnlyList<PropertyInfo> properties = GetSerializableProperties<UserSettings>();

            foreach (string key in expectedSettings.WindowSettings.Keys)
            {
                Assert.True(actualSettings.WindowSettings.ContainsKey(key));

                UserSettings expectedUserSettings = expectedSettings.WindowSettings[key];
                UserSettings actualUserSettings = actualSettings.WindowSettings[key];

                foreach (PropertyInfo property in properties)
                {
                    object expectedValue = property.GetValue(expectedUserSettings);
                    object actualValue = property.GetValue(actualUserSettings);

                    Assert.Equal(expectedValue, actualValue);
                }
            }
        }

        private static NuGetSettings CreateNuGetSettings()
        {
            var settings = new NuGetSettings();

            var userSettings = new UserSettings()
            {
                SourceRepository = "a",
                ShowPreviewWindow = true,
                ShowDeprecatedFrameworkWindow = false,
                RemoveDependencies = true,
                ForceRemove = false,
                IncludePrerelease = true,
                SelectedFilter = ItemFilter.Installed,
                DependencyBehavior = Resolver.DependencyBehavior.HighestMinor,
                FileConflictAction = ProjectManagement.FileConflictAction.Overwrite,
                OptionsExpanded = true,
                SortPropertyName = "b",
                SortDirection = System.ComponentModel.ListSortDirection.Descending
            };

            settings.WindowSettings.Add("c", userSettings);

            return settings;
        }

        private static IReadOnlyList<FieldInfo> GetSerializableFields<T>()
        {
            return typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance)
                .Where(field => !field.GetCustomAttributes<JsonIgnoreAttribute>().Any())
                .ToArray();
        }

        private static IReadOnlyList<PropertyInfo> GetSerializableProperties<T>()
        {
            return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(property => !property.GetCustomAttributes<JsonIgnoreAttribute>().Any())
                .ToArray();
        }
    }
}
