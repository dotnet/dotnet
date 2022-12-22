// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Microsoft.DotNet.Arcade.Sdk
{
    /// <summary>
    /// Groups items by ItemSpec.
    /// 
    /// Given the following items:
    /// <![CDATA[
    /// <ItemGroup>
    ///   <Stuff Include="A" Value="X"/>
    ///   <Stuff Include="A" Value="Y"/>
    ///   <Stuff Include="B" Value="Z"/>
    /// </ItemGroup>
    /// ]]>
    /// 
    /// produces
    /// 
    /// <![CDATA[
    /// <ItemGroup>
    ///   <Stuff Include="A" Value="X;Y"/>
    ///   <Stuff Include="B" Value="Z"/>
    /// </ItemGroup>
    /// ]]>
    /// 
    /// </summary>
    public sealed class GroupItemsBy : Microsoft.Build.Utilities.Task
    {
        /// <summary>
        /// Items to group by their ItemSpec.
        /// </summary>
        [Required]
        public ITaskItem[] Items { get; set; }

        /// <summary>
        /// Names of custom metadata to group (e.g. "Value" in the example above).
        /// When merging two items the values of metadata in this set are merged into a list, 
        /// while the first value is used for metadata not in this set.
        /// </summary>
        [Required]
        public string[] GroupMetadata { get; set; }

        /// <summary>
        /// Items with grouped metadata values.
        /// </summary>
        [Output]
        public ITaskItem[] GroupedItems { get; set; }

        public override bool Execute()
        {
            ITaskItem mergeItems(IEnumerable<ITaskItem> items)
            {
                var result = items.First();

                foreach (var item in items.Skip(1))
                {
                    foreach (string metadataName in GroupMetadata)
                    {
                        var left = result.GetMetadata(metadataName);
                        var right = item.GetMetadata(metadataName);

                        result.SetMetadata(metadataName, 
                            (string.IsNullOrEmpty(left) || left == right) ? right : string.IsNullOrEmpty(right) ? left : left + ";" + right);
                    }
                }

                return result;
            }

            GroupedItems = (from item in Items
                            group item by item.ItemSpec
                            into itemsWithEqualKey
                            select mergeItems(itemsWithEqualKey)).ToArray();

            return true;
        }
    }
}

