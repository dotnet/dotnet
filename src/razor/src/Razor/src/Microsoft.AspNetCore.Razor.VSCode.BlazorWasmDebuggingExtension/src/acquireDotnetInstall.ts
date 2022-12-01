/* --------------------------------------------------------------------------------------------
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License. See License.txt in the project root for license information.
 * ------------------------------------------------------------------------------------------ */

import { commands, extensions, OutputChannel } from 'vscode';

interface IDotnetAcquireResult {
    dotnetPath: string;
}

export async function acquireDotnetInstall(outputChannel: OutputChannel): Promise<string> {
    const extension = extensions.getExtension('ms-dotnettools.blazorwasm-companion');
    const requestingExtensionId = 'blazorwasm-companion';

    const version: string = extension && extension.packageJSON ? extension.packageJSON.dotnetRuntimeVersion : '6.0';
    if (version.split('.').length !== 2) {
        throw new Error('Version should be a valid major.minor version (the latest patch will automatically be selected).');
    }

    try {
        const dotnetResult = await commands.executeCommand<IDotnetAcquireResult>('dotnet.acquire', { version, requestingExtensionId });
        const dotnetPath = dotnetResult?.dotnetPath;
        if (!dotnetPath) {
            throw new Error('Install step returned an undefined path.');
        }
        await commands.executeCommand('dotnet.ensureDotnetDependencies', { command: dotnetPath, arguments: ['--info'] });
        return dotnetPath;
    } catch (error) {
        outputChannel.appendLine(`This extension requires .NET Core to run but we were unable to install it due to the following error:`);
        outputChannel.appendLine((error as Error).message);
        throw error;
    }
}
