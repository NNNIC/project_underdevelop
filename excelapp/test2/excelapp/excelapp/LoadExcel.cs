using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace excelapp
{
    public class LoadExcel
    {
        #region 生成
        private LoadExcel() { }
        public static LoadExcel Load(string file, string sheet)
        {
            var le = new LoadExcel();
            le._load(file,sheet);
            return le;
        }
        #endregion

        #region load update save

        private ExcelUtil.BookCtr  m_book;
        private ExcelUtil.SheetCtr m_sheet;
        public  object[,]          m_values;

        private void _load(string file, string sheet)
        {
            m_book = ExcelUtil.AttackBook(file);
            if (m_book==null)
            {
                m_book = ExcelUtil.OpenBook(file);
            }
            m_book.SetVisible(true);

            m_sheet = m_book.GetSheet(sheet);

            m_values = m_sheet.GetValues();
        }

        public void Update()
        {
            if (m_sheet!=null &&  m_values!=null)
            {
                m_sheet.SetValues(m_values);
            }
        }

        public void Save()
        {
            if (m_sheet!=null && m_book!=null)
            {
                m_book.Write();
            }
        }

        #endregion

    }
}
