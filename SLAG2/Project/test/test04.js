//
// TEST 04
//  

var sum =   0;
for ( var i = 0; i<20; i=i+1) 
{
	sum = sum + i;
	
	if (i==10) { Print("break!!\n");  break;  }
}

Print("total="+sum);


