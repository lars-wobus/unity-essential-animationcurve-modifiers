@ECHO OFF
::Example: scripting\add-submodule https://github.com/lars-wobus/unity-sample-project

:: Extract %SubmodulesPath% from config
CALL %~dp0\config.bat

:: Save input argument
SET RemoteRepositoryUrl=%1
IF [%RemoteRepositoryUrl%] == [] GOTO :MissingArgumentException

:: Extract project name
SET ProjectName=""
CALL :ExtractFilename %RemoteRepositoryUrl% %ProjectName

:: Change directory instead of creating filepaths. This is less error-prone than handling '/' and '\' at once
SET CurrentDirectory=%cd%
CD /D %SubmodulePath%

@ECHO ON

:: Add submodule
git submodule add %RemoteRepositoryUrl% %ProjectName%

@ECHO OFF

CD /D %CurrentDirectory%

:: End on successful execution
GOTO :EOF

:: End on failure
:MissingArgumentException
ECHO Missing argument! First argument must define a repository. For instance: https://github.com/lars-wobus/unity-sample-project
GOTO :EOF

:ExtractFilename
SET %2=%~nx1
GOTO :EOF