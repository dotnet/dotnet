// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Cci;
using Microsoft.Cci.Extensions;

namespace Microsoft.Cci.Extensions
{
    public class TypeDependencies
    {
        private ICollection<ITypeReference> _publicDependents;
        private ICollection<ITypeReference> _dependents;
        private ICollection<IMethodReference> _methodDependents;

        public TypeDependencies(ITypeDefinition type, bool unique = true)
        {
            if (unique)
            {
                _publicDependents = new HashSet<ITypeReference>();
                _dependents = new HashSet<ITypeReference>();
                _methodDependents = new HashSet<IMethodReference>();
            }
            else
            {
                _publicDependents = new List<ITypeReference>();
                _dependents = new List<ITypeReference>();
                _methodDependents = new List<IMethodReference>();
            }
            CalculateForType(type);
        }

        public IEnumerable<ITypeReference> PublicTypeDepdencies { get { return _publicDependents; } }
        public IEnumerable<ITypeReference> AllTypeDependencies { get { return _dependents; } }
        public IEnumerable<IMethodReference> MethodDependencies { get { return _methodDependents; } }

        private void CalculateForType(ITypeDefinition type)
        {
            if (type.HasDeclarativeSecurity)
                AddDependency(type.SecurityAttributes);
            AddDependency(type.Attributes);

            AddDependency(type.BaseClasses);
            AddDependency(type.Interfaces);
            //AddDependency(type.ExplicitImplementationOverrides);

            AddDependency(type.Fields);
            AddDependency(type.Events);
            AddDependency(type.Properties);
            AddDependency(type.Methods);

            if (type.IsGeneric)
                AddDependency(type.GenericParameters);

            AddDependencyNestedTypes(type.NestedTypes);
        }

        private void AddDependencyNestedTypes(IEnumerable<INestedTypeDefinition> nestedTypes)
        {
            // TODO: Verify that this works properly for non-public nested types.
            // I'm guessing that it doesn't at this point and we might need to propagate the ispublic flag.
            foreach (INestedTypeDefinition type in nestedTypes)
                CalculateForType(type);
        }

        private void AddDependency(IEnumerable<IMethodDefinition> methods)
        {
            foreach (IMethodDefinition method in methods)
            {
                bool isPublic = method.IsVisibleOutsideAssembly();

                // Attributes
                if (method.HasDeclarativeSecurity)
                    AddDependency(method.SecurityAttributes, isPublic);
                AddDependency(method.Attributes, isPublic);

                // Return type
                if (method.Type.TypeCode != PrimitiveTypeCode.Void)
                    AddDependency(method.Type, isPublic);
                AddDependency(method.ReturnValueAttributes, isPublic);

                // Generic parameters
                if (method.IsGeneric)
                    AddDependency(method.GenericParameters, isPublic);

                // Parameters
                foreach (IParameterDefinition param in method.Parameters)
                {
                    AddDependency(param.Attributes, isPublic);
                    AddDependency(param.Type, isPublic);
                }

                // Method body
                AddDependencyFromMethodBody(method.Body);
            }
        }

        private void AddDependency(IEnumerable<IFieldDefinition> fields)
        {
            foreach (IFieldDefinition field in fields)
            {
                AddDependency(field.Attributes, field.IsVisibleOutsideAssembly());
                AddDependency(field.Type, field.IsVisibleOutsideAssembly());
            }
        }

        private void AddDependency(IEnumerable<IEventDefinition> events)
        {
            foreach (IEventDefinition evnt in events)
            {
                AddDependency(evnt.Attributes, evnt.IsVisibleOutsideAssembly());
                AddDependency(evnt.Type, evnt.IsVisibleOutsideAssembly());
            }
        }

        private void AddDependency(IEnumerable<IPropertyDefinition> properties)
        {
            foreach (IPropertyDefinition property in properties)
            {
                AddDependency(property.Attributes, property.IsVisibleOutsideAssembly());
                AddDependency(property.Type, property.IsVisibleOutsideAssembly());
            }
        }

        private void AddDependency(IEnumerable<ISecurityAttribute> attributes, bool isPublic = true)
        {
            foreach (ISecurityAttribute attribute in attributes)
                AddDependency(attribute.Attributes);
        }

        private void AddDependency(IEnumerable<ICustomAttribute> attributes, bool isPublic = true)
        {
            foreach (ICustomAttribute attribute in attributes)
                AddDependency(attribute.Type, isPublic);
        }

        private void AddDependency(IEnumerable<IGenericParameter> parameters, bool isPublic = true)
        {
            foreach (var genericParam in parameters)
                foreach (var constraint in genericParam.Constraints)
                    AddDependency(constraint, isPublic);
        }

        private void AddDependencyFromMethodBody(IMethodBody methodBody)
        {
            foreach (ILocalDefinition local in methodBody.LocalVariables)
            {
                AddDependency(local.Type);
            }

            foreach (IOperation op in methodBody.Operations)
            {
                switch (op.OperationCode)
                {
                    case OperationCode.Castclass:
                    case OperationCode.Box:
                    case OperationCode.Unbox:
                    case OperationCode.Unbox_Any:
                        AddDependency((ITypeReference)op.Value);
                        break;
                    case OperationCode.Call:
                    //case OperationCode.Calli: Native calls
                    case OperationCode.Callvirt:
                    case OperationCode.Newobj:
                    case OperationCode.Ldftn:
                    case OperationCode.Ldvirtftn:
                        IMethodReference methodRef = (IMethodReference)op.Value;
                        AddDependencyForCalledMethod(methodRef);
                        break;
                }
            }
        }

        private void AddDependency(IEnumerable<ITypeReference> types, bool isPublic = true)
        {
            foreach (ITypeReference type in types)
                AddDependency(type, isPublic);
        }
        private void AddDependency(ITypeReference type, bool isPublic = true)
        {
            if (isPublic)
                _publicDependents.Add(type);

            _dependents.Add(type);
        }
        private void AddDependencyForCalledMethod(IMethodReference method)
        {
            AddDependency(method.ContainingType, false);

            _methodDependents.Add(method);
        }
    }
}
