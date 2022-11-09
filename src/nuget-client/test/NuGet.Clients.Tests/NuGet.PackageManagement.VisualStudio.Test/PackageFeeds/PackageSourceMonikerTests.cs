// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using Moq;
using NuGet.VisualStudio.Internal.Contracts;
using Xunit;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    public class PackageSourceMonikerTests
    {
        [Fact]
        public async Task PopulateListAsync_WithCancellationToken_ThrowsAsync()
        {
            var cts = new CancellationTokenSource();
            cts.Cancel();

            await Assert.ThrowsAsync<OperationCanceledException>(async () => await PackageSourceMoniker.PopulateListAsync(It.IsAny<IReadOnlyCollection<PackageSourceContextInfo>>(), cts.Token));
        }

        [Fact]
        public async Task PopulateListAsync_WithPackageSources_AggregateSourceGetsPriorityOrderAsync()
        {
            // Arrange
            var sources = new[]
            {
                new PackageSourceContextInfo("SuourceA", "SourceAName", isEnabled: true),
                new PackageSourceContextInfo("SuourceB", "SourceBName", isEnabled: true),
            };

            // Act
            IReadOnlyCollection<PackageSourceMoniker> result = await PackageSourceMoniker.PopulateListAsync(sources, CancellationToken.None);

            // Assert
            Assert.Equal(sources.Length + 1, result.Count);

            IEnumerable<PackageSourceMoniker> aggSource = result.Where(pkgSource => pkgSource.IsAggregateSource);

            PackageSourceMoniker aggMoniker = aggSource.First();
            Assert.All(result.Except(aggSource), psm => Assert.True(psm.PriorityOrder > aggMoniker.PriorityOrder));
        }

        // This test assumes running on english locale
        [Theory]
        [InlineData(new[] { "Aim", "C", "B" }, new[] { "All", "Aim", "B", "C" })]
        [InlineData(new[] { "Air", "Aim" }, new[] { "All", "Aim", "Air" })]
        [InlineData(new[] { "Ask", "Beta", "Arm", "All" }, new[] { "All", "All", "Arm", "Ask", "Beta" })]
        public async Task PopulateListAsync_SimulatingSortingInTopPanel_AggregateSourceAlwaysFirstAsync(string[] sourceNames, string[] expectedSourceNamesOrder)
        {
            // Arrange
            PackageSourceContextInfo[] sourcesContextInfo = sourceNames
                .Select(pkgSrc => new PackageSourceContextInfo(pkgSrc, pkgSrc, isEnabled: true))
                .ToArray();

            // Act
            IReadOnlyCollection<PackageSourceMoniker> result = await PackageSourceMoniker.PopulateListAsync(sourcesContextInfo, CancellationToken.None);

            // Simulates collectionViewSource found at PackageManagerTopPanel.xaml
            var cvs = new CollectionViewSource();
            cvs.SortDescriptions.Add(new SortDescription(nameof(PackageSourceMoniker.PriorityOrder), ListSortDirection.Ascending));
            cvs.SortDescriptions.Add(new SortDescription(nameof(PackageSourceMoniker.SourceName), ListSortDirection.Ascending));
            cvs.Culture = new CultureInfo("en-US");
            cvs.Source = result;

            IEnumerable<PackageSourceMoniker> pkgSourcesSorted = cvs.View.Cast<PackageSourceMoniker>();
            IEnumerable<string> sourceNamesSorted = pkgSourcesSorted.Select(x => x.SourceName);

            // Assert
            Assert.Equal(expectedSourceNamesOrder, sourceNamesSorted);
            Assert.True(pkgSourcesSorted.First().IsAggregateSource); // First package source is an Aggreage Source
            Assert.All(pkgSourcesSorted.Except(new[] { pkgSourcesSorted.First() }), pkgSrc => Assert.False(pkgSrc.IsAggregateSource)); // others are not aggregate sources
        }

        [Fact]
        public async Task PopulateListAsync_OnlyOneSource_NotAnAggregateSourceAsync()
        {
            // Arrange
            var sourcesContextInfo = new[]
            {
                new PackageSourceContextInfo("Air", "Air", isEnabled: true),
            };

            // Act
            IReadOnlyCollection<PackageSourceMoniker> results = await PackageSourceMoniker.PopulateListAsync(sourcesContextInfo, CancellationToken.None);

            // Assert
            Assert.False(results.First().IsAggregateSource);
        }

        [Fact]
        public async Task PopulateListAsync_SimulatingFrenchLocale_AggregateSourceFirstAsync()
        {
            // Arrange
            // Input from https://docs.microsoft.com/globalization/locale/sorting-and-string-comparison#how-do-i-verify-that-sorting-works-correctly-in-my-code
            var input = new[] { "ñú", "cote", "coté", "côté", "ñandú", "côte", "número", "Namibia" };
            var expectedOrder = new[] { "cote", "côte", "coté", "côté", "Namibia", "ñandú", "ñú", "número" };
            var inputCulture = new CultureInfo("fr-FR");

            PackageSourceContextInfo[] sourcesContextInfo = input
                .Select(pkgSrc => new PackageSourceContextInfo(pkgSrc, pkgSrc, isEnabled: true))
                .ToArray();

            CultureInfo currentCulture = Strings.Culture;
            try
            {
                Strings.Culture = inputCulture;
                // Act
                IReadOnlyCollection<PackageSourceMoniker> results = await PackageSourceMoniker.PopulateListAsync(sourcesContextInfo, CancellationToken.None);

                // Simulates collectionViewSource found at PackageManagerTopPanel.xaml
                var cvs = new CollectionViewSource();
                cvs.SortDescriptions.Add(new SortDescription(nameof(PackageSourceMoniker.PriorityOrder), ListSortDirection.Ascending));
                cvs.SortDescriptions.Add(new SortDescription(nameof(PackageSourceMoniker.SourceName), ListSortDirection.Ascending));
                cvs.Culture = inputCulture;
                cvs.Source = results;

                IEnumerable<PackageSourceMoniker> pkgSourcesSorted = cvs.View.Cast<PackageSourceMoniker>();
                IEnumerable<PackageSourceMoniker> pkgSourcesWithoutFirst = pkgSourcesSorted.Except(new[] { pkgSourcesSorted.First() });
                IEnumerable<string> sourceNamesSorted = pkgSourcesWithoutFirst.Select(x => x.SourceName);

                // Assert
                Assert.Equal(expectedOrder, sourceNamesSorted);
                Assert.True(pkgSourcesSorted.First().IsAggregateSource); // First package source is an Aggreage Source
                Assert.All(pkgSourcesWithoutFirst, pkgSrc => Assert.False(pkgSrc.IsAggregateSource)); // others are not aggregate sources
            }
            catch
            {
                throw; // Report any errors to test runner
            }
            finally
            {
                Strings.Culture = currentCulture; // Set it back to its original state
            }
        }
    }
}
