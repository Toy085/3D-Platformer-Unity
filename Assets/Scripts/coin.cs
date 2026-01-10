using UnityEngine;

public class coin : MonoBehaviour
{
    public ParticleSystem pickupFX;
    public AudioClip pickupSound;

    public float magnetRadius = 3f;
    public float magnetSpeed = 10f;

    private Transform player;
    private bool isMagnetized = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (isMagnetized)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                player.position,
                magnetSpeed * Time.deltaTime
            );
        }
        else
        {
            float dist = Vector3.Distance(transform.position, player.position);
            if (dist <= magnetRadius)
            {
                isMagnetized = true;
            }
        }
    }

    public void Collect()
    {
        Instantiate(pickupFX, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(
            pickupSound,
            transform.position,
            Random.Range(0.8f, 1f)
        );
    }
}
