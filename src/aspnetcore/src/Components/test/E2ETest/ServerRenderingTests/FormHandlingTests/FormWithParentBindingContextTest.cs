// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.Net.Http;
using Components.TestServer.RazorComponents;
using Microsoft.AspNetCore.Components.E2ETest.Infrastructure;
using Microsoft.AspNetCore.Components.E2ETest.Infrastructure.ServerFixtures;
using Microsoft.AspNetCore.E2ETesting;
using OpenQA.Selenium;
using TestServer;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Components.E2ETests.ServerRenderingTests.FormHandlingTests;

public class FormWithParentBindingContextTest : ServerTestBase<BasicTestAppServerSiteFixture<RazorComponentEndpointsStartup<App>>>
{
    public FormWithParentBindingContextTest(
        BrowserFixture browserFixture,
        BasicTestAppServerSiteFixture<RazorComponentEndpointsStartup<App>> serverFixture,
        ITestOutputHelper output)
        : base(browserFixture, serverFixture, output)
    {
    }

    public override Task InitializeAsync()
        => InitializeAsync(BrowserFixture.StreamingContext);

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void CanDispatchToTheDefaultForm(bool suppressEnhancedNavigation)
    {
        var dispatchToForm = new DispatchToForm(this)
        {
            Url = "forms/default-form",
            FormCssSelector = "form",
            SuppressEnhancedNavigation = suppressEnhancedNavigation,
        };
        DispatchToFormCore(dispatchToForm);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void CanBindParameterToTheDefaultForm(bool suppressEnhancedNavigation)
    {
        var dispatchToForm = new DispatchToForm(this)
        {
            Url = "forms/default-form-bound-parameter",
            FormCssSelector = "form",
            InputFieldId = "Parameter",
            InputFieldCssSelector = "input[name=Parameter]",
            InputFieldValue = "stranger",
            SuppressEnhancedNavigation = suppressEnhancedNavigation,
        };
        DispatchToFormCore(dispatchToForm);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void MultipleParametersMultipleFormsDoNotConflict(bool suppressEnhancedNavigation)
    {
        var dispatchToForm = new DispatchToForm(this)
        {
            Url = "forms/multiple-forms-bound-parameter-no-conflicts",
            FormCssSelector = "form[name=bind-integer]",
            ExpectedHandlerValue = "bind-integer",
            InputFieldId = "Id",
            InputFieldCssSelector = "form[name=bind-integer] input[name=Id]",
            InputFieldValue = "abc",
            AssertErrors = errors =>
            {
                var error = Assert.Single(errors);
                Assert.Equal("The value 'abc' is not valid for 'Id'.", error.Text);
                Assert.Equal("abc", Browser.FindElement(By.CssSelector("form[name=bind-integer] input[name=Id]")).GetAttribute("value"));
            },
            SuppressEnhancedNavigation = suppressEnhancedNavigation,
        };

        DispatchToFormCore(dispatchToForm);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void MultipleParametersMultipleFormsBindsToCorrectForm(bool suppressEnhancedNavigation)
    {
        var guid = "02385a44-9ea2-4af8-9d82-7a278f71a50c";
        var dispatchToForm = new DispatchToForm(this)
        {
            Url = "forms/multiple-forms-bound-parameter-no-conflicts",
            FormCssSelector = "form[name=bind-guid]",
            ExpectedHandlerValue = "bind-guid",
            SubmitButtonId = "send-guid",
            UpdateFormAction = () =>
            {
                var criteria = By.CssSelector("form[name=bind-guid] input[name=Id]");

                Browser.Exists(criteria).Clear();
                Browser.Exists(criteria).SendKeys(guid);
            },
            SuppressEnhancedNavigation = suppressEnhancedNavigation,
        };

        DispatchToFormCore(dispatchToForm);
        Browser.Contains(guid, () => Browser.Exists(By.Id("pass-guid")).Text);
        Browser.DoesNotExist(By.Id("errors"));
        Browser.DoesNotExist(By.Id("pass"));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void CanBindMultipleParametersToTheDefaultForm(bool suppressEnhancedNavigation)
    {
        var dispatchToForm = new DispatchToForm(this)
        {
            Url = "forms/default-form-bound-multiple-primitive-parameters",
            FormCssSelector = "form",
            UpdateFormAction = () =>
            {
                Browser.Exists(By.CssSelector("input[name=Parameter]")).Clear();
                Browser.Exists(By.CssSelector("input[name=Parameter]")).SendKeys("10");

                Browser.Exists(By.CssSelector("input[name=OtherParameter]")).Clear();
                Browser.Exists(By.CssSelector("input[name=OtherParameter]")).SendKeys("true");
            },
            SuppressEnhancedNavigation = suppressEnhancedNavigation,
        };
        DispatchToFormCore(dispatchToForm);

        Browser.Exists(By.Id("ParameterValue")).Text.Contains("10");
        Browser.Exists(By.Id("OtherParameterValue")).Text.Contains("True");
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void CanChangeFormParameterNames(bool suppressEnhancedNavigation)
    {
        var dispatchToForm = new DispatchToForm(this)
        {
            Url = "forms/default-form-bound-multiple-primitive-parameters-changed-names",
            FormCssSelector = "form",
            UpdateFormAction = () =>
            {
                Browser.Exists(By.CssSelector("input[name=UpdatedParameter]")).Clear();
                Browser.Exists(By.CssSelector("input[name=UpdatedParameter]")).SendKeys("10");

                Browser.Exists(By.CssSelector("input[name=UpdatedOtherParameter]")).Clear();
                Browser.Exists(By.CssSelector("input[name=UpdatedOtherParameter]")).SendKeys("true");
            },
            SuppressEnhancedNavigation = suppressEnhancedNavigation,
        };
        DispatchToFormCore(dispatchToForm);

        Browser.Exists(By.Id("ParameterValue")).Text.Contains("10");
        Browser.Exists(By.Id("OtherParameterValue")).Text.Contains("True");
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void CanDisplayErrorsFromMultipleParametersToTheDefaultForm(bool suppressEnhancedNavigation)
    {
        var dispatchToForm = new DispatchToForm(this)
        {
            Url = "forms/default-form-bound-multiple-primitive-parameters",
            FormCssSelector = "form",
            UpdateFormAction = () =>
            {
                Browser.Exists(By.CssSelector("input[name=Parameter]")).Clear();
                Browser.Exists(By.CssSelector("input[name=Parameter]")).SendKeys("abcd");

                Browser.Exists(By.CssSelector("input[name=OtherParameter]")).Clear();
                Browser.Exists(By.CssSelector("input[name=OtherParameter]")).SendKeys("invalid");
            },
            AssertErrors = errors =>
            {
                Assert.Collection(
                    errors,
                    error =>
                    {
                        Assert.Equal("The value 'abcd' is not valid for 'Parameter'.", error.Text);
                    },
                    error =>
                    {
                        Assert.Equal("The value 'invalid' is not valid for 'OtherParameter'.", error.Text);
                    });
                Assert.Equal("abcd", Browser.FindElement(By.CssSelector("input[name=Parameter]")).GetAttribute("value"));
                Assert.Equal("invalid", Browser.FindElement(By.CssSelector("input[name=OtherParameter]")).GetAttribute("value"));
            },
            SuppressEnhancedNavigation = suppressEnhancedNavigation,
        };
        DispatchToFormCore(dispatchToForm);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void CanHandleBindingErrorsBindParameterToTheDefaultForm(bool suppressEnhancedNavigation)
    {
        var dispatchToForm = new DispatchToForm(this)
        {
            Url = "forms/default-form-bound-primitive-parameter",
            FormCssSelector = "form",
            InputFieldId = "Parameter",
            InputFieldCssSelector = "input[name=Parameter]",
            InputFieldValue = "abc",
            SuppressEnhancedNavigation = suppressEnhancedNavigation,
            AssertErrors = errors =>
            {
                Assert.Collection(
                    errors,
                    error =>
                    {
                        Assert.Equal("The value 'abc' is not valid for 'Parameter'.", error.Text);
                    });
                Assert.Equal("abc", Browser.FindElement(By.CssSelector("input[name=Parameter]")).GetAttribute("value"));
            }
        };
        DispatchToFormCore(dispatchToForm);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void CanBindComplexTypeToDefaultForm(bool suppressEnhancedNavigation)
    {
        var url = "forms/default-form-bound-complextype-parameter";
        var expectedTarget = GetExpectedTarget(this, null, url);

        if (suppressEnhancedNavigation)
        {
            GoTo("");
            Browser.Equal("Hello", () => Browser.Exists(By.TagName("h1")).Text);
            ((IJavaScriptExecutor)Browser).ExecuteScript("sessionStorage.setItem('suppress-enhanced-navigation', 'true')");
        }

        GoTo(url);

        Browser.Exists(By.Id("ready"));
        var form = Browser.Exists(By.CssSelector("form"));
        var formTarget = form.GetAttribute("action");
        var actionValue = form.GetDomAttribute("action");
        Assert.Equal(expectedTarget, formTarget);
        Assert.Null(actionValue);

        var name = Browser.Exists(By.CssSelector("""input[name="Model.Name"]"""));
        name.SendKeys("John");
        var email = Browser.Exists(By.CssSelector("""input[name="Model.Email"]"""));
        email.SendKeys("john@example.com");
        Browser.Click(By.CssSelector("""input[name="Model.IsPreferred"]"""));
        Browser.Click(By.Id("send"));

        Browser.Exists(By.Id("name")).Text.Contains("John");
        Browser.Exists(By.Id("email")).Text.Contains("john@example.com");
        Browser.Exists(By.Id("preferred")).Text.Contains("True");

        if (!suppressEnhancedNavigation)
        {
            // Verify the same form element is still in the page
            // We wouldn't be allowed to read the attribute if the element is stale
            Assert.Equal(expectedTarget, form.GetAttribute("action"));
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void CanBreakFormIntoMultipleComponents(bool suppressEnhancedNavigation)
    {
        var url = "forms/default-form-bound-complextype-multiple-components";
        var expectedTarget = GetExpectedTarget(this, null, url);

        if (suppressEnhancedNavigation)
        {
            GoTo("");
            Browser.Equal("Hello", () => Browser.Exists(By.TagName("h1")).Text);
            ((IJavaScriptExecutor)Browser).ExecuteScript("sessionStorage.setItem('suppress-enhanced-navigation', 'true')");
        }

        GoTo(url);

        Browser.Exists(By.Id("ready"));
        var form = Browser.Exists(By.CssSelector("form"));
        var formTarget = form.GetAttribute("action");
        var actionValue = form.GetDomAttribute("action");
        Assert.Equal(expectedTarget, formTarget);
        Assert.Null(actionValue);

        var name = Browser.Exists(By.CssSelector("""input[name="Model.Name"]"""));
        name.SendKeys("John");
        var email = Browser.Exists(By.CssSelector("""input[name="Model.Email"]"""));
        email.SendKeys("john@example.com");
        Browser.Click(By.CssSelector("""input[name="Model.IsPreferred"]"""));

        var billingAddressStreet = Browser.Exists(By.CssSelector("""input[name="Model.BillingAddress.Street"]"""));
        billingAddressStreet.SendKeys("One Microsoft Way");
        var billingAddressCity = Browser.Exists(By.CssSelector("""input[name="Model.BillingAddress.City"]"""));
        billingAddressCity.SendKeys("Redmond");
        var billingAddressAreaCode = Browser.Exists(By.CssSelector("""input[name="Model.BillingAddress.AreaCode"]"""));
        billingAddressAreaCode.Clear();
        billingAddressAreaCode.SendKeys("98052");
        var shippingAddressStreet = Browser.Exists(By.CssSelector("""input[name="Model.ShippingAddress.Street"]"""));
        shippingAddressStreet.SendKeys("Two Microsoft Way");
        var shippingAddressCity = Browser.Exists(By.CssSelector("""input[name="Model.ShippingAddress.City"]"""));
        shippingAddressCity.SendKeys("Bellevue");
        var shippingAddressAreaCode = Browser.Exists(By.CssSelector("""input[name="Model.ShippingAddress.AreaCode"]"""));
        shippingAddressAreaCode.Clear();
        shippingAddressAreaCode.SendKeys("98053");

        Browser.Click(By.Id("send"));

        Browser.Exists(By.Id("name")).Text.Contains("John");
        Browser.Exists(By.Id("email")).Text.Contains("john@example.com");
        Browser.Exists(By.Id("preferred")).Text.Contains("True");
        Browser.Exists(By.Id("billing-address-street")).Text.Contains("One Microsoft Way");
        Browser.Exists(By.Id("billing-address-city")).Text.Contains("Redmond");
        Browser.Exists(By.Id("billing-address-area-code")).Text.Contains("98052");
        Browser.Exists(By.Id("shipping-address-street")).Text.Contains("Two Microsoft Way");
        Browser.Exists(By.Id("shipping-address-city")).Text.Contains("Bellevue");
        Browser.Exists(By.Id("shipping-address-area-code")).Text.Contains("98053");

        if (!suppressEnhancedNavigation)
        {
            // Verify the same form element is still in the page
            // We wouldn't be allowed to read the attribute if the element is stale
            Assert.Equal(expectedTarget, form.GetAttribute("action"));
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void CanBreakFormIntoMultipleComponentsDisplaysErrorsCorrectly(bool suppressEnhancedNavigation)
    {
        var url = "forms/default-form-bound-complextype-multiple-components";
        var expectedTarget = GetExpectedTarget(this, null, url);

        if (suppressEnhancedNavigation)
        {
            GoTo("");
            Browser.Equal("Hello", () => Browser.Exists(By.TagName("h1")).Text);
            ((IJavaScriptExecutor)Browser).ExecuteScript("sessionStorage.setItem('suppress-enhanced-navigation', 'true')");
        }

        GoTo(url);

        Browser.Exists(By.Id("ready"));
        var form = Browser.Exists(By.CssSelector("form"));
        var formTarget = form.GetAttribute("action");
        var actionValue = form.GetDomAttribute("action");
        Assert.Equal(expectedTarget, formTarget);
        Assert.Null(actionValue);

        var name = Browser.Exists(By.CssSelector("""input[name="Model.Name"]"""));
        name.SendKeys("John");
        var email = Browser.Exists(By.CssSelector("""input[name="Model.Email"]"""));
        email.SendKeys("john@example.com");
        Browser.Click(By.CssSelector("""input[name="Model.IsPreferred"]"""));

        var billingAddressStreet = Browser.Exists(By.CssSelector("""input[name="Model.BillingAddress.Street"]"""));
        billingAddressStreet.SendKeys("One Microsoft Way");
        var billingAddressCity = Browser.Exists(By.CssSelector("""input[name="Model.BillingAddress.City"]"""));
        billingAddressCity.SendKeys("Redmond");
        var billingAddressAreaCode = Browser.Exists(By.CssSelector("""input[name="Model.BillingAddress.AreaCode"]"""));
        billingAddressAreaCode.Clear();
        billingAddressAreaCode.SendKeys("98052");
        var shippingAddressStreet = Browser.Exists(By.CssSelector("""input[name="Model.ShippingAddress.Street"]"""));
        shippingAddressStreet.SendKeys("Two Microsoft Way");
        var shippingAddressCity = Browser.Exists(By.CssSelector("""input[name="Model.ShippingAddress.City"]"""));
        shippingAddressCity.SendKeys("Bellevue");
        var shippingAddressAreaCode = Browser.Exists(By.CssSelector("""input[name="Model.ShippingAddress.AreaCode"]"""));
        shippingAddressAreaCode.Clear();
        shippingAddressAreaCode.SendKeys("abcde");

        Browser.Click(By.Id("send"));

        // Assert 'abcde' error
        Browser.Exists(By.CssSelector("""ul.validation-errors > li.validation-message""")).Text.Contains("The value 'abcde' is not valid for 'AreaCode'.");
        Browser.Exists(By.CssSelector("""div > div.validation-message""")).Text.Contains("The value 'abcde' is not valid for 'AreaCode'.");

        if (!suppressEnhancedNavigation)
        {
            // Verify the same form element is still in the page
            // We wouldn't be allowed to read the attribute if the element is stale
            Assert.Equal(expectedTarget, form.GetAttribute("action"));
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void CanDisplayBindingErrorsComplexTypeToDefaultForm(bool suppressEnhancedNavigation)
    {
        var url = "forms/default-form-bound-complextype-parameter";
        var expectedTarget = GetExpectedTarget(this, null, url);

        if (suppressEnhancedNavigation)
        {
            GoTo("");
            Browser.Equal("Hello", () => Browser.Exists(By.TagName("h1")).Text);
            ((IJavaScriptExecutor)Browser).ExecuteScript("sessionStorage.setItem('suppress-enhanced-navigation', 'true')");
        }

        GoTo(url);

        Browser.Exists(By.Id("ready"));
        var form = Browser.Exists(By.CssSelector("form"));
        var formTarget = form.GetAttribute("action");
        var actionValue = form.GetDomAttribute("action");
        Assert.Equal(expectedTarget, formTarget);
        Assert.Null(actionValue);

        var name = Browser.Exists(By.CssSelector("""input[name="Model.Name"]"""));
        name.SendKeys("John");
        var email = Browser.Exists(By.CssSelector("""input[name="Model.Email"]"""));
        email.SendKeys("john@example.com");
        Browser.Click(By.CssSelector("""input[name="Model.IsPreferred"]"""));
        // Set value attribute to 'invalid' to trigger validation error
        var isPreferred = Browser.Exists(By.CssSelector("""input[name="Model.IsPreferred"]"""));
        ((IJavaScriptExecutor)Browser).ExecuteScript("arguments[0].setAttribute('value', 'invalid')", isPreferred);

        Browser.Click(By.Id("send"));

        Browser.Exists(By.CssSelector("li.validation-message")).Text.Contains("The value 'invalid' is not valid for 'value'.");
        Browser.Exists(By.CssSelector("div.validation-message")).Text.Contains("The value 'invalid' is not valid for 'value'.");

        if (!suppressEnhancedNavigation)
        {
            // Verify the same form element is still in the page
            // We wouldn't be allowed to read the attribute if the element is stale
            Assert.Equal(expectedTarget, form.GetAttribute("action"));
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void CanBindDictionaryToDefaultForm(bool suppressEnhancedNavigation)
    {
        var url = "forms/default-form-bound-dictionary-parameter";
        var expectedTarget = GetExpectedTarget(this, null, url);

        if (suppressEnhancedNavigation)
        {
            GoTo("");
            Browser.Equal("Hello", () => Browser.Exists(By.TagName("h1")).Text);
            ((IJavaScriptExecutor)Browser).ExecuteScript("sessionStorage.setItem('suppress-enhanced-navigation', 'true')");
        }

        GoTo(url);

        Browser.Exists(By.Id("ready"));
        var form = Browser.Exists(By.CssSelector("form"));
        var formTarget = form.GetAttribute("action");
        var actionValue = form.GetDomAttribute("action");
        Assert.Equal(expectedTarget, formTarget);
        Assert.Null(actionValue);

        var name = Browser.Exists(By.CssSelector("""input[name="Model[Name]"]"""));
        name.SendKeys("John");
        var email = Browser.Exists(By.CssSelector("""input[name="Model[Email]"]"""));
        email.SendKeys("john@example.com");
        Browser.Click(By.CssSelector("""input[name="Model[IsPreferred]"]"""));
        Browser.Click(By.Id("send"));

        Browser.Exists(By.Id("name")).Text.Contains("John");
        Browser.Exists(By.Id("email")).Text.Contains("john@example.com");
        Browser.Exists(By.Id("preferred")).Text.Contains("True");

        if (!suppressEnhancedNavigation)
        {
            // Verify the same form element is still in the page
            // We wouldn't be allowed to read the attribute if the element is stale
            Assert.Equal(expectedTarget, form.GetAttribute("action"));
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void CanDisplayBindingErrorsDictionaryToDefaultForm(bool suppressEnhancedNavigation)
    {
        var url = "forms/default-form-bound-dictionary-parameter-errors";
        var expectedTarget = GetExpectedTarget(this, null, url);

        if (suppressEnhancedNavigation)
        {
            GoTo("");
            Browser.Equal("Hello", () => Browser.Exists(By.TagName("h1")).Text);
            ((IJavaScriptExecutor)Browser).ExecuteScript("sessionStorage.setItem('suppress-enhanced-navigation', 'true')");
        }

        GoTo(url);

        Browser.Exists(By.Id("ready"));
        var form = Browser.Exists(By.CssSelector("form"));
        var formTarget = form.GetAttribute("action");
        var actionValue = form.GetDomAttribute("action");
        Assert.Equal(expectedTarget, formTarget);
        Assert.Null(actionValue);

        var name = Browser.Exists(By.CssSelector("""input[name="Model[Name]"]"""));
        ((IJavaScriptExecutor)Browser).ExecuteScript("arguments[0].setAttribute('value', 'name')", name);
        var email = Browser.Exists(By.CssSelector("""input[name="Model[Email]"]"""));
        ((IJavaScriptExecutor)Browser).ExecuteScript("arguments[0].setAttribute('value', 'email')", email);
        var preferred = Browser.Exists(By.CssSelector("""input[name="Model[IsPreferred]"]"""));
        ((IJavaScriptExecutor)Browser).ExecuteScript("arguments[0].setAttribute('value', 'preferred')", preferred);

        Browser.Click(By.Id("send"));

        Browser.Exists(By.CssSelector("[data-name='Name']"));
        Browser.Exists(By.CssSelector("[data-email='Email']"));
        Browser.Exists(By.CssSelector("[data-preferred='IsPreferred']"));

        Browser.Equal(3, () => Browser.FindElements(By.CssSelector("li.validation-message")).Count());

        if (!suppressEnhancedNavigation)
        {
            // Verify the same form element is still in the page
            // We wouldn't be allowed to read the attribute if the element is stale
            Assert.Equal(expectedTarget, form.GetAttribute("action"));
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void CanBindCollectionsToDefaultForm(bool suppressEnhancedNavigation)
    {
        var url = "forms/default-form-bound-collection-parameter";
        var expectedTarget = GetExpectedTarget(this, null, url);

        if (suppressEnhancedNavigation)
        {
            GoTo("");
            Browser.Equal("Hello", () => Browser.Exists(By.TagName("h1")).Text);
            ((IJavaScriptExecutor)Browser).ExecuteScript("sessionStorage.setItem('suppress-enhanced-navigation', 'true')");
        }

        GoTo(url);

        Browser.Exists(By.Id("ready"));
        var form = Browser.Exists(By.CssSelector("form"));
        var formTarget = form.GetAttribute("action");
        var actionValue = form.GetDomAttribute("action");
        Assert.Equal(expectedTarget, formTarget);
        Assert.Null(actionValue);

        for (var i = 0; i < 2; i++)
        {
            var name = Browser.Exists(By.CssSelector($"""input[name="Model[{i}].Name"]"""));
            name.Clear();
            name.SendKeys($"John{i + 4}");
            var email = Browser.Exists(By.CssSelector($"""input[name="Model[{i}].Email"]"""));
            email.Clear();
            email.SendKeys($"john{i + 4}@example.com");
            Browser.Click(By.CssSelector($"""input[name="Model[{i}].IsPreferred"]"""));
        }

        Browser.Click(By.Id("send"));

        for (var i = 0; i < 2; i++)
        {
            Browser.Exists(By.Id($"name[{i}]")).Text.Contains($"John{i + 4}");
            Browser.Exists(By.Id($"email[{i}]")).Text.Contains($"john{i + 4}@example.com");
            Browser.Exists(By.Id($"preferred[{i}]")).Text.Contains(i % 2 == 0 ? "True" : "False");
        }

        if (!suppressEnhancedNavigation)
        {
            // Verify the same form element is still in the page
            // We wouldn't be allowed to read the attribute if the element is stale
            Assert.Equal(expectedTarget, form.GetAttribute("action"));
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void CanDisplayBindingErrorsCollectionsToDefaultForm(bool suppressEnhancedNavigation)
    {
        var url = "forms/default-form-bound-collection-parameter";
        var expectedTarget = GetExpectedTarget(this, null, url);

        if (suppressEnhancedNavigation)
        {
            GoTo("");
            Browser.Equal("Hello", () => Browser.Exists(By.TagName("h1")).Text);
            ((IJavaScriptExecutor)Browser).ExecuteScript("sessionStorage.setItem('suppress-enhanced-navigation', 'true')");
        }

        GoTo(url);

        Browser.Exists(By.Id("ready"));
        var form = Browser.Exists(By.CssSelector("form"));
        var formTarget = form.GetAttribute("action");
        var actionValue = form.GetDomAttribute("action");
        Assert.Equal(expectedTarget, formTarget);
        Assert.Null(actionValue);

        for (var i = 0; i < 2; i++)
        {
            var name = Browser.Exists(By.CssSelector($"""input[name="Model[{i}].Name"]"""));
            name.Clear();
            name.SendKeys($"John{i + 4}");
            var email = Browser.Exists(By.CssSelector($"""input[name="Model[{i}].Email"]"""));
            email.Clear();
            email.SendKeys($"john{i + 4}@example.com");
            var preferredCriteria = By.CssSelector($"""input[name="Model[{i}].IsPreferred"]""");
            var preferred = Browser.FindElement(preferredCriteria);
            if (!preferred.Selected)
            {
                Browser.Click(preferredCriteria);
            }
            // Set the value for preferred ti 'invalid' to trigger a binding error
            ((IJavaScriptExecutor)Browser).ExecuteScript($"arguments[0].setAttribute('value', 'invalid{i}')", preferred);
        }

        Browser.Click(By.Id("send"));

        Browser.Exists(By.CssSelector("[data-index='0']")).Text.Contains("The value 'invalid0' is not valid for 'IsPreferred'.");
        Browser.Exists(By.CssSelector("[data-index='1']")).Text.Contains("The value 'invalid1' is not valid for 'IsPreferred'.");

        Browser.Equal(2, () => Browser.FindElements(By.CssSelector("li.validation-message")).Count());

        if (!suppressEnhancedNavigation)
        {
            // Verify the same form element is still in the page
            // We wouldn't be allowed to read the attribute if the element is stale
            Assert.Equal(expectedTarget, form.GetAttribute("action"));
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void CanHandleBindingErrorsBindParameterToNamedForm(bool suppressEnhancedNavigation)
    {
        var dispatchToForm = new DispatchToForm(this)
        {
            Url = "forms/named-form-bound-primitive-parameter",
            FormCssSelector = "form[name=my-bound-form]",
            ExpectedHandlerValue = "named-form-handler",
            InputFieldId = "Parameter",
            InputFieldCssSelector = "input[name=Parameter]",
            InputFieldValue = "abc",
            SuppressEnhancedNavigation = suppressEnhancedNavigation,
            AssertErrors = errors =>
            {
                Assert.Collection(
                    errors,
                    error =>
                    {
                        Assert.Equal("The value 'abc' is not valid for 'Parameter'.", error.Text);
                    });
                Assert.Equal("abc", Browser.FindElement(By.CssSelector("input[name=Parameter]")).GetAttribute("value"));
            }
        };
        DispatchToFormCore(dispatchToForm);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void CanReadFormValuesDuringOnInitialized(bool suppressEnhancedNavigation)
    {
        var dispatchToForm = new DispatchToForm(this)
        {
            Url = "forms/default-form-with-body-on-initialized",
            FormCssSelector = "form",
            InputFieldValue = "stranger",
            SuppressEnhancedNavigation = suppressEnhancedNavigation,
        };
        DispatchToFormCore(dispatchToForm);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void CanDispatchToNamedForm(bool suppressEnhancedNavigation)
    {
        var dispatchToForm = new DispatchToForm(this)
        {
            Url = "forms/named-form",
            FormCssSelector = "form",
            ExpectedHandlerValue = "named-form-handler",
            SuppressEnhancedNavigation = suppressEnhancedNavigation,
        };
        DispatchToFormCore(dispatchToForm);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void CanBindFormValueFromNamedFormWithBody(bool suppressEnhancedNavigation)
    {
        var dispatchToForm = new DispatchToForm(this)
        {
            Url = "forms/named-form-bound-parameter",
            FormCssSelector = "form",
            ExpectedHandlerValue = "named-form-handler",
            InputFieldId = "Parameter",
            InputFieldCssSelector = "input[name=Parameter]",
            InputFieldValue = "stranger",
            SuppressEnhancedNavigation = suppressEnhancedNavigation,
        };
        DispatchToFormCore(dispatchToForm);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void CanDispatchToNamedFormInMappingScope(bool suppressEnhancedNavigation)
    {
        var dispatchToForm = new DispatchToForm(this)
        {
            Url = "forms/nested-named-form",
            FormCssSelector = "form",
            ExpectedHandlerValue = "[parent-context]named-form-handler",
            SuppressEnhancedNavigation = suppressEnhancedNavigation,
        };
        DispatchToFormCore(dispatchToForm);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void CanBindFormValueFromNamedFormInMappingScopeWithBody(bool suppressEnhancedNavigation)
    {
        var dispatchToForm = new DispatchToForm(this)
        {
            Url = "forms/nested-named-form-bound-parameter",
            FormCssSelector = "form",
            ExpectedHandlerValue = "[parent-context]named-form-handler",
            InputFieldId = "Parameter",
            InputFieldCssSelector = "input[name=Parameter]",
            InputFieldValue = "stranger",
            SuppressEnhancedNavigation = suppressEnhancedNavigation,
        };
        DispatchToFormCore(dispatchToForm);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void CanDispatchToFormDefinedInNonPageComponent(bool suppressEnhancedNavigation)
    {
        var dispatchToForm = new DispatchToForm(this)
        {
            Url = "forms/form-defined-inside-component",
            FormCssSelector = "form",
            SuppressEnhancedNavigation = suppressEnhancedNavigation,
        };
        DispatchToFormCore(dispatchToForm);
    }

    [Fact]
    public void CanRenderAmbiguousForms()
    {
        var dispatchToForm = new DispatchToForm(this)
        {
            Url = "forms/ambiguous-forms",
            FormCssSelector = "form",
            DispatchEvent = false,
            ShouldCauseInternalServerError = false,
        };
        DispatchToFormCore(dispatchToForm);
    }

    [Fact]
    public void CannotSubmitAmbiguousForms()
    {
        var dispatchToForm = new DispatchToForm(this)
        {
            Url = "forms/ambiguous-forms",
            FormCssSelector = "form",
            DispatchEvent = true,
            ShouldCauseInternalServerError = true,
        };
        DispatchToFormCore(dispatchToForm);
    }

    [Fact]
    public void CanDispatchToFormRenderedAsynchronously()
    {
        var dispatchToForm = new DispatchToForm(this)
        {
            Url = "forms/async-rendered-form",
            FormCssSelector = "form",
        };
        DispatchToFormCore(dispatchToForm);
    }

    [Fact]
    public void FormThatDisappearsBeforeQuiesceDoesNotBind()
    {
        var dispatchToForm = new DispatchToForm(this)
        {
            Url = "forms/disappears-before-dispatching",
            FormCssSelector = "form",
            SubmitButtonId = "test-send",
            ShouldCauseBadRequest = true,
        };
        DispatchToFormCore(dispatchToForm);
    }

    [Fact]
    public async Task CanPostFormsWithStreamingRenderingAsync()
    {
        GoTo("forms/streaming-rendering/CanPostFormsWithStreamingRendering");

        Browser.Exists(By.Id("ready"));
        var form = Browser.Exists(By.CssSelector("form"));
        var actionValue = form.GetDomAttribute("action");
        Assert.Null(actionValue);

        Browser.Click(By.Id("send"));

        Browser.Exists(By.Id("progress"));

        using var client = new HttpClient() { BaseAddress = _serverFixture.RootUri };
        var response = await client.PostAsync("subdir/forms/streaming-rendering/complete/CanPostFormsWithStreamingRendering", content: null);
        response.EnsureSuccessStatusCode();

        Browser.Exists(By.Id("pass"));
    }

    [Fact]
    public async Task CanModifyTheHttpResponseDuringEventHandling()
    {
        GoTo("forms/modify-http-context/ModifyHttpContext");

        Browser.Exists(By.Id("ready"));
        var form = Browser.Exists(By.CssSelector("form"));
        var actionValue = form.GetDomAttribute("action");
        Assert.Null(actionValue);

        Browser.Click(By.Id("send"));

        Browser.Exists(By.Id("progress"));

        using var client = new HttpClient() { BaseAddress = _serverFixture.RootUri };
        var response = await client.PostAsync("subdir/forms/streaming-rendering/complete/ModifyHttpContext", content: null);
        response.EnsureSuccessStatusCode();

        Browser.Exists(By.Id("pass"));
        var cookie = Browser.Manage().Cookies.GetCookieNamed("operation");
        Assert.Equal("ModifyHttpContext", cookie.Value);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void FormNoAntiforgeryReturnBadRequest(bool suppressEnhancedNavigation)
    {
        var dispatchToForm = new DispatchToForm(this)
        {
            Url = "forms/no-antiforgery",
            FormCssSelector = "form",
            ShouldCauseBadRequest = true,
            SuppressEnhancedNavigation = suppressEnhancedNavigation,
        };
        DispatchToFormCore(dispatchToForm);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void FormAntiforgeryCheckDisabledOnPage(bool suppressEnhancedNavigation)
    {
        var dispatchToForm = new DispatchToForm(this)
        {
            Url = "forms/disable-antiforgery-check",
            FormCssSelector = "form",
            SuppressEnhancedNavigation = suppressEnhancedNavigation,
        };
        DispatchToFormCore(dispatchToForm);
    }

    [Fact]
    public void FormCanAddAntiforgeryAfterTheResponseHasStarted()
    {
        var dispatchToForm = new DispatchToForm(this)
        {
            Url = "forms/antiforgery-after-response-started",
            FormCssSelector = "form",
            SuppressEnhancedNavigation = true,
        };
        DispatchToFormCore(dispatchToForm);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void FormNoHandlerReturnBadRequest(bool suppressEnhancedNavigation)
    {
        var dispatchToForm = new DispatchToForm(this)
        {
            Url = "forms/no-handler",
            FormCssSelector = "form",
            ShouldCauseBadRequest = true,
            SuppressEnhancedNavigation = suppressEnhancedNavigation,
        };
        DispatchToFormCore(dispatchToForm);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void CanUsePlainForm(bool suppressEnhancedNavigation)
    {
        var dispatchToForm = new DispatchToForm(this)
        {
            Url = "forms/plain-form",
            DispatchEvent = false,
            FormCssSelector = "form",
            ExpectedHandlerValue = "my-form",
            SuppressEnhancedNavigation = suppressEnhancedNavigation,
        };
        DispatchToFormCore(dispatchToForm);

        Browser.Exists(By.CssSelector("#StringViaExplicitPropertyName input")).SendKeys("StringViaExplicitPropertyName value");
        Browser.Exists(By.CssSelector("#StringViaOverriddenName input")).SendKeys("StringViaOverriddenName value");
        Browser.Exists(By.CssSelector("#StringViaOverriddenNameUnmatched input")).SendKeys("StringViaOverriddenNameUnmatched value");
        Browser.Exists(By.CssSelector("#StringViaExpression input")).SendKeys("StringViaExpression value");
        Browser.Exists(By.CssSelector("#StringViaExpressionWithHandler input")).SendKeys("StringViaExpressionWithHandler value");
        Browser.Exists(By.CssSelector("#StringViaExpressionWithUnmatchedHandler input")).SendKeys("StringViaExpressionWithUnmatchedHandler value");
        Browser.Exists(By.CssSelector("#PersonName input")).SendKeys("PersonName value");
        Browser.Exists(By.CssSelector("#PersonAge input")).Clear(); // Remove the existing zero, otherwise we'll get 1230
        Browser.Exists(By.CssSelector("#PersonAge input")).SendKeys("123");

        Browser.Exists(By.Id("send")).Click();
        Browser.Exists(By.Id("pass"));

        Browser.Equal("StringViaExplicitPropertyName value", () => Browser.Exists(By.CssSelector("#StringViaExplicitPropertyName input")).GetAttribute("value"));
        Browser.Equal("StringViaOverriddenName value", () => Browser.Exists(By.CssSelector("#StringViaOverriddenName input")).GetAttribute("value"));
        Browser.Equal(/* should not match */ "", () => Browser.Exists(By.CssSelector("#StringViaOverriddenNameUnmatched input")).GetAttribute("value"));
        Browser.Equal("StringViaExpression value", () => Browser.Exists(By.CssSelector("#StringViaExpression input")).GetAttribute("value"));
        Browser.Equal("StringViaExpressionWithHandler value", () => Browser.Exists(By.CssSelector("#StringViaExpressionWithHandler input")).GetAttribute("value"));
        Browser.Equal(/* should not match */ "", () => Browser.Exists(By.CssSelector("#StringViaExpressionWithUnmatchedHandler input")).GetAttribute("value"));
        Browser.Equal("PersonName value", () => Browser.Exists(By.CssSelector("#PersonName input")).GetAttribute("value"));
        Browser.Equal("123", () => Browser.Exists(By.CssSelector("#PersonAge input")).GetAttribute("value"));
    }

    [Fact]
    public async Task CanHandleFormPostNonStreamingRenderingAsyncHandler()
    {
        GoTo("forms/non-streaming-async-form-handler/CanHandleFormPostNonStreamingRenderingAsyncHandler");

        Browser.Exists(By.Id("ready"));
        var form = Browser.Exists(By.CssSelector("form"));
        var actionValue = form.GetDomAttribute("action");
        Assert.Null(actionValue);

        Browser.Click(By.Id("send"));

        await Task.Yield();

        using var client = new HttpClient() { BaseAddress = _serverFixture.RootUri };
        var response = await client.PostAsync("subdir/forms/streaming-rendering/complete/CanHandleFormPostNonStreamingRenderingAsyncHandler", content: null);
        response.EnsureSuccessStatusCode();

        Browser.Exists(By.Id("pass"));
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void HandleErrorsOutsideErrorBoundary_OnInitialRender(bool suppressEnhancedNavigation, bool enableStreaming)
    {
        SuppressEnhancedNavigation(suppressEnhancedNavigation);
        GoTo($"forms/error-outside-error-boundary{( enableStreaming ? "-streaming" : "" )}");

        Browser.Exists(By.LinkText("Throw during initial render")).Click();
        AssertHasInternalServerError(suppressEnhancedNavigation);
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void HandleErrorsOutsideErrorBoundary_SynchronouslyInSubmitEvent(bool suppressEnhancedNavigation, bool enableStreaming)
    {
        SuppressEnhancedNavigation(suppressEnhancedNavigation);
        GoTo($"forms/error-outside-error-boundary{(enableStreaming ? "-streaming" : "")}");

        Browser.Exists(By.Id("throw-sync")).Click();
        AssertHasInternalServerError(suppressEnhancedNavigation, enableStreaming);
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void HandleErrorsOutsideErrorBoundary_AsynchronouslyInSubmitEvent(bool suppressEnhancedNavigation, bool enableStreaming)
    {
        SuppressEnhancedNavigation(suppressEnhancedNavigation);
        GoTo($"forms/error-outside-error-boundary{(enableStreaming ? "-streaming" : "")}");

        Browser.Exists(By.Id("throw-async")).Click();
        AssertHasInternalServerError(suppressEnhancedNavigation, enableStreaming);
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void HandleErrorsInsideErrorBoundary_OnInitialRender(bool suppressEnhancedNavigation, bool enableStreaming)
    {
        SuppressEnhancedNavigation(suppressEnhancedNavigation);
        GoTo($"forms/error-in-error-boundary{(enableStreaming ? "-streaming" : "")}");

        Browser.Exists(By.LinkText("Throw during initial render")).Click();

        var errorBoundaryContent = Browser.Exists(By.Id("error-content"));
        Assert.Contains("This is a deliberate error during initial render", errorBoundaryContent.Text);
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void HandleErrorsInsideErrorBoundary_SynchronouslyInSubmitEvent(bool suppressEnhancedNavigation, bool enableStreaming)
    {
        SuppressEnhancedNavigation(suppressEnhancedNavigation);
        GoTo($"forms/error-in-error-boundary{(enableStreaming ? "-streaming" : "")}");

        Browser.Exists(By.Id("throw-sync")).Click();

        var errorBoundaryContent = Browser.Exists(By.Id("error-content"));
        Assert.Contains("This is a deliberate form-event synchronous error", errorBoundaryContent.Text);
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void HandleErrorsInsideErrorBoundary_AsynchronouslyInSubmitEvent(bool suppressEnhancedNavigation, bool enableStreaming)
    {
        SuppressEnhancedNavigation(suppressEnhancedNavigation);
        GoTo($"forms/error-in-error-boundary{(enableStreaming ? "-streaming" : "")}");

        Browser.Exists(By.Id("throw-async")).Click();

        var errorBoundaryContent = Browser.Exists(By.Id("error-content"));
        Assert.Contains("This is a deliberate form-event asynchronous error", errorBoundaryContent.Text);
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void CanPostRedirectGet_Synchronous(bool suppressEnhancedNavigation, bool enableStreaming)
    {
        SuppressEnhancedNavigation(suppressEnhancedNavigation);
        GoTo($"forms/post-redirect-get{(enableStreaming ? "-streaming" : "")}");

        Browser.Exists(By.Id("sync-redirect")).Click();
        Browser.Exists(By.Id("nav-home"));
        Browser.True(() => Browser.Url.EndsWith("/nav", StringComparison.Ordinal));
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void CanPostRedirectGet_Asynchronous(bool suppressEnhancedNavigation, bool enableStreaming)
    {
        SuppressEnhancedNavigation(suppressEnhancedNavigation);
        GoTo($"forms/post-redirect-get{(enableStreaming ? "-streaming" : "")}");

        Browser.Exists(By.Id("async-redirect")).Click();
        Browser.Exists(By.Id("nav-home"));
        Browser.True(() => Browser.Url.EndsWith("/nav", StringComparison.Ordinal));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void CanMutateDataSuppliedFromForm(bool suppressEnhancedNavigation)
    {
        SuppressEnhancedNavigation(suppressEnhancedNavigation);
        GoTo($"forms/mutate-and-rerender");

        Browser.Exists(By.Id("simple-value")).SendKeys("Abc");
        Browser.Exists(By.Id("complex-value")).SendKeys("Def");
        Browser.Equal("1", () => Browser.Exists(By.Id("render-count")).Text);

        // Can perform a submit that mutates the data and rerenders the receiver
        // Remember that the rendercount would be reset to zero since this is SSR, so
        // receiving 2 here shows we did render twice on this cycle
        Browser.Exists(By.Id("mutate-and-notify")).Click();
        Browser.Equal("Abc Modified", () => Browser.Exists(By.Id("simple-value")).GetAttribute("value"));
        Browser.Equal("Def Modified", () => Browser.Exists(By.Id("complex-value")).GetAttribute("value"));
        Browser.Equal("2", () => Browser.Exists(By.Id("render-count")).Text);
        Browser.Exists(By.Id("received-notification"));

        // Can perform a submit that replaces the received object entirely
        Browser.Exists(By.Id("clear-and-notify")).Click();
        Browser.Equal("", () => Browser.Exists(By.Id("simple-value")).GetAttribute("value"));
        Browser.Equal("", () => Browser.Exists(By.Id("complex-value")).GetAttribute("value"));
        Browser.Equal("2", () => Browser.Exists(By.Id("render-count")).Text);
        Browser.Exists(By.Id("received-notification"));
    }

    private void SuppressEnhancedNavigation(bool shouldSuppress)
    {
        if (shouldSuppress)
        {
            GoTo("");
            Browser.Equal("Hello", () => Browser.Exists(By.TagName("h1")).Text);
            ((IJavaScriptExecutor)Browser).ExecuteScript("sessionStorage.setItem('suppress-enhanced-navigation', 'true')");
        }
    }

    private void AssertHasInternalServerError(bool suppressedEnhancedNavigation, bool streaming = false)
    {
        if (streaming)
        {
            Browser.True(() => Browser.FindElement(By.TagName("html")).Text.Contains("There was an unhandled exception on the current request"));
        }
        else if (suppressedEnhancedNavigation)
        {
            // Chrome's built-in error UI for a 500 response when there's no response content
            Browser.Exists(By.Id("main-frame-error"));
        }
        else
        {
            // The UI generated by enhanced nav when there's no response content
            Browser.Contains("Error: 500", () => Browser.Exists(By.TagName("html")).Text);
        }
    }

    [Fact]
    public void PostingCollectionsThatExceedTheLimitFails()
    {
        var dispatchToForm = new DispatchToForm(this)
        {
            Url = "forms/default-form-max-collection-limit",
            FormCssSelector = "form",
            AssertErrors = (errors) =>
            {
                var error = Assert.Single(errors);
                Assert.Equal("The number of elements in the collection exceeded the maximum number of '100' elements allowed.", errors[0].Text);
            },
            ErrorSelector = "ul.validation-errors > li.validation-message",
        };
        DispatchToFormCore(dispatchToForm);
    }

    [Fact]
    public void PostingMaxRecursionDepthExceedTheLimitFails()
    {
        var dispatchToForm = new DispatchToForm(this)
        {
            Url = "forms/default-form-max-recursion-depth",
            FormCssSelector = "form",
            AssertErrors = (errors) =>
            {
                Assert.Collection(errors,
                    err => Assert.Equal("The maximum recursion depth of '5' was exceeded for 'Values.Tail.Tail.Tail.Tail.Head'.", errors[0].Text),
                    err => Assert.Equal("The maximum recursion depth of '5' was exceeded for 'Values.Tail.Tail.Tail.Tail.Tail'.", errors[1].Text));
            },
            ErrorSelector = "ul.validation-errors > li.validation-message",
        };
        DispatchToFormCore(dispatchToForm);
    }

    [Fact]
    public void PostingFormWithErrorsDoesNotExceedMaximumErrors()
    {
        var dispatchToForm = new DispatchToForm(this)
        {
            Url = "forms/default-form-max-collection-limit",
            FormCssSelector = "form",
            UpdateFormAction = () =>
            {
                var elements = Browser.FindElements(By.CssSelector("input[type='text']"));
                for (var i = 0; i < elements.Count; i++)
                {
                    var element = elements[i];
                    element.Clear();
                    element.SendKeys("a");
                }
            },
            AssertErrors = (errors) =>
            {
                Assert.Equal(10, errors.Count);
            },
            ErrorSelector = "ul.validation-errors > li.validation-message",
        };
        DispatchToFormCore(dispatchToForm);
    }

    private void DispatchToFormCore(DispatchToForm dispatch)
    {
        SuppressEnhancedNavigation(dispatch.SuppressEnhancedNavigation);
        GoTo(dispatch.Url);

        if (!dispatch.DispatchEvent && dispatch.ShouldCauseInternalServerError)
        {
            // Chrome's built-in error UI for a 500 response when there's no response content
            Browser.Exists(By.Id("main-frame-error"));
            return;
        }

        Browser.Exists(By.Id(dispatch.Ready));
        var form = Browser.Exists(By.CssSelector(dispatch.FormCssSelector));

        if (dispatch.ExpectedHandlerValue != null)
        {
            var handlerInput = form.FindElement(By.CssSelector("input[type=hidden][name=_handler]"));
            Assert.Equal(dispatch.ExpectedHandlerValue, handlerInput.GetAttribute("value"));
        }

        if (!dispatch.DispatchEvent)
        {
            return;
        }

        if (dispatch.UpdateFormAction != null)
        {
            dispatch.UpdateFormAction();
        }
        else if (dispatch.InputFieldValue != null)
        {
            var criteria = dispatch.InputFieldCssSelector != null ?
                By.CssSelector(dispatch.InputFieldCssSelector) :
                By.Id(dispatch.InputFieldId);

            Browser.Exists(criteria).Clear();
            Browser.Exists(criteria).SendKeys(dispatch.InputFieldValue);
        }

        Browser.Click(By.Id(dispatch.SubmitButtonId));

        if (dispatch.ShouldCauseInternalServerError)
        {
            AssertHasInternalServerError(dispatch.SuppressEnhancedNavigation);
        }
        else if (dispatch.ShouldCauseBadRequest)
        {
            if (dispatch.SuppressEnhancedNavigation)
            {
                // Chrome's built-in error UI for a 500 response when there's no response content
                Browser.Contains("HTTP ERROR 400", () => Browser.Exists(By.CssSelector("div.error-code")).Text);
            }
            else
            {
                // The UI generated by enhanced nav when there's no response content
                Browser.Contains("Error: 400", () => Browser.Exists(By.TagName("html")).Text);
            }
        }
        else if (dispatch.ShouldCauseBindingErrors)
        {
            var errors = Browser.FindElements(By.CssSelector(dispatch.ErrorSelector));
            dispatch.AssertErrors(errors);
        }
        else
        {
            if (dispatch.UpdateFormAction == null)
            {
                var text = Browser.Exists(By.Id(dispatch.SubmitPassId)).Text;
                if (dispatch.InputFieldValue != null)
                {
                    Assert.Equal($"Hello {dispatch.InputFieldValue}!", text);
                }
            }

            if (!dispatch.SuppressEnhancedNavigation)
            {
                // Verify the same form element is still in the page
                // We wouldn't be allowed to read the attribute if the element is stale
                Assert.Equal("post", form.GetAttribute("method"));
            }
        }
    }

    private record struct DispatchToForm()
    {
        public DispatchToForm(FormWithParentBindingContextTest test) : this()
        {
            Base = new Uri(test._serverFixture.RootUri, test.ServerPathBase).ToString();
        }

        public string Base;
        public string Url;
        public string SubmitPassId = "pass";
        public string Ready = "ready";
        public string FormCssSelector;
        public string ExpectedHandlerValue;
        public string InputFieldValue;

        public bool DispatchEvent { get; internal set; } = true;

        public string SubmitButtonId { get; internal set; } = "send";
        public string InputFieldId { get; internal set; } = "firstName";
        public string InputFieldCssSelector { get; internal set; } = null;
        public bool ShouldCauseInternalServerError { get; internal set; }
        public bool ShouldCauseBadRequest { get; internal set; }
        public bool ShouldCauseBindingErrors => AssertErrors != null;
        public bool SuppressEnhancedNavigation { get; internal set; }
        public Action UpdateFormAction { get; internal set; }
        public Action<ReadOnlyCollection<IWebElement>> AssertErrors { get; internal set; }
        public string ErrorSelector { get; internal set; } = "#errors > li";
    }

    private string GetExpectedTarget(FormWithParentBindingContextTest test, string expectedActionValue, string url)
        => $"{new Uri(test._serverFixture.RootUri, test.ServerPathBase)}/{expectedActionValue ?? url}";

    private void GoTo(string relativePath)
    {
        Navigate($"{ServerPathBase}/{relativePath}");
    }
}
