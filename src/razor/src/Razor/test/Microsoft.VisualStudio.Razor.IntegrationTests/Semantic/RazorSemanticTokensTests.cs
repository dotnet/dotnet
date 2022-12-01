// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.VisualStudio.Razor.IntegrationTests.InProcess;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Xunit;

namespace Microsoft.VisualStudio.Razor.IntegrationTests;

[IntializeTestFile]
public class RazorSemanticTokensTests : AbstractRazorEditorTest
{
    private static readonly AsyncLocal<string?> s_fileName = new();

    private static readonly string s_projectPath = TestProject.GetProjectDirectory(typeof(RazorSemanticTokensTests), useCurrentDirectory: true);

    // WARNING: If you leave this as "true" it will cause the semantic tokens tests to change their expected values.
    // Do NOT check in set to true.
    protected bool GenerateBaselines { get; set; } = false;

    // Used by the test framework to set the 'base' name for test files.
    public static string? FileName
    {
        get { return s_fileName.Value; }
        set { s_fileName.Value = value; }
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        await TestServices.Workspace.WaitForAsyncOperationsAsync(FeatureAttribute.Classification, ControlledHangMitigatingCancellationToken);
    }

    [IdeFact]
    public async Task Components_AreColored()
    {
        // Arrange
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.MainLayoutFile, ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.SetTextAsync(RazorProjectConstants.MainLayoutContent, ControlledHangMitigatingCancellationToken);

        // Act
        await TestServices.Editor.WaitForComponentClassificationAsync(ControlledHangMitigatingCancellationToken, count: 3);

        // Assert
        var expectedClassifications = await GetExpectedClassificationSpansAsync(nameof(Components_AreColored), ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.VerifyGetClassificationsAsync(expectedClassifications, ControlledHangMitigatingCancellationToken);
    }

    [IdeFact]
    public async Task Edits_UpdateColors()
    {
        // Arrange
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.MainLayoutFile, ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.SetTextAsync(RazorProjectConstants.MainLayoutContent, ControlledHangMitigatingCancellationToken);

        // Act
        await TestServices.Editor.WaitForComponentClassificationAsync(ControlledHangMitigatingCancellationToken, count: 3);

        await TestServices.Editor.SetTextAsync(RazorProjectConstants.IndexPageContent, ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.WaitForComponentClassificationAsync(ControlledHangMitigatingCancellationToken, count: 3);

        // Assert
        var expectedClassifications = await GetExpectedClassificationSpansAsync(nameof(Edits_UpdateColors), ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.VerifyGetClassificationsAsync(expectedClassifications, ControlledHangMitigatingCancellationToken);
    }

    [IdeFact]
    public async Task Directives_AreColored()
    {
        // Arrange
        await TestServices.SolutionExplorer.OpenFileAsync(RazorProjectConstants.BlazorProjectName, RazorProjectConstants.CounterRazorFile, ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.WaitForComponentClassificationAsync(ControlledHangMitigatingCancellationToken);

        // Act and Assert
        var expectedClassifications = await GetExpectedClassificationSpansAsync(nameof(Directives_AreColored), ControlledHangMitigatingCancellationToken);
        await TestServices.Editor.VerifyGetClassificationsAsync(expectedClassifications, ControlledHangMitigatingCancellationToken);
    }

    private async Task<IEnumerable<ClassificationSpan>> GetExpectedClassificationSpansAsync(string testName, CancellationToken cancellationToken)
    {
        var snapshot = await TestServices.Editor.GetActiveSnapshotAsync(ControlledHangMitigatingCancellationToken);

        if (GenerateBaselines)
        {
            var actual = await TestServices.Editor.GetClassificationsAsync(cancellationToken);
            GenerateSemanticBaseline(actual, testName);
        }

        var expectedClassifications = await ReadSemanticBaselineAsync(snapshot, cancellationToken);

        return expectedClassifications;
    }

    private async Task<IEnumerable<ClassificationSpan>> ReadSemanticBaselineAsync(ITextSnapshot snapshot, CancellationToken cancellationToken)
    {
        var baselinePath = Path.ChangeExtension(FileName, ".txt");
        var assembly = GetType().GetTypeInfo().Assembly;
        var semanticFile = TestFile.Create(baselinePath, assembly);

        var semanticStr = await semanticFile.ReadAllTextAsync(cancellationToken);

        return ParseSemanticBaseline(semanticStr, snapshot);

        static IEnumerable<ClassificationSpan> ParseSemanticBaseline(string semanticStr, ITextSnapshot snapshot)
        {
            var result = new List<ClassificationSpan>();
            var strArray = semanticStr.Split(new[] { Separator.ToString(), Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < strArray.Length; i += 3)
            {
                if (!int.TryParse(strArray[i], out var position))
                {
                    throw new InvalidOperationException($"{strArray[i]} was not an int {i}");
                }

                if (!int.TryParse(strArray[i + 1], out var length))
                {
                    throw new InvalidOperationException($"{strArray[i + 1]} was not an int {i}");
                }

                var snapshotSpan = new SnapshotSpan(snapshot, position, length);

                var classification = strArray[i + 2];
                var classificationType = new ClassificationType(classification);

                result.Add(new ClassificationSpan(snapshotSpan, classificationType));
            }

            return result;
        }
    }

    private const char Separator = ',';

    private static void GenerateSemanticBaseline(IEnumerable<ClassificationSpan> actual, string baselineFileName)
    {
        var builder = new StringBuilder();
        foreach (var baseline in actual)
        {
            builder.Append(baseline.Span.Start.Position).Append(Separator);
            builder.Append(baseline.Span.Length).Append(Separator);

            var classification = baseline.ClassificationType;
            string? classificationStr = null;
            if (classification.BaseTypes.Count() > 1)
            {
                foreach (ILayeredClassificationType baseType in classification.BaseTypes)
                {
                    if (baseType.Layer == ClassificationLayer.Semantic)
                    {
                        classificationStr = baseType.Classification;
                        break;
                    }
                }

                if (classificationStr is null)
                {
                    Assert.True(false, "Tried to write layered classifications without Semantic layer");
                    throw new Exception();
                }
            }
            else
            {
                classificationStr = classification.Classification;
            }

            builder.Append(classificationStr).Append(Separator);
            builder.AppendLine();
        }

        var semanticBaselinePath = GetBaselineFileName(baselineFileName);
        File.WriteAllText(semanticBaselinePath, builder.ToString());
    }

    private static string GetBaselineFileName(string testName)
    {
        var semanticBaselinePath = Path.Combine(s_projectPath, "Semantic", "TestFiles", nameof(RazorSemanticTokensTests), testName + ".txt");
        return semanticBaselinePath;
    }

    private class ClassificationComparer : IEqualityComparer<ClassificationSpan>
    {
        public static ClassificationComparer Instance = new();

        public bool Equals(ClassificationSpan x, ClassificationSpan y)
        {
            var spanEquals = x.Span.Equals(y.Span);
            var classificationEquals = ClassificationTypeComparer.Instance.Equals(x.ClassificationType, y.ClassificationType);
            return classificationEquals && spanEquals;
        }

        public int GetHashCode(ClassificationSpan obj)
        {
            throw new NotImplementedException();
        }
    }

    private class ClassificationTypeComparer : IEqualityComparer<IClassificationType>
    {
        public static ClassificationTypeComparer Instance = new();

        public bool Equals(IClassificationType x, IClassificationType y)
        {
            string xString;
            string yString;

            if (x is ILayeredClassificationType xLayered)
            {
                var baseType = xLayered.BaseTypes.Single(b => b is ILayeredClassificationType bLayered && bLayered.Layer == ClassificationLayer.Semantic);
                xString = baseType.Classification;
            }
            else
            {
                xString = x.Classification;
            }

            if (y is ILayeredClassificationType yLayered)
            {
                var baseType = yLayered.BaseTypes.Single(b => b is ILayeredClassificationType bLayered && bLayered.Layer == ClassificationLayer.Semantic);
                yString = baseType.Classification;
            }
            else
            {
                yString = y.Classification;
            }

            return xString.Equals(yString);
        }

        public int GetHashCode(IClassificationType obj)
        {
            throw new NotImplementedException();
        }
    }

    private class ClassificationType : IClassificationType
    {
        public ClassificationType(string classification)
        {
            Classification = classification;
        }

        public string Classification { get; }

        public IEnumerable<IClassificationType> BaseTypes => Array.Empty<IClassificationType>();

        public bool IsOfType(string type)
        {
            throw new System.NotImplementedException();
        }
    }
}
