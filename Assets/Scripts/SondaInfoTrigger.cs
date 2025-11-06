using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class SondaInfoTrigger : MonoBehaviour
{
    private PlanetInfoCompleto painelAbertoAtual = null;
    
    private PlanetInfoCompleto planetaPendenteParaAvancar = null;

    private PlanetProgressionManager progressionManager;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private bool isResetting = false;

    void Start()
    {
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;
        progressionManager = FindFirstObjectByType<PlanetProgressionManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (isResetting) return;

        PlanetInfoCompleto planetInfo = other.GetComponent<PlanetInfoCompleto>();

        if (planetInfo != null)
        {
            if (painelAbertoAtual == planetInfo) return;

            if (painelAbertoAtual != null)
            {
                painelAbertoAtual.EsconderInformacoes();
            }
            planetInfo.MostrarInformacoes();
            painelAbertoAtual = planetInfo;
            
            planetaPendenteParaAvancar = planetInfo;

            Debug.Log($"[Sonda] Contacto com {planetInfo.nomePlaneta}. À espera que o jogador leia e feche com 'X'.");

            StartCoroutine(ResetSondaDelay(0.5f));
        }
    }

    IEnumerator ResetSondaDelay(float delay)
    {
        isResetting = true;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        yield return new WaitForSeconds(delay);

        transform.localPosition = startPosition;
        transform.localRotation = startRotation;
        
        if (rb != null) rb.isKinematic = false; 
        isResetting = false;
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.xKey.wasPressedThisFrame)
        {
            if (painelAbertoAtual != null)
            {
                Debug.Log($"[Sonda] Fechar painel de {painelAbertoAtual.nomePlaneta} com 'X'.");
                painelAbertoAtual.EsconderInformacoes();
                painelAbertoAtual = null;
            }

            if (planetaPendenteParaAvancar != null && progressionManager != null)
            {
                Debug.Log("[Sonda] 'X' pressionado. Avança para o próximo planeta.");
                progressionManager.PlanetExplored(planetaPendenteParaAvancar.gameObject);
                
                planetaPendenteParaAvancar = null;
            }
        }
    }
}