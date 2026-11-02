// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

import assert from "assert";

import { URIValidator } from "../../src";

describe("InDomain Tests", () => {
    it("should return false for null and empty inputs", () => {
        assert.strictEqual(URIValidator.inDomain(null as unknown as string, "bing.com"), false);
        assert.strictEqual(URIValidator.inDomain(null as unknown as URL, "bing.com"), false);
        assert.strictEqual(URIValidator.inDomain(null as unknown as string, ["bing.com"]), false);
        assert.strictEqual(URIValidator.inDomain(null as unknown as URL, ["bing.com"]), false);

        assert.strictEqual(URIValidator.inDomain("http://bing.com", null as unknown as string), false);
        assert.strictEqual(URIValidator.inDomain("http://bing.com", ""), false);
        assert.strictEqual(URIValidator.inDomain(new URL("http://bing.com"), null as unknown as string), false);
        assert.strictEqual(URIValidator.inDomain(new URL("http://bing.com"), ""), false);

        assert.strictEqual(URIValidator.inDomain("http://bing.com", null as unknown as string[]), false);
        assert.strictEqual(URIValidator.inDomain("http://bing.com", []), false);
        assert.strictEqual(URIValidator.inDomain(new URL("http://bing.com"), null as unknown as string[]), false);
        assert.strictEqual(URIValidator.inDomain(new URL("http://bing.com"), []), false);
    });

    it("accepts URLs in trusted single domains", () => {
        const testCases: Array<[string, string]> = [
            ["http://office.com", "office.com"],
            ["https://office.com", "office.com"],
            ["https://azure.com", ".azure.com"],
            ["https://subdomain.microsoft.com", "microsoft.com"],
            ["https://subdomain.microsoft.com", ".microsoft.com"]
        ];

        for (const [url, trustedDomain] of testCases) {
            assert.strictEqual(
                URIValidator.inDomain(url, trustedDomain),
                true,
                `Expected true for ${url} with domain ${trustedDomain}`
            );
            assert.strictEqual(
                URIValidator.inDomain(new URL(url), trustedDomain),
                true,
                `Expected true for URL object ${url} with domain ${trustedDomain}`
            );
        }
    });

    it("accepts URLs in trusted domain arrays", () => {
        const testCases: Array<[string, string[]]> = [
            ["https://subdomain.one.com", ["one.com", ".two.com"]],
            ["http://subdomain.two.com", ["one.com", ".two.com"]],
            ["https://one.com", ["one.com", ".two.com"]],
            ["https://two.net", ["one.com", ".two.net"]]
        ];

        for (const [url, trustedDomains] of testCases) {
            assert.strictEqual(
                URIValidator.inDomain(url, trustedDomains),
                true,
                `Expected true for ${url} with domains ${trustedDomains.join(", ")}`
            );
            assert.strictEqual(
                URIValidator.inDomain(new URL(url), trustedDomains),
                true,
                `Expected true for URL object ${url} with domains ${trustedDomains.join(", ")}`
            );
        }
    });

    it("rejects URLs not in trusted single domains", () => {
        const testCases: Array<[string, string]> = [
            ["http://azure.com", "office.com"],
            ["https://office.com", "subdomain.office.com"],
            ["https://azure.com", ".office.com"],
            ["https://subdomain.microsoft.com", "differentsubdomain.microsoft.com"]
        ];

        for (const [url, trustedDomain] of testCases) {
            assert.strictEqual(
                URIValidator.inDomain(url, trustedDomain),
                false,
                `Expected false for ${url} with domain ${trustedDomain}`
            );
            assert.strictEqual(
                URIValidator.inDomain(new URL(url), trustedDomain),
                false,
                `Expected false for URL object ${url} with domain ${trustedDomain}`
            );
        }
    });

    it("rejects URLs not in trusted domain arrays", () => {
        const testCases: Array<[string, string[]]> = [
            ["http://azure.com", ["one.com", "office.com"]],
            ["https://office.com", ["subdomain.office.com"]],
            ["https://azure.com", [".office.com", "two.com"]],
            ["https://subdomain.microsoft.com", ["differentsubdomain.microsoft.com"]]
        ];

        for (const [url, trustedDomains] of testCases) {
            assert.strictEqual(
                URIValidator.inDomain(url, trustedDomains),
                false,
                `Expected false for ${url} with domains ${trustedDomains.join(", ")}`
            );
            assert.strictEqual(
                URIValidator.inDomain(new URL(url), trustedDomains),
                false,
                `Expected false for URL object ${url} with domains ${trustedDomains.join(", ")}`
            );
        }
    });

    it("correctly parses various URL components", () => {
        const urls = [
            "http://username@bing.com:/",
            "http://username:password@bing.com",
            "http://bing.com:45",
            "http://bing.com/some/path",
            "http://bing.com#fragment",
            "http://bing.com/?query=hi",
            "http:/\\bing.com",
            "http:\\/bing.com"
        ];

        for (const url of urls) {
            assert.strictEqual(
                URIValidator.inDomain(url, "bing.com"),
                true,
                `Expected true for ${url} with single domain`
            );
            assert.strictEqual(
                URIValidator.inDomain(url, ["bing.com"]),
                true,
                `Expected true for ${url} with domain array`
            );
            assert.strictEqual(
                URIValidator.inDomain(new URL(url), "bing.com"),
                true,
                `Expected true for URL object ${url} with single domain`
            );
            assert.strictEqual(
                URIValidator.inDomain(new URL(url), ["bing.com"]),
                true,
                `Expected true for URL object ${url} with domain array`
            );
        }
    });

    it("correctly rejects invalid URL components", () => {
        const urls = [
            "http://bing.com:badPort",
            "http://:bing.com"
        ];

        for (const url of urls) {
            assert.strictEqual(
                URIValidator.inDomain(url, "bing.com"),
                false,
                `Expected false for ${url} with single domain`
            );
            assert.strictEqual(
                URIValidator.inDomain(url, ["bing.com"]),
                false,
                `Expected false for ${url} with domain array`
            );
        }
    });

    it("correctly handles unicode in single domains", () => {
        const testCases: Array<[string, string]> = [
            ["http://español.test.net/", "test.net"],
            ["http://español.test.net/", "xn--espaol-zwa.test.net"],
            ["http://你好/", "xn--6qq79v"],
            ["http://test.你好/", "你好"],
            ["http://bing.hi.com/", "hi.com"],
            ["http://bing.hı.com/", "hı.com"],
            ["http://bing.hí.com/", "hí.com"],
            ["http://😉", "😉"]
        ];

        for (const [url, trustedDomain] of testCases) {
            assert.strictEqual(
                URIValidator.inDomain(url, trustedDomain),
                true,
                `Expected true for ${url} with domain ${trustedDomain}`
            );
            assert.strictEqual(
                URIValidator.inDomain(new URL(url), trustedDomain),
                true,
                `Expected true for URL object ${url} with domain ${trustedDomain}`
            );
        }
    });

    it("correctly handles unicode in domain arrays", () => {
        const testCases: Array<[string, string[]]> = [
            ["http://español.test.net/", ["notempty", "test.net"]],
            ["http://español.test.net/", ["hello", "xn--espaol-zwa.test.net"]],
            ["http://你好/", ["xn--6qq79v", "not_the_domain.com"]],
            ["http://bing.hı.com/", ["hi.com", "hı.com"]],
            ["http://bing.hí.com/", ["hí.com", "notempty"]],
            ["http://test.你好/", ["你好", "bing.com"]]
        ];

        for (const [url, trustedDomains] of testCases) {
            assert.strictEqual(
                URIValidator.inDomain(url, trustedDomains),
                true,
                `Expected true for ${url} with domains ${trustedDomains.join(", ")}`
            );
            assert.strictEqual(
                URIValidator.inDomain(new URL(url), trustedDomains),
                true,
                `Expected true for URL object ${url} with domains ${trustedDomains.join(", ")}`
            );
        }
    });

    it("correctly rejects invalid unicode single domains", () => {
        const testCases: Array<[string, string]> = [
            ["http://bing.hı.com/", "hi.com"],
            ["http://bing.hí.com/", "hi.com"],
            ["http://😉", "🔨"]
        ];

        for (const [url, trustedDomain] of testCases) {
            assert.strictEqual(
                URIValidator.inDomain(url, trustedDomain),
                false,
                `Expected false for ${url} with domain ${trustedDomain}`
            );
            assert.strictEqual(
                URIValidator.inDomain(new URL(url), trustedDomain),
                false,
                `Expected false for URL object ${url} with domain ${trustedDomain}`
            );
        }
    });

    it("correctly rejects invalid unicode domain arrays", () => {
        const testCases: Array<[string, string[]]> = [
            ["http://bing.hı.com/", ["hi.com", "hí.com"]],
            ["http://bing.hí.com/", ["hi.com", "hı.com"]],
            ["http://😉", ["🔨", ""]]
        ];

        for (const [url, trustedDomains] of testCases) {
            assert.strictEqual(
                URIValidator.inDomain(url, trustedDomains),
                false,
                `Expected false for ${url} with domains ${trustedDomains.join(", ")}`
            );
            assert.strictEqual(
                URIValidator.inDomain(new URL(url), trustedDomains),
                false,
                `Expected false for URL object ${url} with domains ${trustedDomains.join(", ")}`
            );
        }
    });

    it("correctly rejects invalid unicode strings", () => {
        assert.strictEqual(
            URIValidator.inDomain("http://evil.c℁.core.azure.net", "azure.net"),
            false,
            "Expected false for invalid unicode domain"
        );
    });

    it("correctly handles upper/lowercase", () => {
        const testCases: Array<[string, string]> = [
            ["hTtP://test.net", "test.net"],
            ["wSS://test.net", "test.net"],
            ["http://HELLO.com", "hello.com"],
            ["http://Hello.你好/", "xn--6qq79v"],
            ["http://español.test.net/", "TeSt.net"],
            ["http://hello.COM", "HELLO.com"]
        ];

        for (const [url, trustedDomain] of testCases) {
            assert.strictEqual(
                URIValidator.inDomain(url, trustedDomain),
                true,
                `Expected true for ${url} with domain ${trustedDomain}`
            );
            assert.strictEqual(
                URIValidator.inDomain(new URL(url), trustedDomain),
                true,
                `Expected true for URL object ${url} with domain ${trustedDomain}`
            );
        }
    });

    it("correctly handles upper/lowercase in domain arrays", () => {
        const testCases: Array<[string, string[]]> = [
            ["http://HELLO.com", ["notempty", "hello.com"]],
            ["http://Hello.你好/", ["xn--6qq79v", "not_the_domain.com"]],
            ["http://español.test.net/", ["TeSt.net", "asdf"]],
            ["http://hello.COM", ["HELLO.com"]]
        ];

        for (const [url, trustedDomains] of testCases) {
            assert.strictEqual(
                URIValidator.inDomain(url, trustedDomains),
                true,
                `Expected true for ${url} with domains ${trustedDomains.join(", ")}`
            );
            assert.strictEqual(
                URIValidator.inDomain(new URL(url), trustedDomains),
                true,
                `Expected true for URL object ${url} with domains ${trustedDomains.join(", ")}`
            );
        }
    });

    it("correctly enforces allowed protocols", () => {
        const urls = [
            "http://bing.com",
            "https://bing.com",
            "ws://bing.com",
            "wss://bing.com"
        ];

        for (const url of urls) {
            assert.strictEqual(
                URIValidator.inDomain(url, "bing.com"),
                true,
                `Expected true for ${url}`
            );
            assert.strictEqual(
                URIValidator.inDomain(new URL(url), "bing.com"),
                true,
                `Expected true for URL object ${url}`
            );
        }
    });

    it("correctly rejects disallowed protocols", () => {
        const urls = [
            "ftp://bing.com",
            "file://bing.com",
            "gopher://bing.com",
            "mailto:bing.com",
            "data://bing.com",
            "javascript:alert('XSS')",
            "evil.com://bing.com"
        ];

        for (const url of urls) {
            assert.strictEqual(
                URIValidator.inDomain(url, "bing.com"),
                false,
                `Expected false for ${url}`
            );
            assert.strictEqual(
                URIValidator.inDomain(new URL(url), "bing.com"),
                false,
                `Expected false for URL object ${url}`
            );
        }
    });

    it("correctly rejects file path strings", () => {
        const testCases: Array<[string, string]> = [
            ["c:\\foo\\bar", "somedomain.com"],
            ["file:///filepath", "somedomain.com"],
            ["CCCCCCCCCCCCCCCCCCCCCCC:\\\\\\\\\\\\foo\\bar", "somedomain.com"],
            ["/foo/bar", "somedomain.com"],
            ["//////////////////////////////////////", "somedomain.com"],
            ["\\\\.\\a\\a\\a\\", "somedomain.com"],
            ["\\\\\\.\\a\\a\\a\\", "somedomain.com"]
        ];

        for (const [url, trustedDomain] of testCases) {
            assert.strictEqual(
                URIValidator.inDomain(url, trustedDomain),
                false,
                `Expected false for file path ${url}`
            );
        }
    });
});
