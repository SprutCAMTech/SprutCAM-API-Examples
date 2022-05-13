@echo off
SETLOCAL
SET EXIT_CODE=0
SET BUILDCONFIG=%1
SET version=

if "%BUILDCONFIG%" == "" (SET BUILDCONFIG=DEBUG)

call :RUNCLEAN "STMachiningToolsImportApi\STConsoleApp\STConsoleApp.csproj"

call :RUNBUILD "STMachiningToolsImportApi\STConsoleApp\STConsoleApp.csproj"

echo Total result %EXIT_CODE%
EXIT /B %EXIT_CODE%

:RUNCLEAN
    call dotnet clean --configuration %BUILDCONFIG% "%~1" --verbosity minimal
    if NOT "%ERRORLEVEL%" == "0" (SET EXIT_CODE=1)
EXIT /B

:RUNBUILD
    call dotnet build --configuration %BUILDCONFIG% "%~1" --verbosity minimal
    if NOT "%ERRORLEVEL%" == "0" (SET EXIT_CODE=1)
EXIT /B