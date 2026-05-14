// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Xunit;
using Microsoft.Build.Utilities;
using System.Collections.Generic;
using Microsoft.Arcade.Test.Common;
using System.Linq;

namespace Microsoft.DotNet.Arcade.Sdk.Tests
{
    public class GroupItemsByTests
    {
        [Fact]
        public void GroupItemsBy()
        {
            var task = new GroupItemsBy()
            {
                Items = new TaskItem[] 
                {
                    new TaskItem("A", new Dictionary<string, string> { { "Y", "A1.Y" }, { "Z", "A1.Z" }, { "W", "A1.W" } }),
                    new TaskItem("B", new Dictionary<string, string> { { "Z", "B1.Z" } }),
                    new TaskItem("A", new Dictionary<string, string> { { "X", "A2.X" }, { "Z", "A2.Z" }, { "W", "A2.W" } }),
                    new TaskItem("C", new Dictionary<string, string> { { "X", "C1.X" }, { "Z", "C1.Z" } }),
                    new TaskItem("C", new Dictionary<string, string> { { "Y", "C2.Y" }, { "Z", "C2.Z" } }),
                },
                GroupMetadata = new[] { "X", "Y", "Z", "U" }
            };

            bool result = task.Execute();
            var inspectMetadata = new[] { "X", "Y", "Z", "U", "W" };

            AssertEx.Equal(new[] 
            {
                "A: X='A2.X' Y='A1.Y' Z='A1.Z;A2.Z' U='' W='A1.W'",
                "B: X='' Y='' Z='B1.Z' U='' W=''",
                "C: X='C1.X' Y='C2.Y' Z='C1.Z;C2.Z' U='' W=''",
            }, task.GroupedItems.Select(i => $"{i.ItemSpec}: {string.Join(" ", inspectMetadata.Select(m => $"{m}='{i.GetMetadata(m)}'"))}"));
            
            Assert.True(result);
        }
    }
}
