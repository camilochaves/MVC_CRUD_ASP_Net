ECHO OFF
cls
echo Step 1: Creating SSL Certificates for the containers...
timeout 3
cls
dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\webapp.pfx -p someRandomPassword
dotnet dev-certs https --trust

echo.
echo DO YOU WANT TO RUN THE TESTS NOW? (y/n)
set /p Input=Enter y or n:
if /I "%Input%"=="y" goto RunTheTests
if /I "%Input%"=="n" goto StartDockerCompose
exit

rem Running the Tests
:RunTheTests
echo Step2: Running the Tests ... 
timeout 5
cls
echo on
docker-compose -f docker-compose.yml -f docker-compose-cache.yml -f docker-compose-rabbitmq.yml up -d Cache
dotnet test Tests
echo off

rem Start Docker-Compose
:StartDockerCompose
echo Press any key to start docker-compose for the containers...
timeout 5
cls
ECHO ON
docker-compose -f docker-compose.yml -f docker-compose-webapp.yml -f docker-compose-db.yml -f docker-compose-cache.yml  -f docker-compose-rabbitmq.yml up -d --remove-orphans
cmd-ps.bat


