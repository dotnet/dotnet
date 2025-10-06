// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xunit;

namespace NuGet.Client.Test
{
    public class ContentModelResourceTests
    {
        [MemberData(nameof(AllCultures))]
        [Theory]
        public void CanParseEverySystemKnownCultureResource(CultureInfo culture)
        {
            var result = ManagedCodeConventions.Locale_Parser(culture.Name.AsMemory(), null, false);
            Assert.Equal(culture.Name, result as string);
        }

        public static IEnumerable<object[]> AllCultures()
        {
            return CultureInfo.GetCultures(CultureTypes.AllCultures).Where(c => !string.IsNullOrEmpty(c.Name)).Select(culture => new[] { culture });
        }
    }
}

