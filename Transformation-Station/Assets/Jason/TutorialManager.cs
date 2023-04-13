using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject[] tutorialCardsUI;
    
    private bool _tutorialMode = false;
    private int _tutorialProgress = 0;

    private void Update()
    {
        SetTimeScale();
    }

    private void SetTimeScale()
    {
        Time.timeScale = _tutorialMode ? 0 : 1;
    }

    private void SetActiveCardUI(bool x)
    {
        if (_tutorialProgress < tutorialCardsUI.Length && tutorialCardsUI[_tutorialProgress])
        {
            tutorialCardsUI[_tutorialProgress].SetActive(x);
        }
        else
        {
            Debug.Log($"GameObject DNE or index out of range");
        }
    }

    public void PauseGame()
    {
        Debug.Log("Game has been paused!");
        
        _tutorialMode = true;

        SetActiveCardUI(_tutorialMode);
    }
    
    public void UnPauseGame()
    {
        Debug.Log("Game has been unpaused!");
        
        _tutorialMode = false;
        
        SetActiveCardUI(_tutorialMode);

        _tutorialProgress++;
    }
}
