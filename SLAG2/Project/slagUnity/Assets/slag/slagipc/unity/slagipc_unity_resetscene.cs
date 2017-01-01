using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class slagipc_unity_resetscene : MonoBehaviour {

    // Use this for initialization
    IEnumerator Start()
    {
        yield return null;
        SceneManager.LoadScene("remote");
    }
}
