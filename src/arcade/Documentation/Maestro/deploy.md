# Maestro deployment

The Maestro web application is deployed at <http://maestro-prod.azurewebsites.net>.  It is currently published in the [maestro-prod app service](https://ms.portal.azure.com/#resource/subscriptions/68672ab8-de0c-40f1-8d1b-ffb20bd62c0f/resourcegroups/maestro/providers/Microsoft.Web/sites/maestro-prod/appServices) in the 'Helix' Azure subscription.

## Logs
The logs are in the [maestroprodstorage Storage Account](https://ms.portal.azure.com/#resource/subscriptions/68672ab8-de0c-40f1-8d1b-ffb20bd62c0f/resourceGroups/maestro/providers/Microsoft.Storage/storageAccounts/maestroprodstorage/overview).  The CSV files can be viewed by opening the maestro-logs blob container in Azure or your favorite blob storage viewer (e.g. Cloud Explorer in VS or Storage Explorer).

## Source code change: Publish
Whenever a change to the Maestro source code is submitted, the app needs to be published.  This is currently a manual process.

 1. Ensure that the local dotnet/versions tree is in sync with the master branch on GitHub.
 2. Open Maestro\Microsoft.DotNet.Maestro.sln in VS.
 3. Remove the "AzureWebJobsStorage" connection string from Web.config.  (This connection string is not needed in Web.config when publishing because it is defined in App Settings in Azure, but it's defined with an empty value in Web.config in source control so that it can be easily set when F5'ing.)
 4. Build the solution.
 5. Right-click on the Microsoft.DotNet.Maestro.WebApi project, and select Publish.
 6. Select “Microsoft Azure App Service”.
 7. Select the “maestro-prod” app service in the 'Helix' subscription and click OK.
 8. On the Settings page of the publish wizard, select the Release configuration.
 9. Click Publish.
 10. Right-click on the Microsoft.DotNet.Maestro.WebJob project, and select Publish as Azure WebJob.
     * Note: You need the Azure Development workload installed in VS2017.
 11. Repeat steps 5-8.
 12. Locally undo the removal of the connection string in Web.config, and the adding of the publish profiles to the csprojs.

## Monitoring logs
Please check in the logs to validate if requests are still flowing:

 1. Open the [logs](#logs).
 2. Navigate to the latest date/hour (e.g. `maestro-prod/2017/08/31/22`).
 3. Download and open `*applicationLog.csv`.
 4. Make sure there are lines like `CommitPushed Complete` and `Successfully queued VSO build.`
    * You may need to wait until a build-info commit happens on https://github.com/dotnet/versions/tree/master/build-info.

## Confirming on Azure that the publish worked
*Note:* This is a diagnosis step if you're not sure the publish worked. It's not a required publish step.

 1. Go to the App Service Editor (preview).
 2. Download `bin/Microsoft.DotNet.Maestro.dll` and `bin/Microsoft.DotNet.Maestro.WebApi.dll`
 3. Use ildasm, ILSpy, or some other disassembler to determine if the IL matches the changes you expected.


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CMaestro%5Cdeploy.md)](https://helix.dot.net/f/p/5?p=Documentation%5CMaestro%5Cdeploy.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CMaestro%5Cdeploy.md)</sub>
<!-- End Generated Content-->
