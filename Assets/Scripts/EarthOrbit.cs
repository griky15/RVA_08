using UnityEngine;
using Vuforia;

public class EarthOrbit : MonoBehaviour
{
    public GameObject sun;                        // Referência ao Sol
    public ObserverBehaviour earthMarker;         // ImageTarget da Terra
    public ObserverBehaviour sunMarker;           // ImageTarget do Sol

    public float orbitSpeed = 20f;                // Velocidade da órbita
    public float rotationSpeed = 50f;             // Rotação da Terra sobre si mesma
    public float tiltAngle = 23.5f;               // Inclinação axial da Terra

    private bool tilted = false;

    void Update()
    {
        // Só orbita se ambos os marcadores estiverem TRACKED
        if (earthMarker.TargetStatus.Status == Status.TRACKED &&
            sunMarker.TargetStatus.Status == Status.TRACKED)
        {
            OrbitAroundSun();
            RotateSelf();
        }
    }

    void OrbitAroundSun()
    {
        // Cria uma inclinação no eixo da órbita (ligeiramente fora do eixo Y)
        Vector3 orbitAxis = new Vector3(0.2f, 1f, 0f);

        // Move a Terra em torno do Sol
        transform.RotateAround(sun.transform.position, orbitAxis.normalized, orbitSpeed * Time.deltaTime);

        // Garante que a Terra “olha” sempre para o Sol (opcional — podes remover)
        // transform.LookAt(sun.transform);
    }

    void RotateSelf()
    {
        // Aplica inclinação uma única vez (23,5°)
        if (!tilted)
        {
            transform.rotation = Quaternion.Euler(tiltAngle, 0f, 0f);
            tilted = true;
        }

        // Faz a Terra girar sobre o seu próprio eixo
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.Self);
    }
}
