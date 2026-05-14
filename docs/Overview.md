# The Unified Build Almanac (TUBA) - Overview

Unified Build for .NET is a combined effort to improve two problematic areas for .NET:
- **True upstream model** – Partners are efficient and can do the same things Microsoft can.
- **Product build and servicing complexity** – Taming .NET's incredibly complex infrastructure.

## Goals

- Predictable builds, faster builds, simpler builds
- Reduce long-term servicing complexity and risk.
- Enable any organization to build, test, deploy, and contribute to .NET in a reasonable manner.
- Enable effective cross-stack development.
- Align .NET’s organizational collaboration model with industry standard upstream/downstream practices.

## High-Level Overview

At a high level, the following changes will be made:

### **Source-build becomes the official build**

Source-build builds the product as an aggregate codebase, which is a better model. It is now proven and has been adopted by partners. Microsoft’s current official build will cease to exist (for new .NET versions). Logically, this means that the .NET product will be built as a series of **vertical builds** for each platform. Assets produced by these verticals will be shipped by Microsoft and its partners (Linux distros, homebrew developers, etc.). There will also exist a join point that will be used to produce assets that require multiple platforms (e.g. global tool packages). This join point may only be used for assets that are not distributed by our partners, and must remain as simple as possible.

### **Product builds move to a “virtual monolithic repository” (VMR)**

.NET's official code base moves to a “virtual monolithic repository”, which is a source layout of all the code needed to build .NET. Most existing dependency flow between repositories ceases, and instead the product builds out of this single coherent source repository. Source changes in individual repositories flow into the VMR, and product build outputs and any associated VMR source changes flow *back* into individual repositories to enable up-to-date development workflows.

### **Testing will be augmented and extended for the VMR**

The testing infrastructure will be augmented to support both individual repos *and* the aggregate VMR. Additional tests will be added for end to end product testing. This will enable Microsoft and its source-build partners to more effectively evaluate quality.

### **Upstream partners primarily focus on the VMR structure**

The VMR focused build enables partners to build and test .NET more easily. This should enable them to more effectively contribute when desired. They of may contribute to either the VMR or individual repos.

### **Individual developers continue to work primarily in individual repositories for mainline development**

Though they may work in the VMR, it is expected that many developers will continue to primarily work in the individual repositories for mainline development, especially those developers who focus on specfic areas of .NET. Maintaining the individual repository workflows enables high-velocity development for changes in isolated components. It also allows for continued community building in specific product areas. Cross-repo breaking changes will be resolved in the VMR, then build outputs and additional source changes will flow back into the individual repositories.

## Roadmap

Please see [Roadmap](Roadmap.md)
