using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NuGet.Test.Utility
{
    /// <summary>
    /// Skip the test if a file does not exist.
    /// Fail when running on the CI. This requires the CI to have it.
    /// </summary>
    public class FileExistsFactAttribute
        : FactAttribute
    {
        private string _skip;

        public override string Skip
        {
            get
            {
                var skip = _skip;

                if (string.IsNullOrEmpty(skip))
                {
                    skip = XunitAttributeUtility.GetFileExistsSkipMessageOrNull(AllowCIToSkip, GetPaths());
                }

                // If this is null the test will run.
                return skip;
            }

            set
            {
                _skip = value;
            }
        }

        public IEnumerable<string> Paths { get; set; } = new List<string>();

        public string Path { get; set; }

        /// <summary>
        /// If true the CI will be allowed to skip this test.
        /// </summary>
        public bool AllowCIToSkip { get; set; }

        public FileExistsFactAttribute()
        {
        }

        public FileExistsFactAttribute(params string[] paths)
        {
            Paths = paths.ToList();
        }

        private string[] GetPaths()
        {
            var paths = new HashSet<string>(Paths ?? Array.Empty<string>(), StringComparer.OrdinalIgnoreCase)
            {
                Path
            };

            return paths.Where(e => !string.IsNullOrEmpty(e)).ToArray();
        }
    }
}
