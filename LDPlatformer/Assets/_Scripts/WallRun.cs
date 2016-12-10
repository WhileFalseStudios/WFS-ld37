using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class WallRun : MonoBehaviour {

    [SerializeField]
    GameObject player;
    [SerializeField]
    CharacterController characterController;
    //[SerializeField]
    //FirstPersonController firstPersonController;

    float orginalGravity;
    float wallRunGravity;

    Ray ray;
    RaycastHit hit = new RaycastHit();

    void Awake()
    {
        //orginalGravity = firstPersonController.m_StickToGroundForce;
        //wallRunGravity = firstPersonController.m_StickToGroundForce * 0.1f;
    }

    void Update()
    {
        //if (!characterController.isGrounded && Input.GetKeyDown(KeyCode.W))
        //{
            if (DetectLeftWall())
            {
                WallRunLeft();
            }
            else
            {
                //firstPersonController.m_StickToGroundForce = orginalGravity;
            }
        //}
    }

    bool DetectLeftWall()
    {
        ray = new Ray(player.transform.position, transform.TransformDirection(Vector3.left));
        Debug.DrawRay(player.transform.position, transform.TransformDirection(Vector3.left), Color.white);
        if (Physics.Raycast(ray.origin, ray.direction, out hit, 1.0f))
        {
            if (hit.collider.tag == "Wall")
            {
                Debug.Log(hit.collider.name);
                Debug.Log(hit.distance);
                return true;
            }
        }
        return false;
    }

    void WallRunLeft()
    {
        // Do animation
        //firstPersonController.m_StickToGroundForce = wallRunGravity;
    }
}
