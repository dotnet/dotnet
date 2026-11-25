// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

import { domainToASCII } from "url";

import { _azureKeyVaultDomains, _azureStorageDomains } from "./Helpers/Domains";
import { AntiSSRFError } from "./AntiSSRFError";

export class URIValidator {
    private static _domainProtocols = ["http:", "https:", "ws:", "wss:"];
    private static _azureSdkProtocols = ["http:", "https:"];

    private static _getValidHostname(address: string | URL, allowedProtocols: string[]): string {
        let url: URL;
        if (typeof address == "string") {
            url = new URL(address);
        } else {
            url = address;
        }

        if (url.hostname == null || url.hostname == "") {
            throw new AntiSSRFError();
        }

        if (!allowedProtocols.includes(url.protocol)) {
            throw new AntiSSRFError();
        }

        return url.hostname;
    }

    private static _hostnameInSingleDomain(parsedHostname: string, parsedDomain: string): boolean {
        if (("." + parsedHostname).endsWith(parsedDomain)) {
            if (parsedHostname.length == parsedDomain.length) {
                return true;
            }

            if (parsedDomain[0] == ".") {
                return true;
            }

            if (parsedHostname.length > parsedDomain.length
                && parsedHostname[parsedHostname.length - parsedDomain.length - 1] == ".") {
                return true;
            }
        }
        return false;
    }

    /**
     * Verifies if an address is in any of the provided domains.
     *
     * @param untrustedAddress The address to verify
     * @param trustedDomain The domain or domains to verify against
     * @return True if address is in any provided domain, false otherwise.
     */
    public static inDomain(untrustedAddress: string | URL, trustedDomain: string | string[]): boolean {
        if (untrustedAddress == null || trustedDomain == null) {
            return false;
        }

        // Standardize untrustedAddress
        let validHostname: string;
        try {
            validHostname = this._getValidHostname(untrustedAddress, this._domainProtocols);
        } catch {
            return false;
        }

        // Standardize trustedDomain and remove invalid domains
        let parsedDomains: string[];
        if (typeof trustedDomain == "string") {
            parsedDomains = [domainToASCII(trustedDomain)];
        } else {
            parsedDomains = trustedDomain.map((d) => domainToASCII(d));
        }
        if (parsedDomains.filter((d) => d == null || d.length == 0).length > 0) {
            return false;
        }

        // Check if the hostname is in any of the provided domains
        for (const parsedDomain of parsedDomains) {
            if (this._hostnameInSingleDomain(validHostname, parsedDomain)) {
                return true;
            }
        }
        return false;
    }

    /**
     * Verifies if an address is in an Azure Key Vault domain.
     *
     * @param untrustedAddress The address to verify
     * @return True if address is in any Azure Key Vault domain, false otherwise.
     */
    public static inAzureKeyVaultDomain(untrustedAddress: string | URL): boolean {
        if (untrustedAddress == null) {
            return false;
        }

        let validHostname: string;
        try {
            validHostname = this._getValidHostname(untrustedAddress, this._azureSdkProtocols);
        } catch {
            return false;
        }

        if (validHostname.includes("--")) {
            return false;
        }


        for (const domain of _azureKeyVaultDomains) {
            if (this._hostnameInSingleDomain(validHostname, domain)) {
                return true;
            }
        }
        return false;
    }

    /**
     * Verifies if an address is in an Azure Storage domain.
     *
     * @param untrustedAddress The address to verify
     * @return True if address is in any Azure Storage domain, false otherwise.
     */
    public static inAzureStorageDomain(untrustedAddress: URL | string): boolean {
        if (untrustedAddress == null) {
            return false;
        }

        let validHostname: string;
        try {
            validHostname = this._getValidHostname(untrustedAddress, this._azureSdkProtocols);
        } catch {
            return false;
        }

        if (validHostname.includes("--")) {
            return false;
        }

        for (const domain of _azureStorageDomains) {
            if (this._hostnameInSingleDomain(validHostname, domain)) {
                return true;
            }
        }
        return false;
    }
}
