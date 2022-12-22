// Copyright(c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NuGet.Shared;

namespace NuGet.Configuration
{
    public abstract class SettingsGroup<T> : SettingElement, ISettingsGroup where T : SettingElement
    {
        protected IList<T> Children { get; private set; }

        protected virtual bool CanBeCleared => true;

        protected SettingsGroup()
            : base()
        {
            Children = new List<T>();
        }

        protected SettingsGroup(IReadOnlyDictionary<string, string> attributes, IEnumerable<T> children)
            : base(attributes)
        {
            if (children == null)
            {
                Children = new List<T>();
            }
            else
            {
                Children = new List<T>(children);
            }
        }

        public override bool IsEmpty() => !Children.Any() || Children.All(c => c.IsEmpty());

        internal SettingsGroup(XElement element, SettingsFile origin)
            : base(element, origin)
        {
            ElementName = element.Name.LocalName;

            Children = SettingFactory.ParseChildren<T>(element, origin, CanBeCleared).ToList();

            foreach (var child in Children)
            {
                child.Parent = this;
            }
        }

        internal override XNode AsXNode()
        {
            if (Node is XElement)
            {
                return Node;
            }

            var element = new XElement(XmlUtility.GetEncodedXMLName(ElementName), Children.Select(c => c.AsXNode()));

            foreach (var attr in Attributes)
            {
                element.SetAttributeValue(attr.Key, attr.Value);
            }

            return element;
        }

        internal override void SetOrigin(SettingsFile origin)
        {
            base.SetOrigin(origin);

            foreach (var child in Children)
            {
                child.SetOrigin(origin);
            }
        }

        internal override void RemoveFromSettings()
        {
            base.RemoveFromSettings();

            foreach (var child in Children)
            {
                child.RemoveFromSettings();
            }
        }

        internal virtual bool Add(T setting)
        {
            if (setting == null)
            {
                throw new ArgumentNullException(nameof(setting));
            }

            if (Origin.IsMachineWide)
            {
                throw new InvalidOperationException(Resources.CannotUpdateMachineWide);
            }

            if (Origin.IsReadOnly)
            {
                throw new InvalidOperationException(Resources.CannotUpdateReadOnlyConfig);
            }

            if (!Children.Contains(setting) && !setting.IsEmpty())
            {
                Children.Add(setting);

                setting.SetOrigin(Origin);
                setting.SetNode(setting.AsXNode());

                XElementUtility.AddIndented(Node as XElement, setting.Node);
                Origin.IsDirty = true;

                setting.Parent = this;

                return true;
            }

            return false;
        }

        internal virtual void Remove(T setting)
        {
            if (setting == null)
            {
                throw new ArgumentNullException(nameof(setting));
            }

            if (Origin != null && Origin.IsMachineWide)
            {
                throw new InvalidOperationException(Resources.CannotUpdateMachineWide);
            }

            if (Origin != null && Origin.IsReadOnly)
            {
                throw new InvalidOperationException(Resources.CannotUpdateReadOnlyConfig);
            }

            if (TryGetChild(setting, out var currentSetting) && Children.Remove(currentSetting))
            {
                currentSetting.RemoveFromSettings();

                if (Parent != null && IsEmpty())
                {
                    Parent.Remove(this);
                }
            }
        }

        protected bool TryGetChild(T expectedChild, out T currentChild)
        {
            currentChild = null;

            foreach (var child in Children)
            {
                if (child.Equals(expectedChild))
                {
                    currentChild = child;

                    return true;
                }
            }

            return false;
        }

        void ISettingsGroup.Remove(SettingElement setting)
        {
            Remove(setting as T);
        }
    }
}
