// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.
#if NET20

[assembly: System.Runtime.Versioning.TargetFramework(".NETFramework,Version=v2.0")]

#else

[assembly: System.Security.AllowPartiallyTrustedCallers]

#endif
