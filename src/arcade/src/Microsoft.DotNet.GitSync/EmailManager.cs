// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CredentialManagement;
using log4net;
using System;
using System.Net;
using System.Net.Mail;

namespace Microsoft.DotNet.GitSync
{
    internal class EmailManager
    {
        private string _sender;
        private string _destinations;
        private SmtpClient _client;
        private ILog _logger;

        public EmailManager(string server, string destinations, ILog logger)
        {
            _logger = logger;
            using (var cred = new Credential())
            {
                cred.Target = "email";
                if (cred.Exists() && !string.IsNullOrEmpty(server) && !string.IsNullOrEmpty(destinations))
                {
                    cred.Load();
                    _client = new SmtpClient(server, 587);
                    _sender = cred.Username;
                    _client.Credentials = new NetworkCredential(cred.Username, cred.Password);
                    _client.EnableSsl = true;
                    _destinations = destinations;
                    _logger.Info("Email Manager is configured correctly");
                }
                else
                {
                    _logger.Info("Email Manager is not able to find the correct configuration to use");
                }
            }
        }
       
        public void Email(string subject, string body)
        {
            try
            {
                if (_client != null)
                {
                    _client.Send(_sender, _destinations, subject, body);
                    _logger.Info("email send");
                }
                else
                {
                    _logger.Info("Email Manager is not able to find the correct configuration to use");
                }
            }
            catch (Exception ex)
            {
                _logger.Info("Sending email failed due to " + ex.Message);
            }
        }
    }
}
