using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.DotNet.Internal.DependencyInjection.Testing
{
    public static class DependencyInjectionValidation
    {
        private static readonly ImmutableList<string> s_exemptTypes = ImmutableList.Create(
            "System.Fabric.ServiceContext",
            "System.Fabric.StatelessServiceContext",
            "System.Fabric.StatefulServiceContext",
            "Microsoft.DotNet.ServiceFabric.ServiceHost.IServiceLoadReporter",
            "Microsoft.Extensions.Options.IConfigureOptions`1",
            "Microsoft.Extensions.Caching.Memory.MemoryCacheOptions",
            "Microsoft.Extensions.Caching.Memory.MemoryDistributedCacheOptions",
            "Microsoft.Extensions.DependencyInjection.IServiceScopeFactory",
            "System.IServiceProvider"
        );

        private static readonly ImmutableList<string> s_exemptNamespaces = ImmutableList.Create(
            "Microsoft.ApplicationInsights.AspNetCore",
            "Microsoft.AspNetCore"
        );

        public static bool IsDependencyResolutionCoherent(
            Action<ServiceCollection> register,
            out string errorMessage,
            IEnumerable<Type> additionalScopedTypes = null,
            IEnumerable<Type> additionalSingletonTypes = null)
        {
            errorMessage = null;

            StringBuilder allErrors = new StringBuilder();
            allErrors.Append("The following types are not resolvable:");

            var services = new ServiceCollection();

            register(services);

            bool allResolved = true;

            foreach (ServiceDescriptor service in services)
            {
                if (service.ImplementationType == null)
                {
                    continue;
                }

                if (IsExemptType(service.ImplementationType) || IsExemptType(service.ServiceType))
                {
                    continue;
                }

                if (!IsTypeResolvable(service.ImplementationType, services, allErrors, service.Lifetime))
                {
                    allResolved = false;
                }
            }

            foreach (Type scopedType in additionalScopedTypes ?? Enumerable.Empty<Type>())
            {
                if (!IsTypeResolvable(scopedType, services, allErrors, ServiceLifetime.Scoped))
                {
                    allResolved = false;
                }
            }

            foreach (Type scopedType in additionalSingletonTypes ?? Enumerable.Empty<Type>())
            {
                if (!IsTypeResolvable(scopedType, services, allErrors, ServiceLifetime.Singleton))
                {
                    allResolved = false;
                }
            }

            if (!allResolved)
                errorMessage = allErrors.ToString();

            return allResolved;
        }

        private static bool IsTypeResolvable(
            Type type,
            ServiceCollection services,
            StringBuilder msgBuilder,
            ServiceLifetime serviceLifetime)
        {
            ConstructorInfo[] constructors = type
                .GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                .OrderBy(c => c.GetParameters().Length)
                .ToArray();

            if (constructors.Length == 0)
            {
                // zero constructor things are implicitly constructable
                return true;
            }

            string errorMessage = null;
            foreach (ConstructorInfo ctor in constructors)
            {
                if (IsConstructorResolvable(ctor, services, errorMessage == null, serviceLifetime, out string ctorMsg))
                {
                    return true;
                }

                errorMessage ??= ctorMsg;
            }

            msgBuilder.AppendLine();
            msgBuilder.AppendLine();
            msgBuilder.AppendLine(errorMessage);

            return false;
        }

        private static bool IsConstructorResolvable(
            ConstructorInfo ctor,
            ServiceCollection services,
            bool recordErrors,
            ServiceLifetime serviceLifetime,
            out string errorMessage)
        {
            errorMessage = null;
            bool resolvedAllParameters = true;
            StringBuilder msgBuilder = null;
            if (recordErrors)
            {
                msgBuilder = new StringBuilder();
                msgBuilder.Append("Type ");
                msgBuilder.Append(ctor.DeclaringType.FullName);
                msgBuilder.Append(" could not find registered definition for parameter(s): ");
            }

            foreach (ParameterInfo p in ctor.GetParameters())
            {
                ServiceDescriptor parameterService = services.FirstOrDefault(s => IsMatchingServiceRegistration(s.ServiceType, p.ParameterType));
                if (parameterService != null)
                {
                    if (serviceLifetime == ServiceLifetime.Singleton &&
                        parameterService.Lifetime == ServiceLifetime.Scoped)
                    {
                        if (!resolvedAllParameters)
                        {
                            msgBuilder.Append(", ");
                        }

                        msgBuilder.Append("<SCOPED> ");
                        msgBuilder.Append(p.Name);
                        msgBuilder.Append(" of type ");
                        msgBuilder.Append(GetDisplayName(p.ParameterType));

                        resolvedAllParameters = false;
                    }

                    continue;
                }

                // Save the first error message, since it's likely to be the most useful
                if (recordErrors)
                {
                    if (!resolvedAllParameters)
                    {
                        msgBuilder.Append(", ");
                    }

                    msgBuilder.Append(p.Name);
                    msgBuilder.Append(" of type ");
                    msgBuilder.Append(GetDisplayName(p.ParameterType));
                }

                resolvedAllParameters = false;
            }

            if (recordErrors && !resolvedAllParameters)
            {
                errorMessage = msgBuilder.ToString();
            }

            return resolvedAllParameters;
        }

        private static string GetDisplayName(Type type)
        {
            if (type.IsConstructedGenericType)
            {
                // The name of IOptions<Pizza> is "IOptions`1"
                // The full name has the other types, but they are all fully qualified (and also still have the `1 on them)
                string baseName = type.Name.Split('`')[0];
                return $"{baseName}<{string.Join(", ", type.GetGenericArguments().Select(GetDisplayName))}>";
            }

            return type.Name;
        }

        private static bool IsMatchingServiceRegistration(Type serviceType, Type parameterType)
        {
            // If it's options, lets make sure they are configured
            if (parameterType.IsConstructedGenericType)
            {
                Type parameterRoot = parameterType.GetGenericTypeDefinition();
                if (IsOptionsType(parameterRoot))
                {
                    if (!serviceType.IsConstructedGenericType) return false;

                    Type optionType = parameterType.GenericTypeArguments[0];

                    if (IsExemptType(optionType))
                        return true;

                    Type serviceRoot = serviceType.GetGenericTypeDefinition();
                    return serviceRoot.FullName == "Microsoft.Extensions.Options.IConfigureOptions`1" &&
                        serviceType.GenericTypeArguments[0] == optionType;
                }
            }

            if (IsExemptType(parameterType))
            {
                return true;
            }

            if (serviceType == parameterType) return true;
            if (!parameterType.IsConstructedGenericType) return false;
            Type def = parameterType.GetGenericTypeDefinition();
            if (def == typeof(IEnumerable<>))
            {
                // IEnumerable can be zero, and that's fine
                return true;
            }

            return serviceType == def;
        }

        private static bool IsOptionsType(Type parameterRoot)
        {
            switch (parameterRoot.FullName)
            {
                case "Microsoft.Extensions.Options.IOptions`1":
                case "Microsoft.Extensions.Options.IOptionsMonitor`1":
                case "Microsoft.Extensions.Options.IOptionsSnapshot`1":
                    return true;
                default:
                    return false;
            }
        }

        private static bool IsExemptType(Type type)
        {
            if (type.IsConstructedGenericType)
                return IsExemptType(type.GetGenericTypeDefinition());

            return s_exemptTypes.Contains(type.FullName) || s_exemptNamespaces.Any(n => type.FullName.StartsWith(n));
        }
    }
}
