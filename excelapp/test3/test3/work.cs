using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using excelwork;

namespace test3
{
    class work
    {

        StateManager m_sm;
        ExcelControlWork m_ew;

        public work()
        {
            m_sm = new StateManager();
            m_sm.Goto(S_IDLE);
        }



        public void Update()
        {
            m_sm.Update();
        }

        // ステート
        void S_IDLE(bool bFirst) { }

        void S_START(bool bFirst)
        {
            if (bFirst)
            {
                m_ew = ExcelControl.Create(@"C:\Users\gea01\Documents\project_underdevelop\excelapp\test3\out\a.xls");           
                ExcelControl.SetVisible(m_ew,true);
            }
        }


    }
}
