dotnet build /r Product.proj
dotnet pack --no-restore -o artifacts --no-build Product.proj
