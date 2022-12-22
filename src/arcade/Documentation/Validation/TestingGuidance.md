# Testing Guidance

## Requirements

### [FluentAssertions](https://fluentassertions.com/introduction)
We are using FluentAssertions for all testing assertions.
The main benefit specific to this repository is that this allows us to use the same assertion library regardless of execution frameworks (XUnit or NUnit),
so that a developer only needs to learn how to write and read a single assertion library to be productive.
Other benefits are outlined well in the documentation on the [FluentAssertions site](https://fluentassertions.com/introduction).

### [NUnit](https://docs.nunit.org/articles/nunit/intro.html)
We are going to use NUnit as the testing framework, but _not_ the assertion library.
This means using the attributes, like `[Test]`, but not the `Assert` class.
This is because we have a lot of integration testing, where each test can take a long time,
and having fine grained control of the parallelism of execution is critical so that these tests can finish in a reasonable amount of time.
With XUnit, this requires putting every test in a separate class or "Test Collection",
which impacts maintainability more than using NUnit.

## Tips

There are a lot of good guides on the internet about how to write good tests.

I highly recommend watching this training day video as well
[Unit Tests that Don't Suck - by Graham Wheele](https://msit.microsoftstream.com/video/93e9a3ff-0400-a936-f58b-f1eaa13859f9)

Here are some good articles I read on the subject that align with our testing usage.
* [Unit Tests, How to Write Testable Code and Why it Matters - Sergey Kolodiy](https://www.toptal.com/qa/how-to-write-testable-code-and-why-it-matters)
* [13 Tips for Writing Useful Unit Tests - Nick Hodges](https://medium.com/better-programming/13-tips-for-writing-useful-unit-tests-ca20706b5368)
* [You Still Don’t Know How to Do Unit Testing (and Your Secret is Safe with Me) - Erik Dietrich](https://stackify.com/unit-testing-basics-best-practices/)

### Takeaways

#### Avoid stateful classes
In general, `[SetUp]` and `[TearDown]` (or constructor/Dipose) methods should be avoided.
And `[SetUp]`/`[TearDown]` in base classes is especially bad.
Not only does this mean that to understand a test you need to go spelunking in multiple files to attempt to track down what's happening,
it also means that the behavior of your test is tightly coupled to the test infrastructure.

In general you get a lot more control by having a class that encapsulates the data your tests need.
In various reading, this is often called an "Object Mother" (example here https://reflectoring.io/objectmother-fluent-builder/).
A test method calls the required methods in the beginning of the test method,
and, if necessary, can "using" any of the returned objects that need to be disposed.
The benefits of this are that everything the test does is done in the test itself
(or methods called directly by the tests, so you can F12 your way into them if necessary).

#### Be verbose
In tests, it is especially helpful for things to be named verbosely.
Test methods, variable names, any test factory methods that are called,
adding string to assertions that might otherwise be unclear
(if you are asserting multiple properties in a method, it is helpful to say which property each assert is for,
so, it is not "expected: 2, actual: 4" but "property exampleProperty.Length, expected: 2, actual: 4".)

Often developers will have only the name of the method and the assert message to go on,
and then might be unfamiliar with the code that is failing (if they were, they likely wouldn't have broken it, after all).
The more information that you can put as close to the failure as possible, the more likely the problem can be addressed quickly and correctly).

#### Use [Dependency Injection](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-3.1), even for small things, like "Now"
The dependency injection we use for all our code is `Microsoft.Extensions.DependencyInjection`.
It's relatively lightweight, built in to ASP.NET Core, and can be used with little effort to solve a large host of problems.
And it can greatly simplify testing and sharing code.

If you find yourself calling some static, stateful method if instead an injected Singleton class/interface might improve the ability to test.
Even innocuous things, like `DateTime.UtcNow`, can almost singlehandedly make testing a function impossible.
Inject an `ISystemClock` instead.

If you find that your class is injecting a large number of dependencies, it's likely a sign the class is trying to do too much
(and at the very least, will be difficult to test since every dependency needs to be accounted for in a test).


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CValidation%5CTestingGuidance.md)](https://helix.dot.net/f/p/5?p=Documentation%5CValidation%5CTestingGuidance.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CValidation%5CTestingGuidance.md)</sub>
<!-- End Generated Content-->
