using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    public float width = 20f;
    public Transform cameraTransform;

    void Update()
    {
        if (cameraTransform.position.x - transform.position.x >= width)
        {
            transform.position += new Vector3(width * 3, 0, 0);
        }
    }
}
