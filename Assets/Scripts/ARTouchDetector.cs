using UnityEngine;
using UnityEngine.InputSystem;

public class ARTouchDetector : MonoBehaviour
{
    private Camera arCamera;
    
    void Start()
    {
        arCamera = Camera.main;
        if (arCamera == null)
        {
            arCamera = FindFirstObjectByType<Camera>();
        }
    }
    
    void Update()
    {
        // Detecta toque/clique usando o novo Input System
        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
        {
            Vector2 posicaoToque = Pointer.current.position.ReadValue();
            DetectarPlaneta(posicaoToque);
        }
    }
    
    void DetectarPlaneta(Vector2 posicaoToque)
    {
        Ray ray = arCamera.ScreenPointToRay(posicaoToque);
        RaycastHit hit;
        
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 2f);
        Debug.Log($"Raycast disparado na posição: {posicaoToque}");
        
        if (Physics.Raycast(ray, out hit, 100f))
        {
            Debug.Log($"✅ Tocou em: {hit.collider.gameObject.name}");
            
            PlanetInfoCompleto planetInfo = hit.collider.GetComponent<PlanetInfoCompleto>();
            
            if (planetInfo != null)
            {
                Debug.Log($"🪐 Abrindo info de {planetInfo.nomePlaneta}");
                planetInfo.MostrarInformacoes();
            }
        }
        else
        {
            Debug.Log("❌ Não tocou em nada");
        }
    }
}