// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

const fs = require("fs");
const path = require("path");
const tar = require("tar");
const assert = require("assert");
const dns = require("dns");
const https = require("https");

describe("Tests for most recent .tgz package", function () {
    const baseDir = path.join(__dirname, "../..");
    const extractPath = path.join(baseDir, "temp-lib");
    let URIValidator;
    let AntiSSRFPolicy;
    let PolicyConfigOptions;

    before(async function () {
        // Find the .tgz file dynamically
        const files = fs.readdirSync(baseDir);
        const tgzFile = files.find((file) => file.match(/^.*\.tgz$/));

        if (!tgzFile) {
            throw new Error("No matching .tgz file found.");
        }

        const tgzPath = path.join(baseDir, tgzFile); // Ensure the temp directory exists

        if (!fs.existsSync(extractPath)) {
            fs.mkdirSync(extractPath);
        } // Extract the .tgz file

        await tar.x({
            file: tgzPath,
            cwd: extractPath,
            sync: true,
            strip: 1
        }); // Dynamically require the AddOne function

        const lib = require(path.join(extractPath, "out/src/index.js")); // Adjust if needed
        URIValidator = lib.URIValidator;
        AntiSSRFPolicy = lib.AntiSSRFPolicy;
        PolicyConfigOptions = lib.PolicyConfigOptions;
    });

    it("URIValidator.inDomain test", () => {
        assert.equal(URIValidator.inDomain("https://example.com", ".example.com"), true);
        assert.equal(URIValidator.inDomain("https://example.com.evil.com", "example.com"), false);
    });

    describe("AntiSSRFPolicy tests", () => {
        let antiSSRFHttpsAgent;

        before(async () => {
            const policy = new AntiSSRFPolicy(PolicyConfigOptions.None);
            policy.addRequiredHeaders(["test-required-header"]);
            policy.addDeniedHeaders(["test-denied-header"]);

            const googleIPs = await dns.promises.lookup("www.google.com", {
                family: 0,
                all: true
            });
            const appleIPs = await dns.promises.lookup("apple.com", { family: 0, all: true });
            policy.addDeniedAddresses([...googleIPs, ...appleIPs].map((address) => address.address));

            antiSSRFHttpsAgent = policy.getHttpsAgent();
        });

        it("Allow valid IP", (done) => {
            const req = https.get(
                "https://www.bing.com",
                {
                    agent: antiSSRFHttpsAgent,
                    port: 443,
                    headers: { "test-required-header": 25 }
                },
                (res) => {
                    res.on("data", (data) => {});
                    res.on("end", () => {
                        assert.equal(res.statusCode, 200);
                        done();
                    });
                }
            );

            req.on("error", (err) => {
                done(err);
            });

            req.end();
        });

        it("Deny blocked IP", (done) => {
            const req = https.get(
                "https://apple.com",
                {
                    agent: antiSSRFHttpsAgent,
                    host: "www.bing.com",
                    headers: { "test-required-header": 25 }
                },
                (res) => {
                    res.on("data", (data) => {});
                    res.on("end", () => {
                        done("Expected error, but got response");
                    });
                }
            );

            req.on("error", (err) => {
                assert.equal(err.message, "IP address disallowed by policy");
                done();
            });

            req.end();
        });

        after(() => {
            antiSSRFHttpsAgent.destroy();
        });
    });
});
