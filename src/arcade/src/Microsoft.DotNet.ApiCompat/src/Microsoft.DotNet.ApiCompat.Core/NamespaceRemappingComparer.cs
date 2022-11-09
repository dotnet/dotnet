// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

namespace Microsoft.Cci.Comparers
{
    public class NamespaceRemappingComparers : CciComparers
    {
        private List<Tuple<string, string>> _mappings = new List<Tuple<string, string>>();

        public NamespaceRemappingComparers(string mappingFile)
        {
            if (!string.IsNullOrEmpty(mappingFile))
                ParseConfig(mappingFile);
            Debug.Assert(_mappings.Count > 0, "NamespaceRemappingComparers has no namespace remappings.");
        }

        public override string GetKey(INamespaceDefinition ns)
        {
            return RemapName(base.GetKey(ns));
        }

        public override string GetKey(ITypeReference type)
        {
            return RemapName(base.GetKey(type));
        }

        public override string GetKey(ITypeDefinitionMember member)
        {
            return RemapName(base.GetKey(member));
        }

        private string RemapName(string name)
        {
            Debug.Assert(name != null);

            Debug.Assert(_mappings.Count > 0, "NamespaceRemappingComparers has no namespace remappings.");

            int maxNumTypesLeft = CountNumTypes(name);
            string mappedName = name;
            foreach (var mapping in _mappings)
            {
                // Note that we can have partial matches on type names!  Do not rename a type like
                // RelativeSourceMode to RelativeSource - we have both names.  Similarly, if we have
                // a rule like DependencyProperty -> IDependencyProperty, then make sure we don't 
                // consider IDependencyProperty a match, converting it to "IIDependencyProperty"
                if (IsBadTypeNameMatch(mappedName, mapping.Item1))
                {
                    //Debug.WriteLine("NamespaceRemappingComparer: Avoiding name mapping for {0} because {1} is a partial match.", name, mapping.Item1);
                    continue;
                }

                mappedName = mappedName.Replace(mapping.Item1, mapping.Item2);

                // Perf optimization - assume if we've found what we're trying to replace, we're done and can return.
                // However this needs to work for strings that contain multiple types, such as method signatures
                // or generic types.  We'd replace hopefully each of those types.
                if (!Object.ReferenceEquals(mappedName, name))
                {
                    maxNumTypesLeft--;
                    if (maxNumTypesLeft == 0)
                        return mappedName;
                }
            }

            return mappedName;
        }

        private static bool IsBadTypeNameMatch(String name, String mappingName)
        {
            // Check whether we have more characters afterwards
            bool moreAfter = (name.StartsWith(mappingName) && !name.Equals(mappingName) &&
                (name[mappingName.Length] != '.' && name[mappingName.Length] != ')' && name[mappingName.Length] != '<'));
            int indexOfName = name.IndexOf(mappingName);
            bool moreBefore = indexOfName > 0 && (name[indexOfName - 1] != '.' && name[indexOfName - 1] != '(' && name[indexOfName - 1] != ' ' && name[indexOfName - 1] != '<');
            return moreAfter || moreBefore;
        }

        // Maximum number of types in a type name is 1 + the number of commas, :'s, ('s and <'s.  Consider Foo<int,object>(int x, Bar y).  5 types.
        // Or consider G<Foo> : Base, IFoo
        // Note name can be a type name or method signature or generic type or delegate.
        private int CountNumTypes(String name)
        {
            Debug.Assert(name != null);

            // For nested types, I think we only need to map the outer type.  But in case it comes up, *you* can think about this.
            Debug.Assert(name.IndexOf('+') < 0, "Encountered a nested type.  Not sure whether to treat that as two types or one, or to only remap the outer type.");

            int numTypes = 1;
            foreach (char c in name)
                if (c == ',' || c == '(' || c == '<' || c == ':')
                    numTypes++;
            return numTypes;
        }

        private void ParseConfig(string mappingFile)
        {
            Debug.Assert(!String.IsNullOrEmpty(mappingFile));

            Debug.Assert(File.Exists(mappingFile), String.Format("Excepting namespace mapping file \"{0}\" to exist.", mappingFile));
            foreach (string mapping in File.ReadAllLines(mappingFile))
            {
                if (string.IsNullOrWhiteSpace(mapping) ||
                    mapping.StartsWith("#") ||
                    mapping.StartsWith("//"))
                    continue;

                string[] split = mapping.Split(',');

                if (split.Length != 2)
                {
                    Debug.WriteLine("ApiCompat NamespaceRemappingComparer: unparsable line found in file {0}.  Line: \"{1}\"", mappingFile, mapping);
                    continue;
                }

                _mappings.Add(Tuple.Create(split[0], split[1]));
            }

            Debug.Assert(_mappings.Count > 0, "Expected to find namespace mappings in our namespace mapping text file.  Is this intentional?");
        }
    }
}
