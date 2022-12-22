// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Linq;
using NuGet.Common;
using NuGet.Configuration;

namespace NuGet.Commands
{
    public static class AddClientCertRunner
    {
        public static void Run(AddClientCertArgs args, Func<ILogger> getLogger)
        {
            args.Validate();
            var settings = RunnerHelper.GetSettings(args.Configfile);
            var clientCertificateProvider = new ClientCertificateProvider(settings);

            var item = clientCertificateProvider.GetClientCertificate(args.PackageSource);

            if (item != null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                                                          Strings.Error_ClientCertificateAlreadyExist,
                                                          args.PackageSource));
            }

            if (args.IsFileCertSettingsProvided())
            {
                item = new FileClientCertItem(args.PackageSource,
                                        args.Path,
                                        args.Password,
                                        args.StorePasswordInClearText,
                                        args.Configfile);
            }
            else if (args.IsStoreCertSettingsProvided())
            {
                item = new StoreClientCertItem(args.PackageSource,
                                         args.FindValue,
                                         args.GetStoreLocation(),
                                         args.GetStoreName(),
                                         args.GetFindBy());
            }
            else
            {
                throw new CommandLineArgumentCombinationException(string.Format(CultureInfo.CurrentCulture,
                                                                                Strings.Error_UnknownClientCertificateStoreType));
            }

            try
            {
                var certificates = item.Search();
                if (!certificates.Any())
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.Error_ClientCertificatesNotFound));
                }
            }
            catch
            {
                if (!args.Force)
                {
                    throw;
                }
            }


            clientCertificateProvider.AddOrUpdate(item);

            getLogger().LogInformation(string.Format(CultureInfo.CurrentCulture, Strings.ClientCertificateSuccessfullyAdded, args.PackageSource));
        }
    }
}
