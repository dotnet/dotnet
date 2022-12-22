// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using log4net;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Microsoft.DotNet.GitSync
{
    internal class ConfigFile
    {
        private readonly string _path;
        private readonly ILog _logger;

        public ConfigFile(string path, ILog logger)
        {
            _path = path;
            _logger = logger;
        }

        public async Task<Configuration> GetAsync()
        {
            if (!File.Exists(_path))
            {
                return null;
            }
            var config = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(_path), new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                PreserveReferencesHandling = PreserveReferencesHandling.All,
            });

            // This app originally took fully qualified secret values; we'll split it up using Uri and string APIs to get the same effect.
            Uri vaultUri = new Uri(config.SecretUri);

            string secretName = vaultUri.AbsolutePath.Replace("/secrets/", "");
            if (secretName.IndexOf('/') > 0) // If the secret includes a version, lop it off so we fetch 'latest'
            {
                secretName = secretName.Substring(0, secretName.IndexOf("/"));
            }

            SecretClient client = new SecretClient(new Uri(vaultUri.GetLeftPart(UriPartial.Authority)),
                new ClientSecretCredential("72f988bf-86f1-41af-91ab-2d7cd011db47", config.ClientId, config.ClientSecret));

            KeyVaultSecret secret = await client.GetSecretAsync(secretName);
            config.Password = secret.Value;
            return config;
        }

        public void Save(Configuration config)
        {
            File.WriteAllText(_path, JsonConvert.SerializeObject(config,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                    PreserveReferencesHandling = PreserveReferencesHandling.All
                }));
            _logger.Info("Configuration file updated");
        }
    }
}
