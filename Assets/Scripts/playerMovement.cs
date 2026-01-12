using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class playerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Vector2 moveInput;
    private Vector3 playerVelocity;
    private Vector3 lastCheckpointPos;
    private bool groundedPlayer;
    private bool jumpPressed;
    private float coyoteTimeCounter;
    private int coins = 0;

    [Header("Movement Modifiers")]
    [Range(0f, 1f)]
    public float airControlMultiplier = 0.5f;
    public float speed = 5f;
    public float jumpPower = 2f;
    public float coyoteTime = 0.2f;
    public float gravity = -9.81f;
    public float deathY = -5f;
    public float health = 100f;
    public float maxHealth = 100f;
    [Header("UI Elements")]
    public HealthBar healthBar;
    public HUDUI hudUI;
    public CanvasGroup fadeGroup;
    public float fadeDuration = 1f;

    [Header("Misc")]
    public CinemachineCamera freeLookCamera;
    public Animator animator;
    public int currentSaveSlot = 1;
    public int levelsCompleted;
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentSaveSlot = PlayerPrefs.GetInt("SelectedSlot", 1);

        LoadGame(currentSaveSlot);

        lastCheckpointPos = transform.position;
        hudUI.SetCoinUI(coins);
        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }
    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    private void OnJump(InputValue value)
    {
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
        if (groundedPlayer)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
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
        float appliedSpeed = groundedPlayer ? speed : speed * airControlMultiplier;
        controller.Move(move * Time.deltaTime * appliedSpeed);

        // Rotate player to face movement direction
        if (move.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }

        // Jump logic
        if (jumpPressed && coyoteTimeCounter > 0f)
        {
            playerVelocity.y += Mathf.Sqrt(jumpPower * -2f * gravity);
            jumpPressed = false;
            coyoteTimeCounter = 0f;
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

        if (jumpPressed && !groundedPlayer)
        {
            jumpPressed = false;
        }

        if (this.transform.position.y <= deathY)
        {
            StartCoroutine(Respawn());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            coin coin = other.GetComponent<coin>();
            coin.Collect();
            Destroy(other.gameObject);
            coins++;
            hudUI.SetCoinUI(coins);
        } else if (other.CompareTag("Checkpoint"))
        {
            lastCheckpointPos = transform.position;
            SaveGame(currentSaveSlot);
        } else if (other.CompareTag("Finish"))
        {
            PlayerData data = SaveSystem.LoadPlayer(currentSaveSlot);
            data.playerPosition = Vector3.zero;
            data.checkpointSceneIndex = -1;

            SaveGame(currentSaveSlot);
            SceneManager.LoadScene("LevelSelect");
        }
    }

    public void SaveGame(int slot)
    {
        PlayerData data = SaveSystem.LoadPlayer(slot);
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        data.levelsCompleted = Mathf.Max(data.levelsCompleted, sceneIndex);
        data.coins = coins;

        data.playerPosition = lastCheckpointPos;
        data.checkpointSceneIndex = sceneIndex;

        SaveSystem.SavePlayer(data, slot);
    }

    public void LoadGame(int slot)
    {
        PlayerData data = SaveSystem.LoadPlayer(slot);
        coins = data.coins;

        int currentScene = SceneManager.GetActiveScene().buildIndex;

        if (data.checkpointSceneIndex == currentScene)
        {
            transform.position = data.playerPosition;
            lastCheckpointPos = data.playerPosition;
        }
        else
        {
            lastCheckpointPos = transform.position;
        }

        controller.enabled = true;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.SetHealth(health);
        if (health <= 0)
        {
            StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn()
    {
        yield return StartCoroutine(Fade(1));

        controller.enabled = false;
        transform.position = lastCheckpointPos;
        playerVelocity = Vector3.zero;
        controller.enabled = true;
        yield return StartCoroutine(Fade(1));
        yield return StartCoroutine(Fade(0));
    }
    IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeGroup.alpha;
        float time = 0f;

        while (time < fadeDuration)
        {
            fadeGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
        }

        fadeGroup.alpha = targetAlpha;
    }

}
