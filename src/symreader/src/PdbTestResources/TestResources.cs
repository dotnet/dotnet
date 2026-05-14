// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.
 // Copyright(c) Microsoft.All Rights Reserved.Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

 using Microsoft.DiaSymReader.Tools.UnitTests;

namespace TestResources
{
    public static class SourceLink
    {
        private static byte[] s_windowsDll;
        public static byte[] WindowsDll => ResourceLoader.GetOrCreateResource(ref s_windowsDll, nameof(SourceLink) + ".dll");

        private static byte[] s_windowsPdb;
        public static byte[] WindowsPdb => ResourceLoader.GetOrCreateResource(ref s_windowsPdb, nameof(SourceLink) + ".pdb");

        public static TestResource WindowsDllAndPdb => new TestResource(WindowsDll, WindowsPdb);
    }
}
