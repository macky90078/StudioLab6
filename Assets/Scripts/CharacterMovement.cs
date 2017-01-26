using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {

    //public float m_maxSpeed;
    public float walkSpeed = 4;
    public float runSpeed = 6;
    public float gravity = -9.8f;
    public float jumpHeight = 1;

    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;

    public float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;

    [Range(0,1)]
    public float airControlPercent;

    float currentSpeed;
    float velocityY;

    Animator animator;
    Transform cameraT;
    CharacterController controller;

    //public PhysicMaterial m_material;

    //public float m_flatVelocity;

    //Rigidbody m_rb;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        cameraT = Camera.main.transform;
        controller = GetComponent<CharacterController>();
        //m_rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //input
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDirection = input.normalized;
        bool running = Input.GetKey(KeyCode.LeftShift);

        Move(inputDirection, running);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        //animator
        float animationSpeedPercent = ((running) ? currentSpeed / runSpeed : currentSpeed / walkSpeed * 0.5f);
        animator.SetFloat("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);

    }

    void Move(Vector2 inputDir, bool running)
    {
        if (inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
        }

        float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

        velocityY += Time.deltaTime * gravity;
        Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);
        currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;

        if (controller.isGrounded)
        {
            velocityY = 0;
        }

    }

    void Jump()
    {
        if(controller.isGrounded)
        {
            float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
            velocityY = jumpVelocity;
        }
    }

    float GetModifiedSmoothTime(float smoothTime)
    {
        if(controller.isGrounded)
        {
            return smoothTime;
        }
        if(airControlPercent == 0)
        {
            return float.MaxValue;
        }
        return smoothTime / airControlPercent;
    }

    //void FixedUpdate()
    //{
    //    CalculateFlatMovment();
    //    //MovePlayer(m_forward, m_sideward);
    //}

    //void CalculateFlatMovment()
    //{
    //    float fNorm = m_rb.mass * (Physics.gravity.magnitude * -1);
    //    float fDynamic = fNorm * m_material.dynamicFriction;
    //    float acceleration = (fDynamic / m_rb.mass);

    //    m_flatVelocity = m_maxSpeed - acceleration;
    //}

    //public void MovePlayer(float xAxis, float zAxis)
    //{
    //    Vector3 moveVelocity = Vector3.zero;
    //    float xMovement = (xAxis * m_flatVelocity);
    //    float zMovement = (zAxis * m_flatVelocity);

    //    moveVelocity.x = xMovement;
    //    moveVelocity.z = -zMovement;

    //    m_rb.velocity = moveVelocity;
    //}
}
