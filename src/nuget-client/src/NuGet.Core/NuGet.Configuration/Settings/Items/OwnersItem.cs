// Copyright(c) .NET Foundation.All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using NuGet.Shared;

namespace NuGet.Configuration
{
    public sealed class OwnersItem : SettingItem
    {
        public static readonly char OwnersListSeparator = ';';

        public override string ElementName => ConfigurationConstants.Owners;

        protected override bool CanHaveChildren => true;

        public IList<string> Content { get; private set; }

        private SettingText _content;

        public OwnersItem(string owners)
            : base()
        {
            if (string.IsNullOrEmpty(owners))
            {
                throw new ArgumentException(Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(owners));
            }

            _content = new SettingText(owners);

            Content = owners.Split(OwnersListSeparator).Select(o => o.Trim()).ToList();
        }

        internal OwnersItem(XElement element, SettingsFile origin)
            : base(element, origin)
        {
            var descendants = element.Nodes().Where(n => n is XText text && !string.IsNullOrWhiteSpace(text.Value) || n is XElement)
                .Select(d => SettingFactory.Parse(d, origin)).Distinct();

            if (descendants == null || descendants.Count() != 1 || descendants.FirstOrDefault(d => d is SettingText) == null)
            {
                throw new NuGetConfigurationException(
                    string.Format(CultureInfo.CurrentCulture, Resources.UserSettings_UnableToParseConfigFile, Resources.OwnersMustOnlyHaveContent, origin.ConfigFilePath));
            }

            _content = descendants.OfType<SettingText>().First();

            Content = _content.Value.Split(OwnersListSeparator).Select(o => o.Trim()).ToList();
        }

        public override SettingBase Clone()
        {
            var newItem = new OwnersItem(_content.Value);

            if (Origin != null)
            {
                newItem.SetOrigin(Origin);
            }

            return newItem;
        }

        internal override XNode AsXNode()
        {
            if (Node is XElement)
            {
                return Node;
            }

            _content.Value = string.Join(OwnersListSeparator.ToString(CultureInfo.CurrentCulture), Content);

            var element = new XElement(ElementName, _content.AsXNode());

            foreach (var attr in Attributes)
            {
                element.SetAttributeValue(attr.Key, attr.Value);
            }

            return element;
        }

        public override bool Equals(object other)
        {
            if (other is OwnersItem owners)
            {
                if (ReferenceEquals(this, owners))
                {
                    return true;
                }

                return Content.OrderedEquals(owners.Content, o => o, StringComparer.Ordinal, StringComparer.Ordinal);
            }

            return false;
        }

        public override int GetHashCode() => Content.GetHashCode();

        internal override void Update(SettingItem other)
        {
            var owners = other as OwnersItem;

            if (!owners.Content.Any())
            {
                throw new InvalidOperationException(Resources.OwnersItemMustHaveAtLeastOneOwner);
            }

            base.Update(other);

            if (!Equals(owners))
            {
                XElementUtility.RemoveIndented(_content.Node);
                Content = owners.Content;

                _content = new SettingText(string.Join(OwnersListSeparator.ToString(CultureInfo.CurrentCulture), Content));

                if (Origin != null)
                {
                    _content.SetOrigin(Origin);

                    if (Node != null)
                    {
                        _content.SetNode(_content.AsXNode());
                        (Node as XElement).Add(_content.Node);
                        Origin.IsDirty = true;
                    }
                }
            }
        }
    }
}
