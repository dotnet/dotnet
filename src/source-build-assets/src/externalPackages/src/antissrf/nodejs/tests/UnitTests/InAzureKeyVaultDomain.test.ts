// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

import assert from "assert";

import { URIValidator } from "../../src";

describe("InAzureKeyVaultDomain Tests", () => {
    it("should return false for null and empty inputs", () => {
        assert.strictEqual(URIValidator.inAzureKeyVaultDomain(null as unknown as string), false);
        assert.strictEqual(URIValidator.inAzureKeyVaultDomain(null as unknown as URL), false);
        assert.strictEqual(URIValidator.inAzureKeyVaultDomain(""), false);
    });

    it("accepts URLs in key vault domains", () => {
        const urls = [
            "https://contoso.vault.azure.net/",
            "https://fabrikam42.managedhsm.azure.net",
            "https://corp-hsm.vault.azure.cn",
            "https://devkeys99.managedhsm.azure.cn/",
            "https://govvault1.vault.usgovcloudapi.net",
            "https://securehsm.managedhsm.usgovcloudapi.net/"
        ];

        for (const url of urls) {
            assert.strictEqual(
                URIValidator.inAzureKeyVaultDomain(url),
                true,
                `Expected true for ${url}`
            );
            assert.strictEqual(
                URIValidator.inAzureKeyVaultDomain(new URL(url)),
                true,
                `Expected true for URL object: ${url}`
            );
        }
    });

    it("supports private link key vault domains", () => {
        const urls = [
            "https://contoso.privatelink.vault.azure.net",
            "https://fabrikam42.privatelink.managedhsm.azure.net",
            "https://corp-hsm.privatelink.vault.azure.cn",
            "https://devkeys99.privatelink.managedhsm.azure.cn",
            "https://govvault1.privatelink.vault.usgovcloudapi.net",
            "https://securehsm.privatelink.managedhsm.usgovcloudapi.net"
        ];

        for (const url of urls) {
            assert.strictEqual(
                URIValidator.inAzureKeyVaultDomain(url),
                true,
                `Expected true for ${url}`
            );
            assert.strictEqual(
                URIValidator.inAzureKeyVaultDomain(new URL(url)),
                true,
                `Expected true for URL object: ${url}`
            );
        }
    });

    it("rejects URLs not in key vault domains", () => {
        const urls = [
            "https://my--vault.vault.azure.net",
            "https://contoso.managedhsm.azure.net.evil.com",
            "https://fabrikam.vault.azure.net.attacker.org",
            "https://corp.vault.azuree.net",
            "https://devkeys.vault.azure.nett",
            "https://contoso.vault.azure.netmalicious",
            "https://contoso.managedhsm.azure.usgovcloudapi.net",
            "https://contoso.azure.vault.net",
            "https://contoso.vault.azure.cn.fake",
            "https://securehsm.managedhsm.usgovcloudapi.net.phishing",
            "https://corp.managedhsm.azure.cnn",
            "https://contoso.vaultazure.net",
            "https://contoso.vault.azure"
        ];

        for (const url of urls) {
            assert.strictEqual(
                URIValidator.inAzureKeyVaultDomain(url),
                false,
                `Expected false for ${url}`
            );
            assert.strictEqual(
                URIValidator.inAzureKeyVaultDomain(new URL(url)),
                false,
                `Expected false for URL object: ${url}`
            );
        }
    });

    it("correctly parses various URL components", () => {
        const testCases: Array<[string, boolean]> = [
            ["http://accountname.vault.azure.net/some/path", true],
            ["http://accountname.vault.azure.net#fragment", true],
            ["http://accountname.vault.azure.net/?query=hi", true],
            ["http://accountname.vault.azure.net:45", true],
            ["https://username@accountname.vault.azure.net", true],
            ["https://username:password@accountname.vault.azure.net", true],
            ["https:accountname.vault.azure.net", true], // NodeJS parses protocols different
            ["http:/accountname.vault.azure.net", true], // NodeJS parses protocols different
            ["http:/\\accountname.vault.azure.net", true],
            ["http:\\/accountname.vault.azure.net", true],
            ["http://accountname.vault.azure.net:badPort", false],
            ["http://:accountname.vault.azure.net", false]
        ];

        for (const [url, expectedResult] of testCases) {
            assert.strictEqual(
                URIValidator.inAzureKeyVaultDomain(url),
                expectedResult,
                `Expected ${expectedResult} for ${url}`
            );

            // Only test URL overload for valid URI formats
            try {
                const parsedUrl = new URL(url);
                assert.strictEqual(
                    URIValidator.inAzureKeyVaultDomain(parsedUrl),
                    expectedResult,
                    `Expected ${expectedResult} for URL object: ${url}`
                );
            } catch {
                // Ignore exceptions for invalid URI formats
            }
        }
    });

    it("correctly rejects unicode", () => {
        const urls = [
            "http://ñame.vault.azure.net/",
            "https://contøso.managedhsm.azure.net",
            "https://fabrikäm.vault.azure.cn/",
            "https://corp.vàult.azure.net",
            "https://devkeys.vault.àzure.net",
            "https://contoso.vault.azure.cñ",
            "https://сontoso.vault.usgovcloudapi.net",
            "https://myapp.mànagedhsm.azure.cn",
            "http://evil.c℁.vault.azure.net",
            "https://データ.vault.usgovcloudapi.net",
            "https://файлы.managedhsm.usgovcloudapi.net"
        ];

        for (const url of urls) {
            assert.strictEqual(
                URIValidator.inAzureKeyVaultDomain(url),
                false,
                `Expected false for ${url}`
            );

            // Test URL overload if the string can be parsed as a URL
            try {
                const parsedUrl = new URL(url);
                assert.strictEqual(
                    URIValidator.inAzureKeyVaultDomain(parsedUrl),
                    false,
                    `Expected false for URL object: ${url}`
                );
            } catch {
                // Ignore exceptions for invalid URI formats
            }
        }
    });

    it("correctly handles upper/lowercase", () => {
        const urls = [
            "http://CONTOSO.vault.azure.net",
            "https://fabrikam42.VAULT.azure.net",
            "https://corp-hsm.vault.AZURE.net",
            "https://DEVKEYS99.managedhsm.azure.cn/",
            "https://govvault1.MANAGEDHSM.azure.cn",
            "https://SECUREHSM.vault.USGOVCLOUDAPI.net",
            "https://contoso.managedhsm.USGOVCLOUDAPI.NET",
            "HTTPS://fabrikam42.vault.azure.net",
            "hTtPs://corp-hsm.managedhsm.azure.net",
            "https://CONTOSO.VAULT.AZURE.NET",
            "HtTpS://devkeys99.vault.azure.cn",
            "https://GovVault1.Vault.Azure.Net"
        ];

        for (const url of urls) {
            assert.strictEqual(
                URIValidator.inAzureKeyVaultDomain(url),
                true,
                `Expected true for ${url}`
            );
            assert.strictEqual(
                URIValidator.inAzureKeyVaultDomain(new URL(url)),
                true,
                `Expected true for URL object: ${url}`
            );
        }
    });

    it("correctly enforces allowed protocols", () => {
        const testCases: Array<[string, boolean]> = [
            ["http://accountname.vault.azure.net", true],
            ["https://accountname.vault.azure.net", true],
            ["ws://accountname.vault.azure.net", false],
            ["wss://accountname.vault.azure.net", false],
            ["ftp://accountname.vault.azure.net", false],
            ["file://accountname.vault.azure.net", false],
            ["gopher://accountname.vault.azure.net", false],
            ["mailto:accountname.vault.azure.net", false],
            ["data://accountname.vault.azure.net", false],
            ["javascript:alert('XSS')", false],
            ["evil.com://accountname.vault.azure.net", false]
        ];

        for (const [url, expectedResult] of testCases) {
            assert.strictEqual(
                URIValidator.inAzureKeyVaultDomain(url),
                expectedResult,
                `Expected ${expectedResult} for ${url}`
            );

            // Only test URL overload for schemes that can create valid URLs
            try {
                const parsedUrl = new URL(url);
                assert.strictEqual(
                    URIValidator.inAzureKeyVaultDomain(parsedUrl),
                    expectedResult,
                    `Expected ${expectedResult} for URL object: ${url}`
                );
            } catch {
                // Ignore exceptions from invalid URI formats
            }
        }
    });
});
