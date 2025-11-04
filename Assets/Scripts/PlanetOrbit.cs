using UnityEngine;
using Vuforia;

public class PlanetOrbit : MonoBehaviour
{
    [Header("Referências Globais (Arrastar)")]
    // O GameObject do modelo 3D do Sol (para saber em torno de quem orbitar)
    public GameObject sun;                        
    
    // O ImageTarget do Sol (para saber se ele está visível)
    public ObserverBehaviour sunMarker;           

    [Header("Configurações Específicas deste Planeta")]
    // Cada planeta terá valores diferentes, que pode ajustar no Inspector
    public float orbitSpeed = 20f;                
    public float rotationSpeed = 50f;             
    public float tiltAngle = 0f;                 

    // Referência interna ao marcador DESTE planeta
    private ObserverBehaviour thisPlanetMarker;

    void Start()
    {
        // 1. Encontra automaticamente o marcador "pai" deste planeta.
        // Isto assume que a sua hierarquia é:
        // - ImageTarget_Planeta (com ObserverBehaviour)
        //   - Modelo3D_Planeta (com este script)
        thisPlanetMarker = GetComponentInParent<ObserverBehaviour>();

        if (thisPlanetMarker == null)
        {
            Debug.LogError("PlanetOrbit.cs: Não foi possível encontrar o ObserverBehaviour no objeto pai!", this.gameObject);
        }

        // 2. Aplica a inclinação axial inicial uma única vez.
        // É mais estável fazer isto no Start do que no Update.
        transform.localRotation = Quaternion.Euler(tiltAngle, 0f, 0f);
    }

    void Update()
    {
        // 3. Verifica se ambos os marcadores (Sol E este Planeta) estão sendo vistos
        if (thisPlanetMarker != null && sunMarker != null &&
            thisPlanetMarker.TargetStatus.Status == Status.TRACKED &&
            sunMarker.TargetStatus.Status == Status.TRACKED)
        {
            // 4. Executa as ações
            OrbitAroundSun();
            RotateSelf();
        }
    }

    void OrbitAroundSun()
    {
        // Gira este planeta em torno da posição do GameObject 'sun'
        // Usamos Vector3.up (0, 1, 0) para uma órbita "plana"
        // (O seu script original usava (0.2, 1, 0). Pode mudar aqui se preferir)
        transform.RotateAround(sun.transform.position, Vector3.up, orbitSpeed * Time.deltaTime);
    }

    void RotateSelf()
    {
        // Gira o planeta sobre o seu próprio eixo Y (vertical)
        // Space.Self garante que ele gira em torno do seu próprio eixo, respeitando a inclinação
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.Self);
    }
}