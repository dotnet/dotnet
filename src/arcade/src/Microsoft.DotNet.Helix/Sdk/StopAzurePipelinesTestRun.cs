using Microsoft.Build.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.DotNet.Helix.AzureDevOps
{
    public class StopAzurePipelinesTestRun : AzureDevOpsTask
    {
        [Required]
        public int TestRunId { get; set; }

        protected override async Task ExecuteCoreAsync(HttpClient client)
        {
            await RetryAsync(
                async () =>
                {
                    using (var req =
                        new HttpRequestMessage(
                            new HttpMethod("PATCH"),
                            $"{CollectionUri}{TeamProject}/_apis/test/runs/{TestRunId}?api-version=5.0")
                        {
                            Content = new StringContent(
                                JsonConvert.SerializeObject(new JObject { ["state"] = "Completed", }),
                                Encoding.UTF8,
                                "application/json"),
                        })
                    {
                        using (var res = await client.SendAsync(req))
                        {
                            res.EnsureSuccessStatusCode();
                        }
                    }
                });
        }
    }
}
