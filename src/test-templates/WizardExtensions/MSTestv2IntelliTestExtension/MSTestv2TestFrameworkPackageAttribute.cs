// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Pex.Framework.Packages;
using MSTestv2IntelliTestExtension;

[assembly: PexPackageType(typeof(MSTestv2TestFrameworkPackage))]
namespace MSTestv2IntelliTestExtension
{
    using Microsoft.ExtendedReflection.ComponentModel;
    using Microsoft.Pex.Engine.ComponentModel;
    using Microsoft.Pex.Engine.TestFrameworks;

    /// <summary>
    /// Extensions package for MSTestv2.
    /// </summary>
    public class MSTestv2TestFrameworkPackageAttribute : PexPackageAttributeBase
    {
        public override string Name => nameof(MSTestv2TestFrameworkPackage);

        protected override void Initialize(IEngine engine)
        {
            base.Initialize(engine);

            var testFrameworkService = engine.GetService<IPexTestFrameworkManager>();
            var host = testFrameworkService as IPexComponent;

            testFrameworkService.AddTestFramework(new MSTestv2TestFramework(host));
        }
    }
}
