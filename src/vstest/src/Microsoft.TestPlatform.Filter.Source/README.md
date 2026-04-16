# Microsoft.TestPlatform.Filter.Source

Provides source code for filter implementation. This package is a development dependency.

- The source code in this package has some code under IS_VSTEST_REPO conditions. These are intended to be used only by VSTest code base itself. Consumers of this package MUST NOT define IS_VSTEST_REPO.
- The only intended supported usage of this package is the following:

    ```csharp
    var wrapper = new FilterExpressionWrapper(vstestFilterString);
    var expression = new TestCaseFilterExpression(wrapper);

    bool match = expression.MatchTestCase(propertyProviderGoesHere);
    ```
