@echo off
SETLOCAL

REM .Net sdk is required to build the project
echo Building the MachiningToolsCreateExample project
call dotnet build %~dp0MachiningToolsCreateExample.csproj -c Debug

pause