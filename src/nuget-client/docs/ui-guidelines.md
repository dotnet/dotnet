
# NuGet.Client UI Guidelines

## Overview
Our graphical user interface (GUI, or commonly abbreviated as UI) components follow a special set of compliance standards. Changes to these components will be evaluated by the product team to ensure we can ship them in the next Visual Studio (VS) release without violations. The goal of this guide is to clarify expectations for, and our feedback on, any pull-request which modifies our UI components, namely our VSIX which ships in VS.

## Globalization

### Localizability Testing

Changes to localized resources or modifying UI elements which contain them means we must perform localizability testing to ensure our UI displays correctly in various languages. We follow the guidance in the Microsoft Localizability Testing document. For more information, see https://learn.microsoft.com/en-us/globalization/testing/localizability-testing.

A member of the product team will be able to help in testing of any changes affecting localizability. If you perform any testing on your own, please mention this in your pull-request (eg, indicate your methodology and/or provide screenshots). 

If we find issues, we'll work with the pull-request author to resolve the issues. 

### Pseudolocalization

Our methodology for testing changes to localization in our UI components is with a technique called pseudolocalization (PLOC). For more information, see https://learn.microsoft.com/en-us/globalization/methodology/pseudolocalization.