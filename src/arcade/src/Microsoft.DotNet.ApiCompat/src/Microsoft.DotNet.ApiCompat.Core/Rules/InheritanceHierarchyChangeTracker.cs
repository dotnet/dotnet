// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Composition;
using System.Diagnostics;
using System.Linq;
using Microsoft.Cci;
using Microsoft.Cci.Differs;
using Microsoft.Cci.Extensions;

namespace Microsoft.DotNet.ApiCompat.Rules
{
    // This rule ensures the inheritance hierarchy didn't change in drastic ways.
    // Our goal is to create a subset of two Frameworks that essentially looks like an older 
    // version of both.  So all the normal versioning rules must apply from the subset to
    // both Frameworks.  For types, this means we can from the subset we can add new base classes
    // to represent either Framework, but the changes we make must both be compatible with each
    // Framework.  Essentially, we can add base classes and we can remove base classes,
    // but we can't do both at the same time.  (In reality, we can both add and remove base classes
    // at the same time, but one of those must inherit from the other.)
    //[ExportDifferenceRule(NonAPIConformanceRule = true)]  // does not enforce that one set is a subset of the other - not appropriate for version-to-version API conformance.
    internal class InheritanceHierarchyChangeTracker : DifferenceRule
    {
        [Import]
        public IEqualityComparer<ITypeReference> _typeComparer { get; set; } = null;

        // Consider the following object hierarchy.  Remember we are not enforcing a subset relationship
        // on both types.  Our goal is to build a third API that is a subset of both, with versioning rules
        // that will produce a consistent universe with both new frameworks.
        //             Object
        //        Fruit        Shape
        //       Apple
        //      Red Delicious
        // 
        // 1) A type is not reparented across the hierarchy.  (ie, Apple may subclass Object, but may not subclass Shape)
        // 2) Removing a type like Apple from the inheritance hierarchy is legal, BUT all new methods on Apple must be
        //    duplicated on Red Delicious.  (Methods will be tracked in a separate rule)
        // 3) If you removed a type like Apple, then all overridden methods from Fruit & higher should be
        //    re-overridden by Red Delicious to ensure behavior is compatible.  (methods will be tracked in a separate rule)
        // 4) It is legal to add a type like Fruit into the hierarchy - consider this the reverse of #2.
        // 5) If one Framework had Object -> NewType -> Apple, that is only legal if NewType is in the other 
        //    Framework's inheritance hierarchy somewhere between Object and Apple.  IE, NewType could be 
        //    "Food" or "RoundFruit", but could not be "Shape"
        public override DifferenceType Diff(IDifferences differences, ITypeDefinition item1, ITypeDefinition item2)
        {
            if (item1 == null || item2 == null)
                return DifferenceType.Unknown;

            Debug.Assert(_typeComparer != null);

            IEnumerable<ITypeReference> item1BaseClassChain = GetBaseClassChain(item1);
            IEnumerable<ITypeReference> item2BaseClassChain = GetBaseClassChain(item2);

            bool added = item2BaseClassChain.Except(item1BaseClassChain, _typeComparer).Any(t => true);
            bool removed = item1BaseClassChain.Except(item2BaseClassChain, _typeComparer).Any(t => true);

            // To a first approximation, we cannot both add base types and remove base types.
            // IE, we cannot take an Apple, remove Fruit and add Shape.
            // However there are more pathologically complicated inheritance hierarchies that are linear but where we 
            // add one type & remove another.  If both additions & removals occur, they're only legal if one of those 
            // added or removed types subclasses the other one.  We do not currently check for that.
            if (added && removed)
            {
                // Special case for DependencyObject and its derived types
                if (item1BaseClassChain.Any((type) => TypeHelper.GetTypeName(type) == "System.Windows.DependencyObject") &&
                    item2BaseClassChain.Any((type) => TypeHelper.GetTypeName(type) == "Windows.UI.DirectUI.DependencyObject"))
                {
                    // If the new type name is the same as the old type name in a new namespace, let's consider that a known issue.
                    String oldBaseTypeName = TypeHelper.GetTypeName(item1BaseClassChain.First());
                    String newBaseTypeName = TypeHelper.GetTypeName(item2BaseClassChain.First());
                    oldBaseTypeName = oldBaseTypeName.Replace("System.Windows", "Windows.UI.DirectUI");
                    if (oldBaseTypeName == newBaseTypeName)
                        return DifferenceType.Unknown;
                }

                differences.AddIncompatibleDifference(this,
                    "Type {0} or one of its base classes was reparented in an incompatible way.  Old base classes: {1}  New Base Classes: {2}",
                    item1.FullName(), PrintClassHierarchy(item1BaseClassChain), PrintClassHierarchy(item2BaseClassChain));
                return DifferenceType.Changed;
            }

            return DifferenceType.Unknown;
        }

        // Does not include this type.
        private IList<ITypeReference> GetBaseClassChain(ITypeDefinition type)
        {
            List<ITypeDefinition> bases = new List<ITypeDefinition>();
            ITypeDefinition t = type;
            while (t != null)
            {
                t = t.BaseClasses.SingleOrDefault().GetDefinitionOrNull();   // If there are multiple base classes, update rule to handle multiple inheritance.
                if (t != null)
                    bases.Add(t);
            }
            return bases.ToArray<ITypeReference>();
        }

        private String PrintClassHierarchy(IEnumerable<ITypeReference> baseClasses)
        {
            Debug.Assert(baseClasses != null);

            return String.Join(", ", baseClasses.Reverse().Select(t => t.FullName()));
        }
    }
}
