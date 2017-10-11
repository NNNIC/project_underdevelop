cd /d %~dp0
set CVT=%~dp0..\..\..\..\..\Tools\ExcelStateChartConverter\ExcelStateChartConverter\bin\Debug\ExcelStateChartConverter.exe
"%CVT%" %~dp0ArrowFlow.xlsx ..\created
::pause