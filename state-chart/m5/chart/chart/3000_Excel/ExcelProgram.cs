//<<<include=using_text.txt
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Detail=DrawStateBox.Detail;
//>>>

public class ExcelProgram
{
    const int NAME_COL  = 1;
    const int START_COL = 3;
    const int STATE_ROW = 1;

    ExcelLoadValues m_ld;

    public List<string> m_state_list {get;private set; }

    public void Load(string file)
    {
        m_ld = new ExcelLoadValues(file);

        //stateを収集
        var state_str = string.Empty;
        var state_list = new List<string>();
        {
            for(var c = START_COL; c<m_ld.GetMaxCol(); c++)
            {
                var s = m_ld.GetValue(STATE_ROW,c);
                if (string.IsNullOrEmpty(s) || string.IsNullOrEmpty(s.Trim())) continue;
                state_list.Add(s.Trim());
            }

            //
            state_list.ForEach(s=> {
                if (!string.IsNullOrEmpty(state_str)) state_str += ",";
                state_str += s + "\n";
            });
        }
        m_state_list = state_list;


        //StateInfoに収集
        StateInfo.m_stateData = new List<StateData>();

        foreach(var st in state_list)
        {
            var itemlist = new List<StateData.Item>();
            for(var i = 0; i<m_ld.GetMaxRow();i++)
            {
                var key = m_ld.GetValue(i,NAME_COL).Trim();
                if (!string.IsNullOrEmpty(key))
                {
                    var item = new StateData.Item();
                    item.index = i;
                    item.key = key;
                    item.value = GetValue(st,key);
                    itemlist.Add(item);
                }
            }
            var statedata = new StateData();
            statedata.m_items = itemlist;

            StateInfo.m_stateData.Add(statedata);
        }

        //各ステートデータのDistinationを設定
        var stateData = StateInfo.m_stateData;
        foreach(var st in stateData)
        {
            var next = st.NextState;
            if (!string.IsNullOrEmpty(next))
            {
                st.m_dist_nextstate = stateData.Find(i=>i.State==next);
            }
            var brnum = st.NumBranches;
            if (brnum!=0)
            {
                st.m_dist_branches = new StateData[brnum];

                for(var i = 0; i<brnum; i++)
                {
                    var next2 = st.GetBranchParam(i);
                    st.m_dist_branches[i] = stateData.Find(s=>s.State == next2);
                }
            }
        }
    }

    int _getColIndexByState(string st)
    {
        var row = _getRowIndexByName("state");
        for(var c = START_COL; c<m_ld.GetMaxCol();c++)
        {
            var s = m_ld.GetValue(row,c).Trim();
            if (s==st)
            {
                return c;
            }
        }
        return -1;
    }

    int _getRowIndexByName(string nm)
    {
        for(var i = 0; i<m_ld.GetMaxRow();i++)
        {
            var s = m_ld.GetValue(i,NAME_COL).Trim();
            if (s==nm)
            {
                return i;
            }
        }
        return -1;
    }

    public string GetValue(string state, string name)
    {
        return _getvalue(state, name);
    }
    string _getvalue(string state, string name)
    {
        var row = _getRowIndexByName(name);
        var col = _getColIndexByState(state);

        return m_ld.GetValue(row,col);
    }
}
