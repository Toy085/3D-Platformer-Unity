using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    public AudioClip[] footstepClips;
    private AudioSource audioSource;
    private CharacterController controller;
    
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1.0f;
        
        controller = GetComponentInParent<CharacterController>();
    }

    public void PlayFootstep()
    {
        if (controller == null) return;

        if (controller.isGrounded)
        {
            AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];
            
            audioSource.pitch = Random.Range(0.8f, 1.1f);
            audioSource.PlayOneShot(clip);
        }
    }
}
