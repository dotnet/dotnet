# Helix Test Log Search POC Summary
To recall, this POC queries Kusto for failed test logs for a specified repository and date range, and parses these logs line by line for a given string.

In general, since we are limiting the number of days a user can search for a string, depending on the number of files that are returned by the Kusto query, the program will run for anywhere between 20s and 1m30s. 

## 📋 How to use
The program requires all arguments to be passed when running it in command line:

        dotnet run --errorString={SEARCH_STRING} --repository={REPOSITORY_NAME} --startDate={STARTING_DATE_RANGE} --endDate={ENDING_DATE_RANGE} --mode={RESULT_TYPE}
        
See the Input section below for more details on input constraints.


### Input
Run the console app in terminal and pass all of the following options:

- `errorString`: The error string the program will search for
- `repository`: The public repository whose test logs will be parsed
- `startDate`: The start date of the date range in which the test logs will be searched. This argument must be in the format **yyyy/MM/dd** and is in UTC.
- `endDate`: The end date of the date range in which the test logs will be searched. This argument must be in the format **yyyy/MM/dd** and is in UTC.
- `mode`: Either `"Hits"` or `"HitsPerFile"`. Hits will return an array of all occurences of `errorString`, including each occurrence's line number within its test log file. HitsPerFile will return an array of hits per file, including the count of occurrences per file.

#### Example

    dotnet run --errorString="UseCallback_BadCertificate_ExpectedPolicyErrors" --repository=dotnet/runtime --startDate=2022-06-01 --endDate=2022-06-07 --mode=HitsPerFile
    
This command will search for all instances of the string "UseCallback_BadCertificate_ExpectedPolicyErrors" in all the `dotnet/runtime` Helix test logs created between June 1, 2022 and June 7, 2022. It will return the number of hits found per file. 

### Output

Here is some sample output showing the 2 different response types.

#### Hits
        {
          "filter": {
            "repository": "dotnet/runtime",
            "errorString": "UseCallback_BadCertificate_ExpectedPolicyErrors",
            "startDate": "2022-05-25T07:00:00Z",
            "endDate": "2022-06-07T07:00:00Z"
          },
          "hits": [
            {
              "lineContent": "System.Net.Http.Functional.Tests.SocketsHttpHandler_HttpClientHandler_ServerCertificates_Test.UseCallback_BadCertificate_ExpectedPolicyErrors(url: \"https://wrong.host.badssl.com/\", expectedErrors: RemoteCertificateNameMismatch) [FAIL]",
              "lineNumber": 129,
              "jobId": 19894144,
              "friendlyName": "System.Net.Http.Functional.Tests",
              "status": "Fail",
              "started": "2022-06-01T12:36:28.573Z",
              "finished": "2022-06-01T12:42:46.786Z",
              "consoleUri": "https://helixre107v0xdeko0k025g8.blob.core.windows.net/dotnet-runtime-refs-heads-release-50-159e0decb4474dbfbb/System.Net.Http.Functional.Tests/1/console.52339d8a.log?helixlogtype=result",
              "queueName": "ubuntu.1804.armarch.open",
              "attempt": 1
            },
            {
              "lineContent": "/_/src/libraries/Common/tests/System/Net/Http/HttpClientHandlerTest.ServerCertificates.cs(334,0): at System.Net.Http.Functional.Tests.HttpClientHandler_ServerCertificates_Test.UseCallback_BadCertificate_ExpectedPolicyErrors_Helper(String url, String useHttp2String, SslPolicyErrors expectedErrors)",
              "lineNumber": 142,
              "jobId": 19894144,
              "friendlyName": "System.Net.Http.Functional.Tests",
              "status": "Fail",
              "started": "2022-06-01T12:36:28.573Z",
              "finished": "2022-06-01T12:42:46.786Z",
              "consoleUri": "https://helixre107v0xdeko0k025g8.blob.core.windows.net/dotnet-runtime-refs-heads-release-50-159e0decb4474dbfbb/System.Net.Http.Functional.Tests/1/console.52339d8a.log?helixlogtype=result",
              "queueName": "ubuntu.1804.armarch.open",
              "attempt": 1
            },
            ...
          ],
          "occurrenceCount": 96,
          "filesCount": 24
        }

#### Hits per File
        {
          "filter": {
            "repository": "dotnet/runtime",
            "errorString": "UseCallback_BadCertificate_ExpectedPolicyErrors",
            "startDate": "2022-05-25T07:00:00Z",
            "endDate": "2022-06-07T07:00:00Z"
          },
          "hits": [
            {
              "occurrences": 4,
              "jobId": 19902082,
              "friendlyName": "System.Net.Http.Functional.Tests",
              "status": "Fail",
              "started": "2022-06-02T12:16:47.584Z",
              "finished": "2022-06-02T12:21:47.023Z",
              "consoleUri": "https://helixre107v0xdeko0k025g8.blob.core.windows.net/dotnet-runtime-refs-heads-release-50-ab8bca7d45fc4cd0bb/System.Net.Http.Functional.Tests/1/console.e36c2801.log?helixlogtype=result",
              "queueName": "ubuntu.1804.armarch.open",
              "attempt": 1
            },
            {
              "occurrences": 4,
              "jobId": 19909540,
              "friendlyName": "System.Net.Http.Functional.Tests",
              "status": "Fail",
              "started": "2022-06-03T13:37:32.456Z",
              "finished": "2022-06-03T13:43:35.135Z",
              "consoleUri": "https://helixre107v0xdeko0k025g8.blob.core.windows.net/dotnet-runtime-refs-heads-release-50-e3fe329caffc4ab3be/System.Net.Http.Functional.Tests/1/console.76909294.log?helixlogtype=result",
              "queueName": "ubuntu.1804.armarch.open",
              "attempt": 1
            },
            ...
          ],
          "occurrenceCount": 547,
          "filesCount": 17
        }


## 🔍 Performance findings

We can take a look at how long the program ran for different volumes of logs retrieved and parsed, as well as for different lengths of the search string.

For one given repo and a one word string, we found:

| Start date  | End date    | # of Occurrences | # of Files with hits     | Total time elapsed | Lines scanned/sec | Files scanned/sec | Total files scanned 
| ----------- | ----------- |-------------- | --------------------- | ------------------ | ----------------- | ----------------- | ------------------ | 
| 2022/06/01  | 2022/06/07   | 2528           | 20                    | 00:00:31.7819027   | 2532.73           | 0.69              | 22
| 2022/05/25  | 2022/06/07   | 22768           | 510                   | 00:00:35.2533931   | 41198.75        | 14.60              | 515
| 2022/05/07  | 2022/06/07   | 124973           | 3023                   | 00:00:41.9848476   | 193610.51        | 79.95             | 3357

For one given repo and a string with 6 words:

| Start date  | End date    | # of Occurrences | # of Files with hits     | Total time elapsed | Lines scanned/sec | Files scanned/sec | Total files scanned 
| ----------- | ----------- |-------------- | --------------------- | ------------------ | ----------------- | ----------------- | ------------------ | 
| 2022/06/01  | 2022/06/07   | 547           | 17                    | 00:00:31.2928707   | 2572.31           | 0.70              | 22
| 2022/05/25  | 2022/06/07   | 6106           | 124                   | 00:00:28.0314279   | 51813.12         | 18.37              | 515
| 2022/05/07  | 2022/06/07   | 16431           | 310                   | 00:00:49.0204510   | 165822.79         | 68.48              | 3357

Note that the time taken can vary based on the repository, date range, network, etc so this is just a snapshot of a couple of times of running the program. But in general the times are pretty consistent and stay under a minute. 

## 🤔 Future considerations
- More input sanitization to limit number of rows returned by Kusto and avoid timeouts, also to avoid injections into the query
- Error handling for if we try to parse a file that doesn't exist on the server anymore
- Before implementing functionality to accept a token from the user to access non-public work item logs, consider how to handle when a user enters a private repo



<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CProject-Docs%5CHelixTestLogSearch%5Cpoc-summary.md)](https://helix.dot.net/f/p/5?p=Documentation%5CProject-Docs%5CHelixTestLogSearch%5Cpoc-summary.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CProject-Docs%5CHelixTestLogSearch%5Cpoc-summary.md)</sub>
<!-- End Generated Content-->
