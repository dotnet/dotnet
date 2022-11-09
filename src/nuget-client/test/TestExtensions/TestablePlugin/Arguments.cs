// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace NuGet.Test.TestExtensions.TestablePlugin
{
    internal sealed class Arguments
    {
        internal bool CauseProtocolException { get; private set; }
        internal bool Freeze { get; private set; }
        internal ushort PortNumber { get; private set; }
        internal int TestRunnerProcessId { get; private set; }
        internal ThrowException ThrowException { get; private set; }

        private Arguments() { }

        internal static bool TryParse(IReadOnlyList<string> args, out Arguments arguments)
        {
            arguments = null;

            var causeProtocolException = false;
            var freezeEnabled = false;
            ushort portNumber = 0;
            var testRunnerProcessId = 0;
            var isPlugin = false;
            var throwException = ThrowException.None;

            for (var i = 0; i < args.Count; ++i)
            {
                var flag = args[i];

                switch (flag.ToLower())
                {
                    case "-causeprotocolexception":
                        causeProtocolException = true;
                        break;

                    case "-freeze":
                        freezeEnabled = true;
                        break;

                    case "-portnumber":
                        if (i + 1 == args.Count)
                        {
                            return false;
                        }

                        ++i;

                        if (!ushort.TryParse(args[i], out portNumber))
                        {
                            return false;
                        }
                        break;

                    case "-plugin":
                        isPlugin = true;
                        break;

                    case "-testrunnerprocessid":
                        if (i + 1 == args.Count)
                        {
                            return false;
                        }

                        ++i;

                        if (!int.TryParse(args[i], out testRunnerProcessId))
                        {
                            return false;
                        }
                        break;

                    case "-throwexception":
                        if (i + 1 == args.Count)
                        {
                            return false;
                        }

                        ++i;

                        if (!Enum.TryParse(args[i], ignoreCase: true, out throwException))
                        {
                            return false;
                        }
                        break;

                    default:
                        return false;
                }
            }

            if (!isPlugin)
            {
                return false;
            }

            arguments = new Arguments()
            {
                CauseProtocolException = causeProtocolException,
                Freeze = freezeEnabled,
                PortNumber = portNumber,
                TestRunnerProcessId = testRunnerProcessId,
                ThrowException = throwException
            };

            return true;
        }
    }
}
