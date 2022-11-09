
using NuGet.Packaging.Core;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class PackageIdentityComparerTests
    {
        private PackageIdentityComparer comp = new PackageIdentityComparer();
        private PackageIdentity A100 = new PackageIdentity("A", new NuGetVersion("1.0.0"));
        private PackageIdentity A200 = new PackageIdentity("A", new NuGetVersion("2.0.0"));
        private PackageIdentity B100 = new PackageIdentity("B", new NuGetVersion("1.0.0"));
        private PackageIdentity B200 = new PackageIdentity("B", new NuGetVersion("2.0.0"));
        private PackageIdentity A100DUP = new PackageIdentity("A", new NuGetVersion("1.0.0"));
        private PackageIdentity A100DUP2 = new PackageIdentity("A", new NuGetVersion("1.0.0"));

        [Fact]
        public void PackageIdentityComparer_Equals()
        {
            // Test equals is Reflexive
            Assert.True(comp.Equals(A100, A100));

            // Test equals is symmetric
            Assert.True(comp.Equals(A100, A100DUP));
            Assert.True(comp.Equals(A100DUP, A100));

            //Test equals is transitive
            Assert.False(comp.Equals(A100, null));
            Assert.False(comp.Equals(A100DUP, null));
            Assert.True(comp.Equals(A100, A100DUP));
            Assert.True(comp.Equals(A100DUP2, A100DUP));
            Assert.True(comp.Equals(A100DUP2, A100));

            // Test equals for null references
            Assert.False(comp.Equals(A100, null));
            Assert.False(comp.Equals(B100, null));


            // Run all tests again to check for consistency
            // Test equals is Reflexive
            Assert.True(comp.Equals(A100, A100));

            // Test equals is symmetric
            Assert.True(comp.Equals(A100, A100DUP));
            Assert.True(comp.Equals(A100DUP, A100));

            //Test equals is transitive
            Assert.False(comp.Equals(A100, null));
            Assert.False(comp.Equals(A100DUP, null));
            Assert.True(comp.Equals(A100, A100DUP));
            Assert.True(comp.Equals(A100DUP2, A100DUP));
            Assert.True(comp.Equals(A100DUP2, A100));

            // Test equals for null references
            Assert.False(comp.Equals(A100, null));
            Assert.False(comp.Equals(B100, null));
        }

        [Fact]
        public void PackageIdentityComparer_Compare()
        {
            // Test Compare is Reflexive
            Assert.True(comp.Compare(A100, A100) == 0);

            // Test compare is symmetric
            Assert.True(comp.Compare(A100, A100DUP) == 0);
            Assert.True(comp.Compare(A100DUP, A100) == 0);
            Assert.True(comp.Compare(A100, A200) < 0);
            Assert.True(comp.Compare(A200, A100) > 0);
            Assert.True(comp.Compare(B100, B200) < 0);
            Assert.True(comp.Compare(B200, B100) > 0);

            // Test null references
            Assert.True(comp.Compare(A100, null) > 0);
            Assert.True(comp.Compare(null, A100) < 0);

            // Test transitivity
            Assert.True(comp.Compare(A100, A200) < 0);
            Assert.True(comp.Compare(A200, B100) < 0);
            Assert.True(comp.Compare(B100, A100) > 0);

            //Run all tests again to check for consistency
            // Test Compare is Reflexive
            Assert.True(comp.Compare(A100, A100) == 0);

            // Test compare is symmetric
            Assert.True(comp.Compare(A100, A100DUP) == 0);
            Assert.True(comp.Compare(A100DUP, A100) == 0);
            Assert.True(comp.Compare(A100, A200) < 0);
            Assert.True(comp.Compare(A200, A100) > 0);
            Assert.True(comp.Compare(B100, B200) < 0);
            Assert.True(comp.Compare(B200, B100) > 0);

            // Test null references
            Assert.True(comp.Compare(A100, null) > 0);
            Assert.True(comp.Compare(null, A100) < 0);

            // Test transitivity
            Assert.True(comp.Compare(A100, A200) < 0);
            Assert.True(comp.Compare(A200, B100) < 0);
            Assert.True(comp.Compare(B100, A100) > 0);
        }


    }
}
