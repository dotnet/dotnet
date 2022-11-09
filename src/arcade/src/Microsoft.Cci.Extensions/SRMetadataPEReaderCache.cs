using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Reflection.Metadata;
using System.Text;
using SRMetadataReader = System.Reflection.Metadata.MetadataReader;

namespace Microsoft.Cci.Extensions
{

    public class SRMetadataPEReaderCache : IDisposable
    {
        private bool _disposed;

        private Dictionary<string, (FileStream, PEReader)> _cache = null;

        public SRMetadataReader GetMetadataReader(string assemblyPath)
        {
            if (_cache == null)
            {
                _cache = new Dictionary<string, (FileStream, PEReader)>();
            }
            else
            {
                if (_cache.TryGetValue(assemblyPath, out (FileStream _, PEReader peReader) value))
                {
                    return value.peReader.GetMetadataReader();
                }
            }

            FileStream stream = File.OpenRead(assemblyPath);
            PEReader peReader = new PEReader(stream);

            _cache.Add(assemblyPath, (stream, peReader));
            return peReader.GetMetadataReader();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (_cache != null)
                {
                    foreach ((FileStream stream, PEReader reader) in _cache.Values)
                    {
                        stream.Dispose();
                        reader.Dispose();
                    }

                    _cache.Clear();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

    }
}
