using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform directionProvider;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float jumpForce;

    private Animator animator;
    private Rigidbody rb;

    private bool isGrounded = false;
    private bool isJumping = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(transform.position + new Vector3(0,.1f,0), groundCheckRadius, groundLayer);

        if(isGrounded)
        {
            animator.SetBool("IsGrounded", true);
        }
        else
        {
            animator.SetBool("IsGrounded", false);
        }

        if(isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
        }


        //Transform directions based on input keys
        float horizontalInput = 0f;
        float verticalInput = 0f;

        if (Input.GetKey(KeyCode.A))
            horizontalInput -= 1f;
        if (Input.GetKey(KeyCode.D))
            horizontalInput += 1f;
        if (Input.GetKey(KeyCode.W))
            verticalInput += 1f;
        if (Input.GetKey(KeyCode.S))
            verticalInput -= 1f;

        // Get the camera's forward direction
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0f; 

        Vector3 moveDirection = (cameraForward * verticalInput + Camera.main.transform.right * horizontalInput).normalized;

        // Calculate the rotation based on input
        if (moveDirection != Vector3.zero)
        {
            animator.SetBool("IsRunning", true);
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }

        // Move the character
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

    }

    private void FixedUpdate()
    {
        if (isJumping)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            isJumping = false;
        }
    }
}
