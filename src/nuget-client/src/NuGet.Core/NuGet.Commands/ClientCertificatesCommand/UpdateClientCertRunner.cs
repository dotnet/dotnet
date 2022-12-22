// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Linq;
using NuGet.Common;
using NuGet.Configuration;

namespace NuGet.Commands
{
    public static class UpdateClientCertRunner
    {
        public static void Run(UpdateClientCertArgs args, Func<ILogger> getLogger)
        {
            args.Validate();

            var settings = RunnerHelper.GetSettings(args.Configfile);
            var clientCertificateProvider = new ClientCertificateProvider(settings);

            var item = clientCertificateProvider.GetClientCertificate(args.PackageSource);

            if (item == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                                                          Strings.Error_ClientCertificateNotExist,
                                                          args.PackageSource));
            }

            switch (item)
            {
                case FileClientCertItem fileCertItem:
                    if (args.IsStoreCertSettingsProvided())
                    {
                        throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                                                                  Strings.Error_ClientCertificateTypeMismatch,
                                                                  args.PackageSource));
                    }

                    fileCertItem.Update(args.Path, args.Password, args.StorePasswordInClearText);
                    break;
                case StoreClientCertItem storeCertItem:
                    if (args.IsFileCertSettingsProvided())
                    {
                        throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                                                                  Strings.Error_ClientCertificateTypeMismatch,
                                                                  args.PackageSource));
                    }

                    storeCertItem.Update(args.FindValue,
                                         args.GetStoreLocation(),
                                         args.GetStoreName(),
                                         args.GetFindBy());
                    break;
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

            getLogger().LogInformation(string.Format(CultureInfo.CurrentCulture, Strings.ClientCertificateSuccessfullyUpdated, args.PackageSource));
        }
    }
}
