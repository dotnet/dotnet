// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace NuGet.ContentModel
{
    /// <summary>
    /// Defines a property that can be used in Content Model query patterns
    /// <seealso cref="PatternSet" />
    /// </summary>
    public class ContentPropertyDefinition
    {
        private static readonly Func<object, object, bool> EqualsTest = (left, right) => Equals(left, right);

        internal ContentPropertyDefinition(
            string name,
            Func<ReadOnlyMemory<char>, PatternTable, bool, object> parser)
            : this(name, parser, null, null, null, false)
        {
        }

        internal ContentPropertyDefinition(
            string name,
            Func<ReadOnlyMemory<char>, PatternTable, bool, object> parser,
            Func<object, object, bool> compatibilityTest)
            : this(name, parser, compatibilityTest, null, null, false)
        {
        }

        internal ContentPropertyDefinition(string name,
            Func<ReadOnlyMemory<char>, PatternTable, bool, object> parser,
            Func<object, object, bool> compatibilityTest,
            Func<object, object, object, int> compareTest)
            : this(name, parser, compatibilityTest, compareTest, null, false)
        {
        }

        internal ContentPropertyDefinition(
            string name,
            Func<ReadOnlyMemory<char>, PatternTable, bool, object> parser,
            IEnumerable<string> fileExtensions)
            : this(name, parser, null, null, fileExtensions, false)
        {
        }

        internal ContentPropertyDefinition(
            string name,
            Func<ReadOnlyMemory<char>, PatternTable, bool, object> parser,
            Func<object, object, bool> compatibilityTest,
            Func<object, object, object, int> compareTest,
            IEnumerable<string> fileExtensions,
            bool allowSubfolders)
        {
            Name = name;
            Parser = parser;
            CompatibilityTest = compatibilityTest ?? EqualsTest;
            CompareTest = compareTest;
            FileExtensions = fileExtensions?.ToList();
            FileExtensionAllowSubFolders = allowSubfolders;
        }

        public string Name { get; }

        public List<string> FileExtensions { get; }

        public bool FileExtensionAllowSubFolders { get; }

        /// <summary>
        /// Parse a ReadOnlyMemory char if it's off the form of this definition.
        /// A null return value means the ReadOnlyMemory char does not match this definition.
        /// If the bool is true, the return object will be non-null, and match what the ReadOnlyMemory char represents.
        /// If the bool is false, the return object will be non-null if the ReadOnlyMemory char represents a valid value for this definition. This is a performance optimization.
        /// </summary>
        internal Func<ReadOnlyMemory<char>, PatternTable, bool, object> Parser { get; }

        /// <summary>
        /// Looks up a definition for the given substring.
        /// Example, say this definition is for an assembly. 
        /// If the name is "assembly.dll", this method would return true and the value would be the assembly name.
        /// If the name is "assembly.xml" this method would return flase and the value would be the null.
        /// If this is a match only lookup the value will be null, but the return bool will be true. This is a performance optimization since the value is unused.
        /// </summary>
        /// <param name="name">The name to lookup.</param>
        /// <param name="table">A replacement table. If name matches a value in the replacement table, it'll be returned instead. </param>
        /// <param name="matchOnly">Whether this is a grouping match, or we actually want to actualize the value of name as a string.</param>
        /// <param name="value">The out param. If matchonly, it will always be null. Otherwise, set to actualized value of name if the return is true, set to null if false.</param>
        /// <returns>True if the name matches the definition. False otherwise.</returns>
        internal virtual bool TryLookup(ReadOnlyMemory<char> name, PatternTable table, bool matchOnly, out object value)
        {
            if (name.IsEmpty)
            {
                value = null;
                return false;
            }

            if (FileExtensions?.Count > 0)
            {
                if (FileExtensionAllowSubFolders || !ContainsSlash(name))
                {
                    foreach (var fileExtension in FileExtensions)
                    {
                        if (name.Span.EndsWith(fileExtension.AsSpan(), StringComparison.OrdinalIgnoreCase))
                        {
                            if (!matchOnly)
                            {
                                value = name.ToString();
                            }
                            else
                            {
                                value = null;
                            }
                            return true;
                        }
                    }
                }
            }

            if (Parser != null)
            {
                value = Parser.Invoke(name, table, matchOnly);
                if (value != null)
                {
                    return true;
                }
            }

            value = null;
            return false;
        }

        private static bool ContainsSlash(ReadOnlyMemory<char> name)
        {
            var containsSlash = false;
            foreach (var ch in name.Span)
            {
                if (ch == '/' || ch == '\\')
                {
                    containsSlash = true;
                    break;
                }
            }

            return containsSlash;
        }

        public Func<object, object, bool> CompatibilityTest { get; }

        /// <summary>
        /// Find the nearest compatible candidate.
        /// </summary>
        public Func<object, object, object, int> CompareTest { get; }

        public virtual bool IsCriteriaSatisfied(object critieriaValue, object candidateValue)
        {
            return CompatibilityTest.Invoke(critieriaValue, candidateValue);
        }

        public virtual int Compare(object criteriaValue, object candidateValue1, object candidateValue2)
        {
            var betterCoverageFromValue1 = IsCriteriaSatisfied(candidateValue1, candidateValue2);
            var betterCoverageFromValue2 = IsCriteriaSatisfied(candidateValue2, candidateValue1);
            if (betterCoverageFromValue1 && !betterCoverageFromValue2)
            {
                return -1;
            }
            if (betterCoverageFromValue2 && !betterCoverageFromValue1)
            {
                return 1;
            }

            if (CompareTest != null)
            {
                // In the case of a tie call the external compare test to determine the nearest candidate.
                return CompareTest.Invoke(criteriaValue, candidateValue1, candidateValue2);
            }

            // No tie breaker was provided.
            return 0;
        }
    }
}
