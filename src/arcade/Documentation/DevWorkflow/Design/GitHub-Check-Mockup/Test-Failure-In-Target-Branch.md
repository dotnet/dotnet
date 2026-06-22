# Build Result Analysis

## Shared test failures with Target Branch
:warning: There are test failures in this build for pipelines that are also failing in master.

Issue tracking this failure [dotnet/runtime#1234](https://github.com/dotnet/runtime/issues/1234)

### System.Net.Tests.HttpWebRequestTest_Sync.ReadWriteTimeout_CancelsResponse [[Test]](https://dev.azure.com/dnceng/public/_build/results?buildId=975143&view=ms.vss-test-web.build-test-results-tab&runId=30605638&resultId=148065&paneView=debug) [[History]](https://dev.azure.com/dnceng/public/_build/results?buildId=975143&view=ms.vss-test-web.build-test-results-tab&runId=30605638&resultId=148065&paneView=history) [[Artifacts/Logs]](https://dev.azure.com/dnceng/public/_build/results?buildId=975143&view=ms.vss-test-web.build-test-results-tab&runId=30605638&resultId=148065&paneView=dotnet-dnceng.dnceng-build-release-tasks.helix-test-information-tab)
<dl>
<dt>Exception Message<dt>
<dd><pre>Assert.Throws() Failure
Expected: typeof(System.IO.IOException)
Actual:   typeof(System.Net.WebException): An error occurred while sending the request.
</pre></dd>

<dt>Callstack</dt>
<dd><pre>   at System.Net.HttpWebRequest.GetResponse() in /_/src/libraries/System.Net.Requests/src/System/Net/HttpWebRequest.cs:line 1032</pre></dd>
<dt>Target branch (master) status</dt>
<dd>:small_red_triangle_down: Current failing in master
<dt>Test failure rate</dt>
<dd>Test has failed 45 out of 245 runs in master (18.36%)</dd>
<dt>Test history</dt>
<dd>Test started failing in master on 2/5.<br/>
<a href="https://dev.azure.com/dnceng/public/_build/results?buildId=975143&view=ms.vss-test-web.build-test-results-tab&runId=30605638&resultId=148065&paneView=debug) [[History]](https://dev.azure.com/dnceng/public/_build/results?buildId=975143&view=ms.vss-test-web.build-test-results-tab&runId=30605638&resultId=148065&paneView=history) [[Artifacts/Logs]](https://dev.azure.com/dnceng/public/_build/results?buildId=975143&view=ms.vss-test-web.build-test-results-tab&runId=30605638&resultId=148065&paneView=dotnet-dnceng.dnceng-build-release-tasks.helix-test-information-tab">[First Failing Test Result in master, build 20200208.12, on 2/8]</a><br/>
<a href="https://dev.azure.com/dnceng/public/_build/results?buildId=975143&view=ms.vss-test-web.build-test-results-tab&runId=30605638&resultId=148065&paneView=debug) [[History]](https://dev.azure.com/dnceng/public/_build/results?buildId=975143&view=ms.vss-test-web.build-test-results-tab&runId=30605638&resultId=148065&paneView=history) [[Artifacts/Logs]](https://dev.azure.com/dnceng/public/_build/results?buildId=975143&view=ms.vss-test-web.build-test-results-tab&runId=30605638&resultId=148065&paneView=dotnet-dnceng.dnceng-build-release-tasks.helix-test-information-tab">[Current/Latest Failing Test Result in master, build 20200209.09, on 2/9]</a>

<dt>:grey_exclamation: This is a new test, introduced on 2/8</dt>
</dl>
