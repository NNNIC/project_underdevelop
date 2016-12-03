//
// test 08a
//


StateInit("S_START");

function S_START(bFirst)
{
    if (bFirst)
    {
        PrintLn("== START ==");
        StateGoto("S_READ");
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
            PrintLn("Input>" + s);
            StateGoto("S_END");
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



