/* --------------------------------------------------------------------------------------------
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License. See License.txt in the project root for license information.
 * ------------------------------------------------------------------------------------------ */

const fetch = require('node-fetch');
const stream = require('stream');
const extract = require('extract-zip');
const fs = require('fs');
const util = require('util');
const path = require('path');
const os = require('os');

const finished = util.promisify(stream.finished);

const formatLog = (text) => `[${new Date()}] ${text}`;
const log = (text) => console.log(formatLog(text));
const logError = (text) => console.error(formatLog(text));

async function downloadProxyPackage(version) {
    const tmpDirectory = path.join(os.tmpdir(), 'blazorwasm-companion-tmp');
    if (!fs.existsSync(tmpDirectory)) {
        fs.mkdirSync(tmpDirectory);
    }

    // nuget.org requires the package name be lower-case
    const nugetUrl = 'https://api.nuget.org/v3-flatcontainer';
    const packageName = 'Microsoft.AspNetCore.Components.WebAssembly.DevServer'.toLowerCase();
    const versionedPackageName = `${packageName}.${version}.nupkg`;
    const downloadUrl = `${nugetUrl}/${packageName}/${version}/${versionedPackageName}`;

    // Download and save nupkg to disk
    log(`Fetching package from ${downloadUrl}...`);
    const response = await fetch(downloadUrl);

    if (!response.ok) {
        logError(`Failed to download ${downloadUrl}`);
        throw new Error(`Unable to download BlazorDebugProxy: ${response.status} ${response.statusText}`);
    }

    const downloadPath = path.join(tmpDirectory, versionedPackageName);
    const outputStream = fs.createWriteStream(downloadPath);
    response.body.pipe(outputStream);
    await finished(outputStream);

    // Extract nupkg to extraction directory
    log(`Extracting NuGet package with directory...`)
    const extractTarget = path.join(tmpDirectory, `extracted-${packageName}.${version}`);
    await extract(downloadPath, { dir: extractTarget });
    return extractTarget;
}

async function copyDebugProxyAssets(version) {
    const targetDirectory = path.join(__dirname, '..', 'BlazorDebugProxy', version);
    if (fs.existsSync(targetDirectory)) {
        log(`BlazorDebugProxy ${version} is already downloaded, nothing to do.`);
        return;
    }

    log(`Downloading BlazorDebugProxy ${version}...`);
    const extracted = await downloadProxyPackage(version);

    log(`Using ${targetDirectory} as targetDirectory...`);
    fs.mkdirSync(targetDirectory, { recursive: true });

    const srcDirectory = path.join(extracted, 'tools', 'BlazorDebugProxy');
    log(`Copying BlazorDebugProxy assets from ${srcDirectory} to ${targetDirectory}...`);
    fs.readdirSync(srcDirectory).forEach(function(file) {
        log(`Copying ${file} to target directory...`);
        fs.copyFileSync(path.join(srcDirectory, file), path.join(targetDirectory, file));
    });
}

const debugProxyVersion = require('../package.json').debugProxyVersion;
copyDebugProxyAssets(debugProxyVersion);
