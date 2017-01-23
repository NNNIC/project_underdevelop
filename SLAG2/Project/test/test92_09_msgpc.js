// メッセージパネル

// Bet line
function msgpc_createPanel__betline($ht)
{
    $ht.betline = util_create_msgobj("BET : X CREDITS");
    $ht.betline.go.name="BET";
    $ht.betline.go.transform.parent = $ht.go.transform;
    $ht.betline.go.transform.localposition = Vector3.up * -110;
}
// bet add button
function msgpc_createPanel__addbetbutton_$_ClickFunc($ht)
{
    PrintLn("Add Button Clicked");
}
function msgpc_createPanel__addbutton($ht)
{
    var but$ht =Hashtable();
    but$ht.width       = 150;
    but$ht.height      =  50;
    but$ht.clickFunc   = msgpc_createPanel__addbetbutton_$_ClickFunc;
   
    but$ht.on_col      = Color.green;
    but$ht.on_text     = "add";
    but$ht.on_text_col = Color.white;
   
    but$ht.off_col     = Color.blue;
    but$ht.off_text    = "ADD";
    but$ht.off_text_col= Color.red;

    $ht.addbutton = CreateButton($ht.butman,"Add",but$ht);
    
    $ht.addbutton.go.transform.parent = $ht.go.transform;
    $ht.addbutton.go.transform.localposition = Vector3.up * -170;
}
// start buton
function msgpc_createPanel__startbutton_$_ClickFunc($ht)
{
    PrintLn("START Button Clicked");
}
function msgpc_createPanel__startbutton($ht)
{
    var but$ht =Hashtable();
    but$ht.width       = 300;
    but$ht.height      =  50;
    but$ht.clickFunc   = msgpc_createPanel__startbutton_$_ClickFunc;
   
    but$ht.on_col      = Color.green;
    but$ht.on_text     = "start";
    but$ht.on_text_col = Color.white;
   
    but$ht.off_col     = Color.blue;
    but$ht.off_text    = "START";
    but$ht.off_text_col= Color.red;

    $ht.startbutton = CreateButton($ht.butman,"START",but$ht);
    
    $ht.startbutton.go.transform.parent = $ht.go.transform;
    
    $ht.startbutton.go.transform.localposition = Vector3.up * -250;
}
// total line
function msgpc_createPanel__totalline($ht)
{
    $ht.totalline = util_create_msgobj("YOU HAVE X CREDITS");
    $ht.totalline.go.transform.parent = $ht.go.transform;
    $ht.totalline.go.transform.localposition = Vector3.up * -330;
    $ht.totalline.go.name = "YOU HAVE X CREDITS";
}

// center line  "DEAL", "OPEN" "SELECT" "WIN" OR "LOSE"
function msgpc_createPanel__centerline($ht)
{
    $ht.centerline = util_create_msgobj("CENTER");
    $ht.centerline.go.transform.parent = $ht.go.transform;
    $ht.centerline.go.transform.localposition = Vector3.up * -210;
    $ht.centerline.go.name = "CENTER";
}

// call button  call/chane 共用
function msgpc_createPanel__callbutton_$_ClickFunc($ht)
{
    PrintLn("CALL Button Clicked");
}
function msgpc_createPanel__callbutton($ht)
{
    var but$ht =Hashtable();
    but$ht.width       = 300;
    but$ht.height      =  50;
    but$ht.clickFunc   = msgpc_createPanel__callbutton_$_ClickFunc;
   
    but$ht.on_col      = Color.green;
    but$ht.on_text     = "call";
    but$ht.on_text_col = Color.white;
   
    but$ht.off_col     = Color.blue;
    but$ht.off_text    = "CALL";
    but$ht.off_text_col= Color.red;

    $ht.callbutton = CreateButton($ht.butman,"CALL",but$ht);
    
    $ht.callbutton.go.transform.parent = $ht.go.transform;
    
    $ht.callbutton.go.transform.localposition = Vector3.up * -250;
}


var msgpc_$ht;

// 本体
function msgpc_createPanel()
{

    msgpc_$ht         = Hashtable();
    msgpc_$ht.go      = new GameObject("msgpc");
    msgpc_$ht.butman  = util_buttonManager();
    
    msgpc_createPanel__betline(msgpc_$ht);
    msgpc_createPanel__addbutton(msgpc_$ht);
    msgpc_createPanel__startbutton(msgpc_$ht);
    msgpc_createPanel__totalline(msgpc_$ht);
    
    //msgpc_$ht.betline.go.SetActive(false);
    msgpc_$ht.addbutton.go.SetActive(false);
    msgpc_$ht.startbutton.go.SetActive(false);
    //msgpc_$ht.totalline.go.SetActive(false);
    
    msgpc_createPanel__centerline(msgpc_$ht);
    msgpc_$ht.centerline.go.SetActive(false);
    
    msgpc_createPanel__callbutton(msgpc_$ht);
}


msgpc_createPanel();
