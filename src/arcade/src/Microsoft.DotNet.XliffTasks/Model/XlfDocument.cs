// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using XliffTasks.Tasks;
using static XliffTasks.Model.XlfNames;

namespace XliffTasks.Model
{
    /// <summary>
    /// Represents a document in XLIFF format which can be updated from source from
    /// <see cref="TranslatableDocument"/> instances and produce translation data
    /// for <see cref="TranslatableDocument.Translate"/>.
    /// 
    /// See https://en.wikipedia.org/wiki/XLIFF
    /// </summary>
    internal sealed class XlfDocument : Document
    {
        private static XmlSchemaSet s_schemaSet;
        
        private static readonly XNamespace XsiNS = "http://www.w3.org/2001/XMLSchema-instance";

        private XDocument _document;

        /// <summary>
        /// Indicates if content has been loaded in to the document.
        /// </summary>
        public override bool HasContent => _document != null;

        /// <summary>
        /// Loads (or reloads) the document content from the given reader.
        /// </summary>
        public override void Load(System.IO.TextReader reader)
        {
            _document = XDocument.Load(reader);
        }

        /// <summary>
        /// Loads initial document content for a new XLIFF document.
        /// </summary>
        public void LoadNew(string targetLanguage)
        {
            _document = new XDocument(
                new XElement(Xliff,
                    new XAttribute("xmlns", XliffNS.NamespaceName),
                    new XAttribute(XNamespace.Xmlns + "xsi", XsiNS.NamespaceName),
                    new XAttribute("version", "1.2"),
                    new XAttribute(XsiNS + "schemaLocation", $"{XliffNS.NamespaceName} xliff-core-1.2-transitional.xsd"),
                    new XElement(File,
                        new XAttribute("datatype", "xml"),
                        new XAttribute("source-language", "en"),
                        new XAttribute("target-language", targetLanguage),
                        new XAttribute("original", "_"), // placeholder will be replaced on first update
                        new XElement(Body))));
        }

        /// <summary>
        /// Saves the document's content (with translations applied if <see cref="Translate" /> was called) to the given file path.
        /// </summary>
        public override void Save(System.IO.TextWriter writer)
        {
            EnsureContent();
            _document.SaveCustom(writer);
        }

        /// <summary>
        /// Updates this XLIFF document with the source data from the given translatable document.
        /// </summary>
        /// <returns>True if any changes were made to this document.</returns>
        public bool Update(TranslatableDocument sourceDocument, string sourceDocumentId)
        {
            bool changed = false;
            Dictionary<string, TranslatableNode> nodesById = new();
            foreach (TranslatableNode node in sourceDocument.Nodes)
            {
                if (nodesById.ContainsKey(node.Id))
                {
                    throw new BuildErrorException($"The document '{sourceDocumentId}' has a duplicate node '{node.Id}'.");
                }

                nodesById.Add(node.Id, node);
            }

            XElement fileElement = _document.Root.Element(File);
            XAttribute originalAttribute = fileElement.Attribute("original");
            if (originalAttribute.Value != sourceDocumentId)
            {
                // update original path in case where user has renaned source file and corresponding xlf
                originalAttribute.Value = sourceDocumentId;
                changed = true;
            }

            XElement bodyElement = fileElement.Element(Body);
            XElement groupElement = bodyElement.Element(Group);

            if (groupElement != null && !groupElement.Elements().Any())
            {
                // remove unnecessary empty group added by older tool. We don't want to bother keeping that unnecessary id up-to-date.
                groupElement.Remove();
                changed = true;
            }

            foreach (XElement unitElement in bodyElement.Descendants(TransUnit).ToList())
            {
                string id = unitElement.GetId();
                string state = unitElement.GetTargetState();
                string source = unitElement.GetSourceValue();
                string note = unitElement.GetNoteValue();

                // delete node in document that has been removed from source.
                if (!nodesById.TryGetValue(id, out TranslatableNode sourceNode))
                {
                    unitElement.Remove();
                    changed = true;
                    continue;
                }

                // update trans-unit state if either the source text or associated note has change.
                if (source != sourceNode.Source || (sourceNode.Note != null && note != sourceNode.Note))
                {
                    unitElement.SetSourceValue(sourceNode.Source);

                    // if sourceNode.Note is null, it indicates that the source format can't have notes, in which case
                    // they may be applied directly to the xlf by the user and we should not revert that on update
                    if (sourceNode.Note != null)
                    {
                        unitElement.SetNoteValue(sourceNode.Note);
                    }

                    switch (state)
                    {
                        case "new":
                            // when a new string gets modified before it has been translated,
                            // update untranslated target to match the new source
                            unitElement.SetTargetValue(sourceNode.Source);
                            break;

                        case "translated":
                            // flag strings that have been modified after translation for review/re-translation
                            unitElement.SetTargetState("needs-review-translation");
                            break;
                    }

                    changed = true;
                }

                // If the source and target require different numbers of formatting items then reset
                // the target string completely. This avoids problems when the source has been updated
                // to remove formatting items--when formatting the target string we won't have as many
                // replacement items as it calls for, leading to an exception.
                // And if the source string is updated to use _more_ items then formatting with the
                // target string is likely to produce misleading (or outright meaningless) text. In
                // either case we lose nothing by just reverting the string until it can be localized
                // again.
                // Note we don't limit this check to when the source has changed in the original
                // document because we also want to catch errors introduced during translation.
                int sourceReplacementCount = unitElement.GetSourceValue().GetReplacementCount();
                int targetReplacementCount = unitElement.GetTargetValue().GetReplacementCount();

                if (targetReplacementCount != sourceReplacementCount)
                {
                    unitElement.SetTargetValue(sourceNode.Source);
                    unitElement.SetTargetState("new");

                    changed = true;
                }

                // signal to loop below that this node is not new
                nodesById.Remove(id);
            }

            // Add new trans-units
            foreach (TranslatableNode sourceNode in sourceDocument.Nodes)
            {
                // Nodes that have been removed from nodesById table are not new and have already been handled.
                // Do not refactor this check away by iterating over dictionary values as the document order must be maintained deterministically.
                if (!nodesById.ContainsKey(sourceNode.Id))
                {
                    continue;
                }

                XElement newTransUnit = 
                    new(TransUnit,
                        new XAttribute("id", sourceNode.Id),
                        new XElement(Source, sourceNode.Source),
                        new XElement(Target, new XAttribute("state", "new"), sourceNode.Source),
                        new XElement(Note, sourceNode.Note == "" ? null : sourceNode.Note));

                bool inserted = false;
                foreach (XElement transUnit in bodyElement.Elements(TransUnit))
                {
                    if (StringComparer.Ordinal.Compare(newTransUnit.GetId(), transUnit.GetId()) < 0)
                    {
                        transUnit.AddBeforeSelf(newTransUnit);
                        inserted = true;
                        break;
                    }
                }

                if (!inserted)
                {
                    bodyElement.Add(newTransUnit);
                }

                changed = true;
            }

            return changed;
        }

        /// <summary>
        /// Sorts the <code>trans-unit</code> elements in the document by their <code>id</code> attribute.
        /// </summary>
        /// <returns>Returns <code>true</code> if the document was modified; <code>false</code> otherwise.</returns>
        public bool Sort()
        {
            bool changed = false;

            XNamespace ns = _document.Root.Name.Namespace;

            XElement fileElement = _document.Root.Element(File);
            XElement bodyElement = fileElement.Element(Body);

            IEnumerable<XElement> transUnits = bodyElement.Elements(TransUnit);

            IComparer<string> comparer = StringComparer.Ordinal;
            if (!transUnits.IsSorted(tu => tu.GetId(), comparer))
            {
                changed = true;
                SortedList<string, XElement> sortedTransUnits = new(comparer);

                // Sort the translation units
                foreach (XElement transUnit in transUnits)
                {
                    sortedTransUnits.Add(transUnit.GetId(), transUnit);
                }

                // Remove them from the body element
                foreach (XElement transUnit in sortedTransUnits.Values)
                {
                    transUnit.Remove();
                }

                // Add them back in sorted order
                bodyElement.Add(sortedTransUnits.Values);
            }

            return changed;
        }

        /// <summary>
        /// Gets the translations (key=id, value=target), which can
        /// be passed on to <see cref="TranslatableDocument.Translate"/>.
        /// </summary>
        public IReadOnlyDictionary<string, string> GetTranslations()
        {
            Dictionary<string, string> dictionary = new();

            foreach (XElement element in _document.Descendants(TransUnit))
            {
                string id = element.GetId();
                string target = element.GetTargetValue();

                dictionary.Add(id, target);
            }

            return dictionary;
        }

        public ISet<string> GetUntranslatedResourceIDs()
        {
            XNamespace ns = _document.Root.Name.Namespace;

            IEnumerable<string> untranslatedResourceIDs =
                (_document.Descendants(TransUnit)
                 .Where(tu =>
                 {
                     return tu.GetTargetState() != "translated";
                 })
                 .Select(tu => tu.GetId()));

            return new HashSet<string>(untranslatedResourceIDs, StringComparer.Ordinal);
        }

        /// <summary>
        /// Runs the document through XSD schema validation and reports any errors.
        /// </summary>
        /// <param name="validationErrorHandler">Handler invoked for each validation error.</param>
        public void Validate(Action<XmlSchemaException> validationErrorHandler)
        {
            if (!HasContent)
            {
                return;
            }

            XmlSchemaSet schemas = GetSchemaSet();

            _document.Validate(schemas, (o, e) => validationErrorHandler(e.Exception));
        }

        private static XmlSchemaSet GetSchemaSet()
        {
            if (s_schemaSet == null)
            {
                System.IO.Stream xmlSchemaResourceStream = typeof(XlfDocument).Assembly.GetManifestResourceStream("XliffTasks.Model.xml.xsd");
                XmlReader xmlSchemaReader = XmlReader.Create(xmlSchemaResourceStream);
                System.IO.Stream xliffSchemaResourceStream = typeof(XlfDocument).Assembly.GetManifestResourceStream("XliffTasks.Model.xliff-core-1.2-transitional.xsd");
                XmlReader xliffSchemaReader = XmlReader.Create(xliffSchemaResourceStream);

                XmlSchemaSet schemas = new();
                schemas.Add(targetNamespace: null, xmlSchemaReader);
                schemas.Add(targetNamespace: null, xliffSchemaReader);

                s_schemaSet = schemas;
            }

            return s_schemaSet;
        }
    }
}