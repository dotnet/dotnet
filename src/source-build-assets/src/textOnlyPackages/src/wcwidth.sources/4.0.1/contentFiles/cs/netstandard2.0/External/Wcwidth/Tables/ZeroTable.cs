#if WCWIDTH
#pragma warning restore
#else
#pragma warning disable
#endif

namespace Wcwidth
{
    internal static partial class ZeroTable
    {
        private static readonly Dictionary<Unicode, int[,]> _lookup;
        private static readonly object _lock;

        static ZeroTable()
        {
            _lookup = new Dictionary<Unicode, int[,]>();
            _lock = new object();
        }

        public static int[,] GetTable(Unicode version)
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
