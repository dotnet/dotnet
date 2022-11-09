// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using log4net;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
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

            var kvc = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(
                async (authority, resource, scope) => 
                {
                    var authContext = new AuthenticationContext(authority);
                    var clientCred = new ClientCredential(config.ClientId, config.ClientSecret);
                    var result = await authContext.AcquireTokenAsync(resource, clientCred);

                    if (result == null)
                        throw new InvalidOperationException("Failed to obtain the github token");

                    return result.AccessToken;
                }));

            SecretBundle secretBundle = await kvc.GetSecretAsync(config.SecretUri);
            config.Password = secretBundle.Value;

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
