// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.DiaSymReader.UnitTests
{
    public class SymUnmanagedWriterTests
    {
        static SymUnmanagedWriterTests() => SymUnmanagedFactoryTests.SetLoadPath();

        [ConditionalFact(typeof(WindowsOnly))]
        public void Deterministic()
        {
            var lang = new Guid("00000000-0000-0000-0000-000000000001");
            var vendor = new Guid("00000000-0000-0000-0000-000000000002");
            var type = new Guid("00000000-0000-0000-0000-000000000003");
            var pdbStream = new MemoryStream();

            var metadataProvider = new TestSymWriterMetadataProvider(
                new Dictionary<int, (string Name, int DeclaringType)>()
                {
                    {  0x06000001, ("M", 0x03000001) }
                },
                new Dictionary<int, (string Namespace, string Name, TypeAttributes Attributes)>()
                {
                    {  0x03000001, ("N", "C", TypeAttributes.Class) }
                });

            Guid pdbId;
            uint pdbStamp;
            int pdbAge;

            using (var writer = SymUnmanagedWriterFactory.CreateWriter(
                metadataProvider, 
                SymUnmanagedWriterCreationOptions.Deterministic | SymUnmanagedWriterCreationOptions.UseAlternativeLoadPath))
            {
                writer.AddCompilerInfo(1, 2, 3, 4, "Compiler");

                var docIndex = writer.DefineDocument("doc", lang, vendor, type, algorithmId: default, checksum: null, source: null); 

                writer.OpenMethod(0x06000001);

                writer.DefineSequencePoints(
                    docIndex,
                    count: 1,
                    offsets: new[] { 0 },
                    startLines: new[] { 1 },
                    startColumns: new[] { 4 },
                    endLines: new[] { 1 },
                    endColumns: new[] { 10 });

                writer.CloseMethod();
                writer.UpdateSignature(default, 0, 1);
                writer.GetSignature(out pdbId, out pdbStamp, out pdbAge);
                writer.WriteTo(pdbStream);
            }

            Assert.Equal(default, pdbId);
            Assert.Equal(0U, pdbStamp);
            Assert.Equal(1, pdbAge);

            var symReader = SymUnmanagedReaderFactory.CreateReader<ISymUnmanagedReader5>(pdbStream, metadataProvider, SymUnmanagedReaderCreationOptions.UseAlternativeLoadPath);

            var infoReader = (ISymUnmanagedCompilerInfoReader)symReader;
            Assert.True(infoReader.TryGetCompilerInfo(out var version, out var name));
            Assert.Equal(new Version(1, 2, 3, 4), version);
            Assert.Equal("Compiler", name);

            var docs = symReader.GetDocuments();
            AssertEx.Equal(new[] { "doc 00000000-0000-0000-0000-000000000001" }, docs.Select(d => $"{d.GetName()} {d.GetLanguage()}"));

            var method = symReader.GetMethod(0x06000001);
            AssertEx.Equal(new[] { "IL_0000: (1, 4)-(1, 10)" }, method.GetSequencePoints().Select(s => $"IL_{s.Offset:X4}: ({s.StartLine}, {s.StartColumn})-({s.EndLine}, {s.EndColumn})"));
            
            Assert.Equal(HResult.S_OK, symReader.MatchesModule(pdbId, pdbStamp, pdbAge, out bool result));
            Assert.True(result);
        }
    }
}
