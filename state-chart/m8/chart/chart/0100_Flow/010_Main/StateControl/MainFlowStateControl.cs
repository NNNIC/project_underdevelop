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

public partial class MainFlowStateControl : StateControlBase {
    
    public enum COMMAND
    {
        NONE,
        LOAD,
        SAVELAYOUT
    }


    //StateManagerWithPhase m_sm;
    COMMAND m_cmd = COMMAND.NONE;
    string  m_error = null;

    //public ChartManager_obs m_chartman { get { return chart.Form1.V.m_chartman; } }
    public ChartManager m_chartman { get { return chart.ChartViewer.V.m_chartman; } }
    public ExcelProgram m_excelpgm { get; private set;                        }

    public void Start()
    {
        sc_start(S_START);
    }

    public void Update()
    {
        m_sm.Update();
    }
    
    string m_filename = string.Empty;
    public void Load(string file)
    {
        m_filename = file;
        m_cmd = COMMAND.LOAD;
    }

    public void SaveLayout()
    {
        m_cmd = COMMAND.SAVELAYOUT;
    }
}