using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class MoveFlowStateControl : StateControlBase {

    public void Init()
    {
        sc_start();
    }

    public void Update()
    {
        sc_update();
    }
}
