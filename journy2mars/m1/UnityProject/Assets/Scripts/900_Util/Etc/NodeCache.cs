using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NodeCache {

    private static Hashtable m_hash = new Hashtable();

    private Guid      m_guid;
    private NodeCache() { }

    public static NodeCache Create(string nodepath)
    {
        var nc = new NodeCache();
        nc.m_guid = Guid.NewGuid();
        
        m_hash[nc.m_guid] = GameObject.Find(nodepath);

        return nc;
    }    

    public static implicit operator GameObject(NodeCache i) {
        return (GameObject)m_hash[i.m_guid];
    }
}
