// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using NuGet.Common;
using NuGet.Configuration;

namespace NuGet.Commands
{
    public static class ListClientCertRunner
    {
        private const int PaddingWidth = 6;

        public static void Run(ListClientCertArgs args, Func<ILogger> getLogger)
        {
            var settings = RunnerHelper.GetSettings(args.Configfile);
            var clientCertificateProvider = new ClientCertificateProvider(settings);

            var items = clientCertificateProvider.GetClientCertificates();

            if (!items.Any())
            {
                getLogger().LogInformation(Strings.NoClientCertificates);
                return;
            }

            var clientCertificatesLogs = new List<LogMessage>();

            getLogger().LogInformation(Strings.RegsiteredClientCertificates);
            getLogger().LogInformation(string.Empty);

            var defaultIndentation = string.Empty.PadRight(PaddingWidth);

            for (var i = 0; i < items.Count; i++)
            {
                var item = items[i];

                var builder = new StringBuilder();
                var indexIndentation = $" {i + 1}.".PadRight(PaddingWidth);

                builder.AppendFormat(CultureInfo.CurrentCulture, Strings.ClientCertificatesLogTitle, indexIndentation, item.PackageSource, item.ElementName);
                builder.AppendLine();

                switch (item)
                {
                    case FileClientCertItem fileCertItem:
                        {
                            builder.AppendFormat(CultureInfo.CurrentCulture, Strings.ClientCertificatesFileCertFilePath, defaultIndentation, fileCertItem.FilePath);
                            builder.AppendLine();

                            if (string.IsNullOrEmpty(fileCertItem.Password))
                            {
                                builder.AppendFormat(CultureInfo.CurrentCulture, Strings.ClientCertificatesFileCertNoPassword, defaultIndentation);
                            }
                            else
                            {
                                builder.AppendFormat(CultureInfo.CurrentCulture, Strings.ClientCertificatesFileCertWithPassword, defaultIndentation);
                            }

                            builder.AppendLine();

                            break;
                        }
                    case StoreClientCertItem storeCertItem:
                        builder.AppendFormat(CultureInfo.CurrentCulture, Strings.ClientCertificatesStoreCertStoreLocation, defaultIndentation, StoreClientCertItem.GetString(storeCertItem.StoreLocation));
                        builder.AppendLine();
                        builder.AppendFormat(CultureInfo.CurrentCulture, Strings.ClientCertificatesStoreCertStoreName, defaultIndentation, StoreClientCertItem.GetString(storeCertItem.StoreName));
                        builder.AppendLine();
                        builder.AppendFormat(CultureInfo.CurrentCulture, Strings.ClientCertificatesStoreCertFindBy, defaultIndentation, StoreClientCertItem.GetString(storeCertItem.FindType));
                        builder.AppendLine();
                        builder.AppendFormat(CultureInfo.CurrentCulture, Strings.ClientCertificatesStoreCertFindValue, defaultIndentation, storeCertItem.FindValue);
                        builder.AppendLine();
                        break;
                }

                try
                {
                    var certificates = item.Search();
                    foreach (var certificate in certificates)
                    {
                        builder.AppendFormat(CultureInfo.CurrentCulture, Strings.ClientCertificatesItemCertificateMessage, defaultIndentation, certificate.GetCertHashString());
                        builder.AppendLine();
                    }
                }
                catch (Exception e)
                {
                    builder.AppendFormat(CultureInfo.CurrentCulture, Strings.ClientCertificatesItemCertificateError, defaultIndentation, e.GetBaseException().Message);
                    builder.AppendLine();
                }

                clientCertificatesLogs.Add(new LogMessage(LogLevel.Information, builder.ToString()));
            }

            getLogger().LogMessages(clientCertificatesLogs);
        }
    }
}
