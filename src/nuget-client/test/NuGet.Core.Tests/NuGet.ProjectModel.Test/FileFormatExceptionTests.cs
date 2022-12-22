// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;

namespace NuGet.ProjectModel.Test
{
    public class FileFormatExceptionTests
    {
        private const int Column = 7;
        private static readonly DivideByZeroException InnerException = new DivideByZeroException();
        private const int Line = 3;
        private const string Message = "a";
        private const string Path = "b";

        [Fact]
        public void Constructor_WithMessageParameter_SetsProperties()
        {
            var exception = new FileFormatException(Message);

            Assert.Equal(Message, exception.Message);
            Assert.Null(exception.Path);
            Assert.Equal(0, exception.Line);
            Assert.Equal(0, exception.Column);
            Assert.Null(exception.InnerException);
        }

        [Fact]
        public void Constructor_WithMessageAndExceptionParameters_SetsProperties()
        {
            var exception = new FileFormatException(Message, InnerException);

            Assert.Equal(Message, exception.Message);
            Assert.Null(exception.Path);
            Assert.Equal(0, exception.Line);
            Assert.Equal(0, exception.Column);
            Assert.Same(InnerException, exception.InnerException);
        }

        [Fact]
        public void Create_WithExceptionLineColumnAndPathParameters_SetsProperties()
        {
            FileFormatException exception = FileFormatException.Create(InnerException, Line, Column, Path);

            Assert.Equal($"Error reading '{Path}' at line {Line} column {Column} : {InnerException.Message}", exception.Message);
            Assert.Equal(Path, exception.Path);
            Assert.Equal(Line, exception.Line);
            Assert.Equal(Column, exception.Column);
            Assert.Same(InnerException, exception.InnerException);
        }

        [Fact]
        public void Create_WithMessageLineColumnAndPathParameters_SetsProperties()
        {
            FileFormatException exception = FileFormatException.Create(Message, Line, Column, Path);

            Assert.Equal(Message, exception.Message);
            Assert.Equal(Path, exception.Path);
            Assert.Equal(Line, exception.Line);
            Assert.Equal(Column, exception.Column);
            Assert.Null(exception.InnerException);
        }
    }
}
