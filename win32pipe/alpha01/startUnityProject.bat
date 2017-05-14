call copy_dll.bat
path=%~dp0winpipe_unityProject\Assets\Plugins;%path%
start "" "%unitypath5%" -projectpath %~dp0winpipe_unityProject
::pause