// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using ILLink.RoslynAnalyzer.DataFlow;
using ILLink.Shared;
using ILLink.Shared.TrimAnalysis;
using Microsoft.CodeAnalysis;

namespace ILLink.RoslynAnalyzer.TrimAnalysis
{
    internal readonly struct ReflectionAccessAnalyzer
    {
        private readonly Action<Diagnostic>? _reportDiagnostic;
        private readonly INamedTypeSymbol? _typeHierarchyType;

        public ReflectionAccessAnalyzer(Action<Diagnostic>? reportDiagnostic, INamedTypeSymbol? typeHierarchyType)
        {
            _reportDiagnostic = reportDiagnostic;
            _typeHierarchyType = typeHierarchyType;
        }

#pragma warning disable CA1822 // Mark members as static - the other partial implementations might need to be instance methods
        internal void GetReflectionAccessDiagnostics(Location location, ITypeSymbol typeSymbol, DynamicallyAccessedMemberTypes requiredMemberTypes, bool declaredOnly = false)
        {
            typeSymbol = typeSymbol.OriginalDefinition;
            foreach (var member in typeSymbol.GetDynamicallyAccessedMembers(requiredMemberTypes, declaredOnly))
            {
                switch (member)
                {
                    case IMethodSymbol method:
                        GetReflectionAccessDiagnosticsForMethod(location, method);
                        break;
                    case IFieldSymbol field:
                        GetDiagnosticsForField(location, field);
                        break;
                    case IPropertySymbol property:
                        GetReflectionAccessDiagnosticsForProperty(location, property);
                        break;
                    /* Skip Type and InterfaceImplementation marking since doesnt seem relevant for diagnostic generation
                    case ITypeSymbol nestedType:
                        MarkType(location, nestedType);
                        break;
                    case InterfaceImplementation interfaceImplementation:
                        MarkInterfaceImplementation(location, interfaceImplementation, dependencyKind);
                        break;
                    */
                    case IEventSymbol @event:
                        GetDiagnosticsForEvent(location, @event);
                        break;
                }
            }
        }

        internal void GetReflectionAccessDiagnosticsForEventsOnTypeHierarchy(Location location, ITypeSymbol typeSymbol, string name, BindingFlags? bindingFlags)
        {
            foreach (var @event in typeSymbol.GetEventsOnTypeHierarchy(e => e.Name == name, bindingFlags))
                GetDiagnosticsForEvent(location, @event);
        }

        internal void GetReflectionAccessDiagnosticsForFieldsOnTypeHierarchy(Location location, ITypeSymbol typeSymbol, string name, BindingFlags? bindingFlags)
        {
            foreach (var field in typeSymbol.GetFieldsOnTypeHierarchy(f => f.Name == name, bindingFlags))
                GetDiagnosticsForField(location, field);
        }

        internal void GetReflectionAccessDiagnosticsForPropertiesOnTypeHierarchy(Location location, ITypeSymbol typeSymbol, string name, BindingFlags? bindingFlags)
        {
            foreach (var prop in typeSymbol.GetPropertiesOnTypeHierarchy(p => p.Name == name, bindingFlags))
                GetReflectionAccessDiagnosticsForProperty(location, prop);
        }

        internal void GetReflectionAccessDiagnosticsForConstructorsOnType(Location location, ITypeSymbol typeSymbol, BindingFlags? bindingFlags, int? parameterCount)
        {
            foreach (var c in typeSymbol.GetConstructorsOnType(filter: parameterCount.HasValue ? c => c.Parameters.Length == parameterCount.Value : null, bindingFlags: bindingFlags))
                GetReflectionAccessDiagnosticsForMethod(location, c);
        }

        internal void GetReflectionAccessDiagnosticsForPublicParameterlessConstructor(Location location, ITypeSymbol typeSymbol)
        {
            foreach (var c in typeSymbol.GetConstructorsOnType(filter: m => (m.DeclaredAccessibility == Accessibility.Public) && m.Parameters.Length == 0))
                GetReflectionAccessDiagnosticsForMethod(location, c);
        }

        private void ReportRequiresUnreferencedCodeDiagnostic(Location location, AttributeData requiresAttributeData, ISymbol member)
        {
            var message = RequiresUnreferencedCodeUtils.GetMessageFromAttribute(requiresAttributeData);
            var url = RequiresAnalyzerBase.GetUrlFromAttribute(requiresAttributeData);
            var diagnosticContext = new DiagnosticContext(location, _reportDiagnostic);
            diagnosticContext.AddDiagnostic(DiagnosticId.RequiresUnreferencedCode, member.GetDisplayName(), message, url);
        }

        internal void GetReflectionAccessDiagnosticsForMethod(Location location, IMethodSymbol methodSymbol)
        {
            if (_typeHierarchyType is not null)
            {
                GetTypeHierarchyReflectionAccessDiagnostics(location, methodSymbol);
                return;
            }

            if (methodSymbol.IsInRequiresUnreferencedCodeAttributeScope(out var requiresUnreferencedCodeAttributeData))
            {
                ReportRequiresUnreferencedCodeDiagnostic(location, requiresUnreferencedCodeAttributeData, methodSymbol);
            }
            else
            {
                GetDiagnosticsForReflectionAccessToDAMOnMethod(location, methodSymbol);
            }
        }

        internal void GetTypeHierarchyReflectionAccessDiagnostics(Location location, ISymbol member)
        {
            Debug.Assert(member is IMethodSymbol or IFieldSymbol);

            // Don't check whether the current scope is a RUC type or RUC method because these warnings
            // are not suppressed in RUC scopes. Here the scope represents the DynamicallyAccessedMembers
            // annotation on a type, not a callsite which uses the annotation. We always want to warn about
            // possible reflection access indicated by these annotations.

            Debug.Assert(_typeHierarchyType is not null);

            static bool IsDeclaredWithinType(ISymbol member, INamedTypeSymbol type)
            {
                INamedTypeSymbol containingType = member.ContainingType;
                while (containingType is not null)
                {
                    if (SymbolEqualityComparer.Default.Equals(containingType, type))
                        return true;

                    containingType = containingType.ContainingType;
                }
                return false;
            }

            var reportOnMember = IsDeclaredWithinType(member, _typeHierarchyType!);
            if (reportOnMember)
                location = DynamicallyAccessedMembersAnalyzer.GetPrimaryLocation(member.Locations);

            var diagnosticContext = new DiagnosticContext(location, _reportDiagnostic);

            if (member.IsInRequiresUnreferencedCodeAttributeScope(out AttributeData? requiresUnreferencedCodeAttribute))
            {
                var id = reportOnMember ? DiagnosticId.DynamicallyAccessedMembersOnTypeReferencesMemberWithRequiresUnreferencedCode : DiagnosticId.DynamicallyAccessedMembersOnTypeReferencesMemberOnBaseWithRequiresUnreferencedCode;
                diagnosticContext.AddDiagnostic(id, _typeHierarchyType!.GetDisplayName(),
                    member.GetDisplayName(),
                    MessageFormat.FormatRequiresAttributeMessageArg(RequiresUnreferencedCodeUtils.GetMessageFromAttribute(requiresUnreferencedCodeAttribute)),
                    MessageFormat.FormatRequiresAttributeMessageArg(RequiresAnalyzerBase.GetUrlFromAttribute(requiresUnreferencedCodeAttribute)));
            }

            if (FlowAnnotations.ShouldWarnWhenAccessedForReflection(member))
            {
                var id = reportOnMember ? DiagnosticId.DynamicallyAccessedMembersOnTypeReferencesMemberWithDynamicallyAccessedMembers : DiagnosticId.DynamicallyAccessedMembersOnTypeReferencesMemberOnBaseWithDynamicallyAccessedMembers;
                diagnosticContext.AddDiagnostic(id, _typeHierarchyType!.GetDisplayName(), member.GetDisplayName());
            }
        }

        internal void GetDiagnosticsForReflectionAccessToDAMOnMethod(Location location, IMethodSymbol methodSymbol)
        {
            var diagnosticContext = new DiagnosticContext(location, _reportDiagnostic);
            if (methodSymbol.IsVirtual && FlowAnnotations.GetMethodReturnValueAnnotation(methodSymbol) != DynamicallyAccessedMemberTypes.None)
            {
                diagnosticContext.AddDiagnostic(DiagnosticId.DynamicallyAccessedMembersMethodAccessedViaReflection, methodSymbol.GetDisplayName());
            }
            else
            {
                foreach (var parameter in methodSymbol.GetParameters())
                {
                    if (FlowAnnotations.GetMethodParameterAnnotation(parameter) != DynamicallyAccessedMemberTypes.None)
                    {
                        diagnosticContext.AddDiagnostic(DiagnosticId.DynamicallyAccessedMembersMethodAccessedViaReflection, methodSymbol.GetDisplayName());
                        break;
                    }
                }
            }
        }

        internal void GetReflectionAccessDiagnosticsForProperty(Location location, IPropertySymbol propertySymbol)
        {
            if (propertySymbol.SetMethod is not null)
                GetReflectionAccessDiagnosticsForMethod(location, propertySymbol.SetMethod);
            if (propertySymbol.GetMethod is not null)
                GetReflectionAccessDiagnosticsForMethod(location, propertySymbol.GetMethod);
        }

        private void GetDiagnosticsForEvent(Location location, IEventSymbol eventSymbol)
        {
            if (eventSymbol.AddMethod is not null)
                GetReflectionAccessDiagnosticsForMethod(location, eventSymbol.AddMethod);
            if (eventSymbol.RemoveMethod is not null)
                GetReflectionAccessDiagnosticsForMethod(location, eventSymbol.RemoveMethod);
            if (eventSymbol.RaiseMethod is not null)
                GetReflectionAccessDiagnosticsForMethod(location, eventSymbol.RaiseMethod);
        }

        private void GetDiagnosticsForField(Location location, IFieldSymbol fieldSymbol)
        {
            if (_typeHierarchyType is not null)
            {
                GetTypeHierarchyReflectionAccessDiagnostics(location, fieldSymbol);
                return;
            }

            if (fieldSymbol.TryGetRequiresUnreferencedCodeAttribute(out var requiresUnreferencedCodeAttributeData))
                ReportRequiresUnreferencedCodeDiagnostic(location, requiresUnreferencedCodeAttributeData, fieldSymbol);

            if (FlowAnnotations.GetFieldAnnotation(fieldSymbol) != DynamicallyAccessedMemberTypes.None)
            {
                var diagnosticContext = new DiagnosticContext(location, _reportDiagnostic);
                diagnosticContext.AddDiagnostic(DiagnosticId.DynamicallyAccessedMembersFieldAccessedViaReflection, fieldSymbol.GetDisplayName());
            }
        }
    }
}
