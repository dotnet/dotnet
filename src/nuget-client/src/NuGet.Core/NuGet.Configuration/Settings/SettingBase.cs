// Copyright(c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Xml.Linq;

namespace NuGet.Configuration
{
    public abstract class SettingBase
    {
        internal XNode? Node { get; private set; }

        internal ISettingsGroup? Parent { get; set; }

        internal SettingsFile? Origin { get; private set; }

        internal SettingBase(XNode node, SettingsFile origin)
        {
            Node = node ?? throw new ArgumentNullException(nameof(node));
            Origin = origin ?? throw new ArgumentNullException(nameof(origin));
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        protected SettingBase() { }

        /// <summary>
        /// Specifies if the setting is an in-memory-only setting. 
        /// </summary>
        internal bool IsAbstract() => Node == null && Origin == null;

        /// <summary>
        /// Specifies if the setting is a copy of a concrete setting in a file.
        /// </summary>
        internal bool IsCopy() => Node == null && Origin != null;

        /// <summary>
        /// Specifies if the setting has attributes or values.
        /// </summary>
        public abstract bool IsEmpty();

        /// <summary>
        /// Gives the representation of this setting as an XNode object
        /// </summary>
        internal virtual XNode? AsXNode() => Node;

        /// <summary>
        /// Creates a shallow copy of the setting.
        /// Does not copy any pointer to the original data structure.
        /// Just copies the abstraction.
        /// </summary>
        public abstract SettingBase Clone();

        internal void SetNode(XNode node)
        {
            if (Node != null)
            {
                throw new InvalidOperationException(Resources.CannotUpdateNode);
            }

            Node = node ?? throw new ArgumentNullException(nameof(node));
        }

        /// <summary>
        /// Convenience method to add an element to an origin.
        /// Since an origin should not be updated, any update will be ignored.
        /// </summary>
        /// <remarks>Each setting can override this method to include any descendants to the origin</remarks>
        internal virtual void SetOrigin(SettingsFile origin)
        {
            if (Origin == null || (Origin != null && Origin == origin))
            {
                Origin = origin;
            }
        }

        /// <summary>
        /// Convenience method to remove an element from it's origin and convert to abstract
        /// </summary>
        /// <remarks>Each setting can override this method to remove any descendants from their origin</remarks>
        internal virtual void RemoveFromSettings()
        {
            if (!IsCopy() && !IsAbstract())
            {
                XElementUtility.RemoveIndented(Node);
                if (Origin != null)
                {
                    Origin.IsDirty = true;
                }

                Node = null;
            }

            Origin = null;
            Parent = null;
        }
    }
}
