// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using MessagePack;
using MessagePack.Formatters;
using MessagePack.Resolvers;
using Microsoft;
using Microsoft.ServiceHub.Framework;
using StreamJsonRpc;

namespace NuGet.VisualStudio.Internal.Contracts
{
    internal class NuGetServiceMessagePackRpcDescriptor : ServiceJsonRpcDescriptor
    {
        private static readonly Lazy<MessagePackSerializerOptions> MessagePackSerializerOptions =
            new Lazy<MessagePackSerializerOptions>(CreateMessagePackSerializerOptions);

        internal NuGetServiceMessagePackRpcDescriptor(ServiceMoniker serviceMoniker)
            : base(serviceMoniker, Formatters.MessagePack, MessageDelimiters.BigEndianInt32LengthHeader)
        {
        }

        internal NuGetServiceMessagePackRpcDescriptor(ServiceMoniker serviceMoniker, Type clientInterface)
            : base(serviceMoniker, clientInterface, Formatters.MessagePack, MessageDelimiters.BigEndianInt32LengthHeader)
        {
        }

        protected NuGetServiceMessagePackRpcDescriptor(NuGetServiceMessagePackRpcDescriptor copyFrom)
            : base(copyFrom)
        {
        }

        protected override ServiceRpcDescriptor Clone() => new NuGetServiceMessagePackRpcDescriptor(this);

        protected override IJsonRpcMessageFormatter CreateFormatter()
        {
            Assumes.True(Formatter == Formatters.MessagePack);

            MessagePackFormatter formatter = base.CreateFormatter() as MessagePackFormatter ?? new MessagePackFormatter();
            MessagePackSerializerOptions options = MessagePackSerializerOptions.Value;

            formatter.SetMessagePackSerializerOptions(options);

            return formatter;
        }

        protected override JsonRpc CreateJsonRpc(IJsonRpcMessageHandler handler)
        {
            return new NuGetJsonRpc(handler);
        }

        // Non-private for testing purposes only.
        internal static IMessagePackFormatter[] CreateMessagePackFormatters()
        {
            return new IMessagePackFormatter[]
            {
                AlternatePackageMetadataContextInfoFormatter.Instance,
                FloatRangeFormatter.Instance,
                IInstalledAndTransitivePackagesFormatter.Instance,
                ILogMessageFormatter.Instance,
                ImplicitProjectActionFormatter.Instance,
                IPackageReferenceContextInfoFormatter.Instance,
                IProjectContextInfoFormatter.Instance,
                IProjectMetadataContextInfoFormatter.Instance,
                ITransitivePackageReferenceContextInfoFormatter.Instance,
                LicenseMetadataFormatter.Instance,
                NuGetFrameworkFormatter.Instance,
                NuGetVersionFormatter.Instance,
                PackageDependencyFormatter.Instance,
                PackageDependencyGroupFormatter.Instance,
                PackageDependencyInfoFormatter.Instance,
                PackageDeprecationMetadataContextInfoFormatter.Instance,
                PackageIdentityFormatter.Instance,
                PackageReferenceFormatter.Instance,
                PackageSearchMetadataContextInfoFormatter.Instance,
                PackageSourceContextInfoFormatter.Instance,
                PackageSourceFormatter.Instance,
                PackageVulnerabilityMetadataContextInfoFormatter.Instance,
                ProjectActionFormatter.Instance,
                RemoteErrorFormatter.Instance,
                SearchFilterFormatter.Instance,
                SearchResultContextInfoFormatter.Instance,
                VersionInfoContextInfoFormatter.Instance,
                VersionRangeFormatter.Instance
            };
        }

        private static MessagePackSerializerOptions CreateMessagePackSerializerOptions()
        {
            IMessagePackFormatter[] formatters = CreateMessagePackFormatters();
            var resolvers = new IFormatterResolver[] { MessagePack.MessagePackSerializerOptions.Standard.Resolver };

            return MessagePack.MessagePackSerializerOptions.Standard
                .WithSecurity(MessagePackSecurity.UntrustedData)
                .WithResolver(CompositeResolver.Create(formatters, resolvers));
        }
    }
}
