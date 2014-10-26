@REM Store args
set _SolutionDir=%1%
set _TargetDir=%2%

@REM Do works
if not exist "%_SolutionDir%xiaowan" mkdir "%_SolutionDir%xiaowan" >nul

copy /y "%_TargetDir%xiaowan.exe" "%_SolutionDir%xiaowan\" >nul
copy /y "%_TargetDir%xiaowan.exe.Config" "%_SolutionDir%xiaowan\" >nul
copy /y "%_TargetDir%ControlExs.dll" "%_SolutionDir%xiaowan\" >nul
copy /y "%_TargetDir%AxInterop.WMPLib.dll" "%_SolutionDir%xiaowan\" >nul
copy /y "%_TargetDir%Interop.WMPLib.dll" "%_SolutionDir%xiaowan\" >nul
copy /y "%_TargetDir%Microsoft.WindowsAPICodePack.dll" "%_SolutionDir%xiaowan\" >nul
copy /y "%_TargetDir%Microsoft.WindowsAPICodePack.Shell.dll" "%_SolutionDir%xiaowan\" >nul
copy /y "%_TargetDir%XwWizard.dll" "%_SolutionDir%xiaowan\" >nul

if not exist "%_SolutionDir%xiaowan\en" mkdir "%_SolutionDir%xiaowan\en" >nul
if not exist "%_SolutionDir%xiaowan\zh-HanT" mkdir "%_SolutionDir%xiaowan\zh-HanT" >nul
if not exist "%_SolutionDir%xiaowan\ja-JP" mkdir "%_SolutionDir%xiaowan\ja-JP" >nul

xcopy /y "%_TargetDir%en" "%_SolutionDir%xiaowan\en" >nul
xcopy /y "%_TargetDir%zh-Hant" "%_SolutionDir%xiaowan\zh-HanT" >nul
xcopy /y "%_TargetDir%ja-JP" "%_SolutionDir%xiaowan\ja-JP" >nul

echo PostBuildEvent Completed.
