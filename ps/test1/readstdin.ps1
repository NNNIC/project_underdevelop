echo "Input Y or N"
$a = read-host
if ($a -eq "Y") {
   echo "You input Y"
}
elseif ($a -eq "N")
{
    echo "You input N"
}
else 
{
    echo "You input unknown : $a"
}