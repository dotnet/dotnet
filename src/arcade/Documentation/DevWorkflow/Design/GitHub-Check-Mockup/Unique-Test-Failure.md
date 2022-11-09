# Build Result Analysis

## Tests Failed
There are test failures in this build for pipelines that are not currently failing in master

### ❌ System.Net.Tests.HttpWebRequestTest_Sync.ReadWriteTimeout_CancelsResponse [[Test]](https://dev.azure.com/dnceng/public/_build/results?buildId=975143&view=ms.vss-test-web.build-test-results-tab&runId=30605638&resultId=148065&paneView=debug) [[History]](https://dev.azure.com/dnceng/public/_build/results?buildId=975143&view=ms.vss-test-web.build-test-results-tab&runId=30605638&resultId=148065&paneView=history) [[Artifacts/Logs]](https://dev.azure.com/dnceng/public/_build/results?buildId=975143&view=ms.vss-test-web.build-test-results-tab&runId=30605638&resultId=148065&paneView=dotnet-dnceng.dnceng-build-release-tasks.helix-test-information-tab)
<dl>
<dt>Exception Message<dt>
<dd><pre>Assert.Throws() Failure
Expected: typeof(System.IO.IOException)
Actual:   typeof(System.Net.WebException): An error occurred while sending the request.
---- System.Net.WebException : An error occurred while sending the request.
-------- System.Net.Http.HttpRequestException : An error occurred while sending the request.
------------ System.IO.IOException : Unable to read data from the transport connection: Operation timed out.
---------------- System.Net.Sockets.SocketException : Operation timed out</pre></dd>

<dt>Callstack</dt>
<dd><pre>   at System.Net.HttpWebRequest.GetResponse() in /_/src/libraries/System.Net.Requests/src/System/Net/HttpWebRequest.cs:line 1032
   at System.Net.Tests.HttpWebRequestTest_Sync.<>c__DisplayClass1_0.<GetResponseAsync>b__0() in /_/src/libraries/System.Net.Requests/tests/HttpWebRequestTest.cs:line 36
   at System.Threading.Tasks.Task`1.InnerInvoke() in /_/src/libraries/System.Private.CoreLib/src/System/Threading/Tasks/Future.cs:line 503
   at System.Threading.Tasks.Task.<>c.<.cctor>b__278_0(Object obj) in /_/src/libraries/System.Private.CoreLib/src/System/Threading/Tasks/Task.cs:line 2367</pre></dd>

<dt>Failing Configuration</dt>
<dd><ul>
<li>Windows.10.Amd64.Open
<a href="https://dev.azure.com/dnceng/public/_build/results?buildId=975143&view=ms.vss-test-web.build-test-results-tab&runId=30605638&resultId=148065&paneView=debug">[Test]</a>
<a href="https://dev.azure.com/dnceng/public/_build/results?buildId=975143&view=ms.vss-test-web.build-test-results-tab&runId=30605638&resultId=148065&paneView=history">[History]</a>
<a href="https://dev.azure.com/dnceng/public/_build/results?buildId=975143&view=ms.vss-test-web.build-test-results-tab&runId=30605638&resultId=148065&paneView=dotnet-dnceng.dnceng-build-release-tasks.helix-test-information-tab">[Artifacts/Logs]</a>
</li>
<li>Windows.10.RH1.Client.Open
<a href="https://dev.azure.com/dnceng/public/_build/results?buildId=975143&view=ms.vss-test-web.build-test-results-tab&runId=30605638&resultId=148065&paneView=debug">[Test]</a>
<a href="https://dev.azure.com/dnceng/public/_build/results?buildId=975143&view=ms.vss-test-web.build-test-results-tab&runId=30605638&resultId=148065&paneView=history">[History]</a>
<a href="https://dev.azure.com/dnceng/public/_build/results?buildId=975143&view=ms.vss-test-web.build-test-results-tab&runId=30605638&resultId=148065&paneView=dotnet-dnceng.dnceng-build-release-tasks.helix-test-information-tab">[Artifacts/Logs]</a>
</li>
<li>Windows.8.Client.Open
<a href="https://dev.azure.com/dnceng/public/_build/results?buildId=975143&view=ms.vss-test-web.build-test-results-tab&runId=30605638&resultId=148065&paneView=debug">[Test]</a>
<a href="https://dev.azure.com/dnceng/public/_build/results?buildId=975143&view=ms.vss-test-web.build-test-results-tab&runId=30605638&resultId=148065&paneView=history">[History]</a>
<a href="https://dev.azure.com/dnceng/public/_build/results?buildId=975143&view=ms.vss-test-web.build-test-results-tab&runId=30605638&resultId=148065&paneView=dotnet-dnceng.dnceng-build-release-tasks.helix-test-information-tab">[Artifacts/Logs]</a>
</li>
<li>... and 12 other ... </li>
</ul></dd>
<dt>:small_blue_diamond: Test is not failing in master<dt>
<dt>:grey_exclamation: This is a new test, introduced on 2/8<dt>
</dl>


### ❌ System.Net.Tests.SomeOtherTest [[Test]](https://dev.azure.com/dnceng/public/_build/results?buildId=975143&view=ms.vss-test-web.build-test-results-tab&runId=30605638&resultId=148065&paneView=debug) [[History]](https://dev.azure.com/dnceng/public/_build/results?buildId=975143&view=ms.vss-test-web.build-test-results-tab&runId=30605638&resultId=148065&paneView=history) [[Artifacts/Logs]](https://dev.azure.com/dnceng/public/_build/results?buildId=975143&view=ms.vss-test-web.build-test-results-tab&runId=30605638&resultId=148065&paneView=dotnet-dnceng.dnceng-build-release-tasks.helix-test-information-tab)
<dl>
<dt>Exception Message<dt>
<dd><pre>Assert.Throws() Failure
Expected: typeof(System.IO.IOException)
Actual:   typeof(System.Net.WebException): An error occurred while sending the request.
---- System.Net.WebException : An error occurred while sending the request.
-------- System.Net.Http.HttpRequestException : An error occurred while sending the request.
------------ System.IO.IOException : Unable to read data from the transport connection: Operation timed out.
---------------- System.Net.Sockets.SocketException : Operation timed out</pre></dd>

<dt>Callstack</dt>
<dd><pre>   at System.Net.HttpWebRequest.GetResponse() in /_/src/libraries/System.Net.Requests/src/System/Net/HttpWebRequest.cs:line 1032
   at System.Net.Tests.HttpWebRequestTest_Sync.<>c__DisplayClass1_0.<GetResponseAsync>b__0() in /_/src/libraries/System.Net.Requests/tests/HttpWebRequestTest.cs:line 36
   at System.Threading.Tasks.Task`1.InnerInvoke() in /_/src/libraries/System.Private.CoreLib/src/System/Threading/Tasks/Future.cs:line 503
   at System.Threading.Tasks.Task.<>c.<.cctor>b__278_0(Object obj) in /_/src/libraries/System.Private.CoreLib/src/System/Threading/Tasks/Task.cs:line 2367</pre></dd>

<dt>Failing Configuration</dt>
<dd><ul>
<li>Windows.10.Amd64.Open
<a href="https://dev.azure.com/dnceng/public/_build/results?buildId=975143&view=ms.vss-test-web.build-test-results-tab&runId=30605638&resultId=148065&paneView=debug">[Test]</a>
<a href="https://dev.azure.com/dnceng/public/_build/results?buildId=975143&view=ms.vss-test-web.build-test-results-tab&runId=30605638&resultId=148065&paneView=history">[History]</a>
<a href="https://dev.azure.com/dnceng/public/_build/results?buildId=975143&view=ms.vss-test-web.build-test-results-tab&runId=30605638&resultId=148065&paneView=dotnet-dnceng.dnceng-build-release-tasks.helix-test-information-tab">[Artifacts/Logs]</a>
</li>
</ul></dd>
<dt>:small_blue_diamond: Test is not failing in master<dt>
</dl>

#### ... and 3 other tests [[All Test Results]](https://dev.azure.com/dnceng/public/_build/results?buildId=975143&view=ms.vss-test-web.build-test-results-tab)


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CDevWorkflow%5CDesign%5CGitHub-Check-Mockup%5CUnique-Test-Failure.md)](https://helix.dot.net/f/p/5?p=Documentation%5CDevWorkflow%5CDesign%5CGitHub-Check-Mockup%5CUnique-Test-Failure.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CDevWorkflow%5CDesign%5CGitHub-Check-Mockup%5CUnique-Test-Failure.md)</sub>
<!-- End Generated Content-->
