// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace Microsoft.IdentityModel.TestUtils
{
    public class CompareContext
    {
        public static CompareContext Default = new CompareContext();

        public CompareContext()
        {
        }

        public CompareContext(string title)
        {
            Title = title;
        }

        public CompareContext(TheoryDataBase theoryData)
        {
            PropertiesToIgnoreWhenComparing = new Dictionary<Type, List<string>>(theoryData.PropertiesToIgnoreWhenComparing);
            Title = theoryData.TestId;
        }

        public CompareContext(string testName, TheoryDataBase theoryData)
        {
            Title = testName;
            PropertiesToIgnoreWhenComparing = new Dictionary<Type, List<string>>(theoryData.PropertiesToIgnoreWhenComparing);
        }

        public CompareContext(CompareContext other)
        {
            if (other == null)
                return;

            ClaimTypesToIgnoreWhenComparing = other.ClaimTypesToIgnoreWhenComparing;
            DictionaryKeysToIgnoreWhenComparing = other.DictionaryKeysToIgnoreWhenComparing;
            ExpectRawData = other.ExpectRawData;
            IgnoreClaimsIdentityType = other.IgnoreClaimsIdentityType;
            IgnoreClaimsPrincipalType = other.IgnoreClaimsPrincipalType;
            IgnoreClaimType = other.IgnoreClaimType;
            IgnoreProperties = other.IgnoreProperties;
            IgnoreSubject = other.IgnoreSubject;
            IgnoreType = other.IgnoreType;
            PropertiesToIgnoreWhenComparing = other.PropertiesToIgnoreWhenComparing;
            StringComparison = other.StringComparison;
            Title = other.Title;
        }

        public void AddDiff(string diff)
        {
            Diffs.Add(diff);
        }

        public void AddClaimTypesToIgnoreWhenComparing(params string[] claimTypes)
        {
            foreach (string claimType in claimTypes)
                ClaimTypesToIgnoreWhenComparing.Add(claimType);
        }

        public void AddDictionaryKeysToIgnoreWhenComparing(params string[] keyValues)
        {
            foreach(string keyValue in keyValues)
                DictionaryKeysToIgnoreWhenComparing.Add(keyValue);
        }

        public ISet<string> ClaimTypesToIgnoreWhenComparing { get; set; } = new HashSet<string>();

        public ISet<string> DictionaryKeysToIgnoreWhenComparing { get; set; } = new HashSet<string>();

        public List<string> Diffs { get; set; } = new List<string>();

        public bool ExpectRawData { get; set; }

        public Dictionary<Type, List<string>> PropertiesToIgnoreWhenComparing { get; set; } = new Dictionary<Type, List<string>>();

        /// <summary>
        /// Adds diffs and returns if any diffs were added.
        /// </summary>
        /// <param name="context"></param>
        /// <returns>true if any diffs were added.</returns>
        public bool Merge(CompareContext context)
        {
            return Merge(null, context);
        }

        public bool Merge(string title, CompareContext context)
        {
            if (context == null)
                return false;

            if (context.Diffs.Count > 0)
            {
                if (!string.IsNullOrEmpty(title))
                    Diffs.Add(title);

                Diffs.AddRange(context.Diffs);
            }

            return (context.Diffs.Count == 0);
        }

        public bool IgnoreClaimsIdentityType { get; set; }

        public bool IgnoreClaimsPrincipalType { get; set; }

        public bool IgnoreClaimType { get; set; }

        public bool IgnoreProperties { get; set; }

        public bool IgnoreSubject { get; set; } = true;

        public bool IgnoreType { get; set; } = true;

        public StringComparison StringComparison { get; set; } = System.StringComparison.Ordinal;

        public string Title { get; set; }
    }
}
