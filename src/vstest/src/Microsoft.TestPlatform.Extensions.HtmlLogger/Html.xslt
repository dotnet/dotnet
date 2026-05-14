<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="2.0"
    xmlns:tp="http://schemas.datacontract.org/2004/07/Microsoft.VisualStudio.TestPlatform.Extensions.HtmlLogger.ObjectModel"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:a="http://schemas.microsoft.com/2003/10/Serialization/Arrays"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxs tp">
  <xsl:output method="html" indent="yes"/>
  <xsl:template match="/">
    <html>
      <body>
        <h1>Test run details</h1>
        <xsl:apply-templates select ="/tp:TestRunDetails"/>
      </body>
        <style>
        body { font-family: Calibri, Verdana, Arial, sans-serif; background-color: White; color: Black; }
        h2 {
        margin-top: 15px;
        margin-bottom:10px;
        }
        pre {
        white-space: pre-wrap;
        }
        .summary {font-family:monospace;
        display: -webkit-flex; /* Safari */
        -webkit-flex-wrap: wrap; /* Safari 6.1+ */
        display: flex;
        flex-wrap: wrap;
        }
        .row {
        border: 2px solid #ffffff;
        width:100%;
        cursor:pointer;
        background-color:#d7e9fa;
        }
        .list-row {
        border: 2px solid #ffffff;
        width:100%;
        cursor:pointer;
        background-color:#ffffff;
        }
        .inner-row{
        border: 2px solid #ffffff;
        padding-left:1%;
        margin-left:1%;
        background-color:#ffffff;
        }
        .block{
        width : 150px;
        }
        .leaf-division{
        border: 2px solid #ffffff;
        background-color:#e6eff7;
        }
        .pass { color: #0c0; }
        .fail { color: #c00; }
        .error-message { color : brown; }
        .error-stack-trace { color: brown; }
        .duration {float:right;padding-right:1%;}
        .total-tests { font-size : 30px}
        .test-run-time { font-size : 30px}
        .pass-percentage { font-size : 30px}
        .error-info{
        margin-left:16px;
        }
      </style>
    </html>
  </xsl:template>

  <xsl:template match="/tp:TestRunDetails">
    <xsl:apply-templates select ="/tp:TestRunDetails/tp:Summary"/>
    <xsl:apply-templates select ="/tp:TestRunDetails/tp:ResultCollectionList"/>
    <xsl:if test="tp:RunLevelMessageInformational != ''">
    <xsl:apply-templates select ="tp:RunLevelMessageInformational"/>
    </xsl:if>
    <xsl:if test="tp:RunLevelMessageErrorAndWarning != ''">
      <xsl:apply-templates select ="tp:RunLevelMessageErrorAndWarning"/>
    </xsl:if>
  </xsl:template>

  <xsl:template match="tp:TestRunDetails/tp:Summary">
    <div class ="summary">
      <div class ="block"><xsl:apply-templates select ="/tp:TestRunDetails/tp:Summary/tp:TotalTests"/></div>
      <div class ="block">
        <xsl:apply-templates select ="/tp:TestRunDetails/tp:Summary/tp:PassedTests"/>
        <xsl:apply-templates select ="/tp:TestRunDetails/tp:Summary/tp:FailedTests"/>
        <xsl:apply-templates select ="/tp:TestRunDetails/tp:Summary/tp:SkippedTests"/>
      </div>
      <div class ="block"><xsl:apply-templates select ="/tp:TestRunDetails/tp:Summary/tp:PassPercentage"/></div>
      <div class ="block"><xsl:apply-templates select ="/tp:TestRunDetails/tp:Summary/tp:TotalRunTime"/></div>
      <br/>
    </div>
  </xsl:template>

  <xsl:template match="/tp:TestRunDetails/tp:ResultCollectionList">
    <xsl:if test="count(tp:TestResultCollection/tp:FailedResultList/tp:TestResult)>0">
      <h2>Failed Results</h2>
      <xsl:call-template name ="FailedResults"/>
    </xsl:if>
    <h2>All Results</h2><xsl:call-template name ="Results"/>
  </xsl:template>

   <xsl:template match="tp:RunLevelMessageInformational">
    <div>
    <h2>Informational messages</h2>
      <xsl:for-each select ="a:string">
        <span><xsl:value-of select = "." /></span><br />
      </xsl:for-each>
    </div>
  </xsl:template>

  <xsl:template match="tp:RunLevelMessageErrorAndWarning">
    <div>
      <h2>Error and Warning messages</h2>
      <xsl:for-each select ="a:string">
        <span><xsl:value-of select = "." /></span><br />
      </xsl:for-each>
    </div>
  </xsl:template>

  <xsl:template name ="Results">
    <xsl:for-each select ="tp:TestResultCollection">
      <xsl:variable name="Source" select="tp:Id" />
      <xsl:if test="tp:ResultList!=''">
        <details>
          <summary><xsl:value-of select = "tp:Source" /></summary>
          <div class ="inner-row" Id="{$Source}">
            <xsl:for-each select ="tp:ResultList/tp:TestResult"><xsl:call-template name ="TestResult"/></xsl:for-each>
          </div>
        </details>
      </xsl:if>
    </xsl:for-each>
  </xsl:template>

  <xsl:template name = "FailedResults">
    <xsl:for-each select ="tp:TestResultCollection">
      <xsl:variable name="Source" select="tp:Id" />
      <xsl:if test="tp:FailedResultList!=''">
        <details>
            <xsl:attribute name="open"/>
            <summary><xsl:value-of select = "tp:Source" /></summary>
          <div class ="inner-row" Id="{concat($Source,'-failedResult')}">
            <xsl:for-each select ="tp:FailedResultList/tp:TestResult"><xsl:call-template name ="TestResult"/></xsl:for-each>
          </div>
        </details>
      </xsl:if>
    </xsl:for-each>
  </xsl:template>

  <xsl:template name="TestResult" match="tp:TestResult">
    <xsl:variable name="TestResultId" select="tp:TestResultId" />

    <xsl:if test ="tp:InnerTestResults!=''">
      <details>
        <summary><xsl:call-template name = "Result" /></summary>
        <a Id="{concat($TestResultId,'-',name(..))}"><xsl:apply-templates select = "tp:InnerTestResults" /></a>
      </details>
    </xsl:if>

    <xsl:if test ="tp:InnerTestResults=''">
      <div class ="leaf-division"><xsl:call-template name = "Result" /></div>
    </xsl:if>

  </xsl:template>

  <xsl:template match="tp:InnerTestResults">
    <div class ="inner-row"><xsl:apply-templates select = "tp:TestResult" /></div>
  </xsl:template>

<xsl:template name="Result" >
    <div>
      <xsl:apply-templates select = "tp:ResultOutcome"/>
      <xsl:apply-templates select = "tp:DisplayName" />
      <div class="duration"><xsl:apply-templates select = "tp:Duration" /></div>
    </div>
   <div class="error-info">
     <xsl:if test ="tp:ErrorMessage!=''"><xsl:apply-templates select = "tp:ErrorMessage" /></xsl:if>
     <xsl:if test ="tp:ErrorStackTrace!=''"><xsl:apply-templates select = "tp:ErrorStackTrace" /></xsl:if>
   </div>
  </xsl:template>

  <xsl:template match = "tp:ErrorMessage">
    Error: <span class="error-message"><pre><xsl:value-of select = "." /></pre></span><br />
  </xsl:template>

  <xsl:template match = "tp:ErrorStackTrace">
    Stack trace: <span class="error-stack-trace"><pre><xsl:value-of select = "." /></pre></span><br />
  </xsl:template>

  <xsl:template match = "tp:FailedTests">
    <span>Failed &#160;:&#160;</span><span class="failedTests"><xsl:value-of select = "." /></span><br />
  </xsl:template>

  <xsl:template match = "tp:PassedTests">
    <span>Passed &#160;:&#160;</span><span class="passedTests" ><xsl:value-of select = "." /></span><br />
  </xsl:template>

  <xsl:template match = "tp:SkippedTests">
    <span>Skipped :&#160;</span><span class="skippedTests" ><xsl:value-of select = "." /></span><br />
  </xsl:template>

  <xsl:template match = "tp:TotalTests">
    <span>Total tests</span><div  class="total-tests"><xsl:value-of select = "." /></div><br />
  </xsl:template>

  <xsl:template match = "tp:TotalRunTime">
    <span>Run duration</span><div  class="test-run-time" ><xsl:value-of select = "." /></div><br />
  </xsl:template>

  <xsl:template match = "tp:PassPercentage">
    <span>Pass percentage</span><div  class="pass-percentage" ><xsl:value-of select = "." /> &#37;</div><br />
  </xsl:template>

  <xsl:template match = "tp:ResultOutcome">
    <xsl:if test =" . = 'Passed' "><span class="pass">&#x2714;</span></xsl:if>
    <xsl:if test =" . = 'Failed'"><span class="fail">&#x2718;</span></xsl:if>
    <xsl:if test =" . = 'Skipped'"><span class="skip">&#x2762;</span></xsl:if>
  </xsl:template>

  <xsl:template match = "tp:Duration">
    <span><xsl:value-of select = "." /></span><br />
  </xsl:template>

  <xsl:template match = "tp:DisplayName">
    <span>&#160;<xsl:value-of select = "." />&#8203;</span>
  </xsl:template>
</xsl:stylesheet>
