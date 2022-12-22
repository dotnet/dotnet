// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using MessagePack;
using MessagePack.Formatters;
using Microsoft;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;

namespace NuGet.VisualStudio.Internal.Contracts
{
    internal sealed class PackageDependencyGroupFormatter : NuGetMessagePackFormatter<PackageDependencyGroup>
    {
        private const string TargetFrameworkPropertyName = "targetframework";
        private const string PackagesPropertyName = "packages";

        internal static readonly IMessagePackFormatter<PackageDependencyGroup?> Instance = new PackageDependencyGroupFormatter();

        private PackageDependencyGroupFormatter()
        {
        }

        protected override PackageDependencyGroup? DeserializeCore(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            NuGetFramework? framework = null;
            IEnumerable<PackageDependency>? packageDependencies = null;

            int propertyCount = reader.ReadMapHeader();

            for (var propertyIndex = 0; propertyIndex < propertyCount; ++propertyIndex)
            {
                switch (reader.ReadString())
                {
                    case TargetFrameworkPropertyName:
                        framework = NuGetFrameworkFormatter.Instance.Deserialize(ref reader, options);
                        break;
                    case PackagesPropertyName:
                        packageDependencies = options.Resolver.GetFormatter<IEnumerable<PackageDependency>>().Deserialize(ref reader, options);
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            Assumes.NotNull(framework);
            Assumes.NotNull(packageDependencies);

            return new PackageDependencyGroup(framework, packageDependencies);
        }

        protected override void SerializeCore(ref MessagePackWriter writer, PackageDependencyGroup value, MessagePackSerializerOptions options)
        {
            writer.WriteMapHeader(count: 2);
            writer.Write(TargetFrameworkPropertyName);
            NuGetFrameworkFormatter.Instance.Serialize(ref writer, value.TargetFramework, options);
            writer.Write(PackagesPropertyName);
            options.Resolver.GetFormatter<IEnumerable<PackageDependency>>().Serialize(ref writer, value.Packages, options);
        }
    }
}
