// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

import { Blazor } from './GlobalExports';
import { shouldAutoStart } from './BootCommon';
import { CircuitStartOptions } from './Platform/Circuits/CircuitStartOptions';
import { setCircuitOptions, startCircuit } from './Boot.Server.Common';
import { ServerComponentDescriptor, discoverComponents } from './Services/ComponentDescriptorDiscovery';
import { DotNet } from '@microsoft/dotnet-js-interop';
import { InitialRootComponentsList } from './Services/InitialRootComponentsList';

let started = false;

function boot(userOptions?: Partial<CircuitStartOptions>): Promise<void> {
  if (started) {
    throw new Error('Blazor has already started.');
  }
  started = true;

  setCircuitOptions(userOptions);

  const serverComponents = discoverComponents(document, 'server') as ServerComponentDescriptor[];
  const components = new InitialRootComponentsList(serverComponents);
  return startCircuit(components);
}

Blazor.start = boot;
window['DotNet'] = DotNet;

if (shouldAutoStart()) {
  boot();
}
