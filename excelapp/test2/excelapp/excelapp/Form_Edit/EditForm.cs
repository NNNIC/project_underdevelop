using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace excelapp
{
    public partial class EditForm : Form
    {
        MainForm m_mainForm;
        LoadExcel m_loadExcel       { get { return m_mainForm.m_loadexcel;    } }
        EditFormUtil m_editFormUtil { get { return m_mainForm.m_editFormUtil; } }

        public bool m_bReady;

        public EditForm(MainForm form)
        {
            m_bReady = false;
            m_mainForm = form;
            InitializeComponent();
        }

        private void EditForm_Load(object sender, EventArgs e)
        {
            this.Activate();
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (m_editFormUtil==null || m_loadExcel==null) return;

            if (!m_bReady) return;

            System.Diagnostics.Debug.WriteLine("Changed!");
         
            m_editFormUtil.userChanged();
            
            m_loadExcel.Update();   
        }
    }
}
