﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

public partial class ChartManager_obs
{
    ExcelProgram m_excelpgm { get {
            if (chart.ChartViewer.V!=null && chart.ChartViewer.V.m_mfsc!=null && chart.ChartViewer.V.m_mfsc.m_excelpgm!=null)
            {
                return chart.ChartViewer.V.m_mfsc.m_excelpgm;
            }
            return null;
        } }

    List<string> get_all_states()
    {
        if (m_excelpgm!=null)
        {
            return m_excelpgm.m_state_list;
        }
        return null;
    }

    string get_nextstate(string state)
    {
        return m_excelpgm.GetValue(state,"nextstate");
    }

    List<string> get_branch(string state)
    {
        var s = m_excelpgm.GetValue(state,"branch");
        var tokens = s.Split('\n',';');
        List<string> lines= new List<string>();
        if (tokens!=null && tokens.Length > 0)
        {
            foreach(var t in tokens)
            {
                var t2 = t.Trim();
                if (string.IsNullOrEmpty(t2)) continue;
                lines.Add(t2);
            }
        }
        return lines;
    }

    void drawNodeBox(Graphics g, string state, Rectangle rect)
    {
        DrawUtil_obs.Pen_set(NODE_LINESIZE);
        DrawUtil_obs.m_font = new Font("Arial",NODE_CHARSIZE);
        DrawUtil_obs.Text(g,state,rect);
        DrawUtil_obs.Rect(g,rect);
    }

    void drawNextArrow(Graphics g, Node node)
    {
        var src = node.srcpoint_next;

        var nextnode = m_nodeList.Find(n=>n.state == node.nextstate);
        if (nextnode==null) return;

        var dst = nextnode.dstpoint;

        DrawUtil_obs.Pen_set(ARROW_WIDTH);
        DrawUtil_obs.ArrowBezir(g,node.arrow_next);
    }

    void drawNextArrow2(Graphics g, Node node)
    {
        var src = node.srcpoint_next;

        var nextnode = m_nodeList.Find(n=>n.state == node.nextstate);
        if (nextnode==null) return;

        var dst = nextnode.dstpoint;


    }

    void drawBranchArrow(Graphics g, Node node)
    {
        if (node.branches==null) return;
        for(var i = 0; i<node.branches.Count; i++)
        {
            var cond = node.branch_cond(i);
            var nst  = node.branch_state(i);
            if (cond==null) continue;
            
            var rect = node.get_branch_text_rect(i);
            var src  = node.srcpoint_branch(i);
            
            var nextnode = m_nodeList.Find(n=>n.state == nst);
            if (nextnode == null) continue;
            
            var dst = nextnode.dstpoint;
            
            DrawUtil_obs.Pen_set(ARROW_WIDTH);
            //DrawUtil.Arrow(g,src,dst);
            DrawUtil_obs.ArrowBezir(g,node.arrow_branch_list[i]);

            DrawUtil_obs.Text(g,cond,rect); 
        }
    }
}
