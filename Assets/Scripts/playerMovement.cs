using System;
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

    private int coins = 0;

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
            // Small downward force to keep the player "snapped" to the floor
            playerVelocity.y = -2f;
        }

        float currentSpeed = speed;
        if (!groundedPlayer)
        {
            currentSpeed /= 2;
        }
        Vector3 move = transform.forward * moveInput.y + transform.right * moveInput.x;
        controller.Move(move * Time.deltaTime * currentSpeed);

        // Jump logic
        if (jumpPressed && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpPower * -2f * gravity);
            jumpPressed = false;
        }

        // Apply gravity then move player
        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            coins += 1;
        }
    }

}
