//
// TEST 91
//
// 数字当てゲーム
// Guess What number is.
//
// 3桁の数字を当ててください。


var MAXTRY = 20;
var NUM = 0;

var s_number = 0;
var s_try    = 0;
var s_guess  = null;

//StateInit("S_Q_START");

var m_sm = StateManager();
m_sm.Goto("S_Q_START");

function S_Q_START(bFirst)
{
    if (bFirst) {
        PrintLn("*START*");

        NUM = NUM + 1;

        while (true) {
            s_number = UnityEngine.Mathf.Floor(UnityEngine.Random.Range(100, 1000));
            var n3 = s_number % 10;
            var n2 = UnityEngine.Mathf.Floor(s_number / 10) % 10;
            var n1 = UnityEngine.Mathf.Floor(s_number / 100);

            PrintLn(s_number);

            if (n1!=n2 && n1!=n3 && n2!=n3)
            {
                break;
            }
        }

        Print("\n\n\n");
        Print("-----------------------------\n");
        Print("!! Question " + NUM + " !! \n");
        Print("-----------------------------\n");

        m_sm.WaitTime(0.5);
    }
    else
    {
        Print("There is a number between from 100 to 999.\n");
        Print("Guess the number!\n\n");

        s_try = 0;

        m_sm.Goto("S_Q_TRY");
    }
}

function S_Q_TRY( bFirst)
{
    if (bFirst)
    {
        s_try++;
        if (s_try > MAXTRY)
        {
            Print("YOU ARE FAILD. IT'S GAME OVER!!\n");
            StateGoto("S_Q_END");
            return;
        }
        Print("### TRY " + (s_try + 1) + " ###\n");
        s_guess = null;
        
        m_sm.Goto("S_Q_INPUT");
    }
}

function S_Q_INPUT(bFirst)
{
    if (bFirst)
    {
        ReadLineStart("Input your guess");
    }
    else
    {
        var s = ReadLineDone();
        if (s != null) {
            var n = ToNumber(s);
            if (n < 100 || n > 999) {
                Print("Input is not correct.\n");
                m_sm.Goto("S_Q_INPUT");
            }
            else {
                s_guess = n;
                m_sm.Goto("S_Q_CHECK");
            }
        }
    }
}

function S_Q_CHECK(bFirst)
{
    if (bFirst)
    {
        if (s_guess == s_number) { 
            m_sm.Goto("S_Q_CONGRATULATION");
            return;
        }
        else if (s_guess > s_number)
        {
            Print("\n\n... The number is less than " + s_guess + "\n\n");
        }
        else //if (s_guess < s_number)
        {
            Print("\n\n... The number is grater than " + s_guess + "\n\n");
        }
        m_sm.WaitTime(1);
    }
    else
    {
        var n3 = s_guess % 10;
        var n2 = UnityEngine.Mathf.Floor(s_guess / 10) % 10;
        var n1 = UnityEngine.Mathf.Floor(s_guess / 100);

        //PrintLn("s_guess.split=" + n1 + n2 + n3);

        var nm3 = s_number % 10;
        var nm2 = UnityEngine.Mathf.Floor(s_number / 10) % 10;
        var nm1 = UnityEngine.Mathf.Floor(s_number / 100);

        //PrintLn("s_number.split=" + nm1 + nm2 + nm3 + "\n");

        var rightplace = 0;
        if (n3 == nm3) { rightplace = rightplace + 1; }
        if (n2 == nm2) { rightplace = rightplace + 1; }
        if (n1 == nm1) { rightplace = rightplace + 1; }

        var anyplace = 0;
        if (n1 == nm2 || n1 == nm3) { anyplace = anyplace + 1; }
        if (n2 == nm1 || n2 == nm3) { anyplace = anyplace + 1; }
        if (n3 == nm2 || n3 == nm1) { anyplace = anyplace + 1; }

        Print("The digits of the number exist the same place : " + rightplace + "\n");
        Print("The digits of the number exist any place      : " + anyplace + "\n");

        m_sm.WaitTime(1);
        m_sm.Goto("S_Q_TRY");
    }
}

function S_Q_END(bFirst)
{
    if (bFirst)
    {
        PrintLn("*END*");
    }
}

function S_Q_CONGRATULATION(bFirst)
{
    if (bFirst) {
        Print("\n");
        Print("******************************************************\n");
        Print("... Congraturations! Yes, the number is " + s_number + "\n");
        Print("******************************************************\n\n");

        m_sm.Goto("S_Q_END");
    }
}
