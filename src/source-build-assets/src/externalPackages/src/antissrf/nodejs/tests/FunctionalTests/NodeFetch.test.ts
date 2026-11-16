// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

/**
 * Library: https://github.com/node-fetch/node-fetch/tree/2.x#readme
 *
 * Description: The node-fetch library provides a window.fetch compatible API for making
 * HTTP requests in Node.js. It only accepts absolute URLs, without any support for
 * relative URLs or protocol-relative URLs. It allows for a custom agent function to
 * choose the agent for the correct protocol automatically.
 */

import assert from "assert";
import http, { createServer, Server } from "http";
import https from "https";
import { promises } from "dns";
const fetch = require("node-fetch").default;

import { AntiSSRFPolicy, PolicyConfigOptions } from "../../src";

const allowedAddressesNoRedirect = ["https://github.com/"];

const allowedAddressesWithRedirect = [
    "https://google.com/",
    "http://localhost:3000/?redirectTo=https://github.com/",
    "http://localhost:3000/?redirectTo=https://google.com/"
];

const deniedAddressesNoRedirect = [
    "https://apple.com/",
    "https://www.facebook.com",
    "https://www.bing.com",
    "https://outlook.live.com",
    "https://www.etsy.com",
    "https://169.254.169.254/"
];

const deniedAddressesWithRedirect = [
    "http://localhost:3000/?redirectTo=https://apple.com/",
    "http://localhost:3000/?redirectTo=https://www.facebook.com",
    "http://localhost:3000/?redirectTo=https://www.bing.com",
    "http://localhost:3000/?redirectTo=https://outlook.live.com",
    "http://localhost:3000/?redirectTo=https://www.etsy.com",
    "http://localhost:3000/?redirectTo=https://169.254.169.254/"
];

describe("Node-Fetch Tests", () => {
    let server: Server;
    let httpAgent: http.Agent;
    let httpsAgent: https.Agent;
    let agentFn: (parsedURL: URL) => http.Agent | https.Agent;

    /**
     * Set up policy:
     * - Add XFF header
     * - Require header "test-required-header"
     * - Deny header "test-denied-header"
     * - Deny all unspecified addresses
     * - Allow addresses from GitHub, Google
     *
     * - Allow plain text HTTP required for local HTTP server
     * - Allow localhost required for local HTTP server
     */
    before(async () => {
        // Set up policy
        const policy = new AntiSSRFPolicy(PolicyConfigOptions.None);
        policy.allowPlainTextHttp = true;
        policy.addXFFHeader = true;
        policy.addRequiredHeaders(["test-required-header"]);
        policy.addDeniedHeaders(["test-denied-header"]);
        policy.denyAllUnspecifiedIPs = true;

        // Set up allowed IPs
        const githubIPs = await promises.lookup("github.com", { family: 0, all: true });
        const googleIPs = await promises.lookup("google.com", { family: 0, all: true });
        const moregoogleIPs = await promises.lookup("www.google.com", { family: 0, all: true });
        policy.addAllowedAddresses([...githubIPs, ...googleIPs, ...moregoogleIPs].map((ip) => ip.address));
        policy.addAllowedAddresses(["::1", "127.0.0.1"]);

        // Set up HTTP server to redirect requests
        server = createServer((req, res) => {
            const { url } = req;
            // Ensure the request has the XFF header
            assert.equal(req.headers["x-forwarded-for"], "true");
            res.writeHead(301, { Location: url?.substring(url.indexOf("=") + 1) });
            res.end();
        });
        server.listen(3000);

        // Get agents from policy
        httpAgent = policy.getHttpAgent({ keepAlive: false });
        httpsAgent = policy.getHttpsAgent({ keepAlive: false });
        agentFn = (_parsedURL) => {
            return _parsedURL.protocol === "https:" ? httpsAgent : httpAgent;
        };
    });

    describe("Allowed addresses that don't cause redirects", () => {
        allowedAddressesNoRedirect.forEach((url) => {
            it(`GET ${url} - follow = 0`, async () => {
                try {
                    const res = await fetch(url, {
                        follow: 0,
                        headers: { "test-required-header": "true" },
                        agent: agentFn
                    });
                    assert.ok(res.status === 200);
                } catch (err) {
                    assert.fail(err as Error);
                }
            });

            it(`GET ${url} - redirect = "manual"`, async () => {
                try {
                    const res = await fetch(url, {
                        redirect: "manual",
                        headers: { "test-required-header": "true" },
                        agent: agentFn
                    });
                    assert.ok(res.status === 200);
                } catch (err) {
                    assert.fail(err as Error);
                }
            });

            it(`GET ${url} - redirect = "follow"`, async () => {
                try {
                    const res = await fetch(url, {
                        redirect: "follow",
                        headers: { "test-required-header": "true" },
                        agent: agentFn
                    });
                    assert.ok(res.status === 200);
                } catch (err) {
                    assert.fail(err as Error);
                }
            });

            it(`GET ${url} - redirect = "error"`, async () => {
                try {
                    const res = await fetch(url, {
                        redirect: "error",
                        headers: { "test-required-header": "true" },
                        agent: agentFn
                    });
                    assert.ok(res.status === 200);
                } catch (err) {
                    assert.fail(err as Error);
                }
            });
        });
    });

    describe("Allowed addresses that cause redirects", () => {
        allowedAddressesWithRedirect.forEach((url) => {
            it(`GET ${url} - follow = 0`, async () => {
                try {
                    await fetch(url, {
                        follow: 0,
                        headers: { "test-required-header": "true" },
                        agent: agentFn
                    });
                    assert.fail("Expected error, but got response");
                } catch (err) {
                    assert.equal((err as Error).message, `maximum redirect reached at: ${url}`);
                }
            });

            it(`GET ${url} - redirect = "manual"`, async () => {
                try {
                    const res = await fetch(url, {
                        redirect: "manual",
                        headers: { "test-required-header": "true" },
                        agent: agentFn
                    });
                    assert.ok(res.status === 301);
                } catch (err) {
                    assert.fail(err as Error);
                }
            });

            it(`GET ${url} - redirect = "follow"`, async () => {
                try {
                    const res = await fetch(url, {
                        redirect: "follow",
                        headers: { "test-required-header": "true" },
                        agent: agentFn
                    });
                    assert.ok(res.status === 200);
                } catch (err) {
                    assert.fail(err as Error);
                }
            });

            it(`GET ${url} - redirect = "error"`, async () => {
                try {
                    await fetch(url, {
                        redirect: "error",
                        headers: { "test-required-header": "true" },
                        agent: agentFn
                    });
                    assert.fail("Expected error, but got response");
                } catch (err) {
                    assert.equal(
                        (err as Error).message,
                        `uri requested responds with a redirect, redirect mode is set to error: ${url}`
                    );
                }
            });
        });
    });

    describe("Denied addresses that don't cause redirects", () => {
        deniedAddressesNoRedirect.forEach((url) => {
            it(`GET ${url} - follow = 0`, async () => {
                try {
                    await fetch(url, {
                        follow: 0,
                        headers: { "test-required-header": "true" },
                        agent: agentFn
                    });
                    assert.fail("Expected error, but got response");
                } catch (err) {
                    assert.ok((err as Error).message.includes("IP address disallowed by policy"));
                }
            });

            it(`GET ${url} - redirect = "manual"`, async () => {
                try {
                    await fetch(url, {
                        redirect: "manual",
                        headers: { "test-required-header": "true" },
                        agent: agentFn
                    });
                    assert.fail("Expected error, but got response");
                } catch (err) {
                    assert.ok((err as Error).message.includes("IP address disallowed by policy"));
                }
            });

            it(`GET ${url} - redirect = "follow"`, async () => {
                try {
                    await fetch(url, {
                        redirect: "follow",
                        headers: { "test-required-header": "true" },
                        agent: agentFn
                    });
                    assert.fail("Expected error, but got response");
                } catch (err) {
                    assert.ok((err as Error).message.includes("IP address disallowed by policy"));
                }
            });

            it(`GET ${url} - redirect = "error"`, async () => {
                try {
                    await fetch(url, {
                        redirect: "error",
                        headers: { "test-required-header": "true" },
                        agent: agentFn
                    });
                    assert.fail("Expected error, but got response");
                } catch (err) {
                    assert.ok((err as Error).message.includes("IP address disallowed by policy"));
                }
            });
        });
    });

    describe("Denied addresses that cause redirects", () => {
        deniedAddressesWithRedirect.forEach((url) => {
            it(`GET ${url} - follow = 0`, async () => {
                try {
                    await fetch(url, {
                        follow: 0,
                        headers: { "test-required-header": "true" },
                        agent: agentFn
                    });
                    assert.fail("Expected error, but got response");
                } catch (err) {
                    assert.equal((err as Error).message, `maximum redirect reached at: ${url}`);
                }
            });

            it(`GET ${url} - redirect = "manual"`, async () => {
                try {
                    const res = await fetch(url, {
                        redirect: "manual",
                        headers: { "test-required-header": "true" },
                        agent: agentFn
                    });
                    assert.ok(res.status === 301);
                } catch (err) {
                    assert.fail(err as Error);
                }
            });

            it(`GET ${url} - redirect = "follow"`, async () => {
                try {
                    await fetch(url, {
                        redirect: "follow",
                        headers: { "test-required-header": "true" },
                        agent: agentFn
                    });
                    assert.fail("Expected error, but got response");
                } catch (err) {
                    assert.ok((err as Error).message.includes("IP address disallowed by policy"));
                }
            });

            it(`GET ${url} - redirect = "error"`, async () => {
                try {
                    await fetch(url, {
                        redirect: "error",
                        headers: { "test-required-header": "true" },
                        agent: agentFn
                    });
                    assert.fail("Expected error, but got response");
                } catch (err) {
                    assert.equal(
                        (err as Error).message,
                        `uri requested responds with a redirect, redirect mode is set to error: ${url}`
                    );
                }
            });
        });
    });

    describe("Header policy enforcement", () => {
        it("Contains denied header", async () => {
            try {
                await fetch("https://github.com", {
                    headers: { "test-required-header": "true", "test-denied-header": "true" },
                    agent: agentFn
                });
                assert.fail("Expected error, but got response");
            } catch (err) {
                assert.ok((err as Error).message.includes("Request headers or protocol disallowed by policy"));
            }
        });

        it("Missing required header", async () => {
            try {
                await fetch("https://github.com", {
                    headers: {},
                    agent: agentFn
                });
                assert.fail("Expected error, but got response");
            } catch (err) {
                assert.ok((err as Error).message.includes("Request headers or protocol disallowed by policy"));
            }
        });
    });

    after(() => {
        httpAgent.destroy();
        httpsAgent.destroy();
        server.close();
    });
});
