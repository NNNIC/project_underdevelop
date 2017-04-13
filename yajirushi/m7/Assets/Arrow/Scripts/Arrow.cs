﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ArrowTool;

// 矢印作成
// 取り扱いを簡単にするため１ファイルに集約

[ExecuteInEditMode]
public class Arrow : MonoBehaviour
{
    public enum TYPE
    {
        NONE,

        ONEWAY,

        TURN_L,
        TURN_R,

        U_TURN_R,
        U_TURN_L,

        S_TURN_R,
        S_TURN_L,

        SS_TURN_R,    //Super S -- 大きく曲がったＳ字
        SS_TURN_L,

        HEAD_C_R,     //先頭がＣの字
        HEAD_C_L,

        TAIL_C_R,     //末尾がＣの字
        TAIL_C_L,
    }
    
    [Serializable]
    public class control
    {
        public  Arrow      m_owner; 
        private GameObject m_go    { get { return m_owner.m_go; } }
        private Transform  m_space { get { return m_owner.m_go.transform; } }
        private TYPE       m_type  { get { return m_owner.m_type; } set { m_owner.m_type = value; } }

        public Transform[] m_hands;

        public Vector3[]   m_space_hands;

        private Vector3[]  m_handsaves;

        private bool is_handsEqaulAndUpdate()
        {
            if (m_hands==null)
            {
                return true;
            }
            if (m_handsaves==null)
            {
                if (m_space_hands!=null)
                {
                    m_handsaves = new Vector3[m_space_hands.Length];
                    Array.Copy(m_space_hands,m_handsaves,m_space_hands.Length);
                    //return false;
                }
                return true;
            }
            // ----

            bool bEqual = false;

            if (m_space_hands!=null && m_handsaves!=null)
            {
                bEqual = true;
                for(var i = 0; i<m_space_hands.Length; i++)
                {
                    if ( !Util.IsEqualVector3( m_space_hands[i], m_handsaves[i] ) )
                    {
                        bEqual = false;
                        break;
                    }
                }
            }
            if (!bEqual)
            {
                Array.Copy(m_space_hands,m_handsaves,m_space_hands.Length);
            }
            return bEqual;
        }

        private void restore_hands()
        {
            if (m_hands!=null && m_handsaves!=null)
            {
                for(var i = 0; i<m_hands.Length; i++)
                {
                    m_hands[i].transform.position = m_space.TransformPoint(m_handsaves[i]);
                }
            }
        }

        public void Move()
        {
            Func<string, Vector3> spaceNodePos = (n)=> {
                var target_tr = Util.FindNode(m_go,n);
                return m_space.InverseTransformPoint(target_tr.position);
            };
            Func<Vector3, Vector3> spacePos = (v) =>
            {
                return m_space.InverseTransformPoint(v);
            };

            m_space_hands = null;
            if (m_hands!=null)
            {
                m_space_hands = new Vector3[m_hands.Length];
                for(var i = 0; i<m_hands.Length; i++)
                {
                    m_space_hands[i] = spacePos(m_hands[i].position);
                }
            }

            if ( is_handsEqaulAndUpdate())
            {
                return;
            }
            if (m_space_hands!=null) {
                var     unit_len   = m_owner.m_unit_len;
                
                Action<string, float> SetZ = (n,z) => {
                    var nz = Mathf.Clamp(z,0,float.MaxValue);
                    var tr = Util.FindNode(m_go,n);
                    tr.localPosition = Util.Vector3_ModZ(tr.localPosition,nz);
                };
                Action<string, float,string,float> SetZZ = (n1,z1,n2,z2) => {
                    var nz1 = Mathf.Clamp(z1,0,float.MaxValue);
                    var nz2 = Mathf.Clamp(z2,0,float.MaxValue);
                    var tr1 = Util.FindNode(m_go,n1);
                    var tr2 = Util.FindNode(m_go,n2);

                    tr1.localPosition = Util.Vector3_ModZ(tr1.localPosition,nz1);
                    tr2.localPosition = Util.Vector3_ModZ(tr2.localPosition,nz1);
                };
            
                var ignore = new List<int>(); //リストア時に無視させるmidのインデックス

                if (m_type== TYPE.ONEWAY)
                {
                    //Ｚ値のみ反映
                    SetZ("arrow",m_space_hands[0].z - unit_len);
                }
                else if (m_type== TYPE.TURN_R || m_type == TYPE.TURN_L)
                {
                    //x値が"arrow"のzに影響 
                    SetZ("arrow",Mathf.Abs(m_space_hands[0].x) - unit_len * 2);
                    //z値が"curve1_curve90"に影響
                    SetZ("curve1_curve90",m_space_hands[0].z - unit_len);
                }
                else if (m_type== TYPE.U_TURN_R || m_type == TYPE.U_TURN_L || m_type == TYPE.S_TURN_R || m_type == TYPE.S_TURN_L )
                {
                    //headのz値  "arrow"の位置に影響
                    SetZ("arrow",Mathf.Abs(m_space_hands[0].z - spaceNodePos("shaft3_shaft").z) - unit_len);
                    //headのx値 "curve2_curve90"の位置に影響
                    SetZ("curve2_curve90",Mathf.Abs(m_space_hands[0].x - spaceNodePos("shaft2_shaft").x) - unit_len);
                    //mid0のz値 "arrow"と"curve1_curve90"位置に影響
                    SetZ("arrow",Mathf.Abs(m_space_hands[0].z - spaceNodePos("arrow").z) -  unit_len);
                    //
                    SetZ("curve1_curve90",Mathf.Abs(m_space_hands[1].z) - unit_len);
                }
                else if (m_type == TYPE.SS_TURN_R || m_type == TYPE.SS_TURN_L )
                {
                    //mid0 z方向のみ影響 curve1_curve90のz位置 curve3_curve90のz位置
                    SetZZ("curve1_curve90", m_space_hands[1].z - unit_len,"curve3_curve90", Mathf.Abs(m_space_hands[1].z -spaceNodePos("curve3_curve90").z) - unit_len);
                }
            }
            //絶対値で修正
            restore_hands();
        }
    }

    public float m_shuft_width   = 0.4f;
    public float m_unit_len      = 1.0f;
    public int   m_curve_divnum  = 10;
    public float m_arrow_width   = 1.3f;

    private TYPE __type;
    public  TYPE  m_type;

    private GameObject m_go;

    private control m_control;

    public Transform[] m_hands { get { if (m_control!=null) { return m_control.m_hands; } return null; }}

    public void Update()
    {
        if (__type != m_type)
        {
            m_go = null;
            m_control = null;
            if (transform.childCount>0)
            {
                GameObject.DestroyImmediate(transform.GetChild(0).gameObject);
            }
            if (transform.childCount==0)
            {
                __type = m_type;
            }
            else
            { 
                return;
            }
        }

        if (m_type!= TYPE.NONE && m_go==null)
        {
            m_go = ArrowMaker.CreateArrowSub(__type,m_shuft_width,m_unit_len,m_curve_divnum,m_arrow_width);
            m_go.transform.parent = transform;
            transform.localEulerAngles      = Vector3.zero;
            m_go.transform.localEulerAngles = Vector3.zero;
            m_go.transform.localScale       = Vector3.one;
            m_go.transform.localPosition    = Vector3.zero;

            m_control = new control();
            m_control.m_owner = this;

            //return; //test

            var headbone = Util.FindNode(m_go,"head");

            var midbone_0 = Util.FindNode(m_go,"mid");
            var midbone_1 = Util.FindNode(m_go,"mid1");
            var midbone_2 = Util.FindNode(m_go,"mid2");

            if (midbone_2!=null)
            {
                m_control.m_hands = new Transform[4];
                m_control.m_hands[0] = headbone;
                m_control.m_hands[1] = midbone_0;
                m_control.m_hands[2] = midbone_1;
                m_control.m_hands[3] = midbone_2;
            }
            else if (midbone_1!=null)
            {
                m_control.m_hands = new Transform[3];
                m_control.m_hands[0] = headbone;
                m_control.m_hands[1] = midbone_0;
                m_control.m_hands[2] = midbone_1;
            }
            else if (midbone_0!=null)
            {
                m_control.m_hands = new Transform[2];
                m_control.m_hands[0] = headbone;
                m_control.m_hands[1] = midbone_0;
            }
            else
            {
                m_control.m_hands = new Transform[1];
                m_control.m_hands[0] = headbone;
            }
        }
        
        if (m_control!=null)
        {
            m_control.Move();
        }
    }
}

// ------------------------------------------------ 以下、矢印作成 --------------------------------------------

namespace ArrowTool
{
    public class Util
    {
        public static bool IsEqualVector3(Vector3 v1, Vector3 v2)
        {
            float epsiron = 0.0010f;  //floatの有効桁は6桁
            var dx = Mathf.Abs(v1.x - v2.x);
            var dy = Mathf.Abs(v1.y - v2.y);
            var dz = Mathf.Abs(v1.z - v2.z);

            return (dx<epsiron && dy<epsiron && dz<epsiron);
        }

        public static Transform FindNode(GameObject go, string name)
        {
            Transform found = null;
            Action<Transform> _traverse = null;
            _traverse = (t)=> {
                if (found==null)
                {
                    if (t.name==name)
                    {
                        found = t;
                        return;
                    }

                    for(int i = 0; i<t.childCount; i++)
                    {
                        _traverse(t.GetChild(i));
                    }
                }
            };

            _traverse(go.transform);

            return found;
        }

        public static Vector3 Vector3_ModZ(Vector3 v, float z)
        {
            return new Vector3(v.x,v.y,z);
        }
    }
}


public class ArrowMaker {
    //
    // 方針：頂点などを記号化した状態のままのデータ構造として、扱いやすい形で
    //       
    //       最終的にスキンメッシュを作成する。
    //

#region 格納クラス
    //
    public class vertex
    {
#region 名前
        private List<string> names = new List<string>();
        public string name {
            get {  return names.Count>0 ? names[names.Count-1] : null; } // 最後に登録したものを返す
            set {  if (!names.Contains(value)) { names.Add(value); }   } // ユニークのみ
        }
        public bool   name_contains(string s) { return names.Contains(s);   }
        public bool   names_have_startsWith(string s)
        {
            foreach(var n in names)
            {
                if (n.StartsWith(s)) return true;
            }
            return false;
        }
#endregion

        public Vector3    v;       //座標
        public string     bone0;   //所属ボーン0
        public string     bone1;   //所属ボーン1

        public float      weight0; //ウエイト0
        public float      weight1; //ウエイト1

        public int        tmp_index; //メッシュ作成時に使用の一時的インデックス

        public vertex(string i_name, Vector3 vec3)
        {
            _init(i_name,vec3);
        }
        public vertex(string i_name,  float x, float y, float z)
        {
            _init(i_name, new Vector3(x,y,z));
        }
        private void _init(string i_name, Vector3 vec3)
        {
            name = i_name;

            v = vec3;

            bone0 = null; //未定義
            bone1 = null; //未定義
            weight0 = 1;
            weight1 = 0;
        }

    }

    public class poligon
    {
        public List<vertex> vertex_list   = new List<vertex>();    //頂点リスト

        public struct trindex {public Vector3 idx0, idx1, idx2; }                            //３角形情報を頂点で管理（※インデックスではない）
        public List<trindex>  tri_index_list= new List<trindex>();                           //トライアングルインデックス

#region 利用時に便宜
        private string  _tmpsaved_poligonname;                                               //利用時に便宜
        public void     Begin(string poligonname)
        {
            _tmpsaved_poligonname = poligonname;
        }
        public void     End()
        {
            _tmpsaved_poligonname = null;
        }
#endregion

        private string _makename(string vertexname) { return _tmpsaved_poligonname + ">" + vertexname;  }
        private string _makename(string poligonname,string vertexname) { return poligonname + ">" + vertexname;  }

        public void AddVertex(string vertexname, float x, float y, float z) {
            AddVertex(vertexname, new Vector3(x,y,z));
        }
        public void AddVertex(string vertexname, Vector3 vec3)
        {
            if (_tmpsaved_poligonname==null) throw new SystemException();
            bool bFound = false;
            for(var i =0; i<vertex_list.Count; i++)
            {
                var v = vertex_list[i];
                var b = Util.IsEqualVector3(v.v, vec3);
                if (b)
                {
                    v.name = _makename(vertexname); //名前を変える　（内部にてhistory保存）
                    bFound = true;
                    break;
                }
            }
            if (!bFound)
            {
                vertex_list.Add(new vertex(_makename(vertexname), vec3));
            }
        }

        public void AddTriIndex(string n1, string n2, string n3)
        {
            tri_index_list.Add(new trindex() { idx0=Pos(n1), idx1=Pos(n2), idx2=Pos(n3) });
        }

        //検索
        public vertex FindVertex(string vertexname)
        {
            var key = _makename(vertexname);
            foreach(var v in vertex_list)
            {
                if (v.name_contains(key))
                {
                    return v;
                }
            }
            return null;
        }
        public vertex FindVertex(Vector3 pos)
        {
            foreach(var v in vertex_list)
            {
                if (Util.IsEqualVector3 (v.v, pos)) return v;
            }
            return null;
        }
        private Vector3 Pos(string vertexname)
        {
            var v = FindVertex(vertexname);
            if (v==null)
            {
                throw new SystemException();
            }
            return v.v;
        }
        public List<vertex> CollectVertex(string poligonname,bool bLatestOnly=true)
        {
            var list = new List<vertex>();
            var nullkeyname = _makename(poligonname,"");
            foreach(var v in vertex_list)
            {
                bool b=false;
                if (bLatestOnly)
                {
                    b = v.name.StartsWith(nullkeyname);
                }
                else
                {
                    b = v.names_have_startsWith(nullkeyname);
                }
                if (b)
                {
                    list.Add(v);
                }
            }
            return list;
        }


        //メッシュ作成用
        public void MESH_create(
                                Skin skin,
                                ref List<Vector3>    mesh_vertex_list, 
                                ref List<int>        mesh_triangle_list, 
                                ref List<Vector2>    mesh_uvlist,
                                ref List<BoneWeight> mesh_weight_list
        )
        {
            var index = mesh_vertex_list.Count;
            var tmp_list = new List<vertex>();
            foreach(var vtx in vertex_list)
            {
                vtx.tmp_index = index++;
                tmp_list.Add(vtx); 
                mesh_vertex_list.Add(vtx.v); //頂点
                mesh_uvlist.Add(Vector2.zero);   //UV暫定

                //BoneWeight
                BoneWeight bw = new BoneWeight();
                bw.boneIndex0 = skin.FindBoneIndex(vtx.bone0);
                if (vtx.bone1!=null)
                {
                    bw.boneIndex1 = skin.FindBoneIndex(vtx.bone1);
                }
                bw.weight0 = vtx.weight0;
                bw.weight1 = vtx.weight1;

                mesh_weight_list.Add(bw);
            }

            Func<Vector3,int> get_index = (v)=> {
                var vrtx = FindVertex(v);
                return vrtx.tmp_index;
            };
            foreach(var t in tri_index_list)
            {
                mesh_triangle_list.Add(get_index(t.idx0)); // 三角形インデックス
                mesh_triangle_list.Add(get_index(t.idx1)); //
                mesh_triangle_list.Add(get_index(t.idx2)); //
            }
        }
    }

    public class bone
    {
        public string     name;

        public string     parent;                    //親ボーン名

        public Vector3    org;                       //起点
        public float      angle_y;                   //方向

        public Transform  tmp_tr;                    //メッシュ作成時に利用

        public bone(string i_name, string i_panrent, Vector3 i_org, float i_angle_y)
        {
            name     = i_name;
            parent   = i_panrent;
            org      = i_org;
            angle_y  = i_angle_y;
        }
    }

    public class Skin
    {
        public poligon       pol       = new poligon();
        public List<bone>    bone_list = new List<bone>();

        public float   shaft_width;   
        public float   next_angle_y;  //次のY軸角
        public Vector3 next_org;      //次の起点

        public void AddBone(string name, Vector3 org, float angle_y)
        {
            string parent = null; //本パターンでは必ず１つ前が親
            if (bone_list.Count>0)
            {
                parent = bone_list[bone_list.Count-1].name;
            }
            var bone = new bone(name,parent,org,angle_y);
            bone_list.Add(bone);
        }

        public void InsertMidBone(string parent, string index=null) //ハンドルとなる中間点
        {
            var parent_bone = bone_list.Find(i=>i.name==parent);

            var bone = new bone("mid" + index,parent,parent_bone.org, parent_bone.angle_y);
            bone_list.Add(bone);
        }

        public void SetWeight(string poligon, string bone0)
        {
            var vertex_list = pol.CollectVertex(poligon);
            foreach(var v in vertex_list)
            {
                v.bone0 = bone0;
            }
        }
        //検索
        public int FindBoneIndex(string name)
        {
            var idx =  bone_list.FindIndex(i=>i.name == name);
            if (idx<0)
                throw new SystemException();
            return idx;
        }
        public bone FindBone(string name)
        {
            return bone_list.Find(i=>i.name == name);
        }

        public void SKINNEDMESH_createBones(Transform transform,  out Transform[] bones, out Matrix4x4[] bindPoses)
        {
            var tf_list = new List<Transform>();
            var bp_list = new List<Matrix4x4>();

            foreach(var b in bone_list)
            {
                var bp   = Matrix4x4.identity;
                var go   = new GameObject(b.name);
                var tr   = go.transform;
                tr.rotation = Quaternion.Euler(0,b.angle_y,0);
                tr.position = b.org;

                b.tmp_tr = tr;
                tr.parent = (b.parent!=null) ?  FindBone(b.parent).tmp_tr : transform;

                //tr.localPosition = b.org - tr.parent.position;
                bp = tr.worldToLocalMatrix * transform.localToWorldMatrix;
                
                tf_list.Add(tr);
                bp_list.Add(bp);
            }

            bones     = tf_list.ToArray();
            bindPoses = bp_list.ToArray();
        }
    }
#endregion

#region ポリゴン作成
    // 矢印の種類
    //
    //  一方向
    //  直角―右・左       
    //  Uターンー右・左
    //  Ｓターンー右・左
    //  

    /// <summary>
    ///      
    ///  長方形を作成
    ///  　　oが底辺の中央　Zポジションを指定
    /// 
    ///          width
    ///      |<------->| c
    ///    b +---------+ ^ 
    ///      |         | |len
    ///      |         | |
    ///    a |----o----| v d    
    ///      
    /// </summary>
    private poligon POL_CreateRectangle(poligon pol, string poligon_name, float width, float len, Vector3 org, float angle_y)
    {
        pol.Begin(poligon_name);
        {
            var a = "a";
            var b = "b";
            var c = "c";
            var d = "d";

            var wh = width / 2.0f;

            var va = new Vector3(-wh,0,0);
            var vb = new Vector3(-wh,0,len);
            var vc = new Vector3( wh,0,len);
            var vd = new Vector3( wh,0,0);

            var mat = Matrix4x4.TRS(Vector3.zero,Quaternion.Euler(0,angle_y,0),Vector3.one);

            Func<Vector3,Vector3> _rot = (v)=> {
                return (Vector3)(mat *v) + org;
            };

            va = _rot(va);
            vb = _rot(vb);
            vc = _rot(vc);
            vd = _rot(vd);

            pol.AddVertex(a,va);
            pol.AddVertex(b,vb);
            pol.AddVertex(c,vc);
            pol.AddVertex(d,vd);

            pol.AddTriIndex(a, b, c);
            pol.AddTriIndex(c, d, a);

        }
        pol.End();
        return pol;
    }

    /// <summary>
    ///   90度のカーブの曲線
    ///       ________ an
    ///      /     　　^ outer radius
    ///     /        bn|
    ///    /     /~~~~ | ^inner radius
    ///   /     /      | 
    /// a0|  o  |b0    |
    ///      <-------->c
    ///         radius
    /// </summary>
    private poligon POL_CreateRightCurve(poligon pol, string poligon_name, float width, float radius, float angle_y, Vector3 org,int divnum)
    {
        //　１． oを0,0,0とする
        //  ２． 右曲がりで９０度の半円を外側と内側に作る .. bReverse自は逆
        //  ３． angle_yで傾ける
        //  ４． orgへ移動

        pol.Begin(poligon_name);
        {
            var wh = width / 2.0f;
            var circle_cneter = Vector3.right * radius;
            var outer_radius  = radius + wh;
            var inner_radius  = radius - wh;

            var step = 90.0f / divnum;
            var list_a = new List<Vector3>();
            var list_b = new List<Vector3>();
            for(var i = 0; i< divnum+1; i++)
            {
                var dx = Mathf.Cos(i * step * Mathf.Deg2Rad);
                var dz = Mathf.Sin(i * step * Mathf.Deg2Rad);
                var dv = Vector3.left * dx + Vector3.forward * dz;
                list_a.Add( outer_radius * dv + circle_cneter);
                list_b.Add( inner_radius * dv + circle_cneter);
            }

            var mat = Matrix4x4.TRS(Vector3.zero,Quaternion.Euler(0,angle_y,0),Vector3.one);

            Func<Vector3,Vector3> _rot = (v)=> {
                return (Vector3)(mat *v) + org;
            };
        
            for(var i=0; i<list_a.Count; i++)
            {
                var av = _rot(list_a[i]);
                var bv = _rot(list_b[i]);

                Func<int, string> a = (n) => ("a" + n.ToString());
                Func<int, string> b = (n) => ("b" + n.ToString());

                pol.AddVertex(a(i), av); // an
                pol.AddVertex(b(i), bv); // bn

                if (i > 0)
                {
                    var prev = i - 1;
                    var a0 = a(prev);
                    var b0 = b(prev);
                    var a1 = a(i);
                    var b1 = b(i);

                    pol.AddTriIndex(a0, a1, b1);
                    pol.AddTriIndex(b1, b0, a0);
                }
            }
        }
        pol.End();

        return pol;
    }
    private poligon POL_CreateLeftCurve(poligon pol, string poligon_name, float width, float radius, float angle_y, Vector3 org,int divnum)
    {
        //　１． oを0,0,0とする
        //  ２． 右曲がりで９０度の半円を外側と内側に作る .. bReverse自は逆
        //  ３． angle_yで傾ける
        //  ４． orgへ移動

        pol.Begin(poligon_name);
        {
            var wh = width / 2.0f;
            var circle_cneter = Vector3.left * radius;
            var outer_radius  = radius + wh;
            var inner_radius  = radius - wh;

            var step = 90.0f / divnum;
            var list_a = new List<Vector3>();
            var list_b = new List<Vector3>();
            for(var i = 0; i< divnum+1; i++)
            {
                var dx = Mathf.Cos(i * step * Mathf.Deg2Rad);
                var dz = Mathf.Sin(i * step * Mathf.Deg2Rad);
                var dv = Vector3.right * dx + Vector3.forward * dz;
                list_a.Add( inner_radius * dv + circle_cneter);
                list_b.Add( outer_radius * dv + circle_cneter);
            }

            var mat = Matrix4x4.TRS(Vector3.zero,Quaternion.Euler(0,angle_y,0),Vector3.one);

            Func<Vector3,Vector3> _rot = (v)=> {
                return (Vector3)(mat *v) + org;
            };
        
            for(var i=0; i<list_a.Count; i++)
            {
                var av = _rot(list_a[i]);
                var bv = _rot(list_b[i]);

                Func<int, string> a = (n) => ("a" + n.ToString());
                Func<int, string> b = (n) => ("b" + n.ToString());

                pol.AddVertex(a(i), av); // an
                pol.AddVertex(b(i), bv); // bn

                if (i > 0)
                {
                    var prev = i - 1;
                    var a0 = a(prev);
                    var b0 = b(prev);
                    var a1 = a(i);
                    var b1 = b(i);

                    pol.AddTriIndex(a0, a1, b1);
                    pol.AddTriIndex(b1, b0, a0);
                }
            }
        }
        pol.End();

        return pol;
    }

    /// <summary>
    ///              c         
    ///              /⧵
    ///             /  ⧵
    ///            /    ⧵
    ///           /      ⧵
    ///          / a o  e ⧵ 
    ///        b --+-()-+-- d
    ///            |    |   
    /// 
    /// </summary>
    private poligon POL_CreateArrowRoot(poligon pol, string poligon_name, float width, float height, float shaftwidth, float angle_y, Vector3 org )
    {
        pol.Begin(poligon_name);
        {
            var a = "a";
            var b = "b";
            var c = "c";
            var d = "d";
            var e = "e";

            var ahw = width / 2.0f;
            var shw = shaftwidth / 2.0f;

            var va = new Vector3(-shw,0,0     );
            var vb = new Vector3(-ahw,0,0     );
            var vc = new Vector3(   0,0,height);
            var vd = new Vector3( ahw,0,0     );
            var ve = new Vector3( shw,0,0     );

            var mat = Matrix4x4.TRS(Vector3.zero,Quaternion.Euler(0,angle_y,0),Vector3.one);

            Func<Vector3,Vector3> _rot = (v)=> {
                return (Vector3)(mat *v) + org;
            };

            va = _rot(va);
            vb = _rot(vb);
            vc = _rot(vc);
            vd = _rot(vd);
            ve = _rot(ve);

            pol.AddVertex(a,va);           //a
            pol.AddVertex(b,vb);           //b
            pol.AddVertex(c,vc);           //c
            pol.AddVertex(d,vd);           //d
            pol.AddVertex(e,ve);           //e

            //a-b-c, a-c-e, e-c-d
            pol.AddTriIndex(a,b,c);
            pol.AddTriIndex(a,c,e);
            pol.AddTriIndex(e,c,d);

        }
        pol.End();
        return pol;
    }
#endregion

#region スキンパーツ作成用
    private Skin SKINPARTS_createRoot(float width) 
    {
        var skin = new Skin();

        skin.AddBone("root",Vector3.zero,0);

        skin.shaft_width = width;
        skin.next_angle_y = 0;
        skin.next_org = Vector3.zero;

        return skin;
    }
    private Skin SKINPARTS_addShaft(Skin skin, string id, float len)
    {
        var name    = id + "_shaft";
        var width   = skin.shaft_width;
        var org     = skin.next_org;
        var angle_y = skin.next_angle_y;

        POL_CreateRectangle(skin.pol, name,width,len, org,angle_y);

        skin.AddBone(name,org,angle_y);

        skin.SetWeight(name,name);

        skin.next_org = org + (Vector3)(Matrix4x4.TRS(Vector3.zero,Quaternion.Euler(0,angle_y,0),Vector3.one) * Vector3.forward);

        return skin;
    }
    private Skin SKINPARTS_addCurve90(Skin skin, string id, float radius, int divnum, bool bRev)
    {
        var name    = id + "_curve90";
        var width   = skin.shaft_width;
        var org     = skin.next_org;
        var angle_y = skin.next_angle_y;

        if (bRev)
        {
            POL_CreateLeftCurve(skin.pol,name,width,radius,angle_y,org,divnum);
            skin.AddBone(name,org,angle_y);
            skin.SetWeight(name,name);
            skin.next_org = org + (Vector3)(Matrix4x4.TRS(Vector3.zero,Quaternion.Euler(0,angle_y,0),Vector3.one) * (new Vector3(-radius,0,radius)));
            skin.next_angle_y -= 90;
        }
        else
        {
            POL_CreateRightCurve(skin.pol,name,width,radius,angle_y,org,divnum);
            skin.AddBone(name,org,angle_y);
            skin.SetWeight(name,name);
            skin.next_org = org + (Vector3)(Matrix4x4.TRS(Vector3.zero,Quaternion.Euler(0,angle_y,0),Vector3.one) * (new Vector3( radius,0,radius)));
            skin.next_angle_y += 90;
        }
        
        return skin;
    }  
    private Skin SKINPARTS_addArrowRoot(Skin skin, float width, float len)
    {
        var name       = "arrow";
        var shaftwidth = skin.shaft_width;
        var org        = skin.next_org;
        var angle_y    = skin.next_angle_y;

        POL_CreateArrowRoot(skin.pol, name,width,len,shaftwidth,angle_y,org);

        skin.AddBone(name, org, angle_y);

        skin.SetWeight(name,name);
        
        skin.next_org += (Vector3)(Matrix4x4.TRS(Vector3.zero,Quaternion.Euler(0,angle_y,0),Vector3.one) * Vector3.forward);

        return skin;
    }
    private Skin SKINPARTS_addArrowHead(Skin skin)
    {
        var name       = "head";
        var org        = skin.next_org;
        var angle_y    = skin.next_angle_y;

        skin.AddBone(name, org, angle_y);

        skin.next_org = (Vector3)(Matrix4x4.TRS(Vector3.zero,Quaternion.Euler(0,angle_y,0),Vector3.one) * Vector3.forward);

        return skin;
    }
#endregion

#region スキン作成
    //一方向矢印のスキン作成
    private Skin CreateSkin_OneWay_Arrow(float shaft_width, float unit_len, int unit_divnum, float arrow_width)
    {
        var skin = SKINPARTS_createRoot(shaft_width);

        skin = SKINPARTS_addShaft(skin,"shaft",unit_len);

        skin = SKINPARTS_addArrowRoot(skin,arrow_width,unit_len);

        skin = SKINPARTS_addArrowHead(skin);

        return skin;
    }

    //右方向矢印のスキン作成
    private Skin CreateSkin_Curve_R_Arrow(float shaft_width, float unit_len, int unit_divnum, float arrow_width)
    {
        var skin = SKINPARTS_createRoot(shaft_width);

        skin = SKINPARTS_addShaft(skin,"shaft1",unit_len);

        skin = SKINPARTS_addCurve90(skin,"curve1",unit_len,unit_divnum,false);

        skin = SKINPARTS_addShaft(skin,"shaft2",unit_len);

        skin = SKINPARTS_addArrowRoot(skin,arrow_width,unit_len);

        skin = SKINPARTS_addArrowHead(skin);

        return skin;
    }
    //左方向矢印のスキン作成
    private Skin CreateSkin_Curve_L_Arrow(float shaft_width, float unit_len, int unit_divnum, float arrow_width)
    {
        var skin = SKINPARTS_createRoot(shaft_width);

        skin = SKINPARTS_addShaft(skin,"shaft1",unit_len);

        skin = SKINPARTS_addCurve90(skin,"curve1",unit_len,unit_divnum,true);

        skin = SKINPARTS_addShaft(skin,"shaft2",unit_len);

        skin = SKINPARTS_addArrowRoot(skin,arrow_width,unit_len);

        skin = SKINPARTS_addArrowHead(skin);

        return skin;
    }
    //Ｕターン右用のスキン作成
    private Skin CreateSkin_U_Turn_R_Arrow(float shaft_width, float unit_len, int unit_divnum, float arrow_width)
    {
        var skin = SKINPARTS_createRoot(shaft_width);

        skin = SKINPARTS_addShaft(skin,"shaft1",unit_len);

        skin = SKINPARTS_addCurve90(skin,"curve1",unit_len,unit_divnum,false);

        skin = SKINPARTS_addShaft(skin,"shaft2",unit_len);

        skin = SKINPARTS_addCurve90(skin,"curve2",unit_len,unit_divnum,false);

        skin = SKINPARTS_addShaft(skin,"shaft3",unit_len);

        skin = SKINPARTS_addArrowRoot(skin,arrow_width,unit_len);

        skin = SKINPARTS_addArrowHead(skin);

        skin.InsertMidBone("shaft2_shaft");
        

        return skin;
    }
    //Uターン左用のスキン作成
    private Skin CreateSkin_U_Turn_L_Arrow(float shaft_width, float unit_len, int unit_divnum, float arrow_width)
    {
        var skin = SKINPARTS_createRoot(shaft_width);

        skin = SKINPARTS_addShaft(skin,"shaft1",unit_len);

        skin = SKINPARTS_addCurve90(skin,"curve1",unit_len,unit_divnum,true);

        skin = SKINPARTS_addShaft(skin,"shaft2",unit_len);

        skin = SKINPARTS_addCurve90(skin,"curve2",unit_len,unit_divnum,true);

        skin = SKINPARTS_addShaft(skin,"shaft3",unit_len);

        skin = SKINPARTS_addArrowRoot(skin,arrow_width,unit_len);

        skin = SKINPARTS_addArrowHead(skin);

        skin.InsertMidBone("shaft2_shaft");

        return skin;
    }
    //Sターン右用のスキン作成
    private Skin CreateSkin_S_Turn_R_Arrow(float shaft_width, float unit_len, int unit_divnum, float arrow_width)
    {
        var skin = SKINPARTS_createRoot(shaft_width);

        skin = SKINPARTS_addShaft(skin,"shaft1",unit_len);

        skin = SKINPARTS_addCurve90(skin,"curve1",unit_len,unit_divnum,false);

        skin = SKINPARTS_addShaft(skin,"shaft2",unit_len);

        skin = SKINPARTS_addCurve90(skin,"curve2",unit_len,unit_divnum,true);

        skin = SKINPARTS_addShaft(skin,"shaft3",unit_len);

        skin = SKINPARTS_addArrowRoot(skin,arrow_width,unit_len);

        skin = SKINPARTS_addArrowHead(skin);

        skin.InsertMidBone("shaft2_shaft");

        return skin;
    }
    //Sターン左用のスキン作成
    private Skin CreateSkin_S_Turn_L_Arrow(float shaft_width, float unit_len, int unit_divnum, float arrow_width)
    {
        var skin = SKINPARTS_createRoot(shaft_width);

        skin = SKINPARTS_addShaft(skin,"shaft1",unit_len);

        skin = SKINPARTS_addCurve90(skin,"curve1",unit_len,unit_divnum,true);

        skin = SKINPARTS_addShaft(skin,"shaft2",unit_len);

        skin = SKINPARTS_addCurve90(skin,"curve2",unit_len,unit_divnum,false);

        skin = SKINPARTS_addShaft(skin,"shaft3",unit_len);

        skin = SKINPARTS_addArrowRoot(skin,arrow_width,unit_len);

        skin = SKINPARTS_addArrowHead(skin);

        skin.InsertMidBone("shaft2_shaft");

        return skin;
    }
    //SSターン右用のスキン作成
    //
    //
    //    +---+  ^
    //    |   |  | 
    //        +---
    private Skin CreateSkin_SS_Turn_R_Arrow(float shaft_width, float unit_len, int unit_divnum, float arrow_width)
    {
        var skin = SKINPARTS_createRoot(shaft_width);

        skin = SKINPARTS_addShaft(skin,"shaft1",unit_len);

        skin = SKINPARTS_addCurve90(skin,"curve1",unit_len,unit_divnum,false);

        skin = SKINPARTS_addShaft(skin,"shaft2",unit_len);

        skin = SKINPARTS_addCurve90(skin,"curve2",unit_len,unit_divnum,false);

        skin = SKINPARTS_addShaft(skin,"shaft3",unit_len);

        skin = SKINPARTS_addCurve90(skin,"curve3",unit_len,unit_divnum,true);

        skin = SKINPARTS_addShaft(skin,"shaft4",unit_len);

        skin = SKINPARTS_addCurve90(skin,"curve4",unit_len,unit_divnum,true);

        skin = SKINPARTS_addShaft(skin,"shaft5",unit_len);

        skin = SKINPARTS_addArrowRoot(skin,arrow_width,unit_len);

        skin = SKINPARTS_addArrowHead(skin);

        skin.InsertMidBone("shaft2_shaft");
        skin.InsertMidBone("shaft3_shaft","1");
        skin.InsertMidBone("shaft4_shaft","2");
        
        return skin;
    }
    private Skin CreateSkin_SS_Turn_L_Arrow(float shaft_width, float unit_len, int unit_divnum, float arrow_width)
    {
        var skin = SKINPARTS_createRoot(shaft_width);

        skin = SKINPARTS_addShaft(skin,"shaft1",unit_len);

        skin = SKINPARTS_addCurve90(skin,"curve1",unit_len,unit_divnum,true);

        skin = SKINPARTS_addShaft(skin,"shaft2",unit_len);

        skin = SKINPARTS_addCurve90(skin,"curve2",unit_len,unit_divnum,true);

        skin = SKINPARTS_addShaft(skin,"shaft3",unit_len);

        skin = SKINPARTS_addCurve90(skin,"curve3",unit_len,unit_divnum,false);

        skin = SKINPARTS_addShaft(skin,"shaft4",unit_len);

        skin = SKINPARTS_addCurve90(skin,"curve4",unit_len,unit_divnum,false);

        skin = SKINPARTS_addShaft(skin,"shaft5",unit_len);

        skin = SKINPARTS_addArrowRoot(skin,arrow_width,unit_len);

        skin = SKINPARTS_addArrowHead(skin);

        skin.InsertMidBone("shaft2_shaft");
        skin.InsertMidBone("shaft3_shaft","1");
        skin.InsertMidBone("shaft4_shaft","2");
        
        return skin;
    }
    // 先頭部分がクランプ
    //             <---+
    //                 |
    //             +---+
    //             |
    private Skin CreateSkin_HEAD_C_R_Arrow(float shaft_width, float unit_len, int unit_divnum, float arrow_width)
    {
        var skin = SKINPARTS_createRoot(shaft_width);

        skin = SKINPARTS_addShaft(skin,"shaft1",unit_len);

        skin = SKINPARTS_addCurve90(skin,"curve1",unit_len,unit_divnum,false);

        skin = SKINPARTS_addShaft(skin,"shaft2",unit_len);

        skin = SKINPARTS_addCurve90(skin,"curve2",unit_len,unit_divnum,true);

        skin = SKINPARTS_addShaft(skin,"shaft3",unit_len);

        skin = SKINPARTS_addCurve90(skin,"curve3",unit_len,unit_divnum,true);

        skin = SKINPARTS_addShaft(skin,"shaft4",unit_len);

        skin = SKINPARTS_addArrowRoot(skin,arrow_width,unit_len);

        skin = SKINPARTS_addArrowHead(skin);

        skin.InsertMidBone("shaft2_shaft");
        skin.InsertMidBone("shaft3_shaft","1");

        return skin;
    }
    private Skin CreateSkin_HEAD_C_L_Arrow(float shaft_width, float unit_len, int unit_divnum, float arrow_width)
    {
        var skin = SKINPARTS_createRoot(shaft_width);

        skin = SKINPARTS_addShaft(skin,"shaft1",unit_len);

        skin = SKINPARTS_addCurve90(skin,"curve1",unit_len,unit_divnum,true);

        skin = SKINPARTS_addShaft(skin,"shaft2",unit_len);

        skin = SKINPARTS_addCurve90(skin,"curve2",unit_len,unit_divnum,false);

        skin = SKINPARTS_addShaft(skin,"shaft3",unit_len);

        skin = SKINPARTS_addCurve90(skin,"curve3",unit_len,unit_divnum,false);

        skin = SKINPARTS_addShaft(skin,"shaft4",unit_len);

        skin = SKINPARTS_addArrowRoot(skin,arrow_width,unit_len);

        skin = SKINPARTS_addArrowHead(skin);

        skin.InsertMidBone("shaft2_shaft");
        skin.InsertMidBone("shaft3_shaft","1");

        return skin;
    }
    //根本がクランプ
    //
    //       +---+   
    //       |   |   
    //           +--->
    private Skin CreateSkin_TAIL_C_R_Arrow(float shaft_width, float unit_len, int unit_divnum, float arrow_width)
    {
        var skin = SKINPARTS_createRoot(shaft_width);

        skin = SKINPARTS_addShaft(skin,"shaft1",unit_len);

        skin = SKINPARTS_addCurve90(skin,"curve1",unit_len,unit_divnum,false);

        skin = SKINPARTS_addShaft(skin,"shaft2",unit_len);

        skin = SKINPARTS_addCurve90(skin,"curve2",unit_len,unit_divnum,false);

        skin = SKINPARTS_addShaft(skin,"shaft3",unit_len);

        skin = SKINPARTS_addCurve90(skin,"curve3",unit_len,unit_divnum,true);

        skin = SKINPARTS_addShaft(skin,"shaft4",unit_len);

        skin = SKINPARTS_addArrowRoot(skin,arrow_width,unit_len);

        skin = SKINPARTS_addArrowHead(skin);

        skin.InsertMidBone("shaft2_shaft");
        skin.InsertMidBone("shaft3_shaft","1");

        return skin;
    }
    private Skin CreateSkin_TAIL_C_L_Arrow(float shaft_width, float unit_len, int unit_divnum, float arrow_width)
    {
        var skin = SKINPARTS_createRoot(shaft_width);

        skin = SKINPARTS_addShaft(skin,"shaft1",unit_len);

        skin = SKINPARTS_addCurve90(skin,"curve1",unit_len,unit_divnum,true);

        skin = SKINPARTS_addShaft(skin,"shaft2",unit_len);

        skin = SKINPARTS_addCurve90(skin,"curve2",unit_len,unit_divnum,true);

        skin = SKINPARTS_addShaft(skin,"shaft3",unit_len);

        skin = SKINPARTS_addCurve90(skin,"curve3",unit_len,unit_divnum,false);

        skin = SKINPARTS_addShaft(skin,"shaft4",unit_len);

        skin = SKINPARTS_addArrowRoot(skin,arrow_width,unit_len);

        skin = SKINPARTS_addArrowHead(skin);

        skin.InsertMidBone("shaft2_shaft");
        skin.InsertMidBone("shaft3_shaft","1");

        return skin;
    }

#endregion

#region スキンからメッシュ生成
    private void CreateMesh(Skin skin, GameObject go)
    {
        // ref https://docs.unity3d.com/jp/540/ScriptReference/Mesh-bindposes.html

        var rend = go.AddComponent<SkinnedMeshRenderer>();

        // Build Mesh
        var vlist = new List<Vector3>();  //頂点
        var tlist = new List<int>();      //三角形インデックス
        var uvlist = new List<Vector2>();
        var bwlist = new List<BoneWeight>();

        skin.pol.MESH_create(skin, ref vlist, ref tlist,ref uvlist, ref bwlist );

        Mesh mesh = new Mesh();
        mesh.vertices = vlist.ToArray();
        mesh.uv = uvlist.ToArray();
        mesh.triangles = tlist.ToArray();
        mesh.RecalculateNormals();
        rend.material = new Material(Shader.Find("Diffuse"));

        // assign bone weight
        mesh.boneWeights = bwlist.ToArray();

        Transform[] bones;
        Matrix4x4[] bindPoses;

        skin.SKINNEDMESH_createBones( go.transform, out bones, out bindPoses);
        mesh.bindposes = bindPoses;
        rend.bones = bones;
        rend.sharedMesh = mesh;
    }
#endregion

#region 公開
    
    public static GameObject CreateArrowSub(Arrow.TYPE type,float shuft_width, float unit_len, int curve_divnum, float arrow_width)
    {
        var p  = new ArrowMaker();
        var go = new GameObject(type.ToString());
        Skin skin = null;

        switch(type)
        {
            case Arrow.TYPE.ONEWAY:    skin =  p.CreateSkin_OneWay_Arrow  (shuft_width,unit_len,curve_divnum,arrow_width);     break;
            case Arrow.TYPE.TURN_R:    skin =  p.CreateSkin_Curve_R_Arrow (shuft_width,unit_len,curve_divnum,arrow_width);     break;
            case Arrow.TYPE.TURN_L:    skin =  p.CreateSkin_Curve_L_Arrow (shuft_width,unit_len,curve_divnum,arrow_width);     break;
            case Arrow.TYPE.U_TURN_R:  skin =  p.CreateSkin_U_Turn_R_Arrow(shuft_width,unit_len,curve_divnum,arrow_width);     break;
            case Arrow.TYPE.U_TURN_L:  skin =  p.CreateSkin_U_Turn_L_Arrow(shuft_width,unit_len,curve_divnum,arrow_width);     break;
            case Arrow.TYPE.S_TURN_R:  skin =  p.CreateSkin_S_Turn_R_Arrow(shuft_width,unit_len,curve_divnum,arrow_width);     break;
            case Arrow.TYPE.S_TURN_L:  skin =  p.CreateSkin_S_Turn_L_Arrow(shuft_width,unit_len,curve_divnum,arrow_width);     break;
            case Arrow.TYPE.SS_TURN_R: skin =  p.CreateSkin_SS_Turn_R_Arrow(shuft_width,unit_len,curve_divnum,arrow_width);    break;
            case Arrow.TYPE.SS_TURN_L: skin =  p.CreateSkin_SS_Turn_L_Arrow(shuft_width,unit_len,curve_divnum,arrow_width);    break;
            case Arrow.TYPE.HEAD_C_R:  skin =  p.CreateSkin_HEAD_C_R_Arrow(shuft_width,unit_len,curve_divnum,arrow_width);     break;
            case Arrow.TYPE.HEAD_C_L:  skin =  p.CreateSkin_HEAD_C_L_Arrow(shuft_width,unit_len,curve_divnum,arrow_width);     break;
            case Arrow.TYPE.TAIL_C_R:  skin =  p.CreateSkin_TAIL_C_R_Arrow(shuft_width,unit_len,curve_divnum,arrow_width);     break;
            case Arrow.TYPE.TAIL_C_L:  skin =  p.CreateSkin_TAIL_C_L_Arrow(shuft_width,unit_len,curve_divnum,arrow_width);     break;
        }
        p.CreateMesh(skin,go);
        return go;
    }

    public static GameObject CreateArrow(Arrow.TYPE type)
    {
        var go = new GameObject("ARROW");
        var ac = go.AddComponent<Arrow>();
        ac.m_type = type;
        return go;
    }
#endregion
}
