#:package System.CommandLine
#:package Microsoft.CodeAnalysis
#:project ..\..\..\..\test\TestUtilities\TestUtilities.csproj

using System.Diagnostics;
using System.CommandLine;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.DotNet.SourceBuild.LicenseScanning;
using TestUtilities;
using static Microsoft.DotNet.SourceBuild.LicenseScanning.LicenseScanPolicy;

// Compares license-scan baselines, exclusions, and allowed identifiers between
// Git revisions or the working tree. It enriches changed findings with supplied
// ScanCode evidence and reports the resulting deltas and consistency issues as JSON.
return await LicenseScanDiffApp.RunAsync(args);

internal static class LicenseScanDiffApp
{
    // These repository-relative paths identify the policy files being compared.
    private const string BaselineDirectory =
        "test/Microsoft.DotNet.SourceBuild.Tests/assets/LicenseScanTests";
    private const string LicenseTestPath =
        "test/Microsoft.DotNet.SourceBuild.Tests/LicenseScanTests.cs";
    private const string ExclusionsPath = BaselineDirectory + "/LicenseExclusions.txt";
    private const string AllowedListMarker = "s_allowedLicenseExpressions";

    // The target name encoded in Licenses.<target>.json also determines the
    // default scan root used to turn baseline-relative paths into VMR paths.
    private static readonly Regex s_baselineName = new(
        @"^Licenses\.(?<target>.+)\.json$",
        RegexOptions.CultureInvariant);

    /// <summary>
    /// Parses the command line, performs the analysis, and writes one JSON document.
    /// Exit code 2 is reserved for a successful analysis that found mechanical issues;
    /// malformed input and operational failures use exit code 1.
    /// </summary>
    public static Task<int> RunAsync(string[] args) =>
        CreateCommand().Parse(args).InvokeAsync();

    private static RootCommand CreateCommand()
    {
        Option<string?> repositoryRootOption = new("--repository-root")
        {
            Description = "VMR checkout; defaults to the current checkout.",
        };
        Option<string> baseRefOption = new("--base-ref")
        {
            Description = "Base Git ref, such as main.",
            Required = true,
        };
        Option<string?> toRefOption = new("--to-ref")
        {
            Description = "Git ref to inspect; omit for the working tree.",
        };
        Option<string[]> scanCodeOption = new("--scancode")
        {
            Description = "Attach ScanCode results as TARGET=FILE; repeat as needed.",
        };
        Option<string[]> scanRootOption = new("--scan-root")
        {
            Description = "Override an inferred VMR scan root as TARGET=PATH.",
        };
        Option<string?> outputOption = new("--output")
        {
            Description = "Write JSON to a file instead of stdout.",
        };
        Option<bool> failOnIssuesOption = new("--fail-on-issues")
        {
            Description = "Exit with code 2 when mechanical issues are found.",
        };

        RootCommand command = new(
            "Compare source-build license baselines, exclusions, and allowed licenses.")
        {
            repositoryRootOption,
            baseRefOption,
            toRefOption,
            scanCodeOption,
            scanRootOption,
            outputOption,
            failOnIssuesOption,
        };
        command.SetAction((parseResult, _) => ExecuteAsync(new Options(
            parseResult.GetValue(repositoryRootOption),
            parseResult.GetValue(baseRefOption)!,
            parseResult.GetValue(toRefOption),
            parseResult.GetValue(scanCodeOption) ?? [],
            parseResult.GetValue(scanRootOption) ?? [],
            parseResult.GetValue(outputOption),
            parseResult.GetValue(failOnIssuesOption))));

        return command;
    }

    private static async Task<int> ExecuteAsync(Options options)
    {
        try
        {
            AnalysisResult result = await AnalyzeAsync(options);
            string json = JsonSerializer.Serialize(
                result,
                AnalysisJsonContext.Default.AnalysisResult) + "\n";

            // Keep stdout machine-readable when no output file is requested. Errors
            // are written to stderr by the catch block below.
            if (options.OutputPath is null)
            {
                Console.Write(json);
            }
            else
            {
                await File.WriteAllTextAsync(
                    Path.GetFullPath(options.OutputPath),
                    json,
                    new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
            }

            return options.FailOnIssues && result.Issues.Length > 0 ? 2 : 0;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine($"error: {exception.Message}");
            return 1;
        }
    }

    /// <summary>
    /// Compares the license policy and baseline state at two points, enriching
    /// changed findings with raw ScanCode evidence.
    /// </summary>
    private static async Task<AnalysisResult> AnalyzeAsync(Options options)
    {
        AnalysisState state = await LoadAnalysisStateAsync(options);
        List<BaselineFinding> findings = [];
        List<AnalysisIssue> issues = [];

        AnalyzeBaselineChanges(state, findings, issues);
        AllowedLicenseDelta allowedLicenseDelta =
            AnalyzeAllowedLicenseChanges(state, issues);
        ExclusionDelta exclusionDelta = AnalyzeExclusionChanges(state, issues);

        return new AnalysisResult(
            Comparison: new ComparisonResult(
                state.RepositoryRoot,
                options.BaseRef,
                options.ToRef),
            AllowedLicenseDelta: allowedLicenseDelta,
            ExclusionDelta: exclusionDelta,
            BaselineFindings: findings.ToArray(),
            Issues: issues.ToArray());
    }

    private static async Task<AnalysisState> LoadAnalysisStateAsync(Options options)
    {
        string repositoryRoot = await ResolveRepositoryRootAsync(options.RepositoryRoot);

        // Load baselines independently because files can be added or removed as part
        // of the proposed correction.
        Dictionary<string, Dictionary<string, string?>> beforeBaselines =
            await LoadBaselinesAsync(repositoryRoot, options.BaseRef);
        Dictionary<string, Dictionary<string, string?>> afterBaselines =
            await LoadBaselinesAsync(repositoryRoot, options.ToRef);

        string? beforeTest = await ReadTextAsync(repositoryRoot, LicenseTestPath, options.BaseRef);
        string? afterTest = await ReadTextAsync(repositoryRoot, LicenseTestPath, options.ToRef);
        if (beforeTest is null || afterTest is null)
        {
            throw new InvalidOperationException(
                $"{LicenseTestPath} must exist on both comparison sides");
        }

        Dictionary<string, string?> beforeAllowed = ParseAllowedLicenses(beforeTest);
        Dictionary<string, string?> afterAllowed = ParseAllowedLicenses(afterTest);
        HashSet<string> allowedIds = new(
            afterAllowed.Keys,
            StringComparer.OrdinalIgnoreCase);

        string beforeExclusionText =
            await ReadTextAsync(repositoryRoot, ExclusionsPath, options.BaseRef) ?? "";
        string afterExclusionText =
            await ReadTextAsync(repositoryRoot, ExclusionsPath, options.ToRef) ?? "";
        Dictionary<string, LicenseExclusion> beforeExclusions =
            LicenseScanPolicy.ParseExclusions(beforeExclusionText);
        Dictionary<string, LicenseExclusion> afterExclusions =
            LicenseScanPolicy.ParseExclusions(afterExclusionText);

        Dictionary<string, string> scanCodeMappings =
            ParseMappings(options.ScanCode, "--scancode");
        Dictionary<string, string> scanRootOverrides =
            ParseMappings(options.ScanRoot, "--scan-root");
        Dictionary<string, Dictionary<string, ScanCodeFile>> scanCodeIndexes =
            await LoadScanCodeIndexesAsync(scanCodeMappings);

        HashSet<string> allTargets = new(beforeBaselines.Keys, StringComparer.Ordinal);
        allTargets.UnionWith(afterBaselines.Keys);
        allTargets.UnionWith(scanCodeIndexes.Keys);

        Dictionary<string, string> scanRoots = new(StringComparer.Ordinal);
        foreach (string target in allTargets)
        {
            scanRoots[target] = NormalizePath(
                scanRootOverrides.GetValueOrDefault(target, DefaultScanRoot(target)))
                .TrimEnd('/');
        }

        return new AnalysisState(
            repositoryRoot,
            beforeBaselines,
            afterBaselines,
            beforeAllowed,
            afterAllowed,
            allowedIds,
            beforeExclusions,
            afterExclusions,
            scanCodeIndexes,
            scanRoots);
    }

    private static void AnalyzeBaselineChanges(
        AnalysisState state,
        ICollection<BaselineFinding> findings,
        ICollection<AnalysisIssue> issues)
    {
        HashSet<string> baselineTargets = new(
            state.BeforeBaselines.Keys,
            StringComparer.Ordinal);
        baselineTargets.UnionWith(state.AfterBaselines.Keys);

        // Baseline targets and paths use ordinal comparison because scans run on
        // Linux, where paths differing only by case are distinct.
        foreach (string target in baselineTargets.Order(StringComparer.Ordinal))
        {
            AnalyzeBaselineTarget(state, target, findings, issues);
        }
    }

    private static void AnalyzeBaselineTarget(
        AnalysisState state,
        string target,
        ICollection<BaselineFinding> findings,
        ICollection<AnalysisIssue> issues)
    {
        Dictionary<string, string?> beforeRecords = state.BeforeBaselines.GetValueOrDefault(
            target,
            new Dictionary<string, string?>(StringComparer.Ordinal));
        Dictionary<string, string?> afterRecords = state.AfterBaselines.GetValueOrDefault(
            target,
            new Dictionary<string, string?>(StringComparer.Ordinal));
        BaselineTargetState targetState = new(
            state,
            target,
            state.ScanRoots[target],
            beforeRecords,
            afterRecords,
            state.ScanCodeIndexes.GetValueOrDefault(target));

        string[] changedRecordPaths = GetChangedRecordPaths(beforeRecords, afterRecords);
        if (changedRecordPaths.Length > 0 && targetState.ScanCodeIndex is null)
        {
            issues.Add(new AnalysisIssue(
                "scancode_results_missing_for_target",
                "No ScanCode results were supplied for a changed baseline target.",
                Target: target));
        }

        foreach (string recordPath in changedRecordPaths)
        {
            AnalyzeChangedBaselineRecord(targetState, recordPath, findings, issues);
        }

        ValidateUnchangedBaselineRecords(targetState, issues);

        if (!state.BeforeBaselines.ContainsKey(target) && afterRecords.Count == 0)
        {
            issues.Add(new AnalysisIssue(
                "empty_new_baseline_file",
                "A newly added baseline file contains no findings.",
                Target: target));
        }
    }

    private static string[] GetChangedRecordPaths(
        IReadOnlyDictionary<string, string?> beforeRecords,
        IReadOnlyDictionary<string, string?> afterRecords)
    {
        HashSet<string> recordPaths = new(beforeRecords.Keys, StringComparer.Ordinal);
        recordPaths.UnionWith(afterRecords.Keys);
        return recordPaths
            .Where(recordPath =>
            {
                bool inBefore = beforeRecords.TryGetValue(
                    recordPath,
                    out string? oldExpression);
                bool inAfter = afterRecords.TryGetValue(
                    recordPath,
                    out string? newExpression);
                return inBefore != inAfter || oldExpression != newExpression;
            })
            .Order(StringComparer.Ordinal)
            .ToArray();
    }

    private static void AnalyzeChangedBaselineRecord(
        BaselineTargetState targetState,
        string recordPath,
        ICollection<BaselineFinding> findings,
        ICollection<AnalysisIssue> issues)
    {
        bool inBefore = targetState.BeforeRecords.TryGetValue(
            recordPath,
            out string? oldExpression);
        bool inAfter = targetState.AfterRecords.TryGetValue(
            recordPath,
            out string? newExpression);
        string change = !inBefore
            ? "added"
            : !inAfter
                ? "removed"
                : "changed";

        string vmrPath = NormalizePath($"{targetState.ScanRoot}/{recordPath}");
        Classification classification = Classify(
            inAfter ? newExpression : oldExpression,
            targetState.Analysis.AllowedIds,
            targetState.Analysis.AfterExclusions.Values,
            vmrPath);
        ScanCodeFile? scanRecord =
            targetState.ScanCodeIndex?.GetValueOrDefault(recordPath);

        findings.Add(new BaselineFinding(
            targetState.Target,
            $"{BaselineDirectory}/Licenses.{targetState.Target}.json",
            change,
            recordPath,
            vmrPath,
            oldExpression,
            newExpression,
            classification.Tokens
                .Select(id => new LicenseIdentifier(
                    id,
                    targetState.Analysis.AllowedIds.Contains(id)))
                .ToArray(),
            classification.RemainingDisallowed.ToArray(),
            ToExclusionResults(classification.MatchingExclusions),
            SummarizeScanCodeRecord(scanRecord)));

        if (change is "added" or "changed")
        {
            ValidateBaselinedFinding(
                targetState.Target,
                recordPath,
                classification,
                issues);

            if (targetState.ScanCodeIndex is not null && scanRecord is null)
            {
                issues.Add(new AnalysisIssue(
                    "scancode_record_missing",
                    "No matching file record was found in the supplied ScanCode results.",
                    Target: targetState.Target,
                    Path: recordPath));
            }
            return;
        }

        ValidateRemovedBaselineRecord(targetState, recordPath, vmrPath, scanRecord, issues);
    }

    private static void ValidateRemovedBaselineRecord(
        BaselineTargetState targetState,
        string recordPath,
        string vmrPath,
        ScanCodeFile? scanRecord,
        ICollection<AnalysisIssue> issues)
    {
        if (scanRecord is null)
        {
            return;
        }

        Classification classification = Classify(
            scanRecord.LicenseExpression,
            targetState.Analysis.AllowedIds,
            targetState.Analysis.AfterExclusions.Values,
            vmrPath);
        if (classification.RemainingDisallowed.Count == 0)
        {
            return;
        }

        issues.Add(new AnalysisIssue(
            "baseline_finding_removed_but_still_reported",
            "A removed baseline finding remains disallowed in ScanCode results.",
            Target: targetState.Target,
            Path: recordPath,
            RemainingDisallowedLicenseIds:
                classification.RemainingDisallowed.ToArray()));
    }

    private static void ValidateUnchangedBaselineRecords(
        BaselineTargetState targetState,
        ICollection<AnalysisIssue> issues)
    {
        // Policy changes can make an unchanged baseline record redundant.
        foreach (string recordPath in targetState.BeforeRecords.Keys
                     .Intersect(targetState.AfterRecords.Keys, StringComparer.Ordinal)
                     .Order(StringComparer.Ordinal))
        {
            if (targetState.BeforeRecords[recordPath] !=
                targetState.AfterRecords[recordPath])
            {
                continue;
            }

            Classification classification = Classify(
                targetState.AfterRecords[recordPath],
                targetState.Analysis.AllowedIds,
                targetState.Analysis.AfterExclusions.Values,
                NormalizePath($"{targetState.ScanRoot}/{recordPath}"));

            ValidateBaselinedFinding(
                targetState.Target,
                recordPath,
                classification,
                issues);
        }
    }

    private static AllowedLicenseDelta AnalyzeAllowedLicenseChanges(
        AnalysisState state,
        ICollection<AnalysisIssue> issues)
    {
        string[] addedAllowed = state.AfterAllowed.Keys
            .Except(state.BeforeAllowed.Keys, StringComparer.OrdinalIgnoreCase)
            .Order(StringComparer.OrdinalIgnoreCase)
            .ToArray();
        string[] removedAllowed = state.BeforeAllowed.Keys
            .Except(state.AfterAllowed.Keys, StringComparer.OrdinalIgnoreCase)
            .Order(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        foreach (string licenseId in addedAllowed)
        {
            string? reference = state.AfterAllowed[licenseId];
            if (reference is null ||
                !Regex.IsMatch(reference, @"https?://\S+", RegexOptions.CultureInvariant))
            {
                issues.Add(new AnalysisIssue(
                    "allowed_license_missing_reference",
                    "A newly allowed license does not have an adjacent HTTP(S) reference.",
                    LicenseId: licenseId));
            }
        }

        return new AllowedLicenseDelta(
            addedAllowed
                .Select(id => new AllowedLicense(id, state.AfterAllowed[id]))
                .ToArray(),
            removedAllowed
                .Select(id => new AllowedLicense(id, state.BeforeAllowed[id]))
                .ToArray(),
            state.AfterAllowed.Count);
    }

    private static ExclusionDelta AnalyzeExclusionChanges(
        AnalysisState state,
        ICollection<AnalysisIssue> issues)
    {
        LicenseExclusion[] addedExclusions = state.AfterExclusions
            .Where(entry => !state.BeforeExclusions.ContainsKey(entry.Key))
            .Select(entry => entry.Value)
            .OrderBy(entry => entry.Path, StringComparer.Ordinal)
            .ThenBy(entry => string.Join(',', entry.Licenses), StringComparer.OrdinalIgnoreCase)
            .ToArray();
        LicenseExclusion[] removedExclusions = state.BeforeExclusions
            .Where(entry => !state.AfterExclusions.ContainsKey(entry.Key))
            .Select(entry => entry.Value)
            .OrderBy(entry => entry.Path, StringComparer.Ordinal)
            .ThenBy(entry => string.Join(',', entry.Licenses), StringComparer.OrdinalIgnoreCase)
            .ToArray();

        ValidateAddedExclusions(
            addedExclusions,
            state.ScanRoots,
            state.ScanCodeIndexes,
            issues);

        return new ExclusionDelta(
            ToExclusionResults(addedExclusions),
            ToExclusionResults(removedExclusions));
    }

    private static void ValidateBaselinedFinding(
        string target,
        string path,
        Classification classification,
        ICollection<AnalysisIssue> issues)
    {
        if (classification.Tokens.Count > 0 && classification.Disallowed.Count == 0)
        {
            issues.Add(new AnalysisIssue(
                "baseline_finding_fully_allowed",
                "The baseline finding contains only globally allowed licenses.",
                Target: target,
                Path: path));
        }

        if (classification.Disallowed.Count > 0 &&
            classification.RemainingDisallowed.Count == 0)
        {
            issues.Add(new AnalysisIssue(
                "baseline_finding_fully_excluded",
                "Exclusions remove every disallowed license in this baseline finding.",
                Target: target,
                Path: path));
        }
    }

    /// <summary>
    /// Checks newly added exclusions against VMR path conventions and verifies that
    /// they match supplied ScanCode records and detected identifiers.
    /// </summary>
    private static void ValidateAddedExclusions(
        IReadOnlyList<LicenseExclusion> addedExclusions,
        IReadOnlyDictionary<string, string> scanRoots,
        IReadOnlyDictionary<string, Dictionary<string, ScanCodeFile>> scanCodeIndexes,
        ICollection<AnalysisIssue> issues)
    {
        foreach (LicenseExclusion exclusion in addedExclusions)
        {
            // LicenseExclusions.txt is interpreted from the VMR root. A baseline-
            // relative path would never match in CI even if it looks plausible.
            if (!exclusion.Path.StartsWith("src/", StringComparison.Ordinal))
            {
                issues.Add(new AnalysisIssue(
                    "exclusion_path_not_vmr_rooted",
                    "An added exclusion path does not start with src/.",
                    Path: exclusion.Path));
            }

            // Several scan roots can share a prefix. Select the longest matching root
            // so a split scan such as source-build-assets.textOnlyPackages wins over
            // the parent source-build-assets target.
            KeyValuePair<string, string>? match = scanRoots
                .Where(entry =>
                    exclusion.Path.StartsWith(entry.Value + "/", StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(entry => entry.Value.Length)
                .Cast<KeyValuePair<string, string>?>()
                .FirstOrDefault();
            if (match is null)
            {
                issues.Add(new AnalysisIssue(
                    "scan_target_missing_for_exclusion",
                    "No scan target matches an added exclusion path.",
                    Path: exclusion.Path));
                continue;
            }

            string target = match.Value.Key;
            string scanRoot = match.Value.Value;
            string relativePattern = exclusion.Path[(scanRoot.Length + 1)..];
            if (!scanCodeIndexes.TryGetValue(
                    target,
                    out Dictionary<string, ScanCodeFile>? scanCodeIndex))
            {
                issues.Add(new AnalysisIssue(
                    "scancode_results_missing_for_exclusion",
                    "No ScanCode results were supplied for an added exclusion.",
                    Target: target,
                    Path: exclusion.Path));
                continue;
            }

            // Exclusion paths support globs, whereas ScanCode records contain literal
            // paths. Expand the proposed exclusion against the raw result index.
            ScanCodeFile[] scanRecords = scanCodeIndex
                .Where(entry => LicenseScanPolicy.PathMatches(relativePattern, entry.Key))
                .Select(entry => entry.Value)
                .ToArray();

            if (scanRecords.Length == 0)
            {
                issues.Add(new AnalysisIssue(
                    "scancode_record_missing_for_exclusion",
                    "No matching file record was found for an added exclusion.",
                    Target: target,
                    Path: exclusion.Path,
                    RelativePattern: relativePattern));
                continue;
            }

            if (exclusion.Licenses.Count == 0)
            {
                continue;
            }

            // A license-scoped exclusion is suspicious if none of its matching files
            // contain that identifier. This catches typos and copied stale entries.
            HashSet<string> detectedLicenses = new(StringComparer.OrdinalIgnoreCase);
            foreach (ScanCodeFile record in scanRecords)
            {
                detectedLicenses.UnionWith(
                    LicenseScanPolicy.SplitExpression(record.LicenseExpression));
            }

            foreach (string licenseId in exclusion.Licenses)
            {
                if (!detectedLicenses.Contains(licenseId))
                {
                    issues.Add(new AnalysisIssue(
                        "excluded_license_not_detected",
                        "An added scoped exclusion does not match a detected license identifier.",
                        Target: target,
                        Path: exclusion.Path,
                        LicenseId: licenseId));
                }
            }
        }
    }

    /// <summary>
    /// Splits a ScanCode expression using the same deliberately simple rules as
    /// LicenseScanTests.cs, then applies global allowlisting and path exclusions.
    /// </summary>
    private static Classification Classify(
        string? expression,
        IReadOnlySet<string> allowedIds,
        IEnumerable<LicenseExclusion> exclusions,
        string vmrPath)
    {
        string[] tokens = LicenseScanPolicy.SplitExpression(expression);
        string[] disallowed = tokens.Where(id => !allowedIds.Contains(id)).ToArray();

        // Exclusion path matching follows FileSystemGlobbing behavior for the patterns
        // used by this repository. License identifiers remain case-insensitive.
        LicenseExclusion[] matchingExclusions = exclusions
            .Where(exclusion => LicenseScanPolicy.PathMatches(exclusion.Path, vmrPath))
            .OrderBy(exclusion => exclusion.Path, StringComparer.Ordinal)
            .ThenBy(exclusion => string.Join(',', exclusion.Licenses), StringComparer.OrdinalIgnoreCase)
            .ToArray();
        ExclusionFileEntry[] matchingEntries = matchingExclusions
            .Select(exclusion => new ExclusionFileEntry(
                exclusion.Path,
                exclusion.Licenses))
            .ToArray();
        string[] remaining = disallowed
            .Where(id => ExclusionsHelper.FindMatchingExclusion(
                matchingEntries,
                vmrPath,
                id) is null)
            .ToArray();

        return new Classification(tokens, disallowed, remaining, matchingExclusions);
    }

    /// <summary>
    /// Extracts allowed identifiers and their adjacent comments from the array
    /// initializer in LicenseScanTests.cs.
    /// </summary>
    private static Dictionary<string, string?> ParseAllowedLicenses(string content)
    {
        SyntaxNode root = CSharpSyntaxTree.ParseText(content).GetRoot();
        VariableDeclaratorSyntax[] declarations = root.DescendantNodes()
            .OfType<VariableDeclaratorSyntax>()
            .Where(declaration =>
                declaration.Identifier.ValueText == AllowedListMarker)
            .ToArray();
        if (declarations.Length != 1)
        {
            throw new InvalidOperationException(
                $"Expected exactly one {AllowedListMarker} declaration in " +
                $"{LicenseTestPath}, but found {declarations.Length}");
        }

        InitializerExpressionSyntax? initializer = declarations[0].Initializer?.Value
            .DescendantNodesAndSelf()
            .OfType<InitializerExpressionSyntax>()
            .SingleOrDefault(node => node.IsKind(SyntaxKind.ArrayInitializerExpression));
        if (initializer is null)
        {
            throw new InvalidOperationException(
                $"Could not find the array initializer for {AllowedListMarker} in " +
                LicenseTestPath);
        }

        Dictionary<string, string?> result = new(StringComparer.OrdinalIgnoreCase);
        foreach (ExpressionSyntax expression in initializer.Expressions)
        {
            if (expression is not LiteralExpressionSyntax literal ||
                !literal.IsKind(SyntaxKind.StringLiteralExpression))
            {
                throw new InvalidOperationException(
                    $"{AllowedListMarker} contains a non-string-literal expression in " +
                    LicenseTestPath);
            }

            SyntaxTriviaList trailingTrivia = expression.GetTrailingTrivia();
            SyntaxToken nextToken = expression.GetLastToken().GetNextToken(
                includeZeroWidth: true);
            if (nextToken.IsKind(SyntaxKind.CommaToken))
            {
                trailingTrivia = trailingTrivia.AddRange(nextToken.TrailingTrivia);
            }

            SyntaxTrivia comment = trailingTrivia.FirstOrDefault(
                trivia => trivia.IsKind(SyntaxKind.SingleLineCommentTrivia));
            string? reference = comment.RawKind == 0
                ? null
                : comment.ToString()[2..].Trim();
            result[literal.Token.ValueText] = reference;
        }
        return result;
    }

    /// <summary>
    /// Loads the license baseline files from a Git tree or the working tree into an
    /// ordinal, path-keyed index grouped by scan target.
    /// </summary>
    private static async Task<Dictionary<string, Dictionary<string, string?>>> LoadBaselinesAsync(
        string repositoryRoot,
        string? gitRef)
    {
        Dictionary<string, Dictionary<string, string?>> baselines =
            new(StringComparer.Ordinal);

        foreach (string path in (await GetBaselinePathsAsync(repositoryRoot, gitRef))
                     .Order(StringComparer.Ordinal))
        {
            Match match = s_baselineName.Match(Path.GetFileName(path));
            if (!match.Success)
            {
                continue;
            }

            string? content = await ReadTextAsync(repositoryRoot, path, gitRef);
            if (content is null)
            {
                continue;
            }

            LicenseScanDocument document;
            try
            {
                document = JsonSerializer.Deserialize(
                    content,
                    AnalysisJsonContext.Default.LicenseScanDocument)
                    ?? throw new JsonException("The document is null.");
            }
            catch (JsonException exception)
            {
                throw new InvalidOperationException($"Invalid JSON in {path}: {exception.Message}");
            }

            Dictionary<string, string?> records = new(StringComparer.Ordinal);
            foreach (LicenseScanFile record in document.Files)
            {
                if (string.IsNullOrWhiteSpace(record.Path))
                {
                    throw new InvalidOperationException(
                        $"A baseline record in {path} does not have a path.");
                }

                string recordPath = NormalizePath(record.Path);
                if (!records.TryAdd(
                        recordPath,
                        record.LicenseExpression))
                {
                    throw new InvalidOperationException(
                        $"Duplicate baseline path {recordPath} in {path}");
                }
            }
            baselines[match.Groups["target"].Value] = records;
        }

        return baselines;
    }

    /// <summary>
    /// Enumerates baseline paths without checking out either comparison side.
    /// </summary>
    private static async Task<string[]> GetBaselinePathsAsync(
        string repositoryRoot,
        string? gitRef)
    {
        if (gitRef is null)
        {
            // The working-tree side must include untracked baseline files, so use the
            // filesystem rather than git ls-files.
            string directory = Path.Combine(repositoryRoot, BaselineDirectory);
            return Directory.EnumerateFiles(directory, "Licenses.*.json")
                .Select(path => NormalizePath(Path.GetRelativePath(repositoryRoot, path)))
                .ToArray();
        }

        ProcessResult result = await RunGitAsync(
            repositoryRoot,
            ["ls-tree", "-r", "--name-only", gitRef, "--", BaselineDirectory]);
        return SplitLines(result.Output)
            .Where(path => s_baselineName.IsMatch(Path.GetFileName(path)))
            .ToArray();
    }

    /// <summary>
    /// Builds a case-sensitive file index for each supplied ScanCode JSON document.
    /// </summary>
    private static async Task<Dictionary<string, Dictionary<string, ScanCodeFile>>>
        LoadScanCodeIndexesAsync(IReadOnlyDictionary<string, string> mappings)
    {
        Dictionary<string, Dictionary<string, ScanCodeFile>> indexes =
            new(StringComparer.Ordinal);
        foreach ((string target, string fileName) in mappings)
        {
            ScanCodeDocument document;
            try
            {
                string content = await File.ReadAllTextAsync(fileName);
                document = JsonSerializer.Deserialize(
                    content,
                    AnalysisJsonContext.Default.ScanCodeDocument)
                    ?? throw new JsonException("The document is null.");
            }
            catch (Exception exception) when (
                exception is IOException or UnauthorizedAccessException or JsonException)
            {
                throw new InvalidOperationException(
                    $"Could not read ScanCode results {fileName}: {exception.Message}");
            }

            // ScanCode paths originate on Linux and can legally differ only by case,
            // so make the intended ordinal comparison explicit.
            Dictionary<string, ScanCodeFile> index = new(StringComparer.Ordinal);
            foreach (ScanCodeFile record in document.Files)
            {
                string recordPath = NormalizePath(record.Path ?? "");
                if (recordPath.Length > 0 && !index.TryAdd(recordPath, record))
                {
                    throw new InvalidOperationException(
                        $"Duplicate ScanCode path {recordPath} in {fileName}");
                }
            }
            indexes[target] = index;
        }
        return indexes;
    }

    /// <summary>
    /// Reduces a potentially large ScanCode file record to evidence useful during
    /// review.
    /// </summary>
    private static ScanCodeEvidence? SummarizeScanCodeRecord(ScanCodeFile? record)
    {
        if (record is null)
        {
            return null;
        }

        List<ScanCodeMatch> matches = [];

        foreach (ScanCodeLicenseDetection detection in record.LicenseDetections)
        {
            foreach (ScanCodeLicenseMatch match in detection.Matches)
            {
                matches.Add(CreateScanCodeMatch(
                    detection.LicenseExpression,
                    match.LicenseExpression,
                    match.RuleIdentifier,
                    match));
            }
        }

        return new ScanCodeEvidence(
            record.Path,
            record.LicenseExpression,
            matches.ToArray());
    }

    /// <summary>
    /// Selects the fields needed to establish what matched, where, and with what
    /// confidence. Missing fields remain null rather than being synthesized.
    /// </summary>
    private static ScanCodeMatch CreateScanCodeMatch(
        string? detectionExpression,
        string? licenseExpression,
        string? ruleIdentifier,
        ScanCodeLicenseMatch match) =>
        new(
            detectionExpression,
            licenseExpression,
            ruleIdentifier,
            match.Matcher,
            match.Score,
            match.StartLine,
            match.EndLine,
            match.MatchedText);

    private static ExclusionResult[] ToExclusionResults(
        IEnumerable<LicenseExclusion> values) =>
        values.Select(exclusion =>
            new ExclusionResult(exclusion.Path, exclusion.Licenses.ToArray())).ToArray();

    /// <summary>
    /// Reconstructs the VMR scan root from the target naming convention used by
    /// LicenseScanTests: repo for root scans and repo/src/subdirectory for split scans.
    /// </summary>
    private static string DefaultScanRoot(string target)
    {
        int separator = target.IndexOf('.');
        return separator < 0
            ? $"src/{target}"
            : $"src/{target[..separator]}/src/{target[(separator + 1)..]}";
    }

    /// <summary>
    /// Parses repeatable target mappings while preserving target-name casing.
    /// </summary>
    private static Dictionary<string, string> ParseMappings(
        IEnumerable<string> values,
        string option)
    {
        Dictionary<string, string> result = new(StringComparer.Ordinal);
        foreach (string value in values)
        {
            int separator = value.IndexOf('=');
            if (separator <= 0 || separator == value.Length - 1)
            {
                throw new ArgumentException($"{option} expects TARGET=VALUE, got '{value}'");
            }
            string key = value[..separator].Trim();
            string mappedValue = value[(separator + 1)..].Trim();
            if (key.Length == 0 || mappedValue.Length == 0)
            {
                throw new ArgumentException($"{option} expects TARGET=VALUE, got '{value}'");
            }
            result[key] = mappedValue;
        }
        return result;
    }

    /// <summary>
    /// Resolves the canonical checkout root from either --repository-root or the
    /// current directory.
    /// </summary>
    private static async Task<string> ResolveRepositoryRootAsync(string? value)
    {
        string start = Path.GetFullPath(value ?? Environment.CurrentDirectory);
        return (await RunGitAsync(start, ["rev-parse", "--show-toplevel"])).Output.Trim();
    }

    /// <summary>
    /// Reads a repository-relative file from disk or directly from a Git tree without
    /// changing the user's checkout.
    /// </summary>
    private static async Task<string?> ReadTextAsync(
        string repositoryRoot,
        string path,
        string? gitRef)
    {
        if (gitRef is null)
        {
            string diskPath = Path.Combine(repositoryRoot, path);
            return File.Exists(diskPath) ? await File.ReadAllTextAsync(diskPath) : null;
        }

        // --to-ref comparisons must not check out the PR. git show provides immutable
        // content and allows the analyzer to run safely in a dirty worktree.
        ProcessResult result = await RunGitAsync(
            repositoryRoot,
            ["show", $"{gitRef}:{NormalizePath(path)}"],
            allowFailure: true);
        if (result.ExitCode == 0)
        {
            return result.Output;
        }
        if (result.Output.Contains("does not exist", StringComparison.Ordinal) ||
            result.Output.Contains("exists on disk, but not in", StringComparison.Ordinal))
        {
            return null;
        }
        throw new InvalidOperationException(
            $"git show {gitRef}:{path} failed: {result.Output.Trim()}");
    }

    /// <summary>
    /// Executes Git with argument-list escaping and drains both output streams
    /// asynchronously to avoid deadlocks on large baseline listings.
    /// </summary>
    private static async Task<ProcessResult> RunGitAsync(
        string repositoryRoot,
        IReadOnlyList<string> arguments,
        bool allowFailure = false)
    {
        ProcessStartInfo startInfo = new("git")
        {
            WorkingDirectory = repositoryRoot,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };
        startInfo.ArgumentList.Add("-C");
        startInfo.ArgumentList.Add(repositoryRoot);
        foreach (string argument in arguments)
        {
            startInfo.ArgumentList.Add(argument);
        }

        using Process process = Process.Start(startInfo)!;

        // Begin reading both redirected streams before waiting for process completion;
        // otherwise a full pipe could block Git and the analyzer indefinitely.
        Task<string> standardOutput = process.StandardOutput.ReadToEndAsync();
        Task<string> standardError = process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();
        string output = await standardOutput;
        string error = await standardError;
        string combined = string.IsNullOrEmpty(error)
            ? output.TrimEnd('\r', '\n')
            : string.IsNullOrEmpty(output)
                ? error.TrimEnd('\r', '\n')
                : output.TrimEnd('\r', '\n') + "\n" + error.TrimEnd('\r', '\n');

        if (process.ExitCode != 0 && !allowFailure)
        {
            throw new InvalidOperationException(
                $"git {string.Join(' ', arguments)} failed: {combined.Trim()}");
        }
        return new ProcessResult(process.ExitCode, combined);
    }

    private static string[] SplitLines(string value) =>
        value.Split(["\r\n", "\n"], StringSplitOptions.RemoveEmptyEntries);

    private sealed record Classification(
        IReadOnlyList<string> Tokens,
        IReadOnlyList<string> Disallowed,
        IReadOnlyList<string> RemainingDisallowed,
        IReadOnlyList<LicenseExclusion> MatchingExclusions);

    private sealed record ProcessResult(int ExitCode, string Output);

    private sealed record AnalysisState(
        string RepositoryRoot,
        Dictionary<string, Dictionary<string, string?>> BeforeBaselines,
        Dictionary<string, Dictionary<string, string?>> AfterBaselines,
        Dictionary<string, string?> BeforeAllowed,
        Dictionary<string, string?> AfterAllowed,
        HashSet<string> AllowedIds,
        Dictionary<string, LicenseExclusion> BeforeExclusions,
        Dictionary<string, LicenseExclusion> AfterExclusions,
        Dictionary<string, Dictionary<string, ScanCodeFile>> ScanCodeIndexes,
        Dictionary<string, string> ScanRoots);

    private sealed record BaselineTargetState(
        AnalysisState Analysis,
        string Target,
        string ScanRoot,
        Dictionary<string, string?> BeforeRecords,
        Dictionary<string, string?> AfterRecords,
        Dictionary<string, ScanCodeFile>? ScanCodeIndex);

    /// <summary>
    /// Immutable command-line configuration. Repeatable mappings are collected as raw
    /// strings and validated centrally by ParseMappings.
    /// </summary>
    private sealed record Options(
        string? RepositoryRoot,
        string BaseRef,
        string? ToRef,
        IReadOnlyList<string> ScanCode,
        IReadOnlyList<string> ScanRoot,
        string? OutputPath,
        bool FailOnIssues);
}
internal sealed record AnalysisResult(
    ComparisonResult Comparison,
    AllowedLicenseDelta AllowedLicenseDelta,
    ExclusionDelta ExclusionDelta,
    BaselineFinding[] BaselineFindings,
    AnalysisIssue[] Issues);

internal sealed record ComparisonResult(
    string RepoRoot,
    string BaseRef,
    string? ToRef);

internal sealed record AllowedLicenseDelta(
    AllowedLicense[] Added,
    AllowedLicense[] Removed,
    int CurrentCount);

internal sealed record AllowedLicense(
    string Id,
    string? Reference);

internal sealed record ExclusionDelta(
    ExclusionResult[] Added,
    ExclusionResult[] Removed);

internal sealed record ExclusionResult(
    string Path,
    string[] Licenses);

internal sealed record BaselineFinding(
    string Target,
    string BaselineFile,
    string Change,
    string Path,
    string VmrPath,
    string? OldExpression,
    string? NewExpression,
    LicenseIdentifier[] LicenseIds,
    string[] RemainingDisallowedLicenseIds,
    ExclusionResult[] MatchingExclusions,
    ScanCodeEvidence? ScancodeEvidence);

internal sealed record LicenseIdentifier(
    string Id,
    bool Allowed);

internal sealed record ScanCodeEvidence(
    string? Path,
    string? DetectedLicenseExpression,
    ScanCodeMatch[] Matches);

internal sealed record ScanCodeMatch(
    string? DetectionExpression,
    string? LicenseExpression,
    string? RuleIdentifier,
    string? Matcher,
    decimal? Score,
    int? StartLine,
    int? EndLine,
    string? MatchedText);

internal sealed record AnalysisIssue(
    string Code,
    string Message,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    string? Target = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    string? Path = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    string? LicenseId = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    string[]? RemainingDisallowedLicenseIds = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    string? RelativePattern = null);

[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower,
    WriteIndented = true)]
[JsonSerializable(typeof(AnalysisResult))]
[JsonSerializable(typeof(LicenseScanDocument))]
[JsonSerializable(typeof(ScanCodeDocument))]
internal sealed partial class AnalysisJsonContext : JsonSerializerContext;
