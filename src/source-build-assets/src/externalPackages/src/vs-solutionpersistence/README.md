# Microsoft.VisualStudio.SolutionPersistence

## About

Shared serializers and models for Visual Studio solution files. Handles traditional .sln file and new .slnx file.

[![NuGet package](https://img.shields.io/nuget/v/Microsoft.VisualStudio.SolutionPersistence.svg)](https://nuget.org/packages/Microsoft.VisualStudio.SolutionPersistence)


## Features

* Serializers for solution files, traditional text based .sln file, and XML based .slnx file.
* Object model for manipulating file contents in common way.
* SLN File Format
  - Consistent with legacy Visual Studio parsing code to ensure consistent behavior when reading .sln files.
  - Model extension that allows specifying the file encoding.
* SLNX File Format
  - Simplified format to make merge conflicts easier to resolve.
  - Preserves user added elements, comments and whitespace when possible. Updaing only modifies changed elements to reduce chances of changing user content.
  - Simplified configuration rules and default logic reduce the file size.
  - Model extension that allows specifying XML formatting.

## Using

The entry point to serializers can be found on the SolutionSerializers static class. This has the helper GetSerializerByMoniker that can pick the serializer for a file extension, or a specific serializers can be used.

SolutionModel is the class that represents a solution, it has a SolutionProjectModel for each project in the solution as well as SolutionFolderModel to represent solution folders (which are logical constructs and do not necessarily represent directories).

For the most part any serializer specific concepts are removed from the model, but it does allow for serializer specific properties to be added using a ISerializerModelExtension. Each serializer has a CreateModelExtension method for creating a default model extension, as well as overloads for specifying options that are specific to each file format. For example .sln files support writing different encodings (ASCII, UTF-8 w/ BOM, and UTF-16). While .slnx files have options to customize XML formatting.

See [Samples Wiki](https://github.com/microsoft/vs-solutionpersistence/wiki/Samples)

## Trademark

This project may contain trademarks or logos for projects, products, or services. Authorized use of Microsoft trademarks or logos is subject to and must follow Microsoft’s Trademark & Brand Guidelines. Use of Microsoft trademarks or logos in modified versions of this project must not cause confusion or imply Microsoft sponsorship. Any use of third-party trademarks or logos are subject to those third-party’s policies.

