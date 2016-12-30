using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using LIST = System.Collections.Generic.List<object>;
using number = System.Double;

namespace slagtool.runtime
{
    public class util
    {
        internal static YVALUE GetOptimize(YVALUE v)
        {
            YVALUE findV = v;

            Action<YVALUE> trv = null;
            trv = (w)=> {
                if (w.list_size()==1)
                {
                    findV = w;
                    trv(w);
                }
                else
                {
                    return;
                }
            };

            return findV;
        }

        internal static bool is_paramlist(YVALUE v)
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
        internal static YVALUE normalize_func_bracket(YVALUE v)
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

        internal static YVALUE check_switch_sentence_block(YVALUE v)
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
        internal static string DelDQ(string i)
        {
            var s = i;
            if (string.IsNullOrEmpty(i)) return "";
            if (s.StartsWith("\"")) s=s.Substring(1);
            if (s.EndsWith("\""))   s=s.Substring(0,s.Length-1);
            return s;
        }

        internal static Func<object, object, string, object> User_Calc_op = null; //ユーザ用
        internal static object Calc_op(object a, object b, string op)
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
            else if (util.IsNumeric(a.GetType()))  // if (a.GetType()==typeof(number))
            {
                var x = util.ToNumber(a);
                var y = util.ToNumber(b);

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
            else if (a.GetType()==typeof(LIST))
            {
                if (b.GetType()==typeof(LIST))
                {
                    var ary_a = (LIST)a;
                    var ary_b = (LIST)b;
                    switch(op)
                    {
                        case "+": ary_a.AddRange(ary_b); return ary_a;
                        case "==": return ary_a.SequenceEqual(ary_b);
                        default:    _error("unexpected bool operaion:" + op);   break;                
                    }
                }
                else
                {
                    var ary_a = (LIST)a;
                    switch(op)
                    {
                        case "+": ary_a.Add(b); return ary_a;
                        default:    _error("unexpected bool operaion:" + op);   break;                
                    }
                }
            }

            if (User_Calc_op!=null)
            {
                return User_Calc_op(a,b,op);
            }

            _error("unexpected calc op:"+op);  
            return null;                 
        }

        internal static bool IsNumeric(Type type)
        {
            return (
                type == typeof(System.Byte)    ||   type == typeof(System.SByte)
                ||
                type == typeof(System.Int16)   ||   type == typeof(System.UInt16)
                ||
                type == typeof(System.Int32)   ||   type == typeof(System.UInt32)
                ||
                type == typeof(System.Int64)   ||   type == typeof(System.UInt64)
                ||
                type == typeof(System.Single)  ||   type == typeof(System.Double)
                );
        }
        internal static number ToNumber(object o, bool ErrorAsNan=false)
        {
            if (o==null)
            {
                if (ErrorAsNan) return number.NaN;
                util._error("number is null");
            }
            var ot =o.GetType();
            if (!IsNumeric(ot))
            {
                if (ErrorAsNan) return number.NaN;
                util._error("it is not numeric");
            }
            if (ot != typeof(number))
            {
                return (number)Convert.ChangeType(o,typeof(number));
            }
            return (number)o;
        }

        // Error
        internal static void _error(string cmt)
        {
            var s = cmt + "\n" + YDEF_DEBUG.RuntimeErrorInfo();
            throw new SystemException(s);
        }
        internal static void Assert(bool condition)
        {
#if UNITY_5
            UnityEngine.Assertions.Assert.IsTrue(condition);
#else
            System.Diagnostics.Debug.Assert(condition);
#endif
        }
    }
}
