// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


namespace System.Reflection.Context.Tests
{
    internal static class ProjectionConstants
    {
        private const string Root = "System.Reflection.Context";

        private const string Tests = Root + ".Tests";
        public const string SecondTestObject = Tests + ".SecondTestObject";

        private const string RootCustom = Root + ".Custom";
        public const string CustomAssembly = RootCustom + ".CustomAssembly";
        public const string CustomModule = RootCustom + ".CustomModule";
        public const string CustomType = RootCustom + ".CustomType";
        public const string CustomParameter = RootCustom + ".CustomParameterInfo";

        private const string RootProjection = Root + ".Projection";
        public const string ProjectingCustomAttributeData = RootProjection + ".ProjectingCustomAttributeData";
        public const string ProjectingManifestResourceInfo = RootProjection + ".ProjectingManifestResourceInfo";

        private const string RootVirtual = Root + ".Virtual";
        public const string VirtualPropertyInfo = RootVirtual + ".VirtualPropertyInfo";
        public const string VirtualPropertyInfoGetter = VirtualPropertyInfo + "+PropertyGetter";
        public const string VirtualPropertyInfoSetter = VirtualPropertyInfo + "+PropertySetter";
        public const string VirtualParameter = RootVirtual + ".VirtualParameter";
        public const string VirtualReturnParameter = RootVirtual + ".VirtualReturnParameter";
    }
}
