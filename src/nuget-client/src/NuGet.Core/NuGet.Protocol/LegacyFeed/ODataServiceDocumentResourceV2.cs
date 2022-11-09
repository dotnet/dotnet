// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    public class ODataServiceDocumentResourceV2 : INuGetResource
    {
        private readonly string _baseAddress;
        private readonly DateTime _requestTime;

        public ODataServiceDocumentResourceV2(string baseAddress, DateTime requestTime)
        {
            _baseAddress = baseAddress.Trim('/');
            _requestTime = requestTime;
        }

        public virtual DateTime RequestTime
        {
            get { return _requestTime; }
        }

        public string BaseAddress
        {
            get { return _baseAddress; }
        }
    }
}
