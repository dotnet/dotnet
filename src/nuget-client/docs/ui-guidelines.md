
# NuGet.Client UI Guidelines

## Overview
Our graphical user interface (GUI, or commonly abbreviated as UI) components follow a special set of compliance standards. Changes to these components will be evaluated by the product team to ensure we can ship them in the next Visual Studio (VS) release without violations. The goal of this guide is to clarify expectations for, and our feedback on, any pull-request which modifies our UI components, namely our VSIX which ships in VS.

If we find issues, the NuGet team will work with the pull-request author to resolve the issues.

## Globalization

### Localizability Testing

Changes to localized resources or modifying UI elements which contain them means we must perform localizability testing to ensure our UI displays correctly in various languages. We follow the guidance in the Microsoft Localizability Testing document. For more information, see [Testing localizability](https://learn.microsoft.com/globalization/testing/localizability-testing).

A member of the product team will be able to help in testing of any changes affecting localizability. If you perform any testing on your own, please mention this in your pull-request (eg, indicate your methodology and/or provide screenshots). 

### Pseudolocalization

Our methodology for testing changes to localization in our UI components is with a technique called pseudolocalization (PLOC). For more information, see [Pseudolocalization](https://learn.microsoft.com/globalization/methodology/pseudolocalization).

## Accessibility

## Custom Controls 

There are occasions where WPF, .NET framework, or VS platform behavior requires custom control modifications to be compliant with accessibility.
The following table lists controls and a basic use case for each one.

|Custom Control|Criteria|Reason|
|---|---|---|
|`ButtonHyperlink`|A `Hyperlink` which doesn't function by opening an external URL (web page), but instead performs some action within VS.|Assistive technologies read the control type, and a `Hyperlink` is expected to behave in a way consistent with Web page hyperlinks. The customization sets the control type as a `Button`. |

## Accessibility Testing

#### Accessibility Insights

We use the [Accessibility Insights For Windows](https://accessibilityinsights.io) tool to validate our UI meets accessibility standards. Although no tool catches every accessibility problem, it provides a baseline of how the UI change may affect assistive technologies.

#### FastPass

The [Accessibility Insights FastPass](https://accessibilityinsights.io/docs/windows/getstarted/fastpass/) needs to be run on the parent control being affected. A screenshot of the result can be placed in the PR description. 

#### Screen-reader Testing

Generally, Accessibility Insights will catch most issues, but sometimes a quick test actually using a screen-reader reveals more.
There can be subtleties where a particular screen-reader behaves differently in the scenario and needs special treatment.

The screen-readers we use for testing are [JAWS](https://www.freedomscientific.com/products/software/jaws/), [NVDA](https://www.nvaccess.org/download/), and [Windows Narrator](https://support.microsoft.com/windows/complete-guide-to-narrator-e4397a0d-ef4f-b386-d8ae-c172f109bdb1).
Ensuring that each screen-reader is able to announce all visible controls and text is ideal. 

## Responsive Testing

When developing new UI we need to make sure that it's responsive for all resolutions and window sizes. We can test that by changing the Display resolution and Scale of our display. Please add proof of this in your pull-requests.
