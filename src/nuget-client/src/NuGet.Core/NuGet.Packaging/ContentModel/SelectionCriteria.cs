// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace NuGet.ContentModel
{
    public class SelectionCriteria
    {
        public SelectionCriteria()
        {
            Entries = new List<SelectionCriteriaEntry>();
        }

        public IList<SelectionCriteriaEntry> Entries { get; set; }
    }

    public class SelectionCriteriaEntry
    {
        public SelectionCriteriaEntry()
        {
            Properties = new Dictionary<string, object>();
        }

        public IDictionary<string, object> Properties { get; set; }
    }
}
