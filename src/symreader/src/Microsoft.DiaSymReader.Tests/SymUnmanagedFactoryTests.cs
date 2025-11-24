// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using System;
using System.IO;
using System.Reflection;
using Microsoft.DiaSymReader.Tools;
using Roslyn.Test.Utilities;
using Xunit;

#if NET9_0_OR_GREATER
using System.Runtime.InteropServices.Marshalling;
#endif

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

#if NET9_0_OR_GREATER
        /// <summary>
        /// This test is intended to verify that the marshalling doesn't fall over when we have to do the marshalling conversion from
        /// <see cref="System.Runtime.InteropServices.ComTypes.IStream"/> to <see cref="IUnsafeComStream"/>.
        /// </summary>
        [ConditionalFact(typeof(WindowsOnly))]
        public unsafe void ComWrappersCanMarshalIStream()
        {
            var pdbStream = new MemoryStream(TestResources.SourceLink.WindowsPdb);
            var reader = SymUnmanagedReaderFactory.CreateReader<ISymUnmanagedReader5>(pdbStream,
                DummySymReaderMetadataProvider.Instance, SymUnmanagedReaderCreationOptions.UseAlternativeLoadPath);
            Assert.NotNull(reader);

            IUnsafeComStream stream = new ComMemoryStream();
            stream.SetSize((int)pdbStream.Length);

            fixed (byte* pBytes = TestResources.SourceLink.WindowsPdb)
            {
                int written = 0;
                stream.Write(pBytes, TestResources.SourceLink.WindowsPdb.Length, &written);

                long newPos = 0;
                stream.Seek(0, 0, &newPos);
            }

            IntPtr pStream = (IntPtr)ComInterfaceMarshaller<IUnsafeComStream>.ConvertToUnmanaged(stream);
            System.Runtime.InteropServices.ComTypes.IStream comStream = ComStreamWrapper.Marshaller.ConvertToManaged(pStream);

            int hr = reader.UpdateSymbolStore(null, comStream);
            Assert.Equal(0, hr);
        }
#endif
    }
}
