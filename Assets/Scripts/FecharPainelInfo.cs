using UnityEngine;

public class FecharPainelInfo : MonoBehaviour
{
    public GameObject painel;
    
    void Start()
    {
        if (painel == null)
        {
            painel = GameObject.Find("PainelInfo");
        }
    }
    
    public void Fechar()
    {
        if (painel != null)
        {
            painel.SetActive(false);
        }
    }
}