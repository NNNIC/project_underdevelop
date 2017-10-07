//<<<include=using_text.txt
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
//>>>

class ExcelLoadValues
{
    public object[,] m_values             { get; private set; }

    public ExcelLoadValues(string file)
    {
        using (var ew = new ExcelWork())
        {
            ew.Load(file);

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
}
