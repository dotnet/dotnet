// Copyright(c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Xml.Linq;
using NuGet.Shared;

namespace NuGet.Configuration
{
    public abstract class SettingsGroup<T> : SettingElement, ISettingsGroup where T : SettingElement
    {
        protected IList<T> Children { get; private set; }

        protected virtual bool CanBeCleared => true;

        public override string ElementName { get; }

        protected SettingsGroup(string name)
            : this(name, attributes: null, children: null)
        {
        }

        protected SettingsGroup(string name, IReadOnlyDictionary<string, string>? attributes, IEnumerable<T>? children)
            : base(attributes)
        {
            ElementName = name ?? throw new ArgumentNullException(message: Resources.Argument_Cannot_Be_Null_Or_Empty, paramName: nameof(name));
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

        internal SettingsGroup(string name, XElement element, SettingsFile origin)
            : base(element, origin)
        {
            ElementName = name;

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

            if (Origin is null)
            {
                throw new InvalidOperationException("Cannot call this method on a setting where Origin is null.");
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

        protected bool TryGetChild(T expectedChild, [NotNullWhen(true)] out T? currentChild)
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
            Remove((T)setting);
        }
    }
}
