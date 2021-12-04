docker-compose -f docker-compose.yml -f docker-compose-cache.yml -f docker-compose-db.yml -f docker-compose-rabbitmq.yml up -d

echo off
echo.
echo DO YOU WANT TO DOCKER-COMPOSE UP THE WEBAPP ALSO? (y/n)
set /p Input=Enter y or n:
if /I "%Input%"=="y" goto upWebApp
exit

:upWebApp
docker-compose -f docker-compose.yml -f docker-compose-cache.yml -f docker-compose-db.yml -f docker-compose-rabbitmq.yml -f docker-compose-webapp.yml up -d