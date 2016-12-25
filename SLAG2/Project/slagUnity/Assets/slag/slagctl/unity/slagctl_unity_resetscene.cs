using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class slagctl_unity_resetscene : MonoBehaviour {

    // Use this for initialization
    IEnumerator Start()
    {
        yield return null;
        SceneManager.LoadScene("remote");
    }
}
