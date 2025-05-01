#if WCWIDTH
#pragma warning restore
#else
#pragma warning disable
#endif

using System.Collections.Generic;

namespace Wcwidth
{
    internal static partial class WideTable
    {
        private static readonly Dictionary<Unicode, uint[,]> _lookup;
        private static readonly object _lock;

        static WideTable()
        {
            _lookup = new Dictionary<Unicode, uint[,]>();
            _lock = new object();
        }

        public static uint[,] GetTable(Unicode version)
        {
            if (!_lookup.TryGetValue(version, out var table))
            {
                lock (_lock)
                {
                    if (_lookup.TryGetValue(version, out table))
                    {
                        return table;
                    }

                    // Generate the table for the version dynamically
                    // since we don't want to load everything into memory.
                    table = GenerateTable(version);
                    _lookup[version] = table;
                }
            }

            return table;
        }
    }
}
