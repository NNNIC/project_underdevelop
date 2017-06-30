using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using formapp;

public class Test : MonoBehaviour {

    [MenuItem("Test/Test 1")]
	static void TEST1() {
		
        var p = new Class1();
        p.ShowMessageBox();
	}
    [MenuItem("Test/Test 2")]
	static void TEST2() {
		
        var p = new Class1();
        p.ShowSelected();
	}

    [MenuItem("Test/Test 3")]
	static void TEST3() {
		
        var p = new Class1();
        p.ShowForm();
                   
	}
	
}
