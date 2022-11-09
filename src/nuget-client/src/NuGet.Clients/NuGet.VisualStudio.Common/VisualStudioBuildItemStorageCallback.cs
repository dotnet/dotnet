// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Shell.Interop;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// <see cref="VisualStudioBuildItemStorageCallback" it is used together with <see cref="IVsBuildItemStorage"/> to find a project's items and read their metadata.
    /// </summary>
    public class VisualStudioBuildItemStorageCallback : IVsBuildItemStorageCallback
    {
        /// <summary>
        /// The list of Ids and requested metadata.
        /// For a project item like
        /// <![CDATA[<PackageVersion Include="Foo" Version="1.1.1"/>]]>
        /// If the requested metadata is "Version" the returned Items will be (Foo, {"1.1.1"})
        /// </summary>
        public List<(string Itemid, string[] ItemMetadata)> Items { get; } = new List<(string Itemid, string[] ItemMetadata)>();

        void IVsBuildItemStorageCallback.ItemFound(string itemSpec, Array metadata)
        {
            var currentItemMetadata = new string[metadata.Length];
            int index = 0;

            foreach (var a in metadata)
            {
                currentItemMetadata[index++] = (string)a;
            }

            Items.Add((itemSpec, currentItemMetadata));
        }
    }
}
