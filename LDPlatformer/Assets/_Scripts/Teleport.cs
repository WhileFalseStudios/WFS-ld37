using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {

    [SerializeField]
    GameObject[] spawnPoints = new GameObject[2];

    [SerializeField]
    GameObject player;

    int currentRoom = 0;

	void Update () {
		if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TeleportToRoom(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TeleportToRoom(1);
        }
	}

    void TeleportToRoom(int roomNumber)
    {
        if (currentRoom == roomNumber)
        {
            return;
            // Fizzes
        }

        Vector3 relativeLocation = spawnPoints[currentRoom].transform.InverseTransformPoint(player.transform.position);
        player.transform.position = spawnPoints[roomNumber].transform.position + relativeLocation;

        currentRoom = roomNumber;
    }
}
