// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using NuGet.Common;
using NuGet.Configuration;

namespace NuGet.Commands
{
    public static class ClientCertArgsExtensions
    {
        public static X509FindType? GetFindBy(this IClientCertArgsWithStoreData args)
        {
            if (Enum.TryParse("FindBy" + args.FindBy, ignoreCase: true, result: out X509FindType value))
            {
                return value;
            }

            return null;
        }

        public static StoreLocation? GetStoreLocation(this IClientCertArgsWithStoreData args)
        {
            if (Enum.TryParse(args.StoreLocation, ignoreCase: true, result: out StoreLocation value))
            {
                return value;
            }

            return null;
        }

        public static StoreName? GetStoreName(this IClientCertArgsWithStoreData args)
        {
            if (Enum.TryParse(args.StoreName, ignoreCase: true, result: out StoreName value))
            {
                return value;
            }

            return null;
        }

        public static bool IsFileCertSettingsProvided(this IClientCertArgsWithFileData args)
        {
            var isFilePathProvided = !string.IsNullOrEmpty(args.Path);
            var isPasswordProvided = !string.IsNullOrEmpty(args.Password);

            return isFilePathProvided || isPasswordProvided;
        }

        public static bool IsPackageSourceSettingProvided(this IClientCertArgsWithPackageSource args)
        {
            return !string.IsNullOrEmpty(args.PackageSource);
        }

        public static bool IsStoreCertSettingsProvided(this IClientCertArgsWithStoreData args)
        {
            var isStoreLocationProvided = false;
            var isStoreNameProvided = false;
            var isFindTypeProvided = false;

            if (!string.IsNullOrWhiteSpace(args.StoreLocation))
            {
                var storeLocation = args.GetStoreLocation();
                if (storeLocation == null)
                {
                    throw new CommandLineArgumentCombinationException(string.Format(CultureInfo.CurrentCulture,
                                                                                    Strings.Error_UnknownClientCertificatesStoreLocation,
                                                                                    args.StoreLocation));
                }

                isStoreLocationProvided = true;
            }

            if (!string.IsNullOrWhiteSpace(args.StoreName))
            {
                var storeName = args.GetStoreName();
                if (storeName == null)
                {
                    throw new CommandLineArgumentCombinationException(string.Format(CultureInfo.CurrentCulture,
                                                                                    Strings.Error_UnknownClientCertificatesStoreName,
                                                                                    args.StoreName));
                }

                isStoreNameProvided = true;
            }

            if (!string.IsNullOrWhiteSpace(args.FindBy))
            {
                var findBy = args.GetFindBy();
                if (findBy == null)
                {
                    throw new CommandLineArgumentCombinationException(string.Format(CultureInfo.CurrentCulture,
                                                                                    Strings.Error_UnknownClientCertificatesFindBy,
                                                                                    args.FindBy));
                }

                isFindTypeProvided = true;
            }

            var isFindValueProvided = !string.IsNullOrEmpty(args.FindValue);

            return isStoreLocationProvided || isStoreNameProvided || isFindTypeProvided || isFindValueProvided;
        }

        public static void Validate(this RemoveClientCertArgs args)
        {
            if (!args.IsPackageSourceSettingProvided())
            {
                throw new CommandLineArgumentCombinationException(string.Format(CultureInfo.CurrentCulture,
                                                                                Strings.Error_PropertyCannotBeNullOrEmpty,
                                                                                nameof(PackageSource)));
            }
        }

        public static void Validate(this AddClientCertArgs args)
        {
            if (!args.IsPackageSourceSettingProvided())
            {
                throw new CommandLineArgumentCombinationException(string.Format(CultureInfo.CurrentCulture,
                                                                                Strings.Error_PropertyCannotBeNullOrEmpty,
                                                                                nameof(PackageSource)));
            }

            if (args.IsFileCertSettingsProvided() == args.IsStoreCertSettingsProvided())
            {
                throw new CommandLineArgumentCombinationException(string.Format(CultureInfo.CurrentCulture,
                                                                                Strings.Error_CouldNotUpdateClientCertificate,
                                                                                Strings.Error_InvalidCombinationOfArguments));
            }
        }

        public static void Validate(this UpdateClientCertArgs args)
        {
            if (!args.IsPackageSourceSettingProvided())
            {
                throw new CommandLineArgumentCombinationException(string.Format(CultureInfo.CurrentCulture,
                                                                                Strings.Error_PropertyCannotBeNullOrEmpty,
                                                                                nameof(PackageSource)));
            }

            if (args.IsFileCertSettingsProvided() == args.IsStoreCertSettingsProvided())
            {
                throw new CommandLineArgumentCombinationException(string.Format(CultureInfo.CurrentCulture,
                                                                                Strings.Error_CouldNotUpdateClientCertificate,
                                                                                Strings.Error_InvalidCombinationOfArguments));
            }
        }
    }
}
