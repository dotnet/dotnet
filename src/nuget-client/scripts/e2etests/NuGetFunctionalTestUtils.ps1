. "$PSScriptRoot\Utils.ps1"
. "$PSScriptRoot\VSUtils.ps1"

Add-Type -AssemblyName System.Web.Extensions

$javaScriptSerializer = [System.Web.Script.Serialization.JavaScriptSerializer]::new()
$javaScriptSerializer.MaxJsonLength = [System.Int32]::MaxValue

Function Convert-FromJsonToDictionary([string] $json)
{
    If ([string]::IsNullOrWhiteSpace($json))
    {
        Return $Null
    }

    Try
    {
        Return $javaScriptSerializer.DeserializeObject($json)
    }
    Catch [System.ArgumentException]
    {
        # Most likely a partial line was read.
        Return $Null
    }
}

function WriteToCI
{
    param (
    [Parameter(Mandatory=$true)]
    [System.Collections.Generic.Dictionary`2[System.String,System.Object]] $singleResult)

    $status = $singleResult.Status
    $testName = $singleResult.TestName

    $guid = [System.Guid]::NewGuid().ToString("d")
    # The below Write-Host commands are no-oping right now in the release environment.
    Write-Host "##vso[task.logdetail id=$guid;name=$testName;type=build;order=1]Test $testName started"

    if ($status -eq 'Failed')
    {
        Write-Host "##vso[task.logdetail id=$guid;progress=100;state=Failed]Test $testName failed"
    }
    ElseIf ($status -eq 'Skipped')
    {
        Write-Host "##vso[task.logdetail id=$guid;progress=100;state=Skipped]Test $testName skipped"
    }
    Else
    {
        Write-Host "##vso[task.logdetail id=$guid;progress=100;state=Succeeded]Test $testName passed"
    }
}

function New-Guid {
    [System.Guid]::NewGuid().ToString("d").Substring(0, 4).Replace("-", "")
}

# This function requires a rewrite. This is a first cut
function RealTimeLogResults
{
    param (
    [Parameter(Mandatory=$true)]
    [string]$NuGetTestPath,
    [Parameter(Mandatory=$true)]
    [int] $EachTestTimeoutInSecs)

    trap
    {
        Write-Host "RealTimeLogResults threw an exception: " -ForegroundColor Red
        Write-Error ($_.Exception | Format-List -Force | Out-String) -ErrorAction Continue
        Write-Error ($_.InvocationInfo | Format-List -Force | Out-String) -ErrorAction Continue
        exit 1
    }

    $currentTestTime = 0
    $currentTestId = 0
    $currentTestName = [string]$null
    $currentBinFolder = [string]$null

    # Get the current bin folder
    while(!$currentBinFolder -and ($currentTestTime -le $EachTestTimeoutInSecs))
    {
        start-sleep 1
        $currentTestTime++
        $currentBinFolder = (ls $NuGetTestPath\bin -ErrorAction SilentlyContinue | Sort-Object LastWriteTimeUtc -Descending | Select -First 1)
    }

    if (!$currentBinFolder)
    {
        Write-Error "Looks like no tests were run. There is no folder under $NuGetTestPath\bin. Please investigate!"
        return $null
    }

    $currentTestTime = 0

    $log = Join-Path $currentBinFolder.FullName "log.txt"
    $testResults = Join-Path $currentBinFolder.FullName "Realtimeresults.txt"

    $lastLogLine = ""

    Try
    {
        While ($currentTestTime -le $EachTestTimeoutInSecs)
        {
            Start-Sleep 1
            $currentTestTime++
            if ((Test-Path $log) -and (Test-Path $testResults))
            {
                $content = Get-Content $testResults
                if (($content.Count -gt 0) -and ($content.Count -gt $currentTestId))
                {
                    $content[($currentTestId)..($content.Count - 1)] | % {
                        $testResult = Convert-FromJsonToDictionary($_)

                        If (!$testResult)
                        {
                            # continues the while loop so that it can be tried again
                            continue
                        }

                        If ($testResult['Type'] -eq 'test result')
                        {
                            WriteToCI $testResult

                            $status = $testResult.Status
                            $testName = $testResult.TestName
                            $timeInMilliseconds = $testResult.TimeInMilliseconds

                            Write-Host "$status $testName $($timeInMilliseconds)ms"
                        }
                    }

                    $currentTestTime = 0
                    $currentTestId = $content.Count
                }
                else
                {
                    $logContent = Get-Content $log
                    $lastLogLine = $logContent[$currentTestId]
                    Write-Host $lastLogLine " and current test time is ${currentTestTime}"
                }

                $logContent = Get-Content $log
                $logContentLastLine = $logContent[$logContent.Count - 1]
                $isError = $false

                $possibleSummary = Convert-FromJsonToDictionary($logContentLastLine)

                if ($possibleSummary -And $possibleSummary['Type'] -eq 'test run summary')
                {
                    # RUN HAS COMPLETED
                    Write-Host 'Run has completed. Copying the results file to CI'

                    $isError = ($possibleSummary['FailedCount'] -ne 0) -And ($possibleSummary['ActualTotalCount'] -eq $possibleSummary['ExpectedTotalCount'])
                    $message = "Ran $($possibleSummary['ActualTotalCount']) Tests and/or Test cases, $($possibleSummary['PassedCount']) Passed, $($possibleSummary['FailedCount']) Failed, $($possibleSummary['SkippedCount']) Skipped, $($possibleSummary['ExpectedTotalCount']) expected total. See `'$($possibleSummary['HtmlResultsFilePath'])`' or `'$($possibleSummary['TextResultsFilePath'])`' for more details."

                    if ($isError)
                    {
                        Write-Error $message
                    }
                    else
                    {
                        Write-Host -ForegroundColor Green $message
                    }

                    $resultsFile = Join-Path $currentBinFolder.FullName results.html
                    if (Test-Path $resultsFile)
                    {
                        CopyResultsToCI $NuGetDropPath $RunCounter $resultsFile
                    }
                    else
                    {
                        CopyResultsToCI $NuGetDropPath $RunCounter $testResults
                    }
                    break
                }
            }
        }

        if ($currentTestTime -gt $EachTestTimeoutInSecs)
        {
            $logLineEntries = $lastLogLine -split " "
            if ($logLineEntries.Count -gt 1)
            {
                $currentTestName = $logLineEntries[2].Replace("...", "")
            }
            else
            {
                $currentTestName = "unknown test name"
            }

            $result = @{
                Type = 'test result'
                TestName = $currentTestName
                Status = 'Failed'
                Message = "Test timed out after $EachTestTimeoutInSecs seconds"
                TimeInMilliseconds = $EachTestTimeoutInSecs * 1000
            }

            $json = ConvertTo-Json $result -Compress
            $json >> $testResults

            $errorMessage = 'Run Failed - Results.html did not get created. ' `
            + 'This indicates that the tests did not finish running. It could be that the VS crashed or a test timed out. Please investigate.'
            CopyResultsToCI $NuGetDropPath $RunCounter $testResults

            Write-Error $errorMessage
            return $null
        }
    }
    Finally
    {
        If (Test-Path $log)
        {
            Write-Host "##vso[task.uploadfile]$log"
        }

        If (Test-Path $testResults)
        {
            Write-Host "##vso[task.uploadfile]$testResults"
        }
    }
}

function CopyResultsToCI
{
    param (
    [Parameter(Mandatory=$true)]
    [string]$NuGetDropPath,
    [Parameter(Mandatory=$true)]
    [string]$RunCounter,
    [Parameter(Mandatory=$true)]
    [string]$resultsFile)

    $DropPathFileInfo = Get-Item $NuGetDropPath
    $DropPathParent = $DropPathFileInfo.Parent
    $ResultsFileInfo = Get-Item $resultsFile
    $ResultsFileParent = $ResultsFileInfo.Directory
    $EndToEndPath = Join-Path $NuGetDropPath "EndToEnd"
    $FullLogFileName = "FullLog_" + $RunCounter + ".txt"
    $FullLogFilePath = Join-Path $EndToEndPath $FullLogFileName
    $RealTimeResultsFilePath = Join-Path $ResultsFileParent.FullName 'Realtimeresults.txt'

    $TestResultsPath = Join-Path $DropPathParent.FullName 'testresults'
    $FullLogFileDestinationPath = Join-Path $TestResultsPath $FullLogFileName
    mkdir $TestResultsPath -ErrorAction Ignore

    $DestinationFileName = 'Run-' + $RunCounter + '-' + (Split-Path $resultsFile -Leaf)
    $DestinationPath = Join-Path $TestResultsPath $DestinationFileName
    Write-Host "Copying html results file from $resultsFile to $DestinationPath"
    Copy-Item $resultsFile $DestinationPath
    Write-Host "##vso[task.uploadfile]$DestinationPath"

    if($env:CI)
    {
        Write-Host "Copying full log file from $FullLogFilePath to $FullLogFileDestinationPath"
        Copy-Item $FullLogFilePath -Destination $FullLogFileDestinationPath -Force  -ErrorAction SilentlyContinue
        Write-Host "##vso[task.uploadfile]$FullLogFileDestinationPath"
    }

    OutputResultsForCI -NuGetDropPath $NuGetDropPath -RunCounter $RunCounter -RealTimeResultsFilePath $RealTimeResultsFilePath
}

function OutputResultsForCI
{
    param (
    [Parameter(Mandatory=$true)]
    [string]$NuGetDropPath,
    [Parameter(Mandatory=$true)]
    [string]$RunCounter,
    [Parameter(Mandatory=$true)]
    [string]$RealTimeResultsFilePath)

    $DropPathFileInfo = Get-Item $NuGetDropPath
    $DropPathParent = $DropPathFileInfo.Parent

    $TestResultsPath = Join-Path $DropPathParent.FullName 'testresults'
    $DestinationFileName = 'E2EResults-' + $RunCounter + '.xml'
    $DestinationPath = Join-Path $TestResultsPath $DestinationFileName
    Write-JunitXml -RealTimeResultsFile $RealTimeResultsFilePath -XmlResultsFilePath $DestinationPath
}

Function Write-JunitXml
{
    param (
        [Parameter(Mandatory=$true)]
        [string]$RealTimeResultsFile,
        [Parameter(Mandatory=$true)]
        [string]$XmlResultsFilePath
    )
$template = @'
<testsuite name="" file="">
<testcase classname="" name="" time="">
    <failure type="failure" message=""></failure>
</testcase>
</testsuite>
'@

    $guid = [System.Guid]::NewGuid().ToString("N")
    $templatePath = [System.IO.Path]::Combine([System.IO.Path]::GetTempPath(), $guid + ".txt");

    $template | Out-File $templatePath -encoding UTF8
    # load template into XML object
    $xml = New-Object xml
    $xml.Load($templatePath)
    # grab template user
    $newTestCaseTemplate = (@($xml.testsuite.testcase)[0]).Clone()

    $className = "NuGet.Client.EndToEndTests"
    $xml.testsuite.name = $className
    $xml.testsuite.file = $className
    $Results = Get-Content $RealTimeResultsFile
    foreach ($result in $Results)
    {
        $parsedResult = Convert-FromJsonToDictionary($result)
        $newTestCase = $newTestCaseTemplate.clone()
        $newTestCase.classname = $className
        $newTestCase.name = $parsedResult.TestName
        $Duration = [System.TimeSpan]::FromMilliseconds($parsedResult.TimeInMilliseconds)
        $newTestCase.time = $Duration.TotalSeconds.ToString()
        if($parsedResult.Status -eq 'Passed')
        {   #Remove the failure node
            $newTestCase.RemoveChild($newTestCase.ChildNodes[0]) | Out-Null
        }
        else
        {
            $newTestCase.failure.message = $parsedResult.Message
            $newTestCase.failure.InnerText = $parsedResult.Callstack
        }
        $xml.testsuite.AppendChild($newTestCase) > $null
    }

    # remove users with undefined name (remove template)
    $xml.testsuite.testcase | Where-Object { $_.Name -eq "" } | ForEach-Object  { [void]$xml.testsuite.RemoveChild($_) }
    # save xml to file
    Write-Host "Path: " $XmlResultsFilePath

    $xml.Save($XmlResultsFilePath)
    Remove-Item $templatePath #clean up
}
