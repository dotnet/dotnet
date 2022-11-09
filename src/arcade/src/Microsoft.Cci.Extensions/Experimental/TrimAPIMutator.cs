// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Cci;
//using Microsoft.Cci.Extensions;
//using Microsoft.Cci.MutableCodeModel;

//namespace AsmDiff
//{
//    public interface ITrimFilter
//    {
//        bool TrimAttribute(ICustomAttribute attribute);
//        bool TrimMember(ITypeDefinitionMember member);
//        bool TrimType(ITypeDefinition type);
//    }

//    public class PublicOlyTrimFilter : ITrimFilter
//    {
//        public bool TrimAttribute(ICustomAttribute attribute)
//        {
//            return false;
//        }

//        public bool TrimMember(ITypeDefinitionMember member)
//        {
//            return !member.IsVisibleOutsideAssembly();
//        }

//        public bool TrimType(ITypeDefinition type)
//        {
//            return !type.IsVisibleOutsideAssembly();
//        }
//    }

//    public class TrimAPIMutator : MetadataMutator
//    {
//        private ITrimFilter _filter;

//        public TrimAPIMutator(IMetadataHost host, ITrimFilter filter)
//            : base(host)
//        {
//            _filter = filter;
//        }

//        public static IAssembly TrimNonPublicAPIs(IAssembly assembly)
//        {
//            TrimAPIMutator trim = new TrimAPIMutator(new HostEnvironment(), new PublicOlyTrimFilter());

//            return trim.Visit(assembly);
//        }

//        public override List<INamespaceMember> Visit(List<INamespaceMember> members)
//        {
//            List<INamespaceMember> newList = new List<INamespaceMember>();
//            foreach (var member in members)
//            {
//                ITypeDefinition type = member as ITypeDefinition;
//                if (type != null)
//                {
//                    if (!_filter.TrimType(type))
//                        newList.Add(member);
//                }
//                else
//                {
//                    newList.Add(member);
//                }
//            }
//            return base.Visit(newList);
//        }

//        public override List<IPropertyDefinition> Visit(List<IPropertyDefinition> properties)
//        {
//            properties.RemoveAll(p => _filter.TrimMember(p));
//            return base.Visit(properties);
//        }

//        public override List<IEventDefinition> Visit(List<IEventDefinition> events)
//        {
//            events.RemoveAll(e => _filter.TrimMember(e));
//            return base.Visit(events);
//        }

//        public override List<IMethodDefinition> Visit(List<IMethodDefinition> methods)
//        {
//            methods.RemoveAll(m => _filter.TrimMember(m));
//            return base.Visit(methods);
//        }

//        public override List<INestedTypeDefinition> Visit(List<INestedTypeDefinition> types)
//        {
//            types.RemoveAll(t => _filter.TrimMember(t));
//            return base.Visit(types);
//        }

//        public override List<ITypeDefinitionMember> Visit(List<ITypeDefinitionMember> members)
//        {
//            members.RemoveAll(m => _filter.TrimMember(m));
//            return base.Visit(members);
//        }

//        public override List<ICustomAttribute> Visit(List<ICustomAttribute> customAttributes)
//        {
//            customAttributes.RemoveAll(a => _filter.TrimAttribute(a));
//            return base.Visit(customAttributes);
//        }

//        public override List<ISecurityAttribute> Visit(List<ISecurityAttribute> securityAttributes)
//        {
//            securityAttributes.RemoveAll(sa => sa.Attributes.Any(a => _filter.TrimAttribute(a)));
//            return base.Visit(securityAttributes);
//        }

//        public override MethodDefinition Visit(MethodDefinition methodDefinition)
//        {
//            // Kill the implementation
//            methodDefinition.Body = Dummy.MethodBody;
//            return base.Visit(methodDefinition);
//        }
//    }

//}
