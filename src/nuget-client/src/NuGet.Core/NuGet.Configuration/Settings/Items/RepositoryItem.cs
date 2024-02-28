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
    public sealed class RepositoryItem : TrustedSignerItem
    {
        public override string ElementName => ConfigurationConstants.Repository;

        public string ServiceIndex => Attributes[ConfigurationConstants.ServiceIndex];

        public new string Name
        {
            get => base.Name;
            set => SetName(value);
        }

        private OwnersItem? _owners;

        public IList<string> Owners { get; private set; }

        protected override IReadOnlyCollection<string> RequiredAttributes { get; }
            = new HashSet<string>(new[] { ConfigurationConstants.NameAttribute, ConfigurationConstants.ServiceIndex });

        public RepositoryItem(string name, string serviceIndex, params CertificateItem[] certificates)
            : this(name, serviceIndex, owners: null, certificates: certificates)
        {
        }

        public RepositoryItem(string name, string serviceIndex, string? owners, params CertificateItem[] certificates)
            : base(name, certificates)
        {
            if (string.IsNullOrEmpty(serviceIndex))
            {
                throw new ArgumentNullException(nameof(serviceIndex));
            }

            AddAttribute(ConfigurationConstants.ServiceIndex, serviceIndex);

            if (!string.IsNullOrEmpty(owners))
            {
                _owners = new OwnersItem(owners!);
                Owners = _owners.Content;
            }
            else
            {
                _owners = null;
                Owners = new List<string>();
            }
        }

        internal RepositoryItem(XElement element, SettingsFile origin)
            : this(element, origin, ParseDescendants(element, origin))
        {
        }

        internal RepositoryItem(XElement element, SettingsFile origin, IEnumerable<SettingBase> parsedDescendants)
            : base(element, origin, parsedDescendants)
        {
            var parsedOwners = parsedDescendants.OfType<OwnersItem>();

            if (parsedOwners != null && parsedOwners.Any())
            {
                if (parsedOwners.Count() > 1)
                {
                    throw new NuGetConfigurationException(
                        string.Format(CultureInfo.CurrentCulture, Resources.UserSettings_UnableToParseConfigFile,
                            string.Format(CultureInfo.CurrentCulture, Resources.RepositoryMustHaveOneOwners, Name, ServiceIndex),
                            origin.ConfigFilePath));
                }

                _owners = parsedOwners.FirstOrDefault();
                Owners = _owners?.Content ?? new List<string>();
            }
            else
            {
                Owners = new List<string>();
            }
        }

        public override SettingBase Clone()
        {
            var newItem = new RepositoryItem(
                Name,
                ServiceIndex,
                string.Join(OwnersItem.OwnersListSeparator.ToString(CultureInfo.CurrentCulture), Owners),
                Certificates.Select(c => (CertificateItem)c.Clone()).ToArray());

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

            var element = new XElement(ElementName);

            foreach (var cert in Certificates)
            {
                element.Add(cert.AsXNode());
            }

            if (_owners != null)
            {
                element.Add(_owners.AsXNode());
            }

            foreach (var attr in Attributes)
            {
                element.SetAttributeValue(attr.Key, attr.Value);
            }

            return element;
        }

        public override bool Equals(object? other)
        {
            if (other is RepositoryItem repository)
            {
                if (ReferenceEquals(this, repository))
                {
                    return true;
                }

                return string.Equals(ServiceIndex, repository.ServiceIndex, StringComparison.Ordinal);
            }

            return false;
        }

        public override int GetHashCode()
        {
            var combiner = new HashCodeCombiner();

            combiner.AddObject(Name);
            combiner.AddObject(ServiceIndex);

            return combiner.CombinedHash;
        }

        internal override void SetOrigin(SettingsFile origin)
        {
            base.SetOrigin(origin);

            _owners?.SetOrigin(origin);
        }

        internal override void RemoveFromSettings()
        {
            base.RemoveFromSettings();

            _owners?.RemoveFromSettings();
        }

        internal override void Update(SettingItem otherRepository)
        {
            base.Update(otherRepository);

            var other = (RepositoryItem)otherRepository;

            if (!Owners.SequenceEqual(other.Owners, StringComparer.Ordinal))
            {
                if (_owners == null || !Owners.Any())
                {
                    _owners = new OwnersItem(string.Join(OwnersItem.OwnersListSeparator.ToString(CultureInfo.CurrentCulture), other.Owners));
                    Owners = _owners.Content;

                    if (Origin != null)
                    {
                        _owners.SetOrigin(Origin);

                        if (Node != null)
                        {
                            _owners.SetNode(_owners.AsXNode());

                            XElementUtility.AddIndented(Node as XElement, _owners.Node);
                            Origin.IsDirty = true;
                        }
                    }
                }
                else if (!other.Owners.Any())
                {
                    XElementUtility.RemoveIndented(_owners.Node);
                    _owners = null;
                    Owners.Clear();

                    if (Origin != null)
                    {
                        Origin.IsDirty = true;
                    }
                }
                else
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    // While adding nullable annotations to the codebase, the compiler is warning about this line, but after a long time
                    // trying to understand this code, I can't convince myself that this is safe. It's also not obvious to me how to fix it.
                    // Since this code has existed for years, apparently without causing NullReferenceExeptions, I'm going to suppress the warning
                    // until someone can refactor this code to make it safe.
                    _owners.Update(other._owners);
#pragma warning restore CS8604 // Possible null reference argument.
                    Owners = _owners.Content;
                }
            }
        }
    }
}
