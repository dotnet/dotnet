#!/usr/bin/env dotnet

#pragma warning disable CS8600, CS8602, CS8604, IL2026, IL3050

using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

// =============================================================================
// flow-health.cs — Batch-scan codeflow PR health across branches
//
// Usage: dotnet <path>/flow-health.cs -- dotnet/runtime [--branch main]
//
// Replaces Get-FlowHealth.ps1 to avoid PowerShell 5.1 compatibility issues
// (bracket-mangling in [bot] author filters, regex parsing in -File mode).
// =============================================================================

var repository = args.Length > 0 ? args[0] : null;
var branch = args.Length > 1 && args[1] == "--branch" && args.Length > 2 ? args[2] : null;

if (string.IsNullOrEmpty(repository) || !Regex.IsMatch(repository, @"^[A-Za-z0-9_.\-]+/[A-Za-z0-9_.\-]+$"))
{
    Console.Error.WriteLine("Usage: flow-health.cs -- <owner/repo> [--branch <name>]");
    return 1;
}

const int ReleasedPreviewThresholdDays = 14;
const int MergedRetries = 3;
const int MaxConcurrentGhCalls = 20;

// =============================================================================
// gh CLI helper — properly async with error tracking
// =============================================================================

var ghSemaphore = new SemaphoreSlim(MaxConcurrentGhCalls);
int ghFailureCount = 0;

async Task<(string? output, string? error)> RunGhCoreAsync(string arguments, int timeoutSeconds = 30)
{
    await ghSemaphore.WaitAsync();
    try
    {
        var psi = new ProcessStartInfo("gh", arguments)
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        using var proc = Process.Start(psi);
        if (proc == null)
        {
            Interlocked.Increment(ref ghFailureCount);
            return (null, "Failed to start gh process");
        }
        var outputTask = proc.StandardOutput.ReadToEndAsync();
        var errorTask = proc.StandardError.ReadToEndAsync();
        var exited = true;
        try { await proc.WaitForExitAsync().WaitAsync(TimeSpan.FromSeconds(timeoutSeconds)); }
        catch (TimeoutException) { exited = false; }
        if (!exited)
        {
            try { proc.Kill(entireProcessTree: true); } catch { }
            Interlocked.Increment(ref ghFailureCount);
            return (null, $"gh timed out after {timeoutSeconds}s: {arguments}");
        }
        var output = await outputTask;
        var stderr = await errorTask;
        if (proc.ExitCode != 0)
        {
            Interlocked.Increment(ref ghFailureCount);
            return (null, stderr);
        }
        return (output, null);
    }
    catch (Exception ex)
    {
        Interlocked.Increment(ref ghFailureCount);
        return (null, ex.Message);
    }
    finally
    {
        ghSemaphore.Release();
    }
}

async Task<string?> RunGhAsync(string arguments, int timeoutSeconds = 30)
{
    var (output, _) = await RunGhCoreAsync(arguments, timeoutSeconds);
    return output;
}

async Task<JsonArray?> RunGhJsonAsync(string arguments, int timeoutSeconds = 30)
{
    var output = await RunGhAsync(arguments, timeoutSeconds);
    if (string.IsNullOrWhiteSpace(output)) return null;
    try { return JsonNode.Parse(output) as JsonArray; }
    catch { return null; }
}

async Task<JsonObject?> RunGhJsonObjectAsync(string arguments, int timeoutSeconds = 60)
{
    var output = await RunGhAsync(arguments, timeoutSeconds);
    if (string.IsNullOrWhiteSpace(output)) return null;
    try { return JsonNode.Parse(output) as JsonObject; }
    catch { return null; }
}

// Verify gh is available before starting
var (ghCheck, ghCheckErr) = await RunGhCoreAsync("auth status", 10);
if (ghCheck == null)
{
    Console.Error.WriteLine($"❌ gh CLI is not available or not authenticated: {ghCheckErr}");
    return 1;
}
ghFailureCount = 0; // reset after auth check

// =============================================================================
// Phase 1: Find codeflow PRs
// =============================================================================

Console.Error.WriteLine($"🔍 Searching for codeflow PRs in {repository}...");

// Open backflow PRs — uses --search to embed author filter (avoids PS 5.1 --author [bot] bug)
var allOpen = await RunGhJsonAsync($"pr list --repo {repository} --search \"author:app/dotnet-maestro\" --state open --json number,title --limit 100");
var openPRs = new List<(int number, string title, string branch)>();
if (allOpen != null)
{
    foreach (var pr in allOpen)
    {
        var title = pr?["title"]?.ToString() ?? "";
        if (!title.Contains("Source code updates from dotnet/dotnet")) continue;
        var num = pr?["number"]?.GetValue<int>() ?? 0;
        var m = Regex.Match(title, @"^\[([^\]]+)\]");
        if (m.Success && num > 0)
            openPRs.Add((num, title, m.Groups[1].Value));
    }
}

// Merged backflow PRs — retry up to 3 times (GitHub search index eventual consistency)
var mergedPRs = new List<(int number, string title, string branch, string? closedAt)>();
for (int retry = 0; retry < MergedRetries && mergedPRs.Count == 0; retry++)
{
    if (retry > 0) await Task.Delay(1000);
    var allMerged = await RunGhJsonAsync($"pr list --repo {repository} --search \"author:app/dotnet-maestro Source code updates from dotnet/dotnet\" --state merged --json number,title,closedAt --limit 30");
    if (allMerged != null)
    {
        foreach (var pr in allMerged)
        {
            var title = pr?["title"]?.ToString() ?? "";
            var num = pr?["number"]?.GetValue<int>() ?? 0;
            var closedAt = pr?["closedAt"]?.ToString();
            var m = Regex.Match(title, @"^\[([^\]]+)\]");
            if (m.Success && num > 0)
            {
                var brName = m.Groups[1].Value;
                if (branch != null && brName != branch) continue;
                mergedPRs.Add((num, title, brName, closedAt));
            }
        }
    }
}

Console.Error.WriteLine($"  ✅ Found {openPRs.Count} open, {mergedPRs.Count} merged codeflow PRs");

// Map open PRs by branch
var openBranches = new Dictionary<string, int>();
foreach (var pr in openPRs)
    openBranches[pr.branch] = pr.number;

// Group merged PRs by branch (most recent per branch)
var branchLastMerged = new Dictionary<string, (int number, string? closedAt)>();
foreach (var pr in mergedPRs)
{
    if (!branchLastMerged.TryGetValue(pr.branch, out var existing) ||
        string.Compare(pr.closedAt, existing.closedAt, StringComparison.Ordinal) > 0)
    {
        branchLastMerged[pr.branch] = (pr.number, pr.closedAt);
    }
}

// =============================================================================
// Phase 2: Parallel fetch — PR bodies, health data, VMR metadata
// =============================================================================

Console.Error.WriteLine("📦 Fetching PR metadata and VMR branch data...");

// Fetch merged PR bodies (to extract VMR branch/commit)
var bodyTasks = new Dictionary<string, Task<string?>>();
foreach (var kv in branchLastMerged.OrderBy(k => k.Key))
{
    if (openBranches.ContainsKey(kv.Key)) continue;
    bodyTasks[kv.Key] = RunGhAsync($"pr view {kv.Value.number} -R {repository} --json body");
}

// Fetch open PR health data
var healthTasks = new Dictionary<string, Task<string?>>();
foreach (var kv in openBranches.OrderBy(k => k.Key))
{
    if (branch != null && kv.Key != branch) continue;
    healthTasks[kv.Key] = RunGhAsync($"pr view {kv.Value} -R {repository} --json body,comments,updatedAt,mergeable,statusCheckRollup", 60);
}

// Await all body tasks, extract VMR metadata
var vmrBranches = new Dictionary<string, string>();
var vmrCommits = new Dictionary<string, string>();
foreach (var kv in bodyTasks)
{
    var json = await kv.Value;
    if (json == null) continue;
    try
    {
        var obj = JsonNode.Parse(json);
        var body = obj?["body"]?.ToString() ?? "";
        var branchMatch = Regex.Match(body, @"\*\*Branch\*\*:\s*\[([^\]]+)\]");
        if (branchMatch.Success) vmrBranches[kv.Key] = branchMatch.Groups[1].Value;
        var commitMatch = Regex.Match(body, @"\*\*Commit\*\*:\s*\[([a-fA-F0-9]+)\]");
        if (commitMatch.Success) vmrCommits[kv.Key] = commitMatch.Groups[1].Value;
    }
    catch { }
}

// =============================================================================
// Phase 3: VMR branch HEAD comparisons
// =============================================================================

Console.Error.WriteLine("🔗 Comparing VMR branch HEADs...");

var vmrHeadTasks = new Dictionary<string, Task<JsonObject?>>();
foreach (var kv in vmrBranches)
{
    if (!vmrCommits.ContainsKey(kv.Key)) continue;
    var encoded = Uri.EscapeDataString(kv.Value);
    vmrHeadTasks[kv.Key] = RunGhJsonObjectAsync($"api /repos/dotnet/dotnet/commits/{encoded}");
}

var vmrHeads = new Dictionary<string, JsonObject>();
foreach (var kv in vmrHeadTasks)
{
    var result = await kv.Value;
    if (result != null) vmrHeads[kv.Key] = result;
}

// Compare commits for branches without open PRs
var compareTasks = new Dictionary<string, Task<JsonObject?>>();
foreach (var kv in vmrCommits)
{
    if (openBranches.ContainsKey(kv.Key)) continue;
    if (!vmrHeads.TryGetValue(kv.Key, out var head)) continue;
    var headSha = head["sha"]?.ToString() ?? "";
    if (headSha.StartsWith(kv.Value) || kv.Value.StartsWith(headSha) || headSha == kv.Value) continue;
    compareTasks[kv.Key] = RunGhJsonObjectAsync($"api /repos/dotnet/dotnet/compare/{kv.Value}...{headSha}");
}

var compareResults = new Dictionary<string, JsonObject>();
foreach (var kv in compareTasks)
{
    var result = await kv.Value;
    if (result != null) compareResults[kv.Key] = result;
}

// =============================================================================
// Phase 4: Open PR health analysis
// =============================================================================

var openPRHealth = new Dictionary<string, Dictionary<string, object>>();
foreach (var kv in healthTasks)
{
    var json = await kv.Value;
    if (json == null) continue;
    try
    {
        var detail = JsonNode.Parse(json);
        if (detail == null) continue;
        var health = new Dictionary<string, object>
        {
            ["status"] = "healthy",
            ["hasConflict"] = false,
            ["hasStaleness"] = false
        };

        if (detail["mergeable"]?.ToString() == "CONFLICTING")
            health["hasConflict"] = true;

        var comments = detail["comments"]?.AsArray();
        if (comments != null)
        {
            foreach (var c in comments)
            {
                var login = c?["author"]?["login"]?.ToString() ?? "";
                if (!login.StartsWith("dotnet-maestro")) continue;
                var body = c?["body"]?.ToString() ?? "";
                if (body.Contains("codeflow cannot continue") || body.Contains("the source repository has received code changes"))
                    health["hasStaleness"] = true;
                if (body.Contains("Conflict detected"))
                    health["hasConflict"] = true;
            }
        }

        // CI status
        var rollup = detail["statusCheckRollup"]?.AsArray();
        if (rollup != null && rollup.Count > 0)
        {
            int failed = 0, pending = 0, total = rollup.Count;
            foreach (var ctx in rollup)
            {
                var conclusion = ctx?["conclusion"]?.ToString() ?? "";
                var status = ctx?["status"]?.ToString() ?? "";
                var state = ctx?["state"]?.ToString() ?? "";
                if (conclusion is "FAILURE" or "ERROR" || state is "FAILURE" or "ERROR") failed++;
                else if (status is "IN_PROGRESS" or "QUEUED" or "PENDING" || state == "PENDING") pending++;
            }
            if (failed > 0) { health["ciStatus"] = "red"; health["ciFailedCount"] = failed; health["ciTotalCount"] = total; }
            else if (pending > 0) health["ciStatus"] = "pending";
            else health["ciStatus"] = "green";
        }
        else health["ciStatus"] = "none";

        if ((bool)health["hasConflict"]) health["status"] = "conflict";
        else if ((bool)health["hasStaleness"]) health["status"] = "stale";
        else if (health["ciStatus"].ToString() == "red") health["status"] = "ci-red";

        // Extract subscription ID and VMR branch from body
        var prBody = detail["body"]?.ToString() ?? "";
        var subMatch = Regex.Match(prBody, @"\(Begin:([a-f0-9\-]+)\)");
        if (subMatch.Success) health["subscriptionId"] = subMatch.Groups[1].Value;
        var vmrBrMatch = Regex.Match(prBody, @"\*\*Branch\*\*:\s*\[([^\]]+)\]");
        if (vmrBrMatch.Success) health["vmrBranch"] = vmrBrMatch.Groups[1].Value;

        openPRHealth[kv.Key] = health;
    }
    catch { }
}

// =============================================================================
// Phase 5: Forward flow scan
// =============================================================================

Console.Error.WriteLine("↔️ Scanning forward flow PRs...");
var repoShortName = Regex.Replace(repository, @"^dotnet/", "");

var fwdJson = await RunGhJsonAsync($"pr list --repo dotnet/dotnet --search \"author:app/dotnet-maestro Source code updates from dotnet/{repoShortName}\" --state open --json number,title --limit 10");
var fwdPRs = new List<(int number, string title, string branch)>();
if (fwdJson != null)
{
    var escapedName = Regex.Escape(repoShortName);
    foreach (var pr in fwdJson)
    {
        var title = pr?["title"]?.ToString() ?? "";
        var num = pr?["number"]?.GetValue<int>() ?? 0;
        if (num > 0 && Regex.IsMatch(title, $"from dotnet/{escapedName}$"))
        {
            var m = Regex.Match(title, @"^\[([^\]]+)\]");
            fwdPRs.Add((num, title, m.Success ? m.Groups[1].Value : "unknown"));
        }
    }
}

// Fetch forward flow health in parallel
var fwdHealthTasks = new Dictionary<int, Task<string?>>();
foreach (var fpr in fwdPRs)
    fwdHealthTasks[fpr.number] = RunGhAsync($"pr view {fpr.number} -R dotnet/dotnet --json comments,mergeable,statusCheckRollup", 60);

var fwdHealth = new Dictionary<int, Dictionary<string, object>>();
foreach (var fpr in fwdPRs)
{
    var json = await fwdHealthTasks[fpr.number];
    if (json == null) continue;
    try
    {
        var detail = JsonNode.Parse(json);
        if (detail == null) continue;
        var h = new Dictionary<string, object>
        {
            ["status"] = "healthy",
            ["hasConflict"] = false,
            ["hasStaleness"] = false,
            ["ciStatus"] = "none"
        };

        if (detail["mergeable"]?.ToString() == "CONFLICTING") h["hasConflict"] = true;

        var comments = detail["comments"]?.AsArray();
        if (comments != null)
        {
            foreach (var c in comments)
            {
                var login = c?["author"]?["login"]?.ToString() ?? "";
                if (!login.StartsWith("dotnet-maestro")) continue;
                var body = c?["body"]?.ToString() ?? "";
                if (body.Contains("codeflow cannot continue") || body.Contains("the source repository has received code changes"))
                    h["hasStaleness"] = true;
                if (body.Contains("Conflict detected"))
                    h["hasConflict"] = true;
            }
        }

        if ((bool)h["hasConflict"]) h["status"] = "conflict";
        else if ((bool)h["hasStaleness"]) h["status"] = "stale";

        var rollup = detail["statusCheckRollup"]?.AsArray();
        if (rollup != null && rollup.Count > 0)
        {
            int failed = 0, pending = 0;
            foreach (var ctx in rollup)
            {
                var conclusion = ctx?["conclusion"]?.ToString() ?? "";
                var status = ctx?["status"]?.ToString() ?? "";
                var state = ctx?["state"]?.ToString() ?? "";
                if (conclusion is "FAILURE" or "ERROR" || state is "FAILURE" or "ERROR") failed++;
                else if (status is "IN_PROGRESS" or "QUEUED" or "PENDING" || state == "PENDING") pending++;
            }
            if (failed > 0) { h["ciStatus"] = "red"; h["ciFailedCount"] = failed; h["ciTotalCount"] = rollup.Count; }
            else if (pending > 0) h["ciStatus"] = "pending";
            else h["ciStatus"] = "green";
        }

        if (h["status"].ToString() == "healthy" && h["ciStatus"].ToString() == "red")
            h["status"] = "ci-red";

        fwdHealth[fpr.number] = h;
    }
    catch { }
}

// =============================================================================
// Phase 6: Build structured JSON output
// =============================================================================

string ShortSha(string? sha) => sha == null ? "(unknown)" : sha[..Math.Min(12, sha.Length)];

var backflowBranches = new List<Dictionary<string, object?>>();

// Open PR branches
foreach (var kv in openBranches.OrderBy(k => k.Key))
{
    if (branch != null && kv.Key != branch) continue;
    var entry = new Dictionary<string, object?>
    {
        ["branch"] = kv.Key,
        ["prNumber"] = kv.Value,
        ["prState"] = "open"
    };
    if (openPRHealth.TryGetValue(kv.Key, out var health))
    {
        entry["status"] = health["status"];
        entry["hasConflict"] = health["hasConflict"];
        entry["hasStaleness"] = health["hasStaleness"];
        entry["ciStatus"] = health["ciStatus"];
        if (health.ContainsKey("ciFailedCount")) { entry["ciFailedCount"] = health["ciFailedCount"]; entry["ciTotalCount"] = health["ciTotalCount"]; }
        if (health.ContainsKey("subscriptionId")) entry["subscriptionId"] = health["subscriptionId"];
        if (health.ContainsKey("vmrBranch")) entry["vmrBranch"] = health["vmrBranch"];
    }
    else entry["status"] = "unknown";
    backflowBranches.Add(entry);
}

// Branches without open PRs
foreach (var kv in branchLastMerged.OrderBy(k => k.Key))
{
    if (branch != null && kv.Key != branch) continue;
    if (openBranches.ContainsKey(kv.Key)) continue;
    var entry = new Dictionary<string, object?>
    {
        ["branch"] = kv.Key,
        ["lastMergedPR"] = kv.Value.number,
        ["lastMergedAt"] = kv.Value.closedAt
    };
    if (vmrBranches.TryGetValue(kv.Key, out var vmrBr)) entry["vmrBranch"] = vmrBr;
    if (vmrCommits.TryGetValue(kv.Key, out var vmrC)) entry["lastVmrCommit"] = ShortSha(vmrC);

    if (vmrHeads.TryGetValue(kv.Key, out var head))
    {
        var headSha = head["sha"]?.ToString() ?? "";
        entry["vmrHeadSha"] = ShortSha(headSha);
        entry["vmrHeadDate"] = head["commit"]?["committer"]?["date"]?.ToString();

        if (vmrCommits.TryGetValue(kv.Key, out var vc) && (headSha.StartsWith(vc) || vc.StartsWith(headSha) || headSha == vc))
        {
            entry["status"] = "up-to-date";
            entry["aheadBy"] = 0;
        }
        else
        {
            var aheadBy = compareResults.TryGetValue(kv.Key, out var cmp) ? cmp["ahead_by"]?.GetValue<int>() ?? -1 : -1;
            entry["aheadBy"] = aheadBy;

            var isReleasedPreview = (vmrBr?.Contains("preview") ?? false)
                && kv.Value.closedAt != null
                && (DateTime.UtcNow - DateTimeOffset.Parse(kv.Value.closedAt).UtcDateTime).TotalDays > ReleasedPreviewThresholdDays;

            if (isReleasedPreview)
                entry["status"] = "released-preview";
            else
            {
                entry["status"] = "missing";
                if (kv.Value.closedAt != null)
                    entry["hoursSinceLastMerge"] = Math.Round((DateTime.UtcNow - DateTimeOffset.Parse(kv.Value.closedAt).UtcDateTime).TotalHours, 1);
            }
        }
    }
    else entry["status"] = "unknown";
    backflowBranches.Add(entry);
}

var forwardFlowPRs = new List<Dictionary<string, object?>>();
foreach (var fpr in fwdPRs)
{
    if (branch != null && fpr.branch != branch) continue;
    var entry = new Dictionary<string, object?>
    {
        ["prNumber"] = fpr.number,
        ["branch"] = fpr.branch,
        ["title"] = fpr.title
    };
    if (fwdHealth.TryGetValue(fpr.number, out var fh))
    {
        entry["status"] = fh["status"];
        entry["hasConflict"] = fh["hasConflict"];
        entry["hasStaleness"] = fh["hasStaleness"];
        entry["ciStatus"] = fh["ciStatus"];
        if (fh.ContainsKey("ciFailedCount")) { entry["ciFailedCount"] = fh["ciFailedCount"]; entry["ciTotalCount"] = fh["ciTotalCount"]; }
    }
    else entry["status"] = "unknown";
    forwardFlowPRs.Add(entry);
}

// Summary counts
int healthy = backflowBranches.Count(b => b["status"]?.ToString() == "healthy");
int upToDate = backflowBranches.Count(b => b["status"]?.ToString() is "up-to-date" or "released-preview");
int blocked = backflowBranches.Count(b => b["status"]?.ToString() is "conflict" or "stale" or "ci-red");
int missing = backflowBranches.Count(b => b["status"]?.ToString() == "missing");

int fwdHealthy = forwardFlowPRs.Count(b => b["status"]?.ToString() == "healthy");
int fwdStale = forwardFlowPRs.Count(b => b["status"]?.ToString() == "stale");
int fwdConflict = forwardFlowPRs.Count(b => b["status"]?.ToString() == "conflict");
int fwdCiRed = forwardFlowPRs.Count(b => b["status"]?.ToString() == "ci-red");
int fwdIssues = fwdStale + fwdConflict + fwdCiRed;

var output = new Dictionary<string, object>
{
    ["repository"] = repository,
    ["backflow"] = new Dictionary<string, object>
    {
        ["branches"] = backflowBranches,
        ["summary"] = new Dictionary<string, int>
        {
            ["healthy"] = healthy,
            ["upToDate"] = upToDate,
            ["blocked"] = blocked,
            ["missing"] = missing
        }
    },
    ["forwardFlow"] = new Dictionary<string, object>
    {
        ["prs"] = forwardFlowPRs,
        ["summary"] = new Dictionary<string, int>
        {
            ["healthy"] = fwdHealthy,
            ["stale"] = fwdStale,
            ["conflicted"] = fwdConflict,
            ["ciRed"] = fwdCiRed
        }
    }
};

if (ghFailureCount > 0)
    output["ghFailures"] = ghFailureCount;

// Summary to stderr
int problemCount = blocked + missing;
if (problemCount == 0 && fwdIssues == 0)
    Console.Error.WriteLine($"✅ {repository}: {backflowBranches.Count} branches healthy, {forwardFlowPRs.Count} forward flow PRs");
else
    Console.Error.WriteLine($"⚠️ {repository}: {problemCount} backflow issues ({blocked} blocked, {missing} missing), {fwdIssues} forward flow issues ({fwdCiRed} ci-red, {fwdConflict} conflict, {fwdStale} stale)");

if (ghFailureCount > 0)
    Console.Error.WriteLine($"⚠️ {ghFailureCount} gh CLI call(s) failed — results may be incomplete");

// JSON to stdout
var options = new JsonSerializerOptions
{
    WriteIndented = true,
    TypeInfoResolver = new System.Text.Json.Serialization.Metadata.DefaultJsonTypeInfoResolver()
};
Console.WriteLine(JsonSerializer.Serialize(output, options));
return ghFailureCount > 0 ? 2 : 0;
