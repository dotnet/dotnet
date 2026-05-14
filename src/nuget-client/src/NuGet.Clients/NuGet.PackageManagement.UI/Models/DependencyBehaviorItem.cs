// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Resolver;

namespace NuGet.PackageManagement.UI
{
    // Represents an item in the DependencyBehavior combobox.
    public class DependencyBehaviorItem
    {
        public string Text { get; }

        public DependencyBehavior Behavior { get; private set; }

        public DependencyBehaviorItem(string text, DependencyBehavior dependencyBehavior)
        {
            Text = text;
            Behavior = dependencyBehavior;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
