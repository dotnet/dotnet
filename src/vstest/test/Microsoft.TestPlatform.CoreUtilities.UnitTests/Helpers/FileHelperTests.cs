// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;

using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.CoreUtilities.UnitTests.Helpers;

[TestClass]
public class FileHelperTests
{
    private readonly FileHelper _fileHelper;
    private readonly string _tempFile;

    public FileHelperTests()
    {
        _tempFile = Path.GetTempFileName();
        File.AppendAllText(_tempFile, "Some content..");
        _fileHelper = new FileHelper();
    }

    [TestCleanup]
    public void Cleanup()
    {
        File.Delete(_tempFile);
    }

    [TestMethod]
    public void GetStreamShouldAbleToGetTwoStreamSimultanouslyIfFileAccessIsRead()
    {
        using var stream1 = _fileHelper.GetStream(_tempFile, FileMode.Open, FileAccess.Read);
        using var stream2 =
            _fileHelper.GetStream(_tempFile, FileMode.Open, FileAccess.Read);
    }
}
