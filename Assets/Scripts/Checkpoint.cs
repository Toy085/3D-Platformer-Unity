using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public ParticleSystem activateEffect;

    private bool activated = false;

    public void Activate()
    {
        if (activated) return;
        activated = true;

        if (activateEffect != null)
        {
            activateEffect.Play();
        }
    }
}
