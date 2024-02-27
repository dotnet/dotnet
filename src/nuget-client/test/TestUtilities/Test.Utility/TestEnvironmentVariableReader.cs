// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Common;

namespace Test.Utility
{
    public sealed class TestEnvironmentVariableReader : IEnvironmentVariableReader
    {
        private readonly IReadOnlyDictionary<string, string> _variables;

        private readonly string _toStringSuffix;

        public static IEnvironmentVariableReader EmptyInstance { get; } = new TestEnvironmentVariableReader();

        private TestEnvironmentVariableReader()
        {
            _variables = new Dictionary<string, string>();
        }

        public TestEnvironmentVariableReader(IReadOnlyDictionary<string, string> variables, string toStringSuffix = null)
        {
            _variables = variables ?? throw new ArgumentNullException(nameof(variables));
            _toStringSuffix = toStringSuffix;
        }

        public string GetEnvironmentVariable(string variable)
        {
            if (_variables.TryGetValue(variable, out var value))
            {
                return value;
            }

            return null;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(_toStringSuffix))
            {
                return base.ToString();
            }
            return $"{base.ToString()}({_toStringSuffix})";
        }
    }
}
