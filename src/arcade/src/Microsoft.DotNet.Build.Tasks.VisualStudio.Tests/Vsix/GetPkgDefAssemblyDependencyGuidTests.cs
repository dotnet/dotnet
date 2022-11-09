// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Arcade.Test.Common;
using Microsoft.Build.Utilities;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Microsoft.DotNet.Build.Tasks.VisualStudio.UnitTests
{
    public class GetPkgDefAssemblyDependencyGuidTests
    {
        [Fact]
        public void InputInMetadata()
        {
            var task = new GetPkgDefAssemblyDependencyGuid()
            {
                Items = new TaskItem[]
                {
                    new TaskItem("Item", new Dictionary<string, string> { { "SomeInput", "SomeValue" } }),
                    new TaskItem("Item", new Dictionary<string, string> { { "SomeInput", "\U00012345" } }),
                    new TaskItem("Item", new Dictionary<string, string> { { "SomeInput", "\uD800" } }), // unpaired surrogate treated as invalid character
                    new TaskItem("Item", new Dictionary<string, string> { { "SomeInput", "\uD801" } }), // unpaired surrogate treated as invalid character
                    new TaskItem("Item", new Dictionary<string, string> { { "SomeInput", "" } }), // empty is skipped
                },
                InputMetadata = "SomeInput",
                OutputMetadata = "SomeOutput"
            };

            bool result = task.Execute();

            AssertEx.Equal(new[]
            {
                "{9E8E5D98-C082-B764-01E5-9ECA6FB4364E}",
                "{ECDA244C-DF2C-D4A2-4AD3-6E9106192060}",
                "{C178F940-C17A-1FA7-F265-D0B78A9C9915}",
                "{C178F940-C17A-1FA7-F265-D0B78A9C9915}",
                "",
            }, task.OutputItems.Select(i => i.GetMetadata("SomeOutput")));

            Assert.True(result);
        }

        [Fact]
        public void InputInItemSpec()
        {
            var task = new GetPkgDefAssemblyDependencyGuid()
            {
                Items = new TaskItem[]
                {
                    new TaskItem("SomeValue"),
                },
                OutputMetadata = "SomeOutput"
            };

            bool result = task.Execute();

            AssertEx.Equal(new[]
            {
                "{9E8E5D98-C082-B764-01E5-9ECA6FB4364E}",
            }, task.OutputItems.Select(i => i.GetMetadata("SomeOutput")));

            Assert.True(result);
        }
    }
}
