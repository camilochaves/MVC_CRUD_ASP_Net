echo off
cls
echo.
echo Start Scenarios:
echo.
echo 1: WebApp MySql Redis RabbitMq Kafka
echo 2: MySql Redis RabbitMq Kafka
echo 3: MySql Redis RabbitMq
echo 4: Redis RabbitMq
echo 5: Redis Kafka
echo 6: Redis
echo 7: RabbitMq
echo 8: Kafka
echo Select Scenario: (1 to 8)
echo.
set /p SCENARIO=Enter scenario number 

echo off
2>NUL CALL :CASE_%SCENARIO% 
IF ERRORLEVEL 1 CALL :DEFAULT_CASE # If label doesn't exist
ECHO Done.
cmd-ps.bat
@echo off
EXIT /B

:CASE_1
    ECHO ON
    docker-compose -f docker-compose.yml -f docker-compose-webapp.yml -f docker-compose-db.yml -f docker-compose-cache.yml  -f docker-compose-rabbitmq.yml  -f docker-compose-kafka.yml up -d --remove-orphans
    ECHO OFF
    GOTO END_CASE
:CASE_2
    ECHO ON
    docker-compose -f docker-compose.yml -f docker-compose-db.yml -f docker-compose-cache.yml  -f docker-compose-rabbitmq.yml  -f docker-compose-kafka.yml up -d --remove-orphans
    ECHO OFF
    GOTO END_CASE
:CASE_3
    ECHO ON
    docker-compose -f docker-compose.yml -f docker-compose-db.yml -f docker-compose-cache.yml  -f docker-compose-rabbitmq.yml up -d --remove-orphans
    ECHO OFF
    GOTO END_CASE
:CASE_4
    ECHO ON
    docker-compose -f docker-compose.yml -f docker-compose-cache.yml  -f docker-compose-rabbitmq.yml up -d --remove-orphans
    ECHO OFF
    GOTO END_CASE
:CASE_5
    ECHO ON
    docker-compose -f docker-compose.yml -f docker-compose-cache.yml  -f docker-compose-kafka.yml up -d --remove-orphans
    ECHO OFF
    GOTO END_CASE
:CASE_6
    ECHO ON
    docker-compose -f docker-compose.yml -f docker-compose-cache.yml up -d --remove-orphans
    ECHO OFF
    GOTO END_CASE
:CASE_7
    ECHO ON
    docker-compose -f docker-compose.yml -f docker-compose-rabbitmq.yml up -d --remove-orphans
    ECHO OFF
    GOTO END_CASE
:CASE_8
    ECHO ON
    docker-compose -f docker-compose.yml -f docker-compose-kafka.yml up -d --remove-orphans
    ECHO OFF
    GOTO END_CASE
:DEFAULT_CASE
    ECHO OFF
    ECHO Unknown scenario "%SCENARIO%"
    GOTO END_CASE
:END_CASE
    ECHO OFF
    VER > NUL # reset ERRORLEVEL
    GOTO :EOF # return from CALL

