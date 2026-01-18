using UnityEngine;

public class movePlatform : MonoBehaviour
{
    [Header("Movement Settings")]
    public Vector3 startPos;
    public Vector3 endPos;
    public float speed = 3f;
    public float waitTime = 2f;
    [Header("Damage Settings")]
    public bool doDamage = false;
    public bool isSaw = false;
    public float damageAmount = 20f;

    private Vector3 targetPos;
    private float waitTimer = 0f;
    private bool movingToEnd = true;

    public Vector3 platformDelta { get; private set; }
    private Vector3 lastPosition;

    void Start()
    {
        transform.position = startPos;
        lastPosition = transform.position;
        targetPos = endPos;
    }

    void Update()
    {
        platformDelta = transform.position - lastPosition;
        lastPosition = transform.position;

        if (waitTimer > 0)
        {
            waitTimer -= Time.deltaTime;
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.001f)
        {
            waitTimer = waitTime;

            movingToEnd = !movingToEnd;
            targetPos = movingToEnd ? endPos : startPos;
        }
        if (isSaw)
        {
            transform.Rotate(Vector3.forward, 360 * Time.deltaTime);
        }   
    }

    void OnTriggerEnter(Collider other)
    {
        playerMovement player = other.GetComponent<playerMovement>();

        if (other.CompareTag("Player") && doDamage && player != null)
        {
            player.TakeDamage(damageAmount);
            player.ApplyKnockback(transform.position);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        if (doDamage)
            Gizmos.color = Color.red;
        Gizmos.DrawLine(startPos, endPos);
        Gizmos.DrawWireMesh(transform.GetComponent<MeshFilter>().sharedMesh, startPos, Quaternion.identity, transform.localScale);
        Gizmos.DrawWireMesh(transform.GetComponent<MeshFilter>().sharedMesh, endPos, Quaternion.identity, transform.localScale);
    }
}