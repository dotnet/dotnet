// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Globalization;
using System.Linq;
using Microsoft.Cci;

namespace Microsoft.Cci.Extensions
{
    public static class AssemblyIdentityHelpers
    {
        public static string Format(this AssemblyIdentity assemblyIdentity)
        {
            var name = new System.Reflection.AssemblyName();
            name.Name = assemblyIdentity.Name.Value;
            name.Version = assemblyIdentity.Version;
            name.SetPublicKeyToken(assemblyIdentity.PublicKeyToken.ToArray());
            return name.ToString();
        }

        public static AssemblyIdentity Parse(INameTable nameTable, string formattedName)
        {
            var name = new System.Reflection.AssemblyName(formattedName);
            return new AssemblyIdentity(nameTable.GetNameFor(name.Name),
                                        name.CultureName,
                                        name.Version,
                                        name.GetPublicKeyToken(),
                                        "");
        }
    }
}
