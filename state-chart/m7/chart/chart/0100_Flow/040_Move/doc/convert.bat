cd /d %~dp0
set CVT=%~dp0..\..\..\..\..\Tools\ExcelStateChartConverter\ExcelStateChartConverter\bin\Debug\ExcelStateChartConverter.exe
"%CVT%" %~dp0MoveFlow.xlsx ..\created
pause