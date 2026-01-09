using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Vector2 moveInput;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private bool jumpPressed;

    public float speed = 5f;
    public float jumpPower = 2f;
    public float gravity = -9.81f;
    public Animator animator;

    private int coins = 0;
    public HUDUI hudUI;

    public CinemachineCamera freeLookCamera;
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    private void OnJump(InputValue value)
    {
        // For buttons, value.isPressed is true when pushed down
        jumpPressed = value.isPressed;
    }

    // Update is called once per frame
    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        // Get camera relative directions
        Vector3 camForward = freeLookCamera.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = freeLookCamera.transform.right;
        camRight.y = 0;
        camRight.Normalize();

        // Movement relative to camera
        Vector3 move = camForward * moveInput.y + camRight * moveInput.x;
        controller.Move(move * Time.deltaTime * speed);

        // Rotate player to face movement direction
        if (move.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }

        // Jump logic
        if (jumpPressed && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpPower * -2f * gravity);
            jumpPressed = false;
            animator.SetBool("IsJumping", true);
        }

        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        float speedPercent = move.magnitude / 1f;
        animator.SetFloat("Speed", speedPercent);

        if (groundedPlayer)
        {
            animator.SetBool("IsJumping", false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            coins++;
            hudUI.setCoinUI(coins);
        }
    }

}
