using UnityEngine;

public class Spring : MonoBehaviour
{
    public float launchForce = 15f;
    public AudioClip springSound;

    private void OnTriggerEnter(Collider other)
    {
        playerMovement player = other.GetComponent<playerMovement>();

        if (player != null)
        {
            player.Launch(launchForce);

            if (springSound != null)
            {
                AudioSource.PlayClipAtPoint(springSound, transform.position, PlayerPrefs.GetFloat("SFXVolume", 1f));
            }
        }
    }
}
