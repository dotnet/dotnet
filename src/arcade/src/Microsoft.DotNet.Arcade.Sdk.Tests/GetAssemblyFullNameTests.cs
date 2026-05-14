// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Utilities;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Arcade.Test.Common;
using Xunit;

namespace Microsoft.DotNet.Arcade.Sdk.Tests
{
    public class GetAssemblyFullNameTests
    {
        [Fact]
        public void PathInMetadata()
        {
            var objectAssembly = typeof(object).Assembly;
            var thisAssembly = typeof(GetAssemblyFullNameTests).Assembly;

            var task = new GetAssemblyFullName()
            {
                Items = new TaskItem[]
                {
                    new TaskItem("Item", new Dictionary<string, string> { { "SomePath", objectAssembly.Location } }),
                    new TaskItem("Item", new Dictionary<string, string> { { "SomePath", thisAssembly.Location } }),
                },
                PathMetadata = "SomePath",
                FullNameMetadata = "SomeFullName"
            };

            bool result = task.Execute();

            AssertEx.Equal(new[]
           {
                objectAssembly.FullName,
                thisAssembly.FullName
            }, task.ItemsWithFullName.Select(i => i.GetMetadata("SomeFullName")));

            Assert.True(result);
        }

        [Fact]
        public void PathInItemSpec()
        {
            var objectAssembly = typeof(object).Assembly;
            var thisAssembly = typeof(GetAssemblyFullNameTests).Assembly;

            var task = new GetAssemblyFullName()
            {
                Items = new TaskItem[]
                {
                    new TaskItem(objectAssembly.Location),
                    new TaskItem(thisAssembly.Location),
                },
                FullNameMetadata = "SomeFullName"
            };

            bool result = task.Execute();

            AssertEx.Equal(new[]
            {
                objectAssembly.FullName,
                thisAssembly.FullName
            }, task.ItemsWithFullName.Select(i => i.GetMetadata("SomeFullName")));

            Assert.True(result);
        }
    }
}
