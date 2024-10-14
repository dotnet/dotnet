// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.SolutionPersistence.Model;

/// <summary>
/// Information used by the model to determine project type details for a project.
/// This maps a friendly name and or a file extension to a project type id.
/// It also contains a list of default configuration rules for the project type.
/// It can reference another project type with BasedOn to inherit its
/// configuration rules and project type id.
/// </summary>
/// <remarks>
/// A <see cref="ProjectType"/> with no Name, Extension, or ProjectTypeId is a special
/// case used to define default configuration rules for all projects in the solution.
/// Otherwise, a <see cref="ProjectType"/> must have either a Name or Extension.
/// Each unique ProjectTypeId must have a unique Name.
/// </remarks>
/// <param name="projectTypeId">The project type id for this item.</param>
/// <param name="rules">Rules to determine the default configurations for projects of this type.</param>
public sealed class ProjectType(Guid projectTypeId, IReadOnlyList<ConfigurationRule> rules)
{
    /// <summary>
    /// Gets the project type id for this item.
    /// </summary>
    /// <remarks>
    /// This is a unique identifier for the project type and must not be Guid.Empty unless BasedOn is specified.
    /// </remarks>
    public Guid ProjectTypeId { get; } = projectTypeId;

    /// <summary>
    /// Gets rules to determine the default configurations for projects of this type.
    /// </summary>
    /// <remarks>
    /// If a project type should not build, it should have a single rule with Build set to false.
    /// </remarks>
    public IReadOnlyList<ConfigurationRule> ConfigurationRules { get; } = rules;

    /// <summary>
    /// Gets the name of the project type. This can be used instead of the ProjectTypeId to be
    /// more friendly to read.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Gets the file extension of the project type. This can be used to determine the project type automatically.
    /// </summary>
    /// <remarks>
    /// The leading dot is recommended, but optional.
    /// </remarks>
    public string? Extension { get; init; }

    /// <summary>
    /// Gets references a base project type to inherit its configuration rules and project type id.
    /// This uses the Name or Extension of the base project type to find it.
    /// </summary>
    public string? BasedOn { get; init; }
}
