// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.IO.Tests
{
    // Concrete class to run common file attributes tests on the FileInfo class
    public class FileInfo_GetSetAttributesCommon : FileGetSetAttributes
    {
        protected override bool CanBeReadOnly => true;
        protected override FileAttributes GetAttributes(string path) => new FileInfo(path).Attributes;
        protected override void SetAttributes(string path, FileAttributes attributes) => new FileInfo(path).Attributes = attributes;
    }
}
