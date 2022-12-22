// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using System;
using System.IO;
using Microsoft.DiaSymReader.Tools;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.DiaSymReader.Native.UnitTests
{
    public class SymUnmanagedFactoryNativeTests
    {
        public interface ISymUnmanagedReaderX : ISymUnmanagedReader5 { }

        [ConditionalFact(typeof(WindowsOnly))]
        public void Create()
        {
            // TODO: Ideally we would run each of these tests in a separate process so they don't interfere with each other.
            // Native library being loaded makes following loads successful.
            var pdbStream = new MemoryStream(TestResources.SourceLink.WindowsPdb);

            Assert.NotNull(SymUnmanagedReaderFactory.CreateReader<ISymUnmanagedReader3>(pdbStream, DummySymReaderMetadataProvider.Instance));
            Assert.NotNull(SymUnmanagedReaderFactory.CreateReader<ISymUnmanagedReader4>(pdbStream, DummySymReaderMetadataProvider.Instance));
            Assert.NotNull(SymUnmanagedReaderFactory.CreateReader<ISymUnmanagedReader5>(pdbStream, DummySymReaderMetadataProvider.Instance));

            Assert.NotNull(SymUnmanagedReaderFactory.CreateReader<ISymUnmanagedReader3>(pdbStream, DummySymReaderMetadataProvider.Instance, SymUnmanagedReaderCreationOptions.UseComRegistry));
            Assert.NotNull(SymUnmanagedReaderFactory.CreateReader<ISymUnmanagedReader4>(pdbStream, DummySymReaderMetadataProvider.Instance, SymUnmanagedReaderCreationOptions.UseComRegistry));
            Assert.NotNull(SymUnmanagedReaderFactory.CreateReader<ISymUnmanagedReader5>(pdbStream, DummySymReaderMetadataProvider.Instance, SymUnmanagedReaderCreationOptions.UseComRegistry));

            Assert.Throws<NotSupportedException>(() =>
                SymUnmanagedReaderFactory.CreateReader<ISymUnmanagedReaderX>(pdbStream, DummySymReaderMetadataProvider.Instance));

            Assert.Throws<NotSupportedException>(() =>
                SymUnmanagedReaderFactory.CreateReader<ISymUnmanagedReaderX>(pdbStream, DummySymReaderMetadataProvider.Instance, SymUnmanagedReaderCreationOptions.UseComRegistry));
        
            Assert.NotNull(SymUnmanagedWriterFactory.CreateWriter(DummySymWriterMetadataProvider.Instance));
            Assert.NotNull(SymUnmanagedWriterFactory.CreateWriter(DummySymWriterMetadataProvider.Instance, SymUnmanagedWriterCreationOptions.Deterministic));
            Assert.NotNull(SymUnmanagedWriterFactory.CreateWriter(DummySymWriterMetadataProvider.Instance, SymUnmanagedWriterCreationOptions.Deterministic | SymUnmanagedWriterCreationOptions.UseComRegistry));
        }
    }
}
