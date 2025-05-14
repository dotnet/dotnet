// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Xml;

namespace Microsoft.VisualStudio.SolutionPersistence.Serializer.Xml.XmlDecorators;

/// <summary>
/// Wraps an <see cref="XmlElement"/> to provide semantic helpers for the Slnx model."/>
/// These methods are used to update the Xml DOM with changes from the model.
/// </summary>
internal abstract partial class XmlDecorator
{
    // CONSIDER: Use StringTable if strings will be kept.
    internal string? GetXmlAttribute(Keyword keyword) => this.XmlElement.GetAttribute(keyword.ToXmlString()).Trim().NullIfEmpty();

    internal void UpdateXmlAttribute<T>(Keyword keyword, bool isDefault, T value, Func<T, string> toString)
    {
        if (isDefault)
        {
            this.XmlElement.RemoveAttribute(keyword.ToXmlString());
        }
        else
        {
            this.XmlElement.SetAttribute(keyword.ToXmlString(), toString(value));
        }
    }

    internal List<XmlNode> GetElementAndTrivia()
    {
        List<XmlNode> trivia = new(8);

        XmlNode? previous = this.XmlElement.PreviousSibling;
        while (previous is XmlWhitespace or XmlComment)
        {
            trivia.Add(previous);
            previous = previous.PreviousSibling;
        }

        trivia.Add(this.XmlElement);
        return trivia;
    }

    internal XmlNode GetFirstTrivia()
    {
        XmlNode? previous = this.XmlElement.PreviousSibling;
        XmlNode? trivia = null;
        while (previous is XmlWhitespace or XmlComment)
        {
            trivia = previous;
            previous = previous.PreviousSibling;
        }

        return trivia ?? this.XmlElement;
    }

    internal StringSpan GetNewLineAndIndent()
    {
        // The solution node doesn't have a newline before it, so create one.
        if (this.ElementName == Keyword.Solution)
        {
            return (this.Root.SerializationSettings.NewLine ?? Environment.NewLine).AsSpan();
        }

        return this.XmlElement.PreviousSibling is not XmlWhitespace previousWhitespace ?
            StringSpan.Empty :
            OnlyOneLine(previousWhitespace.Value.AsSpan());

        static StringSpan OnlyOneLine(StringSpan value)
        {
            if (value.IsEmpty)
            {
                return value;
            }

            int startIndex = value.Length;
            while (startIndex > 0 && value[startIndex - 1] is not '\r' and not '\n')
            {
                startIndex--;
            }

            if (startIndex > 0 && value[startIndex - 1] is '\n')
            {
                startIndex--;
            }

            if (startIndex > 0 && value[startIndex - 1] is '\r')
            {
                startIndex--;
            }

            return value.Slice(startIndex);
        }
    }
}
