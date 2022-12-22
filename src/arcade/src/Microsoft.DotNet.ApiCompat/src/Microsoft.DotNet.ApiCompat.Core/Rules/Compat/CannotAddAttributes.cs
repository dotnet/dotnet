// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Cci.Extensions;
using Microsoft.Cci.Writers.CSharp;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Cci.Mappings;

namespace Microsoft.Cci.Differs.Rules
{
    // @todo: This is still a work-in-progress - suppressing it as it's causing repetition wrt to the Mdil rule that checks for specific custom attributes.
    // [ExportDifferenceRule]
    internal class CannotAddAttributes : CompatDifferenceRule
    {
        private MappingSettings _settings = new MappingSettings();

        public override DifferenceType Diff(IDifferences differences, IAssembly impl, IAssembly contract)
        {
            if (impl == null || contract == null)
                return DifferenceType.Unknown;

            bool added = false;

            added |= AnyAttributeAdded(differences, impl, impl.AssemblyAttributes, contract.AssemblyAttributes);
            added |= AnyAttributeAdded(differences, impl, impl.ModuleAttributes, contract.ModuleAttributes);
            added |= AnySecurityAttributeAdded(differences, impl, impl.SecurityAttributes, contract.SecurityAttributes);

            if (added)
                return DifferenceType.Changed;

            return DifferenceType.Unknown;
        }

        public override DifferenceType Diff(IDifferences differences, ITypeDefinition impl, ITypeDefinition contract)
        {
            if (impl == null || contract == null)
                return DifferenceType.Unknown;

            if (AnyAttributeAdded(differences, impl, impl.Attributes, contract.Attributes))
                return DifferenceType.Changed;

            return DifferenceType.Unknown;
        }

        public override DifferenceType Diff(IDifferences differences, ITypeDefinitionMember impl, ITypeDefinitionMember contract)
        {
            if (impl == null || contract == null)
                return DifferenceType.Unknown;

            bool added = false;

            added |= AnyAttributeAdded(differences, impl, impl.Attributes, contract.Attributes);
            added |= AnyMethodSpecificAttributeAdded(differences, impl as IMethodDefinition, contract as IMethodDefinition);

            if (added)
                return DifferenceType.Changed;

            return DifferenceType.Unknown;
        }

        private bool AnyMethodSpecificAttributeAdded(IDifferences differences, IMethodDefinition implMethod, IMethodDefinition contractMethod)
        {
            if (implMethod == null || contractMethod == null)
                return false;

            bool added = false;

            added |= AnyAttributeAdded(differences, implMethod, implMethod.ReturnValueAttributes, contractMethod.ReturnValueAttributes);
            added |= AnySecurityAttributeAdded(differences, implMethod, implMethod.SecurityAttributes, contractMethod.SecurityAttributes);

            Debug.Assert(implMethod.ParameterCount == contractMethod.ParameterCount);

            IParameterDefinition[] method1Params = implMethod.Parameters.ToArray();
            IParameterDefinition[] method2Params = contractMethod.Parameters.ToArray();
            for (int i = 0; i < implMethod.ParameterCount; i++)
                added |= AnyAttributeAdded(differences, method1Params[i], method1Params[i].Attributes, method2Params[i].Attributes);

            return added;
        }

        private bool AnySecurityAttributeAdded(IDifferences differences, IReference target, IEnumerable<ISecurityAttribute> attribues1, IEnumerable<ISecurityAttribute> attributes2)
        {
            return AnyAttributeAdded(differences, target, attribues1.SelectMany(a => a.Attributes), attributes2.SelectMany(a => a.Attributes));
        }

        private bool AnyAttributeAdded(IDifferences differences, IReference target, IEnumerable<ICustomAttribute> implAttributes, IEnumerable<ICustomAttribute> contractAttributes)
        {
            bool added = false;

            AttributesMapping<IEnumerable<ICustomAttribute>> attributeMapping = new AttributesMapping<IEnumerable<ICustomAttribute>>(_settings);
            attributeMapping.AddMapping(0, implAttributes);
            attributeMapping.AddMapping(1, contractAttributes);

            foreach (var group in attributeMapping.Attributes)
            {
                switch (group.Difference)
                {
                    case DifferenceType.Added:
                        ITypeReference type = group.Representative.Attributes.First().Type;
                        string attribName = type.FullName();

                        if (s_IgnorableAttributes.Contains(attribName))
                            break;

                        differences.AddIncompatibleDifference(this,
                            $"Attribute '{attribName}' exists in the {Contract} but not the {Implementation}.");

                        added = true;

                        break;
                    case DifferenceType.Changed:

                        //TODO: Add some more logic to check the two lists of attributes which have the same type.
                        break;

                    case DifferenceType.Removed:
                        // Removing attributes is OK
                        break;
                }
            }

            return added;
        }

        // Ignore list copied from ApiConformance tool
        private static HashSet<string> s_IgnorableAttributes = new HashSet<string> {
            "System.Reflection.AssemblyFileVersionAttribute",
            "System.Reflection.AssemblyInformationalVersionAttribute",
            "System.Reflection.AssemblyKeyFileAttribute",
            "System.Runtime.AssemblyTargetedPatchBandAttribute",
            "System.ObsoleteAttribute",
            "System.SupportedPlatformsAttribute",
            "System.Reflection.AssemblyProductAttribute",
            "System.Resources.SatelliteContractVersionAttribute",
            "System.Runtime.CompilerServices.TypeForwardedFromAttribute",
            "System.Runtime.CompilerServices.CompilerGeneratedAttribute",
            "System.Runtime.TargetedPatchingOptOutAttribute",
            "System.ComponentModel.EditorBrowsableAttribute",
            "System.Diagnostics.DebuggerDisplayAttribute", // Should this really be ignored?  What about cross sku?
            "System.Diagnostics.DebuggerTypeProxyAttribute", // Should this really be ignored? What about cross sku?
            "System.Diagnostics.DebuggerBrowsableAttribute", // Should this really be ignored? What about cross sku?
            "System.Runtime.CompilerServices.FriendAccessAllowedAttribute", // Should this really be ignorable when checking the a previous version of the same sku?
            "System.Runtime.CompilerServices.InternalsVisibleToAttribute", // Should this really be ignorable when checking the a previous version of the same sku?
            "System.Runtime.CompilerServices.ReferenceAssemblyAttribute",
            "System.Security.UnverifiableCodeAttribute", // Ignoring this for now because all the refasms are build with the /unsafe switch even if it isn't necessary
            "System.Runtime.CompilerServices.ExtensionAttribute",

            // For now we are ignoring security attributes because they can be different in 
            // the contract assemblies from the implementation assemblies. At some future time we 
            // should add a rule to verify that the security is the same in the contracts and implementation.
            "System.Security.SecuritySafeCriticalAttribute",
            "System.Security.SecurityCriticalAttribute",
            "System.Security.AllowPartiallyTrustedCallersAttribute",
            "System.Security.SecurityRulesAttribute",
        };
    }
}
