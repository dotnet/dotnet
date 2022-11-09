// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Microsoft.TestPlatform.Extensions.BlameDataCollector;

public interface IBlameReaderWriter
{
    /// <summary>
    /// Writes tests to document
    /// </summary>
    /// <param name="testSequence">List of test guid in sequence</param>
    /// <param name="testObjectDictionary">Dictionary of test objects</param>
    /// <param name="filePath">The path of file</param>
    /// <returns>File Path</returns>
    string WriteTestSequence(List<Guid> testSequence, Dictionary<Guid, BlameTestObject> testObjectDictionary, string filePath);

    /// <summary>
    /// Reads all tests from file
    /// </summary>
    /// <param name="filePath">The path of saved file</param>
    /// <returns>All tests</returns>
    List<BlameTestObject> ReadTestSequence(string filePath);
}
