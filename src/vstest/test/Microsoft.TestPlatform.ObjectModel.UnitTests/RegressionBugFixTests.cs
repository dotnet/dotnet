// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection.Metadata;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Navigation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.ObjectModel.UnitTests;

/// <summary>
/// Regression test for GH-3454:
/// PortablePdbReader(MetadataReaderProvider) constructor must validate its argument.
/// The fix added null-arg-check: passing null must throw ArgumentNullException.
/// </summary>
[TestClass]
public class RegressionBugFixTests
{
    [TestMethod]
    public void PortablePdbReader_NullMetadataReaderProvider_MustThrowArgumentNullException()
    {
        // GH-3454: The constructor added for embedded PDB support validates its parameter.
        // If the fix were reverted (constructor removed or null check removed),
        // this would either not compile or throw a different exception (NullReferenceException).
        var ex = Assert.ThrowsExactly<ArgumentNullException>(
            () => new PortablePdbReader((MetadataReaderProvider)null!));

        Assert.AreEqual("metadataReaderProvider", ex.ParamName,
            "GH-3454: Parameter name in ArgumentNullException must be 'metadataReaderProvider'.");
    }
}
