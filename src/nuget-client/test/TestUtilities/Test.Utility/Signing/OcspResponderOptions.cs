// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Test.Utility.Signing
{
    public sealed class OcspResponderOptions
    {
        public DateTimeOffset? ThisUpdate { get; set; }
        public DateTimeOffset? NextUpdate { get; set; }
    }
}
