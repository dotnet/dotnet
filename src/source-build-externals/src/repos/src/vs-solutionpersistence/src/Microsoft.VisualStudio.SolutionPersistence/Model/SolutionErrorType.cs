// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.SolutionPersistence.Model;

/// <summary>
/// Reasons the SolutionArgumentException was raised.
/// </summary>
public enum SolutionErrorType
{
    /// <summary>
    /// The cause of the error is not specified.
    /// </summary>
    Undefined,

    /// <summary>
    /// There was an error while trying to move a folder to a child folder.
    /// </summary>
    CannotMoveFolderToChildFolder,

    /// <summary>
    /// The default project type was duplicated.
    /// </summary>
    DuplicateDefaultProjectType,

    /// <summary>
    /// File has two extensions.
    /// </summary>
    DuplicateExtension,

    /// <summary>
    /// Item already exists in the solution.
    /// </summary>
    DuplicateItemRef,

    /// <summary>
    /// Name of item is duplicate.
    /// </summary>
    DuplicateName,

    /// <summary>
    /// A project with the same name already exists.
    /// </summary>
    DuplicateProjectName,

    /// <summary>
    /// A project with the same path already exists.
    /// </summary>
    DuplicateProjectPath,

    /// <summary>
    /// This project type is already specified.
    /// </summary>
    DuplicateProjectTypeId,

    /// <summary>
    /// Invalid syntax for solution configuration.
    /// </summary>
    InvalidConfiguration,

    /// <summary>
    /// Invalid encoding for solution.
    /// </summary>
    InvalidEncoding,

    /// <summary>
    /// Folder path doesn't follow correct format.
    /// </summary>
    InvalidFolderPath,

    /// <summary>
    /// Folder was not found.
    /// </summary>
    InvalidFolderReference,

    /// <summary>
    /// Item is not valid.
    /// </summary>
    InvalidItemRef,

    /// <summary>
    /// Found a circular dependency.
    /// </summary>
    InvalidLoop,

    /// <summary>
    /// Model does not belong to this solution.
    /// </summary>
    InvalidModelItem,

    /// <summary>
    /// Name of item is not valid.
    /// </summary>
    InvalidName,

    /// <summary>
    /// Project was not found.
    /// </summary>
    InvalidProjectReference,

    /// <summary>
    /// Project type was not found.
    /// </summary>
    InvalidProjectTypeReference,

    /// <summary>
    /// File version is not supported.
    /// </summary>
    InvalidVersion,

    /// <summary>
    /// Empty value for project attribute.
    /// </summary>
    MissingProjectValue,

    /// <summary>
    /// The file is not a solution file.
    /// </summary>
    NotSolution,

    /// <summary>
    /// This veersion is not supported.
    /// </summary>
    UnsupportedVersion,

    /// <summary>
    /// Invalid decorator element name.
    /// </summary>
    InvalidXmlDecoratorElementName,
}
