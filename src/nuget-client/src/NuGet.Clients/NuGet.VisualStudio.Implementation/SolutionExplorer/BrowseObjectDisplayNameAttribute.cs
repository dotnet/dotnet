// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.ComponentModel;
using System.Globalization;
using NuGet.VisualStudio.Implementation.Resources;

namespace NuGet.VisualStudio.SolutionExplorer
{
    /// <summary>
    /// Specifies the localized display name for a property, event or public void method which takes no arguments.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    internal sealed class BrowseObjectDisplayNameAttribute : DisplayNameAttribute
    {
        private readonly string _key;

        public BrowseObjectDisplayNameAttribute(string key) => _key = key;

        public override string DisplayName
        {
            get
            {
                // Defer lookup and cache in base class's DisplayNameValue field
                string name = base.DisplayName;

                if (name.Length == 0)
                {
                    name = DisplayNameValue = VsResources.ResourceManager.GetString(_key, CultureInfo.CurrentUICulture);
                }

                return name;
            }
        }
    }
}
