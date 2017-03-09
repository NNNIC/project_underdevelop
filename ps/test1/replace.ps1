cd $PSScriptRoot

$a = Get-Content -Path .\data\b.txt
$a
$b = $a.Replace("[id]","*hog*");
$b | Out-File .\data\bx.txt

