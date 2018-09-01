REM Open cmd as administrator
REM Example: scripting\symlink-submodule submodules\unity-sample-project\SampleProject\Assets\com.company.product DummyProject\Assets\com.company.product

@ECHO off

SET SubmoduleSource=%1
SET SubmoduleTarget=%2

IF [%SubmoduleSource%] == [] GOTO :missingSource
IF [%SubmoduleTarget%] == [] GOTO :missingTarget

mklink /J %SubmoduleTarget% %SubmoduleSource%
CALL :updateGitignoreFile %SubmoduleTarget%

GOTO :EOF



:missingSource
ECHO "Source undefined"
GOTO :EOF

:missingTarget
ECHO "Target undefined"
GOTO :EOF

:updateGitignoreFile
CALL %~dp0\config.bat
CALL :ignoreAllFilesButKeepDirectory %1
GOTO :EOF

:ignoreAllFilesButKeepDirectory
CALL :createKeepFile %1
CALL :ignoreFilesInDirectory %1
GOTO :EOF

:createKeepFile
ECHO. 2>%1/.keep
GOTO :EOF

:ignoreFilesInDirectory
ECHO %GitignoreFile%
SET FILEPATH=%1
SET Filename=""
CALL :extractFilename %FILEPATH% %Filename
FINDSTR %Filename% %GitignoreFile%
IF %errorlevel% EQU 0 GOTO :EOF 

REM Convert backslashes to forward slashes, because of git 
SET FILEPATH=%FILEPATH:\=/%
ECHO %FILEPATH%/* >> %GitignoreFile%
ECHO !%FILEPATH%/.keep >> %GitignoreFile%
GOTO :EOF

:extractFilename
SET %2=%~nx1
GOTO :EOF