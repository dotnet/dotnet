// Copyright(c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace NuGet.Configuration
{
    public sealed class UnknownItem : SettingItem, ISettingsGroup
    {
        public override string ElementName { get; }

        public IReadOnlyList<SettingBase> Children => _mutableChildren.Select(c => c.Value).ToList();

        public override bool IsEmpty() => false;

        protected override bool CanHaveChildren => true;

        private Dictionary<SettingBase, SettingBase> _mutableChildren;

        internal UnknownItem(XElement element, SettingsFile origin)
            : base(element, origin)
        {
            ElementName = element.Name.LocalName;
            _mutableChildren = new Dictionary<SettingBase, SettingBase>();

            var descendants = element.Nodes().Where(n => n is XText text && !string.IsNullOrWhiteSpace(text.Value) || n is XElement)
                .Select(d => SettingFactory.Parse(d, origin)).Distinct();

            foreach (var descendant in descendants)
            {
                descendant.Parent = this;

                _mutableChildren.Add(descendant, descendant);
            }
        }

        public UnknownItem(string name, IReadOnlyDictionary<string, string>? attributes, IEnumerable<SettingBase>? children)
            : base(attributes)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(name));
            }

            ElementName = name;
            _mutableChildren = new Dictionary<SettingBase, SettingBase>();

            if (children != null)
            {
                foreach (var child in children)
                {
                    child.Parent = this;
                    _mutableChildren.Add(child, child);
                }
            }
        }

        public override SettingBase Clone()
        {
            var newSetting = new UnknownItem(ElementName, Attributes, Children.Select(c => c.Clone()));

            if (Origin != null)
            {
                newSetting.SetOrigin(Origin);
            }

            return newSetting;
        }

        internal bool Add(SettingBase setting)
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

            if (!_mutableChildren.ContainsKey(setting) && !setting.IsEmpty())
            {
                _mutableChildren.Add(setting, setting);

                if (Origin != null)
                {
                    setting.SetOrigin(Origin);

                    if (Node != null)
                    {
                        setting.SetNode(setting.AsXNode()!);

                        XElementUtility.AddIndented(Node as XElement, setting.Node);
                        Origin.IsDirty = true;
                    }
                }

                setting.Parent = this;

                return true;
            }

            return false;
        }

        void ISettingsGroup.Remove(SettingElement setting)
        {
            Remove(setting);
        }

        internal void Remove(SettingBase setting)
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

            if (_mutableChildren.TryGetValue(setting, out var currentSetting) && _mutableChildren.Remove(currentSetting))
            {
                currentSetting.RemoveFromSettings();

                if (Parent != null && IsEmpty())
                {
                    Parent.Remove(this);
                }
            }
        }

        internal override XNode AsXNode()
        {
            if (Node is XElement)
            {
                return Node;
            }

            var element = new XElement(ElementName, Children.Select(c => c.AsXNode()));
            foreach (var attr in Attributes)
            {
                element.SetAttributeValue(attr.Key, attr.Value);
            }

            return element;
        }

        public override bool Equals(object? other)
        {
            var unknown = other as UnknownItem;

            if (unknown == null)
            {
                return false;
            }

            if (ReferenceEquals(this, unknown))
            {
                return true;
            }

            return string.Equals(ElementName, unknown.ElementName, StringComparison.Ordinal);
        }

        public override int GetHashCode() => ElementName.GetHashCode();

        internal override void Update(SettingItem setting)
        {
            base.Update(setting);

            var unknown = (UnknownItem)setting;

            var otherChildren = new Dictionary<SettingBase, SettingBase>(unknown._mutableChildren);
            foreach (var child in Children)
            {
                if (otherChildren.TryGetValue(child, out var otherChild))
                {
                    otherChildren.Remove(child);
                }

                if (otherChild == null)
                {
                    Remove(child);
                }
                else if (child is SettingItem item)
                {
                    item.Update((SettingItem)otherChild);
                }
            }

            foreach (var newChild in otherChildren)
            {
                Add(newChild.Value);
            }
        }

        internal void Merge(UnknownItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            foreach (var attribute in item.Attributes)
            {
                AddOrUpdateAttribute(attribute.Key, attribute.Value);
            }

            foreach (var child in item.Children)
            {
                if (_mutableChildren.TryGetValue(child, out var existingChild))
                {
                    if (existingChild is SettingItem childItem)
                    {
                        childItem.Update((SettingItem)child);
                    }
                }
                else
                {
                    _mutableChildren.Add(child, child);
                }
            }
        }

        internal override void SetOrigin(SettingsFile origin)
        {
            base.SetOrigin(origin);

            foreach (var child in _mutableChildren)
            {
                child.Value.SetOrigin(origin);
            }
        }

        internal override void RemoveFromSettings()
        {
            base.RemoveFromSettings();

            foreach (var child in _mutableChildren)
            {
                child.Value.RemoveFromSettings();
            }
        }
    }
}
