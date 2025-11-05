using UnityEngine;

public class MoonOrbit : MonoBehaviour
{
    public Transform earth;
    public float orbitSpeed = 50f;
    public float orbitRadius = 0.004f;
    public float tiltAngle = 5.14f;

    private float currentAngle = 0f;

    void Update()
    {
        if (earth == null) return;

        OrbitAroundEarth();
        RotateSelf();
    }

    void OrbitAroundEarth()
    {
        currentAngle += orbitSpeed * Time.deltaTime;

        float x = Mathf.Cos(currentAngle * Mathf.Deg2Rad) * orbitRadius;
        float z = Mathf.Sin(currentAngle * Mathf.Deg2Rad) * orbitRadius;

        Vector3 offset = Quaternion.Euler(tiltAngle, 0f, 0f) * new Vector3(x, 0f, z);

        transform.position = earth.position + offset;
    }

    void RotateSelf()
    {
        transform.LookAt(earth.position);
    }
}