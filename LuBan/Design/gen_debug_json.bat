set LUBAN_DLL=..\Tools\Luban\Luban.dll
set CONF_ROOT=.
set CLIENT_ROOT=..\..\Assets\Design

dotnet %LUBAN_DLL% ^
    -t client ^
    -d json ^
    --conf %CONF_ROOT%\luban.conf ^
    -x outputDataDir=debug

pause