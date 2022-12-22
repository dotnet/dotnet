# Build Result Analysis

## Non-Deterministic Test Failures Detected
:small_blue_diamond: There are test failures in this build for pipelines that are also failing in master

### System.Net.Tests.HttpWebRequestTest_Sync.ReadWriteTimeout_CancelsResponse [[Test]](https://dev.azure.com/dnceng/public/_build/results?buildId=975143&view=ms.vss-test-web.build-test-results-tab&runId=30605638&resultId=148065&paneView=debug) [[History]](https://dev.azure.com/dnceng/public/_build/results?buildId=975143&view=ms.vss-test-web.build-test-results-tab&runId=30605638&resultId=148065&paneView=history) [[Artifacts/Logs]](https://dev.azure.com/dnceng/public/_build/results?buildId=975143&view=ms.vss-test-web.build-test-results-tab&runId=30605638&resultId=148065&paneView=dotnet-dnceng.dnceng-build-release-tasks.helix-test-information-tab)
<dl>
<dt>Exception Message<dt>
<dd><pre>Assert.Throws() Failure
Expected: typeof(System.IO.IOException)
Actual:   typeof(System.Net.WebException): An error occurred while sending the request.
</pre></dd>

<dt>Callstack</dt>
<dd><pre>   at System.Net.HttpWebRequest.GetResponse() in /_/src/libraries/System.Net.Requests/src/System/Net/HttpWebRequest.cs:line 1032</pre></dd>
<dt>Reruns required</dt>
<dd>1 (out of 3) executions failed in this build</dd>
<dt>Pass Rate</dt>
<dd><ul>
<li>This this fails in 2 out of 25 (8%) master builds since 2/8 <a href="https://dev.azure.com/dnceng/public/_build/results?buildId=975143&view=ms.vss-test-web.build-test-results-tab&runId=30605638&resultId=148065&paneView=debug">[Most Recent Failure in master, build 20200208.12]</a></li>
<li>This this was non-deterministic in 3 out of 49 (6.12%) of PR's targetting master builds since 2/8 <a href="https://dev.azure.com/dnceng/public/_build/results?buildId=975143&view=ms.vss-test-web.build-test-results-tab&runId=30605638&resultId=148065&paneView=debug">[Most Recent Failure in master, build 20200208.12]</a></li>
</ul></dd>
<dt>:grey_exclamation: This is a new test, introduced on 2/8</dt>
</dl>


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CDevWorkflow%5CDesign%5CGitHub-Check-Mockup%5CTest-Pass-On-Retry.md)](https://helix.dot.net/f/p/5?p=Documentation%5CDevWorkflow%5CDesign%5CGitHub-Check-Mockup%5CTest-Pass-On-Retry.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CDevWorkflow%5CDesign%5CGitHub-Check-Mockup%5CTest-Pass-On-Retry.md)</sub>
<!-- End Generated Content-->
