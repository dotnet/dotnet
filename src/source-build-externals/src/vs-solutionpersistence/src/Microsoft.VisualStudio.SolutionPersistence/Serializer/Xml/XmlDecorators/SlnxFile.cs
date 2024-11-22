// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Xml;
using Microsoft.VisualStudio.SolutionPersistence.Model;
using Microsoft.VisualStudio.SolutionPersistence.Utilities;

namespace Microsoft.VisualStudio.SolutionPersistence.Serializer.Xml.XmlDecorators;

/// <summary>
/// Creates an Xml DOM model for reading and updating the slnx file.
/// </summary>
[DebuggerDisplay("{Solution}")]
internal sealed class SlnxFile
{
    internal const int CurrentVersion = 1;

    internal SlnxFile(
        XmlDocument xmlDocument,
        SlnxSerializerSettings serializationSettings,
        StringTable? stringTable,
        string? fullPath)
    {
        this.Document = xmlDocument;
        this.FullPath = fullPath;
        this.StringTable = stringTable ?? new StringTable().WithSolutionConstants();

        XmlElement? xmlSolution = this.Document.DocumentElement;
        if (xmlSolution is not null && Keywords.ToKeyword(xmlSolution.Name) == Keyword.Solution)
        {
            this.Solution = new XmlSolution(this, xmlSolution);
            this.Solution.UpdateFromXml();

            // This is a model part, but needs to be calculated before it can properly turn into a model.
            // These are used to calculate the actual project types from a project's Type attribute.
            this.ProjectTypes = this.Solution.GetProjectTypeTable();
        }
        else
        {
            throw new SolutionException(Errors.NotSolution, SolutionErrorType.NotSolution) { File = this.FullPath };
        }

        this.SerializationSettings = this.GetDefaultSerializationSettings(serializationSettings);
    }

    internal string? FullPath { get; }

    // Slnx file version.
    internal Version? FileVersion { get; set; }

    internal XmlDocument Document { get; }

    internal XmlSolution? Solution { get; private set; }

    internal SlnxSerializerSettings SerializationSettings { get; }

    internal StringTable StringTable { get; }

    internal ProjectTypeTable ProjectTypes { get; private set; }

    // Keep track of user project and file paths to preserve the user's path separators.
    internal Dictionary<string, string> UserPaths { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

    internal bool Tarnished { get; private set; }

    internal SolutionModel ToModel()
    {
        this.UserPaths.Clear();
        SolutionModel model = this.Solution?.ToModel() ?? new SolutionModel() { StringTable = this.StringTable };
        model.SerializerExtension = new SlnXmlModelExtension(SolutionSerializers.SlnXml, this.SerializationSettings, root: this);
        return model;
    }

    /// <summary>
    /// Converts a model project path to use the slashes the user provides, or default to forward slashes.
    /// </summary>
    internal string ConvertToUserPath(string projectPath)
    {
        return this.UserPaths.TryGetValue(projectPath, out string? userProjectPath) ?
            userProjectPath :
            PathExtensions.ConvertModelToForwardSlashPath(projectPath);
    }

    /// <summary>
    /// Update the Xml DOM with changes from the model.
    /// </summary>
    /// <returns>
    /// true if any changes were made to the XML.
    /// </returns>
    internal bool ApplyModel(SolutionModel model)
    {
        this.ProjectTypes = model.ProjectTypeTable;

        bool modified = false;
        if (this.Solution is null)
        {
            // Make the solution element the root element of the document.
            XmlElement xmlSolution = this.Document.CreateElement(Keyword.Solution.ToXmlString());
            _ = this.Document.AppendChild(xmlSolution);
            this.Solution = new XmlSolution(this, xmlSolution);
            this.Solution.UpdateFromXml();
            modified = true;
        }

        modified |= this.Solution.ApplyModelToXml(model);
        return modified;
    }

    internal string ToXmlString()
    {
        return this.Document.OuterXml;
    }

    // Fill out default values.
    private SlnxSerializerSettings GetDefaultSerializationSettings(SlnxSerializerSettings inputSettings)
    {
        string newLineChars = Environment.NewLine;
        string newIndentChars = "  ";
        if ((inputSettings.IndentChars is null || inputSettings.NewLine is null) &&
            this.Solution is not null &&
            this.Solution.TryGetFormatting(out StringSpan newLine, out StringSpan indent))
        {
            newLineChars = newLine.ToString();
            newIndentChars = indent.ToString();
        }

        return inputSettings with
        {
            PreserveWhitespace = inputSettings.PreserveWhitespace ?? this.Document.PreserveWhitespace,
            IndentChars = inputSettings.IndentChars ?? newIndentChars,
            NewLine = inputSettings.NewLine ?? newLineChars,
        };
    }
}
