﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class WallRun : MonoBehaviour {

    [SerializeField]
    GameObject player;
    [SerializeField]
    CharacterController characterController;
    [SerializeField]
    PhysicMaterial notWall;
    [SerializeField]
    private PlayerController playerController;

    //[SerializeField]
    //FirstPersonController firstPersonController;

    //float orginalGravity;
    //float wallRunGravity;

    //Ray ray;

    RaycastHit hit = new RaycastHit();

    void Awake()
    {
        //orginalGravity = firstPersonController.m_StickToGroundForce;
        //wallRunGravity = firstPersonController.m_StickToGroundForce * 0.1f;
    }

    void Update()
    {
        if (!(DetectRightWall() && DetectLeftWall()))
        {
            if (DetectLeftWall() || DetectRightWall())
            {
                // Since there's no animation yet, I'm just putting them together
                playerController.gravityMultiplier = 0.97f;

                // BTW, this is my Unity Player Controller Setup: Walk 20, Run 40, Air Control 6, Terminal 20, Air Max 6, Friction 2, Jump Height 5, Gravity Multiplier 0
            }
            else
            {
                playerController.gravityMultiplier = 0.0f;
            }
        }
        //if (!characterController.isGrounded && Input.GetKeyDown(KeyCode.W))
        //{
        //if (DetectLeftWall())
        //{
        //    WallRunLeft();
        //}
        //else
        //{
        //    //firstPersonController.m_StickToGroundForce = orginalGravity;
        //}
        //}
    }

    //bool DetectLeftWall()
    //{
    //    ray = new Ray(player.transform.position, transform.TransformDirection(Vector3.left));
    //    Debug.DrawRay(player.transform.position, transform.TransformDirection(Vector3.left), Color.white);
    //    if (Physics.Raycast(ray.origin, ray.direction, out hit, 1.0f))
    //    {
    //        if (hit.collider.tag == "Wall")
    //        {
    //            //Debug.Log(hit.collider.name);
    //            //Debug.Log(hit.distance);
    //            return true;
    //        }
    //    }
    //    return false;
    //}

    //void WallRunLeft()
    //{
    //    // Do animation
    //    //firstPersonController.m_StickToGroundForce = wallRunGravity;
    //}

    bool DetectRightWall()
    {
        Debug.DrawLine(player.transform.position, player.transform.right * 1 + player.transform.position, Color.magenta);

        if (Physics.Linecast(player.transform.position, player.transform.right * 1 + player.transform.position, out hit))
        {
            if (hit.collider.material.name != notWall.name + " (Instance)")
            {
                return true;
            }
        }
        return false;
    }

    bool DetectLeftWall()
    {
        Debug.DrawLine(player.transform.position, -player.transform.right * 1 + player.transform.position, Color.green);

        if (Physics.Linecast(player.transform.position, -player.transform.right * 1 + player.transform.position, out hit))
        {
            if (hit.collider.material.name != notWall.name + " (Instance)")
            {
                return true;
            }
        }
        return false;
    }
}
