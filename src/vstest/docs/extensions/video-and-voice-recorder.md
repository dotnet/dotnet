Video and Voice recorder data collector is a VSTest data collector that ships in `Microsoft.TestPlatform` nuget package and `Microsoft.VisualStudio.TestTools.TestPlatform.V2.CLI` (the VSIX we insert into VisualStudio).

It serves to record video and sound of each test, and optionally publish the result only for failing tests. 

Example usage is:

```bash
vstest.console.exe --collect:"Screen and Voice Recorder" bin\Debug\net10.0\mstest320.dll
```

This will record video to the TestResults folder (under some guid).

Additional options can be provided via runsettings.

```xml
<DataCollector uri="datacollector://microsoft/VideoRecorder/1.0" assemblyQualifiedName="Microsoft.VisualStudio.TestTools.DataCollection.VideoRecorder.VideoRecorderDataCollector, Microsoft.VisualStudio.TestTools.DataCollection.VideoRecorder, Version=15.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" friendlyName="Screen and Voice Recorder">
        <!--Video data collector was introduced in Visual Studio 2017 version 15.5 -->
        <Configuration>
          <!-- Set "sendRecordedMediaForPassedTestCase" to "false" to add video attachments to failed tests only -->
          <MediaRecorder sendRecordedMediaForPassedTestCase="true"  xmlns="">           
            <ScreenCaptureVideo bitRate="512" frameRate="2" quality="20" />
          </MediaRecorder>
        </Configuration>
      </DataCollector>
```


Official examples are here, including the runsettings shown above: 
https://learn.microsoft.com/en-us/visualstudio/test/configure-unit-tests-by-using-a-dot-runsettings-file?view=visualstudio#videorecorder-data-collector

https://learn.microsoft.com/en-us/visualstudio/test/configure-unit-tests-by-using-a-dot-runsettings-file?view=visualstudio#example-runsettings-file

Files in the shipment (and similar layout in the VSIX):

tools\net462\Common7\IDE\Extensions\TestPlatform\Extensions\Microsoft.VisualStudio.TestTools.DataCollection.MediaRecorder.Model.dll
tools\net462\Common7\IDE\Extensions\TestPlatform\Extensions\Microsoft.VisualStudio.TestTools.DataCollection.VideoRecorderCollector.dll
tools\net462\Common7\IDE\Extensions\TestPlatform\Extensions\VideoRecorder\Microsoft.VisualStudio.QualityTools.VideoRecorderEngine.dll
tools\net462\Common7\IDE\Extensions\TestPlatform\Extensions\VideoRecorder\Microsoft.VisualStudio.TestTools.DataCollection.MediaRecorder.Model.dll
tools\net462\Common7\IDE\Extensions\TestPlatform\Extensions\VideoRecorder\VSTestVideoRecorder.exe

Previously there was also recorder for `V1`. That was removed with TPv0 removal from VS2026 in https://github.com/microsoft/vstest/pull/15247, where VSTest Video recorder was also removed by mistake.
