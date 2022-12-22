// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace NuGet.Protocol.Plugins
{
    internal sealed class AssemblyLogMessage : PluginLogMessage
    {
        private readonly string _fileVersion;
        private readonly string _fullName;
        private readonly string _informationalVersion;
        private readonly string _entryAssemblyFullName;

        internal AssemblyLogMessage(DateTimeOffset now)
            : base(now)
        {
            var assembly = typeof(PluginFactory).Assembly;
            var entryAssembly = Assembly.GetEntryAssembly();
            var informationalVersionAttribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            var fileVersionAttribute = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>();

            _fullName = assembly.FullName;
            _entryAssemblyFullName = entryAssembly?.FullName;

            if (fileVersionAttribute != null)
            {
                _fileVersion = fileVersionAttribute.Version;
            }

            if (informationalVersionAttribute != null)
            {
                _informationalVersion = informationalVersionAttribute.InformationalVersion;
            }
        }

        public override string ToString()
        {
            var message = new JObject(
                new JProperty("assembly full name", _fullName),
                new JProperty("entry assembly full name", _entryAssemblyFullName));

            if (!string.IsNullOrEmpty(_fileVersion))
            {
                message.Add("file version", _fileVersion);
            }

            if (!string.IsNullOrEmpty(_informationalVersion))
            {
                message.Add("informational version", _informationalVersion);
            }

            return ToString("assembly", message);
        }
    }
}
