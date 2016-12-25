//
// test 08a
//


//StateInit("S_START");
var sm = StateManager();

sm.Goto("S_START");

function S_START(bFirst)
{
    if (bFirst)
    {
        PrintLn("== START ==");
        //StateGoto("S_READ");
        sm.Goto("S_READ");
    }
}

function S_READ(bFirst)
{
    if (bFirst)
    {
        ReadLineStart("Input>");
    }
    else
    {
        var s = ReadLineDone();
        if (s!=null)
        {
            sm.Goto("S_END");
        }
    }
}

function S_END(bFirst)
{
    if (bFirst)
    {
        PrintLn("== END ==");
    }
}
