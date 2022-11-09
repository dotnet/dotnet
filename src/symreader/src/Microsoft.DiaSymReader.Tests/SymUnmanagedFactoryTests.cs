// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using System;
using System.IO;
using System.Reflection;
using Microsoft.DiaSymReader.Tools;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.DiaSymReader.UnitTests
{
    public class SymUnmanagedFactoryTests
    {
        internal static void SetLoadPath()
            => Environment.SetEnvironmentVariable("MICROSOFT_DIASYMREADER_NATIVE_ALT_LOAD_PATH", Path.Combine(Path.GetDirectoryName(typeof(SymUnmanagedFactoryTests).GetTypeInfo().Assembly.Location), "DSRN"));

        static SymUnmanagedFactoryTests()
        {
            SetLoadPath();
        }

        [ConditionalFact(typeof(DesktopOnly), Skip = "https://github.com/dotnet/symreader/issues/96")]
        public void Create()
        {
            // TODO: Ideally we would run each of these tests in a separate process so they don't interfere with each other.
            // Native library being loaded makes following loads successful.

            var pdbStream = new MemoryStream(TestResources.SourceLink.WindowsPdb);

            Assert.Throws<DllNotFoundException>(() => 
                SymUnmanagedReaderFactory.CreateReader<ISymUnmanagedReader5>(pdbStream, DummySymReaderMetadataProvider.Instance, SymUnmanagedReaderCreationOptions.Default));

            Assert.Throws<DllNotFoundException>(() => SymUnmanagedWriterFactory.CreateWriter(DummySymWriterMetadataProvider.Instance));
            Assert.Throws<DllNotFoundException>(() => SymUnmanagedWriterFactory.CreateWriter(DummySymWriterMetadataProvider.Instance, SymUnmanagedWriterCreationOptions.Deterministic));

            Assert.Throws<NotSupportedException>(() =>
                SymUnmanagedReaderFactory.CreateReader<ISymUnmanagedReader5>(pdbStream, DummySymReaderMetadataProvider.Instance, SymUnmanagedReaderCreationOptions.UseComRegistry));

            Assert.Throws<SymUnmanagedWriterException>(() => SymUnmanagedWriterFactory.CreateWriter(DummySymWriterMetadataProvider.Instance, 
                SymUnmanagedWriterCreationOptions.UseComRegistry | SymUnmanagedWriterCreationOptions.Deterministic));

            Assert.NotNull(SymUnmanagedReaderFactory.CreateReader<ISymUnmanagedReader5>(pdbStream, 
                DummySymReaderMetadataProvider.Instance, SymUnmanagedReaderCreationOptions.UseAlternativeLoadPath));

            Assert.NotNull(SymUnmanagedReaderFactory.CreateReader<ISymUnmanagedReader5>(pdbStream, 
                DummySymReaderMetadataProvider.Instance, SymUnmanagedReaderCreationOptions.UseAlternativeLoadPath | SymUnmanagedReaderCreationOptions.UseComRegistry));

            Assert.NotNull(SymUnmanagedWriterFactory.CreateWriter(DummySymWriterMetadataProvider.Instance, 
                SymUnmanagedWriterCreationOptions.UseComRegistry));

            Assert.NotNull(SymUnmanagedWriterFactory.CreateWriter(DummySymWriterMetadataProvider.Instance, 
                SymUnmanagedWriterCreationOptions.UseAlternativeLoadPath));

            Assert.NotNull(SymUnmanagedWriterFactory.CreateWriter(DummySymWriterMetadataProvider.Instance, 
                SymUnmanagedWriterCreationOptions.UseAlternativeLoadPath | SymUnmanagedWriterCreationOptions.UseComRegistry | SymUnmanagedWriterCreationOptions.Deterministic));
        }

        [Fact]
        public void GetEnvironmentVariable()
        {
            Assert.NotNull(SymUnmanagedFactory.GetEnvironmentVariable("MICROSOFT_DIASYMREADER_NATIVE_ALT_LOAD_PATH"));
        }
    }
}
