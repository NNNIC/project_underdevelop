cd %~dp0
set dll_1=..\..\..\..\slag\slaglangtool\bin\Debug\*.dll
set dll_2=..\..\..\..\slag\slagInteractiveDll\bin\Debug\*.dll
set dll_3=..\..\..\..\slag\test\bin\Debug\test.dll
set dll_4=..\..\..\..\monoProject\slagtool\slagtool\bin\Debug\slagtool.dll

::copy "%dll_1%" *.*
::copy "%dll_2%" *.*
::copy "%dll_3%" *.*
copy "%dll_4%" *.*
pause

