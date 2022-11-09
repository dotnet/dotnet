# How to contribute

## Update existing templates

You can submit changes for templates in this repo by submitting a pull request. If you make changes to any
`content/*/.template.config/template.json` files, build locally and include any
`content/*/.template.config/localize/` changes in your pull request. (Your build may update the strings in those
files for later localization.)

## Add to the community ASP.NET Core SPA templates list

You can add a project template to the community-owned ASP.NET Core SPA templates listing by submitting a pull request to update [README.md](https://github.com/dotnet/spa-templates/blob/main/README.md).

Please ensure your pull request adheres to the following:

* Make an individual pull request for each new project template listing.
* Use the following format: \[NAME\]\(TEMPLATE PACKAGE LINK\) - DESCRIPTION.
* Keep descriptions simple.
* End all descriptions with a period.
* Remove trailing whitespace.
* Add new listings in alphabetical order.
* Project templates should be of high quality, maintained, and follow the patterns and conventions used by the templates maintained in this repo.
* Project templates should be open source and should reference the source project from the package metadata.

See the ASP.NET Core docs for details on [integrating ASP.NET Core with frontend JavaScript frameworks](https://aka.ms/aspnetcorespa) and the .NET docs for instructions on [creating .NET project templates](https://docs.microsoft.com/dotnet/core/tutorials/cli-templates-create-project-template).
