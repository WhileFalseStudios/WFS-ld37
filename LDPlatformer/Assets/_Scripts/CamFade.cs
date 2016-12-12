using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFade : MonoBehaviour {

    public Color desiredColor = new Color(0, 0, 0, 0);
    Color curCol = new Color(0, 0, 0, 1);

    public GUISkin skin;

    public float moveSpeed;

    Texture2D bg;

	// Use this for initialization
	void Start () {
		bg = new Texture2D(2, 2);
    }

    public void FadeTo(Color col, float speed)
    {
        desiredColor = col;
        moveSpeed = speed;
    }

    public void SetColor(Color newcol)
    {
        desiredColor = newcol;
    }

    public void FadeBlack()
    {
        desiredColor = new Color(0, 0, 0, 1);
        moveSpeed = 0.005f;
    }
	
	// Update is called once per frame
	void Update () {
        curCol = Vector4.MoveTowards(curCol, desiredColor, moveSpeed);
        if (curCol != desiredColor)
        {            
            bg.SetPixels(new Color[] { curCol, curCol, curCol, curCol });
        }
	}

    void OnGUI()
    {
        //GUI.skin = skin;
        GUI.color = curCol;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), bg);
        //GUI.TextArea(new Rect(10, 10, 200, 50), curCol.ToString());
    }

}
