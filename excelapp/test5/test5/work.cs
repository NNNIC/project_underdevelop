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
        m_ew.SetObject(0,0,"0");
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
