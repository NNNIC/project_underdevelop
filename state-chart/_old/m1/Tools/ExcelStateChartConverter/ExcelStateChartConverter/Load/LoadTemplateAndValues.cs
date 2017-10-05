using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelStateChartConverter
{
    class LoadTemplateAndValues
    {
        public string    m_template_statefunc { get; private set; }
        public string    m_template_source    { get; private set; }
        public object[,] m_values             { get; private set; }

        public LoadTemplateAndValues(string file)
        {
            using (var ew = new ExcelWork())
            {
                ew.Load(file);
                //
                ew.SetSheet("template-source");
                m_template_source = ew.GetValue(0,0);
             
                ew.SetSheet("template-statefunc");
                m_template_statefunc = ew.GetValue(0,0);   

                ew.SetSheet("state-chart");
                m_values = (object[,])ew.GetValues().Clone();
            }
        }

        public int GetMaxRow() { return m_values.GetLength(0); }
        public int GetMaxCol() { return m_values.GetLength(1); }
        public string GetValue(int row,int col) // base 0
        {
            if (
                (row >= 0 && row < GetMaxRow()) 
                &&
                (col >=0 && col < GetMaxCol())
                )
            {
                try {
                    var v = m_values[row+1,col+1].ToString(); 
                    if (v!=null && (v.Length>0 && v[0]!='#'))
                    {
                        return v;
                    }
                }
                catch {
                    
                }
            }
            return "";
        }

        public string GetInitalSource(out string filename)
        {
            string mark = ":output=";
            filename = string.Empty;
            var output = string.Empty;
            foreach(var i in EditUtil.Split(m_template_source))
            {
                if (string.IsNullOrEmpty(i) || string.IsNullOrEmpty(i.TrimEnd())) continue;
                var l = i.TrimEnd();
                if (l.StartsWith(mark))
                {
                    filename = l.Substring(mark.Length);
                    continue;
                }
                if (l[0]==':') continue;

                output += l + "\n";
            }
            return output;
        }

        public string GetInitialFuncSource()
        {
            var output = string.Empty;
            foreach(var i in EditUtil.Split(m_template_statefunc))
            {
                if (string.IsNullOrEmpty(i) || string.IsNullOrEmpty(i.TrimEnd())) continue;
                var l = i.TrimEnd();
                if (l[0]==':') continue;

                output += l + "\n";

            }
            return output;
        }
    }
}
