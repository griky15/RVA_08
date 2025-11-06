using UnityEngine;
using System.Collections.Generic;

public class PlanetProgressionManager : MonoBehaviour
{
    [Header("Lista Ordenada dos Planetas")]
    public List<GameObject> planetsInOrder;

    private int currentTargetIndex = 0;

    void Start()
    {
        UpdatePlanetVisibility();
    }

    public void PlanetExplored(GameObject exploredPlanet)
    {
        // Só faz sentido avançar se ainda não acabámos
        if (currentTargetIndex < planetsInOrder.Count)
        {
            // Se o planeta explorado for o atual objetivo
            if (exploredPlanet == planetsInOrder[currentTargetIndex])
            {
                currentTargetIndex++;
                
                if (currentTargetIndex >= planetsInOrder.Count)
                {
                    Debug.Log("Progresso: TODOS os planetas explorados! Mostrando o sistema completo.");
                }
                
                UpdatePlanetVisibility();
            }
        }
    }

    private void UpdatePlanetVisibility()
    {
        // Verifica se o jogo já acabou (índice passou do último planeta)
        bool isGameFinished = (currentTargetIndex >= planetsInOrder.Count);

        for (int i = 0; i < planetsInOrder.Count; i++)
        {
            if (planetsInOrder[i] != null)
            {
                bool shouldBeActive;

                if (isGameFinished)
                {
                    // SE ACABOU: Mostra TODOS
                    shouldBeActive = true;
                }
                else
                {
                    // SE AINDA JOGA: Mostra só o atual
                    shouldBeActive = (i == currentTargetIndex);
                }
                
                // Aplica o estado se for diferente do atual
                if (planetsInOrder[i].activeSelf != shouldBeActive)
                {
                    planetsInOrder[i].SetActive(shouldBeActive);
                }
            }
        }
    }
}