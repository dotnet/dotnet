// <copyright file="GlobalSuppressions.cs" company="Microsoft">
// Copyright © Microsoft. All Rights Reserved.
// </copyright>
//
// To add a suppression to this file, right-click the message in the 
// Code Analysis results, point to "Suppress Message", and click "In Suppression File".
// You may also add suppressions to this file manually.

#pragma warning disable SA1025 // Code must not contain multiple whitespace in a row

using System.Diagnostics.CodeAnalysis;

[assembly:SuppressMessage(

            category:       "Microsoft.Usage",
            checkId:        "CA2213: Disposable Fields Should Be Disposed",
            MessageId =     "startRunnerEvent",
            Scope =         "member",
            Target =        "Microsoft.ApplicationInsights.Channel.InMemoryTransmitter.#Dispose(System.Boolean)", 
            Justification = "startRunnerEvent is being disposed.")]

[assembly:SuppressMessage(
            category:       "Microsoft.Usage",
            checkId:        "CA2213: Disposable Fields Should Be Disposed", 
            MessageId =     "telemetryChannel", 
            Scope =         "member", 
            Target =        "Microsoft.ApplicationInsights.Extensibility.TelemetrySink.#Dispose()", 
            Justification = "telemetryChannel is being disposed using ?. syntax")]

#pragma warning restore SA1025 // Code must not contain multiple whitespace in a row