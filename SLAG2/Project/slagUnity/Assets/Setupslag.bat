@echo off
cd /d %~dp0
echo : 
echo : Setup slag for development
echo :
echo : 1 ... Copy dll
echo : 2 ... Copy src
echo : 3 ... Overwrite orginal source from here
set /p a="H"
goto ;_%a%

:_1
echo : remove sagtool dir then copy dll. Confirm.
pause

rd /s /q slagtool 2>nul
md slagtool 2>nul
copy N:\Project\slag\slagtool\bin\Debug\slagtool.*  slagtool
goto :_end

:_2
echo : remove slagtool dir then copy dll. Confirm.
pause

rd /s /q slagtool 2>nul
robocopy N:\Project\slag\slagtool slagtool *.cs /S /XD obj /XD Properties
goto :_end

:_3
echo : robocopy slagtool N:\Project\slag\slagtool slagtool *.cs /S   ... Confirm.
pause
robocopy slagtool N:\Project\slag\slagtool *.cs /S /XD obj /XD Properties
goto :_end

:_end
echo :
echo : end
echo :
pause