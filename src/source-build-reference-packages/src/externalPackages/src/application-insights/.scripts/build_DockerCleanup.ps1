
# SUMMARY
# This script will stop and remove docker images from a build server.
# This is used to prevent consuming all available space on a build server.

$ErrorActionPreference = "silentlycontinue"

& docker info
& docker images -a
& docker ps -a

Write-Host "Images before cleanup"
& docker images -a

Write-Host "Containers before cleanup"
& docker ps -a

Write-Host "Stopping E2E Containers"
& docker stop e2etests_ingestionservice_1
& docker stop e2etests_e2etestwebapi_1
& docker stop e2etests_e2etestwebappcore20_1
& docker stop e2etests_e2etestwebappcore30_1
& docker stop e2etests_e2etestwebapp_1
& docker stop e2etests_sql-server_1
& docker stop e2etests_azureemulator_1

Write-Host "Removing E2E Containers"
& docker rm e2etests_ingestionservice_1
& docker rm e2etests_e2etestwebapi_1
& docker rm e2etests_e2etestwebappcore20_1
& docker rm e2etests_e2etestwebappcore30_1
& docker rm e2etests_e2etestwebapp_1
& docker rm e2etests_sql-server_1
& docker rm e2etests_azureemulator_1

Write-Host "Removing E2E Images"
& docker rmi -f e2etests_ingestionservice
& docker rmi -f e2etests_e2etestwebapi
& docker rmi -f e2etests_e2etestwebappcore20
& docker rmi -f e2etests_e2etestwebappcore30
& docker rmi -f e2etests_e2etestwebapp
& docker rmi -f e2etests_azureemulator

Write-Host "Removing dangling images"
docker images -f "dangling=true" -q | ForEach-Object {docker rmi $_ -f}

Write-Host "Removing E2E Containers"
docker ps -a -q | ForEach-Object {docker rm $_ -f}

Write-Host "Images after cleanup"
& docker images -a
Write-Host "Containers after cleanup"
& docker ps -a