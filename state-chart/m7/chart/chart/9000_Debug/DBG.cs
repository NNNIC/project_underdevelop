//<<<include=using_text.txt
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using ChartViewer = chart.ChartViewer;
using Detail=DrawStateBox.Detail;
using LineType=DrawUtil.LineType;
using D=Define;
//>>>

using System.Diagnostics;

public class DBG
{
    public static void LogWrite(string s)
    {
        chart.Form_Debug.V.textBox_log.Text += s;
        Debug.Write(s);   
    }
    public static void LogClear(string s)
    {
        chart.Form_Debug.V.textBox_log.Text="";
    }
    public static void LogEvent(string s)
    {
        chart.Form_Debug.V.textBox_event.Text = s;
    }
}
