using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace excelapp
{
    public class EditFormUtil
    {
        MainForm     m_mainForm;
        EditForm     m_editForm     { get { return m_mainForm.m_editForm; } }
        DataGridView m_dataGridView { get { return m_mainForm.m_dataGridView; } }
        
        private  List<int> m_range      { get { return m_mainForm.m_itemBoxUtil.m_range;      } }
        private  int       m_main_col   { get { return m_mainForm.m_itemBoxUtil.m_main_col;   } }
        private  int       m_header_row { get { return m_mainForm.m_itemBoxUtil.m_header_row; } }
        private  int       m_bottom_row { get { return m_mainForm.m_itemBoxUtil.m_bottom_row; } }

        private  object[,] m_values     { get { return m_mainForm.m_values;                   } }

        int m_save_row;

        public EditFormUtil(MainForm mainform)
        {
            m_mainForm = mainform;
        }

        public void UpdateData(int row)
        {
            if (m_save_row==row) return;

            m_editForm.m_bReady = false;
            {
                m_save_row = row;
            
                //全削除
                m_dataGridView.Rows.Clear();

                foreach(var col in m_range)
                {
                    var newrow  = m_dataGridView.Rows.Add();
                    var newrows = m_dataGridView.Rows[newrow];
                    try {  newrows.Cells[0].Value = m_values[m_header_row,col].ToString() ; } catch { }
                    try { newrows.Cells[1].Value = m_values[m_save_row,col].ToString()   ; } catch { }
                }
            }
            m_editForm.m_bReady = true;
        }

        public void userChanged()
        {
            if (m_values==null || m_dataGridView==null) return;

            m_editForm.m_bReady = false;
            {
                int i = 0;
                foreach(var col in m_range)
                {
                    m_values[m_save_row,col] = m_dataGridView.Rows[i++].Cells[1].Value;
                }
            }
            m_editForm.m_bReady = true;
        }
    }
}
