using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_spinner : MonoBehaviour {

    public float spinSpeed = 1.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<RectTransform>().Rotate(0, 0, spinSpeed * Time.deltaTime);
	}
}
