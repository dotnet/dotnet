// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;

namespace Microsoft.DotNet.AsmDiff.CSV
{
    public abstract class CsvWriter : IDisposable
    {
        private CsvSettings _settings;

        protected CsvWriter(CsvSettings settings)
        {
           _settings = settings;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        public abstract void Write(string value);

        public virtual void Write(IEnumerable<string> values)
        {
            foreach (var value in values)
                Write(value);
        }

        public abstract void WriteLine();

        public virtual void WriteLine(IEnumerable<string> values)
        {
            foreach (var value in values)
                Write(value);

            WriteLine();
        }

        public virtual CsvSettings Settings { get { return _settings; } set { _settings = value; } }
    }
}
