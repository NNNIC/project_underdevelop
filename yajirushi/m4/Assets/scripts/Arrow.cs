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
                var b = IsEqualVector3(v.v, vec3);
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
    private poligon POL_CreateRectangle(poligon pol, string poligon_name, float width, float len, Vector3 org, float angle_y)
    {
        //pol.Begin(poligon_name);
        //{
        //    var a = "a";
        //    var b = "b";
        //    var c = "c";
        //    var d = "d";

        //    var wh = width / 2.0f;

        //    pol.AddVertex(a,-wh,0,org);
        //    pol.AddVertex(b,-wh,0,org + len);
        //    pol.AddVertex(c, wh,0,org + len);
        //    pol.AddVertex(d, wh,0,org);

        //    pol.AddTriIndex(a,b,c);
        //    pol.AddTriIndex(c,d,a);

        //    pol.End();
        //}
        //return pol;

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

    #region スキン作成用
    private Skin SKIN_createRoot(float width) 
    {
        var skin = new Skin();

        skin.AddBone("root",Vector3.zero,0);

        skin.shaft_width = width;
        skin.next_angle_y = 0;
        skin.next_org = Vector3.zero;

        return skin;
    }
    private Skin SKIN_addShaft(Skin skin, string id, float len)
    {
        var name    = id + "_shaft";
        var width   = skin.shaft_width;
        var org     = skin.next_org;
        var angle_y = skin.next_angle_y;

        var pol = POL_CreateRectangle(skin.pol, name,width,len, org,angle_y);

        skin.AddBone(name,org,angle_y);

        skin.SetWeight(name,name);

        skin.next_org = org + (Vector3)(Matrix4x4.TRS(Vector3.zero,Quaternion.Euler(0,angle_y,0),Vector3.one) * Vector3.forward);

        return skin;
    }
    private Skin SKIN_addCurve90(Skin skin, string id, float radius, int divnum, bool bRev)
    {
        var name    = id + "_curve90";
        var width   = skin.shaft_width;
        var org     = skin.next_org;
        var angle_y = skin.next_angle_y;

        poligon pol = null;

        if (bRev)
        {
            pol = POL_CreateLeftCurve(skin.pol,name,width,radius,angle_y,org,divnum);
            skin.AddBone(name,org,angle_y);
            skin.SetWeight(name,name);
            skin.next_org = org + (Vector3)(Matrix4x4.TRS(Vector3.zero,Quaternion.Euler(0,angle_y,0),Vector3.one) * (new Vector3(-radius,0,radius)));
            skin.next_angle_y -= 90;
        }
        else
        {
            pol = POL_CreateRightCurve(skin.pol,name,width,radius,angle_y,org,divnum);
            skin.AddBone(name,org,angle_y);
            skin.SetWeight(name,name);
            skin.next_org = org + (Vector3)(Matrix4x4.TRS(Vector3.zero,Quaternion.Euler(0,angle_y,0),Vector3.one) * (new Vector3( radius,0,radius)));
            skin.next_angle_y += 90;
        }
        
        return skin;
    }
  
    private Skin SKIN_addArrowRoot(Skin skin, float width, float len)
    {
        var name       = "arrow";
        var shaftwidth = skin.shaft_width;
        var org        = skin.next_org;
        var angle_y    = skin.next_angle_y;

        var pol = POL_CreateArrowRoot(skin.pol, name,width,len,shaftwidth,angle_y,org);

        skin.AddBone(name, org, angle_y);

        skin.SetWeight(name,name);
        
        skin.next_org += (Vector3)(Matrix4x4.TRS(Vector3.zero,Quaternion.Euler(0,angle_y,0),Vector3.one) * Vector3.forward);

        return skin;
    }
    private Skin SKIN_addArrowHead(Skin skin)
    {
        var name       = "arrow_head";
        var org        = skin.next_org;
        var angle_y    = skin.next_angle_y;

        skin.AddBone(name, org, angle_y);

        skin.next_org = (Vector3)(Matrix4x4.TRS(Vector3.zero,Quaternion.Euler(0,angle_y,0),Vector3.one) * Vector3.forward);

        return skin;
    }
    #endregion

    //一方向矢印のスキン作成
    public Skin Create_OneWayArrow(float shaft_width, float unit_len, int unit_divnum, float arrow_width)
    {
        var skin = SKIN_createRoot(shaft_width);

        skin = SKIN_addShaft(skin,"shaft",unit_len);

        skin = SKIN_addArrowRoot(skin,arrow_width,unit_len);

        skin = SKIN_addArrowHead(skin);

        return skin;
    }

    //右方向矢印のスキン作成
    public Skin Create_RightCurveArrow(float shaft_width, float unit_len, int unit_divnum, float arrow_width)
    {
        var skin = SKIN_createRoot(shaft_width);

        skin = SKIN_addShaft(skin,"shaft1",unit_len);

        skin = SKIN_addCurve90(skin,"curve1",1,10,false);

        skin = SKIN_addShaft(skin,"shaft2",unit_len);

        skin = SKIN_addArrowRoot(skin,arrow_width,unit_len);

        skin = SKIN_addArrowHead(skin);

        return skin;
    }

    public Skin Create_LeftCurveArrow(float shaft_width, float unit_len, int unit_divnum, float arrow_width)
    {
        var skin = SKIN_createRoot(shaft_width);

        skin = SKIN_addShaft(skin,"shaft1",unit_len);

        skin = SKIN_addCurve90(skin,"curve1",1,10,true);

        skin = SKIN_addShaft(skin,"shaft2",unit_len);

        skin = SKIN_addArrowRoot(skin,arrow_width,unit_len);

        skin = SKIN_addArrowHead(skin);

        return skin;
    }
    public Skin Create_UTurnArrow(float shaft_width, float unit_len, int unit_divnum, float arrow_width)
    {
        var skin = SKIN_createRoot(shaft_width);

        skin = SKIN_addShaft(skin,"shaft1",unit_len);

        skin = SKIN_addCurve90(skin,"curve1",1,10,false);

        skin = SKIN_addShaft(skin,"shaft2",unit_len);

        skin = SKIN_addCurve90(skin,"curve2",1,10,false);

        skin = SKIN_addShaft(skin,"shaft3",unit_len);

        skin = SKIN_addArrowRoot(skin,arrow_width,unit_len);

        skin = SKIN_addArrowHead(skin);

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
    public static GameObject CreateOneWay()
    {
        var p = new Arrow();
        var skin = p.Create_OneWayArrow(0.5f,1,10,1);
        
        var go = new GameObject("ONE WAY");
        p.CreateMesh(skin,go);

        return go;
    }
    public static GameObject CreateRightCurve()
    {
        var p = new Arrow();
        var skin = p.Create_RightCurveArrow(0.5f,1,10,1);
        
        var go = new GameObject("RIGHT CURVE");
        p.CreateMesh(skin,go);

        return go;
    }
    public static GameObject CreateLeftCurve()
    {
        var p = new Arrow();
        var skin = p.Create_LeftCurveArrow(0.5f,1,10,1);
        
        var go = new GameObject("LEFT CURVE");
        p.CreateMesh(skin,go);

        return go;
    }
    public static GameObject CreateUTern()
    {
        var p = new Arrow();
        var skin = p.Create_UTurnArrow(0.5f,1,10,1);
        
        var go = new GameObject("U TERN");
        p.CreateMesh(skin,go);

        return go;
    }
    //public static GameObject CreateTest2()
    //{
    //    var p = new Arrow();
    //    var skin = p.CreateOneWayArrowSkin(0.5f,1,1);
        
    //    var go = new GameObject("TEST2");
    //    p.CreateMesh(skin,go);

    //    return go;
    //}


}
