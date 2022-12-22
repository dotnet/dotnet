// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Cci;
using Microsoft.Cci.Extensions;
using Microsoft.Cci.Mappings;
using Microsoft.Cci.Writers;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.DotNet.AsmDiff
{
    internal sealed class ApiRecordingCSharpDiffWriter : DiffCSharpWriter, ICciDifferenceWriter
    {
        private DiffRecorder _diffRecorder;
        private MappingSettings _settings;
        private List<DiffApiDefinition> _apis = new List<DiffApiDefinition>();
        private Stack<List<DiffApiDefinition>> _apiStack = new Stack<List<DiffApiDefinition>>();
        private Stack<DiffApiDefinition> _apiDefinitionStack = new Stack<DiffApiDefinition>();

        public ApiRecordingCSharpDiffWriter(DiffRecorder diffRecorder, MappingSettings settings, bool includePseudoCustomAttributes)
            : base(diffRecorder, settings, Enumerable.Empty<DiffComment>(), includePseudoCustomAttributes)
        {
            _diffRecorder = diffRecorder;
            _settings = settings;
        }

        public List<DiffApiDefinition> ApiDefinitions
        {
            get { return _apis.ToList(); }
        }

        public new void Write(string oldAssembliesName, IEnumerable<IAssembly> oldAssemblies, string newAssembliesName, IEnumerable<IAssembly> newAssemblies)
        {
            AssemblySetMapping mapping;
            if (!string.IsNullOrEmpty(newAssembliesName))
            {
                _settings.ElementCount = 2;
                mapping = new AssemblySetMapping(_settings);
                mapping.AddMappings(oldAssemblies, newAssemblies);
            }
            else
            {
                _settings.ElementCount = 1;
                mapping = new AssemblySetMapping(_settings);
                mapping.AddMapping(0, oldAssemblies);
            }
            Visit(mapping);
        }

        private void PushApi<T>(ElementMapping<T> elementMapping)
            where T : class, IDefinition
        {
            var left = elementMapping[0];
            var right = elementMapping.ElementCount == 1
                            ? null
                            : elementMapping[1];

            var difference = elementMapping.Difference;

            var newChildren = new List<DiffApiDefinition>();
            var apiDefinition = new DiffApiDefinition(left, right, difference, newChildren)
            {
                StartLine = _diffRecorder.Line
            };
            _apis.Add(apiDefinition);

            _apiStack.Push(_apis);
            _apiDefinitionStack.Push(apiDefinition);
            _apis = newChildren;
        }

        private void PopApi()
        {
            var currentApi = _apiDefinitionStack.Pop();
            currentApi.EndLine = _diffRecorder.Line - 1;

            _apis = _apiStack.Pop();
        }

        public override void Visit(AssemblyMapping assembly)
        {
            _diffRecorder.CancellationToken.ThrowIfCancellationRequested();

            PushApi(assembly);
            base.Visit(assembly);
            PopApi();
        }

        public override void Visit(NamespaceMapping ns)
        {
            _diffRecorder.CancellationToken.ThrowIfCancellationRequested();

            PushApi(ns);
            base.Visit(ns);
            PopApi();
        }

        public override void Visit(TypeMapping type)
        {
            _diffRecorder.CancellationToken.ThrowIfCancellationRequested();

            PushApi(type);
            base.Visit(type);
            PopApi();
        }

        public override void Visit(MemberMapping member)
        {
            _diffRecorder.CancellationToken.ThrowIfCancellationRequested();

            var shouldVisit = !IsPropertyOrEventAccessor(member.Representative) &&
                              !IsEnumValueField(member.Representative) &&
                              !IsDelegateMember(member.Representative);

            if (shouldVisit)
                PushApi(member);

            base.Visit(member);

            if (shouldVisit)
                PopApi();
        }

        private static bool IsPropertyOrEventAccessor(ITypeDefinitionMember representative)
        {
            var methodDefinition = representative as IMethodDefinition;
            if (methodDefinition == null)
                return false;

            return methodDefinition.IsPropertyOrEventAccessor();
        }

        private static bool IsEnumValueField(ITypeDefinitionMember representative)
        {
            var isEnumMember = representative.ContainingTypeDefinition.IsEnum;
            if (!isEnumMember)
                return false;

            return representative.Name.Value == "value__";
        }

        private static bool IsDelegateMember(ITypeDefinitionMember representative)
        {
            return representative.ContainingTypeDefinition.IsDelegate;
        }
    }
}
