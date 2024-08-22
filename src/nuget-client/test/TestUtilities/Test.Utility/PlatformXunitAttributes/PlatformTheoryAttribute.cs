using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NuGet.Test.Utility
{
    public class PlatformTheoryAttribute
        : TheoryAttribute
    {
        private string _skip;

        public override string Skip
        {
            get
            {
                var skip = _skip;

                if (string.IsNullOrEmpty(skip))
                {
                    skip = XunitAttributeUtility.GetPlatformSkipMessageOrNull(GetAllPlatforms());
                }

                if (string.IsNullOrEmpty(skip))
                {
                    skip = XunitAttributeUtility.GetMonoMessage(OnlyOnMono, SkipMono);
                }

                if (string.IsNullOrEmpty(skip))
                {
                    if (CIOnly && !XunitAttributeUtility.IsCI)
                    {
                        skip = "This test only runs on the CI. To run it locally set the env var CI=true";
                    }
                }

                // If this is null the test will run.
                return skip;
            }

            set
            {
                _skip = value;
            }
        }

        public IEnumerable<string> Platforms { get; set; } = new List<string>();

        public string Platform { get; set; }

        public IEnumerable<string> SkipPlatforms { get; set; } = new List<string>();

        public string SkipPlatform { get; set; }

        public bool OnlyOnMono { get; set; }

        public bool SkipMono { get; set; }

        public bool CIOnly { get; set; }

        /// <summary>
        /// Provide property values to use this attribute.
        /// </summary>
        public PlatformTheoryAttribute()
        {
        }

        /// <summary>
        /// Run only on the given platforms
        /// </summary>
        public PlatformTheoryAttribute(params string[] platforms)
        {
            Platforms = platforms.ToList();
        }

        private string[] GetAllPlatforms()
        {
            var platforms = new HashSet<string>(Platforms ?? Array.Empty<string>(), StringComparer.OrdinalIgnoreCase)
            {
                Platform
            };

            var skipPlatforms = new HashSet<string>(SkipPlatforms ?? Array.Empty<string>(), StringComparer.OrdinalIgnoreCase)
            {
                SkipPlatform
            };

            platforms.RemoveWhere(e => string.IsNullOrEmpty(e) || skipPlatforms.Contains(e));

            return platforms.ToArray();
        }
    }
}
