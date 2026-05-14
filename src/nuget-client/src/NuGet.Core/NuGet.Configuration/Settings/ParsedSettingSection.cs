// Copyright(c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace NuGet.Configuration
{
    internal sealed class ParsedSettingSection : SettingSection
    {
        internal ParsedSettingSection(string name, XElement element, SettingsFile origin)
            : base(name, element, origin)
        {
        }

        internal ParsedSettingSection(string name, params SettingItem[] children)
            : base(name, attributes: null, children: new HashSet<SettingItem>(children))
        {
            foreach (var child in Children)
            {
                child.Parent = this;
            }
        }

        public override SettingBase Clone()
        {
            return new VirtualSettingSection(ElementName, Attributes, Items.Select(s => (SettingItem)s.Clone()));
        }
    }
}
