using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test7
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        excelUtil.BookCtr  m_bc;
        excelUtil.SheetCtr m_sheet;

        private void button1_Click(object sender, EventArgs e)
        {
            m_bc = excelUtil.OpenBook(@"C:\Users\gea01\Documents\project_underdevelop\excelapp\test7\out\a.xls");
            m_bc.SetVisible(true);
            m_sheet = m_bc.GetActiveSheet();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var cols = 10;
            var rows = 2;
            var objs = m_sheet.GetValues(rows,cols);
            for(var x = 1; x<=rows; x++) for(var y = 1; y<=cols; y++)
            {
                objs[x,y] = x.ToString() + "|" + y.ToString();
            }
           m_sheet.SetValues(objs);

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_bc!=null)
            {
                m_bc.Dispose();
            }
        }
    }
}
