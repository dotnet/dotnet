// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Microsoft.DotNet.VersionTools.Automation.VstsApi
{
    public class VstsProfile
    {
        public string Id { get; set; }
        public Dictionary<string, VstsCoreProfileAttribute> CoreAttributes { get; set; }
    }

    public class VstsCoreProfileAttribute
    {
        public VstsAttributeDescriptor Descriptor { get; set; }
        public JToken Value { get; set; }
    }

    public class VstsAttributeDescriptor
    {
        public string AttributeName { get; set; }
        public string ContainerName { get; set; }
    }
}
