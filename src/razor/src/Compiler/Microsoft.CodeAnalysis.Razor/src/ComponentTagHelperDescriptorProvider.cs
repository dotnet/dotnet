// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;

namespace Microsoft.CodeAnalysis.Razor;

internal class ComponentTagHelperDescriptorProvider : RazorEngineFeatureBase, ITagHelperDescriptorProvider
{
    private static readonly SymbolDisplayFormat GloballyQualifiedFullNameTypeDisplayFormat =
        SymbolDisplayFormat.FullyQualifiedFormat
            .WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Included)
            .WithMiscellaneousOptions(SymbolDisplayFormat.FullyQualifiedFormat.MiscellaneousOptions & (~SymbolDisplayMiscellaneousOptions.UseSpecialTypes));

    public bool IncludeDocumentation { get; set; }

    public int Order { get; set; }

    public void Execute(TagHelperDescriptorProviderContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var compilation = context.GetCompilation();
        if (compilation == null)
        {
            // No compilation, nothing to do.
            return;
        }

        var types = new List<INamedTypeSymbol>();
        var visitor = new ComponentTypeVisitor(types);

        var targetSymbol = context.Items.GetTargetSymbol();
        if (targetSymbol is not null)
        {
            visitor.Visit(targetSymbol);
        }
        else
        {
            visitor.Visit(compilation.Assembly.GlobalNamespace);
            foreach (var reference in compilation.References)
            {
                if (compilation.GetAssemblyOrModuleSymbol(reference) is IAssemblySymbol assembly)
                {
                    visitor.Visit(assembly.GlobalNamespace);
                }
            }
        }

        for (var i = 0; i < types.Count; i++)
        {
            var type = types[i];

            // Components have very simple matching rules.
            // 1. The type name (short) matches the tag name.
            // 2. The fully qualified name matches the tag name.
            var shortNameMatchingDescriptor = CreateShortNameMatchingDescriptor(type);
            context.Results.Add(shortNameMatchingDescriptor);
            var fullyQualifiedNameMatchingDescriptor = CreateFullyQualifiedNameMatchingDescriptor(type);
            context.Results.Add(fullyQualifiedNameMatchingDescriptor);

            foreach (var childContent in shortNameMatchingDescriptor.GetChildContentProperties())
            {
                // Synthesize a separate tag helper for each child content property that's declared.
                context.Results.Add(CreateChildContentDescriptor(shortNameMatchingDescriptor, childContent));
                context.Results.Add(CreateChildContentDescriptor(fullyQualifiedNameMatchingDescriptor, childContent));
            }
        }
    }

    private TagHelperDescriptor CreateShortNameMatchingDescriptor(INamedTypeSymbol type)
    {
        var builder = CreateDescriptorBuilder(type);
        builder.TagMatchingRule(r =>
        {
            r.TagName = type.Name;
        });

        return builder.Build();
    }

    private TagHelperDescriptor CreateFullyQualifiedNameMatchingDescriptor(INamedTypeSymbol type)
    {
        var builder = CreateDescriptorBuilder(type);
        var containingNamespace = type.ContainingNamespace.ToDisplayString();
        var fullName = $"{containingNamespace}.{type.Name}";
        builder.TagMatchingRule(r =>
        {
            r.TagName = fullName;
        });
        builder.Metadata[ComponentMetadata.Component.NameMatchKey] = ComponentMetadata.Component.FullyQualifiedNameMatch;

        return builder.Build();
    }

    private TagHelperDescriptorBuilder CreateDescriptorBuilder(INamedTypeSymbol type)
    {
        var typeName = type.ToDisplayString(SymbolExtensions.FullNameTypeDisplayFormat);
        var assemblyName = type.ContainingAssembly.Identity.Name;

        var builder = TagHelperDescriptorBuilder.Create(ComponentMetadata.Component.TagHelperKind, typeName, assemblyName);
        builder.SetTypeName(typeName);
        builder.SetTypeNamespace(type.ContainingNamespace.ToDisplayString(SymbolExtensions.FullNameTypeDisplayFormat));
        builder.SetTypeNameIdentifier(type.Name);
        builder.CaseSensitive = true;

        // This opts out this 'component' tag helper for any processing that's specific to the default
        // Razor ITagHelper runtime.
        builder.Metadata[TagHelperMetadata.Runtime.Name] = ComponentMetadata.Component.RuntimeName;

        if (type.IsGenericType)
        {
            builder.Metadata[ComponentMetadata.Component.GenericTypedKey] = bool.TrueString;

            var cascadeGenericTypeAttributes = type
                .GetAttributes()
                .Where(a => a.AttributeClass.HasFullName(ComponentsApi.CascadingTypeParameterAttribute.MetadataName))
                .Select(attribute => attribute.ConstructorArguments.FirstOrDefault().Value as string)
                .ToList();

            for (var i = 0; i < type.TypeArguments.Length; i++)
            {
                var typeParameter = type.TypeArguments[i] as ITypeParameterSymbol;
                if (typeParameter != null)
                {
                    var cascade = cascadeGenericTypeAttributes.Contains(typeParameter.Name, StringComparer.Ordinal);
                    CreateTypeParameterProperty(builder, typeParameter, cascade);
                }
            }
        }

        var xml = type.GetDocumentationCommentXml();
        if (!string.IsNullOrEmpty(xml))
        {
            builder.Documentation = xml;
        }

        foreach (var property in GetProperties(type))
        {
            if (property.kind == PropertyKind.Ignored)
            {
                continue;
            }

            CreateProperty(builder, property.property, property.kind);
        }

        if (builder.BoundAttributes.Any(a => a.IsParameterizedChildContentProperty()) &&
            !builder.BoundAttributes.Any(a => string.Equals(a.Name, ComponentMetadata.ChildContent.ParameterAttributeName, StringComparison.OrdinalIgnoreCase)))
        {
            // If we have any parameterized child content parameters, synthesize a 'Context' parameter to be
            // able to set the variable name (for all child content). If the developer defined a 'Context' parameter
            // already, then theirs wins.
            CreateContextParameter(builder, childContentName: null);
        }

        return builder;
    }

    private void CreateProperty(TagHelperDescriptorBuilder builder, IPropertySymbol property, PropertyKind kind)
    {
        builder.BindAttribute(pb =>
        {
            pb.Name = property.Name;
            pb.TypeName = property.Type.ToDisplayString(SymbolExtensions.FullNameTypeDisplayFormat);
            pb.SetPropertyName(property.Name);
            pb.IsEditorRequired = property.GetAttributes().Any(a => a.AttributeClass.HasFullName("Microsoft.AspNetCore.Components.EditorRequiredAttribute"));
            pb.SetGloballyQualifiedTypeName(property.Type.ToDisplayString(GloballyQualifiedFullNameTypeDisplayFormat));
            if (kind == PropertyKind.Enum)
            {
                pb.IsEnum = true;
            }

            if (kind == PropertyKind.ChildContent)
            {
                pb.Metadata.Add(ComponentMetadata.Component.ChildContentKey, bool.TrueString);
            }

            if (kind == PropertyKind.EventCallback)
            {
                pb.Metadata.Add(ComponentMetadata.Component.EventCallbackKey, bool.TrueString);
            }

            if (kind == PropertyKind.Delegate)
            {
                pb.Metadata.Add(ComponentMetadata.Component.DelegateSignatureKey, bool.TrueString);
                pb.Metadata.Add(ComponentMetadata.Component.DelegateWithAwaitableResultKey, IsAwaitable(property));
            }

            if (HasTypeParameter(property.Type))
            {
                pb.Metadata.Add(ComponentMetadata.Component.GenericTypedKey, bool.TrueString);
            }

            var xml = property.GetDocumentationCommentXml();
            if (!string.IsNullOrEmpty(xml))
            {
                pb.Documentation = xml;
            }
        });

        bool HasTypeParameter(ITypeSymbol type)
        {
            if (type is ITypeParameterSymbol)
            {
                return true;
            }

            // We need to check for cases like:
            // [Parameter] public List<T> MyProperty { get; set; }
            // AND
            // [Parameter] public List<string> MyProperty { get; set; }
            //
            // We need to inspect the type arguments to tell the difference between a property that
            // uses the containing class' type parameter(s) and a vanilla usage of generic types like
            // List<> and Dictionary<,>
            //
            // Since we need to handle cases like RenderFragment<List<T>>, this check must be recursive.
            if (type is INamedTypeSymbol namedType && namedType.IsGenericType)
            {
                var typeArguments = namedType.TypeArguments;
                for (var i = 0; i < typeArguments.Length; i++)
                {
                    if (HasTypeParameter(typeArguments[i]))
                    {
                        return true;
                    }
                }

                // Another case to handle - if the type being inspected is a nested type
                // inside a generic containing class. The common usage for this would be a case
                // where a generic templated component defines a 'context' nested class.
                if (namedType.ContainingType != null && HasTypeParameter(namedType.ContainingType))
                {
                    return true;
                }
            }
            // Also check for cases like:
            // [Parameter] public T[] MyProperty { get; set; }
            else if (type is IArrayTypeSymbol array && HasTypeParameter(array.ElementType))
            {
                return true;
            }

            return false;
        }
    }

    private static string IsAwaitable(IPropertySymbol prop)
    {
        var methodSymbol = ((INamedTypeSymbol)prop.Type).DelegateInvokeMethod;
        if (methodSymbol.ReturnsVoid)
        {
            return bool.FalseString;
        }
        else
        {
            var members = methodSymbol.ReturnType.GetMembers();
            for (var i = 0; i < members.Length; i++)
            {
                var candidate = members[i];
                if (candidate is not IMethodSymbol method || !string.Equals(candidate.Name, "GetAwaiter", StringComparison.Ordinal))
                {
                    continue;
                }
                if (!VerifyGetAwaiter(method))
                {
                    continue;
                }

                return bool.TrueString;
            }
            return methodSymbol.IsAsync ? bool.TrueString : bool.FalseString;

            static bool VerifyGetAwaiter(IMethodSymbol getAwaiter)
            {
                var returnType = getAwaiter.ReturnType;
                if (returnType == null)
                {
                    return false;
                }

                if (!returnType.GetMembers().OfType<IPropertySymbol>().Any(p => p.Name == WellKnownMemberNames.IsCompleted && p.Type.SpecialType == SpecialType.System_Boolean && p.GetMethod != null))
                {
                    return false;
                }

                var methods = returnType.GetMembers().OfType<IMethodSymbol>();

                if (!methods.Any(x => x.Name == WellKnownMemberNames.OnCompleted && x.ReturnsVoid && x.Parameters.Length == 1 && x.Parameters.First().Type.TypeKind == TypeKind.Delegate))
                {
                    return false;
                }

                return methods.Any(m => m.Name == WellKnownMemberNames.GetResult && !m.Parameters.Any());
            }
        }
    }

    private void CreateTypeParameterProperty(TagHelperDescriptorBuilder builder, ITypeParameterSymbol typeParameter, bool cascade)
    {
        builder.BindAttribute(pb =>
        {
            pb.DisplayName = typeParameter.Name;
            pb.Name = typeParameter.Name;
            pb.TypeName = typeof(Type).FullName;
            pb.SetPropertyName(typeParameter.Name);

            pb.Metadata[ComponentMetadata.Component.TypeParameterKey] = bool.TrueString;
            pb.Metadata[ComponentMetadata.Component.TypeParameterIsCascadingKey] = cascade.ToString();

            // Type constraints (like "Image" or "Foo") are stored independently of
            // things like constructor constraints and not null constraints in the
            // type parameter so we create a single string representation of all the constraints
            // here.
            var list = new List<string>();

            // CS0449: The 'class', 'struct', 'unmanaged', 'notnull', and 'default' constraints
            // cannot be combined or duplicated, and must be specified first in the constraints list.
            if (typeParameter.HasReferenceTypeConstraint)
            {
                list.Add("class");
            }

            if (typeParameter.HasNotNullConstraint)
            {
                list.Add("notnull");
            }

            if (typeParameter.HasUnmanagedTypeConstraint)
            {
                list.Add("unmanaged");
            }
            else if (typeParameter.HasValueTypeConstraint)
            {
                // `HasValueTypeConstraint` is also true when `unmanaged` constraint is present.
                list.Add("struct");
            }

            for (var i = 0; i < typeParameter.ConstraintTypes.Length; i++)
            {
                var constraintType = typeParameter.ConstraintTypes[i];
                list.Add(constraintType.ToDisplayString(GloballyQualifiedFullNameTypeDisplayFormat));
            }

            // CS0401: The new() constraint must be the last constraint specified.
            if (typeParameter.HasConstructorConstraint)
            {
                list.Add("new()");
            }

            if (list.Count > 0)
            {
                pb.Metadata[ComponentMetadata.Component.TypeParameterConstraintsKey] = $"where {typeParameter.Name} : {string.Join(", ", list)}";
            }

            pb.Documentation = string.Format(CultureInfo.InvariantCulture, ComponentResources.ComponentTypeParameter_Documentation, typeParameter.Name, builder.Name);
        });
    }

    private TagHelperDescriptor CreateChildContentDescriptor(TagHelperDescriptor component, BoundAttributeDescriptor attribute)
    {
        var typeName = component.GetTypeName() + "." + attribute.Name;
        var assemblyName = component.AssemblyName;

        var builder = TagHelperDescriptorBuilder.Create(ComponentMetadata.ChildContent.TagHelperKind, typeName, assemblyName);
        builder.SetTypeName(typeName);
        builder.SetTypeNamespace(component.GetTypeNamespace());
        builder.SetTypeNameIdentifier(component.GetTypeNameIdentifier());
        builder.CaseSensitive = true;

        // This opts out this 'component' tag helper for any processing that's specific to the default
        // Razor ITagHelper runtime.
        builder.Metadata[TagHelperMetadata.Runtime.Name] = ComponentMetadata.ChildContent.RuntimeName;

        // Opt out of processing as a component. We'll process this specially as part of the component's body.
        builder.Metadata[ComponentMetadata.SpecialKindKey] = ComponentMetadata.ChildContent.TagHelperKind;

        var xml = attribute.Documentation;
        if (!string.IsNullOrEmpty(xml))
        {
            builder.Documentation = xml;
        }

        // Child content matches the property name, but only as a direct child of the component.
        builder.TagMatchingRule(r =>
        {
            r.TagName = attribute.Name;
            r.ParentTag = component.TagMatchingRules[0].TagName;
        });

        if (attribute.IsParameterizedChildContentProperty())
        {
            // For child content attributes with a parameter, synthesize an attribute that allows you to name
            // the parameter.
            CreateContextParameter(builder, attribute.Name);
        }

        if (component.IsComponentFullyQualifiedNameMatch())
        {
            builder.Metadata[ComponentMetadata.Component.NameMatchKey] = ComponentMetadata.Component.FullyQualifiedNameMatch;
        }

        var descriptor = builder.Build();

        return descriptor;
    }

    private void CreateContextParameter(TagHelperDescriptorBuilder builder, string childContentName)
    {
        builder.BindAttribute(b =>
        {
            b.Name = ComponentMetadata.ChildContent.ParameterAttributeName;
            b.TypeName = typeof(string).FullName;
            b.Metadata.Add(ComponentMetadata.Component.ChildContentParameterNameKey, bool.TrueString);
            b.Metadata.Add(TagHelperMetadata.Common.PropertyName, b.Name);

            if (childContentName == null)
            {
                b.Documentation = ComponentResources.ChildContentParameterName_TopLevelDocumentation;
            }
            else
            {
                b.Documentation = string.Format(CultureInfo.InvariantCulture, ComponentResources.ChildContentParameterName_Documentation, childContentName);
            }
        });
    }

    // Does a walk up the inheritance chain to determine the set of parameters by using
    // a dictionary keyed on property name.
    //
    // We consider parameters to be defined by properties satisfying all of the following:
    // - are public
    // - are visible (not shadowed)
    // - have the [Parameter] attribute
    // - have a setter, even if private
    // - are not indexers
    private IEnumerable<(IPropertySymbol property, PropertyKind kind)> GetProperties(INamedTypeSymbol type)
    {
        var properties = new Dictionary<string, (IPropertySymbol, PropertyKind)>(StringComparer.Ordinal);
        do
        {
            if (type.HasFullName(ComponentsApi.ComponentBase.MetadataName))
            {
                // The ComponentBase base class doesn't have any [Parameter].
                // Bail out now to avoid walking through its many members, plus the members
                // of the System.Object base class.
                break;
            }

            var members = type.GetMembers();
            for (var i = 0; i < members.Length; i++)
            {
                var property = members[i] as IPropertySymbol;
                if (property == null)
                {
                    // Not a property
                    continue;
                }

                if (properties.ContainsKey(property.Name))
                {
                    // Not visible
                    continue;
                }

                var kind = PropertyKind.Default;
                if (property.DeclaredAccessibility != Accessibility.Public)
                {
                    // Not public
                    kind = PropertyKind.Ignored;
                }

                if (property.Parameters.Length != 0)
                {
                    // Indexer
                    kind = PropertyKind.Ignored;
                }

                if (property.SetMethod == null)
                {
                    // No setter
                    kind = PropertyKind.Ignored;
                }
                else if (property.SetMethod.DeclaredAccessibility != Accessibility.Public)
                {
                    // No public setter
                    kind = PropertyKind.Ignored;
                }

                if (property.IsStatic)
                {
                    kind = PropertyKind.Ignored;
                }

                if (!property.GetAttributes().Any(a => a.AttributeClass.HasFullName(ComponentsApi.ParameterAttribute.MetadataName)))
                {
                    if (property.IsOverride)
                    {
                        // This property does not contain [Parameter] attribute but it was overridden. Don't ignore it for now.
                        // We can ignore it if the base class does not contains a [Parameter] as well.
                        continue;
                    }

                    // Does not have [Parameter]
                    kind = PropertyKind.Ignored;
                }

                if (kind == PropertyKind.Default && property.Type.TypeKind == TypeKind.Enum)
                {
                    kind = PropertyKind.Enum;
                }

                if (kind == PropertyKind.Default && property.Type.HasFullName(ComponentsApi.RenderFragment.MetadataName))
                {
                    kind = PropertyKind.ChildContent;
                }

                if (kind == PropertyKind.Default &&
                    property.Type is INamedTypeSymbol namedType &&
                    namedType.IsGenericType &&
                    namedType.ConstructedFrom.HasFullName(ComponentsApi.RenderFragmentOfT.DisplayName))
                {
                    kind = PropertyKind.ChildContent;
                }

                if (kind == PropertyKind.Default && property.Type.HasFullName(ComponentsApi.EventCallback.MetadataName))
                {
                    kind = PropertyKind.EventCallback;
                }

                if (kind == PropertyKind.Default &&
                    property.Type is INamedTypeSymbol namedType2 &&
                    namedType2.IsGenericType &&
                    namedType2.ConstructedFrom.HasFullName(ComponentsApi.EventCallbackOfT.DisplayName))
                {
                    kind = PropertyKind.EventCallback;
                }

                if (kind == PropertyKind.Default && property.Type.TypeKind == TypeKind.Delegate)
                {
                    kind = PropertyKind.Delegate;
                }

                properties.Add(property.Name, (property, kind));
            }

            type = type.BaseType;
        }
        while (type != null);

        return properties.Values;
    }

    private enum PropertyKind
    {
        Ignored,
        Default,
        Enum,
        ChildContent,
        Delegate,
        EventCallback,
    }

    private class ComponentTypeVisitor : SymbolVisitor
    {
        private readonly List<INamedTypeSymbol> _results;

        public ComponentTypeVisitor(List<INamedTypeSymbol> results)
        {
            _results = results;
        }

        public override void VisitNamedType(INamedTypeSymbol symbol)
        {
            if (ComponentDetectionConventions.IsComponent(symbol, ComponentsApi.IComponent.MetadataName))
            {
                _results.Add(symbol);
            }
        }

        public override void VisitNamespace(INamespaceSymbol symbol)
        {
            foreach (var member in symbol.GetMembers())
            {
                Visit(member);
            }
        }

        public override void VisitAssembly(IAssemblySymbol symbol)
        {
            // This as a simple yet high-value optimization that excludes the vast majority of
            // assemblies that (by definition) can't contain a component.
            if (symbol.Name != null && !symbol.Name.StartsWith("System.", StringComparison.Ordinal))
            {
                Visit(symbol.GlobalNamespace);
            }
        }
    }
}
