// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Frameworks
{
    public sealed class DefaultFrameworkNameProvider : FrameworkNameProvider
    {
        public DefaultFrameworkNameProvider()
            : base(new IFrameworkMappings[] { DefaultFrameworkMappings.Instance },
                new IPortableFrameworkMappings[] { DefaultPortableFrameworkMappings.Instance })
        {
        }

        private static readonly Lazy<IFrameworkNameProvider> InstanceLazy = new Lazy<IFrameworkNameProvider>(() => new DefaultFrameworkNameProvider());

        public static IFrameworkNameProvider Instance
        {
            get { return InstanceLazy.Value; }
        }
    }
}
