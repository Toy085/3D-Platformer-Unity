using UnityEngine;
using System.Collections;

public class Spikes : MonoBehaviour
{
    public Transform spikes;
    public float maxSpikeHeight = 0f;
    public float minSpikeHeight = -0.15f;
    public float spikeSpeed = 2f;
    public float damageAmount = 20f;

    private bool spikesUp = false;

    void Start()
    {
        StartCoroutine(SpikeRoutine());
    }

    void Update()
    {
        float targetHeight = spikesUp ? maxSpikeHeight : minSpikeHeight;

        Vector3 currentPosition = spikes.localPosition;
        currentPosition.y = Mathf.MoveTowards(currentPosition.y, targetHeight, spikeSpeed * Time.deltaTime);
        spikes.localPosition = currentPosition;
    }

    IEnumerator SpikeRoutine()
    {
        while (true)
        {
            spikesUp = true;
            yield return new WaitForSeconds(5f);

            spikesUp = false;
            yield return new WaitForSeconds(5f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (spikesUp)
            {
                playerMovement player = other.GetComponent<playerMovement>();
                if (player != null)
                {
                    player.TakeDamage(damageAmount);
                    player.ApplyKnockback(transform.position);
                }
            }
        }
    }
}
