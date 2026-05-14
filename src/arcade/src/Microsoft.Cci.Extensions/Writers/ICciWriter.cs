// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Cci.Writers
{
    public interface ICciWriter
    {
        void WriteAssemblies(IEnumerable<IAssembly> assemblies);
    }
}
