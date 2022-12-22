using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.DotNet.Helix.AzureDevOps
{
    public class StartAzurePipelinesTestRun : AzureDevOpsTask
    {
        [Required]
        public string TestRunName { get; set; }

        public string JobName { get; set; }
        public int JobAttempt { get; set; }
        public string StageName { get; set; }
        public int StageAttempt { get; set; }
        public string PhaseName { get; set; }
        public int PhaseAttempt { get; set; }

        [Output]
        public int TestRunId { get; set; }
        
        protected override Task ExecuteCoreAsync(HttpClient client)
        {
            return RetryAsync(
                async () =>
                {
                    var req =
                        new HttpRequestMessage(
                            HttpMethod.Post,
                            $"{CollectionUri}{TeamProject}/_apis/test/runs?api-version=5.0")
                        {
                            Content = new StringContent(
                                JsonConvert.SerializeObject(
                                    new JObject
                                    {
                                        ["automated"] = true,
                                        ["build"] = new JObject {["id"] = BuildId,},
                                        ["name"] = TestRunName,
                                        ["state"] = "InProgress",
                                        ["pipelineReference"] = BuildPipelineReference(),
                                    }),
                                Encoding.UTF8,
                                "application/json"),
                        };
                    using (req)
                    {
                        using (var res = await client.SendAsync(req))
                        {
                            var result = await ParseResponseAsync(req, res);
                            if (result != null)
                            {
                                TestRunId = result["id"].ToObject<int>();
                            }
                        }
                    }
                });
        }

        private JObject BuildPipelineReference()
        {
            var obj = new JObject
            {
                {"jobReference", BuildReference("job", JobName, JobAttempt)},
                {"phaseReference", BuildReference("phase", PhaseName, PhaseAttempt)},
                {"stageReference", BuildReference("stage", StageName, StageAttempt)},
            };

            if (int.TryParse(BuildId, out var buildId))
            {
                obj["pipelineId"] = buildId;
            }

            return obj;
        }

        private JObject BuildReference(string part, string name, int attempt)
        {
            var reference = new JObject
            {
                [$"{part.ToLowerInvariant()}Name"] = name ?? GetEnvironmentVariable($"SYSTEM_{part.ToUpperInvariant()}NAME"),
            };

            if (attempt != 0)
            {
                reference["attempt"] = attempt;
            }
            else if (int.TryParse(GetEnvironmentVariable($"SYSTEM_{part.ToUpperInvariant()}ATTEMPT"), out int attemptFromEnv))
            {
                reference["attempt"] = attemptFromEnv;
            }

            return reference;
        }
    }
}
