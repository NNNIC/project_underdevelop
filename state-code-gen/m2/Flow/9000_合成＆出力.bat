cd /d %~dp0

rd /q /s ..\Tool\Ariawase-run\src\state_code_gen.xlsm 2>nul
md ..\Tool\Ariawase-run\src\state_code_gen.xlsm

:: #
:: # 合成
:: #

:: --- MAIN
copy /A 0120_main.txt + 0110_MainFlow_created.bas.txt  0130_mainflow.txt
copy 0130_mainflow.txt ..\Tool\Ariawase-run\src\state_code_gen.xlsm\MainFlow.cls

:: --- PREP
copy /A 0220_prep.txt + 0210_PrepFlow_created.bas.txt  0230_prepflow.txt
copy 0230_prepflow.txt ..\Tool\Ariawase-run\src\state_code_gen.xlsm\PrepFlow.cls

:: --- FUNC

copy /A 0320_func.txt + 0310_FuncFlow_created.bas.txt  0330_funcflow.txt
copy 0330_funcflow.txt ..\Tool\Ariawase-run\src\state_code_gen.xlsm\FuncFlow.cls

:: --- 

:: --- 9010～
copy 9010_CodeGenerator.bas         ..\Tool\Ariawase-run\src\state_code_gen.xlsm\CodeGenerator.bas
copy 9020_CodeGenerator_Update.bas  ..\Tool\Ariawase-run\src\state_code_gen.xlsm\CodeGenerator_Update.bas
copy 9100_ReadChart.cls             ..\Tool\Ariawase-run\src\state_code_gen.xlsm\ReadChart.cls
copy 9500_StringUtil.cls            ..\Tool\Ariawase-run\src\state_code_gen.xlsm\StringUtil.cls
copy 9600_SaveUtil.cls              ..\Tool\Ariawase-run\src\state_code_gen.xlsm\SaveUtil.cls


:: --- Update用
set UL=9900_updatelist.txt
:: 　　　　　　　　　　　　　　拡張子 空白 ファイル名
:: 　　　　　　　　　　　　　　※アップデータ自身は対象外！！
echo cls MainFlow       > %UL%
echo cls PrepFlow       >>%UL%
echo cls FuncFlow       >>%UL%
echo bas CodeGenerator  >>%UL%
echo cls ReadChart      >>%UL%
echo cls StringUtil     >>%UL%
echo cls SaveUtil       >>%UL%

::pause

:: #
:: # コンバイン
:: #
echo . | call ..\Tool\Ariawase-run\zzz___combine.bat 

:: 出力

copy %~dp0..\Tool\Ariawase-run\bin\state_code_gen.xlsm ..\*.* 

:: #
:: # アップデートテキスト
:: # 

copy %UL% ..\Tool\Ariawase-run\src\state_code_gen.xlsm\updatelist.txt

pause