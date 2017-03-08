cd $PSScriptRoot

$a = Get-Content -Path .\data\a.txt
foreach($i in $a){
    echo "--- $i" 
}

