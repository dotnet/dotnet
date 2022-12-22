using System.Threading.Tasks;
using NuGet.Commands;

namespace NuGet.CommandLine
{
    [Command(typeof(NuGetCommand), "delete", "DeleteCommandDescription",
        MinArgs = 2, MaxArgs = 3, UsageDescriptionResourceName = "DeleteCommandUsageDescription",
        UsageSummaryResourceName = "DeleteCommandUsageSummary", UsageExampleResourceName = "DeleteCommandUsageExamples")]
    public class DeleteCommand : Command
    {
        [Option(typeof(NuGetCommand), "DeleteCommandSourceDescription", AltName = "src")]
        public string Source { get; set; }

        [Option(typeof(NuGetCommand), "DeleteCommandNoPromptDescription", AltName = "np")]
        public bool NoPrompt { get; set; }

        [Option(typeof(NuGetCommand), "CommandApiKey")]
        public string ApiKey { get; set; }

        [Option(typeof(NuGetCommand), "CommandNoServiceEndpointDescription")]
        public bool NoServiceEndpoint { get; set; }

        public override async Task ExecuteCommandAsync()
        {
            if (NoPrompt)
            {
                Console.WriteWarning(LocalizedResourceManager.GetString("Warning_NoPromptDeprecated"));
                NonInteractive = true;
            }

            string packageId = Arguments[0];
            string packageVersion = Arguments[1];
            string apiKeyValue = null;

            if (!string.IsNullOrEmpty(ApiKey))
            {
                apiKeyValue = ApiKey;
            }
            else if (Arguments.Count > 2 && !string.IsNullOrEmpty(Arguments[2]))
            {
                apiKeyValue = Arguments[2];
            }

            await DeleteRunner.Run(
                Settings,
                SourceProvider,
                packageId,
                packageVersion,
                Source,
                apiKeyValue,
                NonInteractive,
                NoServiceEndpoint,
                Console.Confirm,
                Console);
        }
    }
}
