/*
 Tmp
*/

function S_INIT(bFirst)
{
    if (bFirst)
    {
        PrintLn("S_INIT");
        m_sm.Goto(S_SECOND);
    }
}

function S_SECOND(bFirst)
{
    if (bFirst)
    {
        PrintLn("S_SECOND");
        m_sm.Goto(S_END);
    }
}

function S_END(bFirst)
{
}

var m_sm = StateManager();
m_sm.Goto(S_INIT);

