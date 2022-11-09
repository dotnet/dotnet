# SDK README table generator

To generate the table of [SDK per check in build download table](https://github.com/dotnet/installer).

1. Edit the `inputBranches` in Program.fs when there is a new branch created. Make edits to table.fs if there are changes to the specific items or order you want showing up.
2. Run the program, it will generate the table with updated branch. It is not too smart, so if there is new platform added, we need more to change the program. But it is still better than edit it by hand.
3. Replace the table in the main readme file https://github.com/dotnet/installer/blob/main/README.md
    NOTE: The output does not replace the entire file so you have to copy/paste over the pieces that are generated which is the middle of the file
4. Run tests and update the tests

I wrote it to learn F#, since it is almost the "99 bottle of beer" kata. Please point out places I can improve if you are interested.
