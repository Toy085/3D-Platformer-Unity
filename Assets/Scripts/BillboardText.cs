using UnityEngine;

public class BillboardText : MonoBehaviour
{
    Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + mainCam.transform.forward);
    }
}
