dotnet clean Product.proj > clean.log
dotnet build /r Product.proj
dotnet pack --no-restore -o c:\localpackages --no-build Product.proj
