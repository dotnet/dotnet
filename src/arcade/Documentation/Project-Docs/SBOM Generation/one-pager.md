## Generating the Software Bill of Material (SBOM)

## SBOM
The Executive Order(EO) and The National Telecommunications & Information Administration (NTIA) report defines an SBOM as a formal record containing the details and supply chain relationships of various components used in building software. On May 12, 2021, the U.S. Presidential EO, section 4(e)(vii) is requiring all software sold to the federal government to provide a Software Bill of Material (SBOM). 

SBOM is usually a single file (such as .json) that captures this information about the software from the build. Microsoft has decided to use Software Package Data Exchange (SPDX) as its SBOM format of choice. All software produced from Microsoft will have an SPDX SBOM.

SBOMs provide two core benefits:

i) Software Transparency - this is a small step towards increasing trust as the SBOM describes the "ingredients" of the software and their relationships. This also enables external consumers of SBOMs to do vulnerability lookups on the open source software embedded within.
ii) File checksums for integrity verification purposes

## Goals
Primary goal is to generate SBOM for all the software produced by .Net. Here we are focusing on the following areas:

i) Staging pipeline 
ii) Arcade and all the repos that use arcade eg: Runtime, aspnetcore etc.
iii) Repos that are not on-boarded to arcade eg: arcade-services, OSOB, helix etc.

## Stakeholders
- .NET Core Engineering
- .NET Core Engingeering Partners
- Microsoft

## Unknowns 
There are 2 ways to generate SBOM 
1) Azure Task - Helps with generation of SBOM and uploads it to db
2) Executable - Creates the manifest but uploading is TBD

## Rollout and Deployment
- Firstly we will be generating SBOM for staging pipeline. Here we already have a place where we upload all the signed assets, so we will need to add a azure task to generate and upload the SBOM. After generating SBOM, we will need to get a one time manual sign off from partners to see if the generated SBOM is valid and contains all the 'expected' items.
- Then focus on our Engineering systems - In Arcade (main branch) we are planning to use the executable to generate SBOM. Here we will validate if SBOM is generated correctly. In Arcade we will add a feature flag for SBOM generation. We will initially turn on this feature for a few repos and see if SBOM is getting generated correctly, then roll out for all the other repos. This gives repo owners the ability to opt-out of the feature incase of failure, while we investigate.
- Backport SBOM generation changes to release/6.0 branch.
- The repos that use arcade may have multiple places where will have to generate SBOM. We will need to generate SBOM for all the repos that use arcade.There might be multiple SBOM in this scenario. Then we need to get sign off from the repo owners to validate SBOM.
- Lastly, we will have to work on repos that are not on-boarded to arcade like arcade-service, helix, OSOB. 

## FR handoff 
- Will document SBOM generation in arcade and how repo owners can on-board. 
- Will document any failures as I encounter. 


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CProject-Docs%5CSBOM%20Generation%5Cone-pager.md)](https://helix.dot.net/f/p/5?p=Documentation%5CProject-Docs%5CSBOM%20Generation%5Cone-pager.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CProject-Docs%5CSBOM%20Generation%5Cone-pager.md)</sub>
<!-- End Generated Content-->
