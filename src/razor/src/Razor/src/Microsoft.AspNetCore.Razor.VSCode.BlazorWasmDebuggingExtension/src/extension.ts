/* --------------------------------------------------------------------------------------------
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License. See License.txt in the project root for license information.
 * ------------------------------------------------------------------------------------------ */

import { readFileSync } from 'fs';
import { join } from 'path';
import { fileURLToPath } from 'url';
import * as vscode from 'vscode';

export async function activate(context: vscode.ExtensionContext) {
    const launchDebugProxy = vscode.commands.registerCommand('blazorwasm-companion.launchDebugProxy', async (folder: vscode.WorkspaceFolder) => {
        const launchSettings = JSON.parse(readFileSync(join(fileURLToPath(folder.uri.toString()), 'Properties', 'launchSettings.json'), 'utf8'));
        if (launchSettings?.profiles && launchSettings?.profiles[Object.keys(launchSettings.profiles)[0]]?.inspectUri) {
            return {
                inspectUri: launchSettings.profiles[Object.keys(launchSettings.profiles)[0]].inspectUri,
            };
        }
        return {
            inspectUri: '{wsProtocol}://{url.hostname}:{url.port}/_framework/debug/ws-proxy?browser={browserInspectUri}',
        };
    });

    context.subscriptions.push(launchDebugProxy);
}
