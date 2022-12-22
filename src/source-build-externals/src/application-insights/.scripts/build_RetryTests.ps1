param ([string]$TestResultFile = $(throw "Path to Test Run (.trx) is required."), [string]$WorkingDirectory = $(throw "Path to write retry test runs is required."))

# SUMMARY
# The `dotnet test cli` will not automaticalyl retry failed tests.
# This script will inspect a dotnet test result file (*.trx).
# Any failed tests will be retried upto a max value.

Write-Host "inputs:"
Write-Host "-TestResultFile: $TestResultFile"
Write-Host "-WorkingDirectory: $WorkingDirectory"
Write-Host ""

[int]$maxRetries = 5;

# INSPECT TEST RUN RESULTS
[xml]$testRunXml = Get-Content -Path $TestResultFile -ErrorAction Stop
Write-Host "Parsing TestRun '$TestResultFile' Outcome: '$($testRunXml.TestRun.ResultSummary.outcome)' Failed: '$($testRunXml.TestRun.ResultSummary.Counters.failed)'";

# IF TEST RUN RESULTS FAILED, START RETRY
if ($testRunXml.TestRun.ResultSummary.outcome -eq "Failed")
{
    Write-Host "Detected TestRun failed, will retry tests $maxRetries times.";

    # CREATE DIRECTORY FOR RETRY OUTPUTS
    $logDirectoryRetries = Join-Path -Path $WorkingDirectory -ChildPath "RetryResults";
    New-Item -Path $logDirectoryRetries -ItemType directory -ErrorAction Stop | Out-Null

    $results = $testRunXml.TestRun.Results.UnitTestResult
    $testDefinitions = $testRunXml.TestRun.TestDefinitions.UnitTest;
    Write-Debug "TestResults: $($results.Count)";
    Write-Debug "TestDefinitions: $($testDefinitions.Count)";

    [bool]$scriptResult = $true;
    $ScriptSummary = @();

    # FOREACH TEST RUN RESULTS
    foreach ($result in $results)
    {
        if ($result.outcome -eq "Failed")
        {
            $definition = $testDefinitions | Where-Object { $_.id -eq $result.testId}
            if ($null -eq $definition)
            {
                Write-Error -Message "TEST DEFINITION NOT FOUND" -ErrorAction Stop
            }

            Write-Host ""
            Write-Host "- $($definition.TestMethod.codeBase) $($definition.TestMethod.className).$($definition.TestMethod.name) $($result.outcome)"

            # FOREACH TEST, MAX RETRIES
            [bool]$retryResult = $false;
            for($i=0; $i -lt $maxRetries -and $retryResult -eq $false ; $i++)
            {
                $logPath = "$logDirectoryRetries/$($definition.TestMethod.className).$($definition.TestMethod.name)_$i.trx";
                dotnet test $($definition.TestMethod.codeBase) --logger "trx;LogFileName=$logPath" --filter "ClassName=$($definition.TestMethod.className)&Name=$($definition.TestMethod.name)" | Out-Null

                [xml]$retryXml = Get-Content -Path $logPath -ErrorAction Stop
                $retryOutcome = $retryXml.TestRun.ResultSummary.outcome;
                $retryResult = ($retryOutcome -ne "Failed");
                Write-Host "Retry #$i Outcome: '$retryOutcome' Passed: $retryResult"
            }

            if ($retryResult -eq $false)
            {
                $ScriptSummary += "$($definition.TestMethod.className).$($definition.TestMethod.name)"
            }

            $scriptResult = $scriptResult -band $retryResult;
            Write-Debug "script result: $scriptResult"
        }
    }

    Write-Host ""
    Write-Host ""
    Write-Host "========== ========== ========== =========="
    Write-Host ""
    Write-Host ""

    if ($scriptResult)
    {
        Write-Host "Retry complete. All tests pass."
    }
    else {
        Write-Host "The following tests failed after retry:"

        foreach($line in $ScriptSummary)
        {
            Write-Host "- $line";
        }

        Write-Host ""
        Write-Error -Message "Retry failed." -ErrorAction Stop
    }
}
