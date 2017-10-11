using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class ChartManager
{
    List<string> get_all_states()
    {
        var list = new List<string>();
        foreach(var st in m_stateData)
        {
            list.Add(st.State);
        }
        return list;
    }
}
