using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float playerHeight = 2f;
    Rigidbody rb;
    [SerializeField] Transform orientation;
    [SerializeField] PhysicMaterial playerPMat;

    [Header("Movement")]
    public float moveSpeed = 6f;
    public float gravityForce = 20f;
    float movementMultiplier = 10f;
    [SerializeField] float airMultiplier = 0.4f;

    [Header("Sprinting")]
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float sprintSpeed = 6f;
    [SerializeField] float acceleration = 10f;

    [Header("Jumping")]
    public float aditionalJumps = 1;
    public float jumpForce = 5f;
    float storeJumps;

    [Header("Ground Detection")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float groundDistance = 0.1f;
    [SerializeField] bool isGrounded; 

    [Header("Drag")]
    public float groundDrag = 6f;
    public float airDrag = 2f;

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;


    float horizontalMovement;
    float verticalMovement;    

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    RaycastHit slopeHit;
    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        storeJumps = aditionalJumps;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        //isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.1f );
        //print(isGrounded);

        MyInput();
        ControlDrag();
        ControlSpeed();

        if (isGrounded) //se ejecuta todo el tiempo, a lo mejor con un lock tiene mejor performance
        {
            aditionalJumps = storeJumps;
        }
        if (Input.GetKeyDown(jumpKey))
        {
            if (aditionalJumps == 0 && isGrounded) //isGrounded le da limite de saltos, sino es infinito.
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                Jump();
            } 
            else
            if (aditionalJumps != 0) //Si hay multiples saltos no se checkea isGrounded.
            {
                Jump();
                aditionalJumps -= 1;
            }          
        }

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;

    }

    void Jump()
    {
        //With ForceMode.Impulse the jump height will be dependent on mass,
        //if this isn't what you want, change Impulse to VelocityChange
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void ControlSpeed()
    {
        if (Input.GetKey(sprintKey) && isGrounded)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
        }
    }

    void ControlDrag()
    {
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else if (!isGrounded)
        {
            rb.drag = airDrag;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer(); 
    }

    void MovePlayer()
    {
        if (isGrounded && !OnSlope())
        {
            playerPMat.dynamicFriction = 0;
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (isGrounded && OnSlope())
        {
            playerPMat.dynamicFriction = 1; //Si se desea que el player no baje solo las pendientes
            //Aunque esto agrega la necesidad de detectar la pendiente y fijar un limite
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
            rb.AddForce(-transform.up * gravityForce, ForceMode.Acceleration);
        }
    }


}
