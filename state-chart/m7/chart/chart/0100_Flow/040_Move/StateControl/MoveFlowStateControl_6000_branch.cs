using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class MoveFlowStateControl  {
    void br_on_state(Action<int,bool> st) { }
    void br_cancel(Action<int,bool> st) { }
    void br_mouseup(Action<int,bool> st) { }
    void br_mouseup_cancel(Action<int,bool> st) { }
}

