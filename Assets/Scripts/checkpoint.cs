using UnityEngine;

public class checkpoint : MonoBehaviour
{
    public bool isFinish = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isFinish)
            {
                other.GetComponent<playerMovement>().SetCheckpoint(transform.position);
            }
            else if (isFinish)
            {
                //other.GetComponent<playerMovement>().SetCheckpoint(transform.position);
            }
        }
    }
}
