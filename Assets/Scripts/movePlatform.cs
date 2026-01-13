using UnityEngine;

public class movePlatform : MonoBehaviour
{
    [Header("Movement Settings")]
    public Vector3 startPos;
    public Vector3 endPos;
    public float speed = 3f;
    public float waitTime = 2f;

    private Vector3 targetPos;
    private float waitTimer = 0f;
    private bool movingToEnd = true;

    void Start()
    {
        transform.position = startPos;
        targetPos = endPos;
    }

    void Update()
    {
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
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(startPos, endPos);
        Gizmos.DrawWireMesh(transform.GetComponent<MeshFilter>().sharedMesh, startPos, Quaternion.identity, transform.localScale);
        Gizmos.DrawWireMesh(transform.GetComponent<MeshFilter>().sharedMesh, endPos, Quaternion.identity, transform.localScale);
    }
}