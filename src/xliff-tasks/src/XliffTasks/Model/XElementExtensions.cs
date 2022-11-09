// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Xml.Linq;
using static XliffTasks.Model.XlfNames;

namespace XliffTasks.Model
{
    internal static class XElementExtensions
    {
        /// <summary>
        /// Returns the effective target value of a <code>trans-unit</code> element in an XLIFF file.
        /// If the <code>trans-unit</code> has no <code>target</code> child, returns the value of the <code>source</code>
        /// child.
        /// </summary>
        /// <param name="transUnitElement">An <see cref="XElement"/> representing the <code>trans-unit</code>.</param>
        /// <returns>The value of the <code>target</code> element if it exists; otherwise the value of the
        /// <code>source</code> element.</returns>
        public static string GetTargetValue(this XElement transUnitElement) => 
            transUnitElement.Element(Target)?.Value
            ?? transUnitElement.Element(Source).Value;

        /// <summary>
        /// Sets the target value of a <code>trans-unit</code> element in an XLIFF file.
        /// Creates the <code>target</code> element and <code>state</code> attribute as necessary.
        /// </summary>
        /// <param name="transUnitElement">An <see cref="XElement"/> representing the <code>trans-unit</code>.</param>
        /// <param name="value">The new target value.</param>
        public static void SetTargetValue(this XElement transUnitElement, string value)
        {
            XElement targetElement = transUnitElement.Element(Target);
            if (targetElement == null)
            {
                XElement sourceElement = transUnitElement.Element(Source);
                targetElement = new XElement(Target, new XAttribute("state", "new"));
                sourceElement.AddAfterSelf(targetElement);
            }

            targetElement.Value = value;
            if (targetElement.Attribute("state") == null)
            {
                targetElement.Add(new XAttribute("state", "new"));
            }
        }

        /// <summary>
        /// Returns the effective target translation of a <code>trans-unit</code> element in an XLIFF file.
        /// If the <code>trans-unit</code> has no <code>target</code> element or <code>state</code> attribute,
        /// returns a default value ("new").
        /// </summary>
        /// <param name="transUnitElement">An <see cref="XElement"/> representing the <code>trans-unit</code>.</param>
        /// <returns>The value of the <code>state</code> attribute if it exists, otherwise "new".</returns>
        public static string GetTargetState(this XElement transUnitElement) =>
            transUnitElement.Element(Target)?.Attribute("state")?.Value
            ?? "new";

        /// <summary>
        /// Sets the target translation state of a <code>trans-unit</code> element in an XLIFF file.
        /// Creates the <code>target</code> element and <code>state</code> attribute as necessary.
        /// </summary>
        /// <param name="transUnitElement">An <see cref="XElement"/> representing the <code>trans-unit</code>.</param>
        /// <param name="value">The new state value.</param>
        public static void SetTargetState(this XElement transUnitElement, string value)
        {
            XElement targetElement = transUnitElement.Element(Target);
            if (targetElement == null)
            {
                XElement sourceElement = transUnitElement.Element(Source);
                targetElement = new XElement(Target);
                sourceElement.AddAfterSelf(targetElement);
            }

            XAttribute stateAttribute = targetElement.Attribute("state");
            if (stateAttribute == null)
            {
                stateAttribute = new XAttribute("state", value);
                targetElement.Add(stateAttribute);
            }
            else
            {
                stateAttribute.Value = value;
            }
        }

        /// <summary>
        /// Gets the <code>source</code> value of a <code>trans-unit</code> element in an XLIFF file.
        /// </summary>
        /// <param name="transUnitElement">An <see cref="XElement"/> representing the <code>trans-unit</code>.</param>
        /// <returns>The value of the <code>source</code> element.</returns>
        public static string GetSourceValue(this XElement transUnitElement) =>
            transUnitElement.Element(Source).Value;

        /// <summary>
        /// Sets the <code>source</code> value of a <code>trans-unit</code> element in an XLIFF file.
        /// </summary>
        /// <param name="transUnitElement">An <see cref="XElement"/> representing the <code>trans-unit</code>.</param>
        /// <param name="value">The new <code>source</code> value.</param>
        public static void SetSourceValue(this XElement transUnitElement, string value) =>
            transUnitElement.Element(Source).Value = value;

        /// <summary>
        /// Gets the <code>note</code> value of a <code>trans-unit</code> element in an XLIFF file.
        /// </summary>
        /// <param name="transUnitElement">An <see cref="XElement"/> representing the <code>trans-unit</code>.</param>
        /// <returns>The value of the <code>note</code> element, or <code>null</code> if it does not exist.</returns>
        public static string GetNoteValue(this XElement transUnitElement) =>
            transUnitElement.Element(Note)?.Value;

        /// <summary>
        /// Sets the <code>note</code> value of a <code>trans-unit</code> element in an XLIFF file.
        /// Creates the <code>note</code> element as needed, and places it in the correct location
        /// relative to the <code>source</code> and <code>target</code> elements.
        /// </summary>
        /// <param name="transUnitElement">An <see cref="XElement"/> representing the <code>trans-unit</code>.</param>
        /// <param name="value">The new <code>note</code> value.</param>
        public static void SetNoteValue(this XElement transUnitElement, string value)
        {
            XElement noteElement = transUnitElement.Element(Note);
            if (noteElement == null)
            {
                XElement priorElement = transUnitElement.Element(Target) ?? transUnitElement.Element(Source);
                noteElement = new XElement(Note);
                priorElement.AddAfterSelf(noteElement);
            }

            noteElement.Value = value;
            noteElement.SelfCloseIfPossible();
        }

        /// <summary>
        /// Gets the <code>id</code> value of a <code>trans-unit</code> element in an XLIFF file.
        /// </summary>
        /// <param name="transUnitElement">An <see cref="XElement"/> representing the <code>trans-unit</code>.</param>
        /// <returns>The value of the <code>id</code> attribute.</returns>
        public static string GetId(this XElement transUnitElement) =>
            transUnitElement.Attribute("id").Value;
    }
}
