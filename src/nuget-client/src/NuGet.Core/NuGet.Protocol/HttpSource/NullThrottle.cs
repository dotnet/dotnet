// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;

namespace NuGet.Protocol
{
    /// <summary>
    /// An throttle implementation that allows any level of concurrency. That is, the
    /// <see cref="WaitAsync"/> and <see cref="Release"/> methods do nothing.
    /// </summary>
    public class NullThrottle : IThrottle
    {
        private static readonly NullThrottle _instance = new NullThrottle();

        public static NullThrottle Instance
        {
            get
            {
                return _instance;
            }
        }

        public Task WaitAsync()
        {
            return Task.CompletedTask;
        }

        public void Release()
        {
        }
    }
}
