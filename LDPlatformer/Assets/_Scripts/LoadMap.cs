using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMap : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public void Load(string map)
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(map);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
