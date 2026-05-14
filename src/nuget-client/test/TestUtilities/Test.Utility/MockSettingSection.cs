// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Configuration;

namespace Test.Utility
{
    public class MockSettingSection : SettingSection
    {
        public MockSettingSection(string name, IReadOnlyDictionary<string, string> attributes, IEnumerable<SettingItem> children)
            : base(name, attributes, children)
        {
        }

        public MockSettingSection(string name, params SettingItem[] children)
            : base(name, attributes: null, children: new HashSet<SettingItem>(children))
        {
        }

        public override SettingBase Clone()
        {
            throw new NotImplementedException();
        }
    }
}
