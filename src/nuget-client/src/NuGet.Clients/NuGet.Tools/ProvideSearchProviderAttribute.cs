// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using Microsoft.VisualStudio.Shell;

namespace NuGetVSExtension
{
    public class ProvideSearchProviderAttribute : RegistrationAttribute
    {
        private const string ExtensionProvidersKey = "SearchProviders";
        private readonly string _providerTypeGuid;
        private readonly string _providerName;

        public ProvideSearchProviderAttribute(Type providerType, string providerName)
        {
            if (providerType == null)
            {
                throw new ArgumentNullException(nameof(providerType));
            }

            if (String.IsNullOrEmpty(providerName))
            {
                throw new ArgumentException("'providerName' cannot be null or empty.");
            }

            _providerTypeGuid = providerType.GUID.ToString("B", provider: null);
            _providerName = providerName;
        }

        public override void Register(RegistrationContext context)
        {
            using (Key key = context.CreateKey(ExtensionProvidersKey))
            {
                using (Key subKey = key.CreateSubkey(_providerTypeGuid))
                {
                    subKey.SetValue(String.Empty, _providerName);
                    subKey.SetValue("Package", context.ComponentType.GUID.ToString("B", null));
                }
            }
        }

        public override void Unregister(RegistrationContext context)
        {
            context.RemoveKey(String.Format(CultureInfo.CurrentCulture, @"{0}\{1}", ExtensionProvidersKey, _providerTypeGuid));
        }
    }
}
