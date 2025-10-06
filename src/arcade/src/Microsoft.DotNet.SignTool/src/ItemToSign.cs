// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.DotNet.SignTool
{
    internal class ItemToSign
    {
        public ItemToSign(string fullPath, string collisionPriorityId = "")
        {
            FullPath = fullPath;
            CollisionPriorityId = collisionPriorityId;
        }

        public string FullPath { get; }
        public string CollisionPriorityId { get; }
    }
}
