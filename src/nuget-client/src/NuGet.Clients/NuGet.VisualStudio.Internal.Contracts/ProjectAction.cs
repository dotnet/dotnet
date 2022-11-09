// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.PackageManagement;
using NuGet.Packaging.Core;

namespace NuGet.VisualStudio.Internal.Contracts
{
    public sealed class ProjectAction : IEquatable<ProjectAction>
    {
        public string Id { get; }
        public IReadOnlyList<ImplicitProjectAction> ImplicitActions { get; }
        public PackageIdentity PackageIdentity { get; }
        public NuGetProjectActionType ProjectActionType { get; }
        public string ProjectId { get; }

        public ProjectAction(
            string id,
            string projectId,
            PackageIdentity packageIdentity,
            NuGetProjectActionType projectActionType,
            IReadOnlyList<ImplicitProjectAction>? implicitActions)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException(Resources.ArgumentCannotBeNullOrEmpty, nameof(id));
            }

            if (string.IsNullOrEmpty(projectId))
            {
                throw new ArgumentException(Resources.ArgumentCannotBeNullOrEmpty, nameof(projectId));
            }

            Id = id;
            ProjectId = projectId;
            PackageIdentity = packageIdentity ?? throw new ArgumentNullException(nameof(packageIdentity));
            ProjectActionType = projectActionType;
            ImplicitActions = implicitActions ?? Array.Empty<ImplicitProjectAction>();
        }

        public bool Equals(ProjectAction? other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return StringComparer.Ordinal.Equals(Id, other.Id)
                && StringComparer.Ordinal.Equals(ProjectId, other.ProjectId)
                && PackageIdentity.Equals(other.PackageIdentity)
                && ProjectActionType == other.ProjectActionType;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ProjectAction);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
