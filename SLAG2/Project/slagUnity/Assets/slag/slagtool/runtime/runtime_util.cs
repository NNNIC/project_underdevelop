using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using ARRAY = System.Collections.Generic.List<object>;
using number = System.Double;

namespace slagtool.runtime
{
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
        public static void Assert(bool condition)
        {
#if UNITY_5
            UnityEngine.Assertions.Assert.IsTrue(condition);
#else
            System.Diagnostics.Debug.Assert(condition);
#endif
        }
    }
}
