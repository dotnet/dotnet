// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using NuGet.Common;

#if NET5_0_OR_GREATER
using System.Globalization;
#endif

namespace NuGet.Packaging.Signing
{
    /// <summary>
    /// Represents the X.509 trust store that package signing and signed package verification will use.
    /// </summary>
    public static class X509TrustStore
    {
        private static IX509ChainFactory CodeSigningX509ChainFactory;
        private static IX509ChainFactory TimestampingX509ChainFactory;
        private static readonly object LockObject = new();

        /// <summary>
        /// Initializes the X.509 trust store for NuGet .NET SDK scenarios and logs details about the attempt.
        /// If initialization has already happened, a call to this method will have no effect.
        /// </summary>
        /// <param name="logger">A logger.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="logger" /> is <see langword="null" />.</exception>
        public static void InitializeForDotNetSdk(ILogger logger)
        {
            _ = GetX509ChainFactory(X509StorePurpose.CodeSigning, logger, CreateX509ChainFactoryForDotNetSdk);
            _ = GetX509ChainFactory(X509StorePurpose.Timestamping, logger, CreateX509ChainFactoryForDotNetSdk);
        }

        internal static IX509ChainFactory GetX509ChainFactory(X509StorePurpose storePurpose, ILogger logger)
        {
            return GetX509ChainFactory(storePurpose, logger, CreateX509ChainFactory);
        }

        private static IX509ChainFactory GetX509ChainFactory(
            X509StorePurpose storePurpose,
            ILogger logger,
            Func<X509StorePurpose, ILogger, IX509ChainFactory> creator)
        {
            if (logger is null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            if (storePurpose == X509StorePurpose.CodeSigning)
            {
                if (CodeSigningX509ChainFactory is not null)
                {
                    return CodeSigningX509ChainFactory;
                }

                lock (LockObject)
                {
                    if (CodeSigningX509ChainFactory is not null)
                    {
                        return CodeSigningX509ChainFactory;
                    }

                    CodeSigningX509ChainFactory = creator(storePurpose, logger);
                }

                return CodeSigningX509ChainFactory;
            }

            if (storePurpose == X509StorePurpose.Timestamping)
            {
                if (TimestampingX509ChainFactory is not null)
                {
                    return TimestampingX509ChainFactory;
                }

                lock (LockObject)
                {
                    if (TimestampingX509ChainFactory is not null)
                    {
                        return TimestampingX509ChainFactory;
                    }

                    TimestampingX509ChainFactory = creator(storePurpose, logger);
                }

                return TimestampingX509ChainFactory;
            }

            throw new ArgumentException(Strings.InvalidX509StorePurpose, nameof(storePurpose));
        }

        private static IX509ChainFactory CreateX509ChainFactoryForDotNetSdk(X509StorePurpose storePurpose, ILogger logger)
        {
            return CreateX509ChainFactoryForDotNetSdk(storePurpose, logger, fallbackCertificateBundleFile: null);
        }

        // Non-private for testing purposes only
        internal static IX509ChainFactory CreateX509ChainFactoryForDotNetSdk(
            X509StorePurpose storePurpose,
            ILogger logger,
            FileInfo fallbackCertificateBundleFile)
        {
#if NET5_0_OR_GREATER
            if (RuntimeEnvironmentHelper.IsLinux)
            {
                // System certificate bundle probe paths only support code signing not timestamping.
                if (storePurpose == X509StorePurpose.CodeSigning &&
                    SystemCertificateBundleX509ChainFactory.TryCreate(
                    out SystemCertificateBundleX509ChainFactory systemBundleFactory))
                {
                    logger.LogInformation(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Strings.ChainBuilding_UsingSystemCertificateBundle,
                            systemBundleFactory.FilePath));

                    return systemBundleFactory;
                }

                if (FallbackCertificateBundleX509ChainFactory.TryCreate(
                    storePurpose,
                    fallbackCertificateBundleFile?.FullName,
                    out FallbackCertificateBundleX509ChainFactory fallbackBundleFactory))
                {
                    logger.LogInformation(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Strings.ChainBuilding_UsingFallbackCertificateBundle,
                            fallbackBundleFactory.FilePath));

                    return fallbackBundleFactory;
                }

                logger.LogInformation(Strings.ChainBuilding_UsingNoCertificateBundle);

                return new NoCertificateBundleX509ChainFactory();
            }

            if (RuntimeEnvironmentHelper.IsMacOSX)
            {
                if (FallbackCertificateBundleX509ChainFactory.TryCreate(
                    storePurpose,
                    fallbackCertificateBundleFile?.FullName,
                    out FallbackCertificateBundleX509ChainFactory fallbackBundleFactory))
                {
                    logger.LogInformation(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Strings.ChainBuilding_UsingFallbackCertificateBundle,
                            fallbackBundleFactory.FilePath));

                    return fallbackBundleFactory;
                }

                logger.LogInformation(Strings.ChainBuilding_UsingNoCertificateBundle);

                return new NoCertificateBundleX509ChainFactory();
            }
#endif

            return CreateX509ChainFactory(storePurpose, logger);
        }

        // Non-private for testing purposes only
        internal static IX509ChainFactory CreateX509ChainFactory(X509StorePurpose storePurpose, ILogger logger)
        {
            switch (storePurpose)
            {
                case X509StorePurpose.CodeSigning:
                    logger.LogInformation(Strings.ChainBuilding_UsingDefaultTrustStoreForCodeSigning);
                    break;

                case X509StorePurpose.Timestamping:
                    logger.LogInformation(Strings.ChainBuilding_UsingDefaultTrustStoreForTimestamping);
                    break;
            }

            return new DotNetDefaultTrustStoreX509ChainFactory();
        }

        // Only for testing
        internal static void SetCodeSigningX509ChainFactory(IX509ChainFactory chainFactory)
        {
            lock (LockObject)
            {
                CodeSigningX509ChainFactory = chainFactory;
            }
        }

        // Only for testing
        internal static void SetTimestampingX509ChainFactory(IX509ChainFactory chainFactory)
        {
            lock (LockObject)
            {
                TimestampingX509ChainFactory = chainFactory;
            }
        }
    }
}
