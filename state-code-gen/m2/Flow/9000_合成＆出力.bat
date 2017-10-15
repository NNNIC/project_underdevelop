cd /d %~dp0

rd /q /s ..\Tool\Ariawase-run\src\state_code_gen.xlsm 2>nul
md ..\Tool\Ariawase-run\src\state_code_gen.xlsm

:: #
:: # ����
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

:: --- 9010�`
copy 9010_CodeGenerator.bas         ..\Tool\Ariawase-run\src\state_code_gen.xlsm\CodeGenerator.bas
copy 9020_CodeGenerator_Update.bas  ..\Tool\Ariawase-run\src\state_code_gen.xlsm\CodeGenerator_Update.bas
copy 9100_ReadChart.cls             ..\Tool\Ariawase-run\src\state_code_gen.xlsm\ReadChart.cls
copy 9500_StringUtil.cls            ..\Tool\Ariawase-run\src\state_code_gen.xlsm\StringUtil.cls
copy 9600_SaveUtil.cls              ..\Tool\Ariawase-run\src\state_code_gen.xlsm\SaveUtil.cls


:: --- Update�p
set UL=9900_updatelist.txt
:: �@�@�@�@�@�@�@�@�@�@�@�@�@�@�g���q �� �t�@�C����
:: �@�@�@�@�@�@�@�@�@�@�@�@�@�@���A�b�v�f�[�^���g�͑ΏۊO�I�I
echo cls MainFlow       > %UL%
echo cls PrepFlow       >>%UL%
echo cls FuncFlow       >>%UL%
echo bas CodeGenerator  >>%UL%
echo cls ReadChart      >>%UL%
echo cls StringUtil     >>%UL%
echo cls SaveUtil       >>%UL%

::pause

:: #
:: # �R���o�C��
:: #
echo . | call ..\Tool\Ariawase-run\zzz___combine.bat 

:: �o��

copy %~dp0..\Tool\Ariawase-run\bin\state_code_gen.xlsm ..\*.* 

:: #
:: # �A�b�v�f�[�g�e�L�X�g
:: # 

copy %UL% ..\Tool\Ariawase-run\src\state_code_gen.xlsm\updatelist.txt

pause