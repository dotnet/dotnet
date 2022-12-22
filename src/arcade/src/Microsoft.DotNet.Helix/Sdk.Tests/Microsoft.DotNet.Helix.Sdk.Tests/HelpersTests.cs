using System;
using System.IO;
using Xunit;
using System.Net;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Sdk.Tests
{
    public class HelpersTests
    {
        [Fact]
        public void VerifyEncodedChars()
        {
            var workItemNameWithEncodedChars = "work%20item%20name";

            var actual = Helpers.CleanWorkItemName(workItemNameWithEncodedChars);

            Assert.Equal(workItemNameWithEncodedChars, actual);
        }

        [Fact]
        public void VerifyNotEncodedChars()
        {
            var workItemName = "work item name";

            var actual = Helpers.CleanWorkItemName(workItemName);

            Assert.Equal(WebUtility.UrlDecode(workItemName), actual);
        }

        [Fact]
        public void VerifyNonEncodedFowardSlashIsConverted()
        {
            var workItemNameWithFowardSlash = "work/item/name";
            var workItemNameExpected = "work-item-name";

            var actual = Helpers.CleanWorkItemName(workItemNameWithFowardSlash);

            Assert.Equal(workItemNameExpected, actual);
        }

        [Fact]
        public void FailOnceThenPass()
        {
            string target = Path.Combine(Environment.GetEnvironmentVariable("HELIX_WORKITEM_ROOT") ?? Environment.GetEnvironmentVariable("TEMP"), "my-test-file-123456.snt");

            // If we're inside a Helix Docker work item, GetTempPath() is cleaned every execution, 
            // but the work item's own directory is not (and is writeable from inside Docker), so use it.
            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("HELIX_DOCKER_ENTRYPOINT")))
            {
                target = Path.Combine(Environment.GetEnvironmentVariable("HELIX_WORKITEM_PAYLOAD"), "my-test-file-123456.snt");
            }

            bool exists = File.Exists(target);
            if (!exists)
            {
                File.WriteAllText(target, "Test failed once");
                exists = File.Exists(target);
            }
            
            Assert.True(exists, $"File should exist: {target}");
        }
    }
}
