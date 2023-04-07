using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddDeleteCardUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> CardsUI;
    [SerializeField] private InputDataExtract _dataExtract;
    
    private int maxCards = 4;
    private int currNumCards = 1;
    
    public void ActivateNewCard()
    {
        if (currNumCards == maxCards) return;
        
        CardsUI[currNumCards-1].gameObject.SetActive(true);
        currNumCards += 1;
        
        _dataExtract.planet.GetComponent<MatrixTransformation>().AddMatrix();
    }

    public void DeactivateCards()
    {
        if (currNumCards == 1) return;
        
        CardsUI[currNumCards-2].gameObject.SetActive(false);
        currNumCards -= 1;
        
        _dataExtract.planet.GetComponent<MatrixTransformation>().DeleteMatrix();
    }
}
