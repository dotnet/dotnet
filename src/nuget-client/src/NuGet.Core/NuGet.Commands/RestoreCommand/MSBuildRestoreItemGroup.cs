// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace NuGet.Commands
{
    public class MSBuildRestoreItemGroup
    {
        public static readonly string ItemGroup = nameof(ItemGroup);
        public static readonly string ImportGroup = nameof(ImportGroup);

        /// <summary>
        /// Optional position arguement used when ordering groups in the output file.
        /// </summary>
        public int Position { get; set; } = 1;

        /// <summary>
        /// Conditions applied to the item group. These will be AND'd together.
        /// </summary>
        public List<string> Conditions { get; set; } = new List<string>();

        /// <summary>
        /// Items or imports.
        /// </summary>
        public List<XElement> Items { get; set; } = new List<XElement>();

        /// <summary>
        /// Root element name.
        /// </summary>
        public string RootName { get; set; } = ImportGroup;

        /// <summary>
        /// Combined conditions
        /// </summary>
        public string Condition
        {
            get
            {
                if (Conditions.Count > 0)
                {
                    return " " + string.Join(" AND ", Conditions.Select(s => s.Trim())) + " ";
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        internal static MSBuildRestoreItemGroup Create(
            string rootName,
            int position)
        {
            var group = new MSBuildRestoreItemGroup();

            group.RootName = rootName;
            group.Position = position;

            return group;
        }

        public static MSBuildRestoreItemGroup Create(
            string rootName,
            IEnumerable<XElement> items,
            int position,
            IEnumerable<string> conditions)
        {
            var group = new MSBuildRestoreItemGroup();

            group.RootName = rootName;
            group.Items.AddRange(items);
            group.Position = position;
            group.Conditions.AddRange(conditions);

            return group;
        }
    }
}
