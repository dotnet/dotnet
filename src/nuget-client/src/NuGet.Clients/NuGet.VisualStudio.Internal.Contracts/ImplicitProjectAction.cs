// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.PackageManagement;
using NuGet.Packaging.Core;

namespace NuGet.VisualStudio.Internal.Contracts
{
    public sealed class ImplicitProjectAction : IEquatable<ImplicitProjectAction>
    {
        public string Id { get; }
        public PackageIdentity PackageIdentity { get; }
        public NuGetProjectActionType ProjectActionType { get; }

        public ImplicitProjectAction(
            string id,
            PackageIdentity packageIdentity,
            NuGetProjectActionType projectActionType)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException(Resources.ArgumentCannotBeNullOrEmpty, nameof(id));
            }

            Id = id;
            PackageIdentity = packageIdentity ?? throw new ArgumentNullException(nameof(packageIdentity));
            ProjectActionType = projectActionType;
        }

        public bool Equals(ImplicitProjectAction? other)
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
                && PackageIdentity.Equals(other.PackageIdentity)
                && ProjectActionType == other.ProjectActionType;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ImplicitProjectAction);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
