using UnityEngine;
using UnityEngine.InputSystem;

public class levelSelectPlayerMovement : MonoBehaviour
{
    public float speed = 4f;
    public float rotationSpeed = 10f;

    private Vector2 moveInput;

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void Update()
    {
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y);

        if (move.magnitude > 0.1f)
        {
            // Move
            transform.position += move * speed * Time.deltaTime;

            // Rotate to face movement
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }
}
