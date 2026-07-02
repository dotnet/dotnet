// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// Brings the Target enum members (Both, NetFx, Net) into scope unqualified so [TestMatrix] call sites
// read as e.g. [TestMatrix(console: NetFx, testHost: Net)]. The console:/testHost: parameter names
// disambiguate which axis each value applies to.
global using static Microsoft.TestPlatform.TestUtilities.Target;
