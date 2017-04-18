using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class m1_move_dst : MonoBehaviour {

    public float m_speed=300;
    public Arrow[] m_arrows;

	// Use this for initialization
	void Start () {
        var root = GameObject.Find("/root");
        if (root!=null)
        {
            m_arrows = root.GetComponentsInChildren<Arrow>();
        }
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround(Vector3.zero,Vector3.up,m_speed*Time.deltaTime);
        if (m_arrows!=null)
        {
            Array.ForEach(m_arrows,i=> {
                var h = i.GetHead();
                if (h!=null)
                {
                    h.transform.position = transform.position;
                }
            });
        }
	}
}
