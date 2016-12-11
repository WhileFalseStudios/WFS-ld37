using UnityEngine;
using System.Collections;
[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Player Controller")]
public class PlayerController : MonoBehaviour
{
    #region Declarations
    [Header("Properties")]
    public float cameraOffset = 0.5f;
    public float maxAimUp = 90.0f;
    public float maxAimDown = -80.0f;
    [Header("Components")]
    public Camera view;
    public CharacterController controller;
    [Header("Footsteps")]
    public AudioSource footstepSource;
    public AudioClip[] footstepSounds;
    public float footstepSpeed = 0.3f;
    public float footstepVolumeScale = 1.0f;
    [Header("Movement Properties")]
    public float baseWalkSpeed = 1.0f;
    public float baseRunSpeed = 2.5f;
    public float airControl = 1.0f;
    public float terminalVelocity = 6.0f;
    public float airSpeedMax = 3.0f;
    public float friction = 0.8f;
    public float jumpHeight = 2.0f;
    public float gravityMultiplier = 0.0f;
    public bool stopSnapping = false;
    public bool doubleJumped = false;
    private bool jump = false;
    private bool jumped = false;
    private bool previouslyGrounded = false;
    public float WallJumpSpeed = 30.0f;


    public float offsetPitch = 0.0f;
    float offsetYaw = 0.0f;
    float rollAngle = 0.0f;
    float curRollAngle = 0.0f;
    float fov = 90.0f;

    bool runMode = false;

    public Vector3 move;

    float footstepTimer = 0.0f;

    float moveSpeed
    {
        get
        {
            return (runMode ? baseRunSpeed : baseWalkSpeed);
        }
    }

    #endregion

    Vector3 oldPos;

    Vector3 velocity;

    [SerializeField]
    public float TiltSpeed = 30.0f;
    
    [SerializeField]
    WallRun wallRunScript;

    //void Awake()
    //{
    //    Application.targetFrameRate = 60;
    //}

    // Use this for initialization
    void Start()
    {
        //view.transform.localPosition = new Vector3(0, cameraOffset, 0);
        UpdateFOV();
    }

    void FixedUpdate()
    {
        //Debug.LogWarning("Previous: " + previouslyGrounded.ToString());
        //Debug.LogError("Is Grounded: " + controller.isGrounded.ToString());
        if ((!controller.isGrounded && previouslyGrounded)
            || (controller.isGrounded && !previouslyGrounded))
        {
            jump = false;
            doubleJumped = false;
        }

        previouslyGrounded = controller.isGrounded;


        velocity = transform.position - oldPos;

        if (controller.isGrounded)
        {
            GroundMove(move.y);
        }
        else
        {
            //Debug.LogError(move.y);
            AirMove(move.y);
            //jump = false;
        }

        if (jump && wallRunScript.isWallRunning)
        {
            move.y = jumpHeight;
            if (wallRunScript.isWallRunningLeft)
            {
                move += transform.right * airControl * WallJumpSpeed;
                wallRunScript.canWallRun = false;
            }
            else
            {
                move -= transform.right * airControl * WallJumpSpeed;
                wallRunScript.canWallRun = false;
            }
            stopSnapping = true;
        }
        else
        {
            stopSnapping = false;
        }

        controller.Move(move * Time.fixedDeltaTime);

        oldPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked; // HACK
        Look();

        runMode = Input.GetKey(KeyCode.LeftShift);    
        jump = Input.GetButtonDown("Jump");

        //if (!previouslyGrounded && controller.isGrounded)
        //{
        //    jump = false;
        //    //jumped = false;
        //    move.y = 0;
        //}

        //previouslyGrounded = controller.isGrounded;
    }

    void Look()
    {
        curRollAngle = Mathf.MoveTowardsAngle(curRollAngle, rollAngle, Time.deltaTime * TiltSpeed);
        gameObject.transform.Rotate(0, Input.GetAxis("Mouse X"), 0);
        offsetPitch = Mathf.Clamp(offsetPitch - Input.GetAxis("Mouse Y"), maxAimDown, maxAimUp);
        view.transform.eulerAngles = new Vector3(offsetPitch, gameObject.transform.eulerAngles.y, curRollAngle);
    }

    void GroundMove(float oldy)
    {
        wallRunScript.lastWallNormal = new Vector3(0, 0, 0);
        wallRunScript.canWallRun = true;
        move = controller.velocity;
        move /= friction;

        if (jump)
        {
            move.y = jumpHeight;
        }
        else
        {
            // move.y = 0;       
            // float movy = oldy + ((Physics.gravity.y * Time.deltaTime));
            move.y = ((Physics.gravity.y * Time.fixedDeltaTime));
        }
                    
        move += (gameObject.transform.forward * Input.GetAxis("Vertical") + gameObject.transform.right * Input.GetAxis("Horizontal")).normalized * (moveSpeed * 0.2f);
        move = Vector3.ClampMagnitude(move, moveSpeed);
        //Footsteps();   
    }

    void AirMove(float oldy)
    {
        move = controller.velocity;
        if (jump && !wallRunScript.isWallRunning && !doubleJumped)
        {
            oldy += jumpHeight * 1.5f;
            doubleJumped = true;
        }
        if (controller.velocity.y < 0.1f && controller.velocity.y > -0.1f && oldy > 0.0f)
        {
            oldy = 0.0f;
        }

        //move.y = 0;
        move += (gameObject.transform.forward * Input.GetAxis("Vertical") + gameObject.transform.right * Input.GetAxis("Horizontal")).normalized * airControl;
        //Debug.LogError(move);
        //move.x = Mathf.Min(move.x, airSpeedMax);
        //move.z = Mathf.Min(move.z, airSpeedMax);
        //move = Vector3.ClampMagnitude(move, airSpeedMax);
        float movy = oldy + ((Physics.gravity.y * Time.fixedDeltaTime));
        if (movy < 0)
        {
            movy -= ((Physics.gravity.y * Time.fixedDeltaTime)) * gravityMultiplier;
            // Only slow down when falling, this is controlled in wallrun.
        }
        move.y = Mathf.Clamp(movy, -terminalVelocity, terminalVelocity);
        move.x = Mathf.Clamp(move.x, -airSpeedMax, airSpeedMax);
        move.z = Mathf.Clamp(move.z, -airSpeedMax, airSpeedMax);
        //Debug.LogWarning(move);
    }
    //Debug.LogWarning(move);

    public void StickToWall(Vector3 wallNormal)
    {
        RaycastHit stick = new RaycastHit();

        Debug.DrawRay(gameObject.transform.position, wallNormal * -1, Color.cyan);
        if (Physics.Raycast(gameObject.transform.position, wallNormal * -1, out stick, 1.0f))
        {
            //gameObject.transform.position += (gameObject.transform.position - stick.point) - new Vector3(controller.radius, 0, controller.radius);
            gameObject.transform.position = stick.point + (wallNormal * (controller.radius + 0.05f));
        }
    }

    public void CamTilt(float angle)
    {
        rollAngle = angle;
    }

    void Footsteps()
    {
        if (controller.isGrounded)
        {
            if (footstepTimer >= 1 / (controller.velocity.magnitude / footstepSpeed))
            {
                footstepTimer = 0;
                footstepSource.PlayOneShot(footstepSounds[Random.Range(0, footstepSounds.Length)], Mathf.InverseLerp(0, baseRunSpeed, controller.velocity.magnitude) * footstepVolumeScale);
            }
            else
            {
                footstepTimer += Time.fixedDeltaTime;
            }
        }
    }

    void UpdateFOV()
    {
        view.fieldOfView = fov;
    }

}
