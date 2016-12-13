using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Credits : MonoBehaviour {

    public TextAsset text;

	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.None;
        GetComponent<Text>().text = text.text;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
