cd %~dp0
set dll_1=..\..\..\..\slag\slagtool\bin\Debug\slagtool.dll
set dll_2=..\..\..\..\slag\slagmonitor\bin\Debug\slagmonitor.dll

copy "%dll_1%" *.*
copy "%dll_2%" *.*

pause