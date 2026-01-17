using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class levelSelectPlayerMovement : MonoBehaviour
{
    public float speed = 4f;
    public float rotationSpeed = 10f;
    public Shop shop;

    private Vector2 moveInput;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnUse(InputValue value)
    {
        shop.TryOpenShop();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;   
    }
    void Update()
    {
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y);

        if (move.magnitude > 0.1f)
        {
            // Move
            rb.MovePosition(rb.position + move * speed * Time.deltaTime);

            // Rotate to face movement
            /*Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );*/
        }
    }
}
