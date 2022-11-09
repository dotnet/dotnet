// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.PackageManagement.UI
{
    // The item added to the filter combobox on the UI
    public class FilterItem
    {
        public FilterItem(ItemFilter filter, string text)
        {
            Filter = filter;
            Text = text;
        }

        public ItemFilter Filter { get; private set; }

        // The text that is displayed on UI
        public string Text { get; }

        public override string ToString()
        {
            return Text;
        }
    }
}
