using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClimb : MonoBehaviour {

    [SerializeField]
    GameObject player;
    [SerializeField]
    Camera mainCam;
    [SerializeField]
    Camera climbCam;
    [SerializeField]
    Animator anim;
    //[SerializeField]
    //Rigidbody rb;
    [SerializeField]
    PlayerController playerController;

    //Ray emptyRay; // Hit above the wall to check for obstructions;
    //Ray ray; // Hit the wall in front to see if we are climbing
    //RaycastHit hitempty = new RaycastHit();
    RaycastHit hit = new RaycastHit();
    Vector3 checkLocation;
    Vector3 eyePosition;
    bool inAnimation = false;
    public PhysicMaterial notWall;
   

    bool isClimbed;
    	
	// Update is called once per frame
	void Update ()
    {
        //Debug.Log(CanClimb());
        if (Input.GetKey(KeyCode.W) && CanClimb() && !inAnimation)
        {
            //mainCam.SetActive(false);
            //climbCam.SetActive(true);
            climbCam.depth = 1;
            mainCam.depth = 0;
            //mainCam.transform.eulerAngles = new Vector3(0.0f, mainCam.transform.rotation.y, mainCam.transform.rotation.z);
            //playerController.offsetPitch = 0.0f;
            playerController.enabled = false;
            //rb.isKinematic = true;
            inAnimation = true;
            anim.SetTrigger("Climb");
            StartCoroutine(afterClimb());
            //Debug.Log(checkLocation.ToString());
            //player.transform.position += checkLocation;
        }
    }

    IEnumerator afterClimb()
    {
        yield return new WaitForSeconds(1f);
        //mainCam.SetActive(true);
        //climbCam.SetActive(false);
        player.transform.position = climbCam.transform.position - new Vector3(0, 0.5f, 0);
        //playerController.offsetPitch = 0.0f;
        //mainCam.transform.eulerAngles = new Vector3(0.0f, mainCam.transform.rotation.y, mainCam.transform.rotation.z);
        mainCam.depth = 1;
        climbCam.depth = 0;
        playerController.enabled = true;
        //Debug.Log(mainCam.transform.eulerAngles.ToString());
        //Debug.LogWarning(mainCam.transform.rotation.ToString());
        //mainCam.transform.localEulerAngles = new Vector3(0, 0, 0);
        //Debug.Log(mainCam.transform.eulerAngles.ToString());
        //Debug.LogWarning(mainCam.transform.rotation.ToString());
        //rb.isKinematic = false;
        inAnimation = false;

    }

    bool CanClimb()
    {
        // checkLocation = Vector3.forward + new Vector3(0, 1, 0);
        eyePosition = player.transform.position + new Vector3(0, 0.5f, 0);
        checkLocation = player.transform.forward + new Vector3(0, 1.5f, 0);

        //emptyRay = new Ray(player.transform.position, player.transform.TransformDirection(checkLocation));
        //Debug.DrawRay(player.transform.position, player.transform.TransformDirection(checkLocation), Color.red);


        // ray = new Ray(player.transform.position, player.transform.TransformDirection(Vector3.forward));
        // Debug.DrawRay(player.transform.position, player.transform.TransformDirection(Vector3.forward), Color.blue);
        Debug.DrawLine(eyePosition, eyePosition + player.transform.forward * 1, Color.blue);
        Debug.DrawLine(eyePosition, eyePosition + player.transform.forward * 1 + new Vector3(0, 1.5f, 0), Color.red);

        if (Physics.Linecast(eyePosition, eyePosition + player.transform.forward * 1, out hit) && !Physics.Linecast(eyePosition, eyePosition + player.transform.forward * 1 + new Vector3(0, 1.5f, 0)))
        {
            //Debug.Log(hit.collider.material.name.ToString());
            //Debug.LogWarning(notWall.name.ToString());
            if (hit.collider.material.name != notWall.name + " (Instance)")
            {
                return true;
            }
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
