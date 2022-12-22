# The Unified Build Almanac (TUBA) - Roadmap

This is a rough roadmap of the Unified Build rollout. It will be updated as more information becomes available.

The implementation cost of Unified Build is very large. This is the largest change in how we develop, build and deliver the product in the history of .NET. It will take two releases to deliver. The implementation is designed with the following priorities, in order of importance.

### **Priorities*

1. Don’t break or remove the existing product build and developer workflows until we must.
2. Avoid increasing complexity at intermediate stages.
3. Deliver value at every major phase of implementation.
4. Servicing should get easier as soon as possible.

***Note: Work items slipping from .NET 8 into .NET 9 will not affect .NET 8 shipping. The plan is designed to bring incremental benefit over time while avoiding putting at risk our current build/release methodologies.***

## **.NET 8 Milestones**

- Improve current source-build infrastructure to support sustainability and reduce cost.
- Design and create E2E testing against installed products. Prioritize creation of tests that benefit our source-build partners first.
- Enable an experimental macOS source-build variant.
- Enable Linux portable source-build.
- Redesign .NET’s build to reduce complexity and align it with “vertical” requirements for source-build.
- Deliver .NET source-build to Linux partners via a “[VMR-lite](Terminology.md)” repo.

## **.NET 9 Milestones (targeting .NET //Build)**

- Enable .NET repo tests to run against full source-build.
- Build infrastructure to support full VMR source-code flow (forward and backward to individual repos).
- Expand source-build to support Windows and macOS (officially).
- Turn off existing official build.