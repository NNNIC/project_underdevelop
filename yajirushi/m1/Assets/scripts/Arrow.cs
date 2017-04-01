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
        public string     name;    //名前
        public Vector3    v;       //座標
        public string     bone0;   //所属ボーン0
        public string     bone1;   //所属ボーン1

        public float      weight0; //ウエイト0
        public float      weight1; //ウエイト1

        public int        tmp_index; //メッシュ作成時に使用の一時的インデックス

        public vertex(string iname, float x, float y, float z)
        {
            name = iname;

            v = new Vector3(x,y,z);

            bone0 = null; //未定義
            bone1 = null; //未定義
            weight0 = 1;
            weight1 = 0;
        }
    }

    public class poligon
    {
        public string name;

        public Dictionary<string,vertex> vertex_dic   = new Dictionary<string, vertex>();    //頂点リスト

        public struct trindex {public string idx0, idx1, idx2; }
        public List<trindex>  tri_index_list= new List<trindex>();                           //トライアングルインデックス

        public poligon(string i_name)
        {
            name = i_name;
        }

        public void AddVertex(string name, float x, float y, float z)
        {
            vertex_dic.Add(name, new vertex(name, x,y,z));
        }

        public void AddTriIndex(string n1, string n2, string n3)
        {
            tri_index_list.Add(new trindex() { idx0=n1, idx1=n2, idx2=n3 });
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
                var pol = p.Value;
                pol.tmp_index = index++;
                tmp_list.Add(pol); 
                mesh_vertex_list.Add(pol.v); //頂点
                mesh_uvlist.Add(Vector2.zero);   //UV暫定

                //BoneWeight
                BoneWeight bw = new BoneWeight();
                bw.boneIndex0 = skin.FindBoneIndex(pol.bone0);
                if (pol.bone1!=null)
                {
                    bw.boneIndex1 = skin.FindBoneIndex(pol.bone1);
                }
                bw.weight0 = pol.weight0;
                bw.weight1 = pol.weight1;

                mesh_weight_list.Add(bw);
            }

            foreach(var t in tri_index_list)
            {
                mesh_triangle_list.Add(vertex_dic[t.idx0].tmp_index); // 三角形インデックス
                mesh_triangle_list.Add(vertex_dic[t.idx1].tmp_index); //
                mesh_triangle_list.Add(vertex_dic[t.idx2].tmp_index); //
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
        public List<poligon> pol_list = new List<poligon>();
        public List<bone>    bone_list = new List<bone>();

        public float lenght; //矢印の長さ

        public void AddPoligon(poligon pol)
        {
            pol_list.Add(pol);
        }

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
            var pol = pol_list.Find(i=>i.name == poligon);
            foreach(var p in pol.vertex_dic)
            {
                p.Value.bone0 = bone0;
            }
        }
        public void SetWeight(string poligon, string bone0, string bone1)
        {
            SetWeight(poligon,bone0); //暫定
        }

        //検索
        public int FindBoneIndex(string name)
        {
            return bone_list.FindIndex(i=>i.name == name);
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
    private poligon POL_CreateRectangle(string name, float width, float len, float org)
    {
        var a = "a";
        var b = "b";
        var c = "c";
        var d = "d";

        var wh = width / 2.0f;

        var pol = new poligon(name);
        pol.AddVertex(a,-wh,0,org);
        pol.AddVertex(b,-wh,0,org + len);
        pol.AddVertex(c, wh,0,org + len);
        pol.AddVertex(d, wh,0,org);

        pol.AddTriIndex(a,b,c);
        pol.AddTriIndex(c,d,a);

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
    private poligon POL_CreateRectangles_1xN(string name,float width, float len, float org, int divnum)
    {
        var dl = len / divnum;
        var wh = width / 2.0f;

        var pol = new poligon(name);
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
    private poligon POL_CreateArrowRoot(string name, float width, float height, float shaftwidth, float org)
    {
        var a = "a";
        var b = "b";
        var c = "c";
        var d = "d";
        var e = "e";

        var ahw = width / 2.0f;
        var shw = shaftwidth / 2.0f;

        var pol = new poligon(name);
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

        var pol = POL_CreateRectangle(name,width,len,org);
        skin.AddPoligon(pol);

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
        var joint_1st_pol   = POL_CreateRectangles_1xN(joint_1st_name, width,base_len,org         , each_divnum);
        var joint_2nd_pol   = POL_CreateRectangles_1xN(joint_2nd_name, width,base_len,org+base_len,each_divnum);

        var joint_shuft_pol = POL_CreateRectangle(joint_shaft_name, width,base_len, org+base_len*2 );

        skin.AddPoligon(joint_1st_pol);  //0
        skin.AddPoligon(joint_2nd_pol);  //1
        skin.AddPoligon(joint_shuft_pol);//2

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

        var pol = POL_CreateArrowRoot(name,width,len,shaft_width,org);

        skin.AddPoligon(pol);

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

        foreach(var pol in skin.pol_list)
        {
           pol.MESH_create(skin, ref vlist, ref tlist,ref uvlist, ref bwlist );
        }

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
    public static GameObject CreateTest()
    {
        var p = new Arrow();
        var skin = p.CreateUniversalArrowSkin(0.5f,1,10,1);
        
        var go = new GameObject("TEST");
        p.CreateMesh(skin,go);

        return go;
    }


}
