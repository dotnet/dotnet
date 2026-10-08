// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// Marks this assembly as one whose process performs the connection identity check.
//
// The alternative is to infer the capability from the product version, which means predicting the
// patch number a servicing build will ship as. Every consumer of these packages would then carry that
// prediction, so a version that slips, or ships as a different patch, makes every already published
// package wrong and forces all of them to be published again. This marker is a fact about the build it
// is compiled into, so it cannot be wrong and never needs updating.
//
// AssemblyMetadata is used rather than a purpose-made attribute so that nothing has to be compiled
// into the marked assembly beyond this one line, which is the same shape Microsoft.Testing.Platform
// uses to mark its applications.
[assembly: System.Reflection.AssemblyMetadata("Microsoft.TestPlatform.ConnectionSecrets", "true")]
