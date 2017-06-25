using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace excelapp
{
    public class ItemBoxUtil
    {
        private MainForm m_form;
        private Config   m_config { get {  return m_form.m_config; } }
        private List<ItemBoxElement> m_itemBoxElementList {  get { return m_form.m_itemBoxElementList;  } }
        private object[,]            m_values { get {
                if (m_form.m_loadexcel!=null)
                {
                    return m_form.m_loadexcel.m_values;
                }
                return null;
            } }

        public  List<int> m_range      { get; private set; }
        public  int       m_main_col   { get; private set; }
        public  int       m_header_row { get; private set; }
        public  int       m_bottom_row { get; private set; }

        public ItemBoxUtil(MainForm form)
        {
            m_form = form;
            _setup_range();
            _setup_row_col();
        }

        public void Draw()
        {
            EraseAll();

            int index = 1;
            for(var n = m_header_row + 1; n<=m_bottom_row; n++)
            {
                if (_isValidRowValues(n))
                {
                    object o = null;
                    try {
                        o = m_values[n,m_main_col];
                    }
                    catch {  o = null;}

                    string s = o!=null ? o.ToString() : "";

                    var ib = ItemBoxElement.Create(n.ToString(), m_form, 100, 30 * index++);
                    ib.SetText(s);
                    ib.SetRowText(n.ToString("0000"));
                    m_itemBoxElementList.Add(ib);
                }
            }
        }

        public void EraseAll()
        {
            foreach(var i in m_itemBoxElementList)
            {
                i.Destroy();
            }
            m_itemBoxElementList.Clear();
        }
        //
        void _setup_range()
        {
            var org = m_config.Range;
            var tokens = org.Split(',');

            var list = new List<int>();
            foreach(var t in tokens)
            {
                if (t.Contains("-"))
                {
                    var startend = t.Split('-');
                    var start = ExcelUtil._toNum_nb1(startend[0]);
                    var end   = ExcelUtil._toNum_nb1(startend[1]);
                    for(var n = start; n<=end; n++)
                    {
                        if (!list.Contains(n))
                        {
                            list.Add(n);
                        }
                    }
                }
                else
                {
                    var n = ExcelUtil._toNum_nb1(t);
                    if (!list.Contains(n))
                    {
                        list.Add(n);
                    }
                }
            }
            list.Sort();
            m_range = list;

        }
        void _setup_row_col()
        {
            m_header_row = (int)ParseUtil.IntParse(m_config.HeaderRow);
            m_bottom_row = (int)ParseUtil.IntParse(m_config.BottomRow);
            m_main_col   = ExcelUtil._toNum_nb1(m_config.MainCol);
        }
        bool _isValidRowValues(int n)
        {
            if (n<=m_header_row) return false;
            if (n> m_bottom_row) return false;

            bool bOk=false;
            foreach(var i in m_range)
            {
                try {
                    if (m_values[n,i]!=null)
                    {
                        bOk = true;
                    }
                }
                catch { return false;}
            }
            return bOk;
        }
    }
}
