using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {

    [SerializeField]
    GameObject[] spawnPoints = new GameObject[2];
    [SerializeField]
    GameObject[] backgroundScenery = new GameObject[2];
    [SerializeField]
    GameObject player;
    int currentRoom = 0;
    public bool canTeleport = true;

    void Awake()
    {
        backgroundScenery[currentRoom].SetActive(true);
        backgroundScenery[1 - currentRoom].SetActive(false);
    }

	void Update () {
        if (Input.GetKeyDown(KeyCode.Q) && canTeleport)
        {
            TeleportToRoom();
        }
	}

    void TeleportToRoom()
    {
        //Todo: check if we would warp into something when teleporting - do a capsuletrace at the offset and see if it hits anything.

        int roomNumber = 1 - currentRoom;
        Vector3 relativeLocation = spawnPoints[currentRoom].transform.InverseTransformPoint(player.transform.position);
        Vector3 rotDiff = spawnPoints[0].transform.eulerAngles - spawnPoints[1].transform.eulerAngles;
        Debug.Log(rotDiff);        
        Vector3 tempOs = spawnPoints[roomNumber].transform.position + relativeLocation;
        player.transform.position = RotatePointAroundPivot(tempOs, spawnPoints[roomNumber].transform.position, rotDiff);
        backgroundScenery[roomNumber].SetActive(true);
        backgroundScenery[currentRoom].SetActive(false);
        currentRoom = roomNumber;
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
