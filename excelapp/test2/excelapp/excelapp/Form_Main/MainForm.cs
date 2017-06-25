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
        public EditForm     m_editForm;
        public DataGridView m_dataGridView { get { return m_editForm.dataGridView1; } }

        public Config         m_config = new Config();
        public LoadExcel      m_loadexcel;
        public ItemBoxUtil    m_itemBoxUtil;
        public EditFormUtil   m_editFormUtil;

        public List<ItemBoxElement> m_itemBoxElementList = new List<ItemBoxElement>();
        public ItemBoxElement m_draggingItemBox = null;

        public object[,]      m_values { get { return m_loadexcel.m_values; } }

        public MainForm()
        {
            InitializeComponent();
            m_itemBoxUtil  = new ItemBoxUtil(this);
            m_editFormUtil = new EditFormUtil(this);
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_loadexcel = LoadExcel.Load(@"C:\Users\gea01\autoplay\0000_Smoke\Scene.xlsx","MyPageScene");
            m_itemBoxUtil.Draw();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            m_editForm = new EditForm();
            m_editForm.Show();
        }

        public void Update_EditForm(int id)
        {
            m_editFormUtil.UpdateData(id);
        }
    }
}
