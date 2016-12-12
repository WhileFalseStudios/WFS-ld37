using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_loadmap : MonoBehaviour {

    public GameObject LoadingUI;
    public Text loadingText;

    public string loadMessage = "Loading {0}%...";

	// Use this for initialization
	void Start () {
		
	}

    public void LoadMap(string map)
    {
        LoadingUI.SetActive(true);
        StartCoroutine(LoadAsyncMap(map));
    }

    IEnumerator LoadAsyncMap(string map)
    {
        yield return null;

        var load = SceneManager.LoadSceneAsync(map, LoadSceneMode.Single);
        load.allowSceneActivation = false;

        while (!load.isDone)
        {
            loadingText.text = string.Format(loadMessage, (load.progress * 100).ToString("#"));

            if (load.progress >= 0.9f)
            {
                loadingText.text = string.Format("Press any key to continue...");
                if (Input.anyKey)
                    load.allowSceneActivation = true;
            }

            yield return null;

        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
