// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.Build.Tasks;
using System;
using System.IO;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;

namespace Microsoft.DotNet.GenFacades
{
    /// <summary>
    /// Rewrites an Assembly's references to be version 0.0.0.0.
    /// </summary>
    public class ClearAssemblyReferenceVersions : BuildTask
    {
        /// <summary>
        /// Assembly to rewrite.
        /// </summary>
        [Required]
        public string Assembly { get; set; }
        
        public override bool Execute()
        {
            try
            {
                using (FileStream stream = File.Open(Assembly, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
                using (PEReader peReader = new PEReader(stream))
                {
                    using (BinaryWriter writer = new BinaryWriter(stream))
                    {
                        MetadataReader mdReader = peReader.GetMetadataReader();
                        int assemblyRefOffset = mdReader.GetTableMetadataOffset(TableIndex.AssemblyRef);
                        int numAssemblyRef = mdReader.GetTableRowCount(TableIndex.AssemblyRef);
                        int sizeAssemblyRefRow = mdReader.GetTableRowSize(TableIndex.AssemblyRef);

                        for (int assemblyRefRow = 0; assemblyRefRow < numAssemblyRef; assemblyRefRow++)
                        {
                            stream.Position = peReader.PEHeaders.MetadataStartOffset + assemblyRefOffset + assemblyRefRow * sizeAssemblyRefRow;
                            // see ECMA-335 II.22.5 
                            // row starts with
                            // - MajorVersion, MinorVersion, BuildNumber, RevisionNumber (each being 2-byte constants)
                            // 8 bytes total -> long
                            writer.Write((long)0);
                        }

                        // remove signature, if present, since we changed the binary which would break any signature.
                        // adapted from https://github.com/dotnet/arcade/blob/866b2acd5ffc3c2031c102f6415fd7c6a1a370d5/src/Microsoft.DotNet.Arcade.Sdk/src/Unsign.cs#L58-L65
                        if (peReader.PEHeaders.PEHeader.CertificateTableDirectory.Size != 0)
                        {
                            // see https://docs.microsoft.com/en-us/windows/win32/debug/pe-format#optional-header-data-directories-image-only 
                            int certificateTableDirectoryOffset = (peReader.PEHeaders.PEHeader.Magic == PEMagic.PE32Plus) ? 144 : 128;
                            stream.Position = peReader.PEHeaders.PEHeaderStartOffset + certificateTableDirectoryOffset;

                            writer.Write((long)0);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.LogErrorFromException(e, showStackTrace: false);
            }

            return !Log.HasLoggedErrors;
        }
    }
}
