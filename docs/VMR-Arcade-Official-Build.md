# Arcade Official Build in the time of VMR - One Pager

## Introduction
This document outlines the plan and decisions made regarding the official build and validation process for Arcade. The purpose is to ensure that Arcade continues to support both VMR (Virtual Mono Repository) and non-VMR consumers effectively while maintaining the stability and reliability of the .NET ecosystem.

## Plan Summary
- **Arcade will keep its official build process.**
- **Arcade's validation process will remain as it is, ensuring the integrity of the publishing process and some minimal verification.**
- **Potential future enhancement to include VMR validation for Arcade changes, to be implemented reactively.**

## Discussion Points
- The main discussion revolved around whether Arcade should retain its official build or be produced by the VMR (Virtual Mono Repository).
- Concerns were raised about the validation process, the impact on consumers both within and outside the VMR, and the potential complications of breaking changes.
- The current model of Arcade validation and its importance in ensuring the integrity of the publishing process was highlighted.

## Key Decisions
- **Arcade Official Build:** Arcade will retain its official build process. This decision is based on the need to support consumers outside the VMR and to avoid potential complications with the VMR schedule.
- **Arcade Validation:** The existing Arcade validation build will be maintained. This includes the validation of publishing processes and ensuring that changes do not break the overall .NET ecosystem.
- **Future Validation Enhancements:** There is a plan to potentially introduce additional validation by running full VMR builds against code changes in Arcade. This will be implemented reactively, based on the occurrence of issues, rather than proactively.

## Next Steps
- **Monitoring:** The team will monitor the situation and be prepared to implement additional VMR validation if frequent issues arise with Arcade updates.
