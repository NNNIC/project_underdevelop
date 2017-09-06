using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CameraManager : MonoBehaviour {

    public Camera     m_mainCam;
    public GameObject m_depot;

    IEnumerator       m_ertr;

	void Start () {
		
	}
	
	void Update () {
		if (m_ertr!=null) m_ertr.MoveNext();
	}

    void Enumrator_set(IEnumerator func)
    {
        m_ertr = func;
    }

}
