using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public ParticleSystem activateEffect;
    public AudioClip checkpointSound;

    private bool activated = false;

    public void Activate()
    {
        if (activated) return;
        activated = true;

        activateEffect?.Play();
        AudioSource.PlayClipAtPoint(
            checkpointSound,
            transform.position,
            Random.Range(0.8f, 1f)
        );
    }
}
