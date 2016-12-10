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
    private bool jump = false;
    private bool jumped = false;
    private bool previouslyGrounded = false;

    float offsetYaw = 0.0f;
    float offsetPitch = 0.0f;
    float fov = 90.0f;
    
    bool runMode = false;

    Vector3 move;

    float footstepTimer = 0.0f;

    float moveSpeed
    {
        get
        {
            return (runMode ? baseRunSpeed : baseWalkSpeed);
        }
    }

    #endregion

    // Use this for initialization
    void Start()
    {
        //view.transform.localPosition = new Vector3(0, cameraOffset, 0);
        UpdateFOV();
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Look();

        runMode = Input.GetKey(KeyCode.LeftShift);

        controller.Move(move * Time.deltaTime);

        if (!jump)
        {
            jump = Input.GetButtonDown("Jump");
        }

        if (controller.isGrounded)
        {
            GroundMove();
        }
        else
        {
            AirMove(move.y);
            //jump = false;
        }

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
        gameObject.transform.Rotate(0, Input.GetAxis("Mouse X"), 0);
        offsetPitch = Mathf.Clamp(offsetPitch - Input.GetAxis("Mouse Y"), maxAimDown, maxAimUp);
        view.transform.eulerAngles = new Vector3(offsetPitch, gameObject.transform.eulerAngles.y, 0);
    }

    void GroundMove()
    {
        move = controller.velocity;
        move /= friction;
        if (jumped)
        {
            jump = false;
            jumped = false;
        }

        if (jump)
        {
            jumped = true;
            move.y = jumpHeight;
        }
        else
        {
            move.y = 0;
        }
        move += (gameObject.transform.forward * Input.GetAxis("Vertical") + gameObject.transform.right * Input.GetAxis("Horizontal")).normalized * (moveSpeed * 0.2f);
        move = Vector3.ClampMagnitude(move, moveSpeed);
        //Footsteps();   
    }

    void AirMove(float oldy)
    {
        move = controller.velocity;        
        move.y = 0;
        move += (gameObject.transform.forward * Input.GetAxis("Vertical") + gameObject.transform.right * Input.GetAxis("Horizontal")).normalized * airControl;
        move = Vector3.ClampMagnitude(move, airSpeedMax);
        float movy = oldy + ((Physics.gravity.y / 80));
        if (movy < 0)
        {
            movy -= ((Physics.gravity.y / 80)) * gravityMultiplier;
            // Only slow down when falling, this is controlled in wallrun.
        }
        move.y = Mathf.Clamp(movy, -terminalVelocity, terminalVelocity);
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
                footstepTimer += Time.deltaTime;
            }
        }
    }

    void UpdateFOV()
    {
        view.fieldOfView = fov;
    }

}
