using UnityEngine;

public class Heart : MonoBehaviour
{
    public int HealthAmout = 20;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerMovement player = other.GetComponent<playerMovement>();
            if (player != null)
            {
                player.AddHealth(HealthAmout);
                Destroy(gameObject);
            }
        }
    }
}
