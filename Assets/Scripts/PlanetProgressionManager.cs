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
        if (currentTargetIndex < planetsInOrder.Count)
        {
            if (exploredPlanet == planetsInOrder[currentTargetIndex])
            {
                currentTargetIndex++;
                
                if (currentTargetIndex >= planetsInOrder.Count)
                {
                    Debug.Log("Progresso: TODOS os planetas explorados! Mostrar o sistema completo.");
                }
                
                UpdatePlanetVisibility();
            }
        }
    }

    private void UpdatePlanetVisibility()
    {
        bool isGameFinished = (currentTargetIndex >= planetsInOrder.Count);

        for (int i = 0; i < planetsInOrder.Count; i++)
        {
            if (planetsInOrder[i] != null)
            {
                bool shouldBeActive;

                if (isGameFinished)
                {
                    shouldBeActive = true;
                }
                else
                {
                    shouldBeActive = (i == currentTargetIndex);
                }
                
                if (planetsInOrder[i].activeSelf != shouldBeActive)
                {
                    planetsInOrder[i].SetActive(shouldBeActive);
                }
            }
        }
    }
}