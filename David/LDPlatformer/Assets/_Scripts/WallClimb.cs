using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClimb : MonoBehaviour {

    [SerializeField]
    GameObject player;

    Ray emptyRay; // Hit above the wall to check for obstructions;
    Ray ray; // Hit the wall in front to see if we are climbing
    RaycastHit hit = new RaycastHit();
    	
	// Update is called once per frame
	void Update ()
    {
        //if (Input.GetKey(KeyCode.W))
        //{
            CanClimb();
        //}
    }

    void CanClimb()
    { 
        Vector3 checkLocation = Vector3.forward + new Vector3(0, 3, 0);

        emptyRay = new Ray(player.transform.position, player.transform.TransformDirection(checkLocation));
        Debug.DrawRay(player.transform.position, player.transform.TransformDirection(checkLocation), Color.red);


        ray = new Ray(player.transform.position, player.transform.TransformDirection(Vector3.forward));
        Debug.DrawRay(player.transform.position, player.transform.TransformDirection(Vector3.forward), Color.blue);

        if (Physics.Raycast(ray.origin, ray.direction, out hit, 1.0f))
        {
            if (!Physics.Raycast(emptyRay.origin, emptyRay.direction, 1.0f))
            {
                Debug.Log("Can Climb");
            }
        }
    }
}
