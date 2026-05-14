// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using MessagePack.Formatters;
using Xunit;

namespace NuGet.VisualStudio.Internal.Contracts.Test
{
    public class NuGetServiceMessagePackRpcDescriptorTests
    {
        [Fact]
        public void CreateMessagePackFormatters_Always_RegistersAllFormatters()
        {
            IMessagePackFormatter[] registeredFormatters = NuGetServiceMessagePackRpcDescriptor.CreateMessagePackFormatters();
            IReadOnlyList<Type> definedFormatters = FindDefinedMessagePackFormatters();

            HashSet<string> registeredFormatterNames = registeredFormatters
                .Select(formatter => formatter.GetType().FullName)
                .ToHashSet();

            HashSet<string> definedFormatterNames = definedFormatters
                .Where(definedFormatter => !definedFormatter.Equals(typeof(NuGetMessagePackFormatter<>))) // Exclude the base class.
                .Select(formatter => formatter.FullName)
                .ToHashSet();

            definedFormatterNames.ExceptWith(registeredFormatterNames);

            if (definedFormatterNames.Count > 0)
            {
                var message = new StringBuilder($"The following formatters are not registered.  Did you define a new one and forget to register it?");

                message.AppendLine();

                foreach (string definedFormatterName in definedFormatterNames)
                {
                    message.AppendLine(definedFormatterName);
                }

                Assert.Fail(message.ToString());
            }
        }

        private static IReadOnlyList<Type> FindDefinedMessagePackFormatters()
        {
            Assembly assembly = typeof(NuGetServiceMessagePackRpcDescriptor).Assembly;

            return assembly.GetTypes()
                .Where(type => type.GetInterfaces().Contains(typeof(IMessagePackFormatter)))
                .ToList();
        }
    }
}
