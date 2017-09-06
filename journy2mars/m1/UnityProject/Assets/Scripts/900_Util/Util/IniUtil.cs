using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// https://ja.wikipedia.org/wiki/INI%E3%83%95%E3%82%A1%E3%82%A4%E3%83%AB
/// </summary>
public class IniUtil {

    #region basic
    public static string GetValue(string key,string initext)
    {
        var ht = CreateHashtable(initext);
        return GetValueFromHashtable(key,ht);
    }
    public static string GetValue(string category, string key, string initext)
    {
        var ht = CreateHashtable(initext);
        return GetValueFromHashtable(category,key,ht);
    }
    public static string GetValueFromResourceFile(string key, string path)
    {
        var textasset = Resources.Load<TextAsset>(path);
        if (textasset!=null)
        {
            return GetValue(key, textasset.text);
        }
        return null;
    }
    public static string GetValueFromResourceFile(string category, string key, string path)
    {
        var textasset = Resources.Load<TextAsset>(path);
        if (textasset!=null)
        {
            return GetValue(category,key,textasset.text);
        }
        return null;
    }
    public static string GetValueFromHashtable(string key, Hashtable ht)
    {
        if (ht.Contains(key))
        {
            return ht[key].ToString();
        }
        return null;
    }
    public static string GetValueFromHashtable(string category, string key, Hashtable ht)
    {
        if (ht!=null && ht.ContainsKey(category))
        {
            var cateval = ht[category];
            if (cateval!=null && ( cateval is Hashtable))
            {
                var cathash = (Hashtable)cateval;
                if (cathash.ContainsKey(key))
                {
                    return cathash[key].ToString();
                }
            }
        }
        return null;        
    }
    public static Hashtable LoadHashTabeFromResourceFile(string path)
    {
        var textasset = Resources.Load<TextAsset>(path);
        if (textasset!=null)
        {
            return CreateHashtable(textasset.text);
        }
        return null;
    }
    public static Hashtable CreateHashtable(string initext)
    {
        if (string.IsNullOrEmpty(initext)) return null;

        Hashtable mainhash = new Hashtable();
        Hashtable cathash  = null;

        var lines = initext.Split('\n');
        for(var n=0; n<lines.Length;n++)
        {
            var l = lines[n];
            if (string.IsNullOrEmpty(l) || string.IsNullOrEmpty(l.Trim()))
            {
                continue;
            }
            if (l[0]==';') continue;
            // check category
            if (l[0]=='[')
            {
                if (l.Length<=2)
                {
                    Debug.LogWarning("Unknown category at line : " + n );
                    continue;
                }                
                var cindex = l.IndexOf(']',1);
                if (cindex < 0)
                {
                    Debug.LogWarning("category does have a close bracket at line : " + n );
                    continue;
                }
                var category = l.Substring(1,cindex - 1);
                
                cathash = new Hashtable();
                mainhash.Add(category,cathash);
            }


            // check key = value
            var eqindex = l.IndexOf('=');
            if (eqindex<0)
            {
                Debug.LogWarning("Unknown line : " + n);
                continue;
            }
            var key = l.Substring(0,eqindex).Trim();
            if (string.IsNullOrEmpty(key))
            {
                Debug.LogWarning("Unkown key at line : " + n);
                continue;
            }
            
            if (eqindex + 2 > l.Length)
            {
                Debug.LogWarning("No value at line" + n);
                continue;
            } 
            var value = l.Substring(eqindex+1).Trim();
            if (string.IsNullOrEmpty(value))
            {
                Debug.LogWarning("Unknown value at line" + n);
                continue;
            }

            if (cathash!=null)
            {
                cathash.Add(key,value);
            }
            else
            {
                mainhash.Add(key,value);
            }
        }
        return mainhash;
    }
    #endregion

    public static T GetParsedValueFromHashtable<T>(string key, Hashtable ht)
    {
        var value = GetValueFromHashtable(key,ht);
        if (value==null) return default(T);

        var type = typeof(T);
        if (type == typeof(int))
        {
            int x=0;
            if (int.TryParse(value, out x))
            {
                return (T)((object)x);
            } 
            return default(T);            
        }
        if (type == typeof(float))
        {
            float x=0;
            if (float.TryParse(value, out x))
            {
                return (T)((object)x);
            } 
            return default(T);            
        }
        if (type == typeof(Vector2))
        {
            var v2 = ParseUtil.Vector2Parse(value);
            return (T)((object)v2);
        }
        if (type == typeof(List<Vector2>))
        {
            var list = ParseUtil.Vector2ListParse(value);
            return (T)((object)list);
        }

        return default(T);
    }
    

}
