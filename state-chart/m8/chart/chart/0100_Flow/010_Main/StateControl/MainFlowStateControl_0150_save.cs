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

public partial class MainFlowStateControl {
    
    void save_layout()
    {
        var layoutfile = LoadSave.GetLayoutFilename(m_filename);
        if (!string.IsNullOrEmpty(layoutfile))
        {
            if (LoadSave.SaveLayoyt_to_file(layoutfile))
            {
                MessageBox.Show("Layout File was Updated!" + Environment.NewLine + layoutfile);
            }
            else
            {
                MessageBox.Show("Layout File was not Updated!" + Environment.NewLine + layoutfile);
            }
        }
        else
        {
             MessageBox.Show("Layout File was not Updated!" + Environment.NewLine + "Because no data is found!");
        }
    }
 }