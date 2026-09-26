#!/usr/bin/env -S dotnet run
#:package Microsoft.DotNet.ProductConstructionService.Client@1.1.0-beta.26063.2
#:package Azure.Identity@1.17.1
#:package Octokit@13.0.1

using Azure.Core;
using Azure.Identity;
using Microsoft.DotNet.ProductConstructionService.Client;
using Octokit;

const int ChannelIdNet10 = 5173;
const int ChannelIdNet11 = 8298;

var channelId = (args.Length > 0 ? args[0] : "") switch
{
    "net10" => ChannelIdNet10,
    "net11" => ChannelIdNet11,
    _ => ChannelIdNet11,
};

var channelName = channelId == ChannelIdNet10 ? "net10" : "net11";
Console.WriteLine($"[log] Using channel: {channelName} (id={channelId})");

Console.WriteLine("[log] Loading cached PR info...");
var prInfoMap = LoadPullRequestInfo();
Console.WriteLine($"[log] Loaded {prInfoMap.Count} cached PR entries");

Console.WriteLine("[log] Resolving GitHub token via `gh auth token`...");
var psi = new System.Diagnostics.ProcessStartInfo("gh", "auth token")
{
    RedirectStandardOutput = true,
    UseShellExecute = false
};
var proc = System.Diagnostics.Process.Start(psi);
if (proc is null)
    throw new Exception(
        "Could not start `gh`. Install the GitHub CLI and then run:\n" +
        "  gh auth login");
var gitHubToken = (await proc.StandardOutput.ReadToEndAsync()).Trim();
await proc.WaitForExitAsync();
if (proc.ExitCode != 0 || string.IsNullOrEmpty(gitHubToken))
    throw new Exception(
        "GitHub authentication failed. Log in by running:\n" +
        "  gh auth login");
Console.WriteLine("[log] GitHub token resolved");

// Create GitHub client with the token
var githubClient = new GitHubClient(new ProductHeaderValue("fetch-prs"))
{
    Credentials = new Credentials(gitHubToken)
};

Console.WriteLine("[log] Creating PCS client...");
var vmrClient = CreatePcsClient();

try
{
    Console.WriteLine($"[log] Fetching subscriptions for channel {channelId}...");
    IReadOnlyList<Microsoft.DotNet.ProductConstructionService.Client.Models.Subscription> subscriptions;
    try
    {
        subscriptions = await vmrClient.Subscriptions.ListSubscriptionsAsync(channelId: channelId);
    }
    catch (Azure.Identity.CredentialUnavailableException)
    {
        throw new Exception(
            "Azure CLI authentication failed. Log in to the Microsoft tenant by running:\n" +
            "  az login --tenant 72f988bf-86f1-41af-91ab-2d7cd011db47");
    }
    Console.WriteLine("[log] PCS authentication successful");
    var backflowSubs = subscriptions.Where(x => x.IsBackflow()).ToList();
    Console.WriteLine($"[log] Found {subscriptions.Count} subscriptions, {backflowSubs.Count} are backflow");
    var list = new List<(string Owner, string Repository, int PullRequestId, string Url, DateTimeOffset CreationTime)>();
    Console.Write("Fetching backflow PRs ");
    var last = DateTimeOffset.UtcNow;
    var processed = 0;
    foreach (var subscription in backflowSubs)
    {
        try
        {
            processed++;
            Console.WriteLine($"[log] [{processed}/{backflowSubs.Count}] Checking subscription {subscription.Id} ({subscription.SourceRepository} -> {subscription.TargetRepository})...");
            var pr = await vmrClient.PullRequest.GetTrackedPullRequestBySubscriptionIdAsync(subscription.Id.ToString());
            var (owner, repo, prId) = SplitGitHubPullRequestUrl(pr.Url);
            Console.WriteLine($"[log]   Found PR #{prId} in {owner}/{repo}");
            var cached = prInfoMap.ContainsKey($"{owner}/{repo}/pull/{prId}");
            var creationTime = await GetPullRequestCreationTime(owner, repo, prId);
            if (!cached) Console.WriteLine($"[log]   Fetched creation time from GitHub: {creationTime:u}");
            list.Add((owner, repo, prId, pr.Url, creationTime));
        }
        catch (RestApiException ex) when (ex.Response.Status == 404)
        {
            Console.WriteLine($"[log]   No tracked PR found (404), skipping");
            continue;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\tError: {ex.Message}");
        }

        if ((DateTimeOffset.UtcNow - last).TotalSeconds > 10)
        {
            Console.Write(".");
            last = DateTimeOffset.UtcNow;
        }
    }
    Console.WriteLine();
    Console.WriteLine($"[log] Found {list.Count} active backflow PRs");

    var now = DateTimeOffset.UtcNow;
    Console.WriteLine($"{"Pull Request",-50} | Age (days)");
    foreach (var tuple in list.OrderBy(x => x.CreationTime))
    {
        Console.Write($"{tuple.Url,-50} | ");
        var color = Console.ForegroundColor;
        var diff = (now - tuple.CreationTime).TotalDays;
        Console.ForegroundColor = (now - tuple.CreationTime).TotalDays switch
        {
            < 5 => ConsoleColor.Green,
            < 10 => ConsoleColor.Yellow,
            _ => ConsoleColor.Red,
        };
        Console.WriteLine($"{diff:0.##}");
        Console.ForegroundColor = color;
    }
}
finally
{
    Console.WriteLine($"[log] Saving {prInfoMap.Count} cached PR entries...");
    SavePullRequestInfo(prInfoMap);
    Console.WriteLine("[log] Done");
}

static string GetFilePathPullRequestInfo() =>
    Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "fetch-prs",
        "pr-info.txt");

Dictionary<string, DateTimeOffset> LoadPullRequestInfo()
{
    var map = new Dictionary<string, DateTimeOffset>();
    var filePath = GetFilePathPullRequestInfo();
    if (!File.Exists(filePath))
    {
        return map;
    }

    var lines = File.ReadAllLines(filePath);
    foreach (var line in lines)
    {
        var parts = line.Split('@', StringSplitOptions.RemoveEmptyEntries);
        map[parts[0]] = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(parts[1]));
    }

    return map;
}

async Task<DateTimeOffset> GetPullRequestCreationTime(string owner, string repo, int prId)
{
    var key = $"{owner}/{repo}/pull/{prId}";
    if (prInfoMap.TryGetValue(key, out var creationTime))
    {
        return creationTime;
    }

    var pr = await githubClient.PullRequest.Get(owner, repo, prId);
    var createdAt = pr.CreatedAt;
    prInfoMap[key] = createdAt;
    return createdAt;
}

(string Owner, string Repository, int PrId) SplitGitHubPullRequestUrl(string prUrl)
{
    // GitHub PR URL format: https://github.com/{owner}/{repo}/pull/{id}
    var uri = new Uri(prUrl);
    var segments = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);

    if (segments.Length < 4 || segments[2] != "pull")
    {
        throw new ArgumentException($"Invalid GitHub PR URL format: {prUrl}");
    }

    var owner = segments[0];
    var repository = segments[1];
    var prId = int.Parse(segments[3]);

    return (owner, repository, prId);
}

void SavePullRequestInfo(Dictionary<string, DateTimeOffset> prInfoMap)
{
    var lines = new List<string>();
    foreach (var kvp in prInfoMap)
    {
        lines.Add($"{kvp.Key}@{kvp.Value.ToUnixTimeMilliseconds()}");
    }
    var filePath = GetFilePathPullRequestInfo();
    _ = Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
    File.WriteAllLines(filePath, lines);
}

IProductConstructionServiceApi CreatePcsClient()
{
    // Reuse the az cli login session (same tenant darc uses)
    var inner = new AzureCliCredential(new AzureCliCredentialOptions
    {
        TenantId = "72f988bf-86f1-41af-91ab-2d7cd011db47",
    });
    // The PCS client sends multiple scopes, but AzureCliCredential
    // only supports a single scope. Wrap to pick just the first scope.
    var credential = new SingleScopeCredential(inner);

    return new ProductConstructionServiceApi(
        new ProductConstructionServiceApiOptions(
            new Uri("https://maestro.dot.net/"), credential));
}

class SingleScopeCredential : TokenCredential
{
    private readonly TokenCredential _inner;
    public SingleScopeCredential(TokenCredential inner) => _inner = inner;

    // The Maestro app ID scope for production
    private const string MaestroScope = "54c17f3d-7325-4eca-9db7-f090bfc765a8/.default";

    private static TokenRequestContext FixContext(TokenRequestContext ctx)
    {
        if (ctx.Scopes.Length == 1) return ctx;
        // PCS client sends 0 scopes; inject the correct Maestro scope
        var scope = ctx.Scopes.Length == 0
            ? MaestroScope
            : ctx.Scopes.FirstOrDefault(s => s.EndsWith("/.default")) ?? ctx.Scopes[0];
        return new TokenRequestContext([scope], ctx.ParentRequestId, ctx.Claims, ctx.TenantId, ctx.IsCaeEnabled);
    }

    public override Azure.Core.AccessToken GetToken(TokenRequestContext ctx, CancellationToken ct)
        => _inner.GetToken(FixContext(ctx), ct);

    public override ValueTask<Azure.Core.AccessToken> GetTokenAsync(TokenRequestContext ctx, CancellationToken ct)
        => _inner.GetTokenAsync(FixContext(ctx), ct);
}


    /*
var trackedPrs = await client.PullRequest.GetTrackedPullRequestsAsync();
foreach (var pr in trackedPrs.Where(pr => pr.Channel?.Id == ChannelIdNet11))
{
    var sourceRepo = pr.Updates.FirstOrDefault()?.SourceRepository;
    Console.WriteLine($"PR {pr.Id} in {sourceRepo} with {pr.TargetBranch}");
}
*/