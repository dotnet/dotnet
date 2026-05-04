# Localizability

NuGet follows Microsoft's policy of being available in a number of different languages.
Therefore, all strings that are displayed to users must use resx file resources, so they can be translated.

## How to add resource comments

1. Edit the appropriate *.resx* file.
   Only provide the English translation.
   - If you're using Visual Studio, it has a built-in resource editor and you can double click the *.resx* file in Solution Explorer.
     A GUI will appear where you can add a new string
   - If you're using anything else, for example VSCode, you can edit the resx file as an XML file.
     The entries in the resx are not sorted, so you can add the new string as the last element in the file.
1. Build the project.
   If you did not use Visual Studio to edit the *resx* file, this will update the corresponding *.Designer.cs* file with a new class property that can be used to get the string value.
   Regardless of how you edited the resx file, building will update all the *xlf* files, copying the English translation.
   This is normal.
   It sets an attribute to specify that the resource is new, and the [OneLocBuild][1] workflow will later create a pull request with the actual translations.
1. Use the string in whatever output message the code change requires.

If the string has words or phrases that should not be translated in any language, in the comment section of the resx, use something like `{Locked="allowUntrustedRoot"}`.
You can have multiple locked messages by repeating the blocks, for example `{Locked="one"}{Locked="two"}`.

## Fixing translation errors

All translations go through Microsoft's [OneLocBuild][1] system, so do not create a pull request updating xlf files in this repo.
Instead, file a bug at [NuGet/Home](https://github.com/NuGet/Home) and we'll file an internal bug with the OneLoc team to get it fixed.
After they action the change in their backend, an automated system will create a pull request updating the xlf files.

## Do's and Don'ts

If you want your strings localized properly...

❌ Do not use string literals.  Example:

```C#
private void DoSomething(ILogger logger)
{
    ...
    logger.LogWarning("This is a warning.");
}
```

❌ Do not use format string literals.  Example:

```C#
private void DoSomething(ILogger logger)
{
    ...
    string message = string.Format("'{0}' is not a valid version.", version);

    logger.LogError(message);
}
```

✔️ Do use resource strings.  Example:

```C#
private void DoSomething(ILogger logger)
{
    ...
    string message = string.Format(Strings.DownloadingPackage, packageId, packageVersion);

    logger.LogInformation(message);
}
```

✔️ Do add appropriate functional and contextual comments.  [Here][8] is an example of a functional comment.  Our .resx files contain contextual comments, but the comments do not flow automatically into .lcg files.  [Here][9] is an example of a .resx comment (in a different repository) which has both functional and contextual commenting.

✔️ Do use helper methods for strings that are used in multiple places.  Example:

```C#
internal string Error_InvalidOptionValue(string actualValue, string allowedValues)
{
    string message = string.Format(Strings.Error_InvalidOptionValue, actualValue, allowedValues);
    return message;
}
```

By using `Strings.Error_InvalidOptionValue` in only a single place, it means that if the string is modified to add additional `{x}` placeholders, compiler errors will prevent runtime errors if not all usage is updated.

## Resource commenting

[This internal site][0] has a good explanation of the what, why, and how of resource commenting.  In brief, resource comments can contain both:

- functional comments, which enable tools to understand your intent
- contextual comments, which enable human translators to understand your intent

Both are important.

Starting from NuGet 7.0, we use [OneLocBuild][1], like other .NET Arcade-enabled repos, and adding resource comments can be done using the same Visual Studio Resource Editor that you use to add and modify resource strings in .resx files.

## Resources

- [All About Localization][2]

[0]: https://aka.ms/commenting
[1]: https://github.com/dotnet/arcade/blob/main/Documentation/OneLocBuild.md
[2]: https://aka.ms/allaboutloc
