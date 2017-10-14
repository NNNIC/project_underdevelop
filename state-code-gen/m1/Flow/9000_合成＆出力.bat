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

:: --- 

:: --- MODULE
copy 9010_Module1.bas ..\Tool\Ariawase-run\src\test.xlsm\Module1.bas
::pause

:: #
:: # コンバイン
:: #
echo . | call ..\Tool\Ariawase-run\zzz___combine.bat 

:: 出力

copy %~dp0..\Tool\Ariawase-run\bin\test.xlsm ..\*.* 



pause