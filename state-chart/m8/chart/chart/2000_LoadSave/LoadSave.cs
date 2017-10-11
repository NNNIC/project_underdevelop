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

using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters;

public class LoadSave
{
    #region アクセス
    static MainFlowStateControl m_mfsc      { get { return chart.ChartViewer.V.m_mfsc;     } }
    static List<StateData>      m_stateData { get { return StateInfo.m_stateData;          } }
    #endregion

    public static void DO_LoadData()
    {
        var ofd = new OpenFileDialog();
        ofd.InitialDirectory = @"C:\Users\gea01\Documents\project_underdevelop\state-chart\m8\chart\chart\0100_Flow\010_Main\doc";
        var result = ofd.ShowDialog();
        if (result == DialogResult.OK)
        {
            m_mfsc.Load(ofd.FileName);
        }
    }
    //public static void TRY_LoadData_byArgs()
    //{
    //    System.Diagnostics.Debugger.Break();
    //    var args = Environment.GetCommandLineArgs();
    //    if (args!=null && args.Length>1)
    //    {
    //        m_mfsc.Load(args[1]);
    //    }
    //}

    public static void DO_SaveLayout()
    {
        m_mfsc.SaveLayout();
    }
    public static string GetLayoutFilename(string excelfile)
    {
        try {
            return Path.Combine(Path.GetDirectoryName(excelfile), Path.GetFileName(excelfile) + ".lo");
        }
        catch { }
        return null;
    }

    [Serializable]
    public class SAVEDATA
    {
        [Serializable]
        public class StateItem
        {
            public string state;
            public Point  offset;
        }

        public List<StateItem> list;
    }

    public static bool SaveLayoyt_to_file(string file)
    {
        var savedata = new SAVEDATA();
        savedata.list = new List<SAVEDATA.StateItem>();

        foreach(var st in m_stateData)
        {
            if (st==null || st.m_layout == null) continue;
            var item = new SAVEDATA.StateItem();
            item.state = st.State;
            item.offset = st.m_layout.offset;
            savedata.list.Add(item);
        }

        try {
            Byte[] bdata = null;
            var bf = new BinaryFormatter();
            using(var ms = new MemoryStream())
            {
                bf.Serialize(ms,savedata);
                bdata = ms.ToArray();

                File.WriteAllBytes(file,bdata);
            }
        } catch
        {
            return false;
        }
       
        return true;
    }

    public static SAVEDATA m_savedata { get; private set; }
    public static SAVEDATA LoadLayout(string file)
    {
        m_savedata = null;

        try {
            var bdata = File.ReadAllBytes(file);
            var bf = new BinaryFormatter();
            using(var ms = new MemoryStream(bdata))
            {
                m_savedata = (SAVEDATA)bf.Deserialize(ms);
            }
        }
        catch
        { }
        return m_savedata;
    }
}