using UnityEngine;

public class coin : MonoBehaviour
{
    public ParticleSystem pickupFX;

    public void Collect()
    {
        Instantiate(pickupFX, transform.position, Quaternion.identity);
    }
}
