// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Cci.Extensions;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Cci.Mappings;
using System.Composition;
using Microsoft.Cci.Comparers;
using Microsoft.Cci.Filters;

namespace Microsoft.Cci.Differs.Rules
{
    internal class AssemblyAttributeDifferences : AttributeDifference
    {
        public override DifferenceType Diff(IDifferences differences, IAssembly impl, IAssembly contract)
        {
            if (impl == null || contract == null)
                return DifferenceType.Unknown;

            bool added = false;

            //added |= AnyAttributeAdded(differences, impl, impl.AssemblyAttributes, contract.AssemblyAttributes);
            //added |= AnyAttributeAdded(differences, impl, impl.ModuleAttributes, contract.ModuleAttributes);
            //added |= AnySecurityAttributeAdded(differences, impl, impl.SecurityAttributes, contract.SecurityAttributes);

            if (added)
                return DifferenceType.Changed;

            return DifferenceType.Unknown;
        }
    }

    [ExportDifferenceRule]
    internal class AttributeDifference : CompatDifferenceRule
    {
        private readonly MappingSettings _settings = new MappingSettings()
        {
            Filter = new AttributesFilter(includeAttributes: true)
        };

        [Import]
        public IAttributeFilter AttributeFilter { get; set; }

        public override DifferenceType Diff(IDifferences differences, IAssembly impl, IAssembly contract)
        {
            if (impl == null || contract == null)
                return DifferenceType.Unknown;

            bool added = false;

            //added |= AnyAttributeAdded(differences, impl, impl.AssemblyAttributes, contract.AssemblyAttributes);
            //added |= AnyAttributeAdded(differences, impl, impl.ModuleAttributes, contract.ModuleAttributes);
            //added |= AnySecurityAttributeAdded(differences, impl, impl.SecurityAttributes, contract.SecurityAttributes);

            if (added)
                return DifferenceType.Changed;

            return DifferenceType.Unknown;
        }

        public override DifferenceType Diff(IDifferences differences, ITypeDefinition impl, ITypeDefinition contract)
        {
            if (impl == null || contract == null)
                return DifferenceType.Unknown;

            bool changed = CheckAttributeDifferences(differences, impl, impl.Attributes, contract.Attributes);
            if (impl.IsGeneric)
            {
                IGenericParameter[] method1GenParams = impl.GenericParameters.ToArray();
                IGenericParameter[] method2GenParam = contract.GenericParameters.ToArray();
                for (int i = 0; i < impl.GenericParameterCount; i++)
                    changed |= CheckAttributeDifferences(differences, method1GenParams[i], method1GenParams[i].Attributes, method2GenParam[i].Attributes, member: contract);
            }

            return changed ? DifferenceType.Changed : DifferenceType.Unchanged; ;
        }

        public override DifferenceType Diff(IDifferences differences, ITypeDefinitionMember impl, ITypeDefinitionMember contract)
        {
            if (impl == null || contract == null)
                return DifferenceType.Unknown;

            bool changed = CheckAttributeDifferences(differences, impl, impl.Attributes, contract.Attributes);

            var implMethod = impl as IMethodDefinition;
            var contractMethod = contract as IMethodDefinition;
            if (implMethod != null && contractMethod != null)
            {
                IParameterDefinition[] method1Params = implMethod.Parameters.ToArray();
                IParameterDefinition[] method2Params = contractMethod.Parameters.ToArray();
                for (int i = 0; i < implMethod.ParameterCount; i++)
                    changed |= CheckAttributeDifferences(differences, method1Params[i], method1Params[i].Attributes, method2Params[i].Attributes, member: implMethod);

                if (implMethod.IsGeneric)
                {
                    IGenericParameter[] method1GenParams = implMethod.GenericParameters.ToArray();
                    IGenericParameter[] method2GenParam = contractMethod.GenericParameters.ToArray();
                    for (int i = 0; i < implMethod.GenericParameterCount; i++)
                        changed |= CheckAttributeDifferences(differences, method1GenParams[i], method1GenParams[i].Attributes, method2GenParam[i].Attributes, member: implMethod);
                }
            }

            return changed ? DifferenceType.Changed : DifferenceType.Unchanged;
        }

        private bool CheckAttributeDifferences(IDifferences differences, IReference target, IEnumerable<ICustomAttribute> implAttributes, IEnumerable<ICustomAttribute> contractAttributes, IReference member = null)
        {
            AttributesMapping<IEnumerable<ICustomAttribute>> attributeMapping = new AttributesMapping<IEnumerable<ICustomAttribute>>(_settings);
            AttributeComparer attributeComparer = new AttributeComparer();
            attributeMapping.AddMapping(0, contractAttributes.OrderBy(a => a, attributeComparer));
            attributeMapping.AddMapping(1, implAttributes.OrderBy(a => a, attributeComparer));

            string errString = $"'{target.FullName()}'";
            if (target is IParameterDefinition || target is IGenericParameter)
            {
                errString = target is IGenericParameter ? "generic param" : "parameter";
                errString += $" '{target.FullName()}' on member '{member?.FullName()}'";
            }

            bool changed = false;
            foreach (var group in attributeMapping.Attributes)
            {
                switch (group.Difference)
                {
                    case DifferenceType.Added:
                        {
                            ITypeReference type = group.Representative.Attributes.First().Type;

                            if (AttributeFilter.ShouldExclude(type.DocId()))
                                break;

                            // Allow for additions
                            differences.Add(new Difference("AddedAttribute",
                                $"Attribute '{type.FullName()}' exists on {errString} in the {Implementation} but not the {Contract}."));

                            changed = true;
                            break;
                        }
                    case DifferenceType.Changed:
                        {
                            ITypeReference type = group.Representative.Attributes.First().Type;

                            if (AttributeFilter.ShouldExclude(type.DocId()))
                                break;

                            string contractKey = attributeComparer.GetKey(group[0].Attributes.First());
                            string implementationKey = attributeComparer.GetKey(group[1].Attributes.First());

                            differences.AddIncompatibleDifference("CannotChangeAttribute",
                                $"Attribute '{type.FullName()}' on {errString} changed from '{contractKey}' in the {Contract} to '{implementationKey}' in the {Implementation}.");

                            changed = true;
                            break;
                        }

                    case DifferenceType.Removed:
                        {
                            ITypeReference type = group.Representative.Attributes.First().Type;

                            if (AttributeFilter.ShouldExclude(type.DocId()))
                                break;

                            differences.AddIncompatibleDifference("CannotRemoveAttribute",
                                $"Attribute '{type.FullName()}' exists on {errString} in the {Contract} but not the {Implementation}.");


                            // removals of an attribute are considered a "change" of the type
                            changed = true;
                            break;
                        }
                }
            }
            return changed;
        }
    }

    public interface IAttributeFilter
    {
        bool ShouldExclude(string attributeDocID);
    }

    public class AttributeFilter : IAttributeFilter
    {
        private readonly HashSet<string> ignorableAttributes = new HashSet<string>();

        public AttributeFilter()
        { }

        public void AddIgnoreAttributeFile(string filePath)
        {
            foreach(string id in DocIdExtensions.ReadDocIds(filePath))
            {
                ignorableAttributes.Add(id);
            }
        }

        public bool ShouldExclude(string attributeDocID)
        {
            return ignorableAttributes.Contains(attributeDocID);
        }
    }
}
