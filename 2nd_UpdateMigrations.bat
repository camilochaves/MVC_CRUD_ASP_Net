ECHO OFF
cls
echo Do you want to execute first Migrations? (y/n)
set /p Input=Enter Yes or No:
if /I "%Input%"=="y" goto yesExecuteMigrations
if /I "%Input%"=="n" goto startBrowsers
exit

:yesExecuteMigrations

REM MIGRATIONS NEEDS DOTNET EF TOOL! THEREFORE, WILL CHECK FOR IT FIRST!
dotnet ef && (goto dotnetEFisInstalled) || (goto failure)
exit
:failure
echo Please Install Dotnet EF tool to perform migrations!
exit

:dotnetEFisInstalled
echo.
echo.
echo SUCCESS!
echo DOTNET EF is installed! Therefore now we can
echo update migrations on the database.
echo I am assuming DbMySql server has already started! 
echo Check on docker logs DbMySql or docker-compose ps
timeout 5

ECHO OFF
dotnet ef database update --project src/A.UI.MVC

:startBrowsers

REM starting Microsoft Edge in PhpMyAdmin
echo Opening microsoft edge on phpMyAdmin container
timeout 5
echo In phpMyAdmin the name of the server is the name of the container which is DbMySql
start microsoft-edge:http://localhost:8081

REM starting Microsoft Edge to open WebApp
echo starting microsoft edge
timeout 3
start /min microsoft-edge:https://localhost:5001
