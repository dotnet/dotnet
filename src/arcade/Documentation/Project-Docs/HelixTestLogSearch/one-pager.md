# ✔️ Introduction
## Requirements
We want to create a REST API that allows users to find the frequency of a specific error string in a repository's Helix test logs. So we want to be able to query an endpoint with a `repository`, `error_string`, `start_date`, and `end_date` to get a list of all the error occurrences of the string in the build logs matching those arguments.

# 🖥️ Implementation
## High-level diagram
<img width="3007" alt="Project diagram" src="./Resources/ProjectDiagram.png">

## Logic
1. Take and sanitize user input
2. Execute Kusto query (filter by repository, date_ranges, fail status)
3. Iterate through Kusto's results to retrieve log files' path in Azure Storage
4. Read log file's content line by line using file stream
5. Use string searching/matching to find error string in the lines.
6. If match is found, keep track of it and increment occurrences found.
7. Return the results found as a JSON object. 

### Kusto query
    Jobs 
    | where Repository == REPO_NAME 
    | project JobId, IsExternal 
    | join kind = inner WorkItems on JobId 
    | project JobId, FriendlyName, Status, Started, Finished, ConsoleUri, QueueName, Attempt, IsExternal 
    | where Status == 'Fail' 
    | where Started between(START_DATE .. END_DATE) | where IsExternal == 1";

## Input
- Arguments
    - `repository`
    - `error_string`
    - `start_date`
    - `end_date`

- Constraints
    - `repository` must be an existing, public repository. Its spelling must match the repo name exactly.
    - `error_string` should probably have some kind of limit on length.
    - The duration between `start_date` and `end_date` should have a maximum of 7 days). If user input exceeds this value, one possible way of handling this is to just query jobs between our defined max number of months before the given `end_date` and alert the user that this was done instead of their original query.

## Dependencies
- Kusto
- Azure Account Storage

## String Matching
The three possible string matching methods ranked by speed/performance are:
1. C# `String.contains`, `String.replace`, etc
2. Boyer-Moore string searching algorithm
3. Regex

This ranking is based on the following articles:

[Boyer-Moore VS String.contains](http://www.blackbeltcoder.com/Articles/algorithms/fast-text-search-with-boyer-moore)

**TLDR;** Although Boyer-Moore is considered one of the fastest string-matching algorithms, C#'s `String.contains` method is faster as it uses assembly optimization. Although we might need a performance test since we will need to go line by line and load the strings from each log file if we use `String.contains` and that might take even longer.

[String.contains VS Regex.isMatch](https://theburningmonk.com/2012/05/performance-test-string-contains-vs-string-indexof-vs-regex-ismatch/#:~:text=As%20you%20can%20see%2C%20Regex.IsMatch%20is%20by%20far,turned%20out%20to%20be%20significantly%20faster%20than%20String.IndexOf.)

**TLDR;** Regex matching is way slower than String methods. It's only more useful if we want to pattern match as opposed to finding a fixed string. (Actually this raises the question - do we want to pattern match?)

Also, this article [Fastest Ways to Count Substring Occurences in C#](https://cc.davelozinski.com/c-sharp/c-net-fastest-way-count-substring-occurrences-string) compares the speeds of different methods of counting substring occurences.

**TLDR;** Using BCL was the fastest method for the following performance tests:
> Counting the number of times 1 string occurs in 5,000, 25,000, 100,000, and 1,000,000 million strings.
> 
> Counting the number of times 100 strings occur in 5,000, 25,000, 100,000, and 1,000,000 million strings.
> 
> Counting the number of times 1,000 strings occur in 5,000, 25,000, 100,000, and 1,000,000 million strings.

It also corroborates the article saying Regex matching is very slow for long strings.

**❕ Decision is to use `String.Contains` (BCL) for now and stick with fixed string matching. Notes were made in additional features section to possibly include pattern matching down the road.**

## File Reading
Since we will potentially need to be reading text from thousands of files, it's worth taking a look at fastest ways to read file input. The following article benchmarks the time it takes for different ways of reading file input. 

[Fastest Ways to Read Text Files in C#](https://cc.davelozinski.com/c-sharp/fastest-way-to-read-text-files)

**TLDR;** There was no one fastest method found, but in general, reading line by line and storing each line into a string was fast, and should be sufficient for this program. We can also make it faster using parallel threads if needed.

**❕ We want to read different log files async using some version of `Task.WhenAll` to read the files concurrently.**

## Output
#### Possible JSON output:
        {
          "filter": {
            "repository": "...",
            "error_string": "",
            "start_date": "",
            "end_date": "",
            "num_hits": 00,
          },
          "hits": 
          [
            {
              "document_uri": "uri to document",
              "job_id": "helix guid"
              "friendly_name": "",
              "started": "",
              "finished": "",
              "queue_name": "",
            },
            {
              "document_uri": "uri to document",
              "job_id": "helix guid"
              "friendly_name": "",
              "started": "",
              "finished": "",
              "queue_name": "",
            },
            ...
          ]
        }
 
# 👓 Proof-of-Concept
The plan for now is reading log files line by line and using `String.Contains`. We also want to use `Tasks` to parse each log file in parallel. Currently, I’m taking the following steps to implement POC:

1. Write code for parsing a file using a hardcoded URI and getting in the data we want to return
2. Replace the hardcoded URI with the actual URIs retrieved from a Kusto Query and looping through multiple URIs (and eventually the thousands that are actually returned).
3. Deploy the POC so that we can run it on the same data centre that the logs are stored so we can see the actual speed of the program

- Will test out string matching on a fixed number of log files first to see the speed on a local machine (and we also want to see the speed of actually running it on servers)

# 📓 Additional Notes

### Possible additional features
- Include line number and character index that a string match was found
- Allow user to pass either/or these 2 options as arguments:
    - `repository`, `error_string`, `date_range`
    - `build_id` list
- Taking an optional parameter for context lines (e.g also return the 5 lines surrounding the hit line - think GDB)
- Allowing pattern matching using regex (currently only allow for fixed string matching)
- Allow user to pass token to authenticate and allow search in non-external jobs
- Include retries in the output (`Attempt` and `LocalIteration` columns from `Files` table)

### Issues/questions to look into down the road
- Possibly use a profiler (like VS profiler) to look more into performance
- Eventually we want to deploy to use the same data centres as the logs in Azure
- Look more into handling failure cases like limiting user input i.e only 1 outstanding request allowed per person also “(limiting the input sizes, like only X total days, or Y total logs to scan), returning a partial result if we run out of time, a stateful server request, where you could ask "hey, I started this query a bit ago, do you have the answer yet"... Lots of exciting options!
- Keep in mind the constraints for date range input - for now we are using a 7 day max duration but this can be changed if it is actually faster than expected



<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CProject-Docs%5CHelixTestLogSearch%5Cone-pager.md)](https://helix.dot.net/f/p/5?p=Documentation%5CProject-Docs%5CHelixTestLogSearch%5Cone-pager.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CProject-Docs%5CHelixTestLogSearch%5Cone-pager.md)</sub>
<!-- End Generated Content-->
