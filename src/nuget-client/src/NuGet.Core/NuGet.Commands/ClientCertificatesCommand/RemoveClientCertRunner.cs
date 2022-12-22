// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using NuGet.Common;
using NuGet.Configuration;

namespace NuGet.Commands
{
    public static class RemoveClientCertRunner
    {
        public static void Run(RemoveClientCertArgs args, Func<ILogger> getLogger)
        {
            args.Validate();

            var settings = RunnerHelper.GetSettings(args.Configfile);
            var clientCertificateProvider = new ClientCertificateProvider(settings);

            var item = clientCertificateProvider.GetClientCertificate(args.PackageSource);
            if (item == null)
            {
                getLogger().LogInformation(string.Format(CultureInfo.CurrentCulture,
                                                         Strings.NoClientCertificatesMatching,
                                                         args.PackageSource));
                return;
            }

            clientCertificateProvider.Remove(new[] { item });

            getLogger().LogInformation(string.Format(CultureInfo.CurrentCulture, Strings.ClientCertificateSuccessfullyRemoved, args.PackageSource));
        }
    }
}
