:: Example: scripting\remove-submodule unity-sample-project

CALL %~dp0\config.bat

SET GitSubmodule=%1
SET PathWithBackslash=%SubmodulePath%\%GitSubmodule%
SET PathWithSlash=%PathWithBackslash:\=/%

:: Remove the submodule entry from .git/config
git submodule deinit -f -- %PathWithSlash%

:: Remove the submodule directory from the superproject's .git/modules directory
rmdir /s /q .git\modules\%PathWithBackslash%

:: Remove the entry in .gitmodules and remove the submodule directory located at path/to/submodule
git rm -f %PathWithSlash%

:: If the submodule cannot be removed completely or if you removed some parts by hand
:: check follow these instructions https://stackoverflow.com/questions/1260748/how-do-i-remove-a-submodule