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
using LineType=DrawUtil.LineType;
//>>>


public partial class ArrowFlowStateControl2 : StateControlBase
{
    const int DUNIT = 10;

    #region アクセス
    public List<StateData> m_stateData {get { return StateInfo.m_stateData;  } }
    #endregion


    public StateData m_state_start;
    public StateData m_state_goal;

    public Point m_posS;
    public Point m_posG;

    public int?  m_branch_index;
    public bool  m_bNextOrBranch { get { return m_branch_index==null; } }

    public List<Point> m_result;

    /// <summary>
    ///
    /// </summary>
    /// <param name="start_st">始点所属ステートデータ</param>
    /// <param name="goal_st">終点所属ステートデータ</param>
    /// <param name="branch_index">null：next,数値はブランチのインデックス</param>
    /// <param name="start">開始位置</param>
    /// <param name="goal">終了位置</param>
    public void Begin(StateData start_st, StateData goal_st, int? branch_index, Point start, Point goal)
    {
        m_state_start = start_st;
        m_state_goal  = goal_st;

        m_branch_index = branch_index;

        m_posS = start;
        m_posG  = goal;

        sc_start(S_NONE);

        SetNextState(S_START);
        GoNextState();
    }

    public void Calc()
    {
        for(var loop = 0; loop<10000; loop++)
        {
            sc_update();
            if (m_sm.CheckState(S_END))
            {
                break;
            }
        }
    }

    public List<Point> GetResult()
    {
        return m_result;
    }
}
