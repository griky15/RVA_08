using UnityEngine;

public class PlanetOrbit : MonoBehaviour
{
    [Header("Configurações Específicas deste Planeta")]
    public float tiltAngle = 0f;


    void Start()
    {
        transform.localRotation = Quaternion.Euler(tiltAngle, 0f, 0f);
    }
}