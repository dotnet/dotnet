//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

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

        public CompareContext(string testName, TheoryDataBase theoryData)
        {
            Title = testName;
            PropertiesToIgnoreWhenComparing = theoryData.PropertiesToIgnoreWhenComparing;
        }

        public CompareContext(CompareContext other)
        {
            if (other == null)
                return;

            ClaimTypesToIgnoreWhenComparing = other.ClaimTypesToIgnoreWhenComparing;
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

        public void AddClaimTypeToIgnoreWhenComparing(string claimType)
        {
            ClaimTypesToIgnoreWhenComparing.Add(claimType);
        }

        public ISet<string> ClaimTypesToIgnoreWhenComparing { get; set; } = new HashSet<string>();

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
