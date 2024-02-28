// Copyright(c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;

namespace NuGet.Configuration
{
    public class AddItem : SettingItem
    {
        public override string ElementName => ConfigurationConstants.Add;

        public string Key => Attributes[ConfigurationConstants.KeyAttribute];

        public virtual string Value
        {
            get => Settings.ApplyEnvironmentTransform(Attributes[ConfigurationConstants.ValueAttribute]);
            set => AddOrUpdateAttribute(ConfigurationConstants.ValueAttribute, value ?? string.Empty);
        }

        public IReadOnlyDictionary<string, string> AdditionalAttributes => new ReadOnlyDictionary<string, string>(
            Attributes.Where(a =>
                !string.Equals(a.Key, ConfigurationConstants.KeyAttribute, StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(a.Key, ConfigurationConstants.ValueAttribute, StringComparison.OrdinalIgnoreCase)
            ).ToDictionary(a => a.Key, a => a.Value));

        protected override IReadOnlyCollection<string> RequiredAttributes { get; }
            = new HashSet<string>(new[] { ConfigurationConstants.KeyAttribute, ConfigurationConstants.ValueAttribute });

        protected override IReadOnlyDictionary<string, IReadOnlyCollection<string>> DisallowedValues { get; } = new ReadOnlyDictionary<string, IReadOnlyCollection<string>>(
            new Dictionary<string, IReadOnlyCollection<string>>()
            {
                { ConfigurationConstants.KeyAttribute, new HashSet<string>(new [] {string.Empty }) }
            });

        public AddItem(string key, string? value)
            : this(key, value, additionalAttributes: null)
        {
        }

        internal AddItem(XElement element, SettingsFile origin)
            : base(element, origin)
        {
        }

        public AddItem(string key, string? value, IReadOnlyDictionary<string, string>? additionalAttributes)
            : base()
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(key));
            }

            AddAttribute(ConfigurationConstants.KeyAttribute, key);
            AddAttribute(ConfigurationConstants.ValueAttribute, value ?? string.Empty);

            if (additionalAttributes != null)
            {
                foreach (var attribute in additionalAttributes)
                {
                    AddAttribute(attribute.Key, attribute.Value);
                }
            }
        }

        public virtual string GetValueAsPath()
        {
            if (Origin != null)
            {
                return Settings.ResolvePathFromOrigin(Origin.DirectoryPath, Origin.ConfigFilePath, Value);
            }

            return Value;
        }

        public void AddOrUpdateAdditionalAttribute(string attributeName, string value)
        {
            if (Origin != null && Origin.IsReadOnly)
            {
                return;
            }

            if (string.Equals(ConfigurationConstants.KeyAttribute, attributeName, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(ConfigurationConstants.ValueAttribute, attributeName, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            if (Attributes.ContainsKey(attributeName))
            {
                UpdateAttribute(attributeName, value);
            }
            else
            {
                AddAttribute(attributeName, value);
            }
        }

        // Due to how settings AddOrUpdate is implemented, anything extending AddItem must only use the Key in GetHashCode and Equals
        // otherwise existing items won't be updated and new duplicates will always be added.
        // In other words, all sub-class properties must be ignored by these methods.
        public sealed override bool Equals(object? other)
        {
            var item = other as AddItem;

            if (item == null || item.GetType() != GetType())
            {
                return false;
            }

            if (ReferenceEquals(this, item))
            {
                return true;
            }

            return string.Equals(Key, item.Key, StringComparison.Ordinal);
        }

        // Due to how settings AddOrUpdate is implemented, anything extending AddItem must only use the Key in GetHashCode and Equals
        // otherwise existing items won't be updated and new duplicates will always be added.
        // In other words, all sub-class properties must be ignored by these methods.
        public sealed override int GetHashCode() => Key.GetHashCode();

        public override SettingBase Clone()
        {
            var newItem = new AddItem(Key, Value, AdditionalAttributes);

            if (Origin != null)
            {
                newItem.SetOrigin(Origin);
            }

            return newItem;
        }

        internal override void Update(SettingItem other)
        {
            base.Update(other);

            if ((!other.Attributes.TryGetValue(ConfigurationConstants.ValueAttribute, out var value) ||
                string.IsNullOrEmpty(value)) && Parent != null)
            {
                Parent.Remove(this);
            }
        }
    }
}
