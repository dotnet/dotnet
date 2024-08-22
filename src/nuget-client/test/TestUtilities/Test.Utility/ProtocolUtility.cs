// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.IO;

namespace Test.Utility
{
    public static class ProtocolUtility
    {
        public static string GetResource(string name, Type type)
        {
            using (var reader = new StreamReader(type.Assembly.GetManifestResourceStream(name)))
            {
                return reader.ReadToEnd();
            }
        }

        public static string CreateServiceAddress()
        {
            return string.Format(CultureInfo.InvariantCulture, "http://{0}/", Guid.NewGuid());
        }
    }
}
