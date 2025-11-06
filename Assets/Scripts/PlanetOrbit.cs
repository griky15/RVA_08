using UnityEngine;
// using Vuforia; // Já não é necessário para este script

public class PlanetOrbit : MonoBehaviour
{
    [Header("Configurações Específicas deste Planeta")]
    // Cada planeta terá valores diferentes, que pode ajustar no Inspector
    public float tiltAngle = 0f;

    // --- Referências e lógicas de movimento removidas ---

    void Start()
    {
        // 1. Aplica a inclinação axial inicial uma única vez.
        // É mais estável fazer isto no Start do que no Update.
        transform.localRotation = Quaternion.Euler(tiltAngle, 0f, 0f);
    }

    // O método Update() foi removido, pois não há mais movimento.
    // Os métodos OrbitAroundSun() e RotateSelf() também foram removidos.
}