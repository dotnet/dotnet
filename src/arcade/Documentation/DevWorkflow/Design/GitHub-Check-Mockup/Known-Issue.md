A build break has been reported for investigation

## Details

- Build: dotnet/runtime#12345
- Build error: Build Linux arm Debug AllSubsets_Mono / Build Product [Log](https://dev.azure.com/dnceng/public/_build/results?buildId=1397322&view=logs&j=e4e455c5-b032-5051-d2c5-a64982c9ac45&t=9fb1743d-454a-5c96-e751-7c57c8f44cc2&l=3456)
- Build start date: 

To triage this issue:

* Open the build failure log above and investigate
* Add a comment explaining what you found the log
* Is it a new known issue? 
  * Yes
    * Edit this issue and add an error string in the json below that can help us match this issue with future build breaks
    * Remove label "Build Error Needs Triage" 
    * Add the label "Known build error"
  * No - Close the issue

```json
{
    "errorMessage" : ""
}
```
