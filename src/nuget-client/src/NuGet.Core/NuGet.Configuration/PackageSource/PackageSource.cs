// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using NuGet.Common;

namespace NuGet.Configuration
{
    public class PackageSource : IEquatable<PackageSource>
    {
        /// <summary>
        /// The feed version for NuGet prior to v3.
        /// </summary>
        public const int DefaultProtocolVersion = 2;

        private readonly int _hashCode;

        private bool? _isHttp;
        private bool? _isHttps;
        private bool? _isLocal;

        public string Name { get; private set; }

        public string Source { get; set; }

        /// <summary>
        /// Returns null if Source is an invalid URI
        /// </summary>
        public Uri TrySourceAsUri => UriUtility.TryCreateSourceUri(Source, UriKind.Absolute);

        /// <summary>
        /// Throws if Source is an invalid URI
        /// </summary>
        public Uri SourceUri => UriUtility.CreateSourceUri(Source, UriKind.Absolute);

        /// <summary>
        /// This does not represent just the NuGet Official Feed alone
        /// It may also represent a Default Package Source set by Configuration Defaults
        /// </summary>
        public bool IsOfficial { get; set; }

        public bool IsMachineWide { get; set; }

        public bool IsEnabled { get; set; }

        public PackageSourceCredential Credentials { get; set; }

        public string Description { get; set; }

        public bool IsPersistable { get; private set; }

        public int MaxHttpRequestsPerSource { get; set; }

        public IReadOnlyList<X509Certificate> ClientCertificates { get; set; }

        /// <summary>
        /// Gets or sets the protocol version of the source. Defaults to 2.
        /// </summary>
        public int ProtocolVersion { get; set; } = DefaultProtocolVersion;

        /// <summary>
        /// Whether the source is using the HTTP protocol, including HTTPS.
        /// </summary>
        public bool IsHttp
        {
            get
            {
                if (!_isHttp.HasValue)
                {
                    _isHttp = IsHttps || Source.StartsWith("http://", StringComparison.OrdinalIgnoreCase);
                }

                return _isHttp.Value;
            }
        }

        /// <summary>
        /// Whether the source is using the HTTPS protocol.
        /// </summary>
        public bool IsHttps
        {
            get
            {
                if (!_isHttps.HasValue)
                {
                    _isHttps = Source.StartsWith("https://", StringComparison.OrdinalIgnoreCase);
                    if (_isHttps == true)
                    {
                        _isHttp = true;
                    }
                }

                return _isHttps.Value;
            }
        }

        /// <summary>
        /// True if the source path is file based. Unc shares are not included.
        /// </summary>
        public bool IsLocal
        {
            get
            {
                if (!_isLocal.HasValue)
                {
                    var uri = TrySourceAsUri;
                    if (uri != null)
                    {
                        _isLocal = uri.IsFile;
                    }
                    else
                    {
                        _isLocal = false;
                    }
                }

                return _isLocal.Value;
            }
        }

        public PackageSource(string source)
            : this(source, source, isEnabled: true)
        {
        }

        public PackageSource(string source, string name)
            : this(source, name, isEnabled: true)
        {
        }

        public PackageSource(string source, string name, bool isEnabled)
            : this(source, name, isEnabled, isOfficial: false)
        {
        }

        public PackageSource(
            string source,
            string name,
            bool isEnabled,
            bool isOfficial,
            bool isPersistable = true)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Source = source ?? throw new ArgumentNullException(nameof(source));
            IsEnabled = isEnabled;
            IsOfficial = isOfficial;
            IsPersistable = isPersistable;
            _hashCode = Name.ToUpperInvariant().GetHashCode() * 3137 + Source.ToUpperInvariant().GetHashCode();
        }

        public SourceItem AsSourceItem()
        {
            string protocolVersion = null;
            if (ProtocolVersion != DefaultProtocolVersion)
            {
                protocolVersion = $"{ProtocolVersion}";
            }
            return new SourceItem(Name, Source, protocolVersion);
        }

        public bool Equals(PackageSource other)
        {
            if (other == null)
            {
                return false;
            }

            return Name.Equals(other.Name, StringComparison.CurrentCultureIgnoreCase) &&
                   Source.Equals(other.Source, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            var source = obj as PackageSource;
            if (source != null)
            {
                return Equals(source);
            }
            return base.Equals(obj);
        }

        public override string ToString()
        {
            return Name + " [" + Source + "]";
        }

        public override int GetHashCode()
        {
            return _hashCode;
        }

        public PackageSource Clone()
        {
            return new PackageSource(Source, Name, IsEnabled, IsOfficial, IsPersistable)
            {
                Description = Description,
                Credentials = Credentials?.Clone(),
                ClientCertificates = ClientCertificates?.ToList(),
                IsMachineWide = IsMachineWide,
                ProtocolVersion = ProtocolVersion,
            };
        }
    }
}
