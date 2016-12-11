using System.Collections;
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

    [SerializeField]
    Camera view;

    [SerializeField]
    float WallRunMaxTime = 5.0f;
    //float runTime;

    [SerializeField]
    float WallRunDistance = 0.5f;

    //[SerializeField]
    //FirstPersonController firstPersonController;

    //float orginalGravity;
    //float wallRunGravity;

    public bool canWallRun = true;
    public float wallRunTimer = 0.0f;

    //Ray ray;
    public bool wasWallRunning = false;
    public bool isWallRunning = false;
    public bool isWallRunningLeft = false;
    RaycastHit hit = new RaycastHit();

    public Vector3 lastWallNormal = new Vector3(0,0,0);
    //GameObject lastWall = null;

    void Awake()
    {
        //orginalGravity = firstPersonController.m_StickToGroundForce;
        //wallRunGravity = firstPersonController.m_StickToGroundForce * 0.1f;
    }

    void TickRunState()
    {
        lastWallNormal = hit.normal;
        isWallRunning = false;
        playerController.CamTilt(0.0f);
        playerController.gravityMultiplier = 0;
        //wallRunTimer += Time.deltaTime;
        //if (wallRunTimer >= 0.2f)
        //{
            canWallRun = true;          
            //wallRunTimer = 0.0f;
        //}
    }

    void Update()
    {
        if (!canWallRun)
        {
            TickRunState();
            return;
        }
        //else
        //{
        //    wallRunTimer = 0.0f;
        //}

        bool dLeft = DetectLeftWall(); //Better than raycasting 500 times per frame
        bool dRight = DetectRightWall();

        if ((!(dRight && dLeft) && !characterController.isGrounded && canWallRun)
            && ((dLeft || dRight))) //What the?
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                canWallRun = false;
            }

            if (!wasWallRunning)
            {
                if (playerController.move.y < 0)
                {
                    playerController.move.y = 0;
                }
            }

            playerController.gravityMultiplier = 0.97f;

            playerController.StickToWall(hit.normal);

            if (dLeft)
            {
                if (Input.GetKeyDown(KeyCode.D))
                {
                    canWallRun = false;
                }
                playerController.CamTilt(5.0f);
            }
            else if (dRight)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    canWallRun = false;
                }
                playerController.CamTilt(-5.0f);
            }
            //runTime += Time.deltaTime; //Doesnt work
            isWallRunning = true;
            isWallRunningLeft = dLeft;
            playerController.doubleJumped = false;
            // BTW, this is my Unity Player Controller Setup: Walk 20, Run 40, Air Control 6, Terminal 20, Air Max 6, Friction 2, Jump Height 5, Gravity Multiplier 0
        }
        else
        {
            isWallRunning = false;
            playerController.gravityMultiplier = 0.0f;
            playerController.CamTilt(0.0f);
        }

        wasWallRunning = isWallRunning;
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
        Debug.DrawLine(player.transform.position, player.transform.right * WallRunDistance + player.transform.position, Color.magenta);        

        if (Physics.Linecast(player.transform.position, player.transform.right * WallRunDistance + player.transform.position, out hit))
        {
            if (hit.collider.material.name != notWall.name + " (Instance)" && hit.normal != lastWallNormal)
            {
                lastWallNormal = new Vector3(0,0,0);
                return true;
            }
        }
        return false;
    }

    bool DetectLeftWall()
    {
        Debug.DrawLine(player.transform.position, -player.transform.right * WallRunDistance + player.transform.position, Color.green);

        if (Physics.Linecast(player.transform.position, -player.transform.right * WallRunDistance + player.transform.position, out hit))
        {
            if (hit.collider.material.name != notWall.name + " (Instance)" && hit.normal != lastWallNormal)
            {
                lastWallNormal = new Vector3(0,0,0);
                return true;
            }
        }
        return false;
    }
}
