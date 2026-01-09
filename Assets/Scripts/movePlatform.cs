using UnityEngine;

public class movePlatform : MonoBehaviour
{
    public float maxY;
    public float minY;
    public float speed;
    private bool up = true;

    // Update is called once per frame
    void Update()
    {
        float direction = up ? 1f : -1f;
        transform.position += Vector3.up * speed * Time.deltaTime * direction;

        if (transform.position.y >= maxY)
            up = false;
        else if (transform.position.y <= minY)
            up = true;
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

}
