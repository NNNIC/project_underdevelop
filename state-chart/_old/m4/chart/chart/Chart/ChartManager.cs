using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

public partial class ChartManager
{
    const int NODE_WIDTH     = 120;
    const int NODE_HEIGHT    = 60;
    const int NODE_WIDTH_PAD = 60;
    const int NODE_LINESIZE  = 3;
    const int NODE_CHARSIZE  = 11;

    readonly Point POINT_START = new Point(30,30);
    readonly Size  NODE_SIZE   = new Size(NODE_WIDTH,NODE_HEIGHT);

    const int ARROW_WIDTH      = 1;
    const int ARROW_BUFFER     = 15; //曲線の矢印時の緩衝用

    public const int ARROW_GAP =  5;
    public const int ARROW_DIF =  0;

    PictureBox m_mainPicture { get { return chart.Form1.V.pictureBox_main; }  }

    public List<Node>       m_nodeList { get { return __nodeList; } } 
    private List<Node>      __nodeList  = new List<Node>();     
   
    List<PictureBox> m_subPictureList   = new List<PictureBox>();
    

    public void Create()
    {
        m_nodeList.Clear();

        var statelist = get_all_states();
        var point     = POINT_START;
        for(var i = 0; i<statelist.Count; i++)
        {
            var state      = statelist[i];
            var node       = new Node();
            node.state     = state;
            node.nextstate = get_nextstate(state);
            node.branches  = get_branch(state);
            point          = DrawUtil.Add_X(point, (i==0 ? 0 : NODE_WIDTH+NODE_WIDTH_PAD));
            node.rect      = new Rectangle(point,NODE_SIZE);

            m_nodeList.Add(node);
        }

        //Arrowポイント
        foreach(var node in m_nodeList)
        {
            var nextnode = m_nodeList.Find(n=>n.state==node.nextstate);
            if (nextnode !=null)
            {
                node.arrow_next =   ArrowFlowUtil.Create(
                                        node.srcpoint_next,             
                                        nextnode.dstpoint,
                                        node.buf_srcpoint_next,
                                        nextnode.buf_dstpoint,
                                        true
                                    );
            }

            node.arrow_branch_list = new List<Point>[node.branches.Count];
            for(var i=0; i< node.branches.Count ; i++)
            {
                var branch_state = node.branch_state(i);
                nextnode = m_nodeList.Find(n=>branch_state == n.state);
                if (nextnode == null) continue;

                node.arrow_branch_list[i] = ArrowFlowUtil.Create(
                                                node.srcpoint_branch(i),
                                                nextnode.dstpoint,
                                                node.buf_srcpoint_branch(i),
                                                nextnode.buf_dstpoint,
                                                false
                                            );
            }
        }

        Draw();
    }

    public void Draw()
    {
        using (var g = m_mainPicture.CreateGraphics())
        {
            g.Clear(Color.White);

            foreach(var node in m_nodeList)
            {
                drawNodeBox(g,node.state,node.rect);
                drawNextArrow(g,node);
                drawBranchArrow(g,node);
            }
        }
    }

}
