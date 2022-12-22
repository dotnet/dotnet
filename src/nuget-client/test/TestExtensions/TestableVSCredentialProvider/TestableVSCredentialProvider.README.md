# TestableVSCredentialProvider

## Overview

On NuGet VSIX startup in Visual Studio, NuGet will search for any MEF exports of IVsCredentialProvider,
and add them to its internal list of credential providers to consult for credentials when a package 
source fails with an HTTP 401 response.

This project generates a VSIX that exports two test credential providers, to help with validating 
scenarios in which multiple credential providers are present.

By default, each of these test credential providers will trace calls to it, and by default respond 
with null credentials, indicating that it does not own providing credential for the given uri. Testers 
can verify that these providers have been called by configuring tracing in Visual Studio.

## Configuring test responses

A test provider can also be configured to return credentials for a given package source by appending 
query string parameters to the package source url.

| Credential Provider     | Query String                             | Notes                                                                                          |
| ----------------------- | ---------------------------------------- | ---------------------------------------------------------------------------------------------- |
| TestCredentialProvider  | testCredentialProvider-responseUser      | used to set the username of the credential returned by TestCredentialProvider for this source  |
| TestCredentialProvider  | testCredentialProvider-responsePassword  | used to set the password of the credential returned by TestCredentialProvider for this source  |
| TestCredentialProvider2 | testCredentialProvider2-responseUser     | used to set the username of the credential returned by TestCredentialProvider2 for this source |
| TestCredentialProvider2 | testCredentialProvider2-responsePassword | used to set the username of the credential returned by TestCredentialProvider2 for this source |

Example Package Source:

https://www.myget.org/F/my-secure-feed/api/v3/index.json?testCredentialProvider2-responseUser=user1&testCredentialProvider2-responsePassword=Password1

In this example, TestCredentialProvider will return null credentials, and TestCredentialProvider2
will return {"username": "user1", "password": "Password1"} credentials.

## Installation

On build, both a v14 and v15 VSIX will be output. Install the appropriate VSIX in visual studio alongside
the NuGet VSIX in order to use the test credential providers.
