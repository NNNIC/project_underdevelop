using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using excelwork;

class work
{
    ExcelControl m_ew;

    public work()
    {
    }

    public void START()
    {
        m_ew = ExcelControl.Create(@"C:\Users\gea01\Documents\project_underdevelop\excelapp\test5\out\a.xls"); 
        m_ew.SetSheet(m_ew.GetActiveSheetIndex());

        m_ew.SetVisible(true);
    }

    public void WRITE()
    {
        for(var r = 0; r<1000; r++) for(var c=0; c<100; c++)
        {
            m_ew.SetObject(r,c,string.Format("r:{0}|c:{1}",r,c) );
       }
        m_ew.Flush();
    }

    public void SAVE()
    {
        m_ew.Save();
    }
    public void CLOSE()
    {
        m_ew.Close();

    }
}
