using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using excelwork;

class work
{

    ExcelControlWork m_ew;
    IEnumerator      m_etr;

    public work()
    {
    }

    public void START()
    {
        m_ew = ExcelControl.Create(@"C:\Users\gea01\Documents\project_underdevelop\excelapp\test4\out\a.xls"); 
        m_ew.SetSheet(m_ew.GetActiveSheetIndex());

        m_ew.SetVisible(true);
    }

    public void WRITE()
    {
        m_ew.SetObject(0,0,"0");
    }

    public void SAVE()
    {
        ExcelControl.Save(m_ew);
    }
    public void CLOSE()
    {
        ExcelControl.Close(m_ew);
    }

    public void Update()
    {
    }

    // ステート
    void S_IDLE(bool bFirst) { }

    //void S_START(bool bFirst)
    //{
    //    if (bFirst)
    //    {
    //        m_ew = ExcelControl.Create(@"C:\Users\gea01\Documents\project_underdevelop\excelapp\test4\out\a.xls"); 
    //        m_ew.SetSheet(m_ew.GetActiveSheetIndex());

    //        m_ew.SetVisible(true);
    //        m_sm.Goto(S_WRITE);
    //    }
    //}

    //void S_WRITE(bool bFirst)
    //{
    //    if (bFirst)
    //    {
    //        m_etr = Coroutine();
    //    }            
    //    else
    //    {
    //        if (!m_etr.MoveNext())
    //        {
    //            m_sm.Goto(S_IDLE,500);
    //        }
    //    }
    //}

    //IEnumerator Coroutine()
    //{
    //    for(int x = 0; x<10; x++)
    //    {
    //        m_ew.SetObject(x,0,"x=" + x);
    //        yield return null;
    //    }
    //}
       
}
