using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class Teleport : MonoBehaviour {

    [SerializeField]
    GameObject[] spawnPoints = new GameObject[2];
    [SerializeField]
    GameObject[] backgroundScenery = new GameObject[2];
    [SerializeField]
    GameObject player;
    int currentRoom = 0;
    public bool canTeleport = true;

    public AudioSource TeleSound;
    public AudioClip sound;

    bool travelling = false;
    bool hasTravelled = false;

    float travelTimer = 0.0f;

    public float travelTime = 0.4f;

    void Awake()
    {
        backgroundScenery[currentRoom].SetActive(true);
        backgroundScenery[1 - currentRoom].SetActive(false);
    }

	void Update () {
        if (Input.GetKeyDown(KeyCode.Q) && canTeleport && !travelling)
        {            
            TeleportFX();
            travelling = true;
        }
        else if (travelling)
        {
            travelTimer += Time.deltaTime;

            Debug.Log(travelTimer);

            if (travelTimer >= travelTime / 2 && travelTimer < travelTime && !hasTravelled)
            {
                Debug.Log("Travel!");
                TeleportToRoom();
                UnTeleportFX();
                hasTravelled = true;
            }
            if (travelTimer >= travelTime)
            {
                Debug.Log("End travel");
                travelling = false;
                travelTimer = 0.0f;
                hasTravelled = false;
            }
        }
	}

    void TeleportToRoom()
    {
        //Todo: check if we would warp into something when teleporting - do a capsuletrace at the offset and see if it hits anything.

        int roomNumber = 1 - currentRoom;
        Vector3 relativeLocation = spawnPoints[currentRoom].transform.InverseTransformPoint(player.transform.position);
        Vector3 rotDiff = spawnPoints[0].transform.eulerAngles - spawnPoints[1].transform.eulerAngles;
       // Debug.Log(rotDiff);        
        Vector3 tempOs = spawnPoints[roomNumber].transform.position + relativeLocation;
        player.transform.position = RotatePointAroundPivot(tempOs, spawnPoints[roomNumber].transform.position, rotDiff);
        backgroundScenery[roomNumber].SetActive(true);
        backgroundScenery[currentRoom].SetActive(false);
        currentRoom = roomNumber;
    }

    void TeleportFX()
    {
        Camera[] cams = player.GetComponentsInChildren<Camera>();

        TeleSound.PlayOneShot(sound, 0.5f);

        player.GetComponentInChildren<CamFade>().FadeTo(new Color(1, 1, 0.96f, 0.7f), travelTime / 2);
        player.GetComponentInChildren<ParticleSystem>().Play();

        foreach (var cam in cams)
        {            
            if (currentRoom == 1)
            {
                cam.GetComponent<ColorCorrectionLookup>().enabled = true;
                
            }
            else
            {
                cam.GetComponent<ColorCorrectionLookup>().enabled = false;
            }
        }

    }

    void UnTeleportFX()
    {
        player.GetComponentInChildren<CamFade>().FadeTo(new Color(0, 0, 0, 0), travelTime / 2);
    }

    Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        var dir = point - pivot;
        dir = Quaternion.Euler(angles) * dir;
        point = dir + pivot;
        return point;
    }

    void OnCollisionEnter()
    {
        Debug.Log("DED");
    }
}
