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
    private float knockbackTimer;
    private Vector3 knockbackVelocity = Vector3.zero;


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
    [Header("Knockback")]
    public float knockbackForce = 8f;
    public float knockbackDuration = 0.2f;
    public float knockbackUpwardForce = 2f;

    [Header("UI Elements")]
    public HealthBar healthBar;
    public HUDUI hudUI;
    public CanvasGroup fadeGroup;
    public float fadeDuration = 1f;

    [Header("Misc")]
    public CinemachineCamera freeLookCamera;
    public Animator animator;
    public int currentSaveSlot = 1;

    [Header("SFX")]
    public AudioClip levelCompleteSound;
    public AudioClip deathSound;
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
        if (groundedPlayer && playerVelocity.y < 0 && knockbackTimer <= 0)
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
        if (moveInput.magnitude > 1f)
        {
            moveInput.Normalize();
        }
        
        Vector3 move = Vector3.zero;

        if (knockbackTimer > 0)
        {
            controller.Move(knockbackVelocity * Time.deltaTime);

            knockbackVelocity.y = Mathf.Lerp(knockbackVelocity.y, 0, 15f * Time.deltaTime);

            knockbackVelocity.x = Mathf.Lerp(knockbackVelocity.x, 0, 10f * Time.deltaTime);
            knockbackVelocity.z = Mathf.Lerp(knockbackVelocity.z, 0, 10f * Time.deltaTime);

            knockbackTimer -= Time.deltaTime;

            jumpPressed = false;
        }
        else
        {
            move = camForward * moveInput.y + camRight * moveInput.x;
            float appliedSpeed = groundedPlayer ? speed : speed * airControlMultiplier;
            Vector3 playerIntent = move * Time.deltaTime * appliedSpeed;

            Vector3 platformMovement = Vector3.zero;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.5f)) 
            {
                movePlatform platform = hit.collider.GetComponent<movePlatform>();
                if (platform != null)
                {
                    platformMovement = platform.platformDelta;
                }
            }

            controller.Move(playerIntent + platformMovement);

            // Jump logic
            if (jumpPressed && coyoteTimeCounter > 0f)
            {
                playerVelocity.y += Mathf.Sqrt(jumpPower * -2f * gravity);
                jumpPressed = false;
                coyoteTimeCounter = 0f;
                animator.SetBool("IsJumping", true);
            }

            playerVelocity.y += gravity * Time.deltaTime;
            if (controller.enabled)
            {
                controller.Move(playerVelocity * Time.deltaTime);
            }
        }



        // Rotate player to face movement direction
        if (move.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }

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

            Checkpoint checkpoint = other.GetComponent<Checkpoint>();
            checkpoint?.Activate();
        } else if (other.CompareTag("Finish"))
        {
            SpeedrunTimer timer = FindFirstObjectByType<SpeedrunTimer>();
            if (timer != null) timer.StopTimer();

            if (levelCompleteSound != null)
            {
                AudioSource.PlayClipAtPoint(levelCompleteSound, transform.position, PlayerPrefs.GetFloat("SFXVolume", 1f));
            }

            StartCoroutine(LevelCompleteSequence());
        }
    }

    public void SaveGame(int slot)
    {
        PlayerData data = SaveSystem.LoadPlayer(slot);
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        data.levelsCompleted = Mathf.Max(data.levelsCompleted, sceneIndex);
        data.coins = coins;

        data.playerPosition = lastCheckpointPos;
        //data.checkpointSceneIndex = sceneIndex;

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
            if (deathSound != null)
            {
                AudioSource.PlayClipAtPoint(deathSound, transform.position, PlayerPrefs.GetFloat("SFXVolume", 1f));
            }
            StartCoroutine(Respawn());
        }
    }

    public void ApplyKnockback(Vector3 sourcePosition)
    {
        Vector3 direction = (transform.position - sourcePosition).normalized;
        direction.y = 0;

        knockbackVelocity = direction * knockbackForce;
        knockbackVelocity.y = knockbackUpwardForce;
        playerVelocity = Vector3.zero;

        knockbackTimer = knockbackDuration;
    }

    public void AddHealth(int amount)
    {
        health += amount;
        if (health >= maxHealth)
        {
            health = maxHealth;
        }
        healthBar.SetHealth(health);
    }

    private void OnInteract(InputValue value)
    {
        Shop[] shops = FindObjectsByType<Shop>(FindObjectsSortMode.None);
    
        foreach (Shop shop in shops)
        {
            float distance = Vector3.Distance(transform.position, shop.transform.position + shop.Offset);
        
            if (distance <= shop.interactDistance)
            {
                shop.OpenShop();
                break;
            }
        }
    }

    IEnumerator LevelCompleteSequence()
    {
        controller.enabled = false;

        float finalTime = 0f;
        SpeedrunTimer timer = FindFirstObjectByType<SpeedrunTimer>();
        if (timer != null)
        {
            finalTime = timer.GetFinalTime();
            timer.StopTimer();
        }

        yield return new WaitForSeconds(2f);

        PlayerData data = SaveSystem.LoadPlayer(currentSaveSlot);
        data.playerPosition = Vector3.zero;
        data.checkpointSceneIndex = -1;
        data.coins = coins;

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        data.levelsCompleted = Mathf.Max(data.levelsCompleted, currentSceneIndex);

        string currentLevelName = SceneManager.GetActiveScene().name;
        data.UpdateBestTime(currentLevelName, finalTime);

        SaveSystem.SavePlayer(data, currentSaveSlot);
        SceneTransition.Instance.TransitionToScene("LevelSelect");
    }
    IEnumerator Respawn()
    {
        yield return StartCoroutine(Fade(1));

        controller.enabled = false;        
        transform.position = lastCheckpointPos;
        playerVelocity = Vector3.zero;
        health = maxHealth;
        healthBar.SetHealth(health);
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
