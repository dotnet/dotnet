// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;

using Microsoft.Cci.Extensions;

namespace Microsoft.Cci.Filters
{
    public class NotImplementedFilter : ICciFilter
    {
        public virtual bool Include(INamespaceDefinition ns)
        {
            return true;
        }

        public virtual bool Include(ICustomAttribute attribute)
        {
            return true;
        }

        public virtual bool Include(ITypeDefinition type)
        {
            var hasOnlyExcludedMembers = type.Members.Any() && type.Members.All(m => !Include(m));
            return !hasOnlyExcludedMembers;
        }

        public virtual bool Include(ITypeDefinitionMember member)
        {
            var evnt = member as IEventDefinition;
            if (evnt != null)
                return Include(evnt);

            var prop = member as IPropertyDefinition;
            if (prop != null)
                return Include(prop);

            var method = member as IMethodDefinition;
            if (method != null)
                return Include(method);

            return true;
        }

        private bool Include(IPropertyDefinition prop)
        {
            return prop.Accessors.Any(a => Include(a.ResolvedMethod));
        }

        private bool Include(IEventDefinition evnt)
        {
            return evnt.Accessors.Any(a => Include(a.ResolvedMethod));
        }

        private bool Include(IMethodDefinition method)
        {
            return IsImplemented(method);
        }

        private static bool IsImplemented(IMethodDefinition method)
        {
            // Generally, a method that throws NotImplementedException is considered not implemented.
            // However, if the containing assembly is marked to be a reference assembly then this is
            // likely not true as virtually all methods in reference assemblies have bodies that throw.
            //
            // Thus, if the assembly is a reference assembly, we consider it implemented.

            return IsContainedInReferenceAssembly(method) || !ThrowsNotImplementedException(method);
        }

        private static bool IsContainedInReferenceAssembly(IMethodDefinition method)
        {
            var asssembly = method.ContainingType.GetAssemblyReference().ResolvedAssembly;
            return asssembly.IsReferenceAssembly();
        }

        private static bool ThrowsNotImplementedException(IMethodDefinition method)
        {
            // We consider a method not implemented if it contains the following sequence of IL:
            //
            //      ...
            //      newobj instance void [<some assembly>]System.NotImplementedException::.ctor()
            //      throw
            //      ...
            //
            // Note that we deliberately ignore any IL before and after. The reason being that some methods
            // unrelated code, such as argument validation. Yes, this can result in false positives, such as
            // when methods only throw in a specific branch that doesn't apply for all scenarios. For now,
            // we just ignore this case.
            //
            // Also note that empty methods are considered implemented. There are many methods for which doing
            // nothing is a legitimate fulfillment of its contract.
            //
            // In order to be resilient to code gen differences we'll allow for any nop instructions between
            // the newobj and the throw.

            var lastOp = (IOperation)null;

            foreach (var thisOp in method.Body.Operations)
            {
                if (thisOp.OperationCode == OperationCode.Nop)
                    continue;

                if (lastOp != null)
                {
                    if (thisOp.OperationCode == OperationCode.Throw &&
                        lastOp.OperationCode == OperationCode.Newobj &&
                        IsNotImplementedConstructor(lastOp.Value))
                    {
                        return true;
                    }
                }

                lastOp = thisOp;
            }

            return false;
        }

        private static bool IsNotImplementedConstructor(object value)
        {
            var method = value as IMethodReference;
            if (method == null)
                return false;

            var containingType = method.ContainingType;
            var nameOptions = NameFormattingOptions.UseGenericTypeNameSuffix |
                              NameFormattingOptions.UseReflectionStyleForNestedTypeNames;
            var typeName = containingType.GetTypeName(nameOptions);
            return typeName == "System.NotImplementedException";
        }
    }
}

