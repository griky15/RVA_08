using UnityEngine;

public class LightFollow : MonoBehaviour
{
    public Transform sunTransform;

    void Update()
    {
        if (sunTransform != null)
        {
            transform.position = sunTransform.position;
        }
    }
}

