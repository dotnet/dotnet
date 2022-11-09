// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using FluentAssertions;
using NuGet.Commands;
using Xunit;

namespace NuGet.Build.Tasks.Console.Test
{
    public class ProjectItemInstanceEvaluatedIncludeComparerTests
    {
        [Fact]
        public void ProjectItemInstanceEvaluatedIncludeComparer_Equals_DeDupesWhenDifferentCasing()
        {
            ProjectItemInstanceEvaluatedIncludeComparer.Instance
                .Equals(
                    new MSBuildItem("one", new Dictionary<string, string>()),
                    new MSBuildItem("oNE", new Dictionary<string, string>()))
                .Should()
                .BeTrue();
        }

        [Fact]
        public void ProjectItemInstanceEvaluatedIncludeComparer_GetHashCode_DeDupesWhenDifferentCasing()
        {
            Action act = () =>
            {
                var items = new HashSet<IMSBuildItem>(ProjectItemInstanceEvaluatedIncludeComparer.Instance);

                items.Add(new MSBuildItem("one", new Dictionary<string, string>()));
                items.Add(new MSBuildItem("oNe", new Dictionary<string, string>()));
                items.Add(new MSBuildItem("ONE", new Dictionary<string, string>()));

                items.Count.Should().Be(1);
            };

            act.Should().NotThrow();
        }
    }
}
