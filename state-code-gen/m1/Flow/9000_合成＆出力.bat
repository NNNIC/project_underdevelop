cd /d %~dp0

rd /q /s ..\Tool\Ariawase-run\src\test.xlsm 2>nul
md ..\Tool\Ariawase-run\src\test.xlsm

:: #
:: # 合成
:: #

:: --- MAIN
copy /A 0120_main.txt + 0110_MainFlow_created.bas.txt  0130_mainflow.txt
copy 0130_mainflow.txt ..\Tool\Ariawase-run\src\test.xlsm\Mainflow.cls

:: --- PREP
copy /A 0220_prep.txt + 0210_PrepFlow_created.bas.txt  0230_prepflow.txt
copy 0230_prepflow.txt ..\Tool\Ariawase-run\src\test.xlsm\Prepflow.cls

:: --- FUNC

copy /A 0320_func.txt + 0310_FuncFlow_created.bas.txt  0330_funcflow.txt
copy 0330_funcflow.txt ..\Tool\Ariawase-run\src\test.xlsm\Funcflow.cls

:: --- 

:: --- 9010～
copy 9010_Module1.bas    ..\Tool\Ariawase-run\src\test.xlsm\Module1.bas
copy 9100_ReadChart.cls  ..\Tool\Ariawase-run\src\test.xlsm\ReadChart.cls
copy 9500_StringUtil.cls ..\Tool\Ariawase-run\src\test.xlsm\StringUtil.cls
copy 9600_SaveUtil.cls   ..\Tool\Ariawase-run\src\test.xlsm\SaveUtil.cls

::pause

:: #
:: # コンバイン
:: #
echo . | call ..\Tool\Ariawase-run\zzz___combine.bat 

:: 出力

copy %~dp0..\Tool\Ariawase-run\bin\test.xlsm ..\*.* 



pause