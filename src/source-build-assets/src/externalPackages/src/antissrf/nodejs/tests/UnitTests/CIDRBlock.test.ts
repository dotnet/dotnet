// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

import assert from "assert";

import { CIDRBlock } from "../../src/Helpers/CIDRBlock";
import { AntiSSRFError } from "../../src";
import { BlockList } from "net";

const toParsedAddress = (ip: string): string => CIDRBlock._parseIPAddress(ip)[0];

describe("CIDRBlock Tests", () => {
    it("bad inputs", () => {
        // Parse - null input
        assert.throws(
            () => CIDRBlock._parseIPAddress(null!),
            (err: AntiSSRFError) => err.message === "Null argument"
        );

        // Parse - too many /
        assert.throws(
            () => CIDRBlock._parseIPAddress("192.168.1.0/24"),
            (err: AntiSSRFError) => err.message.includes("Invalid IPv4 address")
        );

        // Parse - invalid IP address
        assert.throws(
            () => CIDRBlock._parseIPAddress("256.256.256.256"),
            (err: AntiSSRFError) => err.message.includes("Invalid IPv4 address")
        );
        assert.throws(
            () => CIDRBlock._parseIPAddress("192.168.1.300"),
            (err: AntiSSRFError) => err.message.includes("Invalid IPv4 address")
        );
        assert.throws(
            () => CIDRBlock._parseIPAddress("not-an-ip"),
            (err: AntiSSRFError) => err.message.includes("Invalid IPv4 address")
        );
        assert.throws(
            () => CIDRBlock._parseIPAddress("999.999.999.999"),
            (err: AntiSSRFError) => err.message.includes("Invalid IPv4 address")
        );
        assert.throws(
            () => CIDRBlock._parseIPAddress("[127.0.0.1]"),
            (err: AntiSSRFError) => err.message.includes("Invalid IPv4 address")
        );
        assert.throws(
            () => CIDRBlock._parseIPAddress("127.0.0.01"),
            (err: AntiSSRFError) => err.message.includes("Invalid IPv4 address")
        );
        assert.throws(
            () => CIDRBlock._parseIPAddress(""),
            (err: AntiSSRFError) => err.message.includes("Invalid IPv4 address")
        );
        assert.throws(
            () => CIDRBlock._parseIPAddress(" 127.0.0.1"),
            (err: AntiSSRFError) => err.message.includes("Invalid IPv4 address")
        );
        assert.throws(
            () => CIDRBlock._parseIPAddress("127.0.0.1 "),
            (err: AntiSSRFError) => err.message.includes("Invalid IPv4 address")
        );
        assert.throws(
            () => CIDRBlock._parseIPAddress("127.0.0"),
            (err: AntiSSRFError) => err.message.includes("Invalid IPv4 address")
        );
        assert.throws(
            () => CIDRBlock._parseIPAddress("127.0.0.1.5"),
            (err: AntiSSRFError) => err.message.includes("Invalid IPv4 address")
        );
        assert.throws(
            () => CIDRBlock._parseIPAddress("127.-1.0.1"),
            (err: AntiSSRFError) => err.message.includes("Invalid IPv4 address")
        );
        assert.throws(
            () => CIDRBlock._parseIPAddress("127.0.0.+1"),
            (err: AntiSSRFError) => err.message.includes("Invalid IPv4 address")
        );
        assert.throws(
            () => CIDRBlock._parseIPAddress("01.2.3.4"),
            (err: AntiSSRFError) => err.message.includes("Invalid IPv4 address")
        );
        assert.throws(
            () => CIDRBlock._parseIPAddress("gggg::1"),
            (err: AntiSSRFError) => err.message.includes("Invalid IPv6 address")
        );
        assert.throws(
            () => CIDRBlock._parseIPAddress("2001:::1"),
            (err: AntiSSRFError) => err.message.includes("Invalid IPv6 address")
        );
        assert.throws(
            () => CIDRBlock._parseIPAddress("2001::db8::1"),
            (err: AntiSSRFError) => err.message.includes("Invalid IPv6 address")
        );
        assert.throws(
            () => CIDRBlock._parseIPAddress("2001:db8:1:2:3:4:5:6:7"),
            (err: AntiSSRFError) => err.message.includes("Invalid IPv6 address")
        );
        assert.throws(
            () => CIDRBlock._parseIPAddress("2001:db8::g1"),
            (err: AntiSSRFError) => err.message.includes("Invalid IPv6 address")
        );
        assert.throws(
            () => CIDRBlock._parseIPAddress("::ffff:192.168.1.999"),
            (err: AntiSSRFError) => err.message.includes("Invalid IPv6 address")
        );

        // Parse - null input
        assert.throws(
            () => CIDRBlock._parseCIDR(null!),
            (err: AntiSSRFError) => err.message === "Null argument"
        );

        // Parse - too many /
        assert.throws(
            () => CIDRBlock._parseCIDR("192.168.1.0/24/16"),
            (err: AntiSSRFError) => err.message.includes("Invalid CIDR block")
        );
        assert.throws(
            () => CIDRBlock._parseCIDR("10.0.0.0/8/"),
            (err: AntiSSRFError) => err.message.includes("Invalid CIDR block")
        );
        assert.throws(
            () => CIDRBlock._parseCIDR("2001:db8::/32/64"),
            (err: AntiSSRFError) => err.message.includes("Invalid CIDR block")
        );

        // Parse - invalid IP address
        assert.throws(
            () => CIDRBlock._parseCIDR("256.256.256.256/24"),
            (err: AntiSSRFError) => err.message.includes("Invalid CIDR block")
        );
        assert.throws(
            () => CIDRBlock._parseCIDR("192.168.1.300/24"),
            (err: AntiSSRFError) => err.message.includes("Invalid CIDR block")
        );
        assert.throws(
            () => CIDRBlock._parseCIDR("not-an-ip/24"),
            (err: AntiSSRFError) => err.message.includes("Invalid CIDR block")
        );
        assert.throws(
            () => CIDRBlock._parseCIDR("999.999.999.999"),
            (err: AntiSSRFError) => err.message.includes("Invalid CIDR block")
        );
        assert.throws(
            () => CIDRBlock._parseCIDR("gggg::1/64"),
            (err: AntiSSRFError) => err.message.includes("Invalid CIDR block")
        );

        // Parse - invalid prefix format
        assert.throws(
            () => CIDRBlock._parseCIDR("192.168.1.0/abc"),
            (err: AntiSSRFError) => err.message.includes("Invalid CIDR block")
        );
        assert.throws(
            () => CIDRBlock._parseCIDR("192.168.1.0/24.5"),
            (err: AntiSSRFError) => err.message.includes("Invalid CIDR block")
        );
        assert.throws(
            () => CIDRBlock._parseCIDR("192.168.1.0/"),
            (err: AntiSSRFError) => err.message.includes("Invalid CIDR block")
        );
        assert.throws(
            () => CIDRBlock._parseCIDR("/24"),
            (err: AntiSSRFError) => err.message.includes("Invalid CIDR block")
        );
        assert.throws(
            () => CIDRBlock._parseCIDR("192.168.1.0/+24"),
            (err: AntiSSRFError) => err.message.includes("Invalid CIDR block")
        );
        assert.throws(
            () => CIDRBlock._parseCIDR("127.0.0.0/024"),
            (err: AntiSSRFError) => err.message.includes("Invalid CIDR block")
        );

        // Parse - invalid prefix length
        assert.throws(
            () => CIDRBlock._parseCIDR("192.168.1.0/33"),
            (err: AntiSSRFError) => err.message.includes("Invalid CIDR block")
        );
        assert.throws(
            () => CIDRBlock._parseCIDR("192.168.1.0/-1"),
            (err: AntiSSRFError) => err.message.includes("Invalid CIDR block")
        );
        assert.throws(
            () => CIDRBlock._parseCIDR("192.168.1.0/255"),
            (err: AntiSSRFError) => err.message.includes("Invalid CIDR block")
        );
        assert.throws(
            () => CIDRBlock._parseCIDR("2001:db8::/129"),
            (err: AntiSSRFError) => err.message.includes("Invalid CIDR block")
        );
        assert.throws(
            () => CIDRBlock._parseCIDR("2001:db8::/-5"),
            (err: AntiSSRFError) => err.message.includes("Invalid CIDR block")
        );
    });

    it("contains ipv4 returns expected result", () => {
        // Standard decimal format
        const block1 = CIDRBlock._parseCIDR("192.168.1.0/24");
        const denyList1 = new BlockList();
        denyList1.addSubnet(block1.getAddress(), block1.getPrefix(), "ipv6");
        assert.equal(denyList1.check(toParsedAddress("192.168.1.1"), "ipv6"), true);
        assert.equal(denyList1.check(toParsedAddress("::ffff:192.168.1.255"), "ipv6"), true);
        assert.equal(denyList1.check(toParsedAddress("192.168.2.1"), "ipv6"), false);

        // Test without prefix length (defaults to /32)
        const block2 = CIDRBlock._parseCIDR("127.0.0.1");
        const denyList2 = new BlockList();
        denyList2.addSubnet(block2.getAddress(), block2.getPrefix(), "ipv6");
        assert.equal(denyList2.check(toParsedAddress("127.0.0.1"), "ipv6"), true);
        assert.equal(denyList2.check(toParsedAddress("::ffff:127.0.0.2"), "ipv6"), false);

        // Node.js parsing is strict dotted-quad for IPv4, so other C# formats are invalid.
        assert.throws(
            () => CIDRBlock._parseCIDR("0300.0250.001.000/24"),
            (err: AntiSSRFError) => err.message.includes("Invalid CIDR block")
        );
        assert.throws(
            () => CIDRBlock._parseCIDR("0xC0.0xA8.0x1.0x0/24"),
            (err: AntiSSRFError) => err.message.includes("Invalid CIDR block")
        );
        assert.throws(
            () => CIDRBlock._parseCIDR("192.0250.1.0x0/24"),
            (err: AntiSSRFError) => err.message.includes("Invalid CIDR block")
        );
        assert.throws(
            () => CIDRBlock._parseCIDR("192.168.256/24"),
            (err: AntiSSRFError) => err.message.includes("Invalid CIDR block")
        );
        assert.throws(
            () => CIDRBlock._parseCIDR("192.11010304/24"),
            (err: AntiSSRFError) => err.message.includes("Invalid CIDR block")
        );
        assert.throws(
            () => CIDRBlock._parseCIDR("3232235776/24"),
            (err: AntiSSRFError) => err.message.includes("Invalid CIDR block")
        );
        assert.throws(
            () => CIDRBlock._parseCIDR("0xC0A80101"),
            (err: AntiSSRFError) => err.message.includes("Invalid CIDR block")
        );
        assert.throws(
            () => CIDRBlock._parseCIDR("192.168.257"),
            (err: AntiSSRFError) => err.message.includes("Invalid CIDR block")
        );
    });

    it("contains ipv6 returns expected result", () => {
        // Standard full format
        const block1 = CIDRBlock._parseCIDR("2001:0db8:0000:0000:0000:0000:0000:0000/32");
        const denyList1 = new BlockList();
        denyList1.addSubnet(block1.getAddress(), block1.getPrefix(), "ipv6");
        assert.equal(denyList1.check(toParsedAddress("2001:db8::1"), "ipv6"), true);
        assert.equal(denyList1.check(toParsedAddress("2001:db8:ffff::1"), "ipv6"), true);
        assert.equal(denyList1.check(toParsedAddress("2001:db9::1"), "ipv6"), false);

        // Leading compression
        const block3 = CIDRBlock._parseCIDR("::1/128");
        const denyList3 = new BlockList();
        denyList3.addSubnet(block3.getAddress(), block3.getPrefix(), "ipv6");
        assert.equal(denyList3.check(toParsedAddress("::1"), "ipv6"), true);
        assert.equal(denyList3.check(toParsedAddress("::2"), "ipv6"), false);

        // Trailing compression
        const block4 = CIDRBlock._parseCIDR("2001:db8:1::/48");
        const denyList4 = new BlockList();
        denyList4.addSubnet(block4.getAddress(), block4.getPrefix(), "ipv6");
        assert.equal(denyList4.check(toParsedAddress("2001:db8:1::1"), "ipv6"), true);
        assert.equal(denyList4.check(toParsedAddress("2001:db8:1:ffff::1"), "ipv6"), true);
        assert.equal(denyList4.check(toParsedAddress("2001:db8:2::1"), "ipv6"), false);

        // Middle compression
        const block5 = CIDRBlock._parseCIDR("2001:db8::1:0:0:1/64");
        const denyList5 = new BlockList();
        denyList5.addSubnet(block5.getAddress(), block5.getPrefix(), "ipv6");
        assert.equal(denyList5.check(toParsedAddress("2001:db8::1"), "ipv6"), true);
        assert.equal(denyList5.check(toParsedAddress("2001:db8:0:0:ffff::"), "ipv6"), true);
        assert.equal(denyList5.check(toParsedAddress("2001:db9::1"), "ipv6"), false);

        // ::<ipv4> format
        const block6 = CIDRBlock._parseCIDR("::ffff:192.168.1.0/120");
        const denyList6 = new BlockList();
        denyList6.addSubnet(block6.getAddress(), block6.getPrefix(), "ipv6");
        assert.equal(denyList6.check(toParsedAddress("192.168.1.1"), "ipv6"), true);
        assert.equal(denyList6.check(toParsedAddress("::ffff:192.168.1.255"), "ipv6"), true);
        assert.equal(denyList6.check(toParsedAddress("::ffff:192.168.2.1"), "ipv6"), false);

        // Mixed case hex digits
        const block8 = CIDRBlock._parseCIDR("2001:DB8:aBCD:Ef01::/64");
        const denyList8 = new BlockList();
        denyList8.addSubnet(block8.getAddress(), block8.getPrefix(), "ipv6");
        assert.equal(denyList8.check(toParsedAddress("2001:db8:abcd:ef01::1"), "ipv6"), true);
        assert.equal(denyList8.check(toParsedAddress("2001:db8:abcd:ef02::1"), "ipv6"), false);

        // Test without prefix length (defaults to /128)
        const block9 = CIDRBlock._parseCIDR("2001:db8::1");
        const denyList9 = new BlockList();
        denyList9.addSubnet(block9.getAddress(), block9.getPrefix(), "ipv6");
        assert.equal(denyList9.check(toParsedAddress("2001:db8::1"), "ipv6"), true);
        assert.equal(denyList9.check(toParsedAddress("2001:db8::2"), "ipv6"), false);

        const block10 = CIDRBlock._parseCIDR("::1");
        const denyList10 = new BlockList();
        denyList10.addSubnet(block10.getAddress(), block10.getPrefix(), "ipv6");
        assert.equal(denyList10.check(toParsedAddress("::1"), "ipv6"), true);
        assert.equal(denyList10.check(toParsedAddress("::2"), "ipv6"), false);

        const block11 = CIDRBlock._parseCIDR("::ffff:192.168.1.1");
        const denyList11 = new BlockList();
        denyList11.addSubnet(block11.getAddress(), block11.getPrefix(), "ipv6");
        assert.equal(denyList11.check(toParsedAddress("192.168.1.1"), "ipv6"), true);
        assert.equal(denyList11.check(toParsedAddress("::ffff:192.168.1.2"), "ipv6"), false);
    });

    it("contains ipv6 with scope returns expected result", () => {
        // Scoped addresses should have their scope stripped and still match if the address is in the block.
        const block1 = CIDRBlock._parseCIDR("fe80::/10");
        const denyList1 = new BlockList();
        denyList1.addSubnet(block1.getAddress(), block1.getPrefix(), "ipv6");
        assert.equal(denyList1.check(toParsedAddress("fe80::1%eth0"), "ipv6"), true);
        assert.equal(denyList1.check(toParsedAddress("fe80::1%1"), "ipv6"), true);
        assert.equal(denyList1.check(toParsedAddress("2001:db8::1%eth0"), "ipv6"), false);
    });
});
