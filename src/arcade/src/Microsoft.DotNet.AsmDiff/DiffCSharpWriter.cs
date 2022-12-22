// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Composition.Hosting;
using System.Composition.Hosting.Core;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using Microsoft.Cci;
using Microsoft.Cci.Comparers;
using Microsoft.Cci.Differs;
using Microsoft.Cci.Extensions;
using Microsoft.Cci.Extensions.CSharp;
using Microsoft.Cci.Mappings;
using Microsoft.Cci.Traversers;
using Microsoft.Cci.Writers;
using Microsoft.Cci.Writers.CSharp;
using Microsoft.Cci.Writers.Syntax;
using Assembly = System.Reflection.Assembly;

namespace Microsoft.DotNet.AsmDiff
{
    public class DiffCSharpWriter : MappingsTypeMemberTraverser, IDiffingService, ICciDifferenceWriter
    {
        private readonly IStyleSyntaxWriter _syntaxWriter;
        private readonly MappingSettings _settings;
        private readonly ICciDeclarationWriter _formatter;
        private readonly CSDeclarationHelper _declHelper;
        private bool _firstMemberGroup = false;
        private readonly IEnumerable<DiffComment> _diffComments;

        public DiffCSharpWriter(IStyleSyntaxWriter writer, MappingSettings settings, IEnumerable<DiffComment> diffComments)
            : this(writer, settings, diffComments, includePseudoCustomAttributes:false)
        {
        }

        public DiffCSharpWriter(IStyleSyntaxWriter writer, MappingSettings settings, IEnumerable<DiffComment> diffComments, bool includePseudoCustomAttributes)
            : base (settings)
        {
            _syntaxWriter = writer;
            _settings = InitializeSettings(settings);
            _formatter = new CSDeclarationWriter(_syntaxWriter, _settings.Filter, forCompilation: false, includePseudoCustomAttributes: includePseudoCustomAttributes)
            {
                LangVersion = CSDeclarationWriter.LangVersionPreview
            };
            _declHelper = new CSDeclarationHelper(_settings.Filter, forCompilation: false, includePseudoCustomAttributes: includePseudoCustomAttributes);
            _diffComments = diffComments ?? Enumerable.Empty<DiffComment>();
        }

        public DiffCSharpWriter(IStyleSyntaxWriter writer, MappingSettings settings)
            : this(writer, settings, null, false)
        {          
        }

        public bool IncludeSpaceBetweenMemberGroups { get; set; }

        public bool IncludeMemberGroupHeadings { get; set; }

        public bool HighlightBaseMembers { get; set; }

        public bool IncludeAssemblyProperties { get; set; }

        public void Write(string oldAssembliesName, IEnumerable<IAssembly> oldAssemblies, string newAssembliesName, IEnumerable<IAssembly> newAssemblies)
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

            if (oldAssembliesName != null)
            {
                using (_syntaxWriter.StartStyle(SyntaxStyle.Removed))
                    _syntaxWriter.Write("{0}", oldAssembliesName);
                _syntaxWriter.WriteLine();
            }

            if (newAssembliesName != null)
            {
                using (_syntaxWriter.StartStyle(SyntaxStyle.Added))
                    _syntaxWriter.Write("{0}", newAssembliesName);
                _syntaxWriter.WriteLine();
            }

            Visit(mapping);
        }

        public override void Visit(AssemblySetMapping assemblySet)
        {
            if (this.IncludeAssemblyProperties && assemblySet.ElementCount > 1)
            {
                foreach (var assembly in assemblySet.Assemblies)
                {
                    var attributes = assembly.Attributes.Where(e => DiffFilter.Include(e.Difference));
                    var properties = assembly.Properties.Where(e => DiffFilter.Include(e.Difference));

                    if (!attributes.Any() && !properties.Any())
                        continue;

                    _syntaxWriter.WriteKeyword("assembly");
                    _syntaxWriter.WriteSpace();
                    _syntaxWriter.WriteIdentifier(assembly.Representative.Name.Value);
                    using (_syntaxWriter.StartBraceBlock())
                    {
                        Visit(properties);
                        Visit(attributes);
                    }
                }
            }
            base.Visit(assemblySet);
        }

        public override void Visit(AssemblyMapping assembly)
        {
            Contract.Assert(this.Settings.GroupByAssembly);

            _syntaxWriter.WriteKeyword("assembly");
            _syntaxWriter.WriteSpace();
            _syntaxWriter.WriteIdentifier(assembly.Representative.Name.Value);
            using (_syntaxWriter.StartBraceBlock())
            {
                if (this.IncludeAssemblyProperties)
                {
                    var attributes = assembly.Attributes.Where(e => DiffFilter.Include(e.Difference));
                    var properties = assembly.Properties.Where(e => DiffFilter.Include(e.Difference));

                    Visit(properties);
                    Visit(attributes);
                }
                base.Visit(assembly);
            }
        }

        public override void Visit(NamespaceMapping mapping)
        {
            WriteHeader(mapping);
            using (_syntaxWriter.StartBraceBlock())
            {
                WriteComments(mapping);
                base.Visit(mapping);
            }

            _syntaxWriter.WriteLine();
        }

        public override void Visit(IEnumerable<TypeMapping> types)
        {
            TypeMapping mapping = types.FirstOrDefault(DiffFilter.Include);

            // Need to handle the nested types like members
            if (mapping != null)
                WriteMemberGroupHeader(mapping.Representative as ITypeDefinitionMember);

            base.Visit(types);
        }

        public override void Visit(TypeMapping mapping)
        {
            WriteHeader(mapping);

            if (mapping.ShouldDiffMembers && !mapping.Representative.IsDelegate)
            {
                using (_syntaxWriter.StartBraceBlock())
                {
                    WriteComments(mapping);
                    _firstMemberGroup = true;
                    base.Visit(mapping);
                }
            }
            _syntaxWriter.WriteLine();
        }

        public override void Visit(IEnumerable<MemberMapping> members)
        {
            MemberMapping mapping = members.FirstOrDefault(m => DiffFilter.Include(m) && !IsPropertyOrEventAccessor(m.Representative));

            if (mapping != null)
                WriteMemberGroupHeader(mapping.Representative);

            base.Visit(members);
        }

        public override void Visit(MemberMapping member)
        {
            if (!IsPropertyOrEventAccessor(member.Representative))
            {
                IDisposable style = null;

                if (this.HighlightBaseMembers)
                {
                    if (member.Representative.IsInterfaceImplementation())
                        style = _syntaxWriter.StartStyle(SyntaxStyle.InterfaceMember);
                    else if (member.Representative.IsOverride())
                        style = _syntaxWriter.StartStyle(SyntaxStyle.InheritedMember);
                }

                WriteHeader(member);
                    style?.Dispose();

                _syntaxWriter.WriteLine();
                WriteComments(member);
            }

            base.Visit(member);
        }

        private static bool IsPropertyOrEventAccessor(ITypeDefinitionMember representative)
        {
            return (representative is IMethodDefinition methodDefinition) && methodDefinition.IsPropertyOrEventAccessor();
        }

        private void WriteHeader<T>(ElementMapping<T> element) where T : class, IDefinition
        {
            WriteElement(element,
                e => _formatter.WriteDeclaration(e),
                (e1, e2) => WriteMergedDefinitions(e1, e2));
        }

        private void WriteComments<TElement>(ElementMapping<TElement> mapping) where TElement : class
        {
            var docId = GetDocId(mapping);
            var commentSet = _diffComments.Where((c) => c.DocId == docId).Reverse().ToArray();
            var reviewCommentWriter = _syntaxWriter as IReviewCommentWriter;
            if (commentSet.Any() && reviewCommentWriter != null)
            {
                foreach (var comment in commentSet)
                {
                    reviewCommentWriter.WriteReviewComment(comment.Author, comment.Text);
                    _syntaxWriter.WriteLine();    
                }
            }
        }

        public string GetDocId<TElement>(ElementMapping<TElement> mapping) where TElement : class
        {
            var namespaceMapping = mapping as NamespaceMapping;
            if (namespaceMapping != null)
                return namespaceMapping.Representative.DocId();

            var typeMapping = mapping as TypeMapping;
            if (typeMapping != null)
                return typeMapping.Representative.DocId();

            var memberMapping = mapping as MemberMapping;
            if (memberMapping != null)
                return memberMapping.Representative.DocId();

            var assemblyMapping = mapping as AssemblyMapping;
            if (assemblyMapping != null)
                return assemblyMapping.Representative.DocId();

            return string.Empty;
        }

        private void WriteElement<T>(ElementMapping<T> element, Action<T> write, System.Action<T, T> writeMerged, bool writeNewLine = false) where T : class
        {
            switch (element.Difference)
            {
                case DifferenceType.Unchanged:
                    write(element.Representative);
                    break;

                case DifferenceType.Added:
                    Contract.Assert(element[1] != null);
                    using (_syntaxWriter.StartStyle(SyntaxStyle.Added))
                        write(element[1]);
                    break;

                case DifferenceType.Removed:
                    Contract.Assert(element[0] != null);
                    using (_syntaxWriter.StartStyle(SyntaxStyle.Removed))
                        write(element[0]);
                    break;

                case DifferenceType.Changed:

                    IDisposable style = null;
                    if (!element.Differences.ContainsIncompatibleDifferences())
                        style = _syntaxWriter.StartStyle(SyntaxStyle.NotCompatible, element.Differences.OfType<IncompatibleDifference>());

                    writeMerged(element[0], element[1]);

                    if (style != null)
                        style.Dispose();
                    break;
            }

            if (writeNewLine)
            {
                _syntaxWriter.WriteLine();
                WriteComments(element);
                _syntaxWriter.WriteLine();
            }
        }

        private void WriteMergedDefinitions(IDefinition def1, IDefinition def2)
        {
            var merged = GetTokenDiff(def1, def2);

            IDisposable style = null;
            DifferenceType context = DifferenceType.Unchanged;
            foreach (var mt in merged)
            {
                if (context != mt.Item1 && style != null)
                {
                    style.Dispose();
                    style = null;
                }
                switch (mt.Item1)
                {
                    default:
                    case DifferenceType.Unchanged:
                        _syntaxWriter.WriteSyntaxToken(mt.Item2);
                        break;
                    case DifferenceType.Removed:
                        if (context != DifferenceType.Removed)
                            style = _syntaxWriter.StartStyle(SyntaxStyle.Removed);
                        _syntaxWriter.WriteSyntaxToken(mt.Item2);
                        break;
                    case DifferenceType.Added:
                        if (context != DifferenceType.Added)
                            style = _syntaxWriter.StartStyle(SyntaxStyle.Added);
                        _syntaxWriter.WriteSyntaxToken(mt.Item2);
                        break;
                }
                context = mt.Item1;
            }
            if (style != null)
                style.Dispose();
        }

        public IEnumerable<Tuple<DifferenceType, SyntaxToken>> GetTokenDiff(IDefinition def1, IDefinition def2)
        {
            SyntaxToken[] t1 = GetTokenList(def1).ToArray();
            SyntaxToken[] t2 = GetTokenList(def2).ToArray();

            int t1Start = 0;
            int t2Start = 0;

            List<Tuple<DifferenceType, SyntaxToken>> merged = new List<Tuple<DifferenceType, SyntaxToken>>();

            //TODO: Consider splitting by lines as well to help with the attribute diffing

            // Split the token list at identifiers to help with the merge line up so that we don't get large
            // diffing churn in the output.
            while (t1Start < t1.Length && t2Start < t2.Length)
            {
                int t1End = Array.FindIndex(t1, t1Start, s => s.Type == SyntaxTokenType.Identifier);
                int t2End = Array.FindIndex(t2, t2Start, s => s.Type == SyntaxTokenType.Identifier);

                if (t1End < 0 || t2End < 0)
                    break;

                merged.AddRange(ListMerger.Merge(t1, t1Start, t1End + 1, t2, t2Start, t2End + 1));

                t1Start = t1End + 1;
                t2Start = t2End + 1;
            }

            // Finish any leftover work from either side.
            merged.AddRange(ListMerger.Merge(t1, t1Start, t1.Length, t2, t2Start, t2.Length));
            return merged;
        }

        private MappingSettings InitializeSettings(MappingSettings settings)
        {
            Assembly entryAssembly = Assembly.GetEntryAssembly();
            string appDirectory = Path.GetDirectoryName(entryAssembly.Location);
            var assemblies = Directory.EnumerateFiles(appDirectory, "*.dll")
                                      .Select(Assembly.LoadFrom);

            var configuration = new ContainerConfiguration()
                               .WithAssembly(entryAssembly)
                               .WithAssemblies(assemblies)
                               .WithExport<IDiffingService>(this)
                               .WithExport<IEqualityComparer<ITypeReference>>(settings.TypeComparer);

            var compositionHost = configuration.CreateContainer();

            settings.DiffFactory = new ElementDifferenceFactory(compositionHost);
            return settings;
        }

        public IEnumerable<SyntaxToken> GetTokenList(IDefinition definition)
        {
            return _declHelper.GetTokenList(definition);
        }

        private void WriteMemberGroupHeader(ITypeDefinitionMember member)
        {
            if (member == null)
                return;

            if (IncludeMemberGroupHeadings || IncludeSpaceBetweenMemberGroups)
            {
                string heading = CSharpWriter.MemberGroupHeading(member);

                if (heading != null)
                {
                    if (IncludeSpaceBetweenMemberGroups)
                    {
                        if (!_firstMemberGroup)
                            _syntaxWriter.WriteLine(true);
                        _firstMemberGroup = false;
                    }

                    if (IncludeMemberGroupHeadings)
                    {
                        _syntaxWriter.Write("// {0}", heading);
                        _syntaxWriter.WriteLine();
                    }
                }
            }
        }

        // Consider moving these visitors to the base traverser if they end up being needed by others.
        private void Visit(IEnumerable<ElementMapping<AttributeGroup>> attributes)
        {
            foreach (var attribute in attributes)
                Visit(attribute);
        }

        private void Visit(ElementMapping<AttributeGroup> attribute)
        {
            WriteElement(attribute,
                ag =>
                {
                    foreach (var attr in ag.Attributes)
                        _formatter.WriteAttribute(attr);
                },
                (ag1, ag2) =>
                {
                    // TODO: Need to insert the newlines and indentions, perhaps support GetTokenList(IEnumerable<ICustomAttribute>)
                    var attributeComparer = new AttributeComparer();
                    var ag1Tokens = ag1.Attributes.OrderBy(c => c, attributeComparer)
                        .SelectMany(c => _declHelper.GetTokenList(c));
                    var ag2Tokens = ag2.Attributes.OrderBy(c => c, attributeComparer)
                        .SelectMany(c => _declHelper.GetTokenList(c));

                    foreach (var token in ListMerger.MergeLists(ag1Tokens, ag2Tokens))
                    {
                        WriteElement(token, t =>
                        {
                            _syntaxWriter.WriteSyntaxToken(t);
                        },
                            (t1, t2) =>
                            {
                                using (_syntaxWriter.StartStyle(SyntaxStyle.Removed))
                                    _syntaxWriter.WriteSyntaxToken(t1);
                                using (_syntaxWriter.StartStyle(SyntaxStyle.Added))
                                    _syntaxWriter.WriteSyntaxToken(t2);
                            }, false);
                    }
                }, true);
        }

        private void Visit(IEnumerable<ElementMapping<AssemblyMapping.AssemblyProperty>> properties)
        {
            foreach (var property in properties)
                Visit(property);
        }

        private void Visit(ElementMapping<AssemblyMapping.AssemblyProperty> property)
        {
            WriteElement(property,
                (p) =>
                {
                    _syntaxWriter.Write(string.Format("{0}{1}{2}", p.Name, p.Delimiter, p.Value));
                },
                (p1, p2) =>
                {
                    Contract.Assert(p1.Key == p2.Key);
                    _syntaxWriter.Write(string.Format("{0}{1}", p1.Name, p1.Delimiter));
                    using (_syntaxWriter.StartStyle(SyntaxStyle.Removed))
                        _syntaxWriter.Write(p1.Value);
                    using (_syntaxWriter.StartStyle(SyntaxStyle.Added))
                        _syntaxWriter.Write(p2.Value);
                }, true);
        }
    }
}
