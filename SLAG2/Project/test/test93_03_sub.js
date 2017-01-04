/*
    TEST93 Sub
*/
function _create_circle_notch(go,radius,num_of_div,scale,color)
{
    var angle_div = 360 / num_of_div;
    var angle     = angle_div * UnityEngine.Mathf.Deg2Rad;
    
    for(var n = 0; n<num_of_div; n++)
    {
        var notch = CreateNotch(scale);
        notch.transform.parent = go.transform;
        
        var a = angle * n;
        var x = UnityEngine.Mathf.Cos(a);
        var y = UnityEngine.Mathf.Sin(a);
        
        notch.transform.localPosition = new UnityEngine.Vector3(x*radius,y*radius,0);
        notch.transform.eulerAngles   = UnityEngine.Vector3.forward * (90 + angle_div*n);
        
        util_ChangeColor(notch,color);
    }
}
function _create_circle_text(go, strlist, radius, scale, zpos, color)
{
    var num_of_div = strlist.Count;
    var angle_div = 360 / num_of_div;
    var angle     = angle_div * UnityEngine.Mathf.Deg2Rad;

    for(var n = 0; n<num_of_div; n++)
    {
        var obj = CreateTxtObj(strlist[n],scale);
        obj.transform.parent = go.transform;
        
        var a = - angle * n + 90*UnityEngine.Mathf.Deg2Rad;
        PrintLn(a);
        var x = UnityEngine.Mathf.Cos(a);
        var y = UnityEngine.Mathf.Sin(a);
        
        obj.transform.localPosition = new UnityEngine.Vector3(x*radius,y*radius,zpos);
        
    }
}


// ####################
// # CREATE BIG FRAME #
function Create_big_frame()
{
	var frame = CreateCircle(3.5,80,true);
	util_ChangeColor(frame,new UnityEngine.Color(164/255,164/255,164/255,1));

    _create_circle_notch(frame,3,4,new UnityEngine.Vector3(0.05,0.1,0.1),UnityEngine.Color.red);
    _create_circle_notch(frame,3.025,8,new UnityEngine.Vector3(0.05,0.075,0.05),UnityEngine.Color.white);
    _create_circle_notch(frame,3.05,8*5,new UnityEngine.Vector3(0.05,0.075,0.05),UnityEngine.Color.black);
    
    _create_circle_text(frame,["0.0","0.5","1.0","1.5","2.0","2.5","3.0","3.5"],2.7,0.05,-0.01,UnityEngine.Color.white);
    
    return frame;
}
function Create_big_hand(go)
{
    var hand = CreateHand(0.02,3.2,UnityEngine.Color.red);
    hand.transform.parent =  go.transform;
    hand.transform.localPosition= UnityEngine.Vector3.forward * (-0.15);
    
    return hand;
}
// # CREATE BIG FRAME #
// ####################

// #####################
// # CREATE MINI FRAME #
function Create_mini_frame()
{
    var frame = CreateCircle(1.2,40,true);
    util_ChangeColor(frame,new UnityEngine.Color(94/255,94/255,94/255,1));
    
    _create_circle_notch(frame,1,12,new UnityEngine.Vector3(0.02,0.02,0.1),UnityEngine.Color.white);
    
    _create_circle_text(frame,["0","1","2","3","4","5","6","7","8","9","10","11"],0.8,0.04,-0.01,UnityEngine.Color.white);
    
    return frame;
}
function Create_mini_hand(go)
{
    var hand = CreateHand(0.02,1.1,UnityEngine.Color.red);
    hand.transform.parent =  go.transform;
    hand.transform.localPosition= UnityEngine.Vector3.forward * (-0.15);
    
    return hand;
}
// # CREATE MINI FRAME #
// #####################
