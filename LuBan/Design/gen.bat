set LUBAN_DLL=..\Tools\Luban\Luban.dll
set CONF_ROOT=.
set CLIENT_ROOT=..\..\Assets\Config

dotnet %LUBAN_DLL% ^
    -t client ^
    -c cs-bin ^
    -d bin ^
    --conf %CONF_ROOT%\luban.conf ^
    -x outputDataDir=%CLIENT_ROOT%\Data ^
    -x outputCodeDir=%CLIENT_ROOT%\Code

pause