using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {

    [SerializeField]
    GameObject[] spawnPoints = new GameObject[2];
    [SerializeField]
    GameObject player;
    int currentRoom = 0;
    public bool canTeleport = true;

	void Update () {
        if (Input.GetKeyDown(KeyCode.Q) && canTeleport)
        {
            TeleportToRoom();
        }
	}

    void TeleportToRoom()
    {
        int roomNumber = 1 - currentRoom;
        Vector3 relativeLocation = spawnPoints[currentRoom].transform.InverseTransformPoint(player.transform.position);
        player.transform.position = spawnPoints[roomNumber].transform.position + relativeLocation;
        currentRoom = roomNumber;
    }

    void OnCollisionEnter()
    {
        Debug.Log("DED");
    }
}
