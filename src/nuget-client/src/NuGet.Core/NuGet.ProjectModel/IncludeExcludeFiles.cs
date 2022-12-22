// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using NuGet.Shared;

namespace NuGet.ProjectModel
{
    public class IncludeExcludeFiles : IEquatable<IncludeExcludeFiles>
    {
        public IReadOnlyList<string> Include { get; set; }
        public IReadOnlyList<string> Exclude { get; set; }
        public IReadOnlyList<string> IncludeFiles { get; set; }
        public IReadOnlyList<string> ExcludeFiles { get; set; }

        public bool HandleIncludeExcludeFiles(JObject jsonObject)
        {
            if (jsonObject == null)
            {
                throw new ArgumentNullException(nameof(jsonObject));
            }

            JToken rawInclude = jsonObject["include"];
            JToken rawExclude = jsonObject["exclude"];
            JToken rawIncludeFiles = jsonObject["includeFiles"];
            JToken rawExcludeFiles = jsonObject["excludeFiles"];

            var foundOne = false;

            if (rawInclude != null && TryGetStringEnumerableFromJArray(rawInclude, out IReadOnlyList<string> include))
            {
                Include = include;
                foundOne = true;
            }

            if (rawExclude != null && TryGetStringEnumerableFromJArray(rawExclude, out IReadOnlyList<string> exclude))
            {
                Exclude = exclude;
                foundOne = true;
            }

            if (rawIncludeFiles != null && TryGetStringEnumerableFromJArray(rawIncludeFiles, out IReadOnlyList<string> includeFiles))
            {
                IncludeFiles = includeFiles;
                foundOne = true;
            }

            if (rawExcludeFiles != null && TryGetStringEnumerableFromJArray(rawExcludeFiles, out IReadOnlyList<string> excludeFiles))
            {
                ExcludeFiles = excludeFiles;
                foundOne = true;
            }

            return foundOne;
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCodeCombiner();

            hashCode.AddSequence(Include);
            hashCode.AddSequence(Exclude);
            hashCode.AddSequence(IncludeFiles);
            hashCode.AddSequence(ExcludeFiles);

            return hashCode.CombinedHash;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as IncludeExcludeFiles);
        }

        public bool Equals(IncludeExcludeFiles other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Include.SequenceEqualWithNullCheck(other.Include) &&
                Exclude.SequenceEqualWithNullCheck(other.Exclude) &&
                IncludeFiles.SequenceEqualWithNullCheck(other.IncludeFiles) &&
                ExcludeFiles.SequenceEqualWithNullCheck(other.ExcludeFiles);
        }

        public IncludeExcludeFiles Clone()
        {
            var clonedObject = new IncludeExcludeFiles();
            clonedObject.Include = Include.ToList();
            clonedObject.Exclude = Exclude.ToList();
            clonedObject.IncludeFiles = IncludeFiles.ToList();
            clonedObject.ExcludeFiles = ExcludeFiles.ToList();
            return clonedObject;
        }

        private static bool TryGetStringEnumerableFromJArray(JToken token, out IReadOnlyList<string> result)
        {
            result = null;

            if (token == null)
            {
                return false;
            }
            else if (token.Type == JTokenType.String)
            {
                result = new[]
                {
                    token.Value<string>()
                };
            }
            else if (token.Type == JTokenType.Array)
            {
                result = token.ValueAsArray<string>();
            }
            else
            {
                return false;
            }

            return true;
        }
    }
}
