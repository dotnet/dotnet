// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

import assert from "assert";

import { URIValidator } from "../../src";

describe("InAzureStorageDomain Tests", () => {
    it("should return false for null and empty inputs", () => {
        assert.strictEqual(URIValidator.inAzureStorageDomain(null!), false);
        assert.strictEqual(URIValidator.inAzureStorageDomain(""), false);
    });

    it("accepts URLs in storage domains", () => {
        const urls = [
            "https://myapp.blob.core.windows.net",
            "https://frontend3.web.core.windows.net",
            "https://data-lake.dfs.core.windows.net",
            "https://files123.file.core.windows.net",
            "https://queue-svc.queue.core.windows.net",
            "https://tables01.table.core.windows.net",
            "https://secure.blob.storage.azure.net",
            "https://internal9.web.storage.azure.net",
            "https://private-ep.dfs.storage.azure.net",
            "https://corp-files.file.storage.azure.net",
            "https://company2.queue.storage.azure.net",
            "https://enterprise.table.storage.azure.net",
            "https://gov-data.blob.core.usgovcloudapi.net",
            "https://portal456.web.core.usgovcloudapi.net",
            "https://analytics.dfs.core.usgovcloudapi.net",
            "https://docs-gov.file.core.usgovcloudapi.net",
            "https://notify123.queue.core.usgovcloudapi.net",
            "https://records.table.core.usgovcloudapi.net",
            "https://china-app.blob.core.chinacloudapi.cn",
            "https://website7.web.core.chinacloudapi.cn",
            "https://bigdata99.dfs.core.chinacloudapi.cn",
            "https://storage.file.core.chinacloudapi.cn",
            "https://events-cn.queue.core.chinacloudapi.cn",
            "https://metadata2.table.core.chinacloudapi.cn"
        ];

        for (const url of urls) {
            assert.strictEqual(
                URIValidator.inAzureStorageDomain(url),
                true,
                `Expected true for ${url}`
            );
            assert.strictEqual(
                URIValidator.inAzureStorageDomain(new URL(url)),
                true,
                `Expected true for URL object: ${url}`
            );
        }
    });

    it("supports secondary storage endpoints", () => {
        const urls = [
            "https://myapp-secondary.blob.storage.azure.net",
            "https://website-secondary.web.core.windows.net",
            "https://files5-secondary.dfs.core.usgovcloudapi.net",
            "https://messages-secondary.queue.core.chinacloudapi.cn",
            "https://corp99-secondary.table.storage.azure.net",
            "https://backup-secondary.file.core.windows.net"
        ];

        for (const url of urls) {
            assert.strictEqual(
                URIValidator.inAzureStorageDomain(url),
                true,
                `Expected true for ${url}`
            );
            assert.strictEqual(
                URIValidator.inAzureStorageDomain(new URL(url)),
                true,
                `Expected true for URL object: ${url}`
            );
        }
    });

    it("supports private link storage domains", () => {
        const urls = [
            "https://acct.privatelink.blob.storage.azure.net",
            "https://web12.privatelink.web.core.windows.net",
            "https://data.privatelink.dfs.storage.azure.net",
            "https://files99.privatelink.file.core.usgovcloudapi.net",
            "https://queue.privatelink.queue.core.chinacloudapi.cn",
            "https://tables5-secondary.privatelink.table.storage.azure.net"
        ];

        for (const url of urls) {
            assert.strictEqual(
                URIValidator.inAzureStorageDomain(url),
                true,
                `Expected true for ${url}`
            );
            assert.strictEqual(
                URIValidator.inAzureStorageDomain(new URL(url)),
                true,
                `Expected true for URL object: ${url}`
            );
        }
    });

    it("supports static sites and DNS zones", () => {
        const urls = [
            "https://contosostaticsite.z22.web.core.windows.net",
            "https://webapp5.z03.web.storage.azure.net",
            "https://frontend.z45.blob.core.usgovcloudapi.net",
            "https://portal99.z01.web.core.chinacloudapi.cn",
            "https://static-site.privatelink.z88.dfs.storage.azure.net",
            "https://demo-secondary.z0.web.core.windows.net"
        ];

        for (const url of urls) {
            assert.strictEqual(
                URIValidator.inAzureStorageDomain(url),
                true,
                `Expected true for ${url}`
            );
            assert.strictEqual(
                URIValidator.inAzureStorageDomain(new URL(url)),
                true,
                `Expected true for URL object: ${url}`
            );
        }
    });

    it("rejects URLs not in storage domains", () => {
        const urls = [
            "https://my--app.blob.core.windows.net",
            "https://data--lake.dfs.storage.azure.net",
            "https://web--site.web.core.usgovcloudapi.net",
            "https://myapp.blob.core.windwos.net",
            "https://storage.table.core.windoes.net",
            "https://files.dfs.core.chinacloudapi.com",
            "https://queue.queue.stoarge.azure.net",
            "https://myapp.database.core.windows.net",
            "https://storage.cache.storage.azure.net",
            "https://files.storage.core.usgovcloudapi.net",
            "https://myapp.core.blob.windows.net",
            "https://storage.azure.storage.net",
            "https://files.windows.core.net",
            "https://myapp.blob.core.blob.windows.net",
            "https://storage.web.core.windows.net.storage.azure.net",
            "https://myapp.blob.core.windows.net.evil.com",
            "https://storage.table.core.windows.netmalicious",
            "https://files.dfs.storage.azure.net.attacker.org",
            "https://queue.queue.core.chinacloudapi.cnbad",
            "https://secure.web.storage.azure.netphishing",
            "https://corp.file.core.usgovcloudapi.net.fake"
        ];

        for (const url of urls) {
            assert.strictEqual(
                URIValidator.inAzureStorageDomain(url),
                false,
                `Expected false for ${url}`
            );
            assert.strictEqual(
                URIValidator.inAzureStorageDomain(new URL(url)),
                false,
                `Expected false for URL object: ${url}`
            );
        }
    });

    it("correctly parses various URL components", () => {
        const testCases: Array<[string, boolean]> = [
            ["http://accountname.blob.core.windows.net/some/path", true],
            ["http://accountname.blob.core.windows.net#fragment", true],
            ["http://accountname.blob.core.windows.net/?query=hi", true],
            ["http://accountname.blob.core.windows.net:45", true],
            ["https://username@accountname.blob.core.windows.net", true],
            ["https://username:password@accountname.blob.core.windows.net", true],
            ["https:accountname.blob.core.windows.net", true], // NodeJS parses protocols different
            ["http:/accountname.blob.core.windows.net", true], // NodeJS parses protocols different
            ["http:/\\accountname.blob.core.windows.net", true],
            ["http:\\/accountname.blob.core.windows.net", true],
            ["http://accountname.blob.core.windows.net:badPort", false],
            ["http://:accountname.blob.core.windows.net", false]
        ];

        for (const [url, expectedResult] of testCases) {
            assert.strictEqual(
                URIValidator.inAzureStorageDomain(url),
                expectedResult,
                `Expected ${expectedResult} for ${url}`
            );

            // Only test URL overload for valid URI formats
            try {
                const parsedUrl = new URL(url);
                assert.strictEqual(
                    URIValidator.inAzureStorageDomain(parsedUrl),
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
            "http://ñame.blob.core.windows.net/",
            "http://name.blob.core.wiñdows.net/",
            "http://evil.c℁.blob.core.windows.net",
            "https://tëst.web.storage.azure.net",
            "https://app.blob.core.windöws.net",
            "https://файлы.file.core.chinacloudapi.cn",
            "https://データ.dfs.core.usgovcloudapi.net",
            "https://myapp.bløb.core.windows.net"
        ];

        for (const url of urls) {
            assert.strictEqual(
                URIValidator.inAzureStorageDomain(url),
                false,
                `Expected false for ${url}`
            );

            // Test URL overload if the string can be parsed as a URL
            try {
                const parsedUrl = new URL(url);
                assert.strictEqual(
                    URIValidator.inAzureStorageDomain(parsedUrl),
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
            "http://ACCOUNTNAME.blob.core.windows.net",
            "http://accountname.BLOB.core.windows.net",
            "http://ACCOUNTNAME.BLOB.CORE.WINDOWS.NET",
            "hTtP://test.blob.core.windows.net/",
            "HTTPS://myapp.WEB.storage.azure.net",
            "https://DATA.dfs.STORAGE.AZURE.NET",
            "HtTpS://files.FILE.core.usgovcloudapi.net",
            "http://QUEUE.queue.CORE.chinacloudapi.cn",
            "https://TABLES.table.core.WINDOWS.net"
        ];

        for (const url of urls) {
            assert.strictEqual(
                URIValidator.inAzureStorageDomain(url),
                true,
                `Expected true for ${url}`
            );
            assert.strictEqual(
                URIValidator.inAzureStorageDomain(new URL(url)),
                true,
                `Expected true for URL object: ${url}`
            );
        }
    });

    it("correctly enforces allowed protocols", () => {
        const testCases: Array<[string, boolean]> = [
            ["http://accountname.blob.core.windows.net", true],
            ["https://accountname.blob.core.windows.net", true],
            ["ws://accountname.blob.core.windows.net", false],
            ["wss://accountname.blob.core.windows.net", false],
            ["ftp://accountname.blob.core.windows.net", false],
            ["file://accountname.blob.core.windows.net", false],
            ["gopher://accountname.blob.core.windows.net", false],
            ["mailto:accountname.blob.core.windows.net", false],
            ["data://accountname.blob.core.windows.net", false],
            ["javascript:alert('XSS')", false],
            ["evil.com://accountname.blob.core.windows.net", false]
        ];

        for (const [url, expectedResult] of testCases) {
            assert.strictEqual(
                URIValidator.inAzureStorageDomain(url),
                expectedResult,
                `Expected ${expectedResult} for ${url}`
            );

            // Only test URL overload for schemes that can create valid URLs
            try {
                const parsedUrl = new URL(url);
                assert.strictEqual(
                    URIValidator.inAzureStorageDomain(parsedUrl),
                    expectedResult,
                    `Expected ${expectedResult} for URL object: ${url}`
                );
            } catch {
                // Ignore exceptions from invalid URI formats
            }
        }
    });
});
