// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.DotNet.VersionTools.Util;

namespace Microsoft.DotNet.VersionTools.BuildManifest.Model
{
    public abstract class ArtifactModel
    {
        public abstract XElement ToXml();

        public IDictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Original file path.
        /// </summary>
        public string OriginalFile { get; set; }

        public string Id
        {
            get { return Attributes.GetOrDefault(nameof(Id)); }
            set { Attributes[nameof(Id)] = value; }
        }

        public string RepoOrigin
        {
            get => Attributes.GetOrDefault(nameof(RepoOrigin));
            set => Attributes[nameof(RepoOrigin)] = value;
        }

        public bool NonShipping
        {
            get
            {
                string val = Attributes.GetOrDefault(nameof(NonShipping));
                if (!string.IsNullOrEmpty(val))
                {
                    return bool.Parse(val);
                }
                return false;
            }
            set
            {
                Attributes[nameof(NonShipping)] = value.ToString();
            }
        }

        public ArtifactVisibility Visibility
        {
            get
            {
                string val = Attributes.GetOrDefault(nameof(Visibility));
                if (string.IsNullOrEmpty(val))
                {
                    return ArtifactVisibility.External;
                }
                else if (Enum.TryParse(val, out ArtifactVisibility visibility))
                {
                    return visibility;
                }
                else
                {
                    throw new ArgumentException($"Invalid value for {nameof(Visibility)}: {val}");
                }
            }
            set
            {
                Attributes[nameof(Visibility)] = value.ToString();
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is ArtifactModel other)
            {
                if (ReferenceEquals(this, obj))
                {
                    return true;
                }

                if (Attributes.Count() != other.Attributes.Count())
                {
                    return false;
                }

                foreach (var localAttr in Attributes)
                {
                    if (localAttr.Value == null)
                    {
                        if (other.Attributes.GetOrDefault(localAttr.Key) != null)
                        {
                            return false;
                        }
                    }
                    else if (localAttr.Value.Equals(
                        other.Attributes.GetOrDefault(localAttr.Key),
                        StringComparison.OrdinalIgnoreCase) == false)
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }
        public override int GetHashCode()
        {
            int hash = 1;

            foreach (var item in Attributes)
            {
                hash *= (item.Key, item.Value).GetHashCode();
            }

            return hash;
        }
    }
}
