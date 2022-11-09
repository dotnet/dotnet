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
    /// Specifies a localized description for a property or event.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    internal sealed class BrowseObjectDescriptionAttribute : DescriptionAttribute
    {
        private readonly string _key;

        public BrowseObjectDescriptionAttribute(string key) => _key = key;

        public override string Description
        {
            get
            {
                // Defer lookup and cache in base class's DescriptionValue field
                string name = base.Description;

                if (name.Length == 0)
                {
                    name = DescriptionValue = VsResources.ResourceManager.GetString(_key, CultureInfo.CurrentUICulture);
                }

                return name;
            }
        }
    }
}
