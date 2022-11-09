// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using NuGet.Commands;
using NuGet.Test.Utility;

namespace NuGet.Build.Tasks.Console.Test
{
    internal class MockMSBuildProject : MSBuildItem, IMSBuildProject
    {
        public MockMSBuildProject(TestDirectory directory)
            : this(Path.Combine(directory, "ProjectA.csproj"))
        {
        }

        public MockMSBuildProject(TestDirectory directory, IDictionary<string, string> properties)
            : this(Path.Combine(directory, "ProjectA.csproj"), properties)
        {
        }

        public MockMSBuildProject(TestDirectory directory, IDictionary<string, string> properties, IDictionary<string, string> globalProperties)
            : this(Path.Combine(directory, "ProjectA.csproj"), properties, new Dictionary<string, IList<IMSBuildItem>>(), globalProperties)
        {
        }

        public MockMSBuildProject(string fullPath)
            : this(fullPath, properties: null)
        {
        }

        public MockMSBuildProject(IDictionary<string, string> properties)
            : this("ProjectA", properties)
        {
        }

        public MockMSBuildProject(IDictionary<string, string> properties, IDictionary<string, string> globalProperties)
            : this("ProjectA", properties, new Dictionary<string, IList<IMSBuildItem>>(), globalProperties)
        {
        }

        public MockMSBuildProject(string fullPath, IDictionary<string, string> properties)
            : this(fullPath, properties, items: null)
        {
        }

        public MockMSBuildProject(string fullPath, IDictionary<string, string> properties, IDictionary<string, IList<IMSBuildItem>> items)
            : this(fullPath, properties, items, new Dictionary<string, string>())
        {
        }

        public MockMSBuildProject(string fullPath, IDictionary<string, string> properties, IDictionary<string, IList<IMSBuildItem>> items, IDictionary<string, string> globalProperties)
            : base(Path.GetFileName(fullPath), properties ?? new Dictionary<string, string>())
        {
            Items = items ?? new Dictionary<string, IList<IMSBuildItem>>();

            Directory = Path.GetDirectoryName(fullPath);

            FullPath = fullPath;

            GlobalProperties = globalProperties;
        }

        public string Directory { get; }

        public string FullPath { get; }

        public IDictionary<string, IList<IMSBuildItem>> Items { get; set; }

        public IDictionary<string, string> GlobalProperties { get; set; }

        public IEnumerable<IMSBuildItem> GetItems(string name)
        {
            return Items.TryGetValue(name, out var items) ? items : null;
        }

        public string GetGlobalProperty(string property)
        {
            GlobalProperties.TryGetValue(property, out string value);
            return value;
        }
    }
}
