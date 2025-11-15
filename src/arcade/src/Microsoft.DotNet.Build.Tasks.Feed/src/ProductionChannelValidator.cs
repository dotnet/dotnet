// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.DotNet.Build.Tasks.Feed.Model;
using Microsoft.Extensions.Logging;

namespace Microsoft.DotNet.Build.Tasks.Feed
{
    public class ProductionChannelValidator : ITargetChannelValidator
    {
        private const string RequiredAzureDevOpsTag = "1ES.PT.Official";
        
        private readonly IProductionChannelValidatorBuildInfoService _productionChannelValidatorBuildInfoService;
        private readonly IBranchClassificationService _branchClassificationService;
        private readonly ILogger<ProductionChannelValidator> _logger;
        private readonly ValidationMode _validationMode;

        public ProductionChannelValidator(
            IProductionChannelValidatorBuildInfoService productionChannelValidatorBuildInfoService,
            IBranchClassificationService branchClassificationService,
            ILogger<ProductionChannelValidator> logger,
            ValidationMode validationMode = ValidationMode.Enforce)
        {
            _productionChannelValidatorBuildInfoService = productionChannelValidatorBuildInfoService ?? throw new ArgumentNullException(nameof(productionChannelValidatorBuildInfoService));
            _branchClassificationService = branchClassificationService ?? throw new ArgumentNullException(nameof(branchClassificationService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _validationMode = validationMode;
        }

        public async Task<TargetChannelValidationResult> ValidateAsync(ProductConstructionService.Client.Models.Build build, TargetChannelConfig targetChannel)
        {
            if (build == null)
            {
                throw new ArgumentNullException(nameof(build));
            }

            // If the target channel is not a production channel, no validation is needed
            if (!targetChannel.IsProduction)
            {
                _logger.LogInformation($"Target channel {targetChannel.Id} is not a production channel, skipping validation");
                return TargetChannelValidationResult.Success;
            }

            _logger.LogInformation($"Validating build {build.Id} for production channel {targetChannel.Id}");

            try
            {
                // Get build information from Azure DevOps which includes tags, project, and repository info
                var buildInfo = await GetAzureDevOpsBuildInfoAsync(build);
                if (buildInfo == null)
                {
                    return ApplyValidationMode(TargetChannelValidationResult.Fail);
                }

                // Step 1: Validate Azure DevOps build has the required tag - always return Fail if tags don't validate
                var tagValidationResult = ValidateAzureDevOpsTags(build, buildInfo.Tags);
                if (tagValidationResult != TargetChannelValidationResult.Success)
                {
                    return ApplyValidationMode(tagValidationResult);  // This will be Fail for missing/invalid tags
                }

                // Step 2: Now that tags are validated, check if this is an Azure DevOps repository
                if (!IsAzureDevOpsRepository(build.AzureDevOpsRepository))
                {
                    LogValidationFailure($"Build {build.Id} repository '{build.AzureDevOpsRepository}' is not an Azure DevOps repository.");
                    return ApplyValidationMode(TargetChannelValidationResult.Fail);
                }

                // Step 3: Validate that the branch is a production branch
                var branchValidationResult = await ValidateBranchIsProductionAsync(build, buildInfo);
                if (branchValidationResult != TargetChannelValidationResult.Success)
                {
                    return ApplyValidationMode(branchValidationResult);
                }

                _logger.LogInformation($"Build {build.Id} passed all production channel validations");
                return TargetChannelValidationResult.Success;
            }
            catch (Exception ex)
            {
                LogValidationFailure(ex, $"Error validating build {build.Id} for production channel {targetChannel.Id}");
                return ApplyValidationMode(TargetChannelValidationResult.Fail);
            }
        }

        /// <summary>
        /// Applies the validation mode to convert Fail results to AuditOnlyFailure when in Audit mode
        /// </summary>
        private TargetChannelValidationResult ApplyValidationMode(TargetChannelValidationResult result)
        {
            if (_validationMode == ValidationMode.Audit && result == TargetChannelValidationResult.Fail)
            {
                return TargetChannelValidationResult.AuditOnlyFailure;
            }
            return result;
        }

        /// <summary>
        /// Logs a validation failure message based on the current validation mode.
        /// In Audit mode, logs as a warning. In Enforce mode, logs as an error.
        /// </summary>
        private void LogValidationFailure(string message)
        {
            if (_validationMode == ValidationMode.Audit)
            {
                _logger.LogWarning(message);
            }
            else
            {
                _logger.LogError(message);
            }
        }

        /// <summary>
        /// Logs a validation failure message with exception based on the current validation mode.
        /// In Audit mode, logs as a warning. In Enforce mode, logs as an error.
        /// </summary>
        private void LogValidationFailure(Exception exception, string message)
        {
            if (_validationMode == ValidationMode.Audit)
            {
                _logger.LogWarning(exception, message);
            }
            else
            {
                _logger.LogError(exception, message);
            }
        }

        private static bool IsAzureDevOpsRepository(string repositoryUrl)
        {
            if (string.IsNullOrEmpty(repositoryUrl))
                return false;

            return repositoryUrl.Contains("dev.azure.com", StringComparison.OrdinalIgnoreCase) ||
                   repositoryUrl.Contains("visualstudio.com", StringComparison.OrdinalIgnoreCase);
        }

        private async Task<AzureDevOpsBuildInfo> GetAzureDevOpsBuildInfoAsync(ProductConstructionService.Client.Models.Build build)
        {
            // Extract Azure DevOps information from build
            var azureDevOpsAccount = build.AzureDevOpsAccount;
            var azureDevOpsProject = build.AzureDevOpsProject;
            var azureDevOpsBuildId = build.AzureDevOpsBuildId;

            if (string.IsNullOrEmpty(azureDevOpsAccount) || 
                string.IsNullOrEmpty(azureDevOpsProject) || 
                !azureDevOpsBuildId.HasValue)
            {
                LogValidationFailure($"Build {build.Id} missing Azure DevOps information for validation");
                return null;
            }

            // Don't catch exceptions here - let them bubble up to the main catch block
            // which will treat service communication failures as AuditOnlyFailure
            return await _productionChannelValidatorBuildInfoService.GetBuildInfoAsync(azureDevOpsAccount, azureDevOpsProject, azureDevOpsBuildId.Value);
        }

        private TargetChannelValidationResult ValidateAzureDevOpsTags(ProductConstructionService.Client.Models.Build build, IReadOnlyList<string> tags)
        {
            try
            {
                _logger.LogDebug($"Validating Azure DevOps tags for build {build.Id}");

                if (tags == null)
                {
                    LogValidationFailure($"Build {build.Id} has no tag information available");
                    return TargetChannelValidationResult.Fail;
                }

                bool hasRequiredTag = tags.Contains(RequiredAzureDevOpsTag, StringComparer.OrdinalIgnoreCase);
                
                if (hasRequiredTag)
                {
                    _logger.LogDebug($"Build {build.Id} has required tag '{RequiredAzureDevOpsTag}'");
                    return TargetChannelValidationResult.Success;
                }
                else
                {
                    LogValidationFailure($"Build {build.Id} does not have required tag '{RequiredAzureDevOpsTag}'. Found tags: {string.Join(", ", tags)}");
                    return TargetChannelValidationResult.Fail;
                }
            }
            catch (Exception ex)
            {
                LogValidationFailure(ex, $"Error validating Azure DevOps tags for build {build.Id}");
                return TargetChannelValidationResult.Fail;
            }
        }

        private async Task<TargetChannelValidationResult> ValidateBranchIsProductionAsync(ProductConstructionService.Client.Models.Build build, AzureDevOpsBuildInfo buildInfo)
        {
            // Extract repository and branch information
            var azureDevOpsAccount = build.AzureDevOpsAccount;
            var azureDevOpsBranch = build.AzureDevOpsBranch;

            if (string.IsNullOrEmpty(azureDevOpsAccount) ||
                string.IsNullOrEmpty(azureDevOpsBranch))
            {
                LogValidationFailure($"Build {build.Id} missing repository information for branch validation");
                return TargetChannelValidationResult.Fail;
            }

            string projectId = buildInfo?.Project?.Id;
            string repositoryId = buildInfo?.Repository?.Id;

            if (string.IsNullOrEmpty(projectId))
            {
                LogValidationFailure($"Build {build.Id}: Could not find project GUID from build info");
                return TargetChannelValidationResult.Fail;
            }

            if (string.IsNullOrEmpty(repositoryId))
            {
                LogValidationFailure($"Build {build.Id}: Could not find repository GUID from build info");
                return TargetChannelValidationResult.Fail;
            }

            // Normalize branch name (remove refs/heads/ prefix if present)
            var branchName = NormalizeBranchName(azureDevOpsBranch);

            _logger.LogDebug($"Checking branch classification for {azureDevOpsAccount}/{projectId}/{repositoryId}, branch: {branchName}");

            try
            {
                var branchClassifications = await _branchClassificationService.GetBranchClassificationsAsync(
                    azureDevOpsAccount, projectId, repositoryId);

                bool isProductionBranch = IsProductionBranch(branchName, branchClassifications);

                if (isProductionBranch)
                {
                    _logger.LogDebug($"Branch '{branchName}' is classified as a production branch");
                    return TargetChannelValidationResult.Success;
                }
                else
                {
                    LogValidationFailure($"Branch '{branchName}' is not classified as a production branch");
                    return TargetChannelValidationResult.Fail;
                }
            }
            catch (Exception ex)
            {
                // For branch classification service failures, always log as debug first
                _logger.LogDebug(ex, $"Failed to fetch branch classifications for build {build.Id}");
                
                // Then apply validation mode specific logging
                LogValidationFailure(ex, $"Error validating branch classification for build {build.Id}");
                return TargetChannelValidationResult.Fail;
            }
        }

        private static string NormalizeBranchName(string branchName)
        {
            if (string.IsNullOrEmpty(branchName))
                return branchName;

            // Remove refs/heads/ prefix if present
            if (branchName.StartsWith("refs/heads/", StringComparison.OrdinalIgnoreCase))
            {
                return branchName.Substring("refs/heads/".Length);
            }

            return branchName;
        }

        private static bool IsProductionBranch(string branchName, BranchClassificationResponse branchClassifications)
        {
            if (branchClassifications?.BranchClassifications == null)
                return false;

            foreach (var classification in branchClassifications.BranchClassifications)
            {
                if (classification.Classification.Equals("production", StringComparison.OrdinalIgnoreCase))
                {
                    if (MatchesBranchPattern(branchName, classification.BranchName))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool MatchesBranchPattern(string branchName, string pattern)
        {
            if (string.IsNullOrEmpty(branchName) || string.IsNullOrEmpty(pattern))
                return false;

            // Handle exact match
            if (pattern.Equals(branchName, StringComparison.OrdinalIgnoreCase))
                return true;

            // Handle wildcard patterns (e.g., "release/*")
            if (pattern.EndsWith("/*"))
            {
                var prefix = pattern.Substring(0, pattern.Length - 2);
                return branchName.StartsWith(prefix + "/", StringComparison.OrdinalIgnoreCase);
            }

            // Handle special ~default pattern
            if (pattern.Equals("~default", StringComparison.OrdinalIgnoreCase))
            {
                // This would require additional logic to determine the default branch
                // For now, treat common default branch names as matching
                return branchName.Equals("main", StringComparison.OrdinalIgnoreCase) ||
                       branchName.Equals("master", StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }
    }

    // Interface definitions for dependency injection and testability
    public interface IProductionChannelValidatorBuildInfoService
    {
        Task<AzureDevOpsBuildInfo> GetBuildInfoAsync(string account, string project, int buildId);
    }

    public interface IBranchClassificationService
    {
        Task<BranchClassificationResponse> GetBranchClassificationsAsync(string organizationName, string projectId, string repositoryId);
    }

    // Response models for Azure DevOps API
    public class AzureDevOpsBuildInfo
    {
        public AzureDevOpsProject Project { get; set; }
        public AzureDevOpsRepository Repository { get; set; }
        public IReadOnlyList<string> Tags { get; set; }
    }

    public class AzureDevOpsProject
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class AzureDevOpsRepository
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    // Response models for the branch classification API
    public class BranchClassificationResponse
    {
        public IReadOnlyList<BranchClassification> BranchClassifications { get; set; }
        public string Status { get; set; }
    }

    public class BranchClassification
    {
        public string BranchName { get; set; }
        public string Classification { get; set; }
    }

    // Implementation classes
    public class AzureDevOpsService : IProductionChannelValidatorBuildInfoService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AzureDevOpsService> _logger;
        private readonly string _token;

        public AzureDevOpsService(HttpClient httpClient, ILogger<AzureDevOpsService> logger, string token = null)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _token = token;
            
            // Configure authentication for Azure DevOps API using Basic authentication
            if (!string.IsNullOrEmpty(_token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(Encoding.ASCII.GetBytes($":{_token}")));
            }
        }

        public async Task<AzureDevOpsBuildInfo> GetBuildInfoAsync(string account, string project, int buildId)
        {
            try
            {
                var url = $"https://dev.azure.com/{account}/{project}/_apis/build/builds/{buildId}?api-version=6.0";
                _logger.LogDebug($"Fetching build info from: {url}");

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var buildResponse = JsonSerializer.Deserialize<AzureDevOpsBuildInfoResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return new AzureDevOpsBuildInfo
                {
                    Project = buildResponse?.Project,
                    Repository = buildResponse?.Repository,
                    Tags = buildResponse?.Tags ?? new List<string>()
                };
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, $"Error fetching build info for {account}/{project}/{buildId}");
                throw;
            }
        }

        private class AzureDevOpsBuildInfoResponse
        {
            public AzureDevOpsProject Project { get; set; }
            public AzureDevOpsRepository Repository { get; set; }
            public List<string> Tags { get; set; }
        }
    }

    public class BranchClassificationService : IBranchClassificationService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BranchClassificationService> _logger;
        private readonly string _token;

        public BranchClassificationService(HttpClient httpClient, ILogger<BranchClassificationService> logger, string token = null)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _token = token;
        }

        public async Task<BranchClassificationResponse> GetBranchClassificationsAsync(string organizationName, string projectId, string repositoryId)
        {
            try
            {
                var url = string.Format(CultureInfo.InvariantCulture, 
                    "https://BranchClassification.app.prod.gitops.startclean.microsoft.com/api/getBranchClassifications/{0}/{1}/{2}", 
                    organizationName, projectId, repositoryId);
                _logger.LogDebug($"Fetching branch classifications from: {url}");

                using var request = new HttpRequestMessage(HttpMethod.Get, url);
                
                // Add Bearer token authentication for branch classification service
                if (!string.IsNullOrEmpty(_token))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                }

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var classification = JsonSerializer.Deserialize<BranchClassificationResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return classification;
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, $"Error fetching branch classifications for {organizationName}/{projectId}/{repositoryId}");
                throw;
            }
        }
    }
}
