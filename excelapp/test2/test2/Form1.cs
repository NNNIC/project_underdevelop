using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace test2
{

    public partial class Form1 : Form
    {

        Microsoft.Office.Interop.Excel.Application m_app;
        Microsoft.Office.Interop.Excel.Workbooks   m_wbs;
        Microsoft.Office.Interop.Excel.Workbook    m_wb;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try { m_app = (Microsoft.Office.Interop.Excel.Application)Microsoft.VisualBasic.Interaction.GetObject(null,"Excel.Application"); } catch { m_app = null;}
            if (m_app==null)
            {
                MessageBox.Show("Faild to Attach!");
                return;
            }           
            MessageBox.Show("Success to Attach!");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try {
                if (m_wb!=null) {
                    try { m_wb.Close(false); } catch { }
                    Marshal.ReleaseComObject(m_wb);
                    m_wb = null;
                }
            } catch {  m_wb = null; }
            try {
                if (m_wbs!=null)
                {
                    Marshal.ReleaseComObject(m_wbs);
                    m_wbs = null;
                }
            } catch { m_wbs = null;   }

            try {
                if (m_app!=null)
                {
                    try {  m_app.Quit(); } catch { }
                    Marshal.ReleaseComObject(m_app);
                    m_app = null;
                }
            } catch { m_app = null; }            
            System.GC.Collect();
        }
    }
}
