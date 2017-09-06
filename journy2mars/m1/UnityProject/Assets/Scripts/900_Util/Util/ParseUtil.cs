using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ParseUtil  {

    public static Vector2 Vector2Parse(string s)
    {
        if (string.IsNullOrEmpty(s)) return new Vector2(float.NaN,float.NaN);
        var v = s.Replace("(","").Replace(")","");
        var list = FloatListParse(v);
        if (list == null || list.Count!=2)
        {
            throw new System.Exception("Unknown");
        }
        return new Vector2(list[0],list[1]);
    }
    public static Vector3 Vector3Parse(string s)
    {
        if (string.IsNullOrEmpty(s)) return new Vector3(float.NaN,float.NaN,float.NaN);
        var v = s.Replace("(","").Replace(")","");
        var list = FloatListParse(v);
        if (list == null || list.Count!=3)
        {
            throw new System.Exception("Unknown");
        }
        return new Vector3(list[0],list[1],list[2]);
    }
    
    public static List<Vector2> Vector2ListParse(string s)
    {
        var list = new List<Vector2>();
        var tokens = s.Split(')');

        foreach(var i in tokens)
        {
            if (string.IsNullOrEmpty(i) || string.IsNullOrEmpty(i.Trim()))
            {
                throw new System.Exception();
            }
            var ni = i.TrimStart(',').TrimStart('(').Trim();

            if (string.IsNullOrEmpty(ni)) break;

            var v = Vector2Parse(ni);
            if (!float.IsNaN(v.x))
            {
                list.Add(v);
            }
        } 

        return list;
    }
    public static List<Vector3> Vector3ListParse(string s)
    {
        var list = new List<Vector3>();
        var tokens = s.Split(')');

        foreach(var i in tokens)
        {
            if (string.IsNullOrEmpty(i) || string.IsNullOrEmpty(i.Trim()))
            {
                throw new System.Exception();
            }
            var ni = i.TrimStart(',').TrimStart('(').Trim();

            if (string.IsNullOrEmpty(ni)) break;

            var v = Vector3Parse(ni);

            list.Add(v);            
        } 

        return list;
    }
    public static List<float> FloatListParse(string s)
    {
        var list = new List<float>();
        var tokens = s.Split(',');
        foreach(var i in tokens)
        {
            var ns = i.Trim();
            if (ns==null)
            {
                Debug.LogWarning("floatListParse faild #1 :" + s);
                continue;
            }
            float f = 0;
            if (float.TryParse(ns,out f))
            {
                list.Add(f);
            }
            else
            {
                Debug.LogWarning("floatListParse faild #2 :" + s);
                continue;
            }
        }
        return list;
    }

    public static List<string> Split_obs(string s)
    {
        if (string.IsNullOrEmpty(s)) return null;
        var list = new List<string>();
        var w = string.Empty;

        var bBracket = false;
        for(var i = 0; i<s.Length; i++)
        {
            var c = s[i];

            if (bBracket)
            {
                if (c==')')
                {
                    list.Add(w);
                    w = string.Empty;
                    bBracket = false;

                    if (i+1<s.Length && s[i+1]==',')
                    {
                        i++;
                    }
                    continue;
                }
                w+=c;
            }
            else
            {
                if (c==',')
                {
                    list.Add(w);
                    w = string.Empty;
                    continue;
                }
                if (w==string.Empty && c=='(')
                {
                    bBracket = true;
                    continue;
                }

                w += c;
            }
        }
        list.Add(w);
        
        return list;
    }

    public static List<string> Split(string val)
    {
        if (string.IsNullOrEmpty(val)) return null;

        var list = new List<string>();

        char[] pair_start = new char[] { '(','[','{' };
        char[] pair_end   = new char[] { ')',']','}' };

        var stack = new List<char>();
        Func<char> expect_endchar = ()=> stack.Count>0 ? stack[0] : (char)0;

        var word = string.Empty;
        for(var i = 0; i<val.Length; i++)
        {
            var c = val[i];
            if (expect_endchar()==0 && (c==','))
            {
                list.Add(word);
                word = string.Empty;
                continue;
            }
            var pair_start_index = Array.FindIndex(pair_start,v=>v==c);
            if (pair_start_index>=0)
            {
                var endchar = pair_end[pair_start_index];
                stack.Insert(0,endchar);
            }
            else if (c==expect_endchar())
            {
                stack.RemoveAt(0);
            }

            word += c;
        }
        if (!string.IsNullOrEmpty(word)) list.Add(word);

        return list;
    }

    public static string Extact(string s, char start, char end)
    {
        var si = s.IndexOf(start);
        if (si<0) return null;
        var ei = s.IndexOf(end,si);
        if (ei<0) return null;
        if (ei == si+1) return null;

        return s.Substring(si+1,ei-(si+1));
    }
}
