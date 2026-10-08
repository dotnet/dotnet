// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.Utilities;

namespace Microsoft.VisualStudio.TestPlatform.Common.ExtensionDecorators;

internal class ExtensionDecoratorFactory
{
    private readonly IFeatureFlag _featureFlag;

    public ExtensionDecoratorFactory(IFeatureFlag featureFlag)
    {
        _featureFlag = featureFlag;
    }

    public ITestExecutor Decorate(ITestExecutor originalTestExecutor)
    {
        return _featureFlag.IsSet(FeatureFlag.VSTEST_DISABLE_SERIALTESTRUN_DECORATOR)
            ? originalTestExecutor
            : new SerialTestRunDecorator(originalTestExecutor);
    }
}
