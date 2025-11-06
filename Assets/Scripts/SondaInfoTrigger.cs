using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class SondaInfoTrigger : MonoBehaviour
{
    private PlanetInfoCompleto painelAbertoAtual = null;
    
    // Variável nova para guardar o planeta que acabámos de explorar,
    // mas que ainda estamos a ler a informação.
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
            // 1. Se tocar no mesmo planeta duas vezes seguidas sem fechar, ignora
            if (painelAbertoAtual == planetInfo) return;

            // 2. Abre o painel imediatamente
            if (painelAbertoAtual != null)
            {
                painelAbertoAtual.EsconderInformacoes();
            }
            planetInfo.MostrarInformacoes();
            painelAbertoAtual = planetInfo;
            
            // 3. Guarda este planeta como "pendente" para avançar o progresso SÓ quando fecharmos o painel
            planetaPendenteParaAvancar = planetInfo;

            Debug.Log($"[Sonda] Contacto com {planetInfo.nomePlaneta}. À espera que o jogador leia e feche com 'X'.");

            // 4. Inicia apenas o reset da posição da sonda
            StartCoroutine(ResetSondaDelay(0.5f));
        }
    }

    IEnumerator ResetSondaDelay(float delay)
    {
        isResetting = true;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true; // Congela a física

        yield return new WaitForSeconds(delay);

        // Retorna à base
        transform.localPosition = startPosition;
        transform.localRotation = startRotation;
        
        if (rb != null) rb.isKinematic = false; // Descongela
        isResetting = false;
    }

    void Update()
    {
        // Verifica se a tecla 'X' foi premida
        if (Keyboard.current != null && Keyboard.current.xKey.wasPressedThisFrame)
        {
            // 1. Fecha o painel se estiver algum aberto
            if (painelAbertoAtual != null)
            {
                Debug.Log($"[Sonda] Fechando painel de {painelAbertoAtual.nomePlaneta} com 'X'.");
                painelAbertoAtual.EsconderInformacoes();
                painelAbertoAtual = null;
            }

            // 2. AGORA SIM, se tínhamos um planeta pendente, avançamos para o próximo
            if (planetaPendenteParaAvancar != null && progressionManager != null)
            {
                Debug.Log("[Sonda] 'X' pressionado. Avançando para o próximo planeta.");
                progressionManager.PlanetExplored(planetaPendenteParaAvancar.gameObject);
                
                // Limpa o pendente para não avançar duas vezes
                planetaPendenteParaAvancar = null;
            }
        }
    }
}