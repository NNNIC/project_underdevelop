using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Arrow {
    //
    // 方針：頂点などを記号化した状態のままのデータ構造として、扱いやすい形で
    //       
    //       最終的にスキンメッシュを作成する。
    //

    #region 格納クラス
    //
    public class vertex
    {
        public string     name;        //名前
        public string     poligonname; //所属ポリゴン名

        public Vector3    v;       //座標
        public string     bone0;   //所属ボーン0
        public string     bone1;   //所属ボーン1

        public float      weight0; //ウエイト0
        public float      weight1; //ウエイト1

        public int        tmp_index; //メッシュ作成時に使用の一時的インデックス

        public vertex(string poligon_name, string vertex_name, float x, float y, float z)
        {
            name        = vertex_name;
            poligonname = poligon_name;

            v = new Vector3(x,y,z);

            bone0 = null; //未定義
            bone1 = null; //未定義
            weight0 = 1;
            weight1 = 0;
        }
    }

    public class poligon
    {
        public Dictionary<string,vertex> vertex_dic   = new Dictionary<string, vertex>();    //頂点リスト

        public struct trindex {public Vector3 idx0, idx1, idx2; }                            //３角形情報を頂点で管理（※インデックスではない）
        public List<trindex>  tri_index_list= new List<trindex>();                           //トライアングルインデックス

        #region ポジション一致
        private bool IsEqualVector3(Vector3 v1, Vector3 v2)
        {
            float epsiron = 0.0005f;  //floatの有効桁は6桁
            var dx = Mathf.Abs(v1.x - v2.x);
            var dy = Mathf.Abs(v1.y - v2.y);
            var dz = Mathf.Abs(v1.z - v2.z);

            return (dx<epsiron && dy<epsiron && dz<epsiron);
        }
        #endregion

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

        private string _key(string vertexname) { return _tmpsaved_poligonname + ">" + vertexname;  }
        private string _key(string poligonname,string vertexname) { return poligonname + ">" + vertexname;  }

        public void AddVertex(string vertexname, float x, float y, float z)
        {
            if (_tmpsaved_poligonname==null) throw new SystemException();
            foreach(var p in vertex_dic)
            {
                var b = IsEqualVector3(p.Value.v, new Vector3(x,y,z));
                if (b)
                {
                    vertex_dic.Remove(p.Key);
                    break;
                }
            }
            vertex_dic.Add(_key(vertexname), new vertex(_tmpsaved_poligonname,vertexname, x,y,z));
        }

        public void AddTriIndex(string n1, string n2, string n3)
        {
            tri_index_list.Add(new trindex() { idx0=Pos(n1), idx1=Pos(n2), idx2=Pos(n3) });
        }

        //検索
        public vertex FindVertex(string vertexname)
        {
            var key = _key(vertexname);
            if (vertex_dic.ContainsKey(key))
            {
                return vertex_dic[key];
            }
            return null;
        }
        public vertex FindVertex(Vector3 pos)
        {
            foreach(var k in vertex_dic.Keys)
            {
                var v = vertex_dic[k];
                if (IsEqualVector3 (v.v, pos)) return v;
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
        public List<vertex> CollectVertex(string poligonname)
        {
            var list = new List<vertex>();
            var nullkeyname = _key(poligonname,"");
            foreach(var k in vertex_dic.Keys)
            {
                if (k.StartsWith(nullkeyname))
                {
                    list.Add(vertex_dic[k]);
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
            foreach(var p in vertex_dic)
            {
                var vtx = p.Value;
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
        /*
            前提として、Ｚ方向に真っすぐ整列するため、規定値になるものは除外
        */

        public enum TYPE
        {
            NONE,

            ROOT,               //矢印の根本
            ROOT_SHAFT,         //根元の幹

            JOINT_1ST,         //接続部分の外受け　　
            JOINT_2ND,         //接続部分の中受け
            JOINT_SHAFT,         //接続の幹

            ARROW_ROOT,          //矢じりの付根
            ARROW_HEAD           //矢じりの頭
        }

        public string     name;

        public TYPE       type;

        public string     parent;                    //親ボーン名

        public Vector3    org;                       //起点

        public Transform  tmp_tr;                    //メッシュ作成時に利用

        public bone(string i_name, TYPE i_type, string i_panrent, float i_org)
        {
            name   = i_name;
            type   = i_type;
            parent = i_panrent;
            org    = Vector3.forward * i_org;
        }
    }

    public class Skin
    {
        public poligon       pol       = new poligon();
        public List<bone>    bone_list = new List<bone>();

        public float lenght; //矢印の長さ

        public void AddBone(string name, bone.TYPE type, float org)
        {
            string parent = null; //本パターンでは必ず１つ前が親
            if (bone_list.Count>0)
            {
                parent = bone_list[bone_list.Count-1].name;
            }
            var bone = new bone(name,type,parent,org);
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
        public void SetWeight(string poligon, string bone0, string bone1)
        {
            SetWeight(poligon,bone0); //暫定
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
                tr.localRotation = Quaternion.identity;
                tr.position = b.org;

                b.tmp_tr = tr;
                tr.parent = (b.parent!=null) ?  FindBone(b.parent).tmp_tr : transform;

                tr.localPosition = b.org - tr.parent.position;
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
    //                     繰り返し
    //               [<-----------------]
    // root   rs       j1     j2     js    ar       ah
    // bone0->bone1-> bone2->bone3->bone4->bone5->bone6
    //
    // Ｚ方向へ
    // xz平面の2次平面に描画
    // root と arrow-headは、長さ０で所属するメッシュはない。ポイントとして利用
    // よって矢印の全長は
    // 繰り返しｘ０(直線0)　 --- 3 
    // 繰り返しｘ１(曲ｘ１)  --- 3+3 = 6
    // 繰り返しｘ２(曲ｘ２)  --- 3+3x2 = 9
    // 繰り返しｘn(曲ｘn)    --- 3+3xn
    // 
    // shaftの部分が伸長
    // 
    // 曲がるときは、joint-outerに接続するjoint-innerがy軸 +/- 90度まで傾く
    // 
    // 矢印の種類
    //
    //  ストレート 
    //  直角       
    //  汎用(Uターン/S字)  構造が同じなので
    //  カスタム
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
    private poligon POL_CreateRectangle(poligon pol, string poligon_name, float width, float len, float org)
    {
        pol.Begin(poligon_name);
        {
            var a = "a";
            var b = "b";
            var c = "c";
            var d = "d";

            var wh = width / 2.0f;

            pol.AddVertex(a,-wh,0,org);
            pol.AddVertex(b,-wh,0,org + len);
            pol.AddVertex(c, wh,0,org + len);
            pol.AddVertex(d, wh,0,org);

            pol.AddTriIndex(a,b,c);
            pol.AddTriIndex(c,d,a);

            pol.End();
        }
        return pol;
    }
    /// <summary>
    /// 
    ///    an|---------|bn
    ///      :         :
    ///    a1|---------|b1
    ///    a0|----o----|b0
    /// 
    /// </summary>
    private poligon POL_CreateRectangles_1xN(poligon pol, string poligon_name,float width, float len, float org, int divnum)
    {
        pol.Begin(poligon_name);
        {
            var dl = len / divnum;
            var wh = width / 2.0f;

            for(var i = 0; i<=divnum+1; i++)
            {
                Func<int,string> a = (n)=>("a"+n.ToString());
                Func<int,string> b = (n)=>("b"+n.ToString());

                pol.AddVertex(a(i),  -wh,0, dl*i + org); // an
                pol.AddVertex(b(i),   wh,0, dl*i + org); // bn

                if (i>0)
                {
                    var prev = i-1;
                    var a0 = a(prev);
                    var b0 = b(prev);
                    var a1 = a(i);
                    var b1 = b(i);

                    pol.AddTriIndex(a0,a1,b1);
                    pol.AddTriIndex(b1,b0,a0);
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
    private poligon POL_CreateArrowRoot(poligon pol, string poligon_name, float width, float height, float shaftwidth, float org)
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

            pol.AddVertex(a,-shw,0,org);           //a
            pol.AddVertex(b,-ahw,0,org);           //b
            pol.AddVertex(c,   0,0,org+height);    //c
            pol.AddVertex(d, ahw,0,org);           //d
            pol.AddVertex(e, shw,0,org);           //e

            //a-b-c, a-c-e, e-c-d
            pol.AddTriIndex(a,b,c);
            pol.AddTriIndex(a,c,e);
            pol.AddTriIndex(e,c,d);

            return pol;
        }
        pol.End();
    }
    #endregion

    #region スキン作成用
    private Skin SKIN_0_createRootSkin(float org)
    {
        var skin = new Skin();

        skin.AddBone("root",bone.TYPE.ROOT,0);

        skin.lenght = 0;

        return skin;
    }
    private Skin SKIN_1_addRootShaft(Skin skin, float width, float len,float org)
    {
        var name = "root_shaft";

        var pol = POL_CreateRectangle(skin.pol, name,width,len,org);

        skin.AddBone(name, bone.TYPE.ROOT_SHAFT,0);

        skin.SetWeight(name,name);

        skin.lenght += len;

        return skin;
    }
    private Skin SKIN_2_addJointSkin(Skin skin, string id, float width, float base_len, float org, int each_divnum)
    {

        var joint_1st_name   = id + "_joint1st";
        var joint_2nd_name   = id + "_joint2nd";
        var joint_shaft_name = id + "_jointshaft";

        //Create Joint poligons
        var joint_1st_pol   = POL_CreateRectangles_1xN(skin.pol, joint_1st_name, width,base_len,org         , each_divnum);
        var joint_2nd_pol   = POL_CreateRectangles_1xN(skin.pol, joint_2nd_name, width,base_len,org+base_len,each_divnum);

        var joint_shuft_pol = POL_CreateRectangle(skin.pol, joint_shaft_name, width,base_len, org+base_len*2 );

        //Create Joint bones
        skin.AddBone(joint_1st_name,   bone.TYPE.JOINT_1ST,org);
        skin.AddBone(joint_2nd_name,   bone.TYPE.JOINT_2ND,org + base_len);
        skin.AddBone(joint_shaft_name, bone.TYPE.JOINT_2ND,org + base_len * 2);

        //Set Weight
        skin.SetWeight(joint_1st_name,joint_1st_name,joint_2nd_name);
        skin.SetWeight(joint_2nd_name,joint_2nd_name,joint_1st_name);
        skin.SetWeight(joint_shaft_name,joint_shaft_name);

        skin.lenght += base_len * 3;

        return skin;
    }
    private Skin SKIN_3_addArrowRootSkin(Skin skin, float width, float len, float shaft_width, float org)
    {
        var name = "arrow";

        var pol = POL_CreateArrowRoot(skin.pol, name,width,len,shaft_width,org);

        skin.AddBone(name, bone.TYPE.ARROW_ROOT,org);

        skin.SetWeight(name,name);

        skin.lenght += len;

        return skin;
    }
    private Skin SKIN_4_addArrowHeadSkin(Skin skin, float org)
    {
        var name = "arrow_head";

        skin.AddBone(name, bone.TYPE.ARROW_HEAD,org);

        return skin;
    }
    #endregion

    //汎用矢印のスキン作成
    public Skin CreateUniversalArrowSkin(float line_width, float base_len, int base_divnum, float arrow_width)
    {
        var skin = SKIN_0_createRootSkin(0);

        skin     = SKIN_1_addRootShaft(skin,line_width,base_len,skin.lenght);                    

        skin     = SKIN_2_addJointSkin(skin,"j1",line_width, base_len,skin.lenght,base_divnum);  

        skin     = SKIN_2_addJointSkin(skin,"j2",line_width, base_len,skin.lenght,base_divnum); 

        skin     = SKIN_3_addArrowRootSkin(skin,arrow_width,base_len,base_len, skin.lenght);      

        skin     = SKIN_4_addArrowHeadSkin(skin, skin.lenght);

        return skin;
    }

    //１方向矢印のスキン作成
    public Skin CreateOneWayArrowSkin(float line_width, float base_len,float arrow_width)
    {
        var skin = SKIN_0_createRootSkin(0);

        skin     = SKIN_1_addRootShaft(skin,line_width,base_len,skin.lenght);                    

        skin     = SKIN_3_addArrowRootSkin(skin,arrow_width,base_len,line_width, skin.lenght);      

        skin     = SKIN_4_addArrowHeadSkin(skin, skin.lenght);

        return skin;
    }
    public void CreateMesh(Skin skin, GameObject go)
    {
        // ref https://docs.unity3d.com/jp/540/ScriptReference/Mesh-bindposes.html

        var anim = go.AddComponent<Animation>();
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

    //---
    public static GameObject CreateTest1()
    {
        var p = new Arrow();
        var skin = p.CreateUniversalArrowSkin(0.5f,1,1,1);
        
        var go = new GameObject("TEST1");
        p.CreateMesh(skin,go);

        return go;
    }
    public static GameObject CreateTest2()
    {
        var p = new Arrow();
        var skin = p.CreateOneWayArrowSkin(0.5f,1,1);
        
        var go = new GameObject("TEST2");
        p.CreateMesh(skin,go);

        return go;
    }


}
