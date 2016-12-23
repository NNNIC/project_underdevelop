﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using ARRAY = System.Collections.Generic.List<object>;

#if NUMBERISFLOAT
using number = System.Single;
#else
using number = System.Double;
#endif

namespace slagtool.runtime
{
    public class CFG //Config
    {
        public const int    LOOPMAX      = 9999;
        public const string TMPBIN       = null; //キャッシュ  nullを指定すると内部保存
    }

    public enum BREAKTYPE
    {
        NONE,
        BREAK,
        CONTINUE,
        RETURN
    }

    public class Literal //非変数名。ロケーション要素に使用
    {
        public string s;
    }

#region ピリオド区切りのポインタ変数要素
    public enum  PointervarMode { GET,SET,NEW } 
    public class PointervarItem 
    {
        public PointervarMode mode;

        public object o;
        public override string ToString()
        {
            return o.ToString();
        }
        public Func<object>    getter;
        public Type            setter_parametertype;
        public Action<object>  setter;

        public void getter_setter_null()
        {
            getter = null;
            setter = null;
        }
    }
#endregion

    public class StateBuffer
    {
        public Hashtable      m_root_dic;         //ルート
        public Hashtable      m_front_dic;        //フロント
        public Hashtable      m_func_dic;         //ファンクション格納

        public PointervarItem m_pvitem;           //ポインタ変数のアイテム
        public object         m_cur;

        public BREAKTYPE      m_breakType;

#region api

        public const string KEY_PARENT   = "!PARENT!";
        public const string KEY_CHILD    = "!CHILD!";
        public const string KEY_FUNCTION = "!FUNCTION!";
        public const string KEY_FUNCMARK = "!FUNCWORK!"; //ファンクション実行時のワークエリア。

        public StateBuffer()
        {
            m_root_dic = new Hashtable();
            m_root_dic[KEY_PARENT] = null;
            m_front_dic = m_root_dic;

            m_func_dic = new Hashtable();
            m_root_dic[KEY_FUNCTION] =m_func_dic;

            m_cur = null;
            m_breakType = BREAKTYPE.NONE;
        }

        public void push_blk()
        {
            var newdic = new Hashtable();
            newdic[KEY_PARENT] = m_front_dic;
            m_front_dic[KEY_CHILD] = newdic;
            m_front_dic = newdic;            
        }
        public void pop_blk()
        {
            var p = m_front_dic[KEY_PARENT];
            if (p==null) util._error("block underflow");
            m_front_dic = (Hashtable)p;
            m_front_dic.Remove(KEY_CHILD);
        }
        public void set_funcwork()
        {
            push_blk();
            m_front_dic[KEY_FUNCMARK] = true;
        }
        public void reset_funcwork()
        {
            pop_blk();
        }
        public void add_func(string name,YVALUE v)
        {
            name = name.ToUpper();
            if (m_front_dic!=m_root_dic) util._error("root level can declear function");
            m_func_dic[name] = v;
        }
        public object get_func(string name)
        {
            name = name.ToUpper();
            return m_func_dic[name];
        }
        public object get(string name)
        {
            name = name.ToUpper();
            Func<Hashtable,object> _get = null;
            _get = (d)=> {
                if (d.ContainsKey(name)) return d[name];
                var p = d.ContainsKey(KEY_FUNCMARK) ?  m_root_dic : d[KEY_PARENT];
                if (p==null) util._error(name + " is not defined");
                return _get((Hashtable)p);
            };
            return _get(m_front_dic);
        }
        public object get(string name, object index)
        {
            if (index == null) util._error("null index is invalid");

            var v = get(name);
            if (v!=null)
            {
                var t = v.GetType();
                if (t==typeof(ARRAY) || t.IsArray)
                { 
                    var i = (int)((number)index);
                    if (t==typeof(ARRAY))
                    {
                        var l = (ARRAY)v;
                        if (i < 0 || i >= l.Count)  util._error( name + "["+index+"] is out of range");
                        return l[i];
                    }
                    else
                    {
                        var l = (Array)v;
                        if (i<0 || i >= l.Length)   util._error( name + "["+index+"] is out of range");
                        return l.GetValue(i);
                    }
                }
                if (t==typeof(Hashtable))
                {
                    var ht = (Hashtable)v;
                    if (ht.ContainsKey(index))
                    {
                        return ht[index];
                    }
                    return null;
                }
            }
            util._error(name +"[" + index + "] cannot be found");
            return null;
        }
        public bool exist(string name)
        {
            name = name.ToUpper();
            Func<Hashtable,bool> _find = null;
            _find = (d)=> {
                if (d.ContainsKey(name)) {
                    return true;   
                }
                else
                {
                    var p = d.ContainsKey(KEY_FUNCMARK) ?  m_root_dic : d[KEY_PARENT];
                    if (p==null) { return false; }
                    return _find((Hashtable)p);
                }
            };
            return _find(m_front_dic);
        }
        public void find_and_set(string name, object o)
        {
            name = name.ToUpper();
            Action<Hashtable> _findset = null;
            _findset = (d)=> {
                if (d.ContainsKey(name)) {
                    d[name] = o;
                }
                else
                {
                    var p = d.ContainsKey(KEY_FUNCMARK) ?  m_root_dic : d[KEY_PARENT];
                    if (p==null) util._error(name + " is not defined");
                    _findset((Hashtable)p);
                }
            };
            _findset(m_front_dic);
        }
        public void find_and_set_array(string name, object index, object o)
        {
            //if (index.GetType()!=typeof(number)) util._error("index is not number");
            if (index==null) util._error("NULL index is invalid");

            var v = get(name);
            if (v!=null)
            { 
                var vtype = v.GetType();
                if (vtype == typeof(ARRAY) || vtype.IsArray)
                {
                    var i = (int)((number)index);
                    if (vtype==typeof(ARRAY))
                    {
                        var l = (ARRAY)v;
                        if (i < 0 || i >= l.Count)  util._error( "index("+i+") is out of range");
                        l[i] = o;
                        return;
                    }
                    else
                    {
                        var l = (Array)v;
                        if (i<0||i>=l.Length)   util._error( "index("+i+") is out of range");
                        {//Change type
                            var et =l.GetType().GetElementType();
                            var ot =o.GetType();
                            if (et!=ot && !et.IsEnum && !ot.IsSubclassOf(et))
                            {
                                o = Convert.ChangeType(o,et);
                            }
                        }
                        l.SetValue(o,i);
                        return;
                    }
                }

                if (vtype == typeof(Hashtable))
                {
                    var ht = (Hashtable)v;
                    ht[index] = o;
                    return;
                }
            }
            util._error(name + "is not array");
        }
        public void define(string name, object o)
        {
            name = name.ToUpper();
            if (m_front_dic.ContainsKey(name)) util._error("Multiple defined");
            m_front_dic[name] = o;
        }
        public StateBuffer curnull()
        {
            m_cur = null;
            return this;
        }
        public StateBuffer breaknone()
        {
            m_breakType = BREAKTYPE.NONE;
            return this;
        }
        
        public bool get_bool_cur()
        {
            if (m_cur!=null)
            { 
                if (m_cur.GetType()==typeof(bool))
                {
                    return (bool)m_cur;
                }
                else if (m_cur.GetType()==typeof(number))
                {
                    return (number)m_cur != 0;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }
        public number get_number_cur()
        {
            if (m_cur!=null)
            {
                if (m_cur.GetType()==typeof(number))
                {
                    return (number)m_cur;
                }
            }
            return number.NaN;
        }
        public string get_string_cur()
        {
            if (m_cur==null) return null;
            return m_cur.ToString();
        }
        public void pvitemnull()
        {
            m_pvitem = null;
        }

#endregion
    }

    


    public class run_script
    {
        public static void Run(YVALUE v)
        {
            YDEF_DEBUG.bForceToStop = false;
            YDEF_DEBUG.stoppedLine = -1;
            var buf  = new StateBuffer();
            var nbuf = run(v,buf);
        }

        public static StateBuffer run(YVALUE v, StateBuffer sb)
        {
            YDEF_DEBUG.current_v = v;
            YDEF_DEBUG.current_sb = sb;

            if (YDEF_DEBUG.bRequestAbort)
            {
                YDEF_DEBUG.bRequestAbort = false;
                throw new SystemException("Abort!");
            }

            if (v.type == YDEF.get_type(YDEF.sx_sentence))
            { 
                var dbgline = v.get_dbg_line();
                if (
                        (
                            YDEF_DEBUG.breakpoints!=null
                            &&
                            YDEF_DEBUG.breakpoints.Contains(dbgline) 
                        )
                        ||
                        (
                           (
                            YDEF_DEBUG.stepMode == YDEF_DEBUG.STEPMODE.StepIn
                            ||
                              (
                               YDEF_DEBUG.stepMode== YDEF_DEBUG.STEPMODE.StepOver
                               &&
                               YDEF_DEBUG.funcCntSmp==0
                               )
                           )
                            &&
                           YDEF_DEBUG.stoppedLine != dbgline
                        )
                    )
                {
                    YDEF_DEBUG.stepMode = YDEF_DEBUG.STEPMODE.None;
                    YDEF_DEBUG.stoppedLine = dbgline;
                    YDEF_DEBUG.bForceToStop = true;
                    while(YDEF_DEBUG.bForceToStop)
                    {
                        System.Threading.Thread.Sleep(100);
                        if (YDEF_DEBUG.bRequestAbort)
                        {
                            YDEF_DEBUG.bRequestAbort = false;
                            throw new SystemException("Abort!");
                        }
                    }
                }
            }

            var nsb =sb;
            if (v.type == YDEF.get_type(YDEF.sx_main_block))
            {
                return run(v.list_at(1),nsb);
            }
            if (v.type == YDEF.get_type(YDEF.sx_sentence_list))
            {
                for(int i = 0; i<v.list.Count; i++)
                {
                    nsb = run(v.list_at(i),nsb.curnull());
                    if (nsb.m_breakType != BREAKTYPE.NONE)
                    {
                        break;
                    }
                }
                return nsb;
            }
            if (v.type == YDEF.get_type(YDEF.sx_sentence_block))
            {
                if (v.list.Count==3)
                {
                    nsb.push_blk();
                    nsb = run(v.list_at(1),nsb.curnull());
                    nsb.pop_blk();
                    return nsb;
                }
                if (v.list.Count==2)
                {
                    return nsb.curnull();
                }
                throw new SystemException("unexpected");
            }
            if (v.type == YDEF.get_type(YDEF.sx_sentence))
            {
                return run(v.list[0],nsb);
            }
            //--
            if (v.type == YDEF.get_type(YDEF.sx_expr_clause))
            {
                if (v.list.Count == 4)
                { 
                    var list_0_type = v.list_at(0).type;
                    var assingop    = v.list_at(1).GetString();
                    if (list_0_type == YDEF.NAME)
                    { 
                        var name = v.list_at(0).GetString();
                        nsb = run(v.list_at(2),nsb.curnull());
                        var val = nsb.get(name);
                        if (assingop.Length==2)
                        {
                            nsb.m_cur = util.Calc_op(val,nsb.m_cur,assingop.Substring(0,1));
                        }
                        nsb.find_and_set(name,nsb.m_cur);
                    }
                    else if (list_0_type == YDEF.get_type(YDEF.sx_array_var))
                    {
                        var array_var = v.list_at(0);
                        var name = array_var.list_at(0).GetString();
                        nsb = run(v.list_at(2),nsb.curnull());
                        var value = nsb.m_cur;
                        var array_index = array_var.list_at(1);
                        nsb = run(array_index.list_at(1),nsb.curnull());
                        if (nsb.m_cur==null || nsb.m_cur.GetType()!=typeof(number))
                        {
                            util._error("array_index is invalid." );
                        }
                        var index = (int)((number)nsb.m_cur);
                        nsb.m_cur = nsb.get(name);
                        if (nsb.m_cur!=null && nsb.m_cur.GetType()==typeof(List<object>))
                        {
                            var list = (List<object>)nsb.m_cur;
                            if (index >= 0 && index < list.Count)
                            {
                                list[index] = value;
                                nsb.find_and_set(name,list);             
                            }
                            else
                            {
                                util._error("out of index." );
                            }
                        }
                        else
                        {
                            util._error("unexpected");
                        }
                    }
                    else
                    {
                        util._error("unexpected. Check syntax.");
                    }
                }
                else
                {
                    nsb = run(v.list_at(0),nsb.curnull());
                }
                return nsb;
            }
            if (v.type == YDEF.get_type(YDEF.sx_def_var_clause))
            {
                var v0  = v.list_at(0);
                nsb = run(v0,nsb.curnull());
                var name = nsb.get_string_cur();

                var v2  = v.list_at(2);
                if (v2!=null)
                {
                    nsb  = run(v2,nsb.curnull());
                    nsb.find_and_set(name,nsb.m_cur);
                }
                return nsb;
            }
            if (v.type == YDEF.get_type(YDEF.sx_var_name))
            {
                var name = v.list_at(1).GetString();
                nsb.define(name,null);
                nsb.m_cur = name;
                return nsb;
            }
            if (v.type == YDEF.get_type(YDEF.sx_def_func_clause))
            {
                var name = v.list_at(1).list_at(0).GetString();
                nsb.add_func(name,v);
            }
            if (v.type == YDEF.get_type(YDEF.sx_if_clause))
            {
                nsb = run(v.list_at(1),nsb.curnull());
                bool b = nsb.get_bool_cur();
                if (b)
                {
                    nsb = run(v.list_at(2),nsb.curnull());
                    return nsb;
                }
                else 
                {
                    for(var i = 3; i<v.list.Count; i++)
                    {
                        var v2 = v.list_at(i);
                        if (v2.type == YDEF.get_type(YDEF.sx_elseif_clause))
                        {
                            nsb = run(v2.list_at(2),nsb.curnull());
                            b = nsb.get_bool_cur();
                            if (b)
                            {
                                nsb = run(v2.list_at(3),nsb.curnull());
                                return nsb;
                            }
                        }
                        if (v2.type == YDEF.get_type(YDEF.sx_else_clause))
                        {
                            if (i!=v.list.Count-1) util._error("unexpected. Check if clause syntax.");
                             nsb = run(v2.list_at(1),nsb.curnull());
                            return nsb;
                        }
                    }
                }
                return nsb;
            }   
            if (v.type == YDEF.get_type(YDEF.sx_else_clause))
            {
                nsb = run(v.list_at(1),nsb.curnull());
                return nsb;
            }
            if (v.type == YDEF.get_type(YDEF.sx_for_clause))
            {
                var bkt = v.list_at(1);
                var sts = v.list_at(2);

                nsb.push_blk();
                nsb = run(bkt.list_at(1),nsb.curnull());
                for(var loop = 0; loop<=CFG.LOOPMAX; loop++)
                {
                    if (loop == CFG.LOOPMAX) util._error("unexpected. it reached CFG.LOOPMAX:" + CFG.LOOPMAX);

                    nsb = run(bkt.list_at(2),nsb.curnull());
                    if (nsb.get_bool_cur())
                    {
                        nsb = run(sts,nsb.curnull());                                                
                        if (nsb.m_breakType == BREAKTYPE.BREAK)
                        {
                            nsb.breaknone();
                            break;
                        }
                        if (nsb.m_breakType == BREAKTYPE.RETURN)
                        {
                            break;
                        }
                        if (nsb.m_breakType == BREAKTYPE.CONTINUE)
                        {
                            nsb.breaknone();                            
                        }
                    }
                    else
                    {
                        break;
                    }
                    
                    nsb = run(bkt.list_at(3),nsb.curnull());
                }
                nsb.pop_blk();
                return nsb;
            }            
            if (v.type == YDEF.get_type(YDEF.sx_while_clause))
            {
                var expr = v.list_at(1);
                var sbk  = v.list_at(2);
                for(var loop = 0; loop<=CFG.LOOPMAX; loop++)
                {
                    if (loop == CFG.LOOPMAX) util._error("unexpected. it reached CFG.LOOPMAX:" + CFG.LOOPMAX);

                    nsb = run(expr,nsb.curnull());
                    if (nsb.get_bool_cur())
                    {
                        nsb = run(sbk,nsb.curnull());
                        if (nsb.m_breakType == BREAKTYPE.BREAK)
                        {
                            nsb.breaknone();
                            break;
                        }
                        if (nsb.m_breakType == BREAKTYPE.RETURN)
                        {
                            break;
                        }
                        if (nsb.m_breakType == BREAKTYPE.CONTINUE)
                        {
                            nsb.breaknone();
                            continue;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                return nsb;
            }
            if (v.type == YDEF.get_type(YDEF.sx_switch_clause))
            {
                var sx_expr_bracket   = v.list_at(1);
                var sx_sentence_block = util.check_switch_sentence_block(v.list_at(2));

                nsb = run(sx_expr_bracket,nsb.curnull());
                var x = nsb.m_cur;
                
                var sx_sentence_list = sx_sentence_block.FindValueByTravarse(YDEF.sx_sentence_list);
                bool b = false;
                for(int i = 0; i<sx_sentence_list.list.Count;i++)
                {
                    var vc = sx_sentence_list.list_at(i);
                    if (b)
                    {
                        nsb = run(vc,nsb.curnull());
                        if (nsb.m_breakType == BREAKTYPE.BREAK)
                        {
                            nsb.breaknone();
                            break;
                        }
                    }
                    else
                    { 
                        if (vc.IsType(YDEF.sx_case_clause))
                        {
                            var sx_case_clause = vc.FindValueByTravarse(YDEF.sx_case_clause);
                            nsb = run(sx_case_clause.list_at(1),nsb.curnull());
                            if (x!=null && nsb.m_cur!=null && x.Equals(nsb.m_cur))
                            {
                                b = true;
                            }
                        }
                        else if (vc.IsType(YDEF.sx_default_clause))
                        {
                            var sx_default_clause = vc.FindValueByTravarse(YDEF.sx_default_clause);
                            b = true;
                        }
                    }
                }
                return nsb;
            }
            if (v.type == YDEF.get_type(YDEF.sx_break_clause))
            {
                nsb.m_breakType = BREAKTYPE.BREAK;
                return nsb;
            }
            if (v.type == YDEF.get_type(YDEF.sx_continue_clause))
            {
                nsb.m_breakType = BREAKTYPE.CONTINUE;
                return nsb;
            }
            if (v.type == YDEF.get_type(YDEF.sx_return_clause))
            {
                if (v.list.Count == 3)
                {
                    nsb = run(v.list_at(1),nsb.curnull());
                    nsb.m_breakType = BREAKTYPE.RETURN;
                    return nsb;
                }
                nsb.m_breakType = BREAKTYPE.RETURN;
                return nsb;
            }
            if (v.type == YDEF.get_type(YDEF.sx_expr_clause))
            {
                if (v.list.Count == 2)
                {
                    nsb = run(v.list_at(0),nsb.curnull());      
                    return nsb;
                }
                else util._error("unexpected");
            }
            if (v.type == YDEF.get_type(YDEF.sx_assign_expr))
            {
                if (v.list_size()==3)
                {
                    var v0 = v.list_at(0);
                    var bArrayV0 = (v0.type == YDEF.get_type(YDEF.sx_array_var));
                    var bPvV0    = (v0.IsType(YDEF.get_type(YDEF.sx_pointervar_clause)));
                    var op = v.list_at(1).GetString();
                    var v2 = v.list_at(2);

                    nsb = run(v2,nsb.curnull());
                    var o = nsb.m_cur;

                    if (op.Length==2 && op[1]=='=') //assign operation += *= ...
                    {
                        nsb = run(v0,nsb.curnull());
                        var a = nsb.m_cur;
                        var ans = util.Calc_op(a,o,op[0].ToString());
                        o = ans;
                    }

                    if (bArrayV0)
                    {
                        var name  = v0.list_at(0).s;
                        nsb = run(v0.list_at(1),nsb.curnull());
                        var index = nsb.m_cur;
                        nsb.find_and_set_array(name,index,o);
                        return nsb;
                    }
                    if (bPvV0)
                    {
                        var v0p = v0.FindValueByTravarse(YDEF.get_type(YDEF.sx_pointervar_clause));
                        nsb = runsub_pointervar_clause.run(v0p,nsb,PointervarMode.SET);
                        if (nsb.m_cur!=null)
                        {
                            var item = (PointervarItem)nsb.m_cur;
                            if (item!=null&&item.setter!=null)
                            { 
                                var otype =o.GetType();
                                if (!otype.IsEnum &&item.setter_parametertype!=null && otype!=item.setter_parametertype)
                                {
                                    o = Convert.ChangeType(o,item.setter_parametertype);
                                }
                                item.setter(o);
                            }
                            return nsb;
                        }
                        util._error("unexpected");
                    }
                    else
                    {
                        var name = v0.s;
                        nsb.find_and_set(name,o);
                    }
                    return nsb;
                }
                else
                {
                    util._error("unexpected");
                }
            }
            if (v.type == YDEF.get_type(YDEF.sx_expr))
            {
                if (v.list.Count==1)
                {
                    nsb = run(v.list_at(0),nsb.curnull());
                    return nsb;
                }
                else if (v.list.Count==5) //3項演算子  p0 ? p2 : p4
                {
                    nsb = run(v.list_at(0),nsb.curnull());
                    if (nsb.get_bool_cur())
                    {
                        nsb = run(v.list_at(2),nsb.curnull());
                    }
                    else
                    {
                        nsb = run(v.list_at(4),nsb.curnull());
                    }
                    return nsb;
                }
                else if (v.list.Count==3)
                {
                    nsb = run(v.list_at(0),nsb.curnull());
                    var a = nsb.m_cur;
                    nsb = run(v.list_at(2),nsb.curnull());
                    var b = nsb.m_cur;

                    nsb.m_cur = util.Calc_op(a,b,v.list_at(1).GetString());

                    return nsb;
                }
                else if (v.list.Count==2)
                {
                    var str_1st = v.list[0].GetString();
                    var str_2nd = v.list[1].GetString();

                    var is_1st_incop = (v.list[0].type == YDEF.INCOP);
                    var is_2nd_icnop = (v.list[1].type == YDEF.INCOP);
                    var is_new_word  = (v.list[0].type == YDEF.NEW);

                    if (is_1st_incop)
                    {
                        var n = str_2nd;
                        var o = nsb.get(n);
                        if (o.GetType()==typeof(number))
                        {
                            var num = (number)o;
                            if (str_1st=="++") num++;
                            else if (str_1st=="--") num--;
                            else util._error("unexpected");

                            nsb.find_and_set(n,num);
                            nsb.m_cur = num;
                            return nsb;
                        }
                        else
                        {
                            util._error("unexpected");
                        }
                    }
                    else if (is_2nd_icnop)
                    {
                        var n = str_1st;
                        var o = nsb.get(n);
                        if (o.GetType()==typeof(number))
                        {
                            var num = (number)o;
                            nsb.m_cur = num;
                            if (str_2nd=="++") num++;
                            else if (str_2nd=="--") num--;
                            else util._error("unexpected");

                            nsb.find_and_set(n,num);
                            return nsb;
                        }
                        else
                        {
                            util._error("unexpected");
                        }
                    }
                    else if (is_new_word)
                    {
                        var nv = v.list[1].FindValueByTravarse(YDEF.get_type(YDEF.sx_pointervar_clause));
                        if (nv!=null)
                        {
                            nsb = runsub_pointervar_clause.run(nv,nsb.curnull(),PointervarMode.NEW);
                        }
                        return nsb;
                    }
                    else
                    { 
                        nsb = run(v.list[1],nsb.curnull());
                        if (str_1st == "+")
                        {
                            // nothing
                        }
                        else if (str_1st == "-")
                        {
                            if (nsb.m_cur.GetType()==typeof(number))
                            {
                                nsb.m_cur = (number)nsb.m_cur * (-1.0f);
                            }
                            else
                            {
                                util._error("unexpected");
                            }
                        }
                        else if (str_1st == "!")
                        {
                            if (nsb.m_cur.GetType()==typeof(bool))
                            {
                                nsb.m_cur = !((bool)nsb.m_cur);
                            }
                            else if (nsb.m_cur.GetType()==typeof(number))
                            {
                                var n = (number)nsb.m_cur;
                                nsb.m_cur = !(n!=0);
                            }
                            else
                            {
                                util._error("unexpected");
                            }
                        }
                        else
                        {
                            util._error("unexpected");
                        }
                        return nsb;
                    }
                }
                else
                {
                    util._error("unexpected");
                }
            }
            if (v.type == YDEF.get_type(YDEF.sx_param_list))
            {
                var l = new List<object>();
                for(int i =0; i<v.list.Count; i++)
                {
                    var v2 = v.list[i];
                    if (i%2==1)
                    { 
                        if (v2.GetString()==",")
                        {
                            continue;
                        }
                        else
                        {
                            util._error("unexpected");
                        }
                    }
                    nsb = run(v2,nsb.curnull());
                    l.Add(nsb.m_cur);
                }
                nsb.m_cur = l;
                return nsb;
            }
            if (v.type == YDEF.get_type(YDEF.sx_array_var))
            {
                var save_pvitem = nsb.m_pvitem;
                nsb.pvitemnull();

                var name = v.list_at(0).GetString();
                var array_index = v.list_at(1);
                nsb = run(array_index.list_at(1),nsb.curnull());
                if (nsb.m_cur==null || nsb.m_cur.GetType()!=typeof(number))
                {
                    util._error("array_index is invalid." );
                }
                var index_o = nsb.m_cur;
                var index = (int)((number)nsb.m_cur);

                if (save_pvitem!=null)
                {
                    nsb.m_pvitem = save_pvitem;
                    nsb = runsub_pointervar_clause.run_array_var(v,nsb,name,index_o);
                    return nsb;
                }

                nsb.m_cur = nsb.get(name,index_o);
                return nsb;
            }
            if (v.type == YDEF.get_type(YDEF.sx_array_index))
            {
                if (v.list_size()==3)
                {
                    nsb = run(v.list_at(1),nsb.curnull());
                }
                else if (v.list_size()==2)
                {
                    nsb.m_cur = new ARRAY();
                }
                else
                {
                    nsb.curnull();
                }
                return nsb;
            }

            if (v.type == YDEF.get_type(YDEF.sx_expr_bracket))
            {
                if (v.list.Count==3)
                {
                    var save = nsb.m_pvitem; //ポイント変数保存 , クリア
                    nsb.m_pvitem = null;

                    nsb = run(v.list_at(1),nsb.curnull());

                    nsb.m_pvitem = save;     //ポイント変数復帰
                }
                return nsb;
            }
            if (v.type == YDEF.get_type(YDEF.sx_func))
            {
                var save_pvitem = nsb.m_pvitem;
                nsb.pvitemnull();

                var name = v.list_at(0).GetString();

                var expr_bracket = v.list_at(1);

                int numofparam = 0;
                List<object> ol = new List<object>();
                if (expr_bracket.list_size()==2)
                {
                    numofparam = 0;
                }
                else if (expr_bracket.list_at(1).type == YDEF.get_type(YDEF.sx_expr))
                {
                    nsb = run(expr_bracket.list_at(1),nsb.curnull());
                    ol.Add(nsb.m_cur);
                    numofparam = 1;
                }
                else if (expr_bracket.list_at(1).type == YDEF.get_type(YDEF.sx_param_list))
                {
                    var param_list = expr_bracket.list_at(1);
                    for(int i = 0; i < param_list.list_size(); i+=2)
                    { 
                        nsb = run(param_list.list_at(i),nsb.curnull());
                        ol.Add(nsb.m_cur);
                    }
                    numofparam = param_list.list_size();
                }
                else
                {
                    util._error("unexpected");
                }

                if (save_pvitem!=null) //ポインタ変数
                {
                    nsb.m_pvitem = save_pvitem;
                    nsb = runsub_pointervar_clause.run_func(v,nsb,name,ol);
                    return nsb;
                }

                var fv = (YVALUE)nsb.get_func(name);
                if (fv==null)
                {
                    if (builtin.builtin_func.IsFunc(name))
                    {
                        nsb.m_cur = builtin.builtin_func.Run(name,ol.ToArray(),nsb.curnull());
                        return nsb;
                    }
                    util._error("function is not defined:" + name);
                }

                YDEF_DEBUG.funcCntSmp++;
                nsb.set_funcwork();
                {
                    var fvbk = util.normalize_func_bracket(fv.list_at(1).list_at(1)); //ファンクション定義部の引数部分
                    if (   fvbk.list.Count != numofparam)
                    {
                        util._error("number of arguments in valid.");
                    }
                    int n = 0;
                    if (fvbk!=null) for(int i = 0; i<fvbk.list.Count; i+=2)
                    {
                        var varname = fvbk.list_at(i).GetString();//定義側の変数名
                        object o = ol!=null && n < ol.Count ? ol[n] : null;
                        nsb.define(varname, o);
                        n++;
                    }
                    nsb = run(fv.list_at(2),nsb);
                    nsb.breaknone();
                }
                nsb.reset_funcwork();
                YDEF_DEBUG.funcCntSmp--;
                return nsb;
            }
            if (v.type == YDEF.get_type(YDEF.sx_def_func_clause))
            {
                var n = v.list_at(1).list_at(0).GetString();
                nsb.add_func(n,v);
                return nsb;
            }
            if (v.type == YDEF.get_type(YDEF.sx_pointervar_clause))
            {
                nsb = runsub_pointervar_clause.run(v,nsb);                
                return nsb;
            }
            if (v.type == YDEF.NAME)
            {
                if (nsb.m_pvitem!=null)
                {
                    nsb = runsub_pointervar_clause.run_name(v,nsb);
                    return nsb;
                }
                var n = v.GetString();
                nsb.m_cur = nsb.get(n);
                return nsb;            
            }
            if (v.type == YDEF.NUM)
            {
                if (nsb.m_pvitem!=null)
                {
                    nsb = runsub_pointervar_clause.run_num(v,nsb);
                    return nsb;
                }
                var n = v.GetNumber();
                nsb.m_cur = n;
                return nsb;            
            }
            if (v.type == YDEF.QSTR)
            {
                if (nsb.m_pvitem!=null)
                {
                    nsb = runsub_pointervar_clause.run_qstr(v,nsb);
                    return nsb;
                }
                var n = v.GetString();
                nsb.m_cur = util.DelDQ( n );
                return nsb;            
            }
            if (v.type == YDEF.BOOL)
            {
                var n = v.GetBool();
                nsb.m_cur = n;
                return nsb;            
            }
            if (v.type == YDEF.NULL)
            {
                nsb.m_cur = null;
                return nsb;
            }
            return nsb;            
        }
    }

    public class util
    {
        public static bool is_paramlist(YVALUE v)
        {
            if (v.type == YDEF.get_type(YDEF.sx_expr))
            {
                if (v.list.Count>=3)
                { 
                    for(int i = 1; i < v.list.Count; i+=2)
                    {
                        if (v.list_at(i).GetString()!=",") return false;            
                    }
                    return true;
                }
            }
            return false;
        }
        public static YVALUE normalize_func_bracket(YVALUE v)
        {
            if (v.type != YDEF.get_type(YDEF.sx_expr_bracket)) _error("unexpected");

            Func<YVALUE,YVALUE> comb = null;
            comb = (w) => {
                if (!is_paramlist(w))
                {
                    return w;                    
                }
                var x = comb(w.list_at(0));
                var c = w.list_at(1); //, comma
                var y = w.list_at(2); //
                if (is_paramlist(x))
                {
                    w.list.Clear();
                    w.list.Add(x.list_at(0));
                    w.list.Add(x.list_at(1));
                    w.list.Add(x.list_at(2));
                    w.list.Add(c);
                    w.list.Add(y);
                }
                return w;
            };
            
            if (v.list.Count==3)
            {
                var nv = comb(v.list_at(1));
                return nv;    
            }
            v.list.Clear();
            return v;        
        }

        public static YVALUE check_switch_sentence_block(YVALUE v)
        {
            if (v.type != YDEF.get_type(YDEF.sx_sentence_block)) throw new System.Exception("unexpected switch block #1");
            var inblock = v.list_at(1);
            if (inblock.type == YDEF.get_type(YDEF.sx_sentence))
            {
                check_case(inblock);
                return v;
            }
            if (inblock.type == YDEF.get_type(YDEF.sx_sentence_list))
            {
                var list = inblock.list_at(0);
                for(int i = 0; i<list.list.Count; i++)
                {
                    check_case(list.list_at(i));
                }
                return v;
            }
            _error("unexpected switch block #2");
            return null;
        }
        private static void check_case(YVALUE v)
        {
            if (v.IsType(YDEF.sx_case_clause))
            {
                var expr = v.list_at(1);                
                if (expr.IsType(YDEF.QSTR) || expr.IsType(YDEF.NUM))
                {
                    ;//ok
                }
                else
                {
                    _error("unexpected case sentence");
                }
            }
            else if (v.IsType(YDEF.sx_default_clause))
            {
                ; //ok
            }
            else if (v.IsType(YDEF.sx_sentence))
            {
                ;//ok
            }
            else
            {
                _error("unexpected switch senetence");
            }
        }
        public static string DelDQ(string i)
        {
            var s = i;
            if (string.IsNullOrEmpty(i)) return "";
            if (s.StartsWith("\"")) s=s.Substring(1);
            if (s.EndsWith("\""))   s=s.Substring(0,s.Length-1);
            return s;
        }
        public static object Calc_op(object a, object b, string op)
        {
            if (a==null || b==null)
            {
                switch(op)
                {
                    case "==": return (a==null && b==null);
                    case "!=": return (a!=b);
                }
                _error("unexpected null");
                return null;
            }
            if (a.GetType()==typeof(string))
            {
                var x = a.ToString();
                var y = b.ToString();
                switch(op)
                {
                    case "+":   return x + y;
                    case "==":  return (bool)(x==y);
                    case "!=":  return (bool)(x!=y);
                    default:    _error("unexpected string operaion");   break;
                }
            }
            else if (a.GetType()==typeof(number))
            {
                var x = (number)a;
                number y = 0;
                if (typeof(number)==typeof(float))
                { 
                    y = (number)Convert.ToSingle(b);
                }
                else if (typeof(number)==typeof(double))
                {
                    y = (number)Convert.ToDouble(b);
                }
                else
                {
                    _error("unexpected");
                }
                
                switch(op)
                {
                    case "+":   return x+y;
                    case "-":   return x-y;
                    case "*":   return x*y;
                    case "/":   return x/y;
                    case "%":   return x%y;
                    case "==":  return (bool)(x==y);
                    case "!=":  return (bool)(x!=y);
                    case ">":   return (bool)(x>y);
                    case ">=":  return (bool)(x>=y);
                    case "<":   return (bool)(x<y);
                    case "<=":  return (bool)(x<=y);
                    default:    _error("unexpected number operaion:" + op);   break;                 
                }
            }
            else if (a.GetType()==typeof(bool))
            {
                var x = (bool)a;
                var y = (bool)b;

                switch(op)
                {
                    case "==":  return (bool)(x==y);
                    case "!=":  return (bool)(x!=y);
                    case "||":  return (bool)(x||y);
                    case "&&":  return (bool)(x&&y);
                    default:    _error("unexpected bool operaion:" + op);   break;                
                }
            }
            else if (a.GetType()==typeof(ARRAY))
            {
                if (b.GetType()==typeof(ARRAY))
                {
                    var ary_a = (ARRAY)a;
                    var ary_b = (ARRAY)b;
                    switch(op)
                    {
                        case "+": ary_a.AddRange(ary_b); return ary_a;
                        case "==": return ary_a.SequenceEqual(ary_b);
                        default:    _error("unexpected bool operaion:" + op);   break;                
                    }
                }
                else
                {
                    var ary_a = (ARRAY)a;
                    switch(op)
                    {
                        case "+": ary_a.Add(b); return ary_a;
                        default:    _error("unexpected bool operaion:" + op);   break;                
                    }
                }
            }
            _error("unexpected calc op:"+op);  
            return null;                 
        }

        // Error
        public static void _error(string cmt)
        {
            var s = cmt + "\n" + YDEF_DEBUG.RuntimeErrorInfo();
            throw new SystemException(s);
        }
    }
}
