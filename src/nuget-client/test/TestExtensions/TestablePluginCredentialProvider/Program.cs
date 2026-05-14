// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace NuGet.Test.TestExtensions.TestablePluginCredentialProvider
{
    class Program
    {
        static int Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            if (!VerifyInput(args))
            {
                //We were not passed the correct input arguments so exit with an error.
                return -1;
            }

            var responseDelaySeconds = Environment.GetEnvironmentVariable(TestCredentialResponse.ResponseDelaySeconds);
            var responseShouldThrow = Environment.GetEnvironmentVariable(TestCredentialResponse.ResponseShouldThrow);
            var responseAbortMessage = Environment.GetEnvironmentVariable(TestCredentialResponse.ResponseMessage);
            var responseExitCode = Environment.GetEnvironmentVariable(TestCredentialResponse.ResponseExitCode);
            var responseUsername = Environment.GetEnvironmentVariable(TestCredentialResponse.ResponseUserName);
            var responsePassword = Environment.GetEnvironmentVariable(TestCredentialResponse.ResponsePassword);

            System.Threading.Thread.Sleep(ToInt(responseDelaySeconds) * 1000);

            if (ToBool(responseShouldThrow))
            {
                throw new ApplicationException("Throwing test exception");
            }

            dynamic response = new TestCredentialResponse
            {
                Message = responseAbortMessage,
                Password = responsePassword,
                Username = responseUsername
            };

            Console.WriteLine(JsonConvert.SerializeObject(response));

            return ToInt(responseExitCode);
        }

        private static bool VerifyInput(IReadOnlyList<string> args)
        {
            var isValid = true;

            for (var i = 0; i < args.Count && isValid; i++)
            {
                var flag = args[i];
                switch (flag.ToLower())
                {
                    case "-uri":
                        if (i + 1 == args.Count)
                        {
                            // We have a case were we can't grab 2 items so we either have a flag
                            // without a value or a space in the value either way call this an error
                            isValid = false;
                            break;
                        }
                        var value = args[++i];
                        if (string.IsNullOrWhiteSpace(value))
                        {
                            isValid = false;
                        }
                        break;
                    case "-isretry":
                        break;
                    case "-noninteractive":
                        break;
                    default:
                        isValid = false;
                        break;
                }
            }
            return isValid;
        }

        private static bool ToBool(string s)
        {
            bool b;
            bool.TryParse(s, out b);
            return b;
        }

        private static int ToInt(string s)
        {
            int i;
            int.TryParse(s, out i);
            return i;
        }
    }
}
