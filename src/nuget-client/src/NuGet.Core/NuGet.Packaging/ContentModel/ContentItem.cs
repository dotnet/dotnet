// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System.Collections.Generic;

namespace NuGet.ContentModel
{
    public class ContentItem
    {
        // These must match ManagedCodeConventions.PropertyNames. They cannot be used since they're static and switch requires const.
        internal const string TargetFrameworkMoniker = "tfm";
        internal const string RuntimeIdentifier = "rid";
        internal const string AnyValue = "any";
        internal const string ManagedAssembly = "assembly";
        internal const string Locale = "locale";
        internal const string MSBuild = "msbuild";
        internal const string SatelliteAssembly = "satelliteAssembly";
        internal const string CodeLanguage = "codeLanguage";

        // These do not map to ManagedCodeConventions, they're used internally
        internal const string Related = "related";
        internal const string TfmRaw = "tfm_raw";

        public required string Path { get; init; }

        internal Dictionary<string, object>? _properties;

        public Dictionary<string, object> Properties
        {
            get => _properties ?? CreateDictionary();
            internal set => _properties = value;
        }

        internal object? _assembly;
        internal object? _locale;
        internal object? _related;
        internal object? _msbuild;
        internal object? _tfm;
        internal object? _rid;
        internal object? _any;
        internal object? _satelliteAssembly;
        internal object? _codeLanguage;
        internal object? _tfmRaw;

        internal bool TryGetValue(string key, out object? value)
        {
            if (_properties != null)
            {
                return _properties.TryGetValue(key, out value);
            }

            bool found = true;
            switch (key)
            {
                case TargetFrameworkMoniker:
                    value = _tfm;
                    break;
                case RuntimeIdentifier:
                    value = _rid;
                    break;
                case AnyValue:
                    value = _any;
                    break;
                case ManagedAssembly:
                    value = _assembly;
                    break;
                case Locale:
                    value = _locale;
                    break;
                case MSBuild:
                    value = _msbuild;
                    break;
                case Related:
                    value = _related;
                    break;
                case SatelliteAssembly:
                    value = _satelliteAssembly;
                    break;
                case CodeLanguage:
                    value = _codeLanguage;
                    break;
                case TfmRaw:
                    value = _tfmRaw;
                    break;
                default:
                    value = null;
                    found = false;
                    break;
            }

            if (found && value != null)
            {
                return true;
            }

            return false;
        }

        internal void Add(string key, object value)
        {
            if (_properties != null)
            {
                _properties.Add(key, value);
            }
            else
            {
                switch (key)
                {
                    case TargetFrameworkMoniker:
                        _tfm = value;
                        break;
                    case RuntimeIdentifier:
                        _rid = value;
                        break;
                    case AnyValue:
                        _any = value;
                        break;
                    case ManagedAssembly:
                        _assembly = value;
                        break;
                    case Locale:
                        _locale = value;
                        break;
                    case MSBuild:
                        _msbuild = value;
                        break;
                    case Related:
                        _related = value;
                        break;
                    case SatelliteAssembly:
                        _satelliteAssembly = value;
                        break;
                    case CodeLanguage:
                        _codeLanguage = value;
                        break;
                    case TfmRaw:
                        _tfmRaw = value;
                        break;
                    default:
                        Properties.Add(key, value); // A property we can't pack means we should be using the dictionary instead.
                        break;
                }
            }
        }

        public override string ToString()
        {
            return Path;
        }

        private Dictionary<string, object> CreateDictionary()
        {
            var properties = new Dictionary<string, object>();
            if (_assembly != null) // We always initialize the dictionary with the packed values. 
            {
                properties.Add(ManagedAssembly, _assembly);
            }
            if (_locale != null)
            {
                properties.Add(Locale, _locale);
            }
            if (_related != null)
            {
                properties.Add(Related, _related);
            }
            if (_msbuild != null)
            {
                properties.Add(MSBuild, _msbuild);
            }
            if (_tfm != null)
            {
                properties.Add(TargetFrameworkMoniker, _tfm);
            }
            if (_rid != null)
            {
                properties.Add(RuntimeIdentifier, _rid);
            }
            if (_any != null)
            {
                properties.Add(AnyValue, _any);
            }
            if (_satelliteAssembly != null)
            {
                properties.Add(SatelliteAssembly, _satelliteAssembly);
            }
            if (_codeLanguage != null)
            {
                properties.Add(CodeLanguage, _codeLanguage);
            }
            if (_tfmRaw != null)
            {
                properties.Add(TfmRaw, _tfmRaw);
            }
            _properties = properties;
            return properties;
        }
    }
}
