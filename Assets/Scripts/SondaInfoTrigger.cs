using UnityEngine;
using UnityEngine.InputSystem; // 1. PRECISAMOS DESTA LINHA para ler o teclado

public class SondaInfoTrigger : MonoBehaviour
{
    // Guarda uma referência ao último painel que abrimos
    private PlanetInfoCompleto painelAbertoAtual = null;

    /// <summary>
    /// Chamado automaticamente pela Unity quando este Trigger (Sonda)
    /// entra em contacto com outro Collider (Planeta).
    /// (Esta função fica igual)
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        // Tenta obter o script PlanetInfoCompleto do objeto em que tocámos
        PlanetInfoCompleto planetInfo = other.GetComponent<PlanetInfoCompleto>();

        // Verifica se é mesmo um planeta (se tem o script)
        if (planetInfo != null)
        {
            // Se já tínhamos um painel de outro planeta aberto, fecha-o primeiro
            if (painelAbertoAtual != null && painelAbertoAtual != planetInfo)
            {
                painelAbertoAtual.EsconderInformacoes();
            }

            // Agora, mostra as informações do novo planeta
            planetInfo.MostrarInformacoes();
            
            // Guarda a referência deste planeta como o "atualmente aberto"
            painelAbertoAtual = planetInfo;
            
            Debug.Log($"[SondaTrigger] Entrou na área de: {planetInfo.nomePlaneta}");
        }
    }

    /// <summary>
    /// 2. A FUNÇÃO 'OnTriggerExit' FOI COMPLETAMENTE REMOVIDA.
    /// O painel já não fecha ao afastar a sonda.
    /// </summary>
    // void OnTriggerExit(Collider other) { ... } // APAGADO!


    /// <summary>
    /// 3. ADICIONÁMOS A FUNÇÃO 'Update'
    /// Chamado a cada frame.
    /// </summary>
    void Update()
    {
        // Verifica se um teclado está ligado E se a tecla 'X' foi premida neste frame
        if (Keyboard.current != null && Keyboard.current.xKey.wasPressedThisFrame)
        {
            // Verifica se temos um painel guardado na memória (ou seja, se um painel está aberto)
            if (painelAbertoAtual != null)
            {
                Debug.Log($"[SondaTrigger] Fechando {painelAbertoAtual.nomePlaneta} via tecla 'X'.");
                
                // Manda fechar esse painel
                painelAbertoAtual.EsconderInformacoes();
                
                // Limpa a memória. Isto é MUITO importante para poder abrir o próximo.
                painelAbertoAtual = null;
            }
        }
    }
}