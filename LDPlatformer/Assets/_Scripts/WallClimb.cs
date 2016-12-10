using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClimb : MonoBehaviour {

    [SerializeField]
    GameObject player;

    //Ray emptyRay; // Hit above the wall to check for obstructions;
    //Ray ray; // Hit the wall in front to see if we are climbing
    //RaycastHit hit = new RaycastHit();
    //RaycastHit hitempty = new RaycastHit();
    Vector3 checkLocation;

    bool isClimbed;
    	
	// Update is called once per frame
	void Update ()
    {
        //Debug.Log(CanClimb());
        if (CanClimb())
        {
            //Debug.Log(checkLocation.ToString());
            player.transform.position += checkLocation;
        }
    }

    bool CanClimb()
    {
        // checkLocation = Vector3.forward + new Vector3(0, 1, 0);

        checkLocation = player.transform.forward + new Vector3(0, 1, 0);

        //emptyRay = new Ray(player.transform.position, player.transform.TransformDirection(checkLocation));
        //Debug.DrawRay(player.transform.position, player.transform.TransformDirection(checkLocation), Color.red);


       // ray = new Ray(player.transform.position, player.transform.TransformDirection(Vector3.forward));
       // Debug.DrawRay(player.transform.position, player.transform.TransformDirection(Vector3.forward), Color.blue);

        Debug.DrawLine(player.transform.position, player.transform.forward * 1 + player.transform.position, Color.blue);
        Debug.DrawLine(player.transform.position, player.transform.position + (player.transform.forward * 1 + new Vector3(0, 1, 0)), Color.red);

        if (Physics.Linecast(player.transform.position, player.transform.forward * 1 + player.transform.position) && !Physics.Linecast(player.transform.position, player.transform.position + (player.transform.forward * 1 + new Vector3(0, 1, 0))))
        {
            return true;
        }

        return false;

       /* if (Physics.Raycast(ray.origin, ray.direction, out hit, 1.0f)
            && !Physics.Raycast(emptyRay.origin, emptyRay.direction, out hitempty, 1.0f))
        {
            Debug.Log(hitempty.point.ToString());

            if (hit.collider.tag == "Wall")
            {
                return true;
            }
        }
        return false; */
    }
}
