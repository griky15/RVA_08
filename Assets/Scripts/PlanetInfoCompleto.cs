using UnityEngine;
using TMPro;

public class PlanetInfoCompleto : MonoBehaviour
{
    [Header("Informações do Planeta")]
    public string nomePlaneta;
    [TextArea(3, 10)]
    public string descricao;
    public float distanciaRealDoSol;
    public float temperaturaMedia;
    public int numeroLuas;
    public string curiosidade;
    
    [Header("Referências UI")]
    public GameObject painelInfo;
    public TextMeshProUGUI textoNome;
    public TextMeshProUGUI textoDescricao;
    public TextMeshProUGUI textoDetalhes;
    
    void Start()
    {
        if (GetComponent<Collider>() == null)
        {
            gameObject.AddComponent<SphereCollider>();
        }
        
        if (painelInfo != null)
        {
            painelInfo.SetActive(false);
        }
        
        Debug.Log($"{nomePlaneta} configurado!");
    }
    
    public void MostrarInformacoes()
    {
        if (painelInfo == null)
        {
            Debug.LogError("Painel não foi arrastado no Inspector!");
            return;
        }
        
        painelInfo.SetActive(true);
        
        if (textoNome != null)
            textoNome.text = nomePlaneta;
        else
            Debug.LogError("TextoNome não foi arrastado!");
        
        if (textoDescricao != null)
            textoDescricao.text = descricao;
        else
            Debug.LogError("TextoDescricao não foi arrastado!");
        
        if (textoDetalhes != null)
        {
            textoDetalhes.text = $"Distância do Sol: {distanciaRealDoSol:F1} milhões de km\n" +
                                 $"Temperatura: {temperaturaMedia}°C\n" +
                                 $"Luas: {numeroLuas}\n\n" +
                                 $"{curiosidade}";
        }
        else
            Debug.LogError("TextoDetalhes não foi arrastado!");
        
        Debug.Log($"✅ Painel aberto para {nomePlaneta}!");
    }

    public void EsconderInformacoes()
    {
        if (painelInfo != null)
        {
            painelInfo.SetActive(false);
            Debug.Log($"✅ Painel fechado para {nomePlaneta}!");
        }
    }
}