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
    public partial class MainForm : Form
    {
        Form m_workForm;

        public MainForm()
        {
            InitializeComponent();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_workForm!=null) return;
            m_workForm = new WorkForm();
            m_workForm.Show();
        }
    }
}
